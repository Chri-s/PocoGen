using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.NPoco
{
    internal static class NPocoCSharpWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, IDBEscaper dbEscaper, NPocoWriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("using System;");
            writer.WriteLine("using NPoco;");

            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("namespace ");
                writer.WriteLine(settings.Namespace);
                writer.WriteLine("{");
                writer.Indent();
            }

            NPocoCSharpWriter.WriteTables(tables, dbEscaper, settings, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("}");
            }
        }

        private static void WriteTables(TableCollection tables, IDBEscaper dbEscaper, NPocoWriterSettings settings, CodeIndentationWriter writer)
        {
            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.GeneratedClassName))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                NPocoCSharpWriter.WriteTable(writer, table, dbEscaper, settings);

                isFirstTable = false;
            }
        }

        private static void WriteTable(CodeIndentationWriter writer, Table table, IDBEscaper dbEscaper, NPocoWriterSettings settings)
        {
            NPocoCSharpWriter.WriteTableNameAttribute(writer, table, dbEscaper, settings);
            NPocoCSharpWriter.WritePrimaryKeyAttribute(writer, table);

            writer.WriteLine("[ExplicitColumns]");
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "public" : "internal");
            writer.Write(" partial class ");
            writer.Write(CSharpTools.SafeClassName(table.GeneratedClassName));

            if (!string.IsNullOrWhiteSpace(settings.BaseClass))
            {
                writer.Write(" : ");
                writer.Write(CSharpTools.SafeClassAndNamespaceName(settings.BaseClass));
            }

            writer.WriteLine();

            writer.WriteLine("{");
            writer.Indent();

            NPocoCSharpWriter.WriteColumns(writer, table);

            writer.Outdent();
            writer.WriteLine("}");
        }

        private static void WriteTableNameAttribute(CodeIndentationWriter writer, Table table, IDBEscaper dbEscaper, NPocoWriterSettings settings)
        {
            writer.Write("[TableName(\"");

            if (settings.IncludeSchema)
            {
                string tableName = dbEscaper.EscapeSchemaName(table.Schema) + "." + dbEscaper.EscapeTableName(table.Name);
                writer.Write(CSharpTools.SafeString(tableName));
            }
            else
            {
                if (table.Name.Contains("."))
                {
                    // NPoco assumes that table names which contain a dot are already escaped. So we need to escape them in this case.
                    writer.Write(CSharpTools.SafeString(dbEscaper.EscapeTableName(table.Name)));
                }
                else
                {
                    writer.Write(CSharpTools.SafeString(table.Name));
                }
            }

            writer.WriteLine("\")]");
        }

        private static void WritePrimaryKeyAttribute(CodeIndentationWriter writer, Table table)
        {
            ColumnCollection primaryKeyColumns = table.GetPrimaryKeyColumns();
            if (primaryKeyColumns.Count > 0)
            {
                writer.Write("[PrimaryKey(\"");
                writer.Write(CSharpTools.SafeString(string.Join(",", primaryKeyColumns.Select(c => c.Name))));
                writer.Write("\"");

                if (!primaryKeyColumns.Any(p => p.IsAutoIncrement))
                {
                    writer.Write(", AutoIncrement = false");
                }

                if (!string.IsNullOrEmpty(table.SequenceName))
                {
                    writer.Write(", SequenceName = \"");
                    writer.Write(CSharpTools.SafeString(table.SequenceName));
                    writer.Write("\"");
                }

                writer.WriteLine(")]");
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

                NPocoCSharpWriter.WriteColumn(writer, column);

                isFirstColumn = false;
            }
        }

        private static void WriteColumn(CodeIndentationWriter writer, Column column)
        {
            writer.Write("[Column(\"");
            writer.Write(CSharpTools.SafeString(column.Name));
            writer.WriteLine("\")]");

            writer.Write("public ");
            writer.Write(CSharpTools.GetColumnType(column.PropertyType));
            writer.Write(" ");
            writer.Write(CSharpTools.SafePropertyName(column.EffectivePropertyName));
            writer.WriteLine(" { get; set; }");
        }
    }
}