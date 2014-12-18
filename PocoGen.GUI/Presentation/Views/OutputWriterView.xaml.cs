using System.ComponentModel.Composition;
using System.Windows.Controls;
using PocoGen.Gui.Applications.Views;

namespace PocoGen.Gui.Presentation.Views
{
    [Export(typeof(IOutputWriterView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed partial class OutputWriterView : UserControl, IOutputWriterView
    {
        public OutputWriterView()
        {
            this.InitializeComponent();
        }
    }
}