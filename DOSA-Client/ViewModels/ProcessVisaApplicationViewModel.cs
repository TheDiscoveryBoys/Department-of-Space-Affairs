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

        VisaApplication VisaApplication { get; set; }

        User Officer { get; set; }

        public ProcessVisaApplicationViewModel()
        {
            Task.Run(async ()=>{
            Officer = await ApiClient.GetUserProfile("1");
            });
        }

        public ProcessVisaApplicationViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            Task.Run(async () =>{
                Officer = await ApiClient.GetUserProfile("1");
            });
        }

        public void  Load(bool visibility)
        {
            // make API call
            Task.Run(async () => {
                VisaApplication = await ApiClient.GetVisaApplication(Officer.GoogleId);
            });
        }
    }
}
