using MvvmHybridFramework;

namespace PocoGen.Gui.Applications.Views
{
    internal interface IShellView : IView
    {
        void Show();

        void Close();
    }
}