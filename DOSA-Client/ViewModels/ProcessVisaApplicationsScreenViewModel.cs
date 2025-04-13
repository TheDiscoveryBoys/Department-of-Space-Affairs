using DOSA_Client.lib;
using DOSA_Client.Models;
using System.Windows.Input;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class ProcessVisaApplicationsScreenViewModel : ScreenViewModelBase
    {
        public string Title => "Process Visa Applications";
        public string Description => "You can process visa applications here";
        public PageManager PageManager { get; set; }
        public User CurrentUser { get; set; }

        public ProcessVisaApplicationsScreenViewModel(Func<Task> updateTabsCallback)
        {
            PageManager = new PageManager();
            RegisterPages(updateTabsCallback);
            PageManager.NavigateTo(PageNames.VisaApplicationDetails);
        }

        public void RegisterPages(Func<Task> updateTabsCallback)
        {
            PageManager.RegisterPage(PageNames.VisaApplicationDetails, new VisaApplicationDetailsViewModel(PageManager, updateTabsCallback));
            PageManager.RegisterPage(PageNames.ProcessVisaApplication, new ProcessVisaApplicationViewModel(PageManager, updateTabsCallback));
        }
    }
}