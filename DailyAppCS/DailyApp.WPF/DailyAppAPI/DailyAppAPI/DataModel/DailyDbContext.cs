using Microsoft.EntityFrameworkCore;

namespace DailyAppAPI.DataModel
{
    public class DailyDbContext:DbContext
    {
        public DailyDbContext(DbContextOptions<DailyDbContext> options) : base(options) 
        {

        }
        public virtual DbSet<AccountInfo> AccountInfo { get; set; }

        public virtual DbSet<ToDoInfo> ToDoInfo { get; set; }

        public virtual DbSet<MemoInfo> MemoInfo { get; set; }

    }
}
