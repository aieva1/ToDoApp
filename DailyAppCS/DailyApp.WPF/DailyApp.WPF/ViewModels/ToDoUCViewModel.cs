using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.WPF.ViewModels
{
    class ToDoUCViewModel : BindableBase,INavigationAware
    {
        private List<ToDoInfoDTO> _ToDoList;
        private bool _IsShowAddToDo;
        private readonly HttpResClient HttpClient;
        public ToDoUCViewModel(HttpResClient _HttpClient)
        {
            HttpClient = _HttpClient;
            ShowAddToDoCommand = new DelegateCommand(ShowAddToDo);
            QueryToDoListCmm = new DelegateCommand(QueryToDoList);
            AddToDoCmm = new DelegateCommand(AddToDo);
            DelCmm = new DelegateCommand<ToDoInfoDTO>(Del);
        }


        public List<ToDoInfoDTO> ToDoList
        {
            get { return _ToDoList; }
            set { _ToDoList = value; RaisePropertyChanged(); }
        }
        public string SearchToDoTitle {  get; set; }
        private int _SearchToDoIndex;
        public int SearchToDoIndex { get { return _SearchToDoIndex; } 
            set {
                _SearchToDoIndex=value; RaisePropertyChanged();
            } }
        public DelegateCommand QueryToDoListCmm {  get; set; }

        private void QueryToDoList()
        {
           
            int? status = null;
            if (SearchToDoIndex == 1)
            {
                status = 0;
            }
            if (SearchToDoIndex == 2)
            {
                status = 1;
            }

            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Router = $"ToDo/QueryToDo?title={SearchToDoTitle}&status={status}";

            ApiResponse response = HttpClient.Execute(apiRequest);

            if (response.ResultCode == 1)
            {
                ToDoList = JsonConvert.DeserializeObject<List<ToDoInfoDTO>>(response.ResultData.ToString());
                Visibility = (ToDoList.Count > 0) ? Visibility.Hidden : Visibility.Visible;

            }
            else
            {
               ToDoList = new List<ToDoInfoDTO>();
            }

        }

        

        public bool IsShowAddToDo
        {
            get { return _IsShowAddToDo; }
            set { _IsShowAddToDo = value; RaisePropertyChanged(); }
        }

        private void ShowAddToDo()
        {
            IsShowAddToDo = true;
        }
        private Visibility _Visibility;
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; RaisePropertyChanged(); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("SelectedIndex"))
            {
                SearchToDoIndex = navigationContext.Parameters.GetValue<int>("SelectedIndex");
            }
            else
            {
                SearchToDoIndex= 0;
            }
            QueryToDoList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public DelegateCommand ShowAddToDoCommand { get; set; }

        public ToDoInfoDTO ToDoInfoDTO { get; set; } = new ToDoInfoDTO();

        public DelegateCommand AddToDoCmm { get; set; }
        private void AddToDo()
        {
            if (string.IsNullOrEmpty(ToDoInfoDTO.Title) || string.IsNullOrEmpty(ToDoInfoDTO.Content))
            {
                MessageBox.Show("ToDoList is not completed");
                return;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Parameters = ToDoInfoDTO;
            apiRequest.Router = "ToDo/AddToDo";
            ApiResponse response = HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                MessageBox.Show(response.Msg);
                IsShowAddToDo = false;
                QueryToDoList();
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
        }
        public DelegateCommand<ToDoInfoDTO> DelCmm { get; set; }
        private void Del(ToDoInfoDTO todo)
        {
            var delResult = MessageBox.Show("Are you sure to delete this", "Reminder", MessageBoxButton.OKCancel);
            if (delResult == MessageBoxResult.OK)
            {
                ApiRequest apiRequest = new ApiRequest();
                apiRequest.Method = RestSharp.Method.DELETE;
                apiRequest.Router = $"ToDo/DelToDo?id={todo.ToDoId}";

                ApiResponse response = HttpClient.Execute(apiRequest);

                if (response.ResultCode == 1)
                {
                    MessageBox.Show(response.Msg);
                    QueryToDoList();
                }
                else
                {
                    MessageBox.Show(response.Msg);
                }

            }

        }

    }
}
