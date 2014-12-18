using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PocoGen.Gui.Presentation.Converters
{
    internal class TableTypeConverter : DependencyObject, IValueConverter
    {
        public object TableImage
        {
            get { return (object)GetValue(TableImageProperty); }
            set { this.SetValue(TableImageProperty, value); }
        }

        public static readonly DependencyProperty TableImageProperty =
            DependencyProperty.Register("TableImage", typeof(object), typeof(TableTypeConverter), new PropertyMetadata(null));

        public object ViewImage
        {
            get { return (object)GetValue(ViewImageProperty); }
            set { this.SetValue(ViewImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewImageProperty =
            DependencyProperty.Register("ViewImage", typeof(object), typeof(TableTypeConverter), new PropertyMetadata(null));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? this.ViewImage : this.TableImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}