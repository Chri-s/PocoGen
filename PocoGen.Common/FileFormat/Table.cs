using System.Collections.Generic;
using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    internal class Table : ChangeTrackingBase
    {
        public Table()
        {
            this.Columns = new ColumnCollection();
        }

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

        [XmlArray("Columns")]
        [XmlArrayItem("Column", typeof(Column))]
        public ColumnCollection Columns { get; private set; }
    }
}