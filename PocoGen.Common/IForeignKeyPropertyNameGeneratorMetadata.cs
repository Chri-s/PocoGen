namespace PocoGen.Common
{
    public interface IForeignKeyPropertyNameGeneratorMetadata : IPlugInMetadata
    {
        bool CanChangeParentPropertyName { get; set; }

        bool CanChangeChildPropertyName { get; set; }
    }
}