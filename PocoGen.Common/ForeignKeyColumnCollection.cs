using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common
{
    public class ForeignKeyColumnCollection : Collection<ForeignKeyColumn>
    {
        public ForeignKeyColumnCollection()
        {
        }

        public ForeignKeyColumnCollection(IList<ForeignKeyColumn> list)
            : base(list)
        {
        }
    }
}