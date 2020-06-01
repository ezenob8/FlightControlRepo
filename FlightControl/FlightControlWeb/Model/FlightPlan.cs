using FlightControlWeb.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FlightControlWeb.Model
{
    public class FlightPlan : Entity
    {
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("initial_location")]
        public virtual InitialLocation InitialLocation { get; set; }

        public ICollection<Location> Segments { get; set; }

        [JsonProperty("end_date_flight")]
        public virtual DateTime EndDateFlight {
                                                get 
                                                {
                                                    int seconds = (from segment in this.Segments
                                                                  select segment.TimeSpanSeconds).Sum();
                                                    return InitialLocation.DateTime.AddSeconds(seconds);

                                                }
                                               }

        public Flight Flight { get; set; }
    }
}
