using System;
using System.Windows;
using System.Windows.Media;

namespace View4Logs.Base
{
    public abstract class ViewPresenter : FrameworkElement
    {
        private static readonly Size ZeroSize = new Size();

        private FrameworkElement _view;
        protected FrameworkElement View
        {
            get => _view;

            set
            {
                if (ReferenceEquals(_view, value))
                {
                    return;
                }

                var oldValue = _view;
                _view = null;

                if (oldValue != null)
                {
                    RemoveVisualChild(oldValue);
                }

                if (value != null)
                {
                    _view = value;
                    AddVisualChild(_view);
                }

                InvalidateMeasure();
            }
        }

        protected override int VisualChildrenCount => View != null ? 1 : 0;

        protected override Visual GetVisualChild(int index)
        {
            if (View == null || index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return View;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (View == null)
            {
                return ZeroSize;
            }

            View.Measure(availableSize);
            return View.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            View?.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}
