using System.ComponentModel.Composition;
using System.Reactive.Linq;
using PocoGen.Common;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TableNameGeneratorViewModel : ReactiveObject
    {
        public TableNameGeneratorViewModel()
        {
            this.Configure = ReactiveCommand.Create(this.WhenAnyValue(x => x.TableNameGenerator).Select(tng => tng != null && tng.Settings != null));

            this.Delete = ReactiveCommand.Create();

            this.MoveUp = ReactiveCommand.Create(this.WhenAnyValue(x => x.IsFirstItem).Select(ifi => !ifi));

            this.MoveDown = ReactiveCommand.Create(this.WhenAnyValue(x => x.IsLastItem).Select(ili => !ili));

            this.WhenAnyValue(x => x.TableNameGenerator)
                .Select(x => (x == null) ? string.Empty : x.Name)
                .ToProperty(this, x => x.Name, out this.name);
        }

        public ReactiveCommand<object> Configure { get; private set; }

        public ReactiveCommand<object> Delete { get; private set; }

        public ReactiveCommand<object> MoveUp { get; private set; }

        public ReactiveCommand<object> MoveDown { get; private set; }

        private TableNameGeneratorPlugIn tableNameGenerator;
        public TableNameGeneratorPlugIn TableNameGenerator
        {
            get { return this.tableNameGenerator; }
            set { this.RaiseAndSetIfChanged(ref this.tableNameGenerator, value); }
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