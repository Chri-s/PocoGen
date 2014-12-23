using System.Collections.Generic;

namespace PocoGen.OutputWriters
{
    internal class AttribteHelper
    {
        public AttribteHelper(string typeName)
        {
            this.TypeName = typeName;
            this.PositionalProperties = new List<string>();
            this.NamedProperties = new Dictionary<string, string>();
        }

        public string TypeName { get; set; }

        public List<string> PositionalProperties { get; private set; }

        public Dictionary<string, string> NamedProperties { get; private set; }
    }
}