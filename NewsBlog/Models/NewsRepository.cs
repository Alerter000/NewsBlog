using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class NewsRepository
    {
        private const String ERROR_OPEN_DATABASE = "Error open table News in database";
        public News GetNewsById(Guid newsId)
        {
            return _database.News.SingleOrDefault(x => x.NewsId.Equals(newsId));
        }


        public IEnumerable<News> GetAllNews()
        {
            return _database.News.ToList();
        }
        public IEnumerable<News> GetNewsByUserId(Guid userId)
        {
            return _database.News.Where(x => x.Owner.UserId.Equals(userId)).ToList();
        }
        public NewsRepository(NewsBlogDatabase db)
        {
            if (db == null)
                throw new ArgumentException(ERROR_OPEN_DATABASE);
            _database = db;
        }

        private readonly NewsBlogDatabase _database;
    }
}