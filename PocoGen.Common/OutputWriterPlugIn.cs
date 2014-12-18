using System;
using System.IO;

namespace PocoGen.Common
{
    public class OutputWriterPlugIn : PlugIn<IOutputWriter, IOutputWriterMetadata>
    {
        public OutputWriterPlugIn(Lazy<IOutputWriter, IOutputWriterMetadata> instance)
            : base(instance)
        {
            this.FileName = string.Empty;
        }

        public OutputWriterPlugIn(Lazy<IOutputWriter, IOutputWriterMetadata> instance, string fileName)
            : base(instance)
        {
            this.FileName = fileName;
        }

        public string FileName { get; set; }

        public void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, OutputInformation outputInformation)
        {
            // If the caller is not interested in the output information, create a new object.
            // This way, the plug in does not have to check for null.
            if (outputInformation == null)
            {
                outputInformation = new OutputInformation();
            }

            this.Instance.Write(stream, tables, dbEscaper, this.Settings, outputInformation);
        }

        public OutputWriterPlugIn Clone()
        {
            return new OutputWriterPlugIn(this.LazyInstance, this.FileName);
        }
    }
}