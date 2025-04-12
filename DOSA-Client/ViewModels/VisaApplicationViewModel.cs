using DOSA_Client.lib;
using DOSA_Client.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DOSA_Client.lib.Constants;

namespace DOSA_Client.ViewModels
{
    public class VisaApplicationViewModel : ScreenViewModelBase
    {
        public string Title => "Submit Visa Details";
        public PageManager PageManager { get; set; }
        public string DestinationPlanet { get; set; }

        private string _travelReason;
        public string TravelReason
        {
            get => _travelReason;
            set
            {
                _travelReason = value;
                IsSubmitEnabled = string.IsNullOrWhiteSpace(_travelReason) ? false : true;

                Console.WriteLine($"Enabled: {IsSubmitEnabled}");
                Console.WriteLine($"Reason: '{TravelReason}'");

            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private bool _isSubmitEnabled;
        public bool IsSubmitEnabled
        {
            get => _isSubmitEnabled;
            set
            {
                _isSubmitEnabled = value;
                OnPropertyChanged(nameof(IsSubmitEnabled));
            }
        }

        public ICommand OnSubmitVisaCommand { get; }

        private ObservableCollection<SwapiRecord> _planets;
        public ObservableCollection<SwapiRecord> PlanetsList
        {
            get => _planets;
            set
            {
                _planets = value;
                OnPropertyChanged(nameof(PlanetsList));
            }
        }

        Func<Task> UpdateTabsCallback;

        public VisaApplicationViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            PageManager = pageManager;

            OnSubmitVisaCommand = new RelayCommand(OnSubmitVisa);
            UpdateTabsCallback = updateTabsCallback;

            LoadSwapiOptions();

            StartDate = DateTime.Now.AddDays(1);
            EndDate = DateTime.Now.AddDays(7);
        }

        public void OnSubmitVisa()
        {
            Console.WriteLine("Submitting visa!");
            Task.Run(async () =>
            {
                var visaApplication = await ApiClient.CreateVisaApplication(new VisaApplication(null, Context.Get<User>(ContextKeys.USER).google_id, null, DestinationPlanet, TravelReason, StartDate, EndDate, DateTime.Now, null, null) ?? throw new Exception("Failed to create a visa application"));
                await UpdateTabsCallback();
            });
        }

        private void LoadSwapiOptions()
        {
            Task.Run(async () =>
            {
                List<SwapiRecord> planets = await StarWarsClient.GetPlanets();
                PlanetsList = new ObservableCollection<SwapiRecord>(planets);
            });
        }

    }
}

