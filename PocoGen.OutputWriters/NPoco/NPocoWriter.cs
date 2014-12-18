using System;
using System.ComponentModel.Composition;
using System.IO;
using PocoGen.Common;

namespace PocoGen.OutputWriters.NPoco
{
    [Export(typeof(IOutputWriter))]
    [OutputWriter("Attributed NPoco POCOs", "{C930FABE-0647-43F2-9A22-ED483E6AAB27}", SettingsType = typeof(NPocoWriterSettings))]
    public class NPocoWriter : IOutputWriter
    {
        public void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, ISettings settings, OutputInformation outputInformation)
        {
            NPocoWriterSettings writerSettings = (NPocoWriterSettings)settings;

            switch (writerSettings.Language)
            {
                case Language.CSharp:
                    NPocoCSharpWriter.Write(stream, tables, dbEscaper, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "C#";
                    break;

                case Language.VisualBasic:
                    NPocoVisualBasicWriter.Write(stream, tables, dbEscaper, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "VB";
                    break;

                default:
                    throw new ArgumentException("Unsupported settings.Language");
            }
        }
    }
}