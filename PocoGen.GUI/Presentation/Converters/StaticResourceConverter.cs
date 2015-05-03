using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PocoGen.Gui.Presentation.Converters
{
    internal class StaticResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceKey = (string)value;

            return Application.Current.FindResource(resourceKey);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}