using LMS.Enrollment.API.APIEndpointsHandler;
using LMS.Enrollment.API.Middlewares;
using LMS.Enrollment.Application.Dependencies;
using LMS.Enrollment.Infrastructure.Dependencies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    // Add the StandardizedResponseOperationFilter to the Swagger generation process
    c.OperationFilter<StandardizedResponseOperationFilter>();
});
var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

