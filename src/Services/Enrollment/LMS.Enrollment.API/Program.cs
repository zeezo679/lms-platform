using LMS.Contracts.Events;
using LMS.Enrollment.API.Middlewares;
using LMS.Enrollment.Application.Dependencies;
using LMS.Enrollment.Application.IntegrationEventHandlers;
using LMS.Enrollment.Infrastructure.Dependencies;
using LMS.EventBus.Abstractions;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using LMS.EventBus.Extensions;

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

// --- Event Bus Configuration ---
builder.Services.AddEventBus(builder.Configuration);

var app = builder.Build();

// --- 2. Event Bus Subscriptions ---
var eventBusSubscriptionsManager = app.Services.GetRequiredService<IEventBusSubscriptionsManager>();

#region Event Bus Subscriptions 
eventBusSubscriptionsManager.AddSubscription<CourseDeletedEvent, CourseDeletedEventHandler>();
// eventBusSubscriptionsManager.AddSubscription<UserDeletedIntegrationEvent, UserDeletedEventHandler>();
#endregion

// --- 3. الـ Middleware Pipeline ---
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