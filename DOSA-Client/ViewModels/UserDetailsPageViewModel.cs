
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using DOSA_Client.lib;
using DOSA_Client.Models;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class UserDetailsPageViewModel : ScreenViewModelBase
    {
        public string Title => "User details page";
        public string Description => "You will see your user details here";
        private ObservableCollection<SwapiRecord> _species;
        public ObservableCollection<SwapiRecord> SpeciesList
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged(nameof(SpeciesList));
            }
        }

        private ObservableCollection<SwapiRecord> _planets;
        public ObservableCollection<SwapiRecord> PlanetsList
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged(nameof(PlanetsList));
            }
        }

        public PageManager PageManager { get; set; }

        public User CurrentUser {get; set;}
        public UserDetailsPageViewModel(PageManager pageManager){
            PageManager = pageManager;
            onNextButtonClickedCommand = new DelegateCommand<string>(OnNext);
            CurrentUser = Context.Get<User>("User");
            LoadSwapiOptions();
        }

        public ICommand onNextButtonClickedCommand {get; }
        public async void OnNext(String pageName){
            // First we send an api request to update the state of the user 
            Console.WriteLine(CurrentUser.species);
            await RestClient.UpdateUser(CurrentUser);

            PageManager.NavigateTo(pageName);
        }

        private void LoadSwapiOptions()
        {
            Task.Run(async () =>
            {
                List<SwapiRecord> species = await StarWarsClient.GetSpecies();
                SpeciesList = new ObservableCollection<SwapiRecord>(species);

                List<SwapiRecord> planets = await StarWarsClient.GetPlanets();
                PlanetsList = new ObservableCollection<SwapiRecord>(planets);
            });
        }
    }
}