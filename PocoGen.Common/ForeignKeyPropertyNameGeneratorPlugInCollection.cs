using System.Collections.Generic;

namespace PocoGen.Common
{
    public class ForeignKeyPropertyNameGeneratorPlugInCollection : ChangeTrackingCollection<ForeignKeyPropertyNameGeneratorPlugIn>
    {
        public ForeignKeyPropertyNameGeneratorPlugInCollection()
        {
        }

        public void AddRange(IEnumerable<ForeignKeyPropertyNameGeneratorPlugIn> plugIns)
        {
            foreach (ForeignKeyPropertyNameGeneratorPlugIn plugIn in plugIns)
            {
                this.Add(plugIn);
            }
        }
    }
}