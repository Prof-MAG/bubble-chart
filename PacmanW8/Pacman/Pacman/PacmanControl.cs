

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
namespace Pacman
{
    [TemplateVisualState(Name = "Normal")]
    [TemplateVisualState(Name = "MouseOver")]
    [TemplateVisualState(Name = "Pressed")]
    [TemplatePart(Name = TopChew, Type = typeof(RotateTransform))]
    [TemplatePart(Name = BotChew, Type = typeof(RotateTransform))]
    public class PacmanControl : Control
    {
        private const string BotChew = "BotChew";
        private const string TopChew = "TopChew";

        public static readonly DependencyProperty MouseAngleProperty =
            DependencyProperty.Register("MouthAngle", typeof(double), typeof(PacmanControl),
                new PropertyMetadata(default(double),
                    (o, args) => ((PacmanControl)o).PropertyChangedCallback()));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(PacmanControl),
                new PropertyMetadata(default(double),
                    (o, args) => ((PacmanControl)o).PropertyChangedCallback()));

        private Path _botChew;
        private Path _topChew;

        public PacmanControl()
        {
            Loaded += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
            PointerEntered += (sender, args) => VisualStateManager.GoToState(this, "MouseOver", true);
            PointerExited += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
            PointerPressed += (sender, args) => VisualStateManager.GoToState(this, "Pressed", true);
            PointerReleased += (sender, args) => VisualStateManager.GoToState(this, "Normal", true);
        }

        public double MouthAngle
        {
            get { return (double)GetValue(MouseAngleProperty); }
            set { SetValue(MouseAngleProperty, value); }
        }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _topChew = GetTemplateChild(TopChew) as Path;
            _botChew = GetTemplateChild(BotChew) as Path;
            PropertyChangedCallback();
        }

        private void PropertyChangedCallback()
        {
            if (_topChew != null)
            {
                var topRotator = _topChew.RenderTransform as RotateTransform;
                if (topRotator != null)
                {
                    topRotator.CenterX = Size / 2;
                    topRotator.CenterY = Size / 2;
                    topRotator.Angle = -MouthAngle;
                }
            }
            if(_botChew != null)
            {
                var botRotator = _botChew.RenderTransform as RotateTransform;
                if (botRotator != null)
                {
                    botRotator.CenterX = Size / 2;
                    botRotator.Angle = MouthAngle;
                }
            }
        }
    }
}