using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.NPoco
{
    internal static class NPocoVisualBasicWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, IDBEscaper dbEscaper, NPocoWriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("Imports System");
            writer.WriteLine("Imports NPoco");

            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("Namespace ");
                writer.WriteLine(settings.Namespace);
                writer.Indent();
            }

            NPocoVisualBasicWriter.WriteTables(tables, dbEscaper, settings, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("End Namespace");
            }
        }

        private static void WriteTables(TableCollection tables, IDBEscaper dbEscaper, NPocoWriterSettings settings, CodeIndentationWriter writer)
        {
            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.ClassName))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                NPocoVisualBasicWriter.WriteTable(writer, table, dbEscaper, settings);

                isFirstTable = false;
            }
        }

        private static void WriteTable(CodeIndentationWriter writer, Table table, IDBEscaper dbEscaper, NPocoWriterSettings settings)
        {
            NPocoVisualBasicWriter.WriteTableNameAttribute(writer, table, dbEscaper, settings);
            NPocoVisualBasicWriter.WritePrimaryKeyAttribute(writer, table);

            writer.WriteLine("<ExplicitColumns>");
            writer.Write("Partial ");
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "Public" : "Friend");
            writer.Write(" Class ");
            writer.WriteLine(VisualBasicTools.SafeClassName(table.ClassName));

            writer.Indent();

            if (!string.IsNullOrWhiteSpace(settings.BaseClass))
            {
                writer.Write("Inherits ");
                writer.WriteLine(VisualBasicTools.SafeClassAndNamespaceName(settings.BaseClass));
                writer.WriteLine();
            }

            NPocoVisualBasicWriter.WriteColumns(writer, table);

            writer.Outdent();
            writer.WriteLine("End Class");
        }

        private static void WriteTableNameAttribute(CodeIndentationWriter writer, Table table, IDBEscaper dbEscaper, NPocoWriterSettings settings)
        {
            writer.Write("<TableName(");

            if (settings.IncludeSchema)
            {
                string tableName = dbEscaper.EscapeSchemaName(table.Schema) + "." + dbEscaper.EscapeTableName(table.Name);
                writer.Write(VisualBasicTools.SafeString(tableName));
            }
            else
            {
                if (table.Name.Contains("."))
                {
                    // NPoco assumes that table names which contain a dot are already escaped. So we need to escape them in this case.
                    writer.Write(VisualBasicTools.SafeString(dbEscaper.EscapeTableName(table.Name)));
                }
                else
                {
                    writer.Write(VisualBasicTools.SafeString(table.Name));
                }
            }

            writer.WriteLine(")>");
        }

        private static void WritePrimaryKeyAttribute(CodeIndentationWriter writer, Table table)
        {
            ColumnCollection primaryKeyColumns = table.GetPrimaryKeyColumns();
            if (primaryKeyColumns.Count > 0)
            {
                writer.Write("<PrimaryKey(");
                writer.Write(VisualBasicTools.SafeString(string.Join(",", primaryKeyColumns.Select(c => c.Name))));

                if (!primaryKeyColumns.Any(p => p.IsAutoIncrement))
                {
                    writer.Write(", AutoIncrement = False");
                }

                if (!string.IsNullOrEmpty(table.SequenceName))
                {
                    writer.Write(", SequenceName = ");
                    writer.Write(VisualBasicTools.SafeString(table.SequenceName));
                }

                writer.WriteLine(")>");
            }
        }

        private static void WriteColumns(CodeIndentationWriter writer, Table table)
        {
            bool isFirstColumn = true;
            foreach (Column column in table.Columns.Where(c => !c.Ignore))
            {
                if (!isFirstColumn)
                {
                    writer.WriteLine();
                }

                NPocoVisualBasicWriter.WriteColumn(writer, column);

                isFirstColumn = false;
            }
        }

        private static void WriteColumn(CodeIndentationWriter writer, Column column)
        {
            writer.Write("<Column(");
            writer.Write(VisualBasicTools.SafeString(column.Name));
            writer.WriteLine(")>");

            writer.Write("Public Property ");
            writer.Write(VisualBasicTools.SafePropertyName(column.PropertyName));
            writer.Write(" As ");
            writer.WriteLine(VisualBasicTools.GetColumnType(column.PropertyType));
        }
    }
}