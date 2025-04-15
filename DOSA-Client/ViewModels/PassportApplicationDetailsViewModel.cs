using DOSA_Client.lib;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace DOSA_Client.ViewModels
{
    public class PassportApplicationDetailsViewModel : ScreenViewModelBase
    {
        public string Title => "Passport application details page";
        public PageManager PageManager { get; set; }

        public User CurrentUser { get; set; }

        public ICommand GetNextPassportApplication { get; }

        private Func<Task> _updateTabsCallback;

        public PassportApplicationDetailsViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            PageManager = pageManager;
            GetNextPassportApplication = new DelegateCommand<string>(OnNext);
            CurrentUser = Context.Get<User>(ContextKeys.USER);
            _updateTabsCallback = updateTabsCallback;
        }

        private async void OnNext(string pageName)
        {
            var Officer = Context.Get<User>(ContextKeys.USER);
            var PassportApplication = await ApiClient.GetPassportApplication(Officer.google_id);

            if (PassportApplication != null)
            {
                Context.Add("Current Passport Application", PassportApplication);
                PageManager.NavigateTo(pageName);
            }
            else
            {
                MessageBox.Show("There are no applications for you right now, please try again later.");
            }
        }
    }
}
