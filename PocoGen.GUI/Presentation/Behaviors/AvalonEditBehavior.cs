using System.Windows;
using ICSharpCode.AvalonEdit;

namespace PocoGen.Gui.Presentation.Behaviors
{
    internal static class AvalonEditBehavior
    {
        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(AvalonEditBehavior), new PropertyMetadata(null, TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextEditor editor = d as TextEditor;
            if (editor == null)
            {
                return;
            }

            editor.Text = (string)e.NewValue;
        }
    }
}