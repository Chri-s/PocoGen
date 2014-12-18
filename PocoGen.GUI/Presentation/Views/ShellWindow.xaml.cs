using System.ComponentModel.Composition;
using System.Windows;
using PocoGen.Gui.Applications.Views;

namespace PocoGen.Gui.Presentation.Views
{
    [Export(typeof(IShellView))]
    internal sealed partial class ShellWindow : Window, IShellView
    {
        public ShellWindow()
        {
            this.InitializeComponent();
        }
    }
}
