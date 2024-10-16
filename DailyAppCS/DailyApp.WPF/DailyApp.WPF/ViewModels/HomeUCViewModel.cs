using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using DailyApp.WPF.Models;
using DailyApp.WPF.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.WPF.ViewModels
{
    class HomeUCViewModel : BindableBase, INavigationAware
    {
        private List<StatPanelInfo> _StatPanelList;
        private List<ToDoInfoDTO> _ToDoList;
        private List<MemoInfoDTO> _MemoList;
        private StatToDoDTO SataToDoDTO { get; set; }=new StatToDoDTO();
        private string _LoginInfo;
        private readonly HttpResClient HttpClient;
        private readonly IDialogService DialogService;
        private readonly DialogHostService DialogHostService; 
        private readonly IRegionManager RegionManager;

        public HomeUCViewModel(HttpResClient _HttpClient,IDialogService _DialogService, DialogHostService _DialogHostService, IRegionManager _RegionManager)
        {
            CreateStatPanelList();
            HttpClient = _HttpClient;
            GetMemoList();
            GetToDoList();
            ShowAddToDoDialogCmm  = new DelegateCommand(ShowAddToDoDialog);
            DialogService = _DialogService;
            DialogHostService = _DialogHostService;
            ChangeToDoStatusCmm = new DelegateCommand<ToDoInfoDTO>(ChangeToDoStatus);
            ShowEditToDoDialogCmm = new DelegateCommand<ToDoInfoDTO>(ShowEditToDoDialog);
            NavigateCmm = new DelegateCommand<StatPanelInfo>(Navigate);
            RegionManager = _RegionManager;
            CallStatmemo();
            ShowAddMemoDialogCmm = new DelegateCommand(ShowAddMemoDialog);
            ShowEditMemoDialogCmm = new DelegateCommand<MemoInfoDTO>(ShowEditMemoDialog);
        }
        public string LoginInfo
        {
            get { return _LoginInfo; }
            set { _LoginInfo = value; RaisePropertyChanged(); }
        }

        public List<StatPanelInfo> StatPanelList
        {
            get { return _StatPanelList; }
            set { _StatPanelList = value; RaisePropertyChanged(); }
        }

        public List<ToDoInfoDTO> ToDoList
        {
            get { return _ToDoList; }
            set { _ToDoList = value; RaisePropertyChanged(); }
        }
        public List<MemoInfoDTO> MemoList
        {
            get { return _MemoList; }
            set { _MemoList = value; RaisePropertyChanged(); }
        }
        public DelegateCommand<ToDoInfoDTO> ChangeToDoStatusCmm { get; set; }

        private void ChangeToDoStatus(ToDoInfoDTO todo)
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.PUT;
            apiRequest.Parameters = todo;
            apiRequest.Router = "ToDo/UpdateStatus";
            ApiResponse response = HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)//修改成功
            {
                GetToDoList();//刷新列表
                CallStaToDo();//刷统计数据
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
        }


        private void CreateStatPanelList()
        {
            StatPanelList = new List<StatPanelInfo>();
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockFast", ItemName = "Title", BackColor = "#FF0CA0FF", ViewName = "ToDoUC", Result = "9" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockCheckOutline", ItemName = "Completed", BackColor = "#FF1ECA3A", ViewName = "ToDoUC", Result = "9" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ChartLineVariant", ItemName = "Completion Ratio", BackColor = "#FF02C6DC", Result = "90%" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "PlaylistStar", ItemName = "Memo", BackColor = "#FFFFA000", ViewName = "MemoUC", Result = "20" });
        }
        private void GetToDoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Router = "ToDo/GetToDoList";

            ApiResponse response = HttpClient.Execute(apiRequest);

            if (response.ResultCode == 1)//获取成功
            {
                ToDoList = JsonConvert.DeserializeObject<List<ToDoInfoDTO>>(response.ResultData.ToString());
            }
            else
            {
                ToDoList = new List<ToDoInfoDTO>();
            }

        }
        private void GetMemoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Router = "Memo/QueryMemo";

            ApiResponse response = HttpClient.Execute(apiRequest);

            if (response.ResultCode == 1)//获取成功
            {
                MemoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(response.ResultData.ToString());
            }
            else
            {
                MemoList = new List<MemoInfoDTO>();
            }

        }

       

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("LoginName"))
            {
                DateTime now = DateTime.Now;
                string[] week = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                string loginName = navigationContext.Parameters.GetValue<string>("LoginName");
                LoginInfo = $"Hello! {loginName}. Today is {now.ToString("dd-MM-yyyy")} {week[(int)now.DayOfWeek]}";
                CallStaToDo();
            }
        }

            public bool IsNavigationTarget(NavigationContext navigationContext)
            {
                return true;
            }

            public void OnNavigatedFrom(NavigationContext navigationContext)
            {

            }
        
        private void CallStaToDo()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Router = "ToDo/StatToDo";

            ApiResponse response=HttpClient.Execute(apiRequest);

            if (response.ResultCode == 1)
            {
                SataToDoDTO = JsonConvert.DeserializeObject<StatToDoDTO>(response.ResultData.ToString());
                RefreshToDoStat();
                GetToDoList();
            }
        }

        private void RefreshToDoStat()
        {
            StatPanelList[0].Result= SataToDoDTO.TotalCount.ToString();
            StatPanelList[1].Result = SataToDoDTO.CompletedCount.ToString();
            StatPanelList[2].Result = SataToDoDTO.Completedpercentage.ToString();
        }

        public DelegateCommand ShowAddToDoDialogCmm { get; set; }
        public DelegateCommand ShowAddMemoDialogCmm { get; set; }
        private async void ShowAddMemoDialog()
        {
            var result = await DialogHostService.ShowDialog("AddMemoUC", null);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("AddMemoInfo"))
                {
                    var addModel = result.Parameters.GetValue<MemoInfoDTO>("AddMemoInfo");
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.POST;
                    apiRequest.Parameters = addModel;
                    apiRequest.Router = "Memo/AddMemo";
                    ApiResponse response = HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        MessageBox.Show(response.Msg);
                        CallStatmemo();

                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }

        }
        private async void ShowAddToDoDialog()
        {
           var result=await DialogHostService.ShowDialog("AddToDoUC", null);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("AddToDoInfo")) 
                { 
                    var addModel = result.Parameters.GetValue<ToDoInfoDTO>("AddToDoInfo");
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method= RestSharp.Method.POST;
                    apiRequest.Parameters = addModel;
                    apiRequest.Router = "ToDo/AddToDo";
                    ApiResponse response=HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        MessageBox.Show(response.Msg);
                        CallStaToDo();

                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }
        }
        public DelegateCommand<ToDoInfoDTO> ShowEditToDoDialogCmm { get; set; }

        private async void ShowEditToDoDialog(ToDoInfoDTO todo)
        {
            DialogParameters paras= new DialogParameters();
            paras.Add("OldToDoInfo", todo);
            var result = await DialogHostService.ShowDialog("EditToDoUC", paras);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("EditToDoInfo"))
                {
                    var newModel = result.Parameters.GetValue<ToDoInfoDTO>("EditToDoInfo");
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.PUT;
                    apiRequest.Parameters = newModel;
                    apiRequest.Router = "ToDo/UpdateToDo";
                    ApiResponse response = HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        MessageBox.Show(response.Msg);
                        CallStaToDo();
                        GetToDoList();

                    }else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }
        }

        public DelegateCommand<MemoInfoDTO> ShowEditMemoDialogCmm { get; set; }

        private async void ShowEditMemoDialog(MemoInfoDTO memo)
        {
            DialogParameters paras = new DialogParameters();
            paras.Add("OldMemoInfo",memo);
            var result = await DialogHostService.ShowDialog("EditMemoUC", paras);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("EditMemoInfo"))
                {
                    var newModel = result.Parameters.GetValue<MemoInfoDTO>("EditMemoInfo");
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.PUT;
                    apiRequest.Parameters = newModel;
                    apiRequest.Router = "Memo/EditMemo";
                    ApiResponse response = HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        MessageBox.Show(response.Msg);
                        GetMemoList();

                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }
        }

        public DelegateCommand<StatPanelInfo> NavigateCmm {  get; set; }

        private void Navigate(StatPanelInfo info)
        {
            if (!string.IsNullOrEmpty(info.ViewName)) 
            {
                if (info.ItemName== "Completed")
                {
                    NavigationParameters paras = new NavigationParameters();
                    paras.Add("SelectedIndex", 2);
                    RegionManager.Regions["MainViewRegion"].RequestNavigate(info.ViewName,paras);

                }
                else
                {
                    RegionManager.Regions["MainViewRegion"].RequestNavigate(info.ViewName);
                }
                
            }
            
        }

        private void CallStatmemo()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Router = "Memo/StatMemo";

            ApiResponse response = HttpClient.Execute(apiRequest);

            if (response.ResultCode == 1)
            {
                StatPanelList[3].Result = response.ResultData.ToString();
                GetMemoList();

            }
        }
        



    }
}
