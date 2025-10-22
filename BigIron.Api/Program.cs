using Core.Adapter;
using Core.Processors;
using Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton because there's no need to have multiple instances of that
builder.Services.AddSingleton<IISRCsvReader, ISRCsvReader>();
// Scoped because this needs to be attached to each request context
builder.Services.AddScoped<IISRListProcessor, ISRListProcessor>();
// Scoped because it could have repositories calls or request context stored data.
builder.Services.AddScoped<IISRService, ISRService>();


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

app.Run();

// Make Program class accessible to test projects
public partial class Program { }