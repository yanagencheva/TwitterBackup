using System;

namespace TwitterBackup.Data.Entities
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public int TwitterUserId { get; set; }
        public string BrowsingTweetId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public bool IsFavourite { get; set; }
        public string AddedByUserId { get; set; }
        public TwitterUser TwitterUser { get; set; }
    }
}
