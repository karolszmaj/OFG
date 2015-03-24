using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OFG.Calculators.Rotation;
using OFG.Interfaces;
using System.Windows;
using OFG.Calculators.Scale;
using OFG.EventArgs;
using OFG.Calculators.Angle;
using System.Windows.Media;
using System.Windows.Controls;

namespace OFG.Handlers
{
    public class GestureObserver: IDisposable
    {
        private readonly IManipulationControlAware _target;
        private readonly IRotationCalculator _rotationCalculator;
        private readonly IScaleCalculator _scaleCalculator;
        private Point? _lastPointPosition;
        private Point _lastTouchPointPosition;
        private readonly Point CENTER = new Point(0, 0);

        public event EventHandler<ManipulationEventArgs> ManipulationChanged;

        public GestureObserver(IManipulationControlAware target, IRotationCalculator rotationCalculator, IScaleCalculator scaleCalculator)
        {
            if (rotationCalculator == null)
            {
                throw new ArgumentNullException("rotationCalculator must be passed");
            }
            else if (scaleCalculator == null)
            {
                throw new ArgumentNullException("scaleCalculator must be passed");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target must be passed");
            }

            _target = target;
            _rotationCalculator = rotationCalculator;
            _scaleCalculator = scaleCalculator;

            Initalize();
        }

        public GestureObserver(IManipulationControlAware target)
            : this(target, new RotationCalculator(), new ScaleCalculator())
        {
        }

        public void Dispose()
        {
            _target.ManipulationHandler.ManipulationDelta -= TargetManipulationDeltaEventHandler;
            Touch.FrameReported -= OnFrameReportedEventHandler;
        }

        protected void OnManipulationChanged(double rotationDelta, double scaleDelta)
        {
            var handler = ManipulationChanged;
            if(handler != null)
            {
                handler(this, new ManipulationEventArgs(rotationDelta, scaleDelta));
            }
        }

        protected void Initalize()
        {
            _target.ManipulationHandler.ManipulationDelta += TargetManipulationDeltaEventHandler;
            Touch.FrameReported += OnFrameReportedEventHandler;
        }

        private void OnFrameReportedEventHandler(object sender, TouchFrameEventArgs e)
        {
                _lastTouchPointPosition = e.GetPrimaryTouchPoint(_target.RootControl).Position;
        }

        private void TargetManipulationDeltaEventHandler(object sender, ManipulationDeltaEventArgs e)
        {
            e.Handled = true;
            if(e.PinchManipulation != null)
            {
                return;
            }

            var scaleDelta = CalculateScaleDelta();
            var rotationDelta = CalculateRotation();

            OnManipulationChanged(rotationDelta, scaleDelta);
        }

        private double CalculateScaleDelta()
        {
            if (_lastPointPosition == null)
            {
                _lastPointPosition = MapPointsToInternalCoordinateSystem(_target.GetManipulationControlCenterPoint());
            }

            var currentPoint = MapPointsToInternalCoordinateSystem(_lastTouchPointPosition);
            var lastPointLength = _scaleCalculator.CalculateLength(CENTER, _lastPointPosition.Value);
            var newPointLength = _scaleCalculator.CalculateLength(CENTER, currentPoint);

            var scaleDelta = newPointLength / lastPointLength;
            Debug.WriteLine("SCALE: {0}", scaleDelta);

            _lastPointPosition = currentPoint;
            return scaleDelta;
        }

        private double CalculateRotation()
        {
            if (_lastPointPosition == null)
            {
                _lastPointPosition = MapPointsToInternalCoordinateSystem(_target.GetManipulationControlCenterPoint());
            }

            var centerPoint = MapPointsToInternalCoordinateSystem(_target.GetRootControlCenterPoint());
            var currentFingerPosition = MapPointsToInternalCoordinateSystem(_lastTouchPointPosition);

            var p2 = new Point((float)currentFingerPosition.X, (float)currentFingerPosition.Y);
            var angle = _rotationCalculator.CalculateAngle(p2, RotationUnit.Degrees);
            _lastPointPosition = currentFingerPosition;

            return angle - _target.RotationOffset;
        }

        private Point MapPointsToInternalCoordinateSystem(Point absolutePoint)
        {
            var controlCenterPoint = _target.GetRootControlCenterPoint();
            var mappedPoint = new Point(absolutePoint.X - controlCenterPoint.X, absolutePoint.Y - controlCenterPoint.Y);

            return mappedPoint;
        }
    }
}
