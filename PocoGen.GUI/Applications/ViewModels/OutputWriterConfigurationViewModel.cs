using System.ComponentModel.Composition;
using PocoGen.Gui.Domain;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    internal class OutputWriterConfigurationViewModel : ReactiveObject
    {
        public OutputWriterConfigurationViewModel()
        {
            this.AvailableOutputWriters = new ReactiveList<OutputWriterModel>();
            this.OutputWriters = new ReactiveList<OutputWriterViewModel>();

            this.Add = ReactiveCommand.Create(this.WhenAny(x => x.SelectedOutputWriterToAdd, x => x.GetValue() != null));
        }

        public string Title
        {
            get { return "Output configuration"; }
        }

        public ReactiveList<OutputWriterModel> AvailableOutputWriters { get; private set; }

        public ReactiveList<OutputWriterViewModel> OutputWriters { get; private set; }

        public ReactiveCommand<object> Add { get; private set; }

        private string basePath;
        public string BasePath
        {
            get { return this.basePath; }
            set { this.RaiseAndSetIfChanged(ref this.basePath, value); }
        }

        private OutputWriterModel selectedOutputWriterToAdd;
        public OutputWriterModel SelectedOutputWriterToAdd
        {
            get { return this.selectedOutputWriterToAdd; }
            set { this.RaiseAndSetIfChanged(ref this.selectedOutputWriterToAdd, value); }
        }
    }
}