using System;
using PocoGen.Common.FileFormat;

namespace PocoGen.Common
{
    public class UnrecognizedPlugIn
    {
        public UnrecognizedPlugIn(PlugInType plugInType, PlugIn plugIn)
        {
            if (plugIn == null)
            {
                throw new ArgumentNullException("plugIn", "plugIn is null.");
            }

            this.PlugInType = plugInType;
            this.PlugIn = plugIn;
        }

        public PlugInType PlugInType { get; private set; }

        public PlugIn PlugIn { get; set; }
    }
}