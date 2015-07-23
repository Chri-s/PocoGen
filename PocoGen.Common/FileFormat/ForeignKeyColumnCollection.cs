using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common.FileFormat
{
    internal class ForeignKeyColumnCollection : Collection<ForeignKeyColumn>
    {
        public ForeignKeyColumnCollection()
        {
        }

        public ForeignKeyColumnCollection(IList<ForeignKeyColumn> list)
            : base(list)
        {
        }

        public void AddRange(IEnumerable<ForeignKeyColumn> foreignKeyColumns)
        {
            foreach (ForeignKeyColumn foreignKeyColumn in foreignKeyColumns)
            {
                this.Add(foreignKeyColumn);
            }
        }
    }
}