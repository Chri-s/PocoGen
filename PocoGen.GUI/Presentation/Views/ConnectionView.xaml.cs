using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;
using PocoGen.Gui.Applications.Views;

namespace PocoGen.Gui.Presentation.Views
{
    [Export(typeof(IConnectionView))]
    internal sealed partial class ConnectionView : UserControl, IConnectionView
    {
        public ConnectionView()
        {
            this.InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }
    }
}