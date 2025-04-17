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
using static System.Net.Mime.MediaTypeNames;

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

            System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                Officer = await ApiClient.GetUserProfile(Context.Get<User>(ContextKeys.USER).google_id);
            });
        }

         public async void Load(bool visibility)
        {
            // make API call
            if(visibility){
                PassportApplication = Context.Get<OfficerPassportApplication>(ContextKeys.CURRENT_PASSPORT_APPLICATION);

                if (PassportApplication != null)
                {
                    // assign application to current officer
                    var passport = new PassportApplication(PassportApplication.PassportApplication.Id, PassportApplication.Applicant.google_id, PassportApplication.PassportApplication.StatusId, PassportApplication.PassportApplication.SubmittedAt, null, Officer.google_id, PassportApplication.PassportApplication.OfficerComment);
                    await RestClient.UpdatePassportApplication(passport);
                }
            }
        }

        public async void RejectApplication()
        {
            if (Reason.Length > 255)
            {
                Reason = Reason.Substring(0, 255);
                MessageBox.Show("Reason exceeds 255 characters. Input will be truncated.");
            }

            var passport = new PassportApplication(PassportApplication.PassportApplication.Id, PassportApplication.Applicant.google_id, Constants.REJECTED_STATUS , PassportApplication.PassportApplication.SubmittedAt, DateTime.Now, Officer.google_id, Reason);
            await ApiClient.ProcessPassportApplication(passport);

            // reset form
            Reason = "";
            PassportApplication = null;

            await _updateTabsCallback();
        }

        public async void ApproveApplication()
        {
            if (Reason.Length > 255)
            {
                Reason = Reason.Substring(0, 255);
                MessageBox.Show("Reason exceeds 255 characters. Input will be truncated.");
            }
            var passport = new PassportApplication(PassportApplication.PassportApplication.Id, PassportApplication.Applicant.google_id, Constants.APPROVED_STATUS, PassportApplication.PassportApplication.SubmittedAt, DateTime.Now, Officer.google_id, Reason);
            await ApiClient.ProcessPassportApplication(passport);

            // reset form
            Reason = "";
            PassportApplication = null;

            await _updateTabsCallback();
        }

        private async void DownloadDocumentAsync(ApplicationDocument doc)
        {
           if (doc == null) return; 
           string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
           string filePath = Path.Combine(downloadsPath, doc.FileName);
           await DocumentHelpers.DownloadFileAsync(doc.S3Url, filePath);
           MessageBox.Show($"Downloaded file: '{doc.FileName}'. Please navigate to '{filePath}' to view the document.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
