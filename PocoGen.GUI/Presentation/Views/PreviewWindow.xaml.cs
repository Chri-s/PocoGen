using System.ComponentModel.Composition;
using System.Windows;
using PocoGen.Gui.Applications.Views;

namespace PocoGen.Gui.Presentation.Views
{
    [Export(typeof(IPreviewWindow))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal partial class PreviewWindow : Window, IPreviewWindow
    {
        public PreviewWindow()
        {
            this.InitializeComponent();
        }

        public bool? ShowDialog(Window owner)
        {
            this.Owner = owner;
            return this.ShowDialog();
        }
    }
}