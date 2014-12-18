using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using MvvmHybridFramework;
using PocoGen.Common;
using PocoGen.Gui.Applications.Views;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class OutputWriterViewModel : ReactiveValidatingViewModel<IOutputWriterView>
    {
        [ImportingConstructor]
        public OutputWriterViewModel(IOutputWriterView view)
            : base(view)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Configure = ReactiveCommand.Create(this.WhenAnyValue(x => x.OutputWriter).Select(x => x != null && x.Settings != null));

            this.GeneratePreview = ReactiveCommand.Create(this.WhenAnyValue(x => x.OutputWriter).Select(x => x != null));

            this.Delete = ReactiveCommand.Create(this.WhenAnyValue(x => x.outputWriter).Select(x => x != null));

            this.WhenAnyValue(x => x.OutputWriter)
                .Select(x => (x == null) ? string.Empty : x.Name)
                .ToProperty(this, x => x.Name, out this.name);
        }

        public ReactiveCommand<object> Configure { get; private set; }

        public ReactiveCommand<object> GeneratePreview { get; private set; }

        public ReactiveCommand<object> Delete { get; private set; }

        private OutputWriterPlugIn outputWriter;
        public OutputWriterPlugIn OutputWriter
        {
            get { return this.outputWriter; }
            set { this.RaiseAndSetIfChanged(ref this.outputWriter, value); }
        }

        private string fileName;
        [Required]
        public string FileName
        {
            get { return this.fileName; }
            set { this.RaiseAndSetIfChanged(ref this.fileName, value); }
        }

        private ObservableAsPropertyHelper<string> name;
        public string Name
        {
            get { return this.name.Value; }
        }
    }
}