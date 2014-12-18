using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using PocoGen.Gui.Applications.Views;

namespace PocoGen.Gui.Presentation.Views
{
    [Export(typeof(IAboutWindow))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal partial class AboutWindow : Window, IAboutWindow
    {
        public AboutWindow()
        {
            this.InitializeComponent();
        }

        public bool? ShowDialog(Window owner)
        {
            this.Owner = owner;
            return this.ShowDialog();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;

            Process.Start(hyperlink.NavigateUri.ToString());
            e.Handled = true;
        }
    }
}