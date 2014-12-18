using System.Collections.Generic;
using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.NPoco
{
    internal static class NPocoFluentMappingCSharpWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, IDBEscaper dbEscaper, NPocoFluentMappingWriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("using System;");
            writer.WriteLine("using NPoco.FluentMappings;");

            if (!string.IsNullOrEmpty(settings.PocoNamespace) && settings.PocoNamespace != settings.Namespace)
            {
                writer.Write("using ");
                writer.Write(settings.PocoNamespace);
                writer.WriteLine(";");
            }

            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("namespace ");
                writer.WriteLine(settings.Namespace);
                writer.WriteLine("{");
                writer.Indent();
            }

            NPocoFluentMappingCSharpWriter.WriteClass(settings, tables, dbEscaper, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("}");
            }
        }

        private static void WriteClass(NPocoFluentMappingWriterSettings settings, TableCollection tables, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "public" : "internal");
            writer.Write(" class ");
            writer.Write(settings.ClassName);
            writer.WriteLine(" : Mappings");

            writer.WriteLine("{");
            writer.Indent();

            NPocoFluentMappingCSharpWriter.WriteConstructor(settings, tables, dbEscaper, writer);

            writer.Outdent();
            writer.WriteLine("}");
        }

        private static void WriteConstructor(NPocoFluentMappingWriterSettings settings, TableCollection tables, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            writer.Write("public ");
            writer.Write(settings.ClassName);
            writer.WriteLine("()");

            writer.WriteLine("{");
            writer.Indent();

            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.ClassName))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                isFirstTable = false;
                NPocoFluentMappingCSharpWriter.WriteTable(table, dbEscaper, writer);
            }

            writer.Outdent();
            writer.WriteLine("}");
        }

        private static void WriteTable(Table table, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            writer.Write("this.For<");
            writer.Write(CSharpTools.SafeClassName(table.ClassName));
            writer.WriteLine(">()");
            writer.Indent();

            writer.Write(".TableName(\"");

            if (table.Name.Contains("."))
            {
                // NPoco assumes that table names which contain a dot are already escaped. So we need to escape them in this case.
                writer.Write(CSharpTools.SafeString(dbEscaper.EscapeTableName(table.Name)));
            }
            else
            {
                writer.Write(CSharpTools.SafeString(table.Name));
            }

            writer.WriteLine("\")");

            NPocoFluentMappingCSharpWriter.WritePrimaryKey(writer, table.GetPrimaryKeyColumns());

            writer.WriteLine(".Columns(");
            writer.Indent();
            writer.WriteLine("t =>");
            writer.WriteLine("{");
            writer.Indent();

            foreach (Column column in table.Columns.Where(c => !c.Ignore))
            {
                writer.Write("t.Column(x => x.");
                writer.Write(CSharpTools.SafePropertyName(column.PropertyName));
                writer.Write(").WithName(\"");
                writer.Write(CSharpTools.SafeString(column.Name));
                writer.WriteLine("\");");
            }

            writer.Outdent();
            writer.WriteLine("},");
            writer.WriteLine("true);");
            writer.Outdent();

            writer.Outdent();
        }

        private static void WritePrimaryKey(CodeIndentationWriter writer, ColumnCollection primaryKeyColumns)
        {
            List<Column> ignoredColumns = primaryKeyColumns.Where(pk => pk.Ignore).ToList();

            ignoredColumns.ForEach(c => primaryKeyColumns.Remove(c));

            if (primaryKeyColumns.Count == 1)
            {
                writer.Write(".PrimaryKey(t => t.");
                writer.Write(CSharpTools.SafeString(primaryKeyColumns[0].PropertyName));
                writer.Write(", ");
                writer.Write(primaryKeyColumns[0].IsAutoIncrement ? "true" : "false");
                writer.WriteLine(")");
            }
            else if (primaryKeyColumns.Count > 1)
            {
                writer.Write(".CompositePrimaryKey(");

                bool isFirstColumn = true;
                foreach (Column primaryKeyColumn in primaryKeyColumns)
                {
                    if (!isFirstColumn)
                    {
                        writer.Write(", ");
                    }

                    isFirstColumn = false;

                    writer.Write("t => t.");
                    writer.Write(CSharpTools.SafeString(primaryKeyColumn.PropertyName));
                }

                writer.WriteLine(")");
            }
        }
    }
}