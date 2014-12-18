namespace PocoGen.Common
{
    public interface ISchemaReaderMetadata : IPlugInMetadata
    {
        string ConnectionStringDocumentationUrl { get; }
    }
}