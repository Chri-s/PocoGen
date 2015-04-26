using System;
using System.Linq;

namespace PocoGen.Common
{
    public class Table : ICloneable
    {
        public ColumnCollection Columns { get; private set; }

        /// <summary>
        /// Gets the list of foreign keys which are referenced by this table.
        /// </summary>
        public ForeignKeyCollection ParentForeignKeys { get; private set; }

        /// <summary>
        /// Gets the list of foreign keys which reference this table.
        /// </summary>
        public ForeignKeyCollection ChildForeignKeys { get; private set; }

        public string Name { get; private set; }

        public string Schema { get; set; }

        public bool IsView { get; set; }

        public string ClassName { get; set; }

        public string SequenceName { get; set; }

        public bool Ignore { get; set; }

        public Table(string name)
        {
            this.Columns = new ColumnCollection();
            this.ParentForeignKeys = new ForeignKeyCollection();
            this.ChildForeignKeys = new ForeignKeyCollection();
            this.ClassName = string.Empty;
            this.Ignore = false;
            this.Name = name;
            this.SequenceName = string.Empty;
            this.IsView = false;
        }

        public Table(string schema, string name, bool isView)
            : this(name)
        {
            this.Schema = schema;
            this.Name = name;
            this.IsView = isView;
        }

        public ColumnCollection GetPrimaryKeyColumns()
        {
            ColumnCollection primaryKey = new ColumnCollection();
            primaryKey.AddRange(this.Columns.Where(c => !c.Ignore && c.IsPK));

            return primaryKey;
        }

        public Table Clone()
        {
            Table clone = new Table(this.Name)
            {
                ClassName = this.ClassName,
                Ignore = this.Ignore,
                IsView = this.IsView,
                Schema = this.Schema,
                SequenceName = this.SequenceName
            };

            clone.Columns.AddRange(from c in this.Columns
                                   select c.Clone());

            foreach (ForeignKey parentForeignKey in clone.ParentForeignKeys)
            {
                clone.ParentForeignKeys.Add(parentForeignKey.Clone());
            }

            foreach (ForeignKey childForeignKey in clone.ChildForeignKeys)
            {
                clone.ChildForeignKeys.Add(childForeignKey.Clone());
            }

            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}