using System.Collections.Generic;

namespace PocoGen.Common.FileFormat
{
    internal class ForeignKeyCollection : ChangeTrackingCollection<ForeignKey>
    {
        public ForeignKeyCollection()
        {
        }

        public ForeignKeyCollection(IList<ForeignKey> list)
            : base(list)
        {
        }

        public void AddRange(IEnumerable<ForeignKey> foreignKeys)
        {
            foreach (ForeignKey foreignKey in foreignKeys)
            {
                this.Add(foreignKey);
            }
        }
    }
}