using TwitterStreamConsumer.Models;

namespace TwitterStreamConsumer.Datastores
{
    public sealed class TwitterStreamDatastore : ITwitterStreamDataStore
    {
        private SynchronizedCollection<Tweet>? tweets;

        public  bool AddTweet(Tweetinvi.Models.V2.TweetV2 tweet)
        {
            if(tweets == null)
            {
                tweets = new SynchronizedCollection<Tweet>();
            }

            if (tweet.Lang == "en")
            {
                var hashTags = (tweet.Entities.Hashtags != null) ? tweet.Entities.Hashtags.Select(h => h.Tag).ToList() : null;
                this.tweets.Add(new Tweet(tweet.Id, tweet.Text, tweet.AuthorId, hashTags));
            }

            return true;
        }

        public SynchronizedCollection<TwitterStreamConsumer.Models.Tweet>? GetTweets()
        {
            return tweets;
        }
    }
}
