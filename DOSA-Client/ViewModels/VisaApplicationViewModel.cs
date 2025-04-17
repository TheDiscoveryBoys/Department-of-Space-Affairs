using DOSA_Client.lib;
using DOSA_Client.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DOSA_Client.lib.Constants;
using System.Net.Http;

namespace DOSA_Client.ViewModels
{
    public class VisaApplicationViewModel : ScreenViewModelBase
    {
        public string Title => "Submit Visa Details";
        public PageManager PageManager { get; set; }
        public string DestinationPlanet { get; set; }

        private TravelReason _travelReason;
        public TravelReason TravelReason {
            get => _travelReason;
            set
            {
                _travelReason = value;
                IsSubmitEnabled = true;
            }
        }

        private List<TravelReason> _travelReasons;

        public List<TravelReason> TravelReasons {
            get => _travelReasons;
            set{
                _travelReasons = value;
                OnPropertyChanged(nameof(TravelReasons));
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
            IsSubmitEnabled = false;

            System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                TravelReasons = await ApiClient.GetTravelReasons();
            });
        }

        public async void OnSubmitVisa()
        {
            Console.WriteLine("Submitting visa!");

            try
            {
                var visaApplication = await ApiClient.CreateVisaApplication(new VisaApplication(-1, Context.Get<User>(ContextKeys.USER).google_id, null, DestinationPlanet, TravelReason.Id, StartDate, EndDate, DateTime.Now, null, null, null) ?? throw new Exception("Failed to create a visa application"));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error occurred when applying for VISA. Please try again later", "Error");
            }

            await UpdateTabsCallback();
        }

        private void LoadSwapiOptions()
        {
            System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                List<SwapiRecord> planets = await StarWarsClient.GetPlanets();
                PlanetsList = new ObservableCollection<SwapiRecord>(planets);
            });
        }
    }
}

