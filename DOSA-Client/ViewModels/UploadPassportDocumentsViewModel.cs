using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using DOSA_Client.lib;
using DOSA_Client.ViewModels;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using DOSA_Client.Models;
using DOSA_Client.lib.Constants;

namespace DOSA_Client.ViewModels
{
    public class UploadPassportDocumentsViewModel : ScreenViewModelBase
    {
        public string Title => "Upload Passport Documents Page";
        public ObservableCollection<LocalFile> UploadedDocuments { get; } = new ObservableCollection<LocalFile>();
        public ICommand submitDocumentsCommand { get; }
        public ICommand uploadDocumentCommand { get; }
        public ICommand RemoveDocumentCommand { get; }

        Func<Task> UpdateTabsCallback;

        public record LocalFile(string FileName, string localFilePath);

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
            Task.Run(async () =>
            {
                var passportApplication = await ApiClient.CreatePassportApplication(new PassportApplication(null, Context.Get<User>(ContextKeys.USER).google_id, null, DateTime.Now, null, null)) ?? throw new Exception("Failed to create an application");
                await ApiClient.UploadFiles([.. UploadedDocuments], passportApplication.Id ?? throw new Exception("Something diabolical has occurred"));
                await UpdateTabsCallback();
            });
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
                    UploadedDocuments.Add(new LocalFile(Path.GetFileName(file), file));
                }
            }
        }

        public void OnRemoveDocument(string documentName)
        {

            var documentToRemove = UploadedDocuments.FirstOrDefault(doc => doc.FileName == documentName);

            if (documentToRemove != null)
            {
                UploadedDocuments.Remove(documentToRemove);
            }
            else
            {
                Console.WriteLine("Document not found");
            }
        }
    }
}
