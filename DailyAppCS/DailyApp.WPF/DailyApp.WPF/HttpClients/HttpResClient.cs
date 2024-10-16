using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DailyApp.WPF.HttpClients
{
    internal class HttpResClient
    {
        private readonly RestClient Client;
        private readonly string baseUrl= "http://localhost:28338/api/";

        public HttpResClient() {  Client = new RestClient(); }

        public ApiResponse Execute(ApiRequest apiRequest)
        {
            RestRequest request = new RestRequest(apiRequest.Method);
            request.AddHeader("Content-Type",apiRequest.ContentType);
            if (apiRequest.Parameters != null)
            {
                request.AddParameter("param",JsonConvert.SerializeObject(apiRequest.Parameters),ParameterType.RequestBody);
                
            }
            Client.BaseUrl = new Uri(baseUrl + apiRequest.Router);
            var res = Client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ApiResponse>(res.Content);
            }
            else
            {
                return new ApiResponse { ResultCode = -99, Msg = "Server is busy,Pls wait" };
            }
        }
    }
}
