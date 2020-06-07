using FlightControlWeb.Model.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class Server : Entity
    {
        public string ServerId { get; set; }
        public string ServerURL { get; set; }
    }
}