using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common
{
    public class ForeignKeyCollection : Collection<ForeignKey>
    {
        public ForeignKeyCollection()
        {
        }

        public ForeignKeyCollection(IList<ForeignKey> list)
            : base(list)
        {
        }
    }
}