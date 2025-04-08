
using System.Windows.Controls;
using DOSA_Client.lib;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class PassportApplicationScreenViewModel : ScreenViewModelBase
    {
        public string Title => "Passport Application";
        public PageManager PageManager { get; set; }
        public PassportApplicationScreenViewModel(Func<Task> updateTabsCallback){
            PageManager = new PageManager();
            RegisterPages(updateTabsCallback);
            PageManager.NavigateTo(PageNames.UserDetails);
        }

        public void RegisterPages(Func<Task> updateTabsCallback){
            PageManager.RegisterPage(PageNames.UserDetails, new UserDetailsPageViewModel(PageManager));
            PageManager.RegisterPage(PageNames.UploadPassportDocuments, new UploadPassportDocumentsViewModel(PageManager, updateTabsCallback));
        }
    }
}