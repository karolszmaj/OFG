namespace OFG.EventArgs
{
    public class ManipulationEventArgs : System.EventArgs
    {
        private readonly double _totalRotation;
        private readonly double _scaleDelta;

        public ManipulationEventArgs(double totalRotation, double scaleDelta)
        {
            _totalRotation = totalRotation;
            _scaleDelta = scaleDelta;
        }

        public double TotalRotation
        {
            get
            {
                return _totalRotation;
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
