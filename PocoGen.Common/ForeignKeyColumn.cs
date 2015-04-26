using System;

namespace PocoGen.Common
{
    /// <summary>
    /// Represents a column mapping in a foreign key.
    /// </summary>
    public class ForeignKeyColumn : ICloneable
    {
        public ForeignKeyColumn()
        {
        }

        public ForeignKeyColumn(string parentTablesColumnName, string childTablesColumnName)
        {
            this.ParentTablesColumnName = parentTablesColumnName;
            this.ChildTablesColumnName = childTablesColumnName;
        }

        /// <summary>
        /// Gets or sets the parent table's column name.
        /// </summary>
        public string ParentTablesColumnName { get; set; }

        /// <summary>
        /// Gets or sets the child table's column name.
        /// </summary>
        public string ChildTablesColumnName { get; set; }

        public ForeignKeyColumn Clone()
        {
            return new ForeignKeyColumn()
            {
                ChildTablesColumnName = this.ChildTablesColumnName,
                ParentTablesColumnName = this.ParentTablesColumnName
            };
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}