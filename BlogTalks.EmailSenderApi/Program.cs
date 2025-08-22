using BlogTalks.EmailSenderApi.DTO;
using BlogTalks.EmailSenderApi.Service;
using BlogTalks.EmailSenderApi.Services;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettingseEmailSender>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddSingleton(builder.Configuration
    .GetSection("RabbitMqSettings")
    .Get<RabbitMQSettingseEmailSender>());

builder.Services.AddOpenApi();

builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "EmailSender API v1");
    });
}

app.UseHttpsRedirection();

app.MapPost("/send", async (EmailDto request, IEmailSender emailSender) =>
{
    await emailSender.SendAsync(request);
    return Results.Ok(new { message = "Email sent successfully!" });
});

app.Run();