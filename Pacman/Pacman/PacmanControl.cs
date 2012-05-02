using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pacman
{
    [TemplatePart(Name = TopRotator, Type = typeof(RotateTransform))]
    [TemplatePart(Name = BotRotator, Type = typeof(RotateTransform))]
    public class PacmanControl : Control
    {
        private const string TopRotator = "TopRotator";
        private const string BotRotator = "BotRotator";

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof (double), typeof (PacmanControl),
                                        new PropertyMetadata(default(double),
                                                             (o, args) => ((PacmanControl) o).PropertyChangedCallback()));

        public static readonly DependencyProperty MouseAngleProperty =
            DependencyProperty.Register("MouthAngle", typeof (double), typeof (PacmanControl),
                                        new PropertyMetadata(default(double),
                                                             (o, args) => ((PacmanControl) o).PropertyChangedCallback()));

        public double MouthAngle
        {
            get { return (double) GetValue(MouseAngleProperty); }
            set { SetValue(MouseAngleProperty, value); }
        }

        private RotateTransform _topRotator;
        private RotateTransform _botRotator;

        public PacmanControl()
        {
            Loaded += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
            MouseEnter += (sender, args) => VisualStateManager.GoToState(this, "MouseOver", true);
            MouseLeave += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
            MouseLeftButtonDown += (sender, args) => VisualStateManager.GoToState(this, "Pressed", true);
            MouseLeftButtonUp += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
        }

        public double Size
        {
            get { return (double) GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _topRotator = GetTemplateChild(TopRotator) as RotateTransform;
            _botRotator = GetTemplateChild(BotRotator) as RotateTransform;
            PropertyChangedCallback();
        }

        private void PropertyChangedCallback()
        {
            if (_topRotator != null)
            {
                _topRotator.CenterX = Size/2;
                _topRotator.CenterY = Size/2;
                _topRotator.Angle = -MouthAngle;
            }
            if (_botRotator != null)
            {
                _botRotator.CenterX = Size/2;
                _botRotator.Angle = MouthAngle;
            }
        }
    }
}