using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using DOSA_Client.lib;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class ContainerViewModel : ScreenViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<ScreenViewModelBase> _tabs;
        public ObservableCollection<ScreenViewModelBase> Tabs
        {
            get => _tabs;
            set
            {
                _tabs = value;
                OnPropertyChanged(nameof(Tabs));
            }
        }

        public int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex; set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        public async Task UpdateTabsAsync()
        {
            // this function checks the applications for the current user and decides what tabs to show
            User CurrentUser = Context.Get<User>(ContextKeys.USER);
            var roles = await ApiClient.GetRoles(CurrentUser.google_id);
            if (roles.Any(role => role.role == "APPLICANT"))
            {
                // we have an applicant on our hands so let us check which applications they have right now
                List<Application> applications = await ApiClient.GetApplications(CurrentUser.google_id);
                if (applications.Any(application => application.Status.Name == "APPROVED" && application.ApplicationType == "PASSPORT"))
                {
                    // we have someone who has a passport so they can see their history and the visa application page
                    Tabs = new ObservableCollection<ScreenViewModelBase>(){
                    new VisaApplicationScreenViewModel(() => this.UpdateTabsAsync()),
                    new ApplicationHistoryScreenViewModel()
                    };
                }
                else if (applications.Any(application => application.Status.Name == "PENDING" && application.ApplicationType == "PASSPORT"))
                {
                    // we have someone with a currently open application for a passport so they can only see their history
                    Tabs = new ObservableCollection<ScreenViewModelBase>(){
                    new ApplicationHistoryScreenViewModel()
                    };
                }
                else if (applications.Any(application => application.Status.Name == "REJECTED" && application.ApplicationType == "PASSPORT"))
                {
                    // We have someone that has a recently rejected passport application
                    Tabs = new ObservableCollection<ScreenViewModelBase>(){
                        new PassportApplicationScreenViewModel(() => this.UpdateTabsAsync()),
                        new ApplicationHistoryScreenViewModel()
                    };
                }
                else
                {
                    // we have someone who does not have a passport and does not have any current applications for a passport
                    // so we show them the passport applications tab only
                    Tabs = new ObservableCollection<ScreenViewModelBase>(){
                        new PassportApplicationScreenViewModel(() => this.UpdateTabsAsync())
                    };
                }
            }
            else
            {
                Tabs = new ObservableCollection<ScreenViewModelBase>(){
                    new ProcessPassportApplicationsScreenViewModel(),
                    new ProcessVisaApplicationsScreenViewModel()
                };
            }

            // Delay setting the selected index to ensure the UI has time to bind

            // Force the SelectedIndex update to happen *after* the UI re-renders the new Tabs
            await Task.Delay(50); // wait a tiny bit to ensure binding completes
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                SelectedIndex = 0;
            }, DispatcherPriority.Background);
        }

        public void OnVisibilityChanged(bool visibility)
        {
            if (visibility && Context.Contains("User"))
            {
                // the container is now visible after the login flow and the user was successfully logged in so we
                // can try to update the tabs here
                UpdateTabsAsync();
            }

        }
        public ContainerViewModel()
        {
            if (Context.Contains("User"))
            {
                // we have a logged in user
                UpdateTabsAsync();
            }
            Tabs = new ObservableCollection<ScreenViewModelBase>() { };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}