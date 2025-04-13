using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOSA_Client.Models
{
    public record UserRole(
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonPropertyName("userId")] string UserId,
    [property: JsonPropertyName("roleId")] int? RoleId
);
}
