using IdentityServer.Application.Services;
using IdentityService.Api.Extensions.Registration;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IIdentityService, IdentityServer.Application.Services.IdentityService>();

//Configure Consul
builder.Services.ConfigureConsul(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//Console.WriteLine($"Urls from Program.cs before app.StartAsync(): {string.Join(", ", app.Urls)}");

//await app.StartAsync();

//Console.WriteLine($"Urls from Program.cs after app.StartAsync(): {string.Join(", ", app.Urls)}");
//var appUrl = string.Join(", ", app.Urls);
//await app.WaitForShutdownAsync();


app.Start(); // RegisterWithConsul içerisinde host ve port deðerlerini almak için eklendi
//Application Register to consul.
app.RegisterWithConsul(app.Lifetime);
app.WaitForShutdown(); // RegisterWithConsul içerisinde host ve port deðerlerini almak için eklendi





app.Run();


 