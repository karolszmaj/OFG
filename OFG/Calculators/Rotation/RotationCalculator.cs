﻿namespace OFG.Calculators.Rotation
{
    using System;
    using System.Windows;

    public class RotationCalculator : IRotationCalculator
    {
        public double CalculateAngle(Point p1, Point rotationCenter, RotationUnit unit)
        {
            double result = 0;
            result = CalculateArcTangens(p1, rotationCenter);
            result = ConvertFromRadiansTo(result, unit);
            return result;
        }

        public double CalculateAngle(Point p1, RotationUnit unit)
        {
            double result = 0;
            result = CalculateArcTangens(p1);
            result = ConvertFromRadiansTo(result, unit);
            return result;
        }

        private double ConvertFromRadiansTo(double radians, RotationUnit unit)
        {
            double result = 0;

            if (unit == RotationUnit.Radians)
            {
                result = radians;
            }
            else if (unit == RotationUnit.Degrees)
            {
                result = radians*180f/Math.PI;
            }

            return result;
        }

        private double CalculateArcTangens(Point p1)
        {
            return Math.Atan2(p1.Y, p1.X);
        }

        private double CalculateArcTangens(Point p1, Point center)
        {
            return Math.Atan2(center.Y - p1.Y, center.X - p1.X);
        }
    }
}
