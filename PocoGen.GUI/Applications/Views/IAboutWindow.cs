using System.Windows;
using MvvmHybridFramework;

namespace PocoGen.Gui.Applications.Views
{
    internal interface IAboutWindow : IView
    {
        bool? ShowDialog(Window owner);
    }
}