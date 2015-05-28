using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common
{
    public class UnknownPlugInCollection : ReadOnlyCollection<UnknownPlugIn>
    {
        public UnknownPlugInCollection(IList<UnknownPlugIn> list)
            : base(list)
        {
        }
    }
}