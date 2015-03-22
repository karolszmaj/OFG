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

        public CompositeTransform RootTransform
        {
            get { return ContainerTransform; }
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
            if (ContainerTransform.ScaleX*e.ScaleDelta <= MAX_SCALE &&
                ContainerTransform.ScaleX * e.ScaleDelta >= MIN_SCALE)
            {
                ContainerTransform.ScaleX *= e.ScaleDelta;
                ContainerTransform.ScaleY *= e.ScaleDelta;
            }
        }

        public void Dispose()
        {
            _gestureObserver.ManipulationChanged -= ManipulationChangedEventHandler;
            _gestureObserver.Dispose();
        }
    }
}
