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

namespace DOSA_Client.ViewModels
{
    public class VisaApplicationDetailsViewModel : ScreenViewModelBase
    {
        public string Title => "Visa application details page";
        public PageManager PageManager { get; set; }

        public User CurrentUser { get; set; }
        public VisaApplicationDetailsViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            GetNextVisaApplication = new DelegateCommand<string>(OnNext);
            CurrentUser = Context.Get<User>("User");
        }

        public ICommand GetNextVisaApplication { get; }
        public void OnNext(string pageName)
        {
            Console.WriteLine(pageName);
            Task.Run(async () =>
            {
                var Officer = Context.Get<User>("User");
                var VisaApplication = await ApiClient.GetVisaApplication(Officer.google_id);
                if (VisaApplication != null)
                {
                    Context.Add("Current Visa Application", VisaApplication);
                    PageManager.NavigateTo(pageName);
                }else{
                    MessageBox.Show("There are no Visa Applications for you at the moment, please try again later.");
                }
            });
        }
    }
}
