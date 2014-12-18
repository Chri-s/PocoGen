using System.Text;

namespace PocoGen.OutputWriters
{
    internal class VisualBasicStringBuilder
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private bool isInString = false;

        public void AppendLiteral(string literal)
        {
            this.RequireInString();
            this.stringBuilder.Append(literal);
        }

        public void AppendLiteral(char literal)
        {
            this.RequireInString();
            this.stringBuilder.Append(literal);
        }

        public void AppendConstant(string constant)
        {
            this.RequireOutOfString();
            this.stringBuilder.Append(" & ");
            this.stringBuilder.Append(constant);
        }

        public void Close()
        {
            this.RequireOutOfString();
        }

        public override string ToString()
        {
            return this.stringBuilder.ToString();
        }

        private void RequireInString()
        {
            if (this.isInString)
            {
                return;
            }

            if (this.stringBuilder.Length > 0)
            {
                this.stringBuilder.Append(" & \"");
            }
            else
            {
                this.stringBuilder.Append("\"");
            }

            this.isInString = true;
        }

        private void RequireOutOfString()
        {
            if (!this.isInString)
            {
                return;
            }

            if (this.stringBuilder.Length > 0)
            {
                this.stringBuilder.Append("\"");
            }

            this.isInString = false;
        }
    }
}