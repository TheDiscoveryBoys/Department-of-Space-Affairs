using System.Security.Cryptography;
using System.Text;
using IntergalacticPassportAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Dapper;

var builder = WebApplication.CreateBuilder(args);
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true; // This line ensures that PascalCase prpoperties are correctly mapped to snake case in the DB.

builder.Services.AddControllers();
builder.Services.AddScoped<PassportRepository>();
builder.Services.AddScoped<VisaRepository>();
builder.Services.AddScoped<PassportDocumentRepository>();
builder.Services.AddScoped<StatusRepository>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<RolesRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(async options =>
    {
        options.Audience = "988182050054-vlcub1cr22892gc1e4uesj5d6sa3ji1v.apps.googleusercontent.com"; 

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "https://accounts.google.com", 
            ValidAudience = "988182050054-vlcub1cr22892gc1e4uesj5d6sa3ji1v.apps.googleusercontent.com", 
            IssuerSigningKeys = getGoogleSecurityKeys()
        };
    });
var app = builder.Build();

app.MapControllers();
//app.UseAuthentication();
app.Run();

IEnumerable<SecurityKey> getGoogleSecurityKeys(){
    var jwksUri = "https://www.googleapis.com/oauth2/v3/certs";
    var client = new HttpClient();
    var response = client.GetFromJsonAsync<Jwks>(jwksUri).Result ?? throw new Exception("Google stopped hosting their keys here"); 
    var keys = new List<RsaSecurityKey>();
    
    foreach(var key in response.Keys){
        var rsa = new RSACryptoServiceProvider();
        rsa.ImportParameters(new RSAParameters
        {
            Modulus =  Base64UrlDecode(key.N),
            Exponent = Base64UrlDecode(key.E)
        });
        keys.Add(new RsaSecurityKey(rsa));
    }
    return keys;
}

byte[] Base64UrlDecode(string base64Url)
    {
        string base64 = base64Url
            .Replace('-', '+') 
            .Replace('_', '/'); 
        
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }


