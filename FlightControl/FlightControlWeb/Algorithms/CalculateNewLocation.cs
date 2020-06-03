using FlightControlWeb.Model;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Algorithms
{
    public class CalculateNewLocation
    {
        public static Coordinates Calculate(DateTime initialDate, DateTime actualDate, Coordinates initialLocation, Location[] segments)
        {
            int seconds = Convert.ToInt32(actualDate.Subtract(initialDate).TotalSeconds);
            int sumSeconds = 0, sumPrevSeconds = 0;
            Coordinates actualCoordSegment = initialLocation, actualCoordinates = null;
            for (int i = 0; i < segments.Length; i++)
            {
                var location = segments[i];
                sumPrevSeconds = sumSeconds;
                sumSeconds += location.TimeSpanSeconds;
                if (seconds == sumSeconds)
                {
                    actualCoordinates = location;
                    break;
                }
                else if (seconds < sumSeconds)
                {
                    var sCoord = new GeoCoordinate(actualCoordSegment.Latitude, actualCoordSegment.Longitude);
                    var eCoord = new GeoCoordinate(location.Latitude, location.Longitude);

                    var difLat = Math.Sqrt(Math.Pow((actualCoordSegment.Latitude - location.Latitude), 2));
                    var difLong = Math.Sqrt(Math.Pow((actualCoordSegment.Longitude - location.Longitude), 2));

                    var angle = Math.Atan(difLat / difLong) * (Math.PI / 180);

                    //var distance = sCoord.GetDistanceTo(eCoord);
                    var distance = Math.Sqrt(Math.Pow((actualCoordSegment.Latitude - location.Latitude), 2) +
                                             Math.Pow((actualCoordSegment.Longitude - location.Longitude), 2));

                    distance = (distance / location.TimeSpanSeconds) * (seconds - sumPrevSeconds);

                    var newLat = actualCoordSegment.Latitude + Math.Sin(angle) * distance;
                    var newLong = actualCoordSegment.Longitude + Math.Cos(angle) * distance;

                    actualCoordinates = new Coordinates { Longitude = newLong, Latitude = newLat };

                    break;
                }
                else
                {
                    actualCoordSegment = location;
                }
            }

            return actualCoordinates;
        }
    }


}
