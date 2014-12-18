using System.ComponentModel.Composition;
using MvvmHybridFramework;
using PocoGen.Gui.Applications.Views;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PreviewViewModel : ReactiveViewModel<IPreviewWindow>
    {
        private readonly ShellViewModel shellViewModel;

        [ImportingConstructor]
        public PreviewViewModel(IPreviewWindow view, ShellViewModel shellViewModel)
            : base(view)
        {
            this.shellViewModel = shellViewModel;
        }

        public string Title
        {
            get
            {
                return "Preview";
            }
        }

        private string text;
        public string Text
        {
            get { return this.text; }
            set { this.RaiseAndSetIfChanged(ref this.text, value); }
        }

        private string syntaxHighlightingLanguage;
        public string SyntaxHighlightingLanguage
        {
            get { return this.syntaxHighlightingLanguage; }
            set { this.RaiseAndSetIfChanged(ref this.syntaxHighlightingLanguage, value); }
        }

        public void ShowDialog()
        {
            this.ViewCore.ShowDialog(this.shellViewModel.Window);
        }
    }
}