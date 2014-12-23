using System;
using System.ComponentModel.Composition;
using System.IO;
using PocoGen.Common;

namespace PocoGen.OutputWriters.WcfDataContracts
{
    [Export(typeof(IOutputWriter))]
    [OutputWriter("WCF Data contract", "{FCCA31D1-D53F-4E39-ABB1-D992C19C3AAC}", SettingsType = typeof(WriterSettings))]
    public class WcfDataContractsWriter : IOutputWriter
    {
        public void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, ISettings settings, OutputInformation outputInformation)
        {
            WriterSettings writerSettings = (WriterSettings)settings;

            switch (writerSettings.Language)
            {
                case Language.CSharp:
                    CSharpWriter.Write(stream, tables, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "C#";
                    break;

                case Language.VisualBasic:
                    VisualBasicWriter.Write(stream, tables, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "VB";
                    break;

                default:
                    throw new ArgumentException("Unsupported Language in settings.Language.");
            }
        }
    }
}