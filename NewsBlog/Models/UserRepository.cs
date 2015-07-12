using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class UserRepository
    {
        private const String ERROR_OPEN_DATABASE = "Error open table Users in database";
        public User GetUserById(Guid userId)
        {
            return _database.Users.SingleOrDefault(x => x.UserId.Equals(userId));
        }

        public User GetUserByNameAndPass(String name, String pass)
        {
            string password = MD5Hash.GetMD5Hash(pass);
            return _database.Users.Where(a => a.Name.Equals(name) && a.Password.Equals(password)).FirstOrDefault();
        }

        public User GetUserByName(String name)
        {
            return _database.Users.Where(a => a.Name.Equals(name)).FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return _database.Users.ToList();
        }
        public UserRepository(NewsBlogDatabase db)
        {
            if (db == null)
                throw new ArgumentException(ERROR_OPEN_DATABASE);
            _database = db;
        }

        private readonly NewsBlogDatabase _database;
    }
}