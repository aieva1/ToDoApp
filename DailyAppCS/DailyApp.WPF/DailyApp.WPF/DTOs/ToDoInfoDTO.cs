using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.DTOs
{
    class ToDoInfoDTO:BindableBase
    {
        public int ToDoId { get; set; }

        
        public string Title { get; set; }

        
        public string Content { get; set; }

        
        public int Status { get; set; }

        public string BackColor
        {
            get
            {
                return Status == 0 ? "#FFA500" : "#87CEEB";
            }
        }

        

    }
}
