using System.Configuration;
using DOSA_Client.ViewModels;
using System.ComponentModel;

namespace DOSA_Client.lib{
    public class PageManager : INotifyPropertyChanged{

        private List<ScreenViewModelBase> Pages;

        private ScreenViewModelBase _currentPage;
        public ScreenViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));  
                }
            }
        }
        Dictionary<string, int> PageCatalogue; 
        public PageManager(){
            Pages = new List<ScreenViewModelBase>();
            PageCatalogue = new Dictionary<string, int>();
        }

        public void RegisterPage(string name, ScreenViewModelBase page){
            Pages.Add(page);
            PageCatalogue.Add(name, Pages.Count - 1);
        }

        public void NavigateTo(string pageName){
            CurrentPage = Pages[PageCatalogue.GetValueOrDefault(pageName, 0)];

        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}