using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DOSA_Client.lib;
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
            // call API to reject application
            Console.WriteLine($"Rejected Application with reason {Reason}");
            Task.Run(async ()=>{
                await ApiClient.UpdateVisaApplicationStatus(new Status("REJECTED", Reason), VisaApplication.VisaApplication.id);
                Reason = "";
                VisaApplication = null;
            });
            PageManager.NavigateTo("Visa Application Details Page");
        }

        public void ApproveApplication()
        {
            // call API to approve application
            Console.WriteLine($"Approved Application {Reason}");
            Task.Run(async ()=>{
                await ApiClient.UpdateVisaApplicationStatus(new Status("APPROVED", Reason), VisaApplication.VisaApplication.id);
                // Need to delete the bound values here so that when we come back they are refreshed
                Reason = "";
                VisaApplication = null;
            });
            PageManager.NavigateTo("Visa Application Details Page");
        }

        public ProcessVisaApplicationViewModel(PageManager pageManager)
        {
            RejectCommand = new RelayCommand(RejectApplication);
            ApproveCommand = new RelayCommand(ApproveApplication);

            PageManager = pageManager;

            Task.Run(async () =>{
                Officer = await ApiClient.GetUserProfile("1");
            });
        }

        public void Load(bool visibility)
        {
            // make API call
            Task.Run(async () => {
                VisaApplication = await ApiClient.GetVisaApplication(Officer.google_id);
                Reason = "";
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
