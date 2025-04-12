using DOSA_Client.lib;
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

        public PassportApplicationDetailsViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            GetNextPassportApplication = new DelegateCommand<string>(OnNext);
            CurrentUser = Context.Get<User>("User");
        }

        private void OnNext(string pageName)
        {
            Console.WriteLine($"Navigating to: {pageName}");
            Task.Run(async () =>
            {
                var Officer = await ApiClient.GetUserProfile(Context.Get<User>("User").google_id);

                
            });
            PageManager.NavigateTo(pageName);
        }
    }
}
