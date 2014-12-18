using System.ComponentModel.Composition;
using System.Windows;
using PocoGen.Gui.Applications.Views;

namespace PocoGen.Gui.Presentation.Views
{
    [Export(typeof(ISettingsView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed partial class SettingsView : Window, ISettingsView
    {
        public SettingsView()
        {
            this.InitializeComponent();
        }
    }
}
