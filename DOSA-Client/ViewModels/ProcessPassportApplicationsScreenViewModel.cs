using System;
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
        public ProcessPassportApplicationsScreenViewModel(Func<Task> updateTabsCallback)
        {
            PageManager = new PageManager();
            RegisterPages(updateTabsCallback);
            PageManager.NavigateTo(PageNames.PassportApplicationDetails);
        }

        public void RegisterPages(Func<Task> updateTabsCallback)
        {
            PageManager.RegisterPage(PageNames.PassportApplicationDetails, new PassportApplicationDetailsViewModel(PageManager, updateTabsCallback));
            PageManager.RegisterPage(PageNames.ProcessPassportApplication, new ProcessPassportApplicationViewModel(PageManager, updateTabsCallback));
        }
    }
}