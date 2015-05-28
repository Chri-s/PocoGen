using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(ITableNameGenerator))]
    [TableNameGenerator("Singularize", "{A563A398-AE70-4218-BD42-97E52A02685D}")]
    public class Singularize : ITableNameGenerator
    {
        public string GetClassName(Table table, ISettings settings)
        {
            string singularized = Inflector.Inflector.Singularize(table.GeneratedClassName);

            return string.IsNullOrEmpty(singularized) ? table.GeneratedClassName : singularized;
        }
    }
}