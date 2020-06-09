using FlightControlWeb.Model;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FlightControlWeb.CORSManager
{
    public class AllowCorsAttribute : Attribute, ICorsPolicyProvider
    {
        private string _configName;
     
        public AllowCorsAttribute(string name = null)
        {
            _configName = name ?? "Default";
        }
     
        public string ConfigName
        {
            get { return _configName; }
        }
     
        public Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            FlightPlanDBContext flightPlanDBContext = new FlightPlanDBContext();

            var arrayOrigins = (from server in flightPlanDBContext.Servers.ToList()
                                select server.ServerURL.Substring(0, server.ServerURL.Length - 1)).ToArray();

            var retval = new CorsPolicy();
            retval.SupportsCredentials = false;

            foreach (var each in arrayOrigins)
            {
                retval.Origins.Add(each);
            }

            return Task.FromResult(retval);
        }
    }
}
