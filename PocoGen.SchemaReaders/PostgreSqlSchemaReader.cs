using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Npgsql;
using PocoGen.Common;

namespace PocoGen.SchemaReaders
{
    [SchemaReader("PostgreSQL", "{F39C2939-E311-45C4-B6C8-FA9B95C0BA82}", ConnectionStringDocumentationUrl = "http://www.connectionstrings.com/npgsql/")]
    [Export(typeof(ISchemaReader))]
    public class PostgreSqlSchemaReader : ISchemaReader
    {
        private const string GetTablesSql = @"SELECT table_schema, table_name, table_type
            FROM information_schema.tables 
            WHERE (table_type='BASE TABLE' OR table_type='VIEW')
                AND table_schema NOT IN ('pg_catalog', 'information_schema');";

        private const string GetColumnsSql = @"SELECT column_name, udt_name, is_nullable, column_default
            FROM information_schema.columns 
            WHERE table_name=@tableName;";

        private const string GetPrimaryKeySql = @"SELECT kcu.column_name 
            FROM information_schema.key_column_usage kcu
            JOIN information_schema.table_constraints tc ON kcu.constraint_name=tc.constraint_name
            WHERE lower(tc.constraint_type)='primary key' AND kcu.table_name=@tablename;";

        private static string[] keywords;

        public void TestConnectionString(string connectionString, ISettings settings)
        {
            using (DbConnection connection = PostgreSqlSchemaReader.GetConnection(connectionString))
            {
                connection.Open();
            }
        }

        public TableCollection ReadSchema(string connectionString, ISettings settings)
        {
            using (DbConnection connection = PostgreSqlSchemaReader.GetConnection(connectionString))
            {
                connection.Open();

                TableCollection tables = new TableCollection();

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = GetTablesSql;

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Table table = new Table(reader.GetString(0), reader.GetString(1), string.Compare(reader.GetString(2), "View", StringComparison.OrdinalIgnoreCase) == 0);

                            tables.Add(table);
                        }
                    }

                    foreach (Table table in tables)
                    {
                        table.Columns.AddRange(PostgreSqlSchemaReader.LoadColumns(table, connection));
                    }
                }

