using System.ComponentModel.DataAnnotations;

namespace TwitterBackup.Models
{
    public class TweetsDetailsViewModel
    {
        public TweetsDetailsViewModel()
        {
            Favourite = false;
        }
        public int TweetId { get; set; }
        public int TwitterUserId { get; set; }

        [Display(Name = "Tweet text")]
        public string Text { get; set; }

        public string BrowsingTweetId { get; set; }

        [Display(Name = "Twitter UserName")]
        public string BrowsingTwitterUserName { get; set; }

        [Display(Name = "Twitter Name")]
        public string BrowsingTwitterName { get; set; }

        public string BrowsingTwitterUserId { get; set; }

        public bool Favourite { get; set; }

        public string Date { get; set; }
    }
}
