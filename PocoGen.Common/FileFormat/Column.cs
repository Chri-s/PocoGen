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

        [XmlElement("Name")]
        public string Name { get; set; }

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

        [XmlIgnore]
        public bool HasDefaultValues
        {
            get
            {
                return Column.AreDefaultValues(this.Ignore, this.PropertyName);
            }
        }

        public static bool AreDefaultValues(bool ignore, string propertyName)
        {
            return !ignore && string.IsNullOrEmpty(propertyName);
        }
    }
}