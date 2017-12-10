using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using View4Logs.Common.Data;

namespace View4Logs.UI.Converters
{
    public class LogLevelBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((LogLevel)value)
            {
                case LogLevel.All:
                    return Application.Current.FindResource(Theme.Brush.LogLevelAll);
                case LogLevel.Trace:
                    return Application.Current.FindResource(Theme.Brush.LogLevelTrace);
                case LogLevel.Debug:
                    return Application.Current.FindResource(Theme.Brush.LogLevelDebug);
                case LogLevel.Info:
                    return Application.Current.FindResource(Theme.Brush.LogLevelInfo);
                case LogLevel.Warn:
                    return Application.Current.FindResource(Theme.Brush.LogLevelWarn);
                case LogLevel.Error:
                    return Application.Current.FindResource(Theme.Brush.LogLevelError);
                case LogLevel.Fatal:
                    return Application.Current.FindResource(Theme.Brush.LogLevelFatal);
                case LogLevel.Off:
                    return Application.Current.FindResource(Theme.Brush.LogLevelOff);
                default:
                    throw new InvalidOperationException();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}