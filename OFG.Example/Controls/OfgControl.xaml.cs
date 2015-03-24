using System;
using System.Windows;
using System.Windows.Media;
using OFG.EventArgs;
using OFG.Handlers;
using OFG.Interfaces;

namespace OFG.Example.Controls
{
    public partial class OfgControl: IManipulationControlAware, IDisposable
    {
        private const double MAX_SCALE = 1.8;
        private const double MIN_SCALE = 0.5;

        private readonly GestureObserver _gestureObserver;

        public OfgControl()
        {
            InitializeComponent();
            _gestureObserver = new GestureObserver(this);
            _gestureObserver.ManipulationChanged += ManipulationChangedEventHandler;
        }

        public double RotationOffset
        {
            get { return 45; }
        }

        public FrameworkElement RootControl
        {
            get { return this; }
        }

        public FrameworkElement ManipulationHandler
        {
            get { return Handler; }
        }

        public Point GetRootControlCenterPoint()
        {
            return new Point(this.ActualWidth / 2, this.ActualHeight / 2);
        }

        public Point GetManipulationControlCenterPoint()
        {
            var transform = App.RootFrame.TransformToVisual(Handler);
            var result = transform.Transform(new Point(0, 0));
            return new Point(Math.Abs(result.X), result.Y);
        }

        public void Dispose()
        {
            _gestureObserver.ManipulationChanged -= ManipulationChangedEventHandler;
            _gestureObserver.Dispose();
        }

        private void ManipulationChangedEventHandler(object sender, ManipulationEventArgs e)
        {
            HandleRotation(e.TotalRotation);
            HandleScale(e.ScaleDelta);
        }

        private void HandleScale(double scaleDelta)
        {
            if (ContainerTransform.ScaleX * scaleDelta <= MAX_SCALE &&
               ContainerTransform.ScaleX * scaleDelta >= MIN_SCALE)
            {
                ContainerTransform.ScaleX *= scaleDelta;
                ContainerTransform.ScaleY *= scaleDelta;
            }
        }

        private void HandleRotation(double angle)
        {
            ContainerTransform.Rotation = angle;
        }
    }
}
