using System;
using System.Globalization;
using System.Windows.Data;

namespace View4Logs.UI.Converters
{
    public sealed class MultiEqualsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 1)
            {
                var first = values[0];
                for (var i = 1; i < values.Length; i++)
                {
                    if (values[i] != first)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
