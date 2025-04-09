public record LoginPostBody(
    string GoogleAuthCode
);

public class LoginResponse{
    public String Token {get; set;}
}

public class GoogleTokenExchangeResponse{
    public string AccessToken { get; set; }

    public int ExpiresIn { get; set; }

    public string RefreshToken { get; set; }

    public string Scope { get; set; }

    public string TokenType { get; set; }

    public string id_token{ get; set; }
}
