using System;

namespace PocoGen.Common
{
    public class ColumnChange : ChangeTrackingBase
    {
        public ColumnChange(string name)
        {
            this.Name = name;
        }

        public ColumnChange(string name, string propertyName, bool ignore)
            : this(name)
        {
            this.PropertyName = propertyName;
            this.Ignore = ignore;

            this.AcceptChanges();
        }

        public string Name { get; private set; }

        private bool ignore;
        public bool Ignore
        {
            get { return this.ignore; }
            set { this.ChangeProperty(ref this.ignore, value); }
        }

        private string propertyName;
        public string PropertyName
        {
            get { return this.propertyName; }
            set { this.ChangeProperty(ref this.propertyName, value); }
        }

        public static bool NeedsColumnChange(Column column, bool ignore, string propertyName)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column", "column is null.");
            }

            return (ignore || column.PropertyName != propertyName);
        }

        public bool HasChangesTo(Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column", "column is null.");
            }

            return (this.Ignore || (!string.IsNullOrEmpty(this.PropertyName) && this.PropertyName != column.PropertyName));
        }

        public void ApplyChanges(Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column", "column is null.");
            }

            column.Ignore = this.Ignore;

            if (!string.IsNullOrEmpty(this.PropertyName))
            {
                column.PropertyName = this.PropertyName;
            }
        }
    }
}