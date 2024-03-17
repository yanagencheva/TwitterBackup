using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitterBackup.Data;
using TwitterBackup.Data.Entities;
using TwitterBackup.Services.DataModels;

namespace TwitterBackup.Services.LocalDBTweets
{
    public class LocalDBTweetsService : ILocalDBTweetsService
    {
        private readonly ApplicationDbContext _context;

        public LocalDBTweetsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(TweetDetails tweet, CurrentUserDetails currentUser)
        {
            var twitterUser = _context.TwitterUser.Where(x => x.UserName == tweet.BrowsingTwitterUserName && x.AddedByUserId == currentUser.UserId).FirstOrDefault();
            
            if (twitterUser != null)
            {
                var newTweet = new Tweet
                {
                    Text = tweet.Text,
                    Date = DateTime.Parse(tweet.Date),
                    IsFavourite = tweet.Favourite,
                    AddedByUserId = currentUser.UserId,
                    BrowsingTweetId = tweet.BrowsingTweetId,
                };
                newTweet.TwitterUserId = twitterUser.TwitterUserId;

                _context.Tweet.Add(newTweet);
                _context.SaveChanges();
            }
        }

        public LocalDBTweetsDetails Get(int id)
        {
            return _context.Tweet
                   .Include(x => x.TwitterUser)
                   .Where(x => x.TweetId == id)
                   .Select(x => new LocalDBTweetsDetails
                   {
                       TweetId = x.TweetId,
                       TwitterUserId = x.TwitterUserId,
                       BrowsingTweetId = x.BrowsingTweetId,
                       TwitterUserName = x.TwitterUser.UserName,
                       Favourite = x.IsFavourite,
                       Text = x.Text,
                       Date = x.Date.ToShortDateString()
                   }).FirstOrDefault();
        }

        public void Edit(LocalDBTweetsDetails tweet)
        {
            if (tweet.TweetId > 0)
            {
                var tw = _context.Tweet.Where(x => x.TweetId == tweet.TweetId).FirstOrDefault();
                tw.Text = tweet.Text;
                tw.IsFavourite = tweet.Favourite;

                _context.SaveChanges();
            }
        }
      
        public void Delete(int id)
        {
            if (id > 0)
            {
                var delTweet = _context.Tweet.Where(x => x.TweetId == id).FirstOrDefault();

                _context.Remove(delTweet);
                _context.SaveChanges();
            }
        }

        public IEnumerable<LocalDBTweetsDetails> GetAll(CurrentUserDetails currentUser)
        {
            var result = _context.Tweet.Include(x => x.TwitterUser).ToList();

            if (!currentUser.IsAdmin) result = result.Where(x => x.AddedByUserId == currentUser.UserId).ToList();

            return result.Select(x => new LocalDBTweetsDetails
            {
                TweetId = x.TweetId,                
                BrowsingTweetId = x.BrowsingTweetId,
                TwitterUserId = x.TweetId,
                AddedByUserId = x.AddedByUserId,
                TwitterUserName = x.TwitterUser.UserName,
                Text = x.Text,
                Date = x.Date.ToShortDateString(),
                Favourite = x.IsFavourite
            }).ToList();
        }

        public IEnumerable<LocalDBTweetsDetails> FilterTweets(TweetFilterModel filter, CurrentUserDetails currentUser)
        {
            var result = GetAll(currentUser);

            if (!currentUser.IsAdmin) result = result.Where(x => x.AddedByUserId == currentUser.UserId).ToList();

            if (filter != null && filter.Favourite == true) result = result.Where(x => x.Favourite == true).ToList();

            if (filter != null && !string.IsNullOrEmpty(filter.UserName)) result = result.Where(x => x.TwitterUserName.ToLower().Contains(filter.UserName.ToLower())).ToList();
            
            return result;
        }
    }
}
