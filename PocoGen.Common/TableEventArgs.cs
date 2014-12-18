using System;

namespace PocoGen.Common
{
    public class TableEventArgs : EventArgs
    {
        public Table Table { get; private set; }

        public TableEventArgs(Table table)
        {
            this.Table = table;
        }
    }
}