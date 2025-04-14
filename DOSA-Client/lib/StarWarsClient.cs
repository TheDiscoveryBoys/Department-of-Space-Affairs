using System.Net.Http;
using System.Net.Http.Json;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;

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
                return GetFallbackData(endpoint);
            }
        }

        return [.. allItems.OrderBy(item => item.Name)];
    }

    private static List<SwapiRecord> GetFallbackData(string endpoint)
    {
        return endpoint switch
        {
            "species" => new List<SwapiRecord>
        {
            new SwapiRecord("Human"),
            new SwapiRecord("Droid"),
            new SwapiRecord("Wookiee"),
        },
            "planets" => new List<SwapiRecord>
        {
            new SwapiRecord("Tatooine"),
            new SwapiRecord("Alderaan"),
            new SwapiRecord("Hoth"),
        },
            _ => new List<SwapiRecord>()
        };
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
