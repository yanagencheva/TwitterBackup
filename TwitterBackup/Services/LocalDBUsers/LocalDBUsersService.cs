using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TwitterBackup.Data;
using TwitterBackup.Data.Entities;
using TwitterBackup.Services.DataModels;

namespace TwitterBackup.Services.LocalDBUsers
{
    public class LocalDBUsersService : ILocalDBUsersService
    {
        private readonly ApplicationDbContext _context;

        public LocalDBUsersService(ApplicationDbContext context)
        {
            _context = context;
        }
        void ILocalDBUsersService.Add(CurrentUserDetails currentUser, TwitterUserDetails twuser)
        {
            var hasuser = _context.TwitterUser.Where(x => x.UserName == twuser.UserName && x.AddedByUserId == currentUser.UserId).FirstOrDefault();
            
            if (hasuser == null)
            {
                _context.Add(new TwitterUser
                         { 
                            UserName = twuser.UserName, 
                            Name = twuser.Name, 
                            BrowsingTwitterId = twuser.Id, 
                            AddedByUserId = currentUser.UserId 
                          });

                _context.SaveChanges();
            }
        }

        public LocalDBUserDetails Get(int id)
        {
            return _context.TwitterUser
                   .Where(x => x.TwitterUserId == id)
                   .Select(x => new LocalDBUserDetails
                   {
                       UserId = x.TwitterUserId,
                       Name = x.Name,
                       UserName = x.UserName,
                       Avatar = x.Avatar
                   }).FirstOrDefault();
        }

        public void Edit(LocalDBUserDetails user)
        {
            if (user.UserId > 0)
            {
                var twuser = _context.TwitterUser.Where(x => x.TwitterUserId == user.UserId).FirstOrDefault();

                twuser.UserName = user.UserName;
                twuser.Name = user.Name;
                if(user.Avatar != null) twuser.Avatar = user.Avatar;

                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var user = _context.TwitterUser.Include(x => x.Tweets).Where(x => x.TwitterUserId == id).FirstOrDefault();

            _context.Tweet.RemoveRange(user.Tweets);
            _context.TwitterUser.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<LocalDBUserDetails> GetAll(CurrentUserDetails currentUser)
        {
            var result = _context.TwitterUser.ToList();

            if (!currentUser.IsAdmin) result = result.Where(x => x.AddedByUserId == currentUser.UserId).ToList();

            return result.Select(x => new LocalDBUserDetails
                         {
                            UserId = x.TwitterUserId,
                            Name = x.Name,
                            UserName = x.UserName,
                            Avatar = x.Avatar
                          }).ToList();
        }

        public bool HasUserInLocalDB(string userName, CurrentUserDetails currentUser)
        {
            var user = _context.TwitterUser.Where(x => x.UserName == userName && (!currentUser.IsAdmin && x.AddedByUserId == currentUser.UserId || currentUser.IsAdmin)).FirstOrDefault();
            
            if (user != null) return true;
            
            return false;
        }
    }
}
