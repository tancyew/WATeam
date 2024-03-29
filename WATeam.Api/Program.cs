using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WhatsappBusiness.CloudApi.Response;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World!");

app.MapGet("/webhook", (
    [FromQuery(Name = "hub.mode")] string hubMode,
    [FromQuery(Name = "hub.challenge")] int hubChallenge,
    [FromQuery(Name = "hub.verify_token")] string hubVerifyToken) =>
{
    Console.WriteLine("Received webhook verification request " + $"(hub.mode={hubMode}, hub.challenge={hubChallenge}, hub.verify_token={hubVerifyToken})");
    return Results.Ok(hubChallenge);
})
.WithName("WebhookVerify")
.WithOpenApi();

app.MapPost("/webhook", ([FromBody] dynamic msg) =>
{
    Console.WriteLine("Received message: " + JsonSerializer.Serialize(msg));
})
.WithName("WebhookMessage")
.WithOpenApi();

app.Run();