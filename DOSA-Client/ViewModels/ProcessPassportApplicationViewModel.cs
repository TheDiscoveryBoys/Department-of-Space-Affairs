using CommunityToolkit.Mvvm.Input;
using DOSA_Client.lib;
using DOSA_Client.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DOSA_Client.ViewModels
{
    public class ProcessPassportApplicationViewModel : ScreenViewModelBase, INotifyPropertyChanged
    {
        public string Title => "Passport Application";
        public ICommand RejectCommand { get; }
        public ICommand ApproveCommand { get; }
        public ICommand DownloadDocumentCommand { get; }
        public PageManager PageManager { get; set; }
        public User Officer { get; set; }

        private OfficerPassportApplication _passportApplication;
        public OfficerPassportApplication PassportApplication
        {
            get => _passportApplication;
            set
            {
                _passportApplication = value;
                OnPropertyChanged(nameof(PassportApplication));
            }
        }

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
        public string Reason
        {
            get => _reason;
            set
            {
                _reason = value;
                IsRejectEnabled = string.IsNullOrWhiteSpace(_reason) ? false : true;

                Console.WriteLine($"Enabled: {IsRejectEnabled}");
                Console.WriteLine($"Reason: '{Reason}'");

                OnPropertyChanged(nameof(Reason));
            }
        }

        public ProcessPassportApplicationViewModel(PageManager pageManager)
        {
            PageManager = pageManager;

            RejectCommand = new RelayCommand(RejectApplication);
            ApproveCommand = new RelayCommand(ApproveApplication);
            DownloadDocumentCommand = new RelayCommand<ApplicationDocument>(DownloadDocumentAsync);

            Task.Run(async () =>
            {
                Officer = await ApiClient.GetUserProfile("1");
                Load(true);
            });
        }

         public void Load(bool visibility)
        {
            // make API call
            Task.Run(async () => {
                PassportApplication = await ApiClient.GetPassportApplication(Officer.GoogleId);
                Reason = "";
            });
        }

        // private async Task LoadApplication()
        // {

        //     PassportApplication = await ApiClient.GetPassportApplication(Officer.GoogleId);
        // }

        public void RejectApplication()
        {
            // call API to reject application
            Console.WriteLine($"Rejected Application with reason {Reason}");
            Task.Run(async ()=>{
                await ApiClient.UpdatePassportApplicationStatus(new Status("REJECTED", Reason), PassportApplication.PassportApplication.Id);
                Reason = "";
                PassportApplication = null;
            });
            PageManager.NavigateTo("Passprot Application Details Page");
        }

        public void ApproveApplication()
        {
            Console.WriteLine($"Rejected Application with reason {Reason}");
            Task.Run(async ()=>{
                await ApiClient.UpdatePassportApplicationStatus(new Status("REJECTED", Reason), PassportApplication.PassportApplication.Id);
                Reason = "";
                PassportApplication = null;
            });
            PageManager.NavigateTo("Passprot Application Details Page");;
        }

        private async void DownloadDocumentAsync(ApplicationDocument doc)
        {
           if (doc == null) return; 
           Console.WriteLine("Downloading a file");

           string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
           string filePath = Path.Combine(downloadsPath, "file.pdf");
           await DocumentHelpers.DownloadFileAsync(doc.S3Url, filePath);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
