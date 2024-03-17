using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBackup.Models
{
    public class BrowsingTwitterUsersViewModel
    {
        [Display(Name = "Search by Twitter UserName")]
        public string TwitterUserName { get; set; }

        public IEnumerable<TwitterUserViewModel> Results { get; set; }

        public IEnumerable<TweetsDetailsViewModel> Tweets { get; set; }
    }
}
