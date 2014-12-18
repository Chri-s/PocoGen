using System;
using System.ComponentModel.Composition;
using System.IO;
using PocoGen.Common;

namespace PocoGen.OutputWriters.DapperExtensions
{
    [Export(typeof(IOutputWriter))]
    [OutputWriter("github.com/tmsmith/Dapper-Extensions Mappings", "{BA824ECC-E6C2-4448-8BCE-7FB50037B1A9}", SettingsType = typeof(DapperExtensionsWriterSettings))]
    public class DapperExtensionsWriter : IOutputWriter
    {
        public void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, ISettings settings, OutputInformation outputInformation)
        {
            DapperExtensionsWriterSettings writerSettings = (DapperExtensionsWriterSettings)settings;

            switch (writerSettings.Language)
            {
                case Language.CSharp:
                    DapperExtensionsCSharpWriter.Write(stream, tables, dbEscaper, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "C#";
                    break;

                case Language.VisualBasic:
                    DapperExtensionsVisualBasicWriter.Write(stream, tables, dbEscaper, writerSettings);
                    outputInformation.SyntaxHighlightingLanguage = "VB";
                    break;

                default:
                    throw new ArgumentException("Unsupported Language in settings.Language.");
            }
        }
    }
}