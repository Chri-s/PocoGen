using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common
{
    public abstract class EnhancedKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
        where TItem : class
    {
        protected EnhancedKeyedCollection()
        {
        }

        protected EnhancedKeyedCollection(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        protected EnhancedKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
            : base(comparer, dictionaryCreationThreshold)
        {
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "items is null.");
            }

            foreach (TItem item in items)
            {
                this.Add(item);
            }
        }

        public new TItem this[TKey key]
        {
            get
            {
                return this.Contains(key) ? base[key] : default(TItem);
            }
        }
    }
}