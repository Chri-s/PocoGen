using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PocoGen.Common
{
    public abstract class ChangeTrackingCollection<T> : Collection<T>, IChangeTracking
        where T : IChangeTracking
    {
        public ChangeTrackingCollection()
        {
        }

        public ChangeTrackingCollection(IList<T> list)
            : base(list)
        {
        }

        private bool isChanged;
        public bool IsChanged
        {
            get
            {
                return this.isChanged ? true : this.Any(i => i.IsChanged);
            }
            private set
            {
                this.isChanged = value;
            }
        }

        public void AcceptChanges()
        {
            this.IsChanged = false;

            foreach (T item in this)
            {
                item.AcceptChanges();
            }
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            this.IsChanged = true;
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            this.IsChanged = true;
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            this.IsChanged = true;
        }

        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);

            this.IsChanged = true;
        }
    }
}