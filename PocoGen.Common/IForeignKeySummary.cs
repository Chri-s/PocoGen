using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGen.Common
{
    public interface IForeignKeySummary
    {
        string GetDefinitionSummaryString();
    }
}