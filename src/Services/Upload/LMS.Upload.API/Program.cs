using LMS.Common.Extensions;
using LMS.Common.Security;
using LMS.EventBus.Extensions;
using LMS.Upload.Application.Commands.UploadFile;
using LMS.Upload.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Infrastructure (DbContext, StorageProvider, StorageOptions)
builder.Services.AddInfrastructure(builder.Configuration);

// MediatR (scan Application assembly for handlers)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<UploadFileCommand>());

// Gateway authentication (trusts X-User-Id / X-User-Email / X-User-Role headers from YARP)
builder.Services.AddGatewayAuthentication();

// Global exception handler
builder.Services.AddGlobalExceptionHandler();

// Kafka event bus (producer only — this service publishes FileUploadedIntegrationEvent)
builder.Services.AddEventBusProducer(builder.Configuration);

// Swagger
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

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
