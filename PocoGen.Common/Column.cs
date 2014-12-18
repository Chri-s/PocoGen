using System;

namespace PocoGen.Common
{
    public class Column : ICloneable
    {
        public Column(string name)
        {
            this.Name = name;
            this.PropertyName = string.Empty;
            this.PropertyType = null;
            this.IsPK = false;
            this.IsNullable = false;
            this.IsAutoIncrement = false;
            this.Ignore = false;
        }

        public Column(string name, ColumnBaseType propertyType, bool isNullable, bool isAutoIncrement)
            : this(name)
        {
            this.PropertyType = propertyType;
            this.IsNullable = isNullable;
            this.IsAutoIncrement = isAutoIncrement;
        }

        public string Name { get; private set; }

        public string PropertyName { get; set; }

        public ColumnBaseType PropertyType { get; set; }

        public bool IsPK { get; set; }

        public bool IsNullable { get; set; }

        public bool IsAutoIncrement { get; set; }

        public bool Ignore { get; set; }

        public Column Clone()
        {
            return new Column(this.Name)
            {
                Ignore = this.Ignore,
                IsAutoIncrement = this.IsAutoIncrement,
                IsNullable = this.IsNullable,
                IsPK = this.IsPK,
                PropertyName = this.PropertyName,
                PropertyType = this.PropertyType
            };
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}