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

            if (settings.AddChangeTracking != ChangeTrackingSetting.No)
            {
                writer.WriteLine("Implements System.ComponentModel.IChangeTracking");
                writer.WriteLine();

                WriteChangeTrackingCode(writer, settings);
            }

            PocoVisualBasicWriter.WriteColumns(writer, table, settings);

            writer.Outdent();
            writer.WriteLine("End Class");
        }

        private static void WriteChangeTrackingCode(CodeIndentationWriter writer, PocoWriterSettings settings)
        {
            writer.WriteLine("Private _changeTrackingIsChanged As Boolean");
            writer.WriteLine();

            // AcceptChanges()
            if (settings.AddChangeTracking == ChangeTrackingSetting.ExplicitImplementation)
            {
                writer.Write("Private Sub ChangeTracking_AcceptChanges()");
            }
            else
            {
                writer.Write("Public Sub AcceptChanges()");
            }

            writer.WriteLine(" Implements System.ComponentModel.IChangeTracking.AcceptChanges");
            writer.Indent();
            writer.WriteLine("Me._changeTrackingIsChanged = False");
            writer.Outdent();
            writer.WriteLine("End Sub");
            writer.WriteLine();

            // IsChanged
            if (settings.AddChangeTracking == ChangeTrackingSetting.ExplicitImplementation)
            {
                writer.Write("Private ReadOnly Property ChangeTracking_IsChanged");
            }
            else
            {
                writer.Write("Public ReadOnly Property IsChanged");
            }

            writer.WriteLine(" As Boolean Implements System.ComponentModel.IChangeTracking.IsChanged");
            writer.Indent();
            writer.WriteLine("Get");
            writer.Indent();
            writer.WriteLine("Return Me._changeTrackingIsChanged");
            writer.Outdent();
            writer.WriteLine("End Get");
            writer.Outdent();
            writer.WriteLine("End Property");
            writer.WriteLine();

            // _changeTrackingSetChanged
            writer.WriteLine("Protected Sub _changeTrackingSetChanged()");
            writer.Indent();
            writer.WriteLine("Me._changeTrackingIsChanged = True");
            writer.Outdent();
            writer.WriteLine("End Sub");
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

                PocoVisualBasicWriter.WriteColumn(writer, column, settings);

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
                variableName = VisualBasicTools.SafePropertyName(variableName);

                writer.Write("Private ");
                writer.Write(variableName);
                writer.Write(" As ");
                writer.WriteLine(VisualBasicTools.GetColumnType(column.PropertyType));
            }

            writer.Write("Public Property ");
            writer.Write(VisualBasicTools.SafePropertyName(column.PropertyName));
            writer.Write(" As ");
            writer.WriteLine(VisualBasicTools.GetColumnType(column.PropertyType));

            if (PropertiesNeedImplementation(settings))
            {
                WritePropertyImplementation(writer, column, variableName);
            }
        }

        private static void WritePropertyImplementation(CodeIndentationWriter writer, Column column, string variableName)
        {
            writer.Indent();

            writer.WriteLine("Get");
            writer.Indent();
            writer.Write("Return Me.");
            writer.WriteLine(variableName);
            writer.Outdent();
            writer.WriteLine("End Get");

            writer.Write("Set(ByVal value As ");
            writer.Write(VisualBasicTools.GetColumnType(column.PropertyType));
            writer.WriteLine(")");
            writer.Indent();

            writer.Write("If Me.");
            writer.Write(variableName);
            writer.Write(" ");
            writer.Write(VisualBasicTools.GetUnequalComparison(column.PropertyType));
            writer.WriteLine(" value Then");
            writer.Indent();
            writer.Write("Me.");
            writer.Write(variableName);
            writer.WriteLine(" = value");

            writer.WriteLine("Me._changeTrackingSetChanged()");

            writer.Outdent();
            writer.WriteLine("End If");

            writer.Outdent();
            writer.WriteLine("End Set");

            writer.Outdent();
            writer.WriteLine("End Property");
        }

        private static bool PropertiesNeedImplementation(PocoWriterSettings settings)
        {
            return settings.AddChangeTracking != ChangeTrackingSetting.No;
        }
    }
}