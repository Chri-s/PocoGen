using System;

namespace PocoGen.Common
{
    public interface IPlugInMetadata
    {
        string Name { get; }

        string Guid { get; }

        Type SettingsType { get; }
    }
}