using FlightControlWeb.Model;
using GeoCoordinatePortable;
using System;

namespace FlightControlWeb.Algorithms
{
    public class CalculateNewLocation
    {
        // Calculate the location of a planebased on time given in actualDate
        public static Coordinates Calculate(DateTime initialDate, DateTime actualDate, Coordinates initialLocation, Location[] segments)
        {
            int secondesPastFromStart = Convert.ToInt32(actualDate.Subtract(initialDate).TotalSeconds);
            int sumSeconds = 0, sumPrevSeconds = 0;
            Coordinates actualCoordSegment = initialLocation, actualCoordinates = null;
            for (int i = 0; i < segments.Length; i++)
            {
                var location = segments[i];
                sumPrevSeconds = sumSeconds;
                sumSeconds += location.TimeSpanSeconds;
                if (secondesPastFromStart == sumSeconds)
                {
                    // The plane is on the location now
                    actualCoordinates = location;
                    break;
                }
                else if (secondesPastFromStart < sumSeconds)
                {
                    double newLat, newLong;
                    FindCoordinates(secondesPastFromStart, sumPrevSeconds, actualCoordSegment, location, out newLat, out newLong);
                    actualCoordinates = new Coordinates { Longitude = newLong, Latitude = newLat };
                    break;
                }
                else
                {
                    // We need to calculate from the next segment
                    actualCoordSegment = location;
                }
            }
            return actualCoordinates;
        }

        private static void FindCoordinates(int secondesPastFromStart, int sumPrevSeconds, Coordinates actualCoordSegment,
            Location location, out double newLat, out double newLong)
        {
            var initCoord = new GeoCoordinate(actualCoordSegment.Latitude, actualCoordSegment.Longitude);
            var finalCoord = new GeoCoordinate(location.Latitude, location.Longitude);

            var difLat = Math.Abs(actualCoordSegment.Latitude - location.Latitude);
            var difLong = Math.Abs(actualCoordSegment.Longitude - location.Longitude);

            var distance = ((difLat + difLong) / location.TimeSpanSeconds) * (secondesPastFromStart - sumPrevSeconds);
            // Base value for plane move
            double deltaLat = 1, deltaLong = 1;
            GetDeltaValues(initCoord, finalCoord, difLat, difLong, distance, out deltaLat, out deltaLong);

            newLat = actualCoordSegment.Latitude + deltaLat;
            newLong = actualCoordSegment.Longitude + deltaLong;
        }

        private static void GetDeltaValues(GeoCoordinate initCoord, GeoCoordinate finalCoord, double difLat, double difLong,
            double distance, out double deltaLat, out double deltaLong)
        {
            // The base angle (Up-Right)
            var angle = Math.Atan(difLat / difLong);

            // The base move distance
            deltaLat = 1.0; deltaLong = 1.0;
            CorrectAngle(initCoord, finalCoord, ref deltaLat, ref deltaLong, ref angle);

            deltaLat *= (Math.Sin(angle) * distance);
            deltaLong *= (Math.Cos(angle) * distance);
        }

        private static void CorrectAngle(GeoCoordinate initCoord, GeoCoordinate finalCoord, ref double deltaLat,
            ref double deltaLong, ref double angle)
        {
            if (finalCoord.Longitude > initCoord.Longitude &&
                            finalCoord.Latitude < initCoord.Latitude)
            {
                // Down-Right
                angle -= (Math.PI / 2);
            }
            else if (finalCoord.Longitude < initCoord.Longitude &&
               finalCoord.Latitude >= initCoord.Latitude)
            {
                // Up-Left
                angle += (Math.PI / 2);
            }
            else if (finalCoord.Longitude < initCoord.Longitude &&
               finalCoord.Latitude < initCoord.Latitude)
            {
                // Down-Left
                angle += Math.PI;
            }
        }
    }
}
