using System;
using System.Windows;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public class LogSource : Component<LogSourceView, LogSourceViewModel>
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ILogSource), typeof(LogSource));
        public ILogSource Source
        {
            get => (ILogSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            ViewModel.Source = Source;
        }
    }
}
