using System;
using System.Globalization;
using System.Windows.Data;

namespace View4Logs.UI.Converters
{
    public class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value, CultureInfo.InvariantCulture) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value, CultureInfo.InvariantCulture) / System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
        }
    }
}
