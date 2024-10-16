using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.DTOs
{
    internal class StatToDoDTO
    {
        public int TotalCount { get; set; }
        public int CompletedCount { get; set; }
        public string Completedpercentage { get; set; }
    }
}
