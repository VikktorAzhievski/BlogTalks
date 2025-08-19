using BlogTalks.EmailSenderApi.DTO;
using BlogTalks.EmailSenderApi.Service;
using BlogTalks.EmailSenderApi.Services;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddOpenApi();

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
