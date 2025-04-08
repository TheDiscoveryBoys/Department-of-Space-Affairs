using DOSA_Client.lib;
using DOSA_Client.Models;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class ProcessPassportApplicationsScreenViewModel : ScreenViewModelBase
    {
        public string Title => "Process Passport Applications";
        public string Description => "You can process passport applications here";
        public PageManager PageManager { get; set; }
        public User CurrentUser { get; set; }
        public ProcessPassportApplicationsScreenViewModel()
        {
            PageManager = new PageManager();
            RegisterPages();
            PageManager.NavigateTo(PageNames.PassportApplicationDetails);
        }

        public void RegisterPages()
        {
            PageManager.RegisterPage(PageNames.PassportApplicationDetails, new PassportApplicationDetailsViewModel(PageManager));
            PageManager.RegisterPage(PageNames.ProcessPassportApplication, new ProcessPassportApplicationViewModel(PageManager));
        }
    }
}