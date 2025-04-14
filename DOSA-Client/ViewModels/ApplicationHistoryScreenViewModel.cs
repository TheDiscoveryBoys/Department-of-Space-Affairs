using DOSA_Client.lib;

namespace DOSA_Client.ViewModels
{
    class ApplicationHistoryScreenViewModel: ScreenViewModelBase
    {
        public string Title => "Application History";
        public PageManager PageManager { get; set; }
        private Func<Task> _updateTabsCallback;

        public ApplicationHistoryScreenViewModel(Func<Task> updateTabsCallback)
        {
            PageManager = new PageManager();
            _updateTabsCallback = updateTabsCallback;

            RegisterPages();
            PageManager.NavigateTo(PageNames.ApplicationHistory);

        }
        public void RegisterPages()
        {
            PageManager.RegisterPage(PageNames.ApplicationHistory, new ApplicationHistoryPageViewModel(PageManager, _updateTabsCallback));
        }
    }
}
