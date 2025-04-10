﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib;
using DOSA_Client.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Xaml;

namespace DOSA_Client.ViewModels
{
    class VisaApplicationViewModel : ScreenViewModelBase
    {
        public string Title => "Visa Application";
        public PageManager PageManager { get; set; }
        public ICommand OnSubmitVisaCommand { get; }
        public string DestinationPlanet { get; set; }
        public string TravelReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        Func<Task> UpdateTabsCallback;

        public VisaApplicationViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            PageManager = pageManager;

            OnSubmitVisaCommand = new RelayCommand(OnSubmitVisa);
            UpdateTabsCallback = updateTabsCallback;
        }

        public void OnSubmitVisa()
        {
            Console.WriteLine("Submitting visa!");
            Task.Run(async () =>
            {
                var visaApplication = await ApiClient.CreateVisaApplication(new VisaApplication(null, Context.Get<User>("User").google_id, null, DestinationPlanet, TravelReason, StartDate, EndDate, DateTime.Now, null, null) ?? throw new Exception("Failed to create a visa application"));
                await UpdateTabsCallback();
            });
        }

    }
}

