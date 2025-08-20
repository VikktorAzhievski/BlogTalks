using BlogTalks.API;
using BlogTalks.API.Middlewares;
using BlogTalks.Application;
using BlogTalks.Infrastructure;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(@"c:\logs\myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHttpClient("EmailSenderApi", client =>

{

    var config = builder.Configuration.GetSection("Services:EmailSenderApi");

    client.BaseAddress = new Uri(config["Url"]);

});


// Dependency Injection
builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

