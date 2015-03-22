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

        public FrameworkElement ManipulationHandler
        {
            get { return Handler; }
        }

        public double CurrentRotation
        {
            get { return ContainerTransform.Rotation; }
        }

        public Point GetRootControlCenterPoint()
        {
            var x = this.ActualWidth/2;
            var y = this.ActualHeight/2;

            return new Point(x,y);
        }

        public Point GetManipulationControlCenterPoint()
        {
            var x = this.ActualHeight + (18 - 24); //24 is the half of the handler and we have to add the marign offset
            var y = this.ActualHeight + (18 - 24);

            return new Point(x,y);
        }

        private void ManipulationChangedEventHandler(object sender, ManipulationEventArgs e)
        {
            HandleScale(e.ScaleDelta);
            HandleRotation(e.RotationDelta);

        }

        public void Dispose()
        {
            _gestureObserver.ManipulationChanged -= ManipulationChangedEventHandler;
            _gestureObserver.Dispose();
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

        private void HandleRotation(double rotationDelta)
        {
            if (ContainerTransform.Rotation >= 360 ||
                ContainerTransform.Rotation < 0)
            {
                ContainerTransform.Rotation = 0;
            }

            ContainerTransform.Rotation += rotationDelta;
        }
    }
}
