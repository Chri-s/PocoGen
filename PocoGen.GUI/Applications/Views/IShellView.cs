using System.ComponentModel;
using MvvmHybridFramework;

namespace PocoGen.Gui.Applications.Views
{
    internal interface IShellView : IView
    {
        event CancelEventHandler Closing;

        void Show();

        void Close();
    }
}