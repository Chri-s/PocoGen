using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using PocoGen.Common;

namespace PocoGen.SchemaReaders
{
    [Export(typeof(ISchemaReader))]
    [SchemaReader("MySQL", "{1CD7ADD1-869A-47B5-8BC4-E69A56ABEA93}", ConnectionStringDocumentationUrl = "http://www.connectionstrings.com/mysql-connector-net-mysqlconnection/")]
    public class MySqlSchemaReader : ISchemaReader
    {
        private const string GetTableSql = @"SELECT TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE
            FROM information_schema.tables
            WHERE (table_type='BASE TABLE' OR table_type='VIEW') AND table_schema = @schema;";

        private const string GetColumnSql = @"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_TYPE, COLUMN_KEY, extra
FROM information_schema.columns
WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table;";

        private const string GetForeignKeysSql = @"SELECT C.CONSTRAINT_NAME, PK.TABLE_NAME AS PK_TABLE_NAME, PK.COLUMN_NAME AS PK_COLUMN_NAME, FK.TABLE_NAME AS FK_TABLE_NAME, FK.COLUMN_NAME AS FK_COLUMN_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE FK ON C.CONSTRAINT_CATALOG = FK.CONSTRAINT_CATALOG AND C.CONSTRAINT_SCHEMA = FK.CONSTRAINT_SCHEMA AND C.TABLE_NAME = FK.TABLE_NAME AND C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE PK ON C.UNIQUE_CONSTRAINT_CATALOG = PK.CONSTRAINT_CATALOG AND C.UNIQUE_CONSTRAINT_SCHEMA = PK.CONSTRAINT_SCHEMA AND C.REFERENCED_TABLE_NAME = PK.TABLE_NAME AND C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME AND FK.ORDINAL_POSITION = PK.ORDINAL_POSITION
WHERE C.CONSTRAINT_SCHEMA = @schema
ORDER BY C.CONSTRAINT_NAME, FK.ORDINAL_POSITION";

        private static string[] keywords;

        public void TestConnectionString(string connectionString, ISettings settings)
        {
            using (DbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
            }
        }

        public TableCollection ReadSchema(string connectionString, ISettings settings)
        {
            using (DbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                TableCollection tables = new TableCollection();

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = GetTableSql;
                    DbParameter schemaParameter = cmd.CreateParameter();
                    schemaParameter.ParameterName = "@schema";
                    schemaParameter.Value = connection.Database;
                    cmd.Parameters.Add(schemaParameter);

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Table table = new Table(reader.GetString(0), reader.GetString(1), string.Compare(reader.GetString(2), "View", StringComparison.OrdinalIgnoreCase) == 0);

                            tables.Add(table);
                        }
                    }
                }

                foreach (Table table in tables)
                {
                    table.Columns.AddRange(MySqlSchemaReader.GetColumns(connection, table));
                }

                MySqlSchemaReader.LoadForeignKeys(tables, connection);

