using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PocoGen.Common
{
    [DebuggerDisplay("\\{ Schema = {Schema}, Name = {Name} \\}")]
    public sealed class KeyWithSchema : IEquatable<KeyWithSchema>
    {
        private readonly string schema;
        private readonly string name;

        public KeyWithSchema(string schema, string name)
        {
            this.schema = schema;
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyWithSchema)
            {
                return this.Equals((KeyWithSchema)obj);
            }

            return false;
        }

        public bool Equals(KeyWithSchema obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!EqualityComparer<string>.Default.Equals(this.schema, obj.schema))
            {
                return false;
            }

            if (!EqualityComparer<string>.Default.Equals(this.name, obj.name))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            hash ^= EqualityComparer<string>.Default.GetHashCode(this.schema);
            hash ^= EqualityComparer<string>.Default.GetHashCode(this.name);
            return hash;
        }

        public override string ToString()
        {
            return string.Format("{{ Schema = {0}, Name = {1} }}", this.schema, this.name);
        }

        public string Schema
        {
            get
            {
                return this.schema;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}