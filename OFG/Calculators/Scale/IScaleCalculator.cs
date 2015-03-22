namespace OFG.Calculators.Scale
{
    using System.Windows;

    public interface IScaleCalculator
    {
        double CalculateLength(Point p1, Point p2);
    }
}
