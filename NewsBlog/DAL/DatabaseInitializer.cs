using NewsBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NewsBlog.DAL
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<NewsBlogDatabase>
    {
        protected override void Seed(NewsBlogDatabase context)
        {
            var listsNews1 = new List<News>
            {
                new News {Title = "News11", Content = "Contents11", Category = Category.Politics},
                new News {Title = "News12", Content = "Contents12", Category = Category.Art},
                new News {Title = "News13", Content = "Contents13", Category = Category.Politics},
            };

            var listsNews2 = new List<News>
            {
                new News {Title = "News21", Content = "Contents21", Category = Category.Entertainment},
                new News {Title = "News22", Content = "Contents22", Category = Category.Politics},
                new News {Title = "News23", Content = "Contents23", Category = Category.Sport},
            };

            var listsNews3 = new List<News>
            {
                new News {Title = "News31", Content = "Contents31", Category = Category.Politics},
                new News {Title = "News32", Content = "Contents32", Category = Category.Politics},
                new News {Title = "News33", Content = "Contents33", Category = Category.Other},
            };
            var users = new List<User> 
            { 
                new User { Name = "Carson",   Password = "1234567890", EMail = "alex@mail.ru", Publications = listsNews1 }, 
                new User { Name = "Meredith", Password = "1234567890", EMail = "mer@mail.ru", Publications = listsNews2 }, 
                new User { Name = "Nino",     Password = "1234567890", EMail = "nin@mail.ru", Publications = listsNews3 } 
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
        }
    }
}