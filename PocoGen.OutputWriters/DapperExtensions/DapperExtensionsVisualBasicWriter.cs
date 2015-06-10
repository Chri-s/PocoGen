using System;
using System.Globalization;
using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.DapperExtensions
{
    internal static class DapperExtensionsVisualBasicWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, IDBEscaper dbEscaper, DapperExtensionsWriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            NamespaceSorter namespaces = new NamespaceSorter("System", "DapperExtensions.Mapper");
            if (!string.IsNullOrEmpty(settings.PocoNamespace) && settings.PocoNamespace != settings.Namespace)
            {
                namespaces.AddNamespace(settings.PocoNamespace);
            }

            writer.WriteLines(namespaces.GetSortedNamespaces("Imports {0}"));
            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("Namespace ");
                writer.WriteLine(settings.Namespace);
                writer.Indent();
            }

            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                isFirstTable = false;

                DapperExtensionsVisualBasicWriter.WriteClass(settings, table, dbEscaper, writer);
            }

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("End Namespace");
            }
        }

        private static void WriteClass(DapperExtensionsWriterSettings settings, Table table, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            string className = string.Format(CultureInfo.InvariantCulture, settings.ClassNameFormat, table.EffectiveClassName);

            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "Public" : "Friend");
            writer.Write(" Class ");
            writer.WriteLine(VisualBasicTools.SafeClassName(className));

            writer.Indent();

            writer.Write("Inherits ClassMapper(Of ");
            writer.Write(VisualBasicTools.SafeClassName(table.EffectiveClassName));
            writer.WriteLine(")");
            writer.WriteLine();

            DapperExtensionsVisualBasicWriter.WriteConstructor(table, dbEscaper, writer);

            writer.Outdent();
            writer.WriteLine("End Class");
        }

        private static void WriteConstructor(Table table, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            writer.WriteLine("Public Sub New()");
            writer.Indent();

            foreach (Column column in table.Columns.Where(c => !c.Ignore))
            {
                writer.Write("Me.Map(Function(x) x.");
                writer.Write(VisualBasicTools.SafePropertyName(column.EffectivePropertyName));
                writer.Write(").Column(");
                writer.Write(VisualBasicTools.SafeString(dbEscaper.EscapeColumnName(column.Name)));
                writer.Write(")");

                if (column.IsPK)
                {
                    writer.Write(".Key(KeyType.");
                    writer.Write(DapperExtensionsVisualBasicWriter.GetKeyType(column));
                    writer.Write(")");
                }

                writer.WriteLine();
            }

            if (!string.IsNullOrEmpty(table.Schema))
            {
                writer.Write("Me.Schema(");
                writer.Write(VisualBasicTools.SafeString(dbEscaper.EscapeSchemaName(table.Schema)));
                writer.WriteLine(")");
            }

            writer.Write("Me.Table(");
            writer.Write(VisualBasicTools.SafeString(dbEscaper.EscapeTableName(table.Name)));
            writer.WriteLine(");");

            writer.Outdent();
            writer.WriteLine("End Sub");
        }

        private static string GetKeyType(Column column)
        {
            if (column.IsAutoIncrement)
            {
                return "Identity";
            }

            if (column.PropertyType is ColumnType && ((ColumnType)column.PropertyType).Type == typeof(Guid))
            {
                return "Guid";
            }

            return "Assigned";
        }
    }
}