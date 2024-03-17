using System.Collections.Generic;
using TwitterBackup.Services.DataModels;

namespace TwitterBackup.Services.LocalDBTweets
{
    public interface ILocalDBTweetsService
    {
        void Add(TweetDetails tweet, CurrentUserDetails currentUser);
        LocalDBTweetsDetails Get(int id);
        void Edit(LocalDBTweetsDetails user);
        void Delete(int id);
        IEnumerable<LocalDBTweetsDetails> GetAll(CurrentUserDetails currentUser);
        IEnumerable<LocalDBTweetsDetails> FilterTweets(TweetFilterModel filter, CurrentUserDetails currentUser);
    }
}
