using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.Service
{
    interface IDialogHostAware
    {
        
        void OnDialogOpening(IDialogParameters parameters);

        
        DelegateCommand SaveCommand { get; set; }

        
        DelegateCommand CancelCommand { get; set; }
    }
}
