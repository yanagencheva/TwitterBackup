namespace TwitterBackup.Services.DataModels
{
    public class TweetDetails
    {
        public int? TweetId { get; set; }
        public int? TwitterUserId { get; set; }
        public string Text { get; set; }
        public string BrowsingTwitterUserName { get; set; }
        public string BrowsingTwitterName { get; set; }
        public string BrowsingTwitterUserId { get; set; }
        public string BrowsingTweetId { get; set; }
        public bool IsIncluded { get; set; }
        public bool Favourite { get; set; }
        public string Date { get; set; }
    }
}
