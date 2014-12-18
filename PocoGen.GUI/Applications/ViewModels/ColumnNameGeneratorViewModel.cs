using System.ComponentModel.Composition;
using System.Reactive.Linq;
using PocoGen.Common;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ColumnNameGeneratorViewModel : ReactiveObject
    {
        public ColumnNameGeneratorViewModel()
        {
            this.Configure = ReactiveCommand.Create(this.WhenAnyValue(x => x.ColumnNameGenerator).Select(cng => cng != null && cng.Settings != null));

            this.Delete = ReactiveCommand.Create();

            this.MoveUp = ReactiveCommand.Create(this.WhenAnyValue(x => x.IsFirstItem).Select(ifi => !ifi));

            this.MoveDown = ReactiveCommand.Create(this.WhenAnyValue(x => x.IsLastItem).Select(ili => !ili));

            this.WhenAnyValue(x => x.ColumnNameGenerator)
                .Select(x => (x == null) ? string.Empty : x.Name)
                .ToProperty(this, x => x.Name, out this.name);
        }

        public ReactiveCommand<object> Configure { get; private set; }

        public ReactiveCommand<object> Delete { get; private set; }

        public ReactiveCommand<object> MoveUp { get; private set; }

        public ReactiveCommand<object> MoveDown { get; private set; }

        private ColumnNameGeneratorPlugIn columnNameGenerator;
        public ColumnNameGeneratorPlugIn ColumnNameGenerator
        {
            get { return this.columnNameGenerator; }
            set { this.RaiseAndSetIfChanged(ref this.columnNameGenerator, value); }
        }

        private ObservableAsPropertyHelper<string> name;
        public string Name
        {
            get { return this.name.Value; }
        }

        private bool isFirstItem;
        public bool IsFirstItem
        {
            get { return this.isFirstItem; }
            set { this.RaiseAndSetIfChanged(ref this.isFirstItem, value); }
        }

        private bool isLastItem;
        public bool IsLastItem
        {
            get { return this.isLastItem; }
            set { this.RaiseAndSetIfChanged(ref this.isLastItem, value); }
        }
    }
}