using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFG.Calculators.Angle
{
    public static class AngleConverter
    {
        public static double DegreeAngleToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double RatianAngleToDegrees(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
    }
}
