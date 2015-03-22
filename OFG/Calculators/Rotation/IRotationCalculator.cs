namespace OFG.Calculators.Rotation
{
    using System.Windows;

    public interface IRotationCalculator
    {
        double CalculateAngle(Point p1, RotationUnit unit);

        double CalculateAngle(Point p1, Point p2, RotationUnit unit);
    }
}
