using System.Security.Cryptography;
using System.Text;
using IntergalacticPassportAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Swashbuckle.AspNetCore;
using IntergalacticPassportAPI.Data;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true; // This line ensures that PascalCase prpoperties are correctly mapped to snake case in the DB.

builder.Services.AddControllers();
builder.Services.AddScoped<PassportRepository>();
builder.Services.AddScoped<VisaRepository>();
builder.Services.AddScoped<PassportDocumentRepository>();
builder.Services.AddScoped<StatusRepository>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<RolesRepository>();
builder.Services.AddScoped<VisaRepository>();
builder.Services.AddScoped<PassportDocumentRepository>();
builder.Services.AddScoped<StatusRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://accounts.google.com",
            ValidateLifetime = false,
            ValidAudience = "988182050054-vlcub1cr22892gc1e4uesj5d6sa3ji1v.apps.googleusercontent.com",
            ValidateAudience = true,
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                Console.WriteLine("--- All Claims ---");
                foreach (var claim in context.Principal.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
                Console.WriteLine("------------------");

                var googleId = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (googleId == null)
                {
                    return;
                }
                var userRepository = context.HttpContext.RequestServices.GetRequiredService<UsersRepository>();

                var userRoles = await userRepository.GetUserRoles(googleId);

                var identity = context.Principal.Identity as ClaimsIdentity;
                foreach (var role in userRoles)
                {
                    identity?.AddClaim(new Claim(ClaimTypes.Role, role.Role));  
                }

                await Task.CompletedTask;
            }
        };
    });

// TODO: Come readd this 
 // builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Intergalactic Passport API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseAuthentication();
//app.UseAuthorization();
app.Run();



