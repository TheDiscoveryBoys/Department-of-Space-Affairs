using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DOSA_Client.lib;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;

namespace DOSA_Client.ViewModels
{
    class ApplicationHistoryPageViewModel : ScreenViewModelBase, INotifyPropertyChanged
    {
        public string Title => "List of Previous Passport and Visa Applications";
        public PageManager PageManager { get; set; }
        private ObservableCollection<Application> _applications;
        public ObservableCollection<Application> Applications { 
            get => _applications;
        
            set{
                _applications = value;
                OnPropertyChanged(nameof(Applications));
            }
        }

        public ApplicationHistoryPageViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            onNextButtonClickedCommand = new DelegateCommand<string>(OnNext);
            // Call API to get list of passport applications and their statuses
            Task.Run(async ()=>{
                Applications = new ObservableCollection<Application>(await ApiClient.GetApplications(Context.Get<User>(ContextKeys.USER).google_id));
            });
        }
        public ICommand onNextButtonClickedCommand { get; }
        public void OnNext(String pageName)
        {
            PageManager.NavigateTo(pageName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
