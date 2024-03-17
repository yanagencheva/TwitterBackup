namespace TwitterBackup.Services.DataModels
{
    public class LocalDBTweetsDetails
    {
        public int TweetId { get; set; }
        public int TwitterUserId { get; set; }
        public string BrowsingTweetId { get; set; }
        public string TwitterUserName { get; set; }
        public string Text { get; set; }
        public bool Favourite { get; set; }
        public string Date { get; set; }
        public string AddedByUserId { get; set; }
    }
}
