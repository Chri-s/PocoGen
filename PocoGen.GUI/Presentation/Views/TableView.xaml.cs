using System.Windows;
using System.Windows.Controls;

namespace PocoGen.Gui.Presentation.Views
{
    internal sealed partial class TableView : UserControl
    {
        public TableView()
        {
            this.InitializeComponent();
        }

        private void ContentPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter rootElement = sender as ContentPresenter;

            ContentPresenter contentPresenter = rootElement.TemplatedParent as ContentPresenter;
            contentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
    }
}