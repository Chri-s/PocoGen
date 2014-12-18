using System.Windows;
using MvvmHybridFramework;

namespace PocoGen.Gui.Applications.Views
{
    internal interface ISettingsView : IView
    {
        Window Owner { get; set; }

        bool? ShowDialog();
    }
}