using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PocoGen.Common
{
    /// <summary>
    /// Represents a foreign key between two tables in the database.
    /// </summary>
    public class ForeignKey : INotifyPropertyChanged, IForeignKeySummary
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new foreign key.
        /// </summary>
        /// <param name="name">The foreign key's name. Can be String.Empty if the database doesn't name foreign keys.</param>
        /// <param name="childTableName">The foreign key's child table.</param>
        /// <param name="parentTableName">The foreign key's parent table.</param>
        public ForeignKey(string name, string childTableName, string parentTableName)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(childTableName))
            {
                throw new ArgumentException("childTableName is null or empty.", "childTableName");
            }

            if (string.IsNullOrEmpty(parentTableName))
            {
                throw new ArgumentException("parentTableName is null or empty.", "parentTableName");
            }

            this.Columns = new ForeignKeyColumnCollection();
            this.Schema = string.Empty;
            this.Name = name;
            this.ChildSchema = string.Empty;
            this.ChildTableName = childTableName;
            this.ParentSchema = string.Empty;
            this.ParentTableName = parentTableName;
        }

        /// <summary>
        /// Creates a new foreign key.
        /// </summary>
        /// <param name="schema">The foreign key's schema. Can be String.Empty if the database doesn't support schemas.</param>
        /// <param name="name">The foreign key's name. Can be String.Empty if the database doesn't name foreign keys.</param>
        /// <param name="childSchemaName">The child table's schema. Can be String.Empty if the database doesn't support schemas.</param>
        /// <param name="childTableName">The foreign key's child table.</param>
        /// <param name="parentSchemaName">The parent table's schema. Can be String.Empty if the database doesn't support schemas.</param>
        /// <param name="parentTableName">The foreign key's parent table.</param>
        public ForeignKey(string schema, string name, string childSchemaName, string childTableName, string parentSchemaName, string parentTableName)
            : this(name, childTableName, parentTableName)
        {
            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }

            if (childSchemaName == null)
            {
                throw new ArgumentNullException("childSchemaName");
            }

            if (parentSchemaName == null)
            {
                throw new ArgumentNullException("parentSchemaName");
            }

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

        private Table childTable;
        /// <summary>
        /// Gets the child table.
        /// </summary>
        public Table ChildTable
        {
            get
            {
                return this.childTable;
            }
            internal set
            {
                if (this.childTable == value)
                {
                    return;
                }

                this.childTable = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the schema name of the parent table. Can be String.Empty if the database doesn't support schemas.
        /// </summary>
        public string ParentSchema { get; private set; }

        /// <summary>
        /// Gets or sets the parent table's name.
        /// </summary>
        public string ParentTableName { get; private set; }

        private Table parentTable;
        /// <summary>
        /// Gets the parent table.
        /// </summary>
        public Table ParentTable
        {
            get
            {
                return this.parentTable;
            }
            internal set
            {
                if (this.parentTable == value)
                {
                    return;
                }

                this.parentTable = value;
                this.OnPropertyChanged();
            }
        }

        private string generatedParentPropertyName;
        /// <summary>
        /// Gets or sets the parent property name which was generated by the parent property name generators.
        /// </summary>
        public string GeneratedParentPropertyName
        {
            get
            {
                return this.generatedParentPropertyName;
            }
            set
            {
                if (this.generatedParentPropertyName == value)
                {
                    return;
                }

                this.generatedParentPropertyName = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.EffectiveParentPropertyName));
            }
        }

        /// <summary>
        /// Gets or sets the effective parent property name. This is the <see cref="GeneratedParentPropertyName"/> if the user didn't change it, otherwise the user-changed parent property name.
        /// </summary>
        public string EffectiveParentPropertyName
        {
            get
            {
                // If the user changed the parent property name, it was saved to userChangedParentPropertyName. Otherwise it is the GeneratedParentPropertyName.
                return this.userChangedParentPropertyName ?? this.GeneratedParentPropertyName;
            }
            set
            {
                if (this.EffectiveParentPropertyName == value)
                {
                    return;
                }

                // If the user changes the parent property name, save the override in userChangedParentPropertyName.
                if (this.GeneratedParentPropertyName != value)
                {
                    this.userChangedParentPropertyName = value;
                }
                else
                {
                    this.userChangedParentPropertyName = null;
                }
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.UserChangedParentPropertyName));
            }
        }

        private string userChangedParentPropertyName;
        /// <summary>
        /// Gets the changed parent property name if the user changed it, otherwise null.
        /// </summary>
        public string UserChangedParentPropertyName
        {
            get
            {
                return this.userChangedParentPropertyName;
            }
        }

        private bool ignoreParentProperty;
        /// <summary>
        /// Gets or sets whether the parent property should be included in the POCO.
        /// </summary>
        public bool IgnoreParentProperty
        {
            get
            {
                return this.ignoreParentProperty;
            }
            set
            {
                if (this.ignoreParentProperty == value)
                {
                    return;
                }

                this.ignoreParentProperty = value;
                this.OnPropertyChanged();
            }
        }

        private string generatedChildPropertyName;
        /// <summary>
        /// Gets or sets the child property name which was generated by the child property name generators.
        /// </summary>
        public string GeneratedChildPropertyName
        {
            get
            {
                return this.generatedChildPropertyName;
            }
            set
            {
                if (this.generatedChildPropertyName == value)
                {
                    return;
                }

                this.generatedChildPropertyName = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.EffectiveChildPropertyName));
            }
        }

        /// <summary>
        /// Gets or sets the effective child property name. This is the <see cref="GeneratedChildPropertyName"/> if the user didn't change it, otherwise it is the user-changed class name.
        /// </summary>
        public string EffectiveChildPropertyName
        {
            get
            {
                // If the user changed the child property name, it was saved to userChangedChildPropertyName. Otherwise it is the GeneratedChildPropertyName.
                return this.userChangedChildPropertyName ?? this.GeneratedChildPropertyName;
            }
            set
            {
                if (this.EffectiveChildPropertyName == value)
                {
                    return;
                }

                // If the user changes the child property name, save the override in userChangedChildPropertyName.
                if (this.GeneratedChildPropertyName != value)
                {
                    this.userChangedChildPropertyName = value;
                }
                else
                {
                    this.userChangedChildPropertyName = null;
                }
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.UserChangedChildPropertyName));
            }
        }

        private string userChangedChildPropertyName;
        /// <summary>
        /// Gets the changed child property name if the user changed it, otherwise null.
        /// </summary>
        public string UserChangedChildPropertyName
        {
            get
            {
                return this.userChangedChildPropertyName;
            }
        }

        private bool ignoreChildProperty;
        /// <summary>
        /// Gets or sets whether this column should be included in the POCO.
        /// </summary>
        public bool IgnoreChildProperty
        {
            get
            {
                return this.ignoreChildProperty;
            }
            set
            {
                if (this.ignoreChildProperty == value)
                {
                    return;
                }

                this.ignoreChildProperty = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the list of columns.
        /// </summary>
        public ForeignKeyColumnCollection Columns { get; private set; }

        private RelationshipType relationshipType;
        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public RelationshipType RelationshipType
        {
            get
            {
                return this.relationshipType;
            }
            set
            {
                if (this.relationshipType == value)
                {
                    return;
                }

                this.relationshipType = value;
                this.OnPropertyChanged();
            }
        }

        string IForeignKeySummary.GetDefinitionSummaryString()
        {
            return this.ParentSchema + "\r" + this.ParentTableName + "\r" +
                   this.ChildSchema + "\r" + this.ChildTableName + "\r" +
                   this.GetColumnDefinitionString();
        }

        private string GetColumnDefinitionString()
        {
            var sortedColumns = from c in this.Columns
                                orderby c.ParentTablesColumnName
                                select c.ParentTablesColumnName + "\r" + c.ChildTablesColumnName;

            return string.Join("\r", sortedColumns);
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}