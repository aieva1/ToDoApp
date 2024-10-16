using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyAppAPI.DataModel
{
    [Table("AccountInfo")]
    public class AccountInfo
    {
        [Key]
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        


    }
}
