using System.Text;
using AuthService.Application.Commands.RegisterUser;
using AuthService.Application.Interfaces;
using Infrastructure.Communication;
using Infrastructure.Data;
using Infrastructure.Security;
using LMS.EventBus.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LMS.Common.Extensions;
using Infrastructure.Extensions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using LMS.EventBus.Kafka;


namespace LMS.Auth.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommand>());

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddEventBusProducer(builder.Configuration);
        
        builder.Services.AddJwtAuthentication(builder.Configuration);

        builder.Services.AddAuthorization();

        var app = builder.Build();


        app.UseHttpsRedirection();

        
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    
}
