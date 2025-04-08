
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
        public PageManager PageManager { get; set; }

        public User CurrentUser {get; set;}
        public UserDetailsPageViewModel(PageManager pageManager){
            PageManager = pageManager;
            onNextButtonClickedCommand = new DelegateCommand<string>(OnNext);
            CurrentUser = Context.Get<User>("User");
        }

        public ICommand onNextButtonClickedCommand {get; }
        public void OnNext(String pageName){
            // First we send an api request to update the state of the user 


            PageManager.NavigateTo(pageName);
        }
    }
}