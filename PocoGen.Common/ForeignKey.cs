using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGen.Common
{
    /// <summary>
    /// Represents a foreign key between two tables in the database.
    /// </summary>
    public class ForeignKey : ICloneable
    {
        public ForeignKey()
        {
            this.Columns = new ForeignKeyColumnCollection();
        }

        public ForeignKey(string name, string childTableName, string parentTableName)
            : this()
        {
            this.Name = name;
            this.ChildTableName = childTableName;
            this.ParentTableName = parentTableName;
        }

        public ForeignKey(string schemaName, string name, string childSchemaName, string childTableName, string parentSchemaName, string parentTableName)
            : this()
        {
            this.SchemaName = schemaName;
            this.Name = name;
            this.ChildSchemaName = childSchemaName;
            this.ChildTableName = childTableName;
            this.ParentSchemaName = parentSchemaName;
            this.ParentTableName = parentTableName;
        }

        /// <summary>
        /// Gets or sets the foreign key's schema.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the foreign key's name. Can be null if the database doesn't support names for foreign keys.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the schema name of the child table.
        /// </summary>
        public string ChildSchemaName { get; set; }

        /// <summary>
        /// Gets or sets the child table's name.
        /// </summary>
        public string ChildTableName { get; set; }

        /// <summary>
        /// Gets or sets the schema name of the parent table.
        /// </summary>
        public string ParentSchemaName { get; set; }

        /// <summary>
        /// Gets or sets the parent table's name.
        /// </summary>
        public string ParentTableName { get; set; }

        /// <summary>
        /// Gets the list of columns.
        /// </summary>
        public ForeignKeyColumnCollection Columns { get; private set; }

        public ForeignKey Clone()
        {
            ForeignKey clone = new ForeignKey()
                        {
                            ChildSchemaName = this.ChildSchemaName,
                            ChildTableName = this.ChildTableName,
                            Name = this.Name,
                            ParentSchemaName = this.ParentSchemaName,
                            ParentTableName = this.ParentTableName,
                            SchemaName = this.SchemaName
                        };

            foreach (ForeignKeyColumn column in this.Columns)
            {
                clone.Columns.Add(column.Clone());
            }

            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}