                return tables;
            }
        }

        public string EscapeSchemaName(string schemaName)
        {
            return PostgreSqlSchemaReader.Escape(schemaName);
        }

        public string EscapeTableName(string tableName)
        {
            return PostgreSqlSchemaReader.Escape(tableName);
        }

        public string EscapeColumnName(string columnName)
        {
            return PostgreSqlSchemaReader.Escape(columnName);
        }

        private static string[] Keywords
        {
            get
            {
                if (PostgreSqlSchemaReader.keywords == null)
                {
                    PostgreSqlSchemaReader.keywords = new string[] { "ALL", "ANALYSE", "ANALYZE", "AND", "ANY", "ARRAY", "AS", "ASC", "ASYMMETRIC", "AUTHORIZATION", "BINARY", "BOTH", "CASE", "CAST", "CHECK", "COLLATE", "COLLATION", "COLUMN", "CONCURRENTLY", "CONSTRAINT", "CREATE", "CROSS", "CURRENT_CATALOG", "CURRENT_DATE", "CURRENT_ROLE", "CURRENT_SCHEMA", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "DEFAULT", "DEFERRABLE", "DESC", "DISTINCT", "DO", "ELSE", "END", "EXCEPT", "FALSE", "FETCH", "FOR", "FOREIGN", "FREEZE", "FROM", "FULL", "GRANT", "GROUP", "HAVING", "ILIKE", "IN", "INITIALLY", "INNER", "INTERSECT", "INTO", "IS", "ISNULL", "JOIN", "LATERAL", "LEADING", "LEFT", "LIKE", "LIMIT", "LOCALTIME", "LOCALTIMESTAMP", "NATURAL", "NOT", "NOTNULL", "NULL", "OFFSET", "ON", "ONLY", "OR", "ORDER", "OUTER", "OVER", "OVERLAPS", "PLACING", "PRIMARY", "REFERENCES", "RETURNING", "RIGHT", "SELECT", "SESSION_USER", "SIMILAR", "SOME", "SYMMETRIC", "TABLE", "THEN", "TO", "TRAILING", "TRUE", "UNION", "UNIQUE", "USER", "USING", "VARIADIC", "VERBOSE", "WHEN", "WHERE", "WINDOW", "WITH" };
                }

                return PostgreSqlSchemaReader.keywords;
            }
        }

        private static DbConnection GetConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        private static string Escape(string identifier)
        {
            return PostgreSqlSchemaReader.NeedsEscaping(identifier) ? "\"" + identifier + "\"" : identifier;
        }

        private static bool NeedsEscaping(string identifier)
        {
            /*
             * SQL identifiers and key words must begin with a letter (a-z, but also letters with diacritical marks and non-Latin letters) or an underscore (_).
             * Subsequent characters in an identifier or key word can be letters, underscores, digits (0-9), or dollar signs ($).
             * 
             * (source: http://www.postgresql.org/docs/7.1/static/sql-syntax.html#SQL-SYNTAX-IDENTIFIERS)
             * 
             * The first Regex checks for the character ranges, the second that the identifier doesn't consist solely of digits.
             * Also unescaped identifiers must be lower case in postgresql.
             */

            return !Regex.IsMatch(identifier, @"^[a-z\p{L}_][a-z\p{L}_0-9$]*$", RegexOptions.CultureInvariant) ||
                Regex.IsMatch(identifier, "^[0-9]+$", RegexOptions.CultureInvariant) ||
                PostgreSqlSchemaReader.Keywords.Any(k => k.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
                identifier.ToLowerInvariant() != identifier);
        }

        private static List<Column> LoadColumns(Table table, DbConnection connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetColumnsSql;

                DbParameter p = cmd.CreateParameter();
                p.ParameterName = "@tableName";
                p.Value = table.Name;
                cmd.Parameters.Add(p);

                List<Column> result = new List<Column>();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string sqlType = reader.GetString(1);
                        bool isNullable = reader.GetString(2) == "YES";
                        Column column = new Column(name, PostgreSqlSchemaReader.GetPropertyType(sqlType, isNullable), isNullable, PostgreSqlSchemaReader.IsAutoIncrement(reader, 3));

                        result.Add(column);
                    }
                }

                foreach (string column in PostgreSqlSchemaReader.GetPK(table.Name, connection))
                {
                    result.Single(c => string.Compare(c.Name, column, StringComparison.Ordinal) == 0).IsPK = true;
                }

                return result;
            }
        }

        private static List<string> GetPK(string table, DbConnection connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetPrimaryKeySql;

                DbParameter p = cmd.CreateParameter();
                p.ParameterName = "@tablename";
                p.Value = table;
                cmd.Parameters.Add(p);

                List<string> columns = new List<string>();

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(0));
                    }
                }

                return columns;
            }
        }

        private static ColumnBaseType GetPropertyType(string sqlType, bool isNullable)
        {
            Type sysType;
            switch (sqlType)
            {
                case "int8":
                case "serial8":
                    sysType = typeof(long);
                    break;

                case "bool":
                    sysType = typeof(bool);
                    break;

                case "bytea	":
                    sysType = typeof(byte[]);
                    break;

                case "float8":
                    sysType = typeof(double);
                    break;

                case "int4":
                case "serial4":
                    sysType = typeof(int);
                    break;

                case "money	":
                    sysType = typeof(decimal);
                    break;

                case "numeric":
                    sysType = typeof(decimal);
                    break;

                case "float4":
                    sysType = typeof(float);
                    break;

                case "int2":
                    sysType = typeof(short);
                    break;

                case "time":
                case "timetz":
                case "timestamp":
                case "timestamptz":
                case "date":
                    sysType = typeof(DateTime);
                    break;

                default:
                    sysType = typeof(string);
                    break;
            }

            if (isNullable && sysType.IsValueType)
            {
                return new ColumnType(typeof(Nullable<>).MakeGenericType(sysType));
            }
            else
            {
                return new ColumnType(sysType);
            }
        }

        private static bool IsAutoIncrement(DbDataReader reader, int fieldIndex)
        {
            object value = reader.GetValue(fieldIndex);

            if (value == DBNull.Value)
            {
                return false;
            }

            string stringValue = (string)value;

            return stringValue.StartsWith("nextval(", StringComparison.OrdinalIgnoreCase);
        }
    }
}