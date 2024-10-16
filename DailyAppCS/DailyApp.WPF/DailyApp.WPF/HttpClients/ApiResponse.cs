using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.HttpClients
{
    internal class ApiResponse
    {
        public int ResultCode { get; set; }
        public string Msg { get; set; }
        public object ResultData { get; set; }
    }
}
