using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib;
using DOSA_Client.Models;
using System.Windows.Input;

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
            PageManager.NavigateTo(pageName);
        }
    }
}
