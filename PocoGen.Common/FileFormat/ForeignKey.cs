using System;
using System.Linq;
using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    internal class ForeignKey : ChangeTrackingBase, IForeignKeySummary
    {
        public ForeignKey()
        {
        }

        public ForeignKey(string schema, string name, string parentTableSchema, string parentTable, string childTableSchema, string childTable)
            : this()
        {
            this.Schema = schema;
            this.Name = name;
            this.ParentTableSchema = parentTableSchema;
            this.ParentTable = parentTable;
            this.ChildTableSchema = childTableSchema;
            this.ChildTable = childTable;
            this.Columns = new ForeignKeyColumnCollection();
        }

        [XmlElement("ParentTableSchema")]
        public string ParentTableSchema { get; set; }

        [XmlElement("ParentTable")]
        public string ParentTable { get; set; }

        [XmlElement("ChildTableSchema")]
        public string ChildTableSchema { get; set; }

        [XmlElement("ChildTable")]
        public string ChildTable { get; set; }

        [XmlElement("Schema")]
        public string Schema { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlArray("Columns")]
        [XmlArrayItem("Column", typeof(ForeignKeyColumn))]
        public ForeignKeyColumnCollection Columns { get; private set; }

        private bool ignoreChildProperty;
        [XmlElement("IgnoreChildProperty")]
        public bool IgnoreChildProperty
        {
            get
            {
                return this.ignoreChildProperty;
            }
            set
            {
                this.ChangeProperty(ref this.ignoreChildProperty, value);
            }
        }

        private bool ignoreParentProperty;
        [XmlElement("IgnoreParentProperty")]
        public bool IgnoreParentProperty
        {
            get
            {
                return this.ignoreParentProperty;
            }
            set
            {
                this.ChangeProperty(ref this.ignoreParentProperty, value);
            }
        }

        private string childPropertyName;
        [XmlElement("ChildPropertyName")]
        public string ChildPropertyName
        {
            get
            {
                return this.childPropertyName;
            }
            set
            {
                this.ChangeProperty(ref this.childPropertyName, value);
            }
        }

        private string parentPropertyName;
        [XmlElement("ParentPropertyName")]
        public string ParentPropertyName
        {
            get
            {
                return this.parentPropertyName;
            }
            set
            {
                this.ChangeProperty(ref this.parentPropertyName, value);
            }
        }

        /// <summary>
        /// Gets whether this foreign key has the default values.
        /// </summary>
        [XmlIgnore]
        public bool HasDefaultValues
        {
            get
            {
                return ForeignKey.AreDefaultValues(this.IgnoreParentProperty, this.ParentPropertyName, this.IgnoreChildProperty, this.ChildPropertyName);
            }
        }

        public static bool AreDefaultValues(bool ignoreParentProperty, string parentPropertyName, bool ignoreChildProperty, string childPropertyName)
        {
            return !ignoreParentProperty && string.IsNullOrEmpty(parentPropertyName) && !ignoreParentProperty && string.IsNullOrEmpty(childPropertyName);
        }

        public string GetDefinitionSummaryString()
        {
            return this.ParentTableSchema + "\r" + this.ParentTable + "\r" +
                   this.ChildTableSchema + "\r" + this.ChildTable + "\r" +
                   this.GetColumnDefinitionString();
        }

        private string GetColumnDefinitionString()
        {
            var sortedColumns = from c in this.Columns
                                orderby c.ParentTablesColumnName
                                select c.ParentTablesColumnName + "\r" + c.ChildTablesColumnName;

            return string.Join("\r", sortedColumns);
        }
    }
}