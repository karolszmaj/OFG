namespace OFG.EventArgs
{
    public class ManipulationEventArgs : System.EventArgs
    {
        private readonly double _rotationDelta;
        private readonly double _scaleDelta;

        public ManipulationEventArgs(double rotationDelta, double scaleDelta)
        {
            _rotationDelta = rotationDelta;
            _scaleDelta = scaleDelta;
        }

        public double RotationDelta
        {
            get
            {
                return _rotationDelta;
            }
        }

        public double ScaleDelta
        {
            get
            {
                return _scaleDelta;
            }
        }
    }
}
