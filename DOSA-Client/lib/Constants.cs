using DOSA_Client.Models;

namespace DOSA_Client.lib.Constants{
public static class Constants{
    public const string googleAuthURI = "https://accounts.google.com/o/oauth2/auth?client_id=988182050054-vlcub1cr22892gc1e4uesj5d6sa3ji1v.apps.googleusercontent.com&redirect_uri=http://localhost:3000&response_type=code&scope=openid%20phone%20email%20profile";
    public const string BaseURI = "http://localhost:5000/";
    public const string StarWarsURI = "http://swapi.dev/api/";
    public const int PENDING_STATUS = 1;
    public const int APPROVED_STATUS = 2;
    public const int REJECTED_STATUS = 3;
    }

public static class ContextKeys
    {
        public const string USER = "User";
        public const string CURRENT_PASSPORT_APPLICATION = "Current Passport Application";
        public const string CURRENT_VISA_APPLICATION  = "Current Visa Application";
    }
}
