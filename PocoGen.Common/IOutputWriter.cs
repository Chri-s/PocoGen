using System.IO;

namespace PocoGen.Common
{
    /// <summary>
    /// Generates the code for a database schema.
    /// </summary>
    public interface IOutputWriter
    {
        /// <summary>
        /// Writes the code for the database schema to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write the code to.</param>
        /// <param name="tables">The collection of tables and views in the database schema.</param>
        /// <param name="dbEscaper">The escaper that should be used if database object names need to be escaped.</param>
        /// <param name="settings">The settings for this module if it specifies a settings type, otherwise null.</param>
        /// <param name="outputInformation">An object to gather generation information.</param>
        void Write(TextWriter stream, TableCollection tables, IDBEscaper dbEscaper, ISettings settings, OutputInformation outputInformation);
    }
}