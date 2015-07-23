using System.Xml.Serialization;

namespace PocoGen.Common.FileFormat
{
    internal class ForeignKeyColumn
    {
        public ForeignKeyColumn()
        {
        }
        
        public ForeignKeyColumn(string parentTablesColumnName, string childTablesColumnName)
        {
            this.ParentTablesColumnName = parentTablesColumnName;
            this.ChildTablesColumnName = childTablesColumnName;
        }

        [XmlElement("ParentTablesColumnName")]
        public string ParentTablesColumnName { get; set; }

        [XmlElement("ChildTablesColumnName")]
        public string ChildTablesColumnName { get; set; }
    }
}