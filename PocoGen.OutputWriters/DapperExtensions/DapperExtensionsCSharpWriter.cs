using System;
using System.Globalization;
using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.DapperExtensions
{
    internal static class DapperExtensionsCSharpWriter
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

            writer.WriteLines(namespaces.GetSortedNamespaces("using {0};"));
            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("namespace ");
                writer.WriteLine(settings.Namespace);
                writer.WriteLine("{");
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

                DapperExtensionsCSharpWriter.WriteClass(settings, table, dbEscaper, writer);
            }

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("}");
            }
        }

        private static void WriteClass(DapperExtensionsWriterSettings settings, Table table, IDBEscaper dbEscaper, CodeIndentationWriter writer)
        {
            string className = string.Format(CultureInfo.InvariantCulture, settings.ClassNameFormat, table.ClassName);

            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "public" : "internal");
            writer.Write(" class ");
            writer.Write(CSharpTools.SafeClassName(className));
            writer.Write(" : ClassMapper<");
            writer.Write(CSharpTools.SafeClassName(table.ClassName));
            writer.WriteLine(">");

            writer.WriteLine("{");
            writer.Indent();

            DapperExtensionsCSharpWriter.WriteConstructor(table, dbEscaper, writer, className);

            writer.Outdent();
            writer.WriteLine("}");
        }

        private static void WriteConstructor(Table table, IDBEscaper dbEscaper, CodeIndentationWriter writer, string className)
        {
            writer.Write("public ");
            writer.Write(CSharpTools.SafeClassName(className));
            writer.WriteLine("()");

            writer.WriteLine("{");
            writer.Indent();

            foreach (Column column in table.Columns.Where(c => !c.Ignore))
            {
                writer.Write("this.Map(x => x.");
                writer.Write(CSharpTools.SafePropertyName(column.PropertyName));
                writer.Write(").Column(\"");
                writer.Write(CSharpTools.SafeString(dbEscaper.EscapeColumnName(column.Name)));
                writer.Write("\")");

                if (column.IsPK)
                {
                    writer.Write(".Key(KeyType.");
                    writer.Write(DapperExtensionsCSharpWriter.GetKeyType(column));
                    writer.Write(")");
                }

                writer.WriteLine(";");
            }

            if (!string.IsNullOrEmpty(table.Schema))
            {
                writer.Write("this.Schema(\"");
                writer.Write(CSharpTools.SafeString(dbEscaper.EscapeSchemaName(table.Schema)));
                writer.WriteLine("\");");
            }

            writer.Write("this.Table(\"");
            writer.Write(CSharpTools.SafeString(dbEscaper.EscapeTableName(table.Name)));
            writer.WriteLine("\");");

            writer.Outdent();
            writer.WriteLine("}");
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