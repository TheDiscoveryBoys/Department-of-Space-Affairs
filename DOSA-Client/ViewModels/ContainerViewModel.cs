using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DOSA_Client.ViewModels;

namespace DOSA_Client.ViewModels
{
    public class ContainerViewModel : ScreenViewModelBase
    {
        public ObservableCollection<ScreenViewModelBase> Tabs {get;set;}
        public enum Role{
            APPLICANT,
            OFFICER
        }
        // have logic to get the role from the database here 
        Role userRole = Role.APPLICANT;
        public ContainerViewModel()
        {

            if(userRole == Role.APPLICANT){
                Tabs = new ObservableCollection<ScreenViewModelBase>(){
                new PassportApplicationScreenViewModel(),
                new VisaApplicationScreenViewModel()
                };
            }else{
                Tabs = new ObservableCollection<ScreenViewModelBase>(){
                    new ProcessPassportApplicationsScreenViewModel(),
                    new ProcessVisaApplicationsScreenViewModel()
                };
            }

        }
    }
}