﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.DTOs
{
    internal class AccountInfoDTO
    {
        public string UserName { get; set; }
        public string Pwd { get; set; }

        public string ConfirmPwd {  get; set; }
    }
}
