using System.ComponentModel.Composition;
using System.Reactive.Linq;
using PocoGen.Common;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    internal class TableNameGeneratorConfiguationViewModel : ReactiveObject
    {
        public TableNameGeneratorConfiguationViewModel()
        {
            this.AvailableTableNameGenerators = new ReactiveList<TableNameGeneratorPlugIn>();
            this.TableNameGenerators = new ReactiveList<TableNameGeneratorViewModel>();

            this.Add = ReactiveCommand.Create(this.WhenAnyValue(x => x.SelectedTableNameGeneratorToAdd).Select(stng => stng != null));
        }

        public string Title
        {
            get { return "Table Naming"; }
        }

        public ReactiveList<TableNameGeneratorPlugIn> AvailableTableNameGenerators { get; private set; }

        public ReactiveList<TableNameGeneratorViewModel> TableNameGenerators { get; private set; }

        public ReactiveCommand<object> Add { get; private set; }

        private TableNameGeneratorPlugIn selectedTableNameGeneratorToAdd;
        public TableNameGeneratorPlugIn SelectedTableNameGeneratorToAdd
        {
            get { return this.selectedTableNameGeneratorToAdd; }
            set { this.RaiseAndSetIfChanged(ref this.selectedTableNameGeneratorToAdd, value); }
        }
    }
}