using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBackup.Models
{
    public class LocalDBTweetsViewModel
    {
        public LocalDBTweetsViewModel()
        {
            Favourite = false;
        }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Favourite tweets")]
        public bool Favourite { get; set; }

        public IEnumerable<LocalDBTweetDetailsViewModel> Tweets { get; set; }
    }
}
