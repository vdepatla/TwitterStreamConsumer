
namespace TwitterStreamConsumer.Datastores
{
    public interface ITwitterStreamDataStore
    {
        bool AddTweet(Tweetinvi.Models.V2.TweetV2 tweet);
        SynchronizedCollection<TwitterStreamConsumer.Models.Tweet>? GetTweets();
    }
}
