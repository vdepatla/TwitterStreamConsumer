using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using TwitterStreamConsumer.Datastores;

namespace TwitterStreamConsumer.Controllers
{
    [ApiController]
    [Route("twitterstream")]
    public class TwitterStreamController : ControllerBase
    {
        private readonly ITwitterStreamDataStore _twitterStreamDataStore;
        private readonly ILogger<TwitterStreamController> _logger;

        public TwitterStreamController(ITwitterStreamDataStore twitterStreamDataStore, ILogger<TwitterStreamController> logger)
        {
            _twitterStreamDataStore = twitterStreamDataStore;
            _logger = logger;
        }

        [Route("TweetCount")]
        [HttpGet]
        public ActionResult<int> GetTweetCount()
        {
            int tweetCount = 0;

            try
            {
                var tweets = _twitterStreamDataStore.GetTweets();
                if(tweets != null)
                {
                    tweetCount = tweets.Count;
                }
                else
                {
                    _logger.LogInformation("No tweets received from data store.");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred in getting tweet informtion. Exception: {ex}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }                

            return Ok(tweetCount);

        }

        [Route("Top10HashTags")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTop10HashTags()
        {
            IEnumerable<string>? result = null;

            try
            {
                var tweets = _twitterStreamDataStore.GetTweets();

                if(tweets == null)
                {
                    _logger.LogInformation("No tweets received from data store.");
                    return Ok(result);
                }

                var hashTags = tweets.Where(t => t.Hashtags != null).SelectMany(t => t.Hashtags);

                if(hashTags != null)
                {
                    var groupedHashTags = hashTags.GroupBy(h => h);
                    result = groupedHashTags.OrderByDescending(h => h.Count()).Take(10).Select(t => t.Key);
                }
                else
                {
                    _logger.LogInformation("No hash tags received from data store.");
                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred in getting hash tag informtion. Exception: {ex}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(result);
        }
    }
}