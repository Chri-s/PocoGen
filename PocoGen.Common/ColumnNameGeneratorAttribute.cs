using System;
using System.ComponentModel.Composition;

namespace PocoGen.Common
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ColumnNameGeneratorAttribute : ExportAttribute
    {
        public string Name { get; private set; }

        public string Guid { get; private set; }

        public Type SettingsType { get; set; }

        public ColumnNameGeneratorAttribute(string name, string guid)
        {
            this.Name = name;
            this.Guid = guid.ToUpperInvariant();
        }
    }
}