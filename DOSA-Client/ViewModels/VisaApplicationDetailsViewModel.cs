using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib;
using DOSA_Client.Models;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using DOSA_Client.lib.Constants;

namespace DOSA_Client.ViewModels
{
    public class VisaApplicationDetailsViewModel : ScreenViewModelBase
    {
        public string Title => "Visa application details page";
        public PageManager PageManager { get; set; }

        public User CurrentUser { get; set; }
        private Func<Task> _updateTabsCallback;
        public VisaApplicationDetailsViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            PageManager = pageManager;
            GetNextVisaApplication = new DelegateCommand<string>(OnNext);
            CurrentUser = Context.Get<User>(ContextKeys.USER);
            _updateTabsCallback = updateTabsCallback;
        }

        public ICommand GetNextVisaApplication { get; }
        public async void OnNext(string pageName)
        {
            Console.WriteLine(pageName);

            var Officer = Context.Get<User>(ContextKeys.USER);
            var VisaApplication = await ApiClient.GetVisaApplication(Officer.google_id);

            if (VisaApplication != null)
            {
                Context.Add("Current Visa Application", VisaApplication);
                PageManager.NavigateTo(pageName);
            }
            else
            {
                MessageBox.Show("There are no Visa Applications for you at the moment, please try again later.");
            }
        }
    }
}
