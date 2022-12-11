namespace TwitterStreamConsumer.Models
{
    public class Tweet
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }

        public string Text { get; set; }

        public List<string> Hashtags { get; set; }

        public Tweet(string id, string authorId, string text, List<string> hashtags)
        {
            Id = id;
            AuthorId = authorId;
            Text = text;
            Hashtags = hashtags;
        }
    }
}
