using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStreamConsumer.Controllers;
using TwitterStreamConsumer.Datastores;

namespace TwitterStreamConsumer.BackgroundServices
{
    public  class TwitterSampleStreamV2 : BackgroundService
    {
        private readonly ITwitterStreamDataStore _twitterStreamDataStore;
        private readonly ITwitterClient _twitterClient;
        private readonly ILogger<TwitterSampleStreamV2> _logger;

        public TwitterSampleStreamV2(ITwitterStreamDataStore twitterStreamDataStore, ITwitterClient twitterClient, ILogger<TwitterSampleStreamV2> logger)
        {
            _twitterStreamDataStore = twitterStreamDataStore;
            _twitterClient = twitterClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var sampleStreamV2 = _twitterClient.StreamsV2.CreateSampleStream();
                    sampleStreamV2.TweetReceived += (sender, args) =>
                    {
                        _twitterStreamDataStore.AddTweet(args.Tweet);

                    };
                    await sampleStreamV2.StartAsync();

                }
                catch (TwitterException tex)
                {
                    _logger.LogError("Twitter exception occurred when starting stream. Exception: {tex}", tex);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Twitter exception occurred when starting stream. Exception: {tex}", ex);
                    throw;
                }
            }
        }
    }
}
