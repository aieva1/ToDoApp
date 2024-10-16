using DailyApp.WPF.DTOs;
using DailyApp.WPF.Service;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.WPF.ViewModels.Dialogs
{
    internal class EditMemoUCViewModel : IDialogHostAware
    {
        private const string DailogHostName = "RootDialog";
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public EditMemoUCViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }
        public MemoInfoDTO MemoInfoDTO { get; set; } = new MemoInfoDTO();

        public void OnDialogOpening(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("OldMemoInfo"))
            {
                MemoInfoDTO = parameters.GetValue<MemoInfoDTO>("OldMemoInfo");
            }
            else
            {
                MemoInfoDTO =new MemoInfoDTO();
            }
        }
        private void Save()
        {
            if (string.IsNullOrEmpty(MemoInfoDTO.Title) || string.IsNullOrEmpty(MemoInfoDTO.Content))
            {
                MessageBox.Show("Incompleted MemoItem");
                return;
            }
            if (DialogHost.IsDialogOpen(DailogHostName))
            {
                DialogParameters paras = new DialogParameters();
                paras.Add("EditMemoInfo", MemoInfoDTO);
                DialogHost.Close(DailogHostName, new DialogResult(ButtonResult.OK, paras));
            }


        }
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DailogHostName))
            {
                DialogHost.Close(DailogHostName, new DialogResult(ButtonResult.No));
            }

        }
    }
}
