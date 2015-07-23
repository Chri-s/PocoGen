using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    internal class Table : ChangeTrackingBase
    {
        public Table()
        {
            this.Columns = new ColumnCollection();
        }

        public Table(string schema, string name, string className, bool ignore)
            : this()
        {
            this.Schema = schema;
            this.Name = name;
            this.className = className;
            this.ignore = ignore;
        }

        // Set modifier is internal so that the DefinitionSerializer can still set this property.
        [XmlElement("Schema")]
        public string Schema { get; internal set; }

        // Set modifier is internal so that the DefinitionSerializer can still set this property.
        [XmlElement("Name")]
        public string Name { get; internal set; }

        private bool ignore;
        [XmlElement("Ignore")]
        public bool Ignore
        {
            get
            {
                return this.ignore;
            }
            set
            {
                this.ChangeProperty(ref this.ignore, value);
            }
        }

        // Unfortunatly an older version named this property "PropertyName" instead of "ClassName", thus the XmlElement has another name than the property.
        // If we changed this, the older files would not be deserialized correctly :(
        private string className;
        [XmlElement("PropertyName")]
        public string ClassName
        {
            get
            {
                return this.className;
            }
            set
            {
                this.ChangeProperty(ref this.className, value);
            }
        }

        /// <summary>
        /// Gets whether this table has the default values and contains no changed columns.
        /// </summary>
        [XmlIgnore]
        public bool HasDefaultValues
        {
            get
            {
                return Table.AreDefaultValues(this.Ignore, this.ClassName) && this.Columns.Count == 0;
            }
        }

        public static bool AreDefaultValues(bool ignore, string className)
        {
            return !ignore && string.IsNullOrEmpty(className);
        }

        [XmlArray("Columns")]
        [XmlArrayItem("Column", typeof(Column))]
        public ColumnCollection Columns { get; private set; }
    }
}