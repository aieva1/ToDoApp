namespace DailyAppAPI.ApiResponses
{
    public class ApiResponse
    {
        public int ResultCode { get; set; }
        public string Msg { get; set; }
        public object ResultData { get; set; }  
    }
}
