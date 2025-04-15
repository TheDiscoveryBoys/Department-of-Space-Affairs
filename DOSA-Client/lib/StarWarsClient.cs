using System.Net.Http;
using System.Net.Http.Json;
using DOSA_Client.lib.Constants;
using DOSA_Client.Models;

public static class StarWarsClient
{
    private static HttpClient SwapiHttpClient = new HttpClient(new HttpClientHandler
    {
        // don't even worry about this, its definetely not dangerous
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

    private static async Task<List<SwapiRecord>> GetAllFromEndpoint(string endpoint)
    {
        var allItems = new List<SwapiRecord>();
        string nextUrl = $"{Constants.StarWarsURI}{endpoint}/";

        while (!string.IsNullOrEmpty(nextUrl))
        {
            try
            {
                var response = await SwapiHttpClient.GetFromJsonAsync<SwapiResponse>(nextUrl);
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
            new SwapiRecord("Twi'lek"),
            new SwapiRecord("Rodian"),
            new SwapiRecord("Trandoshan"),
            new SwapiRecord("Mon Calamari"),
            new SwapiRecord("Zabrak"),
            new SwapiRecord("Togruta"),
            new SwapiRecord("Chiss"),
            new SwapiRecord("Ewok"),
            new SwapiRecord("Gamorrean"),
            new SwapiRecord("Hutt"),
            new SwapiRecord("Bothan"),
            new SwapiRecord("Nautolan"),
        }.OrderBy(r => r.Name).ToList(),

        "planets" => new List<SwapiRecord>
        {
            new SwapiRecord("Tatooine"),
            new SwapiRecord("Alderaan"),
            new SwapiRecord("Hoth"),
            new SwapiRecord("Dagobah"),
            new SwapiRecord("Endor"),
            new SwapiRecord("Naboo"),
            new SwapiRecord("Coruscant"),
            new SwapiRecord("Kamino"),
            new SwapiRecord("Geonosis"),
            new SwapiRecord("Mustafar"),
            new SwapiRecord("Jakku"),
            new SwapiRecord("Kashyyyk"),
            new SwapiRecord("Scarif"),
            new SwapiRecord("Bespin"),
            new SwapiRecord("Yavin IV"),
        }.OrderBy(r => r.Name).ToList(),

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
