using FlightControlWeb.Model.Common;

namespace FlightControlWeb.Model
{
    public class Server : Entity
    {
        public string ServerId { get; set; }
        public string ServerURL { get; set; }
    }
}