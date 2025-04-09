using System.IO;
using System.Net.Http;

namespace DOSA_Client.lib
{
    public static class DocumentHelpers
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task DownloadFileAsync(string fileUrl, string destinationPath)
        {
            try
            {
                using var response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);

                await stream.CopyToAsync(fileStream);
                Console.WriteLine("Download complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }
    }
}