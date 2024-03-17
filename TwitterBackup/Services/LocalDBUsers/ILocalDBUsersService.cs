using System.Collections.Generic;
using TwitterBackup.Services.DataModels;

namespace TwitterBackup.Services.LocalDBUsers
{
    public interface ILocalDBUsersService
    {
        void Add(CurrentUserDetails currentUser, TwitterUserDetails twuser);
        LocalDBUserDetails Get(int id);
        void Edit(LocalDBUserDetails user);
        void Delete(int id);
        IEnumerable<LocalDBUserDetails> GetAll(CurrentUserDetails currentUser);
        bool HasUserInLocalDB(string userName, CurrentUserDetails currentUser);
    }
}