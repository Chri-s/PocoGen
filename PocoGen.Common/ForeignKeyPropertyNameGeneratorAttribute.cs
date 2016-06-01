using System;
using System.ComponentModel.Composition;

namespace PocoGen.Common
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ForeignKeyPropertyNameGeneratorAttribute : ExportAttribute
    {
        public string Guid { get; private set; }

        public string Name { get; private set; }

        public Type SettingsType { get; set; }

        public bool CanChangeParentPropertyName { get; set; }

        public bool CanChangeChildPropertyName { get; set; }

        public ForeignKeyPropertyNameGeneratorAttribute(string name, string guid)
        {
            this.Name = name;
            this.Guid = guid.ToUpperInvariant();
        }
    }
}