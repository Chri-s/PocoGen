using System.Collections.Generic;
using System.IO;

namespace PocoGen.OutputWriters
{
    internal class CodeIndentationWriter
    {
        private readonly TextWriter textWriter;
        private int indentationSteps = 0;
        private bool currentLineIsIndentated;

        public CodeIndentationWriter(TextWriter textWriter, string indentationString, int indentationSize)
        {
            this.textWriter = textWriter;
            this.IndentationString = indentationString;
            this.IndentationSize = indentationSize;
        }

        public string IndentationString { get; private set; }

        public int IndentationSize { get; private set; }

        public void Indent()
        {
            this.indentationSteps++;
        }

        public void Outdent()
        {
            if (this.indentationSteps > 0)
            {
                this.indentationSteps--;
            }
        }

        public void Write(string text)
        {
            this.IndentateCurrentLine();
            this.textWriter.Write(text);
        }

        public void WriteLine(string text)
        {
            this.IndentateCurrentLine();
            this.textWriter.WriteLine(text);
            this.currentLineIsIndentated = false;
        }

        public void WriteLine()
        {
            this.WriteLine(string.Empty);
            this.currentLineIsIndentated = false;
        }

        public void WriteLines(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                this.WriteLine(line);
            }
        }

        private void IndentateCurrentLine()
        {
            if (this.currentLineIsIndentated)
            {
                return;
            }

            int count = this.indentationSteps * this.IndentationSize;
            for (int i = 0; i < count; i++)
            {
                this.textWriter.Write(this.IndentationString);
            }

            this.currentLineIsIndentated = true;
        }
    }
}