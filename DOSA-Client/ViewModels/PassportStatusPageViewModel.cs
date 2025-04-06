using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DOSA_Client.lib;
using DOSA_Client.Models;

namespace DOSA_Client.ViewModels
{
    class PassportStatusPageViewModel : ScreenViewModelBase
    {
        public string Title => "Passport status details page";
        public PageManager PageManager { get; set; }
        public ObservableCollection<PassportApplication> PassportApplications { get; set; }

        public PassportStatusPageViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            onNextButtonClickedCommand = new DelegateCommand<string>(OnNext);

            // Call API to get list of passport applications and their statuses
            PassportApplications = new ObservableCollection<PassportApplication>(ApiClient.getPassportApplications(Context.UserGoogleId));
        }
        public ICommand onNextButtonClickedCommand { get; }
        public void OnNext(String pageName)
        {
            PageManager.NavigateTo(pageName);
        }
    }
}
