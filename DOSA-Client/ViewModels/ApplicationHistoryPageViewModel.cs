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
    class ApplicationHistoryPageViewModel : ScreenViewModelBase
    {
        public string Title => "List of Previous Passport and Visa Applications";
        public PageManager PageManager { get; set; }
        public ObservableCollection<Application> Applications { get; set; }

        public ApplicationHistoryPageViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            onNextButtonClickedCommand = new DelegateCommand<string>(OnNext);

            // Call API to get list of passport applications and their statuses
            Applications = new ObservableCollection<Application>(ApiClient.GetApplications("1").GetAwaiter().GetResult());
        }
        public ICommand onNextButtonClickedCommand { get; }
        public void OnNext(String pageName)
        {
            PageManager.NavigateTo(pageName);
        }
    }
}
