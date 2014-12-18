using System;

namespace PocoGen.Common
{
    /// <summary>
    /// Describes a column type that is not part of the CLR. Usually just needed for database-specific types.
    /// </summary>
    public class ColumnForeignType : ColumnBaseType
    {
        public ColumnForeignType(string typeName, bool isArray)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("typeName is null or empty.", "typeName");
            }

            this.TypeName = typeName;
            this.IsArray = isArray;
        }

        /// <summary>
        /// Gets the type name.
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Gets whether the column stores an array of the type name.
        /// </summary>
        public bool IsArray { get; set; }
    }
}