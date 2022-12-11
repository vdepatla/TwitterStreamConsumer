using Microsoft.AspNetCore.Mvc;
using TwitterStreamConsumer.Datastores;

namespace TwitterStreamConsumer.Controllers
{
    [ApiController]
    [Route("twitterstream")]
    public class TwitterStreamController : ControllerBase
    {
        private readonly ITwitterStreamDataStore _twitterStreamDataStore;

        public TwitterStreamController(ITwitterStreamDataStore twitterStreamDataStore)
        {
            _twitterStreamDataStore = twitterStreamDataStore;
        }


        [Route("TweetCount")]
        [HttpGet]
        public int GetTweetCount()
        {
           return _twitterStreamDataStore.GetTweets().Count;
        }

        [Route("Top10HashTags")]
        [HttpGet]
        public IEnumerable<string> GetTop10HashTags()
        {
            var tweets = _twitterStreamDataStore.GetTweets();
            var hashTags = tweets.Where(t => t.Hashtags != null).SelectMany(t => t.Hashtags);

            var groupedHashTags = hashTags.GroupBy(h => h);
            var orderedHashTags = groupedHashTags.OrderByDescending(h => h.Count()).Select(t => t.Key);

            return orderedHashTags.Take(10);
        }
    }
}