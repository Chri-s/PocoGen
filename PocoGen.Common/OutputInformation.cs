using System;

namespace PocoGen.Common
{
    public class OutputInformation
    {
        public OutputInformation()
        {
            this.SyntaxHighlightingLanguage = string.Empty;
        }

        public string SyntaxHighlightingLanguage { get; set; }
    }
}