                return tables;
            }
        }

        public string EscapeSchemaName(string schemaName)
        {
            return MySqlSchemaReader.Escape(schemaName);
        }

        public string EscapeTableName(string tableName)
        {
            return MySqlSchemaReader.Escape(tableName);
        }

        public string EscapeColumnName(string columnName)
        {
            return MySqlSchemaReader.Escape(columnName);
        }

        private static string[] Keywords
        {
            get
            {
                if (MySqlSchemaReader.keywords == null)
                {
                    // source: http://dev.mysql.com/doc/refman/5.7/en/reserved-words.html
                    MySqlSchemaReader.keywords = new string[] { "ACCESSIBLE", "ADD", "ALL", "ALTER", "ANALYZE", "AND", "AS", "ASC", "ASENSITIVE", "BEFORE", "BETWEEN", "BIGINT", "BINARY", "BLOB", "BOTH", "BY", "CALL", "CASCADE", "CASE", "CHANGE", "CHAR", "CHARACTER", "CHECK", "COLLATE", "COLUMN", "CONDITION", "CONSTRAINT", "CONTINUE", "CONVERT", "CREATE", "CROSS", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DATABASES", "DAY_HOUR", "DAY_MICROSECOND", "DAY_MINUTE", "DAY_SECOND", "DEC", "DECIMAL", "DECLARE", "DEFAULT", "DELAYED", "DELETE", "DESC", "DESCRIBE", "DETERMINISTIC", "DISTINCT", "DISTINCTROW", "DIV", "DOUBLE", "DROP", "DUAL", "EACH", "ELSE", "ELSEIF", "ENCLOSED", "ESCAPED", "EXISTS", "EXIT", "EXPLAIN", "FALSE", "FETCH", "FLOAT", "FLOAT4", "FLOAT8", "FOR", "FORCE", "FOREIGN", "FROM", "FULLTEXT", "GET", "GRANT", "GROUP", "HAVING", "HIGH_PRIORITY", "HOUR_MICROSECOND", "HOUR_MINUTE", "HOUR_SECOND", "IF", "IGNORE", "IN", "INDEX", "INFILE", "INNER", "INOUT", "INSENSITIVE", "INSERT", "INT", "INT1", "INT2", "INT3", "INT4", "INT8", "INTEGER", "INTERVAL", "INTO", "IO_AFTER_GTIDS", "IO_BEFORE_GTIDS", "IS", "ITERATE", "JOIN", "KEY", "KEYS", "KILL", "LEADING", "LEAVE", "LEFT", "LIKE", "LIMIT", "LINEAR", "LINES", "LOAD", "LOCALTIME", "LOCALTIMESTAMP", "LOCK", "LONG", "LONGBLOB", "LONGTEXT", "LOOP", "LOW_PRIORITY", "MASTER_BIND", "MASTER_SSL_VERIFY_SERVER_CERT", "MATCH", "MAXVALUE", "MEDIUMBLOB", "MEDIUMINT", "MEDIUMTEXT", "MIDDLEINT", "MINUTE_MICROSECOND", "MINUTE_SECOND", "MOD", "MODIFIES", "NATURAL", "NOT", "NO_WRITE_TO_BINLOG", "NULL", "NUMERIC", "ON", "OPTIMIZE", "OPTIMIZER_COSTS", "OPTION", "OPTIONALLY", "OR", "ORDER", "OUT", "OUTER", "OUTFILE", "PARTITION", "PRECISION", "PRIMARY", "PROCEDURE", "PURGE", "RANGE", "READ", "READS", "READ_WRITE", "REAL", "REFERENCES", "REGEXP", "RELEASE", "RENAME", "REPEAT", "REPLACE", "REQUIRE", "RESIGNAL", "RESTRICT", "RETURN", "REVOKE", "RIGHT", "RLIKE", "SCHEMA", "SCHEMAS", "SECOND_MICROSECOND", "SELECT", "SENSITIVE", "SEPARATOR", "SET", "SHOW", "SIGNAL", "SMALLINT", "SPATIAL", "SPECIFIC", "SQL", "SQLEXCEPTION", "SQLSTATE", "SQLWARNING", "SQL_BIG_RESULT", "SQL_CALC_FOUND_ROWS", "SQL_SMALL_RESULT", "SSL", "STARTING", "STRAIGHT_JOIN", "TABLE", "TERMINATED", "THEN", "TINYBLOB", "TINYINT", "TINYTEXT", "TO", "TRAILING", "TRIGGER", "TRUE", "UNDO", "UNION", "UNIQUE", "UNLOCK", "UNSIGNED", "UPDATE", "USAGE", "USE", "USING", "UTC_DATE", "UTC_TIME", "UTC_TIMESTAMP", "VALUES", "VARBINARY", "VARCHAR", "VARCHARACTER", "VARYING", "WHEN", "WHERE", "WHILE", "WITH", "WRITE", "XOR", "YEAR_MONTH", "ZEROFILL" };
                }

                return MySqlSchemaReader.keywords;
            }
        }

        private static string Escape(string identifier)
        {
            return MySqlSchemaReader.NeedsEscaping(identifier) ? "`" + identifier + "`" : identifier;
        }

        private static bool NeedsEscaping(string identifier)
        {
            /*
             * Permitted characters in unquoted identifiers:
             *  ASCII: [0-9,a-z,A-Z$_] (basic Latin letters, digits 0-9, dollar, underscore)
             *  Extended: U+0080 .. U+FFFF
             * 
             * (source: http://dev.mysql.com/doc/refman/5.7/en/identifiers.html)
             * 
             * The first Regex checks for the character ranges, the second that the identifier doesn't consist solely of digits.
             */

            return !Regex.IsMatch(identifier, @"^[0-9a-zA-Z$_\u0080-\uFFFF]+$", RegexOptions.CultureInvariant) ||
                Regex.IsMatch(identifier, "^[0-9]+$", RegexOptions.CultureInvariant) ||
                MySqlSchemaReader.Keywords.Any(k => k.Equals(identifier, StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<Column> GetColumns(DbConnection connection, Table table)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetColumnSql;

                DbParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = "@schema";
                parameter.Value = table.Schema;
                cmd.Parameters.Add(parameter);

                parameter = cmd.CreateParameter();
                parameter.ParameterName = "@table";
                parameter.Value = table.Name;
                cmd.Parameters.Add(parameter);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool isNullable = string.Compare(reader.GetString(2), "YES", StringComparison.OrdinalIgnoreCase) == 0;

                        ColumnBaseType propertyType = MySqlSchemaReader.GetPropertyType(
                            reader.GetString(1),
                            isNullable,
                            reader.GetString(3).IndexOf("unsigned", StringComparison.OrdinalIgnoreCase) >= 0);

                        Column column = new Column(
                            reader.GetString(0),
                            propertyType,
                            isNullable,
                            reader.GetString(4).IndexOf("auto_increment", StringComparison.OrdinalIgnoreCase) >= 0);

                        column.IsPK = string.Compare(reader.GetString(4), "PRI", StringComparison.OrdinalIgnoreCase) == 0;

                        yield return column;
                    }
                }
            }
        }

        private static void LoadForeignKeys(TableCollection tables, DbConnection connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetForeignKeysSql;
                DbParameter schemasParameter = cmd.CreateParameter();
                schemasParameter.ParameterName = "@schema";
                schemasParameter.Value = connection.Database;
                cmd.Parameters.Add(schemasParameter);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    ForeignKey foreignKey = null;

                    string lastForeignKeyName = null;

                    while (reader.Read())
                    {
                        string constraintName = reader.GetString(0);
                        string pkTableName = reader.GetString(1);
                        string pkColumnName = reader.GetString(2);
                        string fkTableName = reader.GetString(3);
                        string fkColumnName = reader.GetString(4);

                        if (lastForeignKeyName != constraintName)
                        {
                            foreignKey = new ForeignKey(constraintName, fkTableName, pkTableName);
                            tables[pkTableName].ParentForeignKeys.Add(foreignKey);
                            tables[fkTableName].ChildForeignKeys.Add(foreignKey);
                        }

                        foreignKey.Columns.Add(new ForeignKeyColumn(pkColumnName, fkColumnName));
                        lastForeignKeyName = constraintName;
                    }
                }
            }
        }

        private static ColumnBaseType GetPropertyType(string sqlType, bool isNullable, bool isUnsigned)
        {
            Type sysType;
            switch (sqlType)
            {
                case "bigint":
                    sysType = isUnsigned ? typeof(ulong) : typeof(long);
                    break;
                case "int":
                    sysType = isUnsigned ? typeof(uint) : typeof(int);
                    break;
                case "smallint":
                    sysType = isUnsigned ? typeof(ushort) : typeof(short);
                    break;
                case "guid":
                    sysType = typeof(Guid);
                    break;
                case "smalldatetime":
                case "date":
                case "datetime":
                case "timestamp":
                    sysType = typeof(DateTime);
                    break;
                case "float":
                    sysType = typeof(float);
                    break;
                case "double":
                    sysType = typeof(double);
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    sysType = typeof(decimal);
                    break;
                case "bit":
                case "bool":
                case "boolean":
                    sysType = typeof(bool);
                    break;
                case "tinyint":
                    sysType = isUnsigned ? typeof(byte) : typeof(sbyte);
                    break;
                case "image":
                case "binary":
                case "blob":
                case "mediumblob":
                case "longblob":
                case "varbinary":
                    sysType = typeof(byte[]);
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
    }
}