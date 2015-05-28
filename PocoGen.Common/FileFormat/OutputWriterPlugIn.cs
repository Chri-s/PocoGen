using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    internal class OutputWriterPlugIn : PlugIn
    {
        public OutputWriterPlugIn()
            : base()
        {
        }

        public OutputWriterPlugIn(string guid, string name, string fileName, SettingsRepository configuration)
            : base(guid, name, configuration)
        {
            this.FileName = fileName;
        }

        [XmlElement("FileName")]
        public string FileName { get; set; }
    }
}