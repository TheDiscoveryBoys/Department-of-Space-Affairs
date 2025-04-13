using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib;

namespace DOSA_Client.ViewModels
{
    class ManagerScreenViewModel : ScreenViewModelBase
    {
        public string Title => "Administrator";
        public PageManager PageManager { get; set; }

        public ManagerScreenViewModel()
        {
            PageManager = new PageManager();
            RegisterPages();
            PageManager.NavigateTo(PageNames.Manager);
        }
        public void RegisterPages()
        {
            PageManager.RegisterPage(PageNames.Manager, new ManagerPageViewModel(PageManager));
        }
    }
}