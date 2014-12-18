using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PocoGen.Common;

namespace PocoGen.OutputWriters
{
    internal static class VisualBasicTools
    {
        private static readonly Lazy<Regex> CleanUpRegex = new Lazy<Regex>(() => new Regex(@"[^\w\d_]", RegexOptions.Compiled | RegexOptions.CultureInvariant));
        private static readonly Lazy<Regex> SystemTypeRegex = new Lazy<Regex>(() => new Regex(@"^System\.[\w\d_]+$", RegexOptions.Compiled));
        private static readonly Lazy<Dictionary<Type, string>> BaseTypeNames = new Lazy<Dictionary<Type, string>>(() => VisualBasicTools.GetBaseTypeDictionary());
        private static readonly string[] ReservedKeywords = new string[] { "AddHandler", "AddressOf", "Alias", "And", "AndAlso", "As", "Boolean", "ByRef", "Byte", "ByVal", "Call", "Case", "Catch", "CBool", "CByte", "CChar", "CDate", "CDec", "CDbl", "Char", "CInt", "Class", "CLng", "CObj", "Const", "Continue", "CSByte", "CShort", "CSng", "CStr", "CType", "CUInt", "CULng", "CUShort", "Date", "Decimal", "Declare", "Default", "Delegate", "Dim", "DirectCast", "Do", "Double", "Each", "Else", "ElseIf", "End", "EndIf", "Enum", "Erase", "Error", "Event", "Exit", "False", "Finally", "For", "Friend", "Function", "Get", "GetType", "GetXMLNamespace", "Global", "GoSub", "GoTo", "Handles", "If", "If()", "Implements", "Imports (.NET Namespace and Type)", "Imports (XML Namespace)", "In", "Inherits", "Integer", "Interface", "Is", "IsNot", "Let", "Lib", "Like", "Long", "Loop", "Me", "Mod", "Module", "MustInherit", "MustOverride", "MyBase", "MyClass", "Namespace", "Narrowing", "New", "Next", "Not", "Nothing", "NotInheritable", "NotOverridable", "Object", "Of", "On", "Operator", "Option", "Optional", "Or", "OrElse", "Overloads", "Overridable", "Overrides", "ParamArray", "Partial", "Private", "Property", "Protected", "Public", "RaiseEvent", "ReadOnly", "ReDim", "REM", "RemoveHandler", "Resume", "Return", "SByte", "Select", "Set", "Shadows", "Shared", "Short", "Single", "Static", "Step", "Stop", "String", "Structure", "Sub", "SyncLock", "Then", "Throw", "To", "True", "Try", "TryCast", "TypeOf", "Variant", "Wend", "UInteger", "ULong", "UShort", "Using", "When", "While", "Widening", "With", "WithEvents", "WriteOnly", "Xor" };

        public static string SafeString(string text)
        {
            VisualBasicStringBuilder stringBuilder = new VisualBasicStringBuilder();

            foreach (char item in text)
            {
                if (item == '\"')
                {
                    stringBuilder.AppendLiteral("\"\"");
                }
                else if (item == '\t')
                {
                    stringBuilder.AppendConstant("vbTab");
                }
                else if (item == '\r')
                {
                    stringBuilder.AppendConstant("vbCr");
                }
                else if (item == '\n')
                {
                    stringBuilder.AppendConstant("vbLf");
                }
                else
                {
                    stringBuilder.AppendLiteral(item);
                }
            }

            stringBuilder.Close();

            return stringBuilder.ToString();
        }

        public static string SafePropertyName(string propertyName)
        {
            string safeName = CleanUpRegex.Value.Replace(propertyName, "_");
            if (safeName.Length > 0 && safeName[0] >= '0' && safeName[0] <= '9')
            {
                safeName = "_" + safeName;
            }

            if (ReservedKeywords.Any(rk => string.Compare(rk, safeName, StringComparison.OrdinalIgnoreCase) == 0))
            {
                safeName = "[" + safeName + "]";
            }

            return safeName;
        }

        public static string SafeClassName(string className)
        {
            string safeName = CleanUpRegex.Value.Replace(className, "_");
            if (safeName.Length > 0 && safeName[0] >= '0' && safeName[0] <= '9')
            {
                safeName = "_" + safeName;
            }

            if (ReservedKeywords.Any(rk => string.Compare(rk, safeName, StringComparison.OrdinalIgnoreCase) == 0))
            {
                safeName = "[" + safeName + "]";
            }

            return safeName;
        }

        public static string GetColumnType(PocoGen.Common.ColumnBaseType type)
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
                    result += "()";
                }
            }

            ColumnType columnType = type as ColumnType;
            if (columnType != null)
            {
                Type realColumnType = columnType.Type;

                if (realColumnType.IsArray)
                {
                    return GetBaseTypeName(realColumnType.GetElementType()) + "()";
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
            if (VisualBasicTools.BaseTypeNames.Value.ContainsKey(type))
            {
                return VisualBasicTools.BaseTypeNames.Value[type];
            }

            // Remove "System."
            string baseTypeName = type.FullName;
            if (VisualBasicTools.SystemTypeRegex.Value.IsMatch(baseTypeName))
            {
                return baseTypeName.Substring(7);
            }

            return baseTypeName;
        }

        private static Dictionary<Type, string> GetBaseTypeDictionary()
        {
            Dictionary<Type, string> baseTypeDictionary = new Dictionary<Type, string>();
            baseTypeDictionary.Add(typeof(bool), "Boolena");
            baseTypeDictionary.Add(typeof(byte), "Byte");
            baseTypeDictionary.Add(typeof(char), "Char");
            baseTypeDictionary.Add(typeof(decimal), "Decimal");
            baseTypeDictionary.Add(typeof(double), "Double");
            baseTypeDictionary.Add(typeof(float), "Single");
            baseTypeDictionary.Add(typeof(int), "Integer");
            baseTypeDictionary.Add(typeof(long), "Long");
            baseTypeDictionary.Add(typeof(object), "Object");
            baseTypeDictionary.Add(typeof(sbyte), "SByte");
            baseTypeDictionary.Add(typeof(short), "Short");
            baseTypeDictionary.Add(typeof(string), "String");
            baseTypeDictionary.Add(typeof(uint), "UInt32");
            baseTypeDictionary.Add(typeof(ulong), "UInt64");
            baseTypeDictionary.Add(typeof(ushort), "UInt16");

            return baseTypeDictionary;
        }
    }
}