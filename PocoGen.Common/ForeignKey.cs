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
    public class ForeignKey
    {
        public ForeignKey(string name, string childTableName, string parentTableName)
        {
            this.Columns = new ForeignKeyColumnCollection();
            this.Schema = string.Empty;
            this.Name = name;
            this.ChildSchema = string.Empty;
            this.ChildTableName = childTableName;
            this.ParentSchema = string.Empty;
            this.ParentTableName = parentTableName;
        }

        public ForeignKey(string schema, string name, string childSchemaName, string childTableName, string parentSchemaName, string parentTableName)
            : this(name, childTableName, parentTableName)
        {
            this.Schema = schema;
            this.ChildSchema = childSchemaName;
            this.ParentSchema = parentSchemaName;
        }

        /// <summary>
        /// Gets or sets the foreign key's schema. Can be String.Empty if the database doesn't support schemas for foreign keys.
        /// </summary>
        public string Schema { get; private set; }

        /// <summary>
        /// Gets or sets the foreign key's name. Can be String.Empty if the database doesn't support names for foreign keys.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the schema name of the child table. Can be String.Empty if the database doesn't support schemas.
        /// </summary>
        public string ChildSchema { get; private set; }

        /// <summary>
        /// Gets or sets the child table's name.
        /// </summary>
        public string ChildTableName { get; private set; }

        /// <summary>
        /// Gets or sets the schema name of the parent table. Can be String.Empty if the database doesn't support schemas.
        /// </summary>
        public string ParentSchema { get; private set; }

        /// <summary>
        /// Gets or sets the parent table's name.
        /// </summary>
        public string ParentTableName { get; private set; }

        /// <summary>
        /// Gets the list of columns.
        /// </summary>
        public ForeignKeyColumnCollection Columns { get; private set; }

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public RelationshipType RelationshipType { get; set; }
    }
}