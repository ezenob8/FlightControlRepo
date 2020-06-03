using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightsTests
{
    class FakeFlightPlanDBContex : FlightControlWeb.Model.FlightPlanDBContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("FakeServer");
        }
    }
}
