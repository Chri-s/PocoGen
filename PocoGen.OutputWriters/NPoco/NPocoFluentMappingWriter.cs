using System;
using System.ComponentModel.Composition;
using System.IO;
using PocoGen.Common;

namespace PocoGen.OutputWriters.NPoco
{
    [Export(typeof(IOutputWriter))]
    [OutputWriter("NPoco Fluent Mappings", "{6DE8CEF4-895A-43DB-B3E5-CF7312B922AF}", SettingsType = typeof(NPocoFluentMappingWriterSettings))]
    public class NPocoFluentMappingWriter : IOutputWriter
    {
        public void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, ISettings settings, OutputInformation outputInformation)
        {
            NPocoFluentMappingWriterSettings writerSettings = (NPocoFluentMappingWriterSettings)settings;

            switch (writerSettings.Language)
            {
                case Language.CSharp:
                    NPocoFluentMappingCSharpWriter.Write(stream, tables, dbEscaper, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "C#";
                    break;

                case Language.VisualBasic:
                    NPocoFluentMappingVisualBasicWriter.Write(stream, tables, dbEscaper, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "VB";
                    break;

                default:
                    throw new ArgumentException("Unsupported Language in settings.Language.");
            }
        }
    }
}