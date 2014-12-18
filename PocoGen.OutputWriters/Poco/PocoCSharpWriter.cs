using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.Poco
{
    internal static class PocoCSharpWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, PocoWriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("using System;");
            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("namespace ");
                writer.WriteLine(settings.Namespace);
                writer.WriteLine("{");
                writer.Indent();
            }

            PocoCSharpWriter.WriteTables(tables, settings, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("}");
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

                PocoCSharpWriter.WriteTable(writer, table, settings);

                isFirstTable = false;
            }
        }

        private static void WriteTable(CodeIndentationWriter writer, Table table, PocoWriterSettings settings)
        {
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "public" : "internal");
            writer.Write(" partial class ");
            writer.WriteLine(CSharpTools.SafeClassName(table.ClassName));

            writer.WriteLine("{");
            writer.Indent();

            PocoCSharpWriter.WriteColumns(writer, table);

            writer.Outdent();
            writer.WriteLine("}");
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

                PocoCSharpWriter.WriteColumn(writer, column);

                isFirstColumn = false;
            }
        }

        private static void WriteColumn(CodeIndentationWriter writer, Column column)
        {
            writer.Write("public ");
            writer.Write(CSharpTools.GetColumnType(column.PropertyType));
            writer.Write(" ");
            writer.Write(CSharpTools.SafePropertyName(column.PropertyName));
            writer.WriteLine(" { get; set; }");
        }
    }
}