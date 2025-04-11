using System.ComponentModel;
using System.Net;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using DOSA_Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using DOSA_Client.lib.Constants;
using System.Configuration;
using static DOSA_Client.ViewModels.UploadPassportDocumentsViewModel;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Windows;

    public static class StarWarsClient
    {
        public static HttpClient HttpClient = new HttpClient();

        public static async Task<SpeciesResponse> GetSpecies()
        {
            try
            {
                return await HttpClient.GetFromJsonAsync<SpeciesResponse>($"{Constants.StarWarsURI}species/");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new SpeciesResponse();
            }
        }

        public static async Task<SpeciesResponse> GetPlanets()
        {
            try
            {
                return await HttpClient.GetFromJsonAsync<SpeciesResponse>($"{Constants.StarWarsURI}species/");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new SpeciesResponse();
            }
        }
}
