using LMS.Gateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1 Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token here"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// --- 2. Swagger 
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("http://localhost:5001/swagger/v1/swagger.json", "Auth Service");
    options.SwaggerEndpoint("http://localhost:5002/swagger/v1/swagger.json", "Course Service");
    options.SwaggerEndpoint("http://localhost:5003/swagger/v1/swagger.json", "Enrollment Service");
    options.SwaggerEndpoint("http://localhost:5004/swagger/v1/swagger.json", "Upload Service");
    options.SwaggerEndpoint("http://localhost:5005/swagger/v1/swagger.json", "User Service");

    options.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseMiddleware<ClaimsToHeadersMiddleware>();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();