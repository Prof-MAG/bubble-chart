using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;

namespace BubbleChartWin8.Controls
{
    public class BubbleChartControl : Control
    {
        private const double MaxBubbleSize = 30;

        public static readonly DependencyProperty BubblesProperty =
            DependencyProperty.Register("Bubbles", typeof(List<BubbleControl>), typeof(BubbleChartControl),
                new PropertyMetadata(default(List<BubbleControl>)));

        public static readonly DependencyProperty BubblesSourceProperty =
            DependencyProperty.Register("BubblesSource", typeof(object), typeof(BubbleChartControl),
                new PropertyMetadata(default(object),
                    (o, args) => ((BubbleChartControl)o).OnBubblesSourceChanged(args)));

        public static readonly DependencyProperty XLabelContentProperty =
            DependencyProperty.Register("XLabelContent", typeof(object), typeof(BubbleChartControl),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty XLabelContentTemplateProperty =
            DependencyProperty.Register("XLabelContentTemplate", typeof(DataTemplate), typeof(BubbleChartControl),
                new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty YLabelContentProperty =
            DependencyProperty.Register("YLabelContent", typeof(object), typeof(BubbleChartControl),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty YLabelContentTemplateProperty =
            DependencyProperty.Register("YLabelContentTemplate", typeof(DataTemplate), typeof(BubbleChartControl),
                new PropertyMetadata(default(DataTemplate)));

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

        public object XLabelContent
        {
            get { return GetValue(XLabelContentProperty); }
            set { SetValue(XLabelContentProperty, value); }
        }

        public DataTemplate XLabelContentTemplate
        {
            get { return (DataTemplate)GetValue(XLabelContentTemplateProperty); }
            set { SetValue(XLabelContentTemplateProperty, value); }
        }

        public string XMember { get; set; }

        public object YLabelContent
        {
            get { return GetValue(YLabelContentProperty); }
            set { SetValue(YLabelContentProperty, value); }
        }

        public DataTemplate YLabelContentTemplate
        {
            get { return (DataTemplate)GetValue(YLabelContentTemplateProperty); }
            set { SetValue(YLabelContentTemplateProperty, value); }
        }

        public string YMember { get; set; }

        protected override void OnApplyTemplate()
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
                bubble.SetBinding(BubbleControl.LegendValueProperty, new Binding { Path = new PropertyPath(LegendMember) });
                bubble.SetBinding(BubbleControl.XValueProperty, new Binding { Path = new PropertyPath(XMember) });
                bubble.SetBinding(BubbleControl.YValueProperty, new Binding { Path = new PropertyPath(YMember) });
                bubble.SetBinding(BubbleControl.RadiusProperty, new Binding { Path = new PropertyPath(RadiusMember) });
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
            return _bubblesCanvas.ActualHeight - GetPixels(_yMin, _yMax, yValue, _bubblesCanvas.ActualHeight);
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
            foreach(BubbleControl bubble in Bubbles)
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
                Storyboard.SetTargetProperty(sizeAnimation, "(BubbleControl.Size)");
                Storyboard.SetTargetProperty(leftAnimation, "(Canvas.Left)");
                Storyboard.SetTargetProperty(topAnimation, "(Canvas.Top)");
                bubble.Size = GetBubbleSize(bubble.Radius); 
                /*bubble.SetValue(Canvas.LeftProperty, GetCanvasLeft(bubble.XValue));
                bubble.SetValue(Canvas.TopProperty, GetCanvasTop(bubble.YValue));*/
            }
            storyboard.BeginTime = TimeSpan.FromSeconds(0);
            storyboard.Duration = TimeSpan.FromSeconds(1);
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