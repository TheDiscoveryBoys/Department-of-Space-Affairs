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

        public UploadPassportDocumentsViewModel(PageManager pageManager)
        {
            PageManager = pageManager;
            submitDocumentsCommand = new RelayCommand(OnSubmitDocuments);
            uploadDocumentCommand = new RelayCommand(OnUploadDocument);
        }

        public PageManager PageManager { get; set; }

        public void OnSubmitDocuments()
        {
            Console.WriteLine("Documents submitted.");
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
    }
}
