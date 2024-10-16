using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using DailyApp.WPF.MsgEvents;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.WPF.ViewModels
{
    internal class LoginUCViewModel : BindableBase, IDialogAware
    {

        public event Action<IDialogResult>? RequestClose;
        private readonly HttpResClient HttpClient;
        private readonly IEventAggregator Aggregator;
        public DelegateCommand LoginCmm { get; set; }
        public DelegateCommand RegCmm { get; set; }
        public LoginUCViewModel(HttpResClient _HttpClient,IEventAggregator _Aggregator)
        {
            LoginCmm = new DelegateCommand(Login);
            ShowRegInfoCmm = new DelegateCommand<string>(ShowRegInfo);
            RegCmm = new DelegateCommand(Reg);
            AccountInfoDTO = new AccountInfoDTO();
            HttpClient = _HttpClient;
            Aggregator = _Aggregator;
        }
        private void Login()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Pwd))
            {
                Aggregator.GetEvent<MsgEvent>().Publish("Detais of Login is not completed");
                return;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            Pwd=MD5Helper.GetMd5(Pwd);
            apiRequest.Router = $"Account/Login?username={UserName}&pwd={Pwd}";
            ApiResponse response = HttpClient.Execute(apiRequest);



            if (response.ResultCode == 1)
            {
                if (RequestClose != null)
                {
                    AccountInfoDTO accountInfoDTO = JsonConvert.DeserializeObject<AccountInfoDTO>(response.ResultData.ToString());
                    DialogParameters paras = new DialogParameters();
                    paras.Add("LoginName", accountInfoDTO.UserName);
                    RequestClose(new DialogResult(ButtonResult.OK,paras));
                }
            }
            else 
            {
                Aggregator.GetEvent<MsgEvent>().Publish(response.Msg);
            }
        }
        private void Reg()
        {
            if (string.IsNullOrEmpty(AccountInfoDTO.UserName) || string.IsNullOrEmpty(AccountInfoDTO.Pwd) || string.IsNullOrEmpty(AccountInfoDTO.ConfirmPwd))
            {
                //MessageBox.Show("register not completed");
                Aggregator.GetEvent<MsgEvent>().Publish("register not completed");
                return;
            }
            if (AccountInfoDTO.Pwd != AccountInfoDTO.ConfirmPwd)
            {
                //MessageBox.Show("Two inputs of Password are different");
                Aggregator.GetEvent<MsgEvent>().Publish("Two inputs of Password are different");
                return;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Router = "Account/Register";
            AccountInfoDTO.Pwd = MD5Helper.GetMd5(AccountInfoDTO.Pwd);
            AccountInfoDTO.ConfirmPwd = MD5Helper.GetMd5(AccountInfoDTO.ConfirmPwd);
            apiRequest.Parameters = AccountInfoDTO;

            ApiResponse response = HttpClient.Execute(apiRequest);
            if(response.ResultCode==1)
            {
                //MessageBox.Show(response.Msg);
                Aggregator.GetEvent<MsgEvent>().Publish(response.Msg);
                SelectedIndex = 0;
            }
            else
            {
                //MessageBox.Show(response.Msg);
                Aggregator.GetEvent<MsgEvent>().Publish(response.Msg);
            }
        }



        public string Title { get; set; } = "Login Dialog";




        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
        public void CloseDialog(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private int _SelectedIndex;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                RaisePropertyChanged();
            }

        }
        public DelegateCommand<string> ShowRegInfoCmm { get; set; }
        private void ShowRegInfo(string indexString)
        {
            if (int.TryParse(indexString, out int index))
            {
                SelectedIndex = index;
            }
        }
        private string _UserName;
        public string UserName 
        {
            get { return _UserName; }
            set { _UserName = value; RaisePropertyChanged(); }
        }

        private string _Pwd;

        public string Pwd
        {
            get { return _Pwd; }
            set { _Pwd = value;  }
            
        }

        private AccountInfoDTO _AccountInfoDTO;

        public AccountInfoDTO AccountInfoDTO
        {
            get { return _AccountInfoDTO; }
            set
            {
                _AccountInfoDTO = value;
                RaisePropertyChanged();

            }

        }
    }
}
