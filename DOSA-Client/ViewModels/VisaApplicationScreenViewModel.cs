
using DOSA_Client.lib;

namespace DOSA_Client.ViewModels
{
    public class VisaApplicationScreenViewModel : ScreenViewModelBase
    {
        public PageManager PageManager { get; set; }
        public VisaApplicationScreenViewModel(Func<Task> updateTabsCallback)
        {
            PageManager = new PageManager();
            RegisterPages(updateTabsCallback);
            PageManager.NavigateTo(PageNames.VisaApplication);
        }

        public void RegisterPages(Func<Task> updateTabsCallback)
        {
            PageManager.RegisterPage(PageNames.VisaApplication, new VisaApplicationViewModel(PageManager, updateTabsCallback));
        }
    }
}