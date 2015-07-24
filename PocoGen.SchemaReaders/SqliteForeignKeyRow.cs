namespace PocoGen.SchemaReaders
{
    internal class SqliteForeignKeyRow
    {
        public SqliteForeignKeyRow(int id, int sequence, string table, string from, string to)
        {
            this.Id = id;
            this.Sequence = sequence;
            this.Table = table;
            this.From = from;
            this.To = to;
        }

        public int Id { get; set; }

        public int Sequence { get; set; }

        public string Table { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}