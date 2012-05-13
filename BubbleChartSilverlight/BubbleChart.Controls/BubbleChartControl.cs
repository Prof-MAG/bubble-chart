using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace BubbleChart.Controls
{
    public class BubbleChartControl : Control
    {
        private const double MaxBubbleSize = 30;

        public static readonly DependencyProperty BubblesProperty =
            DependencyProperty.Register("Bubbles", typeof(List<BubbleControl>), typeof(BubbleChartControl),
                new PropertyMetadata(default(List<BubbleControl>)));

        public static readonly DependencyProperty BubblesSourceProperty =
            DependencyProperty.Register("BubblesSource", typeof(IEnumerable), typeof(BubbleChartControl),
                new PropertyMetadata(default(IEnumerable),
                    (o, args) => ((BubbleChartControl)o).OnBubblesSourceChanged(args)));

        private Canvas _bubblesCanvas;
        private double _radiusMax;
        private double _radiusMin;
        private double _xMax;
        private double _xMin;
        private double _yMax;
        private double _yMin;

        public BubbleChartControl()
        {
            DefaultStyleKey = typeof(BubbleChartControl);
            Bubbles = new List<BubbleControl>();
        }

        public List<BubbleControl> Bubbles
        {
            get { return (List<BubbleControl>)GetValue(BubblesProperty); }
            set { SetValue(BubblesProperty, value); }
        }

        public IEnumerable BubblesSource
        {
            get { return (IEnumerable)GetValue(BubblesSourceProperty); }
            set { SetValue(BubblesSourceProperty, value); }
        }

        public string LegendMember { get; set; }
        public string RadiusMember { get; set; }
        public string XMember { get; set; }
        public string YMember { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _bubblesCanvas = (Canvas)GetTemplateChild("BubblesCanvas");
            _bubblesCanvas.SizeChanged += (sender, args) => RefreshBubblePositions();
            RefreshBubbles();
        }

        private static double GetPixels(double min, double max, double value, double pixelRange)
        {
            double res = (value - min) / (max - min) * pixelRange;
            return double.IsNaN(res)
                ? pixelRange
                : res;
        }

        private void BubbleSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == XMember || e.PropertyName == YMember || e.PropertyName == RadiusMember)
                RefreshBubblePositions();
        }

        private void BubblesSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems != null)
            {
                foreach(INotifyPropertyChanged oldItem in e.OldItems.OfType<INotifyPropertyChanged>())
                    oldItem.PropertyChanged -= BubbleSourcePropertyChanged;
            }
            if(e.NewItems != null)
            {
                foreach(INotifyPropertyChanged oldItem in e.NewItems.OfType<INotifyPropertyChanged>())
                    oldItem.PropertyChanged += BubbleSourcePropertyChanged;
            }
            RefreshBubbles();
        }

        private BubbleControl GetBubble(object bubbleSource)
        {
            BubbleControl bubble = Bubbles.FirstOrDefault(b => b.DataContext == bubbleSource);
            if(bubble == null)
            {
                bubble = new BubbleControl { DataContext = bubbleSource };
                bubble.SetBinding(BubbleControl.LegendValueProperty, new Binding(LegendMember));
                bubble.SetBinding(BubbleControl.XValueProperty, new Binding(XMember));
                bubble.SetBinding(BubbleControl.YValueProperty, new Binding(YMember));
                bubble.SetBinding(BubbleControl.RadiusProperty, new Binding(RadiusMember));
            }
            return bubble;
        }

        private double GetBubbleSize(double radius)
        {
            return GetPixels(_radiusMin, _radiusMax, radius, MaxBubbleSize);
        }

        private double GetCanvasLeft(double xValue)
        {
            return GetPixels(_xMin, _xMax, xValue, _bubblesCanvas.ActualWidth);
        }

        private double GetCanvasTop(double yValue)
        {
            return GetPixels(_yMin, _yMax, yValue, _bubblesCanvas.ActualHeight);
        }

        private void OnBubblesSourceChanged(DependencyPropertyChangedEventArgs args)
        {
            if(Equals(args.OldValue, args.NewValue)) return;

            // subscribe to CollectionChanged event
            var oldNotifyCollection = args.OldValue as INotifyCollectionChanged;
            var newNotifyCollection = args.NewValue as INotifyCollectionChanged;
            if(oldNotifyCollection != null)
                oldNotifyCollection.CollectionChanged -= BubblesSourceCollectionChanged;
            if(newNotifyCollection != null)
                newNotifyCollection.CollectionChanged += BubblesSourceCollectionChanged;

            // subscribe to PropertyChanged event
            var oldEnumerable = (IEnumerable)args.OldValue;
            var newEnumerable = (IEnumerable)args.NewValue;
            if(oldEnumerable != null)
            {
                foreach(INotifyPropertyChanged notifyObject in oldEnumerable.OfType<INotifyPropertyChanged>())
                    notifyObject.PropertyChanged -= BubbleSourcePropertyChanged;
            }
            if(newEnumerable != null)
            {
                foreach(INotifyPropertyChanged notifyObject in newEnumerable.OfType<INotifyPropertyChanged>())
                    notifyObject.PropertyChanged += BubbleSourcePropertyChanged;
            }

            RefreshBubbles();
        }

        private void RefreshBubblePositions()
        {
            if(Bubbles.Count == 0) return;
            _xMin = Bubbles.Min(bubble => bubble.XValue);
            _xMax = Bubbles.Max(bubble => bubble.XValue);
            _yMin = Bubbles.Min(bubble => bubble.YValue);
            _yMax = Bubbles.Max(bubble => bubble.YValue);
            _radiusMin = Bubbles.Min(bubble => bubble.Radius);
            _radiusMax = Bubbles.Max(bubble => bubble.Radius);
            var storyboard = new Storyboard();
            foreach (BubbleControl bubble in Bubbles)
            {
                var sizeAnimation = new DoubleAnimation { To = GetBubbleSize(bubble.Radius) };
                var leftAnimation = new DoubleAnimation { To = GetCanvasLeft(bubble.XValue) };
                var topAnimation = new DoubleAnimation { To = GetCanvasTop(bubble.YValue) };
                storyboard.Children.Add(sizeAnimation);
                storyboard.Children.Add(leftAnimation);
                storyboard.Children.Add(topAnimation);
                Storyboard.SetTarget(sizeAnimation, bubble);
                Storyboard.SetTarget(leftAnimation, bubble);
                Storyboard.SetTarget(topAnimation, bubble);
                Storyboard.SetTargetProperty(sizeAnimation, new PropertyPath(BubbleControl.SizeProperty));
                Storyboard.SetTargetProperty(leftAnimation, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTargetProperty(topAnimation, new PropertyPath(Canvas.TopProperty));
            }
            storyboard.Begin();
        }

        private void RefreshBubbles()
        {
            if(_bubblesCanvas == null) return;
            List<BubbleControl> newBubbleControls = BubblesSource.Cast<object>().Select(GetBubble).ToList();
            Bubbles.Clear();
            Bubbles.AddRange(newBubbleControls);
            _bubblesCanvas.Children.Clear();
            foreach(BubbleControl bubble in Bubbles)
                _bubblesCanvas.Children.Add(bubble);
            RefreshBubblePositions();
        }
    }
}