
namespace TwitterStreamConsumer.Datastores
{
    public interface ITwitterStreamDataStore
    {
        bool AddTweet(Tweetinvi.Models.V2.TweetV2 tweet);
        List<TwitterStreamConsumer.Models.Tweet>? GetTweets();
    }
}
