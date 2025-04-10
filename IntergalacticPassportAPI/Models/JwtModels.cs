// The class representing the entire JWKS object
using Newtonsoft.Json;

public class Jwks
{
    [JsonProperty("keys")]
    public List<Jwk> Keys { get; set; }
}

// The class representing a single JWK (JSON Web Key)
public class Jwk
{
    [JsonProperty("kty")]
    public string Kty { get; set; }

    [JsonProperty("kid")]
    public string Kid { get; set; }

    [JsonProperty("use")]
    public string Use { get; set; }

    [JsonProperty("n")]
    public string N { get; set; }

    [JsonProperty("e")]
    public string E { get; set; }
}