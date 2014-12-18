using System;
using System.ComponentModel.Composition;
using System.IO;
using PocoGen.Common;

namespace PocoGen.OutputWriters.Poco
{
    [Export(typeof(IOutputWriter))]
    [OutputWriter("POCOs", "{62CD00D6-5AEE-4AA3-805F-9BDBC3C8445F}", SettingsType = typeof(PocoWriterSettings))]
    public class PocoWriter : IOutputWriter
    {
        public void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, ISettings settings, OutputInformation outputInformation)
        {
            PocoWriterSettings writerSettings = (PocoWriterSettings)settings;

            switch (writerSettings.Language)
            {
                case Language.CSharp:
                    PocoCSharpWriter.Write(stream, tables, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "C#";
                    break;

                case Language.VisualBasic:
                    PocoVisualBasicWriter.Write(stream, tables, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "VB";
                    break;

                default:
                    throw new ArgumentException("Unsupported Language in settings.Language.");
            }
        }
    }
}