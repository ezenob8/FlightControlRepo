using Newtonsoft.Json;

namespace FlightControlWeb.DTO
{
    public class FlightDTO
    {
        [JsonProperty("flight_id")]
        public string FlightIdentifier { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("date_time")]
        public string DateTime { get; set; }

        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }

        public string ServerId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is FlightDTO)
            {
                var that = obj as FlightDTO;
                return this.CompanyName == that.CompanyName && this.DateTime == that.DateTime &&
                    this.FlightIdentifier == that.FlightIdentifier && this.IsExternal == that.IsExternal &&
                    this.Latitude == that.Latitude && this.Longitude == that.Longitude &&
                    this.Passengers == that.Passengers;
            }
            else return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.FlightIdentifier.GetHashCode();
        }
    }
}
