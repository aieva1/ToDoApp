using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyAppAPI.DataModel
{
    [Table("MemoInfo")]
    public class MemoInfo
    {
        [Key]
        public int MemoId { get; set; }


        public string Title { get; set; }


        public string Content { get; set; }

        
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
