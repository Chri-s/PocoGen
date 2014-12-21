using System;
using System.Collections.Generic;

namespace PocoGen.Common
{
    public class TableChange : ChangeTrackingBase
    {
        public TableChange(string name)
        {
            this.Columns = new ColumnChangeCollection();
            this.Name = name;
        }

        public TableChange(string name, string className, bool ignore)
            : this(name)
        {
            this.ClassName = className;
            this.Ignore = ignore;

            this.AcceptChanges();
        }

        public TableChange(string name, string className, bool ignore, IEnumerable<ColumnChange> columnChanges)
            : this(name, className, ignore)
        {
            this.Columns.AddRange(columnChanges);
        }

        public string Name { get; private set; }

        private bool ignore;
        public bool Ignore
        {
            get { return this.ignore; }
            set { this.ChangeProperty(ref this.ignore, value); }
        }

        private string className;
        public string ClassName
        {
            get { return this.className; }
            set { this.ChangeProperty(ref this.className, value); }
        }

        public ColumnChangeCollection Columns { get; private set; }

        public static bool NeedsTableChange(Table table, bool ignore, string className)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table is null.");
            }

            return (ignore || table.ClassName != className);
        }

        public bool HasChangesTo(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table is null.");
            }

            return (this.Ignore || (!string.IsNullOrEmpty(this.ClassName) && this.ClassName != table.ClassName) || this.Columns.Count > 0);
        }

        public void ApplyChanges(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table is null.");
            }

            table.Ignore = this.Ignore;

            if (!string.IsNullOrEmpty(this.ClassName))
            {
                table.ClassName = this.ClassName;
            }

            this.Columns.ApplyChanges(table.Columns);
        }
    }
}