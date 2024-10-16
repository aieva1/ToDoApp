using DailyApp.WPF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.ViewModels
{
    class SettingsUCViewModel : BindableBase
    {
        private List<LeftMenuInfo> _leftMenuList;
        private readonly IRegionManager RegionManager;

        public SettingsUCViewModel(IRegionManager _RegionManager)
        {
             CreateMenuList();
             RegionManager = _RegionManager;
             NavigateCmm = new DelegateCommand<LeftMenuInfo>(Navigate);
        }
        public DelegateCommand<LeftMenuInfo> NavigateCmm { get; set; }

        private void Navigate(LeftMenuInfo menu)
        {
            if (menu == null || string.IsNullOrEmpty(menu.ViewName))
            {
                return;
            }
            RegionManager.Regions["SettingRegion"].RequestNavigate(menu.ViewName);


        }




        public List<LeftMenuInfo> leftMenuList
        {
            get { return _leftMenuList; }
            set { _leftMenuList = value; }
        }


        void CreateMenuList()
        {

            leftMenuList = new List<LeftMenuInfo>();
            leftMenuList.Add(new LeftMenuInfo() { Icon = "Palette", MenuName = "Personalization", ViewName = "PersonalUC" });
            leftMenuList.Add(new LeftMenuInfo() { Icon = "Cog", MenuName = "System Settings", ViewName = "SysSetUC" });
            leftMenuList.Add(new LeftMenuInfo() { Icon = "Information", MenuName = "About Me", ViewName = "AboutUsUC" });
        }
    }
}
