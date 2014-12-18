using System.Collections.Generic;
using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.NPoco
{
    internal static class NPocoFluentMappingVisualBasicWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, IDBEscaper dbEscaper, NPocoFluentMappingWriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("Imports NPoco.FluentMappings");

            if (!string.IsNullOrEmpty(settings.PocoNamespace) && settings.PocoNamespace != settings.Namespace)
            {
                writer.Write("Imports ");
                writer.WriteLine(settings.PocoNamespace);
            }

            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("Namespace ");
                writer.WriteLine(settings.Namespace);
                writer.Indent();
            }

            NPocoFluentMappingVisualBasicWriter.WriteClass(settings, tables, dbEscaper, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("End Namespace");
            }
        }

        private static void WriteClass(NPocoFluentMappingWriterSettings settings, TableCollection tables, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "Public" : "Friend");
            writer.Write(" Class ");
            writer.WriteLine(settings.ClassName);
            writer.Indent();

            writer.WriteLine("Inherits Mappings");
            writer.WriteLine();

            NPocoFluentMappingVisualBasicWriter.WriteConstructor(settings, tables, dbEscaper, writer);

            writer.WriteLine();

            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.ClassName))
            {
                writer.WriteLine();
                NPocoFluentMappingVisualBasicWriter.WriteColumnMapping(table, writer);
            }

            writer.Outdent();
            writer.WriteLine("End Class");
        }

        private static void WriteConstructor(NPocoFluentMappingWriterSettings settings, TableCollection tables, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            writer.WriteLine("Public Sub New()");
            writer.Indent();

            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.ClassName))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                isFirstTable = false;
                NPocoFluentMappingVisualBasicWriter.WriteTable(settings, table, dbEscaper, writer);
            }

            writer.Outdent();
            writer.WriteLine("End Sub");
        }

        private static void WriteTable(NPocoFluentMappingWriterSettings settings, Table table, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            writer.Write("Me.For(Of ");
            writer.Write(VisualBasicTools.SafeClassName(table.ClassName));
            writer.WriteLine(")() _");
            writer.Indent();

            writer.Write(".TableName(");

            if (table.Name.Contains("."))
            {
                // NPoco assumes that table names which contain a dot are already escaped. So we need to escape them in this case.
                writer.Write(VisualBasicTools.SafeString(dbEscaper.EscapeTableName(table.Name)));
            }
            else
            {
                writer.Write(VisualBasicTools.SafeString(table.Name));
            }

            writer.WriteLine(") _");

            NPocoFluentMappingVisualBasicWriter.WritePrimaryKey(writer, table.GetPrimaryKeyColumns());

            writer.Write(".Columns(AddressOf ");
            writer.Write(settings.ClassName);
            writer.Write(".");
            writer.Write(VisualBasicTools.SafeClassName(table.ClassName + "Columns"));
            writer.Write(", True)");
            writer.Outdent();
            writer.WriteLine();
        }

        private static void WritePrimaryKey(CodeIndentationWriter writer, ColumnCollection primaryKeyColumns)
        {
            List<Column> ignoredColumns = primaryKeyColumns.Where(pk => pk.Ignore).ToList();

            ignoredColumns.ForEach(c => primaryKeyColumns.Remove(c));

            if (primaryKeyColumns.Count == 1)
            {
                writer.Write(".PrimaryKey(Function(t) t.");
                writer.Write(CSharpTools.SafeString(primaryKeyColumns[0].PropertyName));
                writer.Write(", ");
                writer.Write(primaryKeyColumns[0].IsAutoIncrement ? "True" : "False");
                writer.WriteLine(") _");
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

                    writer.Write("Function(t) t.");
                    writer.Write(CSharpTools.SafeString(primaryKeyColumn.PropertyName));
                }

                writer.WriteLine(") _");
            }
        }

        private static void WriteColumnMapping(Table table, CodeIndentationWriter writer)
        {
            writer.Write("Private Shared Sub ");
            writer.Write(VisualBasicTools.SafeClassName(table.ClassName + "Columns"));
            writer.Write("(t As ColumnConfigurationBuilder(Of ");
            writer.Write(VisualBasicTools.SafeClassName(table.ClassName));
            writer.WriteLine("))");
            writer.Indent();

            foreach (Column column in table.Columns.Where(c => !c.Ignore))
            {
                writer.Write("t.Column(Function(x) x.");
                writer.Write(VisualBasicTools.SafePropertyName(column.PropertyName));
                writer.Write(").WithName(");
                writer.Write(VisualBasicTools.SafeString(column.Name));
                writer.WriteLine(")");
            }

            writer.Outdent();
            writer.WriteLine("End Sub");
        }
    }
}