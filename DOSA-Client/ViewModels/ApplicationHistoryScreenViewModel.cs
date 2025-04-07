using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib;

namespace DOSA_Client.ViewModels
{
    class ApplicationHistoryScreenViewModel: ScreenViewModelBase
    {
        public string Title => "Application History";
        public PageManager PageManager { get; set; }

        public ApplicationHistoryScreenViewModel()
        {
            PageManager = new PageManager();
            RegisterPages();
            PageManager.NavigateTo(PageNames.ApplicationHistory);
        }
        public void RegisterPages()
        {
            PageManager.RegisterPage(PageNames.ApplicationHistory, new ApplicationHistoryPageViewModel(PageManager));
        }
    }
}
