
using System.Windows.Input;
using DOSA_Client.lib;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class UploadPassportDocumentsViewModel : ScreenViewModelBase
    {
        public string Title => "Upload Passport Documents Page ";
        public string Description => "You will see upload passport document screen here";
        public PageManager PageManager { get; set; } 
        public ICommand OnNextButtonClickedCommand {get; }
        public UploadPassportDocumentsViewModel( PageManager pageManager){
            PageManager = pageManager;
            OnNextButtonClickedCommand = new DelegateCommand<string>(OnNext);
        }
        public void OnNext(String pageName){
            PageManager.NavigateTo(pageName);
        }
    }
}