using System;
using PocoGen.Common;

namespace PocoGen.Gui.Domain
{
    internal class OutputWriterModel
    {
        private readonly OutputWriterPlugIn plugIn;

        public OutputWriterModel(OutputWriterPlugIn plugIn)
        {
            if (plugIn == null)
            {
                throw new ArgumentNullException("plugIn", "plugIn is null.");
            }

            this.plugIn = plugIn;
        }

        public string Name
        {
            get
            {
                return this.plugIn.Name;
            }
        }

        public OutputWriterPlugIn OutputWriter
        {
            get
            {
                return this.plugIn;
            }
        }
    }
}