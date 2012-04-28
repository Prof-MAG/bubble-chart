using System.Windows;
using System.Windows.Controls;

namespace Pacman
{
    public class PacmanControl : Control
    {
        public static readonly DependencyProperty MouseAngleProperty =
            DependencyProperty.Register("MouseAngle", typeof(double), typeof(PacmanControl),
                new PropertyMetadata(default(double)));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(PacmanControl),
                new PropertyMetadata(default(double)));

        public PacmanControl()
        {
            Loaded += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
            MouseEnter += (sender, args) => VisualStateManager.GoToState(this, "MouseOver", true);
            MouseLeave += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
        }

        public double MouseAngle
        {
            get { return (double)GetValue(MouseAngleProperty); }
            set { SetValue(MouseAngleProperty, value); }
        }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
    }
}