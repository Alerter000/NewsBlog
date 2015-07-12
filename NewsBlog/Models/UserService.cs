using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class UserService
    {
        private const String ERROR_OPEN_DATABASE = "Error open table UserFiles in database";
        private const String ERROR_CREATE_USER = "Error create user in database";
        private const String ERROR_DELETE_USER = "Error delete user in database";
        private const String ERROR_UPDATE_USER = "Error update user in database";


        public UserService(NewsBlogDatabase db)
        {
            if (db == null)
                throw new ArgumentException(ERROR_OPEN_DATABASE);
            _database = db;
        }
        public User Create(String name, String pass, String mail)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(mail))
                throw new ArgumentException(ERROR_CREATE_USER);
            User user = new User()
            {
                Name = name,
                Password = MD5Hash.GetMD5Hash(pass),
                EMail = mail,
                DateRegistration = DateTime.Now,
                UserId = Guid.NewGuid()
            };
            
            _database.Users.Add(user);
            
            return user;
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentException(ERROR_DELETE_USER);
            _database.Users.Remove(user);
        }

        public void Update(User user, String name, String pass, String mail)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(mail) || user == null)
                throw new ArgumentException(ERROR_UPDATE_USER);
            user.Name = name;
            user.Password = pass;
            user.EMail = mail;
        }

        private readonly NewsBlogDatabase _database;
    }
}