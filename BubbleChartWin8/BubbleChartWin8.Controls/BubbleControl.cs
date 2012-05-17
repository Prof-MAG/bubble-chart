using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BubbleChartWin8.Controls
{
    public class BubbleControl : Control
    {
        public static readonly DependencyProperty BubbleMarginProperty =
            DependencyProperty.Register("BubbleMargin", typeof(Thickness), typeof(BubbleControl),
                new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty LegendValueProperty =
            DependencyProperty.Register("LegendValue", typeof(object), typeof(BubbleControl),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(BubbleControl),
                new PropertyMetadata(default(double)));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(BubbleControl),new PropertyMetadata(default(double), (o, args) => ((BubbleControl)o).OnSizeChanged()));

        public static readonly DependencyProperty XValueProperty =
            DependencyProperty.Register("XValue", typeof(double), typeof(BubbleControl),
                new PropertyMetadata(default(double)));

        public static readonly DependencyProperty YValueProperty =
            DependencyProperty.Register("YValue", typeof(double), typeof(BubbleControl),
                new PropertyMetadata(default(double)));

        public BubbleControl()
        {
            DefaultStyleKey = typeof(BubbleControl);
        }

        public Thickness BubbleMargin
        {
            get { return (Thickness)GetValue(BubbleMarginProperty); }
            set { SetValue(BubbleMarginProperty, value); }
        }

        public object LegendValue
        {
            get { return GetValue(LegendValueProperty); }
            set { SetValue(LegendValueProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static double GetSize(BubbleControl control)
        {
            return (double) control.GetValue(SizeProperty);
        }

        public static void SetSize(BubbleControl control, double value)
        {
            control.SetValue(SizeProperty, value);
        }

        public double XValue
        {
            get { return (double)GetValue(XValueProperty); }
            set { SetValue(XValueProperty, value); }
        }

        public double YValue
        {
            get { return (double)GetValue(YValueProperty); }
            set { SetValue(YValueProperty, value); }
        }

        private void OnSizeChanged()
        {
            BubbleMargin = new Thickness(-Size / 2, -Size / 2, 0, 0);
        }
    }
}