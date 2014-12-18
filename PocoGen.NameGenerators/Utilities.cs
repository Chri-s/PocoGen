using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGen.NameGenerators
{
    internal static class Utilities
    {
        public static string ToPascalCase(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text", "text is null.");
            }

            StringBuilder pascalCasedString = new StringBuilder(text.Length);

            bool nextToUpper = true;
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    if (nextToUpper)
                    {
                        pascalCasedString.Append(char.ToUpperInvariant(c));
                        nextToUpper = false;
                    }
                    else
                    {
                        pascalCasedString.Append(c);
                    }
                }
                else if (c >= '0' && c <= '9')
                {
                    pascalCasedString.Append(c);
                    nextToUpper = true;
                }
                else
                {
                    nextToUpper = true;
                }
            }

            return pascalCasedString.ToString();
        }
    }
}