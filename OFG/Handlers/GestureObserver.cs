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

namespace OFG.Handlers
{
    public class GestureObserver: IDisposable
    {
        private readonly IManipulationControlAware _target;
        private readonly IRotationCalculator _rotationCalculator;
        private readonly IScaleCalculator _scaleCalculator;

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
        }

        private void TargetManipulationDeltaEventHandler(object sender, ManipulationDeltaEventArgs e)
        {
            e.Handled = true;
            if(e.PinchManipulation != null)
            {
                return;
            }

            var rotationDelta = CalculateRotation(e.DeltaManipulation.Translation);
            var scaleDelta = CalculateScale(e.DeltaManipulation.Translation);

            OnManipulationChanged(rotationDelta, scaleDelta);
        }

        private double CalculateScale(Point delta)
        {
            var centerPoint = _target.GetRootControlCenterPoint();
            var lastPointOrigin = _target.GetManipulationControlCenterPoint();
            var newPointOrigin = new Point(lastPointOrigin.X + delta.X, lastPointOrigin.Y + delta.Y);

            var lastPointLength = _scaleCalculator.CalculateLength(centerPoint, lastPointOrigin);
            var newPointLength = _scaleCalculator.CalculateLength(centerPoint, newPointOrigin);

            var scale = newPointLength / lastPointLength;

            Debug.WriteLine("Scale: ManipulationPoint[X,Y]: {0} {1}\nDelta[X,Y]: {2} {3}\nScale: {4}\n",
                centerPoint.X,
                centerPoint.Y,
                delta.X,
                delta.Y,
                scale);

            return scale;
        }

        private int CalculateRotation(Point delta)
        {
            var manipulationPoint = _target.GetManipulationControlCenterPoint();
            var centerPoint = _target.GetRootControlCenterPoint();

            var deltaPoint = new Point(manipulationPoint.X + delta.X, manipulationPoint.Y + delta.Y);
            var angle = _rotationCalculator.CalculateAngle(centerPoint, deltaPoint, RotationUnit.Degrees);
            var angleDelta = _target.CurrentRotation - angle;

            Debug.WriteLine("Rotation: ManipulationPoint[X,Y]: {0} {1}\nDelta[X,Y]: {2} {3}\nAngle: {4}\n",
                manipulationPoint.X,
                manipulationPoint.Y,
                delta.X,
                delta.Y,
                angle);

            return (int)angleDelta;
        }
    }
}
