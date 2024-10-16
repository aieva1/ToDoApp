using DailyApp.WPF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DailyApp.WPF.ViewModels
{
    internal class MainWinViewModel:BindableBase
    {
        private List<LeftMenuInfo> _LeftMenuList;
        private readonly IRegionManager RegionManager;
        private IRegionNavigationJournal Journal;
        public List<LeftMenuInfo> LeftMenuList
        {
            get {  return _LeftMenuList; }
            set { _LeftMenuList = value;RaisePropertyChanged(); }
        }

        public MainWinViewModel(IRegionManager _RegionManager)
        {
            LeftMenuList=new List<LeftMenuInfo>();
            CreateMenuList();
            RegionManager = _RegionManager;
            NavigateCmm = new DelegateCommand<LeftMenuInfo>(Navigate);
            GoBackCmm=new DelegateCommand(GoBack);
            GoForwardCmm=new DelegateCommand(GoForward);    

    }
        private void CreateMenuList()
        {
            LeftMenuList.Add(new LeftMenuInfo() { Icon = "Home", MenuName = "Home", ViewName = "HomeUC" });
            LeftMenuList.Add(new LeftMenuInfo() { Icon = "NotebookOutline", MenuName = "To-Do List", ViewName = "ToDoUC" });
            LeftMenuList.Add(new LeftMenuInfo() { Icon = "NotebookPlus", MenuName = "Memo", ViewName = "MemoUC" });
            LeftMenuList.Add(new LeftMenuInfo() { Icon = "Cog", MenuName = "Settings", ViewName = "SettingsUC" });
        }
        public DelegateCommand<LeftMenuInfo> NavigateCmm {  get; set; }
        private void Navigate(LeftMenuInfo menu)
        {
            if(menu ==null || string.IsNullOrEmpty(menu.ViewName))
            {
                return;
            }
            RegionManager.Regions["MainViewRegion"].RequestNavigate(menu.ViewName, callback =>
            {
                Journal =callback.Context.NavigationService.Journal;    
            }
            );


        }
        public DelegateCommand GoBackCmm { get;private set; }
        public DelegateCommand GoForwardCmm { get;private set; }  
        
        private void GoBack()
        {
            if(Journal != null && Journal.CanGoBack)
            {
                Journal.GoBack();
            }
        }

        private void GoForward()
        {
            if (Journal != null && Journal.CanGoForward)
            {
                Journal.GoForward();
            }
        }
        public void SetDefaultNav(string loginName)
        {
            NavigationParameters paras= new NavigationParameters();
            paras.Add("LoginName",loginName);
            RegionManager.Regions["MainViewRegion"].RequestNavigate("HomeUC", callback =>
            {
                Journal = callback.Context.NavigationService.Journal;
            },paras);

        }


    }
}
