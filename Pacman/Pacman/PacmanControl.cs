using System.Windows;
using System.Windows.Controls;

namespace Pacman
{
    public class PacmanControl : Control
    {
        public static readonly DependencyProperty MouseAngleProperty =
            DependencyProperty.Register("MouseAngle", typeof(int), typeof(PacmanControl),
                new PropertyMetadata(default(int)));


        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(PacmanControl),
                new PropertyMetadata(default(double)));

        public int MouseAngle
        {
            get { return (int)GetValue(MouseAngleProperty); }
            set { SetValue(MouseAngleProperty, value); }
        }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
    }
}