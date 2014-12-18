using System.Collections.Generic;
using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    public class Table
    {
        public Table()
        {
            this.Columns = new ColumnCollection();
        }

        public Table(string name, string propertyName, bool ignore)
            : this()
        {
            this.Name = name;
            this.PropertyName = propertyName;
            this.Ignore = ignore;
        }

        public Table(string name, string propertyName, bool ignore, IEnumerable<Column> columns)
            : this(name, propertyName, ignore)
        {
            this.Columns.AddRange(columns);
        }

        // Set modifier is internal so that the DefinitionSerializer can still set this property.
        [XmlElement("Name")]
        public string Name { get; internal set; }

        [XmlElement("Ignore")]
        public bool Ignore { get; set; }

        [XmlElement("PropertyName")]
        public string PropertyName { get; set; }

        [XmlArray("Columns")]
        [XmlArrayItem("Column", typeof(Column))]
        public ColumnCollection Columns { get; private set; }
    }
}