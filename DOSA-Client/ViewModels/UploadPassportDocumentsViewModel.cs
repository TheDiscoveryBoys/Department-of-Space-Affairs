using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using DOSA_Client.lib;
using DOSA_Client.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace DOSA_Client.ViewModels
{
    public class UploadPassportDocumentsViewModel : ScreenViewModelBase
    {
        public string Title => "Upload Passport Documents Page";
        public ObservableCollection<string> UploadedDocuments { get; } = new ObservableCollection<string>();
        public ICommand submitDocumentsCommand { get; }
        public ICommand uploadDocumentCommand { get; }
        public ICommand RemoveDocumentCommand { get; }

        Func<Task> UpdateTabsCallback;

        public UploadPassportDocumentsViewModel(PageManager pageManager, Func<Task> updateTabsCallback)
        {
            PageManager = pageManager;
            submitDocumentsCommand = new RelayCommand(OnSubmitDocuments);
            uploadDocumentCommand = new RelayCommand(OnUploadDocument);
            RemoveDocumentCommand = new DelegateCommand<string>(OnRemoveDocument);
            UpdateTabsCallback = updateTabsCallback;
        }

        public PageManager PageManager { get; set; }

        public void OnSubmitDocuments()
        {
            Task.Run(async ()=>{
                RestClient.DynStatus = "PENDING"; // mimic changing the application status to pending
                await ApiClient.UploadFiles([.. UploadedDocuments]);
                await UpdateTabsCallback();
            });
            Console.WriteLine("Documents submitted successfully.");
        }

        public void OnUploadDocument()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files (*.*)|*.*"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    // For now, just store the file name
                    UploadedDocuments.Add(Path.GetFileName(file));
                }
            }
        }

        public void OnRemoveDocument(string documentName)
        {
            Console.WriteLine(documentName);
            if (UploadedDocuments.Contains(documentName))
            {
                Console.WriteLine("removing");
                UploadedDocuments.Remove(documentName);
            }
        }
    }
}
