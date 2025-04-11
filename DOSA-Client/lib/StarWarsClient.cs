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
    private static HttpClient HttpClient = new HttpClient();
    private static async Task<List<SwapiRecord>> GetAllFromEndpoint(string endpoint)
    {
        var allItems = new List<SwapiRecord>();
        string nextUrl = $"{Constants.StarWarsURI}{endpoint}/";

        while (!string.IsNullOrEmpty(nextUrl))
        {
            try
            {
                var response = await HttpClient.GetFromJsonAsync<SwapiResponse>(nextUrl);
                if (response?.Results != null)
                {
                    allItems.AddRange(response.Results);
                    nextUrl = response.Next;
                }
                else
                {
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching {endpoint}: {e.Message}");
                break;
            }
        }

        return [.. allItems.OrderBy(item => item.Name)];
    }

    public static Task<List<SwapiRecord>> GetSpecies()
    {
        return GetAllFromEndpoint("species");
    }

    public static Task<List<SwapiRecord>> GetPlanets()
    {
        return GetAllFromEndpoint("planets");
    }
}
