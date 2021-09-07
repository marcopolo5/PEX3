using System;

namespace ChatServerModule.ProximityChatLogic
{ 
    public static class DistanceCalculator
    {
        /// <summary>
        /// Returns the distance between 2 points.
        /// </summary>
        /// <param name="sLatitude">First point's latitude</param>
        /// <param name="sLongitude">First point's longitude</param>
        /// <param name="eLatitude">Second point's latitude</param>
        /// <param name="eLongitude">Second point's longitude</param>
        /// <returns>The distance in meters</returns>
        public static double CalculateDistance(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var earthRadius = 6376500.0;
            var d1 = sLatitude * (Math.PI / 180.0);
            var num1 = sLongitude * (Math.PI / 180.0);
            var d2 = eLatitude * (Math.PI / 180.0);
            var num2 = eLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return earthRadius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
