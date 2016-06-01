using System;

namespace PocoGen.Common
{
    public class ForeignKeyPropertyNameGeneratorPlugIn : PlugIn<IForeignKeyPropertyNameGenerator, IForeignKeyPropertyNameGeneratorMetadata>
    {
        public ForeignKeyPropertyNameGeneratorPlugIn(Lazy<IForeignKeyPropertyNameGenerator, IForeignKeyPropertyNameGeneratorMetadata> instance)
            : base(instance)
        {
        }

        public string GetParentPropertyName(ForeignKey foreignKey)
        {
            return this.Instance.GetParentPropertyName(foreignKey, this.Settings);
        }

        public string GetChildPropertyName(ForeignKey foreignKey)
        {
            return this.Instance.GetChildPropertyName(foreignKey, this.Settings);
        }

        public ForeignKeyPropertyNameGeneratorPlugIn Clone()
        {
            return new ForeignKeyPropertyNameGeneratorPlugIn(this.LazyInstance);
        }
    }
}