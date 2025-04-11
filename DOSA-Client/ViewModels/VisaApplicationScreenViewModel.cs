using DOSA_Client.lib;

namespace DOSA_Client.ViewModels
{
    class VisaApplicationScreenViewModel : ScreenViewModelBase
    {
        public string Title => "Visa Application";
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