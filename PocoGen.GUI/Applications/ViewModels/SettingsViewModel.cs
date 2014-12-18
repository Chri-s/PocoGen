using System.ComponentModel.Composition;
using System.Windows;
using MvvmHybridFramework;
using PocoGen.Gui.Applications.Views;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SettingsViewModel : ReactiveViewModel<ISettingsView>
    {
        [ImportingConstructor]
        public SettingsViewModel(ISettingsView view)
            : base(view)
        {
        }

        private object settingsObject;
        public object SettingsObject
        {
            get { return this.settingsObject; }
            set { this.RaiseAndSetIfChanged(ref this.settingsObject, value); }
        }

        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.RaiseAndSetIfChanged(ref this.title, value); }
        }

        public void ShowDialog(Window owner)
        {
            this.ViewCore.Owner = owner;
            this.ViewCore.ShowDialog();
        }
    }
}