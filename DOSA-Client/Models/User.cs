namespace DOSA_Client.Models
{
    public record User(
        string google_id,
        string email,
        string name,
        string? species = null,
        string? planet_of_origin = null,
        string? primary_language = null,
        DateTime? date_of_birth = null
    );
}

