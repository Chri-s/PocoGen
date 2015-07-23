using System.Linq;
using System.Collections.Generic;

namespace PocoGen.Common.FileFormat
{
    internal class TableCollection : ChangeTrackingCollection<Table>
    {
        public TableCollection()
        {
        }

        public void AddRange(IEnumerable<Table> tables)
        {
            foreach (Table table in tables)
            {
                this.Add(table);
            }
        }

        public Table this[string schema, string name]
        {
            get
            {
                return this.FirstOrDefault(t => t.Schema == schema && t.Name == name);
            }
        }
    }
}