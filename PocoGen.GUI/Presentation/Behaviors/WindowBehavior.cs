using System.Windows;
using System.Windows.Input;

namespace PocoGen.Gui.Presentation.Behaviors
{
    internal static class WindowBehavior
    {
        public static bool GetCloseOnEscape(DependencyObject obj)
        {
            return (bool)obj.GetValue(CloseOnEscapeProperty);
        }

        public static void SetCloseOnEscape(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseOnEscapeProperty, value);
        }

        public static readonly DependencyProperty CloseOnEscapeProperty =
            DependencyProperty.RegisterAttached("CloseOnEscape", typeof(bool), typeof(WindowBehavior), new PropertyMetadata(false, CloseOnEscapeChanged));

        private static void CloseOnEscapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if (window == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                window.KeyUp += Window_KeyUp;
            }
            else
            {
                window.KeyUp -= Window_KeyUp;
            }
        }

        private static void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((Window)sender).Close();
            }
        }
    }
}