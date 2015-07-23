using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    [XmlRoot("PocoGenDefinition")]
    internal class Definition
    {
        public Definition()
        {
            this.TableNameGenerators = new PlugInCollection();
            this.ColumnNameGenerators = new PlugInCollection();
            this.OutputWriters = new OutputWriterPlugInCollection();
            this.Tables = new TableCollection();
            this.ForeignKeys = new ForeignKeyCollection();
        }

        [XmlElement("SchemaReader")]
        public PlugIn SchemaReader { get; set; }

        [XmlElement("ConnectionString")]
        public string ConnectionString { get; set; }

        [XmlElement("UseAnsiQuoting")]
        public bool UseAnsiQuoting { get; set; }

        [XmlArray("TableNameGenerators")]
        [XmlArrayItem("TableNameGenerator", typeof(PlugIn))]
        public PlugInCollection TableNameGenerators { get; private set; }

        [XmlArray("ColumnNameGenerators")]
        [XmlArrayItem("ColumnNameGenerator", typeof(PlugIn))]
        public PlugInCollection ColumnNameGenerators { get; private set; }

        [XmlArray("Tables")]
        [XmlArrayItem("Table", typeof(Table))]
        public TableCollection Tables { get; private set; }

        [XmlArray("ForeignKeys")]
        [XmlArrayItem("ForeignKey", typeof(ForeignKey))]
        public ForeignKeyCollection ForeignKeys { get; private set; }

        [XmlArray("OutputWriters")]
        [XmlArrayItem("OutputWriter", typeof(OutputWriterPlugIn))]
        public OutputWriterPlugInCollection OutputWriters { get; private set; }

        [XmlElement("OutputBasePath")]
        public string OutputBasePath { get; set; }

        public static Definition Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path is null or empty.", "path");
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Definition.Load(stream);
            }
        }

        public static Definition Load(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream is null.");
            }

            using (XmlReader reader = XmlReader.Create(stream))
            {
                return Definition.Load(reader);
            }
        }

        public static Definition Load(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader", "reader is null.");
            }

            DefinitionSerializer serializer = new DefinitionSerializer();
            return (Definition)serializer.Deserialize(reader);
        }

        public void Save(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path is null or empty.", "path");
            }

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                this.Save(stream);
            }
        }

        public void Save(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream is null.");
            }

            using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true }))
            {
                this.Save(writer);
            }
        }

        public void Save(XmlWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer", "writer is null.");
            }

            DefinitionSerializer serializer = new DefinitionSerializer();
            serializer.Serialize(writer, this);
        }
    }
}