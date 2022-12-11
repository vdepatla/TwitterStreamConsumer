using TwitterStreamConsumer.BackgroundServices;
using TwitterStreamConsumer.Datastores;
using Tweetinvi;
using Tweetinvi.Models;
using TwitterStreamConsumer.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITwitterStreamDataStore, TwitterStreamDatastore>();


var section = builder.Configuration.GetSection("TwitterClientConfig");
var twitterClientConfig = section.Get<TwitterClientConfig>();
var appCredentials = new ConsumerOnlyCredentials(twitterClientConfig.ConsumerKey, twitterClientConfig.ConsumerSecret)
{
    BearerToken = twitterClientConfig.BearerToken

};
builder.Services.AddSingleton<ITwitterClient>(t => new TwitterClient(appCredentials));


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


