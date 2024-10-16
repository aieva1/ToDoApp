using DailyApp.WPF.HttpClients;
using DailyApp.WPF.Service;
using DailyApp.WPF.ViewModels;
using DailyApp.WPF.ViewModels.Dialogs;
using DailyApp.WPF.Views;
using DailyApp.WPF.Views.Dailogs;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Xml.Linq;

namespace DailyApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWin>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<LoginUC, LoginUCViewModel>();

            containerRegistry.GetContainer().Register<HttpResClient>(made:Parameters.Of.Type<string>(serviceKey:"webUrl"));

            containerRegistry.RegisterForNavigation<HomeUC, HomeUCViewModel>();
            containerRegistry.RegisterForNavigation<ToDoUC, ToDoUCViewModel>();
            containerRegistry.RegisterForNavigation<MemoUC, MemoUCViewModel>();
            containerRegistry.RegisterForNavigation<SettingsUC, SettingsUCViewModel>();

            //Setting
            containerRegistry.RegisterForNavigation<PersonalUC, PersonalUCViewModel>();
            containerRegistry.RegisterForNavigation<SysSetUC>();
            containerRegistry.RegisterForNavigation<AboutUsUC>();

            //dialog
            containerRegistry.RegisterForNavigation<AddToDoUC, AddToDoUCViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoUC, AddMemoUCViewModel>();
            containerRegistry.RegisterForNavigation<EditToDoUC, EditToDoUCViewModel>();
            containerRegistry.RegisterForNavigation<EditMemoUC, EditMemoUCViewModel>();
            //Custom Dialog Service
            containerRegistry.Register<DialogHostService>();

        }
        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginUC", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                var mainVM =App.Current.MainWindow.DataContext as MainWinViewModel;
                if(mainVM!=null)
                {
                    if(callback.Parameters.ContainsKey("LoginName"))
                    {
                        string name=callback.Parameters.GetValue<string>("LoginName");
                        mainVM.SetDefaultNav(name);
                    }
                    

                }
                
                base.OnInitialized();
            });
        }

    }
}



