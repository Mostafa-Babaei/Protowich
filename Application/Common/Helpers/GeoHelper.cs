using System;

namespace Application.Common.Helpers
{
    public static class GeoHelper
    {
        /// <summary>
        /// محاسبه فاصله بین دو نقطه جغرافیایی بر اساس فرمول Haversine
        /// خروجی به متر برگردانده می‌شود
        /// </summary>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // شعاع زمین به متر

            double lat1Rad = DegreesToRadians(lat1);
            double lat2Rad = DegreesToRadians(lat2);
            double deltaLat = DegreesToRadians(lat2 - lat1);
            double deltaLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // فاصله به متر
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
