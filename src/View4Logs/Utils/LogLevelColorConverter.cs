using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using View4Logs.Common.Data;

namespace View4Logs.Utils
{
    public class LogLevelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((LogLevel)value)
            {
                case LogLevel.Trace:
                    return ColorConverter.ConvertFromString("#616161");
                case LogLevel.Debug:
                    return ColorConverter.ConvertFromString("#9E9E9E");
                case LogLevel.Warn:
                    return ColorConverter.ConvertFromString("#FFD600");
                case LogLevel.Error:
                case LogLevel.Fatal:
                    return ColorConverter.ConvertFromString("#FF0000");
                default:
                    return ColorConverter.ConvertFromString("#c0c0c2");
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}