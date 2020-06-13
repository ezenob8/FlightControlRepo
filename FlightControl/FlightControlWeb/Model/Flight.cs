using FlightControlWeb.Model.Common;
using Newtonsoft.Json;

namespace FlightControlWeb.Model
{
    public class Flight : Entity
    {       
        public string FlightIdentifier { get; set; }
        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }
        public long FlightPlanId { get; set; }
        public FlightPlan FlightPlan { get; set; }
    }
}
