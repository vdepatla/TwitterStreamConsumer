using Castle.Core.Logging;
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
            controller = new TwitterStreamController(mockDataStore.Object, logger.Object);
        }


        private readonly List<Tweet> mockTweets = new List<Tweet>(){
            new Tweet("mockId1", "mockAuthor1", "unit test tweet1", null) };

        private readonly List<Tweet> mockTweetsWithHashTags = new List<Tweet>(){
            new Tweet("mockId1", "mockAuthor1", "unit test tweet1", new List<string>() { "#a", "#ab" }),
            new Tweet("mockId2", "mockAuthor2", "unit test tweet2", new List<string>() { "#abc", "#abcd" }),
            new Tweet("mockId3", "mockAuthor3", "unit test tweet3", new List<string>() { "#abcde", "#abcdef" }),
            new Tweet("mockId4", "mockAuthor4", "unit test tweet4", new List<string>() { "#acdefg", "#abcdefgh" }),
            new Tweet("mockId5", "mockAuthor5", "unit test tweet5", new List<string>() { "#acdefghi", "#abcdefghij" }),
            new Tweet("mockId6", "mockAuthor6", "unit test tweet6", new List<string>() { "#a", "#abcd" }),
            new Tweet("mockId7", "mockAuthor7", "unit test tweet7", new List<string>() { "#abc", "#abcd" })
        };

        public List<Tweet> MockTweets => mockTweets;

        [Fact]
        public void GetTweetCountReturnsOK()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Returns(MockTweets);

            //Act
            var result = controller.GetTweetCount();

            //Assert
            //var okResult = Assert.IsType<OkResult>(result);
            //var returnValue = Assert.IsType<int>(okResult.Value);

            //Assert.Equal("One", idea.Name);
        }

        [Fact]
        public void GetTop10HashTags()
        {
            //Arrange
            mockDataStore.Setup(m => m.GetTweets()).Returns(mockTweetsWithHashTags);

            //Act
            var result = controller.GetTop10HashTags();

            //Assert
            //var okResult = Assert.IsType<OkResult>(result);
            //var returnValue = Assert.IsType<int>(okResult.Value);

            //Assert.Equal("One", idea.Name);
        }
    }

}