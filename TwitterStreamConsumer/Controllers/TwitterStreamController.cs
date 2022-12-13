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
        private readonly int _topHashTagsCount;

        public TwitterStreamController(ITwitterStreamDataStore twitterStreamDataStore, ILogger<TwitterStreamController> logger, int topHashTagsCount=10)
        {
            _twitterStreamDataStore = twitterStreamDataStore;
            _logger = logger;
            _topHashTagsCount = topHashTagsCount;
        }

        [Route("tweetcount")]
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

        [Route("top10hashtags")]
        [HttpGet]
        public ActionResult<List<string>> GetTop10HashTags()
        {
            List<string>? result = null;

            try
            {
                var tweets = _twitterStreamDataStore.GetTweets();
                if (tweets == null)
                {
                    _logger.LogInformation("No tweets received from data store.");
                    return Ok(result);
                }

                var hashTags = tweets.Where(t => t.Hashtags != null).SelectMany(t => t.Hashtags);

                if(hashTags != null)
                {
                    var groupedHashTags = hashTags.GroupBy(h => h);
                    var orderedHashTags = groupedHashTags.OrderByDescending(h => h.Count()).Select(t => t.Key);
                    result = orderedHashTags.Take(_topHashTagsCount).ToList();

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