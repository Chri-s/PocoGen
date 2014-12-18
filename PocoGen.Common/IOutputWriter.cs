using System.IO;

namespace PocoGen.Common
{
    public interface IOutputWriter
    {
        void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, ISettings settings, OutputInformation outputInformation);
    }
}