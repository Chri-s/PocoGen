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
            WHERE table_schema=@tableSchema AND table_name=@tableName;";

        private const string GetPrimaryKeySql = @"SELECT kcu.column_name 
            FROM information_schema.key_column_usage kcu
            JOIN information_schema.table_constraints tc ON kcu.constraint_name=tc.constraint_name
            WHERE lower(tc.constraint_type)='primary key' AND kcu.table_schema=@tableSchema AND kcu.table_name=@tablename;";

        private const string GetForeignKeysSql = @"SELECT  c.constraint_schema, c.constraint_name, pk.table_schema AS pk_table_schema, pk.table_name AS pk_table_name, pk.column_name AS pk_column_name, fk.table_schema AS fk_table_schema, fk.table_name AS fk_table_name, fk.column_name AS fk_column_name
FROM    information_schema.referential_constraints c
INNER JOIN information_schema.key_column_usage fk ON fk.constraint_catalog = c.constraint_catalog AND fk.constraint_schema = c.constraint_schema AND fk.constraint_name = c.constraint_name
INNER JOIN information_schema.key_column_usage PK ON pk.constraint_catalog = c.unique_constraint_catalog AND pk.constraint_schema = c.unique_constraint_schema AND pk.constraint_name = c.unique_constraint_name AND pk.ordinal_position = fk.ordinal_position
ORDER BY c.constraint_schema, c.constraint_name, fk.ordinal_position;";

        private static string[] keywords;

        public void TestConnectionString(string connectionString, ISettings settings)
        {
            using (DbConnection connection = PostgreSqlSchemaReader.GetConnection(connectionString))
            {
                connection.Open();
            }
        }

        public TableCollection ReadTables(string connectionString, ISettings settings)
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

                PostgreSqlSchemaReader.LoadForeignKeys(tables, connection);

                return tables;
            }
        }

        public ForeignKeyCollection ReadForeignKeys(string connectionString, ISettings settings)
        {
            using (DbConnection connection = PostgreSqlSchemaReader.GetConnection(connectionString))
            {
                connection.Open();

                ForeignKeyCollection foreignKeys = new ForeignKeyCollection();

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = GetForeignKeysSql;

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        ForeignKey foreignKey = null;

                        string lastSchema = null;
                        string lastForeignKeyName = null;

                        while (reader.Read())
                        {
                            string constraintSchema = reader.GetString(0);
                            string constraintName = reader.GetString(1);
                            string pkTableSchema = reader.GetString(2);
                            string pkTableName = reader.GetString(3);
                            string pkColumnName = reader.GetString(4);
                            string fkTableSchema = reader.GetString(5);
                            string fkTableName = reader.GetString(6);
                            string fkColumnName = reader.GetString(7);

                            if (lastSchema != constraintSchema || lastForeignKeyName != constraintName)
                            {
                                // Add the foreign key to the collection after all its columns are added because they are part of the generated key in the collection
                                if (foreignKey != null)
                                {
                                    foreignKeys.Add(foreignKey);
                                }

                                foreignKey = new ForeignKey(constraintSchema, constraintName, fkTableSchema, fkTableName, pkTableSchema, pkTableName);
                            }

                            foreignKey.Columns.Add(new ForeignKeyColumn(pkColumnName, fkColumnName));
                            lastSchema = constraintSchema;
                            lastForeignKeyName = constraintName;
                        }

                        if (foreignKey != null)
                        {
                            foreignKeys.Add(foreignKey);
                        }
                    }
                }

                return foreignKeys;
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

                p = cmd.CreateParameter();
                p.ParameterName = "@tableSchema";
                p.Value = table.Schema;
                cmd.Parameters.Add(p);

                List<Column> result = new List<Column>();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string sqlType = reader.GetString(1);
                        bool isNullable = reader.GetString(2) == "YES";
                        Column column = new Column(table, name, PostgreSqlSchemaReader.GetPropertyType(sqlType, isNullable), isNullable, PostgreSqlSchemaReader.IsAutoIncrement(reader, 3));

                        result.Add(column);
                    }
                }

                foreach (string column in PostgreSqlSchemaReader.GetPK(table.Schema, table.Name, connection))
                {
                    result.Single(c => string.Compare(c.Name, column, StringComparison.Ordinal) == 0).IsPK = true;
                }

                return result;
            }
        }

        private static List<string> GetPK(string schema, string tableName, DbConnection connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetPrimaryKeySql;

                DbParameter p = cmd.CreateParameter();
                p.ParameterName = "@tablename";
                p.Value = tableName;
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@tableSchema";
                p.Value = schema;
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

        private static void LoadForeignKeys(TableCollection tables, DbConnection connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetForeignKeysSql;

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    ForeignKey foreignKey = null;

                    string lastSchema = null;
                    string lastForeignKeyName = null;

                    while (reader.Read())
                    {
                        string constraintSchema = reader.GetString(0);
                        string constraintName = reader.GetString(1);
                        string pkTableSchema = reader.GetString(2);
                        string pkTableName = reader.GetString(3);
                        string pkColumnName = reader.GetString(4);
                        string fkTableSchema = reader.GetString(5);
                        string fkTableName = reader.GetString(6);
                        string fkColumnName = reader.GetString(7);

                        if (lastSchema != constraintSchema || lastForeignKeyName != constraintName)
                        {
                            foreignKey = new ForeignKey(constraintSchema, constraintName, fkTableSchema, fkTableName, pkTableSchema, pkTableName);
                            tables[pkTableSchema, pkTableName].ParentForeignKeys.Add(foreignKey);
                            tables[fkTableSchema, fkTableName].ChildForeignKeys.Add(foreignKey);
                        }

                        foreignKey.Columns.Add(new ForeignKeyColumn(pkColumnName, fkColumnName));
                        lastSchema = constraintSchema;
                        lastForeignKeyName = constraintName;
                    }
                }
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