using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//GetcurrentPath içerisinde configuration klasörü içierisinde yer alan ocelot.json'ý oku.
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Configurations/ocelot.json");


builder.Services.AddOcelot().AddConsul();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseOcelot().Wait();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
