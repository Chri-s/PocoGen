using System;
using PocoGen.Common.FileFormat;

namespace PocoGen.Common
{
    public class UnknownPlugIn
    {
        private readonly PlugIn plugIn;

        internal UnknownPlugIn(PlugInType plugInType, PlugIn plugIn)
        {
            if (plugIn == null)
            {
                throw new ArgumentNullException("plugIn", "plugIn is null.");
            }

            this.PlugInType = plugInType;
            this.plugIn = plugIn;
        }

        public PlugInType PlugInType { get; private set; }

        public string Guid
        {
            get
            {
                return this.plugIn.Guid;
            }
        }

        public string Name
        {
            get
            {
                return this.plugIn.Name;
            }
        }
    }
}