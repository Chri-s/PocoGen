using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PocoGen.Common
{
    public static class Utilities
    {
        public static bool CheckFileNameNotAbsolute(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return !(string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || Path.IsPathRooted(path));
        }

        public static string GetExceptionMessages(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception", "exception is null.");
            }

            StringBuilder messages = new StringBuilder();

            Exception currentException = exception;
            while (currentException != null)
            {
                if (messages.Length > 0)
                {
                    messages.Append("\r\n\r\n--> ");
                }

                messages.Append(currentException.Message);

                currentException = currentException.InnerException;
            }

            return messages.ToString();
        }

        public static SchemaReaderPlugIn GetPlugIn(this Lazy<ISchemaReader, ISchemaReaderMetadata> reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader", "reader is null.");
            }

            return new SchemaReaderPlugIn(reader);
        }

        public static TableNameGeneratorPlugIn GetPlugIn(this Lazy<ITableNameGenerator, ITableNameGeneratorMetadata> generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator", "generator is null.");
            }

            return new TableNameGeneratorPlugIn(generator);
        }

        public static ColumnNameGeneratorPlugIn GetPlugIn(this Lazy<IColumnNameGenerator, IColumnNameGeneratorMetadata> generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator", "generator is null.");
            }

            return new ColumnNameGeneratorPlugIn(generator);
        }

        public static OutputWriterPlugIn GetPlugIn(this Lazy<IOutputWriter, IOutputWriterMetadata> writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer", "writer is null.");
            }

            return new OutputWriterPlugIn(writer);
        }

        internal static FileFormat.Table ToTable(this TableChange tableChange)
        {
            if (tableChange == null)
            {
                throw new ArgumentNullException("tableChange", "tableChange is null.");
            }

            return new FileFormat.Table(tableChange.Name, tableChange.ClassName, tableChange.Ignore, tableChange.Columns.Select(c => c.ToColumn()));
        }

        internal static FileFormat.Column ToColumn(this ColumnChange columnChange)
        {
            if (columnChange == null)
            {
                throw new ArgumentNullException("columnChange", "columnChange is null.");
            }

            return new FileFormat.Column(columnChange.Name, columnChange.PropertyName, columnChange.Ignore);
        }

        internal static TableChange ToTableChange(this FileFormat.Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table is null.");
            }

            return new TableChange(table.Name, table.PropertyName, table.Ignore, table.Columns.Select(c => c.ToColumnChange()));
        }

        internal static ColumnChange ToColumnChange(this FileFormat.Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column", "column is null.");
            }

            return new ColumnChange(column.Name, column.PropertyName, column.Ignore);
        }
    }
}