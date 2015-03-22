namespace OFG.Calculators.Scale
{
    using System;
    using System.Windows;

    public class ScaleCalculator : IScaleCalculator
    {
        public double CalculateLength(Point p1, Point p2)
        {
            double result = 0;
            result = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            return result;
        }
    }
}
