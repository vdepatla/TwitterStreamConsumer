using TwitterStreamConsumer.BackgroundServices;
using TwitterStreamConsumer.Models;
using Microsoft.Extensions.Configuration;
using TwitterStreamConsumer.Datastores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITwitterStreamDataStore, TwitterStreamDatastore>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<TwitterSampleStreamV2>();


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


