using System;
using System.ComponentModel;

namespace PocoGen.Common
{
    public abstract class PlugIn<TPlugIn, TPlugInMetadata> : IChangeTracking
        where TPlugIn : class
        where TPlugInMetadata : class, IPlugInMetadata
    {
        private readonly Lazy<TPlugIn, TPlugInMetadata> lazyInstance;

        protected PlugIn(Lazy<TPlugIn, TPlugInMetadata> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "instance is null.");
            }

            this.lazyInstance = instance;
            this.Settings = (instance.Metadata.SettingsType == null) ? null : (ISettings)Activator.CreateInstance(instance.Metadata.SettingsType);
        }

        public TPlugInMetadata Metadata
        {
            get { return this.LazyInstance.Metadata; }
        }

        public string Guid
        {
            get { return this.LazyInstance.Metadata.Guid; }
        }

        public string Name
        {
            get { return this.LazyInstance.Metadata.Name; }
        }

        public bool IsChanged
        {
            get { return (this.Settings == null) ? false : this.Settings.IsChanged; }
        }

        public ISettings Settings { get; private set; }

        protected TPlugIn Instance
        {
            get { return this.LazyInstance.Value; }
        }

        protected Lazy<TPlugIn, TPlugInMetadata> LazyInstance
        {
            get { return this.lazyInstance; }
        }

        public void AcceptChanges()
        {
            if (this.Settings != null)
            {
                this.Settings.AcceptChanges();
            }
        }

        public void ResetSettingsToDefaults()
        {
            if (this.Settings != null)
            {
                this.Settings.ResetToDefaults();
            }
        }
    }
}