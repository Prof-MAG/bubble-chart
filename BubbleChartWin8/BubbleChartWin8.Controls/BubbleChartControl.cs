using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;

namespace BubbleChartWin8.Controls
{
    public class BubbleChartControl : Control
    {
        private const double MaxBubbleSize = 100;

        public static readonly DependencyProperty BubblesSourceProperty =
            DependencyProperty.Register("BubblesSource", typeof (object), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(object),
                                                             (o, args) =>
                                                             ((BubbleChartControl) o).OnBubblesSourceChanged(args)));

        public static readonly DependencyProperty RadiusMaxProperty =
            DependencyProperty.Register("RadiusMax", typeof (double), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(double)));

        public static readonly DependencyProperty RadiusMinProperty =
            DependencyProperty.Register("RadiusMin", typeof (double), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(double)));

        public static readonly DependencyProperty XLabelContentProperty =
            DependencyProperty.Register("XLabelContent", typeof (object), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty XLabelContentTemplateProperty =
            DependencyProperty.Register("XLabelContentTemplate", typeof (DataTemplate), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty XMaxProperty =
            DependencyProperty.Register("XMax", typeof (double), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(double)));

        public static readonly DependencyProperty XMinProperty =
            DependencyProperty.Register("XMin", typeof (double), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(double)));

        public static readonly DependencyProperty YLabelContentProperty =
            DependencyProperty.Register("YLabelContent", typeof (object), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty YLabelContentTemplateProperty =
            DependencyProperty.Register("YLabelContentTemplate", typeof (DataTemplate), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty YMaxProperty =
            DependencyProperty.Register("YMax", typeof (double), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(double)));

        public static readonly DependencyProperty YMinProperty =
            DependencyProperty.Register("YMin", typeof (double), typeof (BubbleChartControl),
                                        new PropertyMetadata(default(double)));
        
        public BubbleChartControl()
        {
            DefaultStyleKey = typeof (BubbleChartControl);
            _bubbles = new List<BubbleControl>();
        }

        private List<BubbleControl> _bubbles;

        public IEnumerable BubblesSource
        {
            get { return (IEnumerable) GetValue(BubblesSourceProperty); }
            set { SetValue(BubblesSourceProperty, value); }
        }

        public string LegendMember { get; set; }

        public double RadiusMax
        {
            get { return (double) GetValue(RadiusMaxProperty); }
            set { SetValue(RadiusMaxProperty, value); }
        }

        public string RadiusMember { get; set; }

        public double RadiusMin
        {
            get { return (double) GetValue(RadiusMinProperty); }
            set { SetValue(RadiusMinProperty, value); }
        }

        public object XLabelContent
        {
            get { return GetValue(XLabelContentProperty); }
            set { SetValue(XLabelContentProperty, value); }
        }

        public DataTemplate XLabelContentTemplate
        {
            get { return (DataTemplate) GetValue(XLabelContentTemplateProperty); }
            set { SetValue(XLabelContentTemplateProperty, value); }
        }

        public double XMax
        {
            get { return (double) GetValue(XMaxProperty); }
            set { SetValue(XMaxProperty, value); }
        }

        public string XMember { get; set; }

        public double XMin
        {
            get { return (double) GetValue(XMinProperty); }
            set { SetValue(XMinProperty, value); }
        }

        public object YLabelContent
        {
            get { return GetValue(YLabelContentProperty); }
            set { SetValue(YLabelContentProperty, value); }
        }

        public DataTemplate YLabelContentTemplate
        {
            get { return (DataTemplate) GetValue(YLabelContentTemplateProperty); }
            set { SetValue(YLabelContentTemplateProperty, value); }
        }

        public double YMax
        {
            get { return (double) GetValue(YMaxProperty); }
            set { SetValue(YMaxProperty, value); }
        }

        public string YMember { get; set; }

        public double YMin
        {
            get { return (double) GetValue(YMinProperty); }
            set { SetValue(YMinProperty, value); }
        }

        private Canvas _bubblesCanvas;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _bubblesCanvas = (Canvas) GetTemplateChild("BubblesCanvas");
            _bubblesCanvas.SizeChanged += (sender, args) => RefreshBubblePositions();
            RefreshBubbles();
        }

        private void BubblesSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged oldItem in e.OldItems.OfType<INotifyPropertyChanged>())
                    oldItem.PropertyChanged -= BubbleSourcePropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged oldItem in e.NewItems.OfType<INotifyPropertyChanged>())
                    oldItem.PropertyChanged += BubbleSourcePropertyChanged;
            }
            RefreshBubbles();
        }

        private void BubbleSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == XMember || e.PropertyName == YMember || e.PropertyName == RadiusMember)
                QueueRefreshBubblePositions();
        }

        private BubbleControl GetBubble(object bubbleSource)
        {
            BubbleControl bubble = _bubbles.FirstOrDefault(b => b.DataContext == bubbleSource);
            if (bubble == null)
            {
                bubble = new BubbleControl {DataContext = bubbleSource};
                bubble.SetBinding(BubbleControl.LegendValueProperty, new Binding {Path = new PropertyPath(LegendMember)});
                bubble.SetBinding(BubbleControl.XValueProperty, new Binding {Path = new PropertyPath(XMember)});
                bubble.SetBinding(BubbleControl.YValueProperty, new Binding {Path = new PropertyPath(YMember)});
                bubble.SetBinding(BubbleControl.RadiusProperty, new Binding {Path = new PropertyPath(RadiusMember)});
            }
            return bubble;
        }

        private static double GetPixels(double min, double max, double value, double pixelRange)
        {
            double res = (value - min) / (max - min) * pixelRange;
            return double.IsNaN(res)
                       ? pixelRange
                       : res;
        }

        private double GetBubbleSize(double radius)
        {
            return GetPixels(RadiusMin, RadiusMax, radius, MaxBubbleSize);
        }

        private double GetCanvasLeft(double xValue)
        {
            return GetPixels(XMin, XMax, xValue, _bubblesCanvas.ActualWidth);
        }

        private double GetCanvasTop(double yValue)
        {
            return _bubblesCanvas.ActualHeight - GetPixels(YMin, YMax, yValue, _bubblesCanvas.ActualHeight);
        }

        private void OnBubblesSourceChanged(DependencyPropertyChangedEventArgs args)
        {
            if (Equals(args.OldValue, args.NewValue)) return;

            // subscribe to CollectionChanged event
            var oldNotifyCollection = args.OldValue as INotifyCollectionChanged;
            var newNotifyCollection = args.NewValue as INotifyCollectionChanged;
            if (oldNotifyCollection != null)
                oldNotifyCollection.CollectionChanged -= BubblesSourceCollectionChanged;
            if (newNotifyCollection != null)
                newNotifyCollection.CollectionChanged += BubblesSourceCollectionChanged;

            // subscribe to PropertyChanged event
            var oldEnumerable = (IEnumerable) args.OldValue;
            var newEnumerable = (IEnumerable) args.NewValue;
            if (oldEnumerable != null)
            {
                foreach (INotifyPropertyChanged notifyObject in oldEnumerable.OfType<INotifyPropertyChanged>())
                    notifyObject.PropertyChanged -= BubbleSourcePropertyChanged;
            }
            if (newEnumerable != null)
            {
                foreach (INotifyPropertyChanged notifyObject in newEnumerable.OfType<INotifyPropertyChanged>())
                    notifyObject.PropertyChanged += BubbleSourcePropertyChanged;
            }

            RefreshBubbles();
        }
        
        private bool _isQueuedRefreshPositions = false;

        private void QueueRefreshBubblePositions()
        {
            if (!_isQueuedRefreshPositions)
            {
                _isQueuedRefreshPositions = true;
                Dispatcher.InvokeAsync(CoreDispatcherPriority.High, (sender, args) =>
                                                                        {
                                                                            RefreshBubblePositions();
                                                                            _isQueuedRefreshPositions = false;
                                                                        }, this, null);
            }
        }

        private void RefreshBubblePositions()
        {
            if (_bubbles.Count == 0) return;
            var storyboard = new Storyboard();
            foreach (BubbleControl bubble in _bubbles)
            {
                var sizeAnimation = new DoubleAnimation
                                        {To = GetBubbleSize(bubble.Radius), EnableDependentAnimation = true};
                var leftAnimation = new DoubleAnimation {To = GetCanvasLeft(bubble.XValue)};
                var topAnimation = new DoubleAnimation {To = GetCanvasTop(bubble.YValue)};
                storyboard.Children.Add(sizeAnimation);
                storyboard.Children.Add(leftAnimation);
                storyboard.Children.Add(topAnimation);
                Storyboard.SetTarget(sizeAnimation, bubble);
                Storyboard.SetTarget(leftAnimation, bubble);
                Storyboard.SetTarget(topAnimation, bubble);
                Storyboard.SetTargetProperty(sizeAnimation, "(BubbleControl.Size)");
                Storyboard.SetTargetProperty(leftAnimation, "(Canvas.Left)");
                Storyboard.SetTargetProperty(topAnimation, "(Canvas.Top)");
            }
            storyboard.BeginTime = TimeSpan.FromSeconds(0);
            storyboard.Duration = TimeSpan.FromSeconds(1);
            storyboard.Begin();
        }

        private void RefreshBubbles()
        {
            if (_bubblesCanvas == null) return;
            List<BubbleControl> newBubbleControls = BubblesSource.Cast<object>().Select(GetBubble).ToList();
            _bubbles.Clear();
            _bubbles.AddRange(newBubbleControls);
            _bubblesCanvas.Children.Clear();
            foreach (BubbleControl bubble in _bubbles)
                _bubblesCanvas.Children.Add(bubble);
            QueueRefreshBubblePositions();
        }
    }
}