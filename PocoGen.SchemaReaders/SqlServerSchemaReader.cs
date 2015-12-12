using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using PocoGen.Common;

namespace PocoGen.SchemaReaders
{
    [SchemaReader("Microsoft SQL Server", "{D7C9BACC-992C-4702-9686-297BFC4A5F50}", ConnectionStringDocumentationUrl = "http://www.connectionstrings.com/sqlconnection/")]
    [Export(typeof(ISchemaReader))]
    public class SqlServerSchemaReader : ISchemaReader
    {
        private const string GetTablesSql = @"SELECT TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE
        FROM  INFORMATION_SCHEMA.TABLES
        WHERE TABLE_TYPE='BASE TABLE' OR TABLE_TYPE='VIEW'";

        private const string GetColumnsSql = @"SELECT 
            COLUMN_NAME AS ColumnName,
            DATA_TYPE AS DataType, 
            IS_NULLABLE AS IsNullable,
            COLUMNPROPERTY(object_id('[' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']'), COLUMN_NAME, 'IsIdentity') AS IsIdentity
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME=@tableName AND TABLE_SCHEMA=@schemaName
        ORDER BY ORDINAL_POSITION ASC";

        private const string GetPrimaryKeySql = @"SELECT c.name AS ColumnName
                FROM sys.indexes AS i 
                INNER JOIN sys.index_columns AS ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id 
                INNER JOIN sys.objects AS o ON i.object_id = o.object_id
                INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
                LEFT OUTER JOIN sys.columns AS c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
                WHERE (i.type = 1) AND (s.name = @tableSchema) AND (o.name = @tableName)";

        private const string GetForeignKeysSql = @"SELECT  C.CONSTRAINT_SCHEMA, C.CONSTRAINT_NAME, PK.TABLE_SCHEMA AS PK_TABLE_SCHEMA, PK.TABLE_NAME AS PK_TABLE_NAME, PK.COLUMN_NAME AS PK_COLUMN_NAME, FK.TABLE_SCHEMA AS FK_TABLE_SCHEMA, FK.TABLE_NAME AS FK_TABLE_NAME, FK.COLUMN_NAME AS FK_COLUMN_NAME
FROM    INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE FK ON FK.CONSTRAINT_CATALOG = c.CONSTRAINT_CATALOG AND FK.CONSTRAINT_SCHEMA = C.CONSTRAINT_SCHEMA AND FK.CONSTRAINT_NAME = C.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE PK ON PK.CONSTRAINT_CATALOG = C.UNIQUE_CONSTRAINT_CATALOG AND PK.CONSTRAINT_SCHEMA = C.UNIQUE_CONSTRAINT_SCHEMA AND PK.CONSTRAINT_NAME = C.UNIQUE_CONSTRAINT_NAME AND PK.ORDINAL_POSITION = FK.ORDINAL_POSITION
ORDER BY C.CONSTRAINT_SCHEMA, C.CONSTRAINT_NAME, FK.ORDINAL_POSITION";

        private static string[] keywords;

        public void TestConnectionString(string connectionString, ISettings settings)
        {
            using (DbConnection connection = SqlServerSchemaReader.GetConnection(connectionString))
            {
                connection.Open();
            }
        }

        public TableCollection ReadTables(string connectionString, ISettings settings)
        {
            using (DbConnection connection = SqlServerSchemaReader.GetConnection(connectionString))
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
                }

                foreach (Table table in tables)
                {
                    table.Columns.AddRange(SqlServerSchemaReader.LoadColumns(table, connection));
                }

                return tables;
            }
        }

        public ForeignKeyCollection ReadForeignKeys(string connectionString, ISettings settings)
        {
            using (DbConnection connection = SqlServerSchemaReader.GetConnection(connectionString))
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
                                foreignKey = new ForeignKey(constraintSchema, constraintName, fkTableSchema, fkTableName, pkTableSchema, pkTableName);
                                foreignKeys.Add(foreignKey);
                            }

                            foreignKey.Columns.Add(new ForeignKeyColumn(pkColumnName, fkColumnName));
                            lastSchema = constraintSchema;
                            lastForeignKeyName = constraintName;
                        }
                    }
                }

                return foreignKeys;
            }
        }

        public string EscapeSchemaName(string schemaName)
        {
            return SqlServerSchemaReader.Escape(schemaName);
        }

        public string EscapeTableName(string tableName)
        {
            return SqlServerSchemaReader.Escape(tableName);
        }

        public string EscapeColumnName(string columnName)
        {
            return SqlServerSchemaReader.Escape(columnName);
        }

        private static string[] Keywords
        {
            get
            {
                if (SqlServerSchemaReader.keywords == null)
                {
                    SqlServerSchemaReader.keywords = new string[] { "ADD", "EXTERNAL", "PROCEDURE", "ALL", "FETCH", "PUBLIC", "ALTER", "FILE", "RAISERROR", "AND", "FILLFACTOR", "READ", "ANY", "FOR", "READTEXT", "AS", "FOREIGN", "RECONFIGURE", "ASC", "FREETEXT", "REFERENCES", "AUTHORIZATION", "FREETEXTTABLE", "REPLICATION", "BACKUP", "FROM", "RESTORE", "BEGIN", "FULL", "RESTRICT", "BETWEEN", "FUNCTION", "RETURN", "BREAK", "GOTO", "REVERT", "BROWSE", "GRANT", "REVOKE", "BULK", "GROUP", "RIGHT", "BY", "HAVING", "ROLLBACK", "CASCADE", "HOLDLOCK", "ROWCOUNT", "CASE", "IDENTITY", "ROWGUIDCOL", "CHECK", "IDENTITY_INSERT", "RULE", "CHECKPOINT", "IDENTITYCOL", "SAVE", "CLOSE", "IF", "SCHEMA", "CLUSTERED", "IN", "SECURITYAUDIT", "COALESCE", "INDEX", "SELECT", "COLLATE", "INNER", "SEMANTICKEYPHRASETABLE", "COLUMN", "INSERT", "SEMANTICSIMILARITYDETAILSTABLE", "COMMIT", "INTERSECT", "SEMANTICSIMILARITYTABLE", "COMPUTE", "INTO", "SESSION_USER", "CONSTRAINT", "IS", "SET", "CONTAINS", "JOIN", "SETUSER", "CONTAINSTABLE", "KEY", "SHUTDOWN", "CONTINUE", "KILL", "SOME", "CONVERT", "LEFT", "STATISTICS", "CREATE", "LIKE", "SYSTEM_USER", "CROSS", "LINENO", "TABLE", "CURRENT", "LOAD", "TABLESAMPLE", "CURRENT_DATE", "MERGE", "TEXTSIZE", "CURRENT_TIME", "NATIONAL", "THEN", "CURRENT_TIMESTAMP", "NOCHECK", "TO", "CURRENT_USER", "NONCLUSTERED", "TOP", "CURSOR", "NOT", "TRAN", "DATABASE", "NULL", "TRANSACTION", "DBCC", "NULLIF", "TRIGGER", "DEALLOCATE", "OF", "TRUNCATE", "DECLARE", "OFF", "TRY_CONVERT", "DEFAULT", "OFFSETS", "TSEQUAL", "DELETE", "ON", "UNION", "DENY", "OPEN", "UNIQUE", "DESC", "OPENDATASOURCE", "UNPIVOT", "DISK", "OPENQUERY", "UPDATE", "DISTINCT", "OPENROWSET", "UPDATETEXT", "DISTRIBUTED", "OPENXML", "USE", "DOUBLE", "OPTION", "USER", "DROP", "OR", "VALUES", "DUMP", "ORDER", "VARYING", "ELSE", "OUTER", "VIEW", "END", "OVER", "WAITFOR", "ERRLVL", "PERCENT", "WHEN", "ESCAPE", "PIVOT", "WHERE", "EXCEPT", "PLAN", "WHILE", "EXEC", "PRECISION", "WITH", "EXECUTE", "PRIMARY", "WITHIN GROUP", "EXISTS", "PRINT", "WRITETEXT", "EXIT", "PROC" };
                }

                return SqlServerSchemaReader.keywords;
            }
        }

        private static string Escape(string identifier)
        {
            return SqlServerSchemaReader.NeedsEscaping(identifier) ? "[" + identifier + "]" : identifier;
        }

        private static bool NeedsEscaping(string identifier)
        {
            /*
             * The first character must be one of the following:
             *    A letter as defined by the Unicode Standard 3.2. The Unicode definition of letters includes Latin characters from a through z, from A through Z,
             *    and also letter characters from other languages.
             *    The underscore (_), at sign (@), or number sign (#).
             *    Certain symbols at the beginning of an identifier have special meaning in SQL Server. A regular identifier that starts with the at sign always
             *    denotes a local variable or parameter and cannot be used as the name of any other type of object. An identifier that starts with a number sign
             *    denotes a temporary table or procedure. An identifier that starts with double number signs (##) denotes a global temporary object. Although the
             *    number sign or double number sign characters can be used to begin the names of other types of objects, we do not recommend this practice.
             *   
             * Subsequent characters can include the following:
             *    Letters as defined in the Unicode Standard 3.2.
             *    Decimal numbers from either Basic Latin or other national scripts.
             *    The at sign, dollar sign ($), number sign, or underscore.
             * 
             * (source: http://msdn.microsoft.com/en-us/library/ms175874.aspx)
             * 
             * The first Regex checks for the character ranges, the second that the identifier doesn't consist solely of digits.
             */

            return !Regex.IsMatch(identifier, @"^[\p{L}_][\p{L}0-9@$#_]*$", RegexOptions.CultureInvariant) ||
                Regex.IsMatch(identifier, "^[0-9]+$", RegexOptions.CultureInvariant) ||
                SqlServerSchemaReader.Keywords.Any(k => k.Equals(identifier, StringComparison.OrdinalIgnoreCase));
        }

        private static DbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
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
                p.ParameterName = "@schemaName";
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
                        bool isAutoIncrement = reader.GetInt32(3) == 1;

                        Column column = new Column(table, name, SqlServerSchemaReader.GetPropertyType(sqlType, isNullable), isNullable, isAutoIncrement);
                        result.Add(column);
                    }
                }

                foreach (string column in SqlServerSchemaReader.GetPK(table.Schema, table.Name, connection))
                {
                    result.Single(c => string.Compare(c.Name, column, StringComparison.Ordinal) == 0).IsPK = true;
                }

                return result;
            }
        }

        private static List<string> GetPK(string schema, string table, DbConnection connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetPrimaryKeySql;

                DbParameter p = cmd.CreateParameter();
                p.ParameterName = "@tableName";
                p.Value = table;
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

        private static PocoGen.Common.ColumnBaseType GetPropertyType(string sqlType, bool isNullable)
        {
            Type sysType = typeof(string);
            switch (sqlType)
            {
                case "bigint":
                    sysType = typeof(long);
                    break;
                case "smallint":
                    sysType = typeof(short);
                    break;
                case "int":
                    sysType = typeof(int);
                    break;
                case "uniqueidentifier":
                    sysType = typeof(Guid);
                    break;
                case "smalldatetime":
                case "datetime":
                case "date":
                case "time":
                    sysType = typeof(DateTime);
                    break;
                case "float":
                    sysType = typeof(double);
                    break;
                case "real":
                    sysType = typeof(float);
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    sysType = typeof(decimal);
                    break;
                case "tinyint":
                    sysType = typeof(byte);
                    break;
                case "bit":
                    sysType = typeof(bool);
                    break;
                case "image":
                case "binary":
                case "varbinary":
                case "timestamp":
                    sysType = typeof(byte[]);
                    break;
                case "geography":
                    return new ColumnForeignType("Microsoft.SqlServer.Types.SqlGeography", false);
                case "geometry":
                    return new ColumnForeignType("Microsoft.SqlServer.Types.SqlGeometry", false);
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