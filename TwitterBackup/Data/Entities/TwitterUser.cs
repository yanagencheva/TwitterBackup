using System.Collections.Generic;

namespace TwitterBackup.Data.Entities
{
    public class TwitterUser
    {
        public int TwitterUserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string BrowsingTwitterId { get; set; }
        public byte[] Avatar { get; set; }
        public string AddedByUserId { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
    }
}
