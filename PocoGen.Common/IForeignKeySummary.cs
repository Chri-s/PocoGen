using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGen.Common
{
    internal interface IForeignKeySummary
    {
        string GetDefinitionSummaryString();
    }
}