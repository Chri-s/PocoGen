using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using MvvmHybridFramework;
using PocoGen.Gui.Applications.Views;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AboutViewModel : ReactiveViewModel<IAboutWindow>
    {
        [ImportingConstructor]
        public AboutViewModel(IAboutWindow view)
            : base(view)
        {
            this.Title = "About";
            this.Modules = new ReactiveList<AssemblyName>();
        }

        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.RaiseAndSetIfChanged(ref this.title, value); }
        }

        private string version;
        public string Version
        {
            get { return this.version; }
            set { this.RaiseAndSetIfChanged(ref this.version, value); }
        }

        public ReactiveList<AssemblyName> Modules
        {
            get;
            private set;
        }

        public void Show(Window owner)
        {
            this.ViewCore.ShowDialog(owner);
        }
    }
}