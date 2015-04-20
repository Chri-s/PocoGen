using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.WcfDataContracts
{
    internal static class VisualBasicWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, WriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("Imports System");
            writer.WriteLine("Imports System.Runtime.Serialization");
            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("Namespace ");
                writer.WriteLine(settings.Namespace);
                writer.Indent();
            }

            VisualBasicWriter.WriteTables(tables, settings, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("End Namespace");
            }
        }

        private static void WriteTables(TableCollection tables, WriterSettings settings, CodeIndentationWriter writer)
        {
            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.ClassName))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                VisualBasicWriter.WriteDataContractAttribute(writer, table, settings);
                VisualBasicWriter.WriteTable(writer, table, settings);

                isFirstTable = false;
            }
        }

        private static void WriteDataContractAttribute(CodeIndentationWriter writer, Table table, WriterSettings settings)
        {
            AttribteHelper dataContractAttribute = new AttribteHelper("DataContract");
            if (!string.IsNullOrEmpty(settings.XmlNamespace))
            {
                dataContractAttribute.NamedProperties.Add("Namespace", VisualBasicTools.SafeString(settings.XmlNamespace));
            }

            if (settings.WriteName)
            {
                dataContractAttribute.NamedProperties.Add("Name", VisualBasicTools.SafeString(table.ClassName));
            }

            writer.WriteLine(VisualBasicTools.GetAttributeString(dataContractAttribute));
        }

        private static void WriteTable(CodeIndentationWriter writer, Table table, WriterSettings settings)
        {
            writer.Write("Partial ");
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "Public" : "Friend");
            writer.Write(" Class ");
            writer.WriteLine(VisualBasicTools.SafeClassName(table.ClassName));

            writer.Indent();

            if (!string.IsNullOrWhiteSpace(settings.BaseClass))
            {
                writer.Write("Inherits ");
                writer.WriteLine(VisualBasicTools.SafeClassAndNamespaceName(settings.BaseClass));
            }

            writer.WriteLine();

            VisualBasicWriter.WriteColumns(writer, table, settings);

            writer.Outdent();
            writer.WriteLine("End Class");
        }

        private static void WriteColumns(CodeIndentationWriter writer, Table table, WriterSettings settings)
        {
            bool isFirstColumn = true;
            foreach (Column column in table.Columns.Where(c => !c.Ignore))
            {
                if (!isFirstColumn)
                {
                    writer.WriteLine();
                }

                VisualBasicWriter.WriteDataMemberAttribute(writer, column, settings);
                VisualBasicWriter.WriteColumn(writer, column);

                isFirstColumn = false;
            }
        }

        private static void WriteDataMemberAttribute(CodeIndentationWriter writer, Column column, WriterSettings settings)
        {
            AttribteHelper dataMemberAttribute = new AttribteHelper("DataMember");
            if (settings.WriteName)
            {
                dataMemberAttribute.NamedProperties.Add("Name", VisualBasicTools.SafeString(column.PropertyName));
            }

            writer.WriteLine(VisualBasicTools.GetAttributeString(dataMemberAttribute));
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