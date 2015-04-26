using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using PocoGen.Common;

namespace PocoGen.SchemaReaders
{
    [Export(typeof(ISchemaReader))]
    [SchemaReader("SQLite", "{71377408-5171-43E1-8578-BA025EE4A71A}", ConnectionStringDocumentationUrl = "http://www.connectionstrings.com/sqlite-net-provider/")]
    public class SqliteSchemaReader : ISchemaReader
    {
        private const string GetTableSql = @"SELECT name, type, sql
FROM SQLITE_MASTER
WHERE type IN ('table', 'view') AND NOT name LIKE 'sqlite_%'
ORDER BY name;";

        private const string GetColumnSql = "PRAGMA table_info({0});";

        private const string GetForeignKeysSql = "PRAGMA foreign_key_list({0});";

        private static string[] keywords;

        public void TestConnectionString(string connectionString, ISettings settings)
        {
            using (DbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
            }
        }

        public TableCollection ReadSchema(string connectionString, ISettings settings)
        {
            using (DbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                TableCollection tables = new TableCollection();
                Dictionary<Table, bool> hasRowIds = new Dictionary<Table, bool>();

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = GetTableSql;

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Table table = new Table(string.Empty, reader.GetString(0), string.Compare(reader.GetString(1), "view", StringComparison.OrdinalIgnoreCase) == 0);
                            bool hasRowId = !Regex.IsMatch(reader.GetString(2), @"WITHOUT\W+ROWID", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

                            hasRowIds.Add(table, hasRowId);

                            tables.Add(table);
                        }
                    }
                }

                foreach (Table table in tables)
                {
                    table.Columns.AddRange(this.GetColumns(table, connection, hasRowIds[table]));
                }

                this.LoadForeignKeys(tables, connection);

