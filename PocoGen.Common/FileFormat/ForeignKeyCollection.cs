using System;

namespace PocoGen.Common.FileFormat
{
    internal class ForeignKeyCollection : EnhancedKeyedCollection<string, ForeignKey>
    {
        protected override string GetKeyForItem(ForeignKey item)
        {
            return item.GetDefinitionSummaryString();
        }

        public ForeignKey this[IForeignKeySummary foreignKey]
        {
            get
            {
                if (foreignKey == null)
                {
                    throw new ArgumentNullException(nameof(foreignKey));
                }

                return this[foreignKey.GetDefinitionSummaryString()];
            }
        }
    }
}