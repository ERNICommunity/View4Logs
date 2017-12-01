using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LogGen
{
    public partial class MainWindow : Window
    {
        private readonly NLog.ILogger _nlogLogger = NLog.LogManager.GetCurrentClassLogger();
        private readonly log4net.ILog _log4netLogger = log4net.LogManager.GetLogger(typeof(MainWindow));

        private readonly Dictionary<int, NLog.LogLevel> _nlogLevels = new Dictionary<int, NLog.LogLevel>
        {
            {0, NLog.LogLevel.Trace},
            {1, NLog.LogLevel.Debug},
            {2, NLog.LogLevel.Info},
            {3, NLog.LogLevel.Warn},
            {4, NLog.LogLevel.Error},
        };

        private readonly Dictionary<int, log4net.Core.Level> _log4netLevels = new Dictionary<int, log4net.Core.Level>
        {
            {0, log4net.Core.Level.Trace},
            {1, log4net.Core.Level.Debug},
            {2, log4net.Core.Level.Info},
            {3, log4net.Core.Level.Warn},
            {4, log4net.Core.Level.Error},
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WriteClick(object sender, RoutedEventArgs e)
        {
            var message = Message.Text;
            var levelIndex = Level.SelectedIndex;

            _nlogLogger.Log(_nlogLevels[levelIndex], message);
            _log4netLogger.Logger.Log(typeof(MainWindow), _log4netLevels[levelIndex], message, null);
        }
    }
}
