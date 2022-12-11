using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using TwitterStreamConsumer.Datastores;

namespace TwitterStreamConsumer.BackgroundServices
{
    public  class TwitterSampleStreamV2 : BackgroundService
    {
        private readonly ITwitterStreamDataStore _twitterStreamDataStore;

        public TwitterSampleStreamV2(ITwitterStreamDataStore twitterStreamDataStore)
        {
            _twitterStreamDataStore = twitterStreamDataStore;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var appCredentials = new ConsumerOnlyCredentials("MOaqzlpjn2wCmMKpw0h6YjSn7", "3tD3I6KScbmpQhwVMdZTqhp3V0TGqN5Y8anXdzjc2l2HwcwD9a")
                    {
                        BearerToken = "AAAAAAAAAAAAAAAAAAAAAMUkkAEAAAAA5NOefwkeLBkfm43RbtPz8eViCVg%3Dd54XYvlVyM34umlFd3Wy1OjAE5ATLFsudKJKuUIphegSSDh57K"

                    };

                    var appClient = new TwitterClient(appCredentials);

                    var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
                    sampleStreamV2.TweetReceived += (sender, args) =>
                    {
                        _twitterStreamDataStore.AddTweet(args.Tweet);

                    };
                    await sampleStreamV2.StartAsync();

                }
                catch (TwitterException tex)
                {
                    Console.WriteLine(tex.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
