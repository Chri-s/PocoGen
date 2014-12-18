using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.Poco
{
    internal static class PocoVisualBasicWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, PocoWriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("Imports System");
            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("Namespace ");
                writer.WriteLine(settings.Namespace);
                writer.Indent();
            }

            PocoVisualBasicWriter.WriteTables(tables, settings, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("End Namespace");
            }
        }

        private static void WriteTables(TableCollection tables, PocoWriterSettings settings, CodeIndentationWriter writer)
        {
            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.ClassName))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                PocoVisualBasicWriter.WriteTable(writer, table, settings);

                isFirstTable = false;
            }
        }

        private static void WriteTable(CodeIndentationWriter writer, Table table, PocoWriterSettings settings)
        {
            writer.Write("Partial ");
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "Public" : "Friend");
            writer.Write(" Class ");
            writer.WriteLine(VisualBasicTools.SafeClassName(table.ClassName));

            writer.Indent();

            PocoVisualBasicWriter.WriteColumns(writer, table);

            writer.Outdent();
            writer.WriteLine("End Class");
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

                PocoVisualBasicWriter.WriteColumn(writer, column);

                isFirstColumn = false;
            }
        }

        private static void WriteColumn(CodeIndentationWriter writer, Column column)
        {
            writer.Write("Public Property ");
            writer.Write(VisualBasicTools.SafePropertyName(column.PropertyName));
            writer.Write(" As ");
            writer.WriteLine(VisualBasicTools.GetColumnType(column.PropertyType));
        }
    }
}