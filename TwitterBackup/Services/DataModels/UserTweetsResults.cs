using System.Collections.Generic;

namespace TwitterBackup.Services.DataModels
{
    public class UserTweetsResults
    {
        public string UserName { get; set; }
        public List<JSTweetDetails> data { get; set; }
    }
}
