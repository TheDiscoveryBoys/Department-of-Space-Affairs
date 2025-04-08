using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib;
using DOSA_Client.Models;

namespace DOSA_Client.ViewModels
{
    public class ProcessVisaApplicationViewModel : ScreenViewModelBase, INotifyPropertyChanged
    {
        public string Title => "Process User Visa Application";

        public PageManager PageManager { get; set; }

        public User Officer { get; set; }

        private VisaApplication _visaApplication;
        public VisaApplication VisaApplication
        {
            get => _visaApplication;
            set
            {
                _visaApplication = value;
                OnPropertyChanged(nameof(VisaApplication));
            }
        }

        public ProcessVisaApplicationViewModel()
        {
            Task.Run(async ()=>{
            Officer = await ApiClient.GetUserProfile("1");
            });
        }

        public ProcessVisaApplicationViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            Task.Run(async () =>{
                Officer = await ApiClient.GetUserProfile("1");
            });
        }

        public void  Load(bool visibility)
        {
            // make API call
            Task.Run(async () => {
                VisaApplication = await ApiClient.GetVisaApplication(Officer.GoogleId);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
