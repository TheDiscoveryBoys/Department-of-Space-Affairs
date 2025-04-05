using Newtonsoft.Json;

public class LoginPostBody{
    public String GoogleAuthCode {get; set;}
}

public class LoginResponse{
    public String Token {get; set;}
}

public class GoogleTokenExchangeResponse{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonProperty("scope")]
    public string Scope { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    public string id_token{ get; set; }
}
