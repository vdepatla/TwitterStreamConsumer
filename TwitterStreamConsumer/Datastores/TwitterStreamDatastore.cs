using System.Collections.Concurrent;
using TwitterStreamConsumer.Models;

namespace TwitterStreamConsumer.Datastores
{
    public sealed class TwitterStreamDatastore : ITwitterStreamDataStore
    {
        private ConcurrentBag<Tweet>? tweets;

        public  bool AddTweet(Tweetinvi.Models.V2.TweetV2 tweet)
        {
            if(tweets == null)
            {
                tweets = new ConcurrentBag<Tweet>();
            }

            if (tweet.Lang == "en")
            {
                var hashTags = (tweet.Entities.Hashtags != null) ? tweet.Entities.Hashtags.Select(h => h.Tag).ToList() : null;
                this.tweets.Add(new Tweet(tweet.Id, tweet.Text, tweet.AuthorId, hashTags));
            }

            return true;
        }

        public ConcurrentBag<TwitterStreamConsumer.Models.Tweet>? GetTweets()
        {
            return tweets;
        }
    }
}
