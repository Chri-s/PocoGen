using System.ComponentModel.Composition;
using System.Reactive.Linq;
using PocoGen.Common;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    internal class ColumnNameGeneratorConfigurationViewModel : ReactiveObject
    {
        public ColumnNameGeneratorConfigurationViewModel()
        {
            this.AvailableColumnNameGenerators = new ReactiveList<ColumnNameGeneratorPlugIn>();
            this.ColumnNameGenerators = new ReactiveList<ColumnNameGeneratorViewModel>();

            this.Add = ReactiveCommand.Create(this.WhenAnyValue(x => x.SelectedColumnNameGeneratorToAdd).Select(stng => stng != null));
        }

        public string Title
        {
            get { return "Column Naming"; }
        }

        public ReactiveList<ColumnNameGeneratorPlugIn> AvailableColumnNameGenerators { get; private set; }

        public ReactiveList<ColumnNameGeneratorViewModel> ColumnNameGenerators { get; private set; }

        public ReactiveCommand<object> Add { get; private set; }

        private ColumnNameGeneratorPlugIn selectedColumnNameGeneratorToAdd;
        public ColumnNameGeneratorPlugIn SelectedColumnNameGeneratorToAdd
        {
            get { return this.selectedColumnNameGeneratorToAdd; }
            set { this.RaiseAndSetIfChanged(ref this.selectedColumnNameGeneratorToAdd, value); }
        }
    }
}