using System.Windows;
using System.Windows.Controls;

namespace BubbleChart.Controls
{
    public class BubbleControl : Control
    {
        public static readonly DependencyProperty LegendValueProperty =
            DependencyProperty.Register("LegendValue", typeof(object), typeof(BubbleControl),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(BubbleControl),
                new PropertyMetadata(default(double)));

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

        public object LegendValue
        {
            get { return (object)GetValue(LegendValueProperty); }
            set { SetValue(LegendValueProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
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
    }
}