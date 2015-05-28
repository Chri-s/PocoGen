using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    internal class Column : ChangeTrackingBase
    {
        public Column()
        {
        }

        public Column(string name, string propertyName, bool ignore)
        {
            this.Name = name;
            this.PropertyName = propertyName;
            this.Ignore = ignore;
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

        private string propertyName;
        [XmlElement("PropertyName")]
        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
            set
            {
                this.ChangeProperty(ref this.propertyName, value);
            }
        }
    }
}