using System.Collections.Generic;

namespace TwitterBackup.Models
{
    public class TweetsViewModel
    {
        public bool IsUserInclusedToLocalDB { get; set; }

        public IEnumerable<TweetsDetailsViewModel> Results { get; set; }
    }
}
