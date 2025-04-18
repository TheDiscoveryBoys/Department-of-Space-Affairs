﻿using System.ComponentModel;
using System.Reflection.Metadata;
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

        public async void RejectApplication()
        {
            if (Reason.Length > 255)
            {
                Reason = Reason.Substring(0, 255);
                MessageBox.Show("Reason exceeds 255 characters. Input will be truncated.");
            }

            var visa = new VisaApplication(VisaApplication.VisaApplication.Id, VisaApplication.Applicant.google_id, Constants.REJECTED_STATUS, VisaApplication.VisaApplication.DestinationPlanet, VisaApplication.VisaApplication.TravelReasonId, VisaApplication.VisaApplication.StartDate, VisaApplication.VisaApplication.EndDate, VisaApplication.VisaApplication.SubmittedAt, DateTime.Now, Officer.google_id, Reason);

            await ApiClient.ProcessVisaApplication(visa);

            // reset form
            Reason = "";
            VisaApplication = null;

            await _updateTabsCallback();
        }

        public async void ApproveApplication()
        {
            if (Reason.Length > 255)
            {
                Reason = Reason.Substring(0, 255);
                MessageBox.Show("Reason exceeds 255 characters. Input will be truncated.");
            }

            var visa = new VisaApplication(VisaApplication.VisaApplication.Id, VisaApplication.Applicant.google_id, Constants.APPROVED_STATUS, VisaApplication.VisaApplication.DestinationPlanet, VisaApplication.VisaApplication.TravelReasonId, VisaApplication.VisaApplication.StartDate, VisaApplication.VisaApplication.EndDate, VisaApplication.VisaApplication.SubmittedAt, DateTime.Now, Officer.google_id, Reason);

            await ApiClient.ProcessVisaApplication(visa);

            // reset form
            Reason = "";
            VisaApplication = null;

            await _updateTabsCallback();
        }

        public ProcessVisaApplicationViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            RejectCommand = new RelayCommand(RejectApplication);
            ApproveCommand = new RelayCommand(ApproveApplication);

            PageManager = pageManager;
            _updateTabsCallback = updateTabsCallback;

            System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                Officer = await ApiClient.GetUserProfile(Context.Get<User>(ContextKeys.USER).google_id);
            });
        }

        public async void Load(bool visibility)
        {
            // make API call
            if(visibility){
                VisaApplication = Context.Get<OfficerVisaApplication>(ContextKeys.CURRENT_VISA_APPLICATION);
                Reason = "";

                if (VisaApplication != null)
                {
                    // assign the application to the current officer
                    var visa = new VisaApplication(VisaApplication.VisaApplication.Id, VisaApplication.Applicant.google_id, VisaApplication.VisaApplication.StatusId, VisaApplication.VisaApplication.DestinationPlanet, VisaApplication.VisaApplication.TravelReasonId, VisaApplication.VisaApplication.StartDate, VisaApplication.VisaApplication.EndDate, VisaApplication.VisaApplication.SubmittedAt, null, Officer.google_id, VisaApplication.VisaApplication.OfficerComment);
                    await RestClient.UpdateVisaApplication(visa);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
