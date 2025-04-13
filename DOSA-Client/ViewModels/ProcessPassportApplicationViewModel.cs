using CommunityToolkit.Mvvm.Input;
using DOSA_Client.lib;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Runtime.InteropServices;
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
        private Func<Task> _updateTabsCallback;
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

        public ProcessPassportApplicationViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            PageManager = pageManager;
            _updateTabsCallback = updateTabsCallback;

            RejectCommand = new RelayCommand(RejectApplication);
            ApproveCommand = new RelayCommand(ApproveApplication);
            DownloadDocumentCommand = new RelayCommand<ApplicationDocument>(DownloadDocumentAsync);

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
                    PassportApplication = Context.Get<OfficerPassportApplication>(ContextKeys.CURRENT_PASSPORT_APPLICATION);
                    if (PassportApplication != null)
                    {
                        // assign application to current officer
                        var passport = new PassportApplication(PassportApplication.PassportApplication.Id, PassportApplication.Applicant.google_id, PassportApplication.PassportApplication.StatusId, PassportApplication.PassportApplication.SubmittedAt, null, Officer.google_id);
                        await RestClient.UpdatePassportApplication(passport);
                    }
                });
            }
        }

        public void RejectApplication()
        {
            Task.Run(async ()=>{
                // using records makes this very shit (I think we should have stuck to classes imo)
                var status = new Status(PassportApplication.PassportApplication.StatusId, "REJECTED", Reason);
                var passport = new PassportApplication(PassportApplication.PassportApplication.Id, PassportApplication.Applicant.google_id, status.Id, PassportApplication.PassportApplication.SubmittedAt, DateTime.Now, Officer.google_id);
                await ApiClient.ProcessPassportApplication(passport, status); 
                Reason = "";
                PassportApplication = null;
            });
            PageManager.NavigateTo("Passprot Application Details Page");
            _updateTabsCallback();
        }

        public void ApproveApplication()
        {
            Task.Run(async ()=>{
                var status = new Status(PassportApplication.PassportApplication.StatusId, "APPROVED", Reason);
                var passport = new PassportApplication(PassportApplication.PassportApplication.Id, PassportApplication.Applicant.google_id, status.Id, PassportApplication.PassportApplication.SubmittedAt, DateTime.Now, Officer.google_id);
                await ApiClient.ProcessPassportApplication(passport, status);
                Reason = "";
                PassportApplication = null;
            });
            PageManager.NavigateTo("Passprot Application Details Page");
            _updateTabsCallback();
        }

        private async void DownloadDocumentAsync(ApplicationDocument doc)
        {
           if (doc == null) return; 
           string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
           string filePath = Path.Combine(downloadsPath, doc.FileName);
           await DocumentHelpers.DownloadFileAsync(doc.S3Url, filePath);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
