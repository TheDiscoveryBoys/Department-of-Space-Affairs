using System;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace IntergalacticPassportAPI.Services 
{
    public interface IGoogleAuthService
    {
        Task<GoogleTokenExchangeResponse> GetJwt(string authCode);
        Dictionary<string, object>? DecodeClaims(string jwtToken);
    }
}

