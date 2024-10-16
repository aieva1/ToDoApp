﻿using DailyApp.WPF.DTOs;
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
    internal class AddToDoUCViewModel : IDialogHostAware
    {
        private const string DailogHostName = "RootDialog";
        public DelegateCommand SaveCommand { get ; set ; }
        public DelegateCommand CancelCommand { get ; set ; }
        public AddToDoUCViewModel() 
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }
        public ToDoInfoDTO ToDoInfoDTO {  get; set; }= new ToDoInfoDTO();

        public void OnDialogOpening(IDialogParameters parameters)
        {
           
        }
        private void Save()
        {
            if(string.IsNullOrEmpty(ToDoInfoDTO.Title) || string.IsNullOrEmpty(ToDoInfoDTO.Content))
            {
                MessageBox.Show("Incompleted ToDoItem");
                return;
            }
            if (DialogHost.IsDialogOpen(DailogHostName))
            {
                DialogParameters paras= new DialogParameters();
                paras.Add("AddToDoInfo", ToDoInfoDTO);
                DialogHost.Close(DailogHostName, new DialogResult(ButtonResult.OK,paras));
            }


        }
        private void Cancel()
        {
            if(DialogHost.IsDialogOpen(DailogHostName))
            {
                DialogHost.Close (DailogHostName,new DialogResult(ButtonResult.No));
            }
            
        }

    }
}
