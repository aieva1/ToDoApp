namespace DailyAppAPI.DTOs
{
    public class StatToDoDTO
    {
        public int TotalCount { get; set; }
        public int CompletedCount {  get; set; }
        public string Completedpercentage
        {
            get
            {
                if (TotalCount == 0)
                {
                    return "0.00%";
                }
                return (CompletedCount * 100.00 / TotalCount).ToString("f2") + "%";
            }
        }
    }
}
