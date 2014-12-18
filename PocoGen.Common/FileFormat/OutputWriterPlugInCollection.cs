using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common.FileFormat
{
    public class OutputWriterPlugInCollection : Collection<OutputWriterPlugIn>
    {
        public OutputWriterPlugInCollection()
        {
        }

        public void AddRange(IEnumerable<OutputWriterPlugIn> plugIns)
        {
            foreach (OutputWriterPlugIn plugIn in plugIns)
            {
                this.Add(plugIn);
            }
        }
    }
}