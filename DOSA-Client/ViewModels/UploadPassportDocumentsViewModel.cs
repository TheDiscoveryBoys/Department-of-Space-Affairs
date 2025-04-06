
using System.Windows.Input;
using DOSA_Client.lib;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class UploadPassportDocumentsViewModel : ScreenViewModelBase
    {
        public string Title => "Upload Passport Documents Page ";
        public PageManager PageManager { get; set; } 
        public UploadPassportDocumentsViewModel( PageManager pageManager){
            PageManager = pageManager;
            submitDocumentsCommand = new DelegateCommand<string>(OnNext);
        }

        public ICommand submitDocumentsCommand { get; }
        public void OnNext(String pageName){
            Console.WriteLine($"Navigating to {pageName}");
            PageManager.NavigateTo(pageName);
        }
    }
}