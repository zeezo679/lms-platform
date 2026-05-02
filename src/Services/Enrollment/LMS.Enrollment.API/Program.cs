using LMS.Enrollment.API.APIEndpointsHandler;
using LMS.Enrollment.API.Middlewares;
using LMS.Enrollment.Application.Dependencies;
using LMS.Enrollment.Infrastructure.Dependencies;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. (Services) ---
// Controllers & JSON Options & Api Behavior
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Convert enums to strings in JSON responses for better readability
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // السطر ده بيقفل الفلتر الرخم بتاع مايكروسوفت، وبيسيب الريكويست يعدي للـ Pipeline بتاعنا
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddAuthentication();
builder.Services.AddAuthorization(); 

builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<StandardizedResponseOperationFilter>();
});

var app = builder.Build();

// --- 2. الـ Middleware Pipeline ---
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("LMS Enrollment API")
               .WithTheme(ScalarTheme.DeepSpace) // ثيم شكله شيك جداً
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();