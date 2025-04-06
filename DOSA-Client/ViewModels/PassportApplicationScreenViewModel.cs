
using System.Windows.Controls;
using DOSA_Client.lib;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class PassportApplicationScreenViewModel : ScreenViewModelBase
    {
        public string Title => "Passport Applications";
        public string Description => "You will see the screen to apply for a passport here";
        public PageManager PageManager { get; set; }
        public PassportApplicationScreenViewModel(){
            PageManager = new PageManager();
            RegisterPages();
            PageManager.NavigateTo("User Details Page");
        }

        public void RegisterPages(){
            PageManager.RegisterPage("User Details Page", new UserDetailsPageViewModel(PageManager));
            PageManager.RegisterPage("Upload Passport Documents Page", new UploadPassportDocumentsViewModel(PageManager));
        }
    }
}