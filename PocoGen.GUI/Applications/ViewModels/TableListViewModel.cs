using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Data;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    internal class TableListViewModel : ReactiveObject
    {
        public TableListViewModel()
        {
            this.SelectAll = ReactiveCommand.Create();
            this.UnselectAll = ReactiveCommand.Create();
            this.Tables = new ReactiveList<TableViewModel>();

            this.ShowTables = true;
            this.ShowViews = true;

            this.TableView = new ListCollectionView(this.Tables);
            this.TableView.SortDescriptions.Add(new SortDescription("TableName", ListSortDirection.Ascending));
            this.TableView.Filter = table => this.IncludeInView((TableViewModel)table);

            this.WhenAnyValue(x => x.Filter, x => x.ShowTables, x => x.ShowViews).Subscribe(_ => this.TableView.Refresh());
        }

        public ReactiveCommand<object> SelectAll { get; private set; }

        public ReactiveCommand<object> UnselectAll { get; private set; }

        public string Title
        {
            get { return "Tables"; }
        }

        public ReactiveList<TableViewModel> Tables { get; private set; }

        public ListCollectionView TableView { get; private set; }

        private bool showTables;
        public bool ShowTables
        {
            get { return this.showTables; }
            set { this.RaiseAndSetIfChanged(ref this.showTables, value); }
        }

        private bool showViews;
        public bool ShowViews
        {
            get { return this.showViews; }
            set { this.RaiseAndSetIfChanged(ref this.showViews, value); }
        }

        private string filter;
        public string Filter
        {
            get { return this.filter; }
            set { this.RaiseAndSetIfChanged(ref this.filter, value); }
        }

        private bool IncludeInView(TableViewModel table)
        {
            if (!this.ShowTables && !table.IsView)
            {
                return false;
            }

            if (!this.ShowViews && table.IsView)
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.filter))
            {
                return true;
            }

            return table.TableName.IndexOf(this.filter, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}