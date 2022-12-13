
using System.Collections.Concurrent;

namespace TwitterStreamConsumer.Datastores
{
    public interface ITwitterStreamDataStore
    {
        bool AddTweet(Tweetinvi.Models.V2.TweetV2 tweet);
        ConcurrentBag<TwitterStreamConsumer.Models.Tweet>? GetTweets();
    }
}
