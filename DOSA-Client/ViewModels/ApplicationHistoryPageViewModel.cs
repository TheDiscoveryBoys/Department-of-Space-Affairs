using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
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
        private Func<Task> _updateTabsCallback;
        public ObservableCollection<Application> Applications { 
            get => _applications;
        
            set{
                _applications = value;
                OnPropertyChanged(nameof(Applications));
            }
        }

        public ApplicationHistoryPageViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            PageManager = pageManager;
            _updateTabsCallback = updateTabsCallback;

            onNextButtonClickedCommand = new DelegateCommand<string>(OnNext);
            RefreshCommand = new RelayCommand(RefreshHistory);
            // Call API to get list of passport applications and their statuses
            Task.Run(async () => {
                Applications = new ObservableCollection<Application>(await ApiClient.GetApplications(Context.Get<User>(ContextKeys.USER).google_id));
            });
        }
        public ICommand onNextButtonClickedCommand { get; }
        public ICommand RefreshCommand { get; }
        public void OnNext(String pageName)
        {
            PageManager.NavigateTo(pageName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void RefreshHistory()
        {
            _updateTabsCallback();
        }
    }
}
