using System;

namespace PocoGen.Common
{
    /// <summary>
    /// Represents a column type which is part of the CLR. Used for most column types.
    /// </summary>
    public class ColumnType : ColumnBaseType
    {
        public ColumnType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type is null.");
            }

            this.Type = type;
        }

        public Type Type { get; private set; }
    }
}