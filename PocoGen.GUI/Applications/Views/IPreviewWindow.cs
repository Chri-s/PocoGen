using System.Windows;
using MvvmHybridFramework;

namespace PocoGen.Gui.Applications.Views
{
    internal interface IPreviewWindow : IView
    {
        bool? ShowDialog(Window owner);
    }
}