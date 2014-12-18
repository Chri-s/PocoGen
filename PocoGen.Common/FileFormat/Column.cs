using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    public class Column
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

        [XmlElement("Ignore")]
        public bool Ignore { get; set; }

        [XmlElement("PropertyName")]
        public string PropertyName { get; set; }
    }
}