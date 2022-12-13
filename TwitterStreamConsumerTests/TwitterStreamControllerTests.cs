using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TwitterStreamConsumer.Controllers;
using TwitterStreamConsumer.Datastores;
using TwitterStreamConsumer.Models;

namespace TwitterStreamConsumerTests
{
    public class TwitterStreamControllerTests
    {
        private readonly Mock<ITwitterStreamDataStore> mockDataStore;
        private readonly Mock<ILogger<TwitterStreamController>> logger;
        private readonly TwitterStreamController controller;

        public TwitterStreamControllerTests()
        {
            mockDataStore = new Mock<ITwitterStreamDataStore>();
            logger = new Mock<ILogger<TwitterStreamController>>();
            controller = new TwitterStreamController(mockDataStore.Object, logger.Object, 10);
        }


        private readonly List<Tweet> mockTweets = new List<Tweet>(){
            new Tweet("mockId1", "mockAuthor1", "unit test tweet1", null),
            new Tweet("mockId2", "mockAuthor2", "unit test tweet2", null)};

        private readonly List<Tweet> mockTweetsWithLessThan10RepeatedHashTags = new List<Tweet>(){
            new Tweet("mockId1", "mockAuthor1", "unit test tweet1", new List<string>() { "#a", "#ab" }),
            new Tweet("mockId2", "mockAuthor2", "unit test tweet2", new List<string>() { "#abc", "#abcd" }),
            new Tweet("mockId3", "mockAuthor3", "unit test tweet3", new List<string>() { "#abcde", "#abcdef" }),
            new Tweet("mockId4", "mockAuthor4", "unit test tweet4", new List<string>() { "#abcdefg", "#abcdefgh" }),
            new Tweet("mockId5", "mockAuthor5", "unit test tweet5", new List<string>() { "#abcdefghi", "#abcdefghij" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#a", "#abcd" }),
            new Tweet("mockId7", "mockAuthor7", "unit test tweet7", new List<string>() { "#abc", "#abcd" })
        };

        private readonly List<Tweet> mockTweetsWithGreaterThan10RepeatedHashTags = new List<Tweet>(){
            new Tweet("mockId1", "mockAuthor1", "unit test tweet1", new List<string>() { "#a", "#ab" }),
            new Tweet("mockId2", "mockAuthor2", "unit test tweet2", new List<string>() { "#abc", "#abcd" }),
            new Tweet("mockId3", "mockAuthor3", "unit test tweet3", new List<string>() { "#a", "#ab" }),
            new Tweet("mockId4", "mockAuthor4", "unit test tweet4", new List<string>() { "#abc", "#abcd" }),
            new Tweet("mockId5", "mockAuthor5", "unit test tweet5", new List<string>() { "#abcd", "#abcde" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#abcde", "#abcdef" }),
            new Tweet("mockId7", "mockAuthor7", "unit test tweet7", new List<string>() { "#abcdef", "#abcdefg" }),
            new Tweet("mockId5", "mockAuthor5", "unit test tweet5", new List<string>() { "#abcdefg", "#xxxx" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#rddd", "#abcdefgh" }),
            new Tweet("mockId7", "mockAuthor7", "unit test tweet7", new List<string>() { "#abcdefgh", "#rrrr" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#dddd", "#abcdefgh" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#abcdefghij", "#abcdefghijk" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#abcdefghij", "#abcdefghijk" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#abcdefghilm", "#abcdefghijk" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#abcdefghilm", "#abcdefghijk" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#abcdefghil", "#abcdefghijklmno" }),
        };

        private readonly List<Tweet> lessThan10MockTweets = new List<Tweet>(){
            new Tweet("mockId1", "mockAuthor1", "unit test tweet1", new List<string>() { "#a", "#ab" }),
            new Tweet("mockId2", "mockAuthor2", "unit test tweet2", new List<string>() { "#abc", "#abcd" }),
            new Tweet("mockId3", "mockAuthor3", "unit test tweet3", new List<string>() { "#a", "#ab" })
        };

        private readonly List<Tweet> mockTweetsWithNoRepeatedHashTags = new List<Tweet>(){
            new Tweet("mockId1", "mockAuthor1", "unit test tweet1", new List<string>() { "#a", "#ab" }),
            new Tweet("mockId2", "mockAuthor2", "unit test tweet2", new List<string>() { "#abc", "#abcd" }),
            new Tweet("mockId3", "mockAuthor3", "unit test tweet3", new List<string>() { "#abcde", "#abcdef" })
        };

        public List<Tweet> MockTweets => mockTweets;

        [Fact]
        public void GetTweetCountReturnsOKWithCorrectCount()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Returns(MockTweets);

            //Act
            var result = controller.GetTweetCount();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Value);
        }

        [Fact]
        public void GetTweetCountReturnsInternalServerError()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Throws<Exception>();

            //Act
            var result = controller.GetTweetCount();

            //Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.NotNull(statusCodeResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void GetTop10HashTagsLessThan10RepeatedHashesTags()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Returns(mockTweetsWithLessThan10RepeatedHashTags);
            var top10Tags = new List<string>() { "#abcd", "#a", "#abc", "#ab", "#abcde", "#abcdef", "#abcdefg", "#abcdefgh", "#abcdefghi", "#abcdefghij" };

            //Act
            var result = controller.GetTop10HashTags();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(top10Tags, okResult.Value);
        }

        [Fact]
        public void GetTop10HashTagsMoreThan10RepeatedHashTags()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Returns(mockTweetsWithGreaterThan10RepeatedHashTags);
            var top10Tags = new List<string>() { "#abcdefghijk", "#abcd", "#abcdefgh", "#a", "#ab", "#abc", "#abcde", "#abcdef", "#abcdefg", "#abcdefghij" };

            //Act
            var result = controller.GetTop10HashTags();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(top10Tags, okResult.Value);
        }

        [Fact]
        public void GetTop10HashTagsTweetCountLessThan10()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Returns(lessThan10MockTweets);
            var top10Tags = new List<string>() { "#a", "#ab", "#abc", "#abcd" };

            //Act
            var result = controller.GetTop10HashTags();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(top10Tags, okResult.Value);
        }

        [Fact]
        public void GetTop10HashTagsMockTweetsWithNoRepeatedHashTags()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Returns(mockTweetsWithNoRepeatedHashTags);
            var top10Tags = new List<string>() { "#a", "#ab", "#abc", "#abcd", "#abcde", "#abcdef" };

            //Act
            var result = controller.GetTop10HashTags();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(top10Tags, okResult.Value);
        }

        [Fact]
        public void GetTop10HashTagsEmptyTweets()
        {
            //Act
            var result = controller.GetTop10HashTags();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public void GetTop10HashTagsReturnsInternalServerError()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Throws<Exception>();

            //Act
            var result = controller.GetTop10HashTags();

            //Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.NotNull(statusCodeResult);
        }
    }
 }