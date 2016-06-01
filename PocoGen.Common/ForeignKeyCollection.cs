using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common
{
    public class ForeignKeyCollection : EnhancedKeyedCollection<string, ForeignKey>
    {
        protected override string GetKeyForItem(ForeignKey item)
        {
            return ((IForeignKeySummary)item).GetDefinitionSummaryString();
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