                return tables;
            }
        }

        public string EscapeSchemaName(string schemaName)
        {
            return SqliteSchemaReader.Escape(schemaName);
        }

        public string EscapeTableName(string tableName)
        {
            return SqliteSchemaReader.Escape(tableName);
        }

        public string EscapeColumnName(string columnName)
        {
            return SqliteSchemaReader.Escape(columnName);
        }

        private static string[] Keywords
        {
            get
            {
                if (SqliteSchemaReader.keywords == null)
                {
                    // source: http://www.sqlite.org/lang_keywords.html
                    SqliteSchemaReader.keywords = new string[] { "ABORT", "ACTION", "ADD", "AFTER", "ALL", "ALTER", "ANALYZE", "AND", "AS", "ASC", "ATTACH", "AUTOINCREMENT", "BEFORE", "BEGIN", "BETWEEN", "BY", "CASCADE", "CASE", "CAST", "CHECK", "COLLATE", "COLUMN", "COMMIT", "CONFLICT", "CONSTRAINT", "CREATE", "CROSS", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "DATABASE", "DEFAULT", "DEFERRABLE", "DEFERRED", "DELETE", "DESC", "DETACH", "DISTINCT", "DROP", "EACH", "ELSE", "END", "ESCAPE", "EXCEPT", "EXCLUSIVE", "EXISTS", "EXPLAIN", "FAIL", "FOR", "FOREIGN", "FROM", "FULL", "GLOB", "GROUP", "HAVING", "IF", "IGNORE", "IMMEDIATE", "IN", "INDEX", "INDEXED", "INITIALLY", "INNER", "INSERT", "INSTEAD", "INTERSECT", "INTO", "IS", "ISNULL", "JOIN", "KEY", "LEFT", "LIKE", "LIMIT", "MATCH", "NATURAL", "NO", "NOT", "NOTNULL", "NULL", "OF", "OFFSET", "ON", "OR", "ORDER", "OUTER", "PLAN", "PRAGMA", "PRIMARY", "QUERY", "RAISE", "RECURSIVE", "REFERENCES", "REGEXP", "REINDEX", "RELEASE", "RENAME", "REPLACE", "RESTRICT", "RIGHT", "ROLLBACK", "ROW", "SAVEPOINT", "SELECT", "SET", "TABLE", "TEMP", "TEMPORARY", "THEN", "TO", "TRANSACTION", "TRIGGER", "UNION", "UNIQUE", "UPDATE", "USING", "VACUUM", "VALUES", "VIEW", "VIRTUAL", "WHEN", "WHERE", "WITH", "WITHOUT" };
                }

                return SqliteSchemaReader.keywords;
            }
        }

        private static string Escape(string identifier)
        {
            return SqliteSchemaReader.NeedsEscaping(identifier) ? "\"" + identifier + "\"" : identifier;
        }

        private static bool NeedsEscaping(string identifier)
        {
            /*
             * An identifier name must begin with a letter or the underscore character, which may be followed by a number of alphanumeric characters or underscores.
             * 
             * (source: http://flylib.com/books/en/4.112.1.25/1/)
             * 
             * The first Regex checks for the character ranges, the second that the identifier doesn't consist solely of digits.
             */

            return !Regex.IsMatch(identifier, @"^[\p{L}_][\p{L}_0-9]*$", RegexOptions.CultureInvariant) ||
                Regex.IsMatch(identifier, "^[0-9]+$", RegexOptions.CultureInvariant) ||
                SqliteSchemaReader.Keywords.Any(k => k.Equals(identifier, StringComparison.OrdinalIgnoreCase));
        }

        private void LoadForeignKeys(TableCollection tables, DbConnection connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                foreach (Table table in tables)
                {
                    cmd.CommandText = string.Format(CultureInfo.InvariantCulture, GetForeignKeysSql, this.EscapeTableName(table.Name));

                    // SQLite returns the rows for all foreign keys for this table. One row for one column.
                    // This means that a foreign key using more than one column (because the referenced tables has a primary key consisting
                    // of more than one column) returns more than one row for one foreign key.
                    // Because we can't specify an order by-clause and the documentation doesn't specify whether
                    // the rows for a foreign key are returned successively, we cache them and group them afterwards by referenced table.
                    // Tuple = (referenced table, column, referenced column)
                    List<Tuple<string, string, string>> foreignKeys = new List<Tuple<string, string, string>>();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            foreignKeys.Add(new Tuple<string, string, string>(reader.GetString(2), reader.GetString(3), reader.GetString(4)));
                        }
                    }

                    var groupedByTables = from t in foreignKeys
                                          group t by t.Item1 into g
                                          select g;

                    foreach (var group in groupedByTables)
                    {
                        ForeignKey foreignKey = new ForeignKey(null, table.Name, group.Key);

                        foreach (var column in group)
                        {
                            foreignKey.Columns.Add(new ForeignKeyColumn(column.Item3, column.Item2));
                        }

                        tables[foreignKey.ParentTableName].ParentForeignKeys.Add(foreignKey);
                        tables[foreignKey.ChildTableName].ChildForeignKeys.Add(foreignKey);
                    }
                }
            }
        }

        private static ColumnBaseType GetPropertyType(string columnType, bool isNullable)
        {
            Type sysType = null;

            if (columnType.IndexOf("INT", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                sysType = typeof(long);
            }
            else if (columnType.IndexOf("CHAR", StringComparison.OrdinalIgnoreCase) >= 0 ||
                columnType.IndexOf("CLOB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                columnType.IndexOf("TEXT", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                sysType = typeof(string);
            }
            else if (columnType.IndexOf("BLOB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                string.IsNullOrWhiteSpace(columnType))
            {
                sysType = typeof(byte[]);
            }
            else if (columnType.IndexOf("REAL", StringComparison.OrdinalIgnoreCase) >= 0 ||
                columnType.IndexOf("FLOA", StringComparison.OrdinalIgnoreCase) >= 0 ||
                columnType.IndexOf("DOUB", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                sysType = typeof(double);
            }
            else
            {
                sysType = typeof(double);
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

        private IEnumerable<Column> GetColumns(Table table, DbConnection connection, bool hasRowIds)
        {
            ColumnCollection columns = new ColumnCollection();

            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format(CultureInfo.InvariantCulture, GetColumnSql, this.EscapeTableName(table.Name));

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Column column = new Column(reader.GetString(1), SqliteSchemaReader.GetPropertyType(reader.GetString(2), !reader.GetBoolean(3)), !reader.GetBoolean(3), false);
                        column.IsPK = reader.GetBoolean(5);

                        // If the column is part of a primary key and "NOT NULL" is not specified while creating the table,
                        // SQLite will report that the column is nullable, even if it will not allow to insert NULL.
                        if (column.IsPK)
                        {
                            column.IsNullable = false;
                        }

                        // If the primary key is of type INTEGER (exactly, no synonyms like INT) and it is the only column in the primary key, then it is auto incremented.
                        // Also, the table must have a ROWID
                        column.IsAutoIncrement = column.IsPK && string.Compare(reader.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase) == 0 && hasRowIds;

                        columns.Add(column);
                    }
                }
            }

            // If the primary key consists of more than one column, remove any enabled IsAutoIncrement above
            if (columns.Count(c => c.IsPK) > 1)
            {
                foreach (Column column in columns)
                {
                    column.IsAutoIncrement = false;
                }
            }

            return columns;
        }
    }
}