using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.HttpClients
{
    internal class ApiRequest
    {
        public string Router{ get; set; }

        public Method Method { get; set; }

        public object Parameters {  get; set; }

        public string ContentType { get; set; } = "application/json";
    }
}
