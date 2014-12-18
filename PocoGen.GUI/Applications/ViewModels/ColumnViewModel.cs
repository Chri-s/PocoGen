using System.ComponentModel.Composition;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ColumnViewModel : ReactiveObject
    {
        public ColumnViewModel()
        {
        }

        private string tableName;
        public string TableName
        {
            get { return this.tableName; }
            set { this.RaiseAndSetIfChanged(ref this.tableName, value); }
        }

        private bool included;
        public bool Included
        {
            get { return this.included; }
            set { this.RaiseAndSetIfChanged(ref this.included, value); }
        }

        private string columnName;
        public string ColumnName
        {
            get { return this.columnName; }
            set { this.RaiseAndSetIfChanged(ref this.columnName, value); }
        }

        private string propertyName;
        public string PropertyName
        {
            get { return this.propertyName; }
            set { this.RaiseAndSetIfChanged(ref this.propertyName, value); }
        }

        private bool isPrimaryKey;
        public bool IsPrimaryKey
        {
            get { return this.isPrimaryKey; }
            set { this.RaiseAndSetIfChanged(ref this.isPrimaryKey, value); }
        }
    }
}