using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DOSA_Client.lib;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;

namespace DOSA_Client.ViewModels
{
    public class ProcessVisaApplicationViewModel : ScreenViewModelBase, INotifyPropertyChanged
    {
        public string Title => "Visa Application";
        public ICommand RejectCommand { get; }
        public ICommand ApproveCommand { get; }
        public PageManager PageManager { get; set; }
        public User Officer { get; set; }

        private bool _isRejectEnabled;

        private Func<Task> _updateTabsCallback;
        public bool IsRejectEnabled
        {
            get => _isRejectEnabled;
            set
            {
                _isRejectEnabled = value;
                OnPropertyChanged(nameof(IsRejectEnabled));
            }
        }

        private string _reason;
        public string Reason {
            get => _reason;
            set{
                _reason = value;
                IsRejectEnabled = string.IsNullOrWhiteSpace(_reason) ? false : true;

                Console.WriteLine($"Enabled: {IsRejectEnabled}");
                Console.WriteLine($"Reason: '{Reason}'");

                OnPropertyChanged(nameof(Reason));
            }
        }

        private OfficerVisaApplication _visaApplication;
        public OfficerVisaApplication VisaApplication
        {
            get => _visaApplication;
            set
            {
                _visaApplication = value;
                OnPropertyChanged(nameof(VisaApplication));
            }
        }

        public void RejectApplication()
        {
            if (Reason.Length > 255)
            {
                Reason = Reason.Substring(0, 255);
                MessageBox.Show("Reason exceeds 255 characters. Input will be truncated.");
            }
            // call API to reject application
            Task.Run(async ()=>{
                // eish, same issue with structs here
                var status = new Status(VisaApplication.VisaApplication.StatusId, "REJECTED", Reason);
                var visa = new VisaApplication(VisaApplication.VisaApplication.Id, VisaApplication.Applicant.google_id, status.Id, VisaApplication.VisaApplication.DestinationPlanet, VisaApplication.VisaApplication.TravelReason, VisaApplication.VisaApplication.StartDate, VisaApplication.VisaApplication.EndDate, VisaApplication.VisaApplication.SubmittedAt, DateTime.Now, Officer.google_id);
                await ApiClient.ProcessVisaApplication(visa, status); 
                Reason = "";
                VisaApplication = null;
            });
            PageManager.NavigateTo("Passprot Application Details Page");
            _updateTabsCallback();
        }

        public void ApproveApplication()
        {
            if (Reason.Length > 255)
            {
                Reason = Reason.Substring(0, 255);
                MessageBox.Show("Reason exceeds 255 characters. Input will be truncated.");
            }
            Task.Run(async ()=>{
                var status = new Status(VisaApplication.VisaApplication.StatusId, "APPROVED", Reason);
                var visa = new VisaApplication(VisaApplication.VisaApplication.Id, VisaApplication.Applicant.google_id, status.Id, VisaApplication.VisaApplication.DestinationPlanet, VisaApplication.VisaApplication.TravelReason, VisaApplication.VisaApplication.StartDate, VisaApplication.VisaApplication.EndDate, VisaApplication.VisaApplication.SubmittedAt, DateTime.Now, Officer.google_id);
                await ApiClient.ProcessVisaApplication(visa, status);
                Reason = "";
                VisaApplication = null;
            });
            PageManager.NavigateTo("Passprot Application Details Page");
            _updateTabsCallback();
        }

        public ProcessVisaApplicationViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            RejectCommand = new RelayCommand(RejectApplication);
            ApproveCommand = new RelayCommand(ApproveApplication);

            PageManager = pageManager;
            _updateTabsCallback = updateTabsCallback;

            Task.Run(async () =>
            {
                Officer = await ApiClient.GetUserProfile(Context.Get<User>(ContextKeys.USER).google_id);
            });
        }

        public void Load(bool visibility)
        {
            // make API call
            if(visibility){
                Task.Run(async () => {
                    VisaApplication = Context.Get<OfficerVisaApplication>(ContextKeys.CURRENT_VISA_APPLICATION);
                    Reason = "";
                    if (VisaApplication != null)
                    {
                        // assign the application to the current officer
                        var visa = new VisaApplication(VisaApplication.VisaApplication.Id, VisaApplication.Applicant.google_id, VisaApplication.VisaApplication.StatusId, VisaApplication.VisaApplication.DestinationPlanet, VisaApplication.VisaApplication.TravelReason, VisaApplication.VisaApplication.StartDate, VisaApplication.VisaApplication.EndDate, VisaApplication.VisaApplication.SubmittedAt, null, Officer.google_id);
                        await RestClient.UpdateVisaApplication(visa);
                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
