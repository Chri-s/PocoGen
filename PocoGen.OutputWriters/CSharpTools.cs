using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PocoGen.Common;

namespace PocoGen.OutputWriters
{
    internal static class CSharpTools
    {
        private static readonly Lazy<Regex> CleanUpRegex = new Lazy<Regex>(() => new Regex(@"[^\w\d_]", RegexOptions.Compiled | RegexOptions.CultureInvariant));
        private static readonly Lazy<Regex> SystemTypeRegex = new Lazy<Regex>(() => new Regex(@"^System\.[\w\d_]+$", RegexOptions.Compiled));
        private static readonly Lazy<Dictionary<Type, string>> BaseTypeNames = new Lazy<Dictionary<Type, string>>(() => CSharpTools.GetBaseTypeDictionary());
        private static readonly string[] ReservedKeywords = new string[] { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "in (generic modifier)", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "out (generic modifier)", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while" };

        public static string SafeString(string text)
        {
            return text.Replace("\"", "\\\"").Replace("\t", "\\\t").Replace("\r", "\\\r").Replace("\n", "\\\n");
        }

        public static string SafePropertyName(string propertyName)
        {
            string safeName = CleanUpRegex.Value.Replace(propertyName, "_");
            if (safeName.Length > 0 && safeName[0] >= '0' && safeName[0] <= '9')
            {
                safeName = "_" + safeName;
            }

            if (ReservedKeywords.Contains(safeName))
            {
                safeName = "@" + safeName;
            }

            return safeName;
        }

        public static string GetAttributeString(AttribteHelper attribute)
        {
            StringBuilder builder = new StringBuilder("[");
            builder.Append(CSharpTools.SafeClassName(attribute.TypeName));

            if (attribute.NamedProperties.Any() || attribute.PositionalProperties.Any())
            {
                builder.Append("(");

                bool isFirst = true;
                foreach (string positionalProperty in attribute.PositionalProperties)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        builder.Append(", ");
                    }

                    builder.Append(positionalProperty);
                }

                foreach (KeyValuePair<string, string> namedProperty in attribute.NamedProperties)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        builder.Append(", ");
                    }

                    builder.Append(CSharpTools.SafePropertyName(namedProperty.Key));
                    builder.Append(" = ");
                    builder.Append(namedProperty.Value);
                }

                builder.Append(")");
            }

            builder.Append("]");

            return builder.ToString();
        }

        public static string SafeClassAndNamespaceName(string fullName)
        {
            return string.Join(".", from s in fullName.Split('.')
                                    select CSharpTools.SafeClassName(s));
        }

        public static string SafeClassName(string className)
        {
            string safeName = CleanUpRegex.Value.Replace(className, "_");
            if (safeName.Length > 0 && safeName[0] >= '0' && safeName[0] <= '9')
            {
                safeName = "_" + safeName;
            }

            if (ReservedKeywords.Contains(safeName))
            {
                safeName = "@" + safeName;
            }

            return safeName;
        }

        public static string GetColumnType(ColumnBaseType type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type is null.");
            }

            ColumnForeignType foreignType = type as ColumnForeignType;
            if (foreignType != null)
            {
                string result = foreignType.TypeName;

                if (foreignType.IsArray)
                {
                    result += "[]";
                }

                return result;
            }

            ColumnType columnType = type as ColumnType;
            if (columnType != null)
            {
                Type realColumnType = columnType.Type;

                if (realColumnType.IsArray)
                {
                    return GetBaseTypeName(realColumnType.GetElementType()) + "[]";
                }
                else if (realColumnType.IsGenericType && realColumnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return GetBaseTypeName(realColumnType.GetGenericArguments().Single()) + "?";
                }
                else
                {
                    return GetBaseTypeName(realColumnType);
                }
            }
            else
            {
                throw new ArgumentException("Unknown column type.");
            }
        }

        private static string GetBaseTypeName(Type type)
        {
            if (CSharpTools.BaseTypeNames.Value.ContainsKey(type))
            {
                return CSharpTools.BaseTypeNames.Value[type];
            }

            // Remove "System."
            string baseTypeName = type.FullName;
            if (CSharpTools.SystemTypeRegex.Value.IsMatch(baseTypeName))
            {
                return baseTypeName.Substring(7);
            }

            return baseTypeName;
        }

        private static Dictionary<Type, string> GetBaseTypeDictionary()
        {
            Dictionary<Type, string> baseTypeDictionary = new Dictionary<Type, string>();
            baseTypeDictionary.Add(typeof(bool), "bool");
            baseTypeDictionary.Add(typeof(byte), "byte");
            baseTypeDictionary.Add(typeof(char), "char");
            baseTypeDictionary.Add(typeof(decimal), "decimal");
            baseTypeDictionary.Add(typeof(double), "double");
            baseTypeDictionary.Add(typeof(float), "float");
            baseTypeDictionary.Add(typeof(int), "int");
            baseTypeDictionary.Add(typeof(long), "long");
            baseTypeDictionary.Add(typeof(object), "object");
            baseTypeDictionary.Add(typeof(sbyte), "sbyte");
            baseTypeDictionary.Add(typeof(short), "short");
            baseTypeDictionary.Add(typeof(string), "string");
            baseTypeDictionary.Add(typeof(uint), "uint");
            baseTypeDictionary.Add(typeof(ulong), "ulong");
            baseTypeDictionary.Add(typeof(ushort), "ushort");

            return baseTypeDictionary;
        }
    }
}