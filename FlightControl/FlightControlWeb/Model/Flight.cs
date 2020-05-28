using FlightControlWeb.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.Model
{
    public class Flight : Entity
    {       
        public string FlightIdentifier { get; set; }

        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }
        
        public int Passengers { get { return FlightPlan.Passengers; } }
        public string CompanyName { get { return FlightPlan.CompanyName; } }
        public DateTime DateTime { get { return FlightPlan.InitialLocation.DateTime; } }
        public double Longitude { get { return FlightPlan.InitialLocation.Longitude; } }
        public double Latitude { get { return FlightPlan.InitialLocation.Latitude; } }

        public long FlightPlanId { get; set; }

        public FlightPlan FlightPlan { get; set; }
    }
}
