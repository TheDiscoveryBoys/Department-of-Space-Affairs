
using System.Windows.Controls;
using DOSA_Client.lib;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class PassportApplicationScreenViewModel : ScreenViewModelBase
    {
        public string Title => "Passport Applications";
        public PageManager PageManager { get; set; }
        public PassportApplicationScreenViewModel(){
            PageManager = new PageManager();
            RegisterPages();
            PageManager.NavigateTo(PageNames.UserDetails);
        }

        public void RegisterPages(){
            PageManager.RegisterPage(PageNames.UserDetails, new UserDetailsPageViewModel(PageManager));
            PageManager.RegisterPage(PageNames.UploadPassportDocuments, new UploadPassportDocumentsViewModel(PageManager));
            PageManager.RegisterPage(PageNames.PassportStatus, new PassportStatusPageViewModel(PageManager));
        }
    }
}