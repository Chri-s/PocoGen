using System.ComponentModel;

namespace PocoGen.Common
{
    public abstract class ChangeTrackingBase : IChangeTracking
    {
        public virtual void AcceptChanges()
        {
            this.IsChanged = false;
        }

        [Browsable(false)]
        public virtual bool IsChanged { get; private set; }

        protected virtual void SetChanged()
        {
            this.IsChanged = true;
        }

        protected void ChangeProperty<T>(ref T backingField, T value)
        {
            if (!object.Equals(backingField, value))
            {
                backingField = value;
                this.SetChanged();
            }
        }
    }
}