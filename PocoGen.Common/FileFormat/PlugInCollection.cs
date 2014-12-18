using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common.FileFormat
{
    public class PlugInCollection : Collection<PlugIn>
    {
        public PlugInCollection()
        {
        }

        public void AddRange(IEnumerable<PlugIn> plugIns)
        {
            foreach (PlugIn plugIn in plugIns)
            {
                this.Add(plugIn);
            }
        }
    }
}