using CommunityToolkit.Mvvm.Input;
using DOSA_Client.lib;
using DOSA_Client.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DOSA_Client.ViewModels
{
    public class ProcessPassportApplicationViewModel : ScreenViewModelBase, INotifyPropertyChanged
    {
        public string Title => "Process Passport Application";
        public ICommand RejectCommand { get; }
        public ICommand ApproveCommand { get; }
        public ICommand DownloadDocumentCommand { get; }
        public PageManager PageManager { get; set; }
        public User Officer { get; set; }

        private PassportApplication _passportApplication;
        public PassportApplication PassportApplication
        {
            get => _passportApplication;
            set
            {
                _passportApplication = value;
                OnPropertyChanged(nameof(PassportApplication));
            }
        }

        public ProcessPassportApplicationViewModel(PageManager pageManager)
        {
            PageManager = pageManager;

            RejectCommand = new RelayCommand(RejectApplication);
            ApproveCommand = new RelayCommand(ApproveApplication);
            //DownloadDocumentCommand = new RelayCommand<PassportDocument>(DownloadDocument);

            Task.Run(async () =>
            {
                Officer = await ApiClient.GetUserProfile("1");
                await LoadApplication();
            });
        }

        private async Task LoadApplication()
        {
            //PassportApplication = await ApiClient.GetPassportApplication(Officer.GoogleId);
        }

        public void RejectApplication()
        {
            Console.WriteLine("Passport Application Rejected");
            // Call API here
        }

        public void ApproveApplication()
        {
            Console.WriteLine("Passport Application Approved");
            // Call API here
        }

        //private async void DownloadDocument(PassportDocument doc)
        //{
        //    if (doc == null) return;
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
