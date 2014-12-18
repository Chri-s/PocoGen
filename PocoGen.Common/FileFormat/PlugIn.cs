using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    public class PlugIn
    {
        public PlugIn()
        {
        }

        public PlugIn(string guid, string name, SettingsRepository configuration)
        {
            this.Guid = guid;
            this.Name = name;
            this.Configuration = configuration;
        }

        [XmlElement("Guid")]
        public string Guid { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Configuration")]
        public SettingsRepository Configuration { get; set; }
    }
}