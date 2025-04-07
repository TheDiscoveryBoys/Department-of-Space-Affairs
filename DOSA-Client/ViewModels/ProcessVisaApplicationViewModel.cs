using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.lib;
using DOSA_Client.Models;

namespace DOSA_Client.ViewModels
{
    public class ProcessVisaApplicationViewModel : ScreenViewModelBase
    {
        public string Title => "Process User Visa Application";

        public PageManager PageManager { get; set; }

        public ProcessVisaApplicationViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
        }
    }
}
