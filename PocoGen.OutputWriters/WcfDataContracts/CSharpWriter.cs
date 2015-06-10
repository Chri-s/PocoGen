using System.IO;
using System.Linq;
using PocoGen.Common;

namespace PocoGen.OutputWriters.WcfDataContracts
{
    internal static class CSharpWriter
    {
        public static void Write(TextWriter textWriter, TableCollection tables, WriterSettings settings)
        {
            CodeIndentationWriter writer = new CodeIndentationWriter(
                                                textWriter: textWriter,
                                                indentationString: (settings.IndentationChar == IndentationChar.Tab) ? "\t" : " ",
                                                indentationSize: settings.IndentationSize);

            writer.WriteLine("using System;");
            writer.WriteLine("using System.Runtime.Serialization;");
            writer.WriteLine();

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Write("namespace ");
                writer.WriteLine(settings.Namespace);
                writer.WriteLine("{");
                writer.Indent();
            }

            CSharpWriter.WriteTables(tables, settings, writer);

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                writer.Outdent();
                writer.WriteLine("}");
            }
        }

        private static void WriteTables(TableCollection tables, WriterSettings settings, CodeIndentationWriter writer)
        {
            bool isFirstTable = true;
            foreach (Table table in tables.Where(t => !t.Ignore).OrderBy(t => t.EffectiveClassName))
            {
                if (!isFirstTable)
                {
                    writer.WriteLine();
                }

                CSharpWriter.WriteDataContractAttribute(writer, table, settings);
                CSharpWriter.WriteTable(writer, table, settings);

                isFirstTable = false;
            }
        }

        private static void WriteDataContractAttribute(CodeIndentationWriter writer, Table table, WriterSettings settings)
        {
            AttribteHelper dataContractAttribute = new AttribteHelper("DataContract");
            if (!string.IsNullOrEmpty(settings.XmlNamespace))
            {
                dataContractAttribute.NamedProperties.Add("Namespace", "\"" + CSharpTools.SafeString(settings.XmlNamespace) + "\"");
            }

            if (settings.WriteName)
            {
                dataContractAttribute.NamedProperties.Add("Name", "\"" + CSharpTools.SafeString(table.EffectiveClassName) + "\"");
            }

            writer.WriteLine(CSharpTools.GetAttributeString(dataContractAttribute));
        }

        private static void WriteTable(CodeIndentationWriter writer, Table table, WriterSettings settings)
        {
            writer.Write((settings.ClassModifier == ClassModifier.Public) ? "public" : "internal");
            writer.Write(" partial class ");
            writer.Write(CSharpTools.SafeClassName(table.EffectiveClassName));

            if (!string.IsNullOrWhiteSpace(settings.BaseClass))
            {
                writer.Write(" : ");
                writer.Write(CSharpTools.SafeClassAndNamespaceName(settings.BaseClass));
            }

            writer.WriteLine();

            writer.WriteLine("{");
            writer.Indent();

            CSharpWriter.WriteColumns(writer, table, settings);

            writer.Outdent();
            writer.WriteLine("}");
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

                CSharpWriter.WriteDataMemberAttribute(writer, column, settings);
                CSharpWriter.WriteColumn(writer, column);

                isFirstColumn = false;
            }
        }

        private static void WriteDataMemberAttribute(CodeIndentationWriter writer, Column column, WriterSettings settings)
        {
            AttribteHelper dataMemberAttribute = new AttribteHelper("DataMember");
            if (settings.WriteName)
            {
                dataMemberAttribute.NamedProperties.Add("Name", "\"" + CSharpTools.SafeString(column.EffectivePropertyName) + "\"");
            }

            writer.WriteLine(CSharpTools.GetAttributeString(dataMemberAttribute));
        }

        private static void WriteColumn(CodeIndentationWriter writer, Column column)
        {
            writer.Write("public ");
            writer.Write(CSharpTools.GetColumnType(column.PropertyType));
            writer.Write(" ");
            writer.Write(CSharpTools.SafePropertyName(column.EffectivePropertyName));
            writer.WriteLine(" { get; set; }");
        }
    }
}