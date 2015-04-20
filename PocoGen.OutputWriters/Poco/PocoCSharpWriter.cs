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
            writer.Write(CSharpTools.SafeClassName(table.ClassName));

            if (settings.AddChangeTracking != ChangeTrackingSetting.No)
            {
                writer.Write(" : System.ComponentModel.IChangeTracking");
            }

            writer.WriteLine();

            writer.WriteLine("{");
            writer.Indent();

            if (settings.AddChangeTracking == ChangeTrackingSetting.ImplicitImplementation)
            {
                WriteChangeTrackingCodeImplicit(writer);
            }
            else if (settings.AddChangeTracking == ChangeTrackingSetting.ExplicitImplementation)
            {
                WriteChangeTrackingCodeExplicit(writer);
            }

            PocoCSharpWriter.WriteColumns(writer, table, settings);

            writer.Outdent();
            writer.WriteLine("}");
        }

        private static void WriteChangeTrackingCodeImplicit(CodeIndentationWriter writer)
        {
            writer.WriteLine("public bool IsChanged { get; protected set; }");
            writer.WriteLine();
            writer.WriteLine("public void AcceptChanges()");
            writer.WriteLine("{");
            writer.Indent();
            writer.WriteLine("this.IsChanged = false;");
            writer.Outdent();
            writer.WriteLine("}");
            writer.WriteLine();
        }

        private static void WriteChangeTrackingCodeExplicit(CodeIndentationWriter writer)
        {
            writer.WriteLine("private bool _changeTrackingIsChanged;");
            writer.WriteLine();
            writer.WriteLine("bool System.ComponentModel.IChangeTracking.IsChanged { get { return this._changeTrackingIsChanged; } }");
            writer.WriteLine();
            writer.WriteLine("void System.ComponentModel.IChangeTracking.AcceptChanges()");
            writer.WriteLine("{");
            writer.Indent();
            writer.WriteLine("this._changeTrackingIsChanged = false;");
            writer.Outdent();
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("protected void _changeTrackingSetChanged()");
            writer.WriteLine("{");
            writer.Indent();
            writer.WriteLine("this._changeTrackingIsChanged = true;");
            writer.Outdent();
            writer.WriteLine("}");
            writer.WriteLine();
        }

        private static void WriteColumns(CodeIndentationWriter writer, Table table, PocoWriterSettings settings)
        {
            bool isFirstColumn = true;
            foreach (Column column in table.Columns.Where(c => !c.Ignore))
            {
                if (!isFirstColumn)
                {
                    writer.WriteLine();
                }

                PocoCSharpWriter.WriteColumn(writer, column, settings);

                isFirstColumn = false;
            }
        }

        private static void WriteColumn(CodeIndentationWriter writer, Column column, PocoWriterSettings settings)
        {
            string variableName = null;
            if (PropertiesNeedImplementation(settings))
            {
                variableName = column.PropertyName;
                variableName = "_" + char.ToLowerInvariant(variableName[0]) + variableName.Substring(1);
                variableName = CSharpTools.SafePropertyName(variableName);

                writer.Write("private ");
                writer.Write(CSharpTools.GetColumnType(column.PropertyType));
                writer.Write(" ");
                writer.Write(variableName);
                writer.WriteLine(";");
            }

            writer.Write("public ");
            writer.Write(CSharpTools.GetColumnType(column.PropertyType));
            writer.Write(" ");
            writer.Write(CSharpTools.SafePropertyName(column.PropertyName));

            if (PropertiesNeedImplementation(settings))
            {
                WritePropertyImplementation(writer, settings, variableName);
            }
            else
            {
                writer.WriteLine(" { get; set; }");
            }
        }

        private static void WritePropertyImplementation(CodeIndentationWriter writer, PocoWriterSettings settings, string variableName)
        {
            writer.WriteLine();
            writer.WriteLine("{");
            writer.Indent();

            writer.WriteLine("get");
            writer.WriteLine("{");
            writer.Indent();
            writer.Write("return this.");
            writer.Write(variableName);
            writer.WriteLine(";");
            writer.Outdent();
            writer.WriteLine("}");

            writer.WriteLine("set");
            writer.WriteLine("{");
            writer.Indent();

            writer.Write("if (this.");
            writer.Write(variableName);
            writer.WriteLine(" != value)");
            writer.WriteLine("{");
            writer.Indent();
            writer.Write("this.");
            writer.Write(variableName);
            writer.WriteLine(" = value;");

            if (settings.AddChangeTracking == ChangeTrackingSetting.ImplicitImplementation)
            {
                writer.WriteLine("this.IsChanged = true;");
            }
            else
            {
                writer.WriteLine("this._changeTrackingSetChanged();");
            }

            writer.Outdent();
            writer.WriteLine("}");

            writer.Outdent();
            writer.WriteLine("}");

            writer.Outdent();
            writer.WriteLine("}");
        }

        private static bool PropertiesNeedImplementation(PocoWriterSettings settings)
        {
            return settings.AddChangeTracking != ChangeTrackingSetting.No;
        }
    }
}