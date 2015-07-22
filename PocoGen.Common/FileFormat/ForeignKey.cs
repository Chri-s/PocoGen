using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    internal class ForeignKey : ChangeTrackingBase
    {
        [XmlElement("ParentTableSchema")]
        public string ParentTableSchema { get; set; }

        [XmlElement("ParentTable")]
        public string ParentTable { get; set; }

        [XmlElement("ChildTableSchema")]
        public string ChildTableSchema { get; set; }

        [XmlElement("ChildTable")]
        public string ChildTable { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        private bool ignoreChildProperty;
        [XmlElement("IgnoreChildProperty")]
        public bool IgnoreChildProperty
        {
            get
            {
                return this.ignoreChildProperty;
            }
            set
            {
                this.ChangeProperty(ref this.ignoreChildProperty, value);
            }
        }

        private bool ignoreParentProperty;
        [XmlElement("IgnoreParentProperty")]
        public bool IgnoreParentProperty
        {
            get
            {
                return this.ignoreParentProperty;
            }
            set
            {
                this.ChangeProperty(ref this.ignoreParentProperty, value);
            }
        }

        private string childPropertyName;
        [XmlElement("ChildPropertyName")]
        public string ChildPropertyName
        {
            get
            {
                return this.childPropertyName;
            }
            set
            {
                this.ChangeProperty(ref this.childPropertyName, value);
            }
        }

        private string parentPropertyName;
        [XmlElement("ParentPropertyName")]
        public string ParentPropertyName
        {
            get
            {
                return this.parentPropertyName;
            }
            set
            {
                this.ChangeProperty(ref this.parentPropertyName, value);
            }
        }
    }
}