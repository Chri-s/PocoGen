using System;

namespace PocoGen.Common
{
    public class ColumnChange
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
        }

        public string Name { get; private set; }

        public bool Ignore { get; set; }

        public string PropertyName { get; set; }

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