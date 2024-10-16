using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.WPF.ViewModels
{
    class MemoUCViewModel:BindableBase
    {
        private List<MemoInfoDTO> _MemoList;
        private bool _IsShowAddMemo;
        private readonly HttpResClient HttpClient;
        public MemoUCViewModel(HttpResClient _HttpClient)
        {
            HttpClient = _HttpClient;
            QueryMemoList();
            ShowAddMemoCommand = new DelegateCommand(ShowAddMemo);
            QueryMemoListCmm = new DelegateCommand(QueryMemoList);
            AddMemoCmm=new DelegateCommand(AddMemo);
            DelCmm = new DelegateCommand<MemoInfoDTO>(Del);
        }
        public DelegateCommand QueryMemoListCmm { get; set; }

        public string SearchTitle { get; set; }


        public List<MemoInfoDTO> MemoList
        {
            get { return _MemoList; }
            set { _MemoList = value; RaisePropertyChanged(); }
        }

        private void QueryMemoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Router = $"Memo/QueryMemo?title={SearchTitle}";

            ApiResponse response = HttpClient.Execute(apiRequest);

            if (response.ResultCode == 1)
            {
                MemoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(response.ResultData.ToString());
                Visibility =(MemoList.Count>0)? Visibility.Hidden: Visibility.Visible;
            }
            else
            {
                MemoList = new List<MemoInfoDTO>();
            }
        }



        public bool IsShowAddMemo
        {
            get { return _IsShowAddMemo; }
            set { _IsShowAddMemo = value; RaisePropertyChanged(); }
        }

        private void ShowAddMemo()
        {
            IsShowAddMemo = true;
        }
        public DelegateCommand ShowAddMemoCommand { get; set; }

        private Visibility _Visibility;
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; RaisePropertyChanged(); }
        }
        public MemoInfoDTO MemoInfoDTO { get; set; }= new MemoInfoDTO();
        public DelegateCommand AddMemoCmm { get; set; }
        private void AddMemo()
        {
            if (string.IsNullOrEmpty(MemoInfoDTO.Title) || string.IsNullOrEmpty(MemoInfoDTO.Content))
            {
                MessageBox.Show("MemoList is not completed");
                return;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Parameters = MemoInfoDTO;
            apiRequest.Router = "Memo/AddMemo";
            ApiResponse response = HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                MessageBox.Show(response.Msg);
                IsShowAddMemo = false;
                QueryMemoList();
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
        }
        public DelegateCommand<MemoInfoDTO> DelCmm {  get; set; }   
        private void Del(MemoInfoDTO memo)
        {
            var delResult = MessageBox.Show("Are you sure to delete this", "Reminder", MessageBoxButton.OKCancel);
            if (delResult == MessageBoxResult.OK)
            {
                ApiRequest apiRequest = new ApiRequest();
                apiRequest.Method = RestSharp.Method.DELETE;
                apiRequest.Router = $"Memo/DelMemo?id={memo.MemoId}";

                ApiResponse response = HttpClient.Execute(apiRequest);

                if (response.ResultCode == 1)
                {
                    MessageBox.Show(response.Msg);
                    QueryMemoList();
                } else
                {
                    MessageBox.Show(response.Msg);
                } 

            }

        }

    }
 }

