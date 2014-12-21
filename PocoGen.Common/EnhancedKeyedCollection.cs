using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PocoGen.Common
{
    public abstract class EnhancedKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, IChangeTracking
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

        private bool isChanged;
        public virtual bool IsChanged
        {
            get { return this.isChanged; }
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

        public virtual void AcceptChanges()
        {
            this.isChanged = false;
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            this.isChanged = true;
        }

        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);

            this.isChanged = true;
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            this.isChanged = true;
        }

        protected override void SetItem(int index, TItem item)
        {
            base.SetItem(index, item);

            this.isChanged = true;
        }
    }
}