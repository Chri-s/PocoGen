using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PocoGen.OutputWriters
{
    internal class NamespaceSorter : IComparer<string>
    {
        private readonly List<string> namespaces = new List<string>();

        public NamespaceSorter()
        {
        }

        public NamespaceSorter(params string[] namespaces)
        {
            this.namespaces.AddRange((from ns in namespaces
                                      where !string.IsNullOrEmpty(ns)
                                      select ns).Distinct());
        }

        public void AddNamespace(string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace))
            {
                return;
            }

            if (!this.namespaces.Contains(@namespace))
            {
                this.namespaces.Add(@namespace);
            }
        }

        public IEnumerable<string> GetSortedNamespaces(string formatString)
        {
            this.namespaces.Sort(this);

            return from ns in this.namespaces
                   select string.Format(CultureInfo.InvariantCulture, formatString, ns);
        }

        int IComparer<string>.Compare(string x, string y)
        {
            if (x.StartsWith("System", StringComparison.Ordinal) && !y.StartsWith("System", StringComparison.Ordinal))
            {
                return -1;
            }

            if (y.StartsWith("System", StringComparison.Ordinal) && !x.StartsWith("System", StringComparison.Ordinal))
            {
                return 1;
            }

            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}