using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Businesses.Queries
{
    public static class DistanceCalculator
    {
        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers
            var latRad1 = lat1 * (Math.PI / 180);
            var latRad2 = lat2 * (Math.PI / 180);
            var deltaLat = (lat2 - lat1) * (Math.PI / 180);
            var deltaLon = (lon2 - lon1) * (Math.PI / 180);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c; // Distance in kilometers

            return distance;
        }
    }
}
