using System.Collections.Generic;

namespace PocoGen.Common
{
    public class OutputWriterPlugInCollection : ChangeTrackingCollection<OutputWriterPlugIn>
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