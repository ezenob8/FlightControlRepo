using FlightControlWeb.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.DTO
{
    public class LocationDTO
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [JsonProperty("timespan_seconds")]
        public int TimeSpanSeconds { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is LocationDTO)
            {
                var that = obj as LocationDTO;
                //Not checking if the time is the same because of formatting issue
                return this.Latitude == that.Latitude && this.Longitude == that.Longitude;
            };
            return base.Equals(obj);
        }
    }
}
