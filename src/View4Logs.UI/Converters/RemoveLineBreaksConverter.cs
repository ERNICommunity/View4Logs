using System;
using System.Globalization;
using System.Windows.Data;

namespace View4Logs.UI.Converters
{
    public sealed class RemoveLineBreaksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.Replace('\n', ' ');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}