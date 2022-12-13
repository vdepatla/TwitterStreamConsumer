# TwitterStreamConsumer

Twitter credentials are located in appsettings.json which can be replaced.

The project template is ASP.NET Core WebAPI

The project code has been organized as below

1. BackgroudServices : This contains twitter sample stream which runs as a background service on start up.
2. Controllers : Contains TwitterStreamController which has routes for GET count and GETTop10HashTags
3. DataStores : Contains data access logic for storing tweets.
4. Models

TwitterStreamConsumerTests has unit tests for the controller.
 
