using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyAppAPI.DataModel
{
    [Table("ToDoInfo")]
    public class ToDoInfo
    {
        [Key]
        public int ToDoId { get; set; }


        public string Title { get; set; }


        public string Content { get; set; }

        /// <summary>
        /// status 0-ToDo,1-completed
        /// </summary>
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }= DateTime.Now;
    }
}
