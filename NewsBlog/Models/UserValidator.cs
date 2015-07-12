using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public static class UserValidator
    {
        public static bool IsValid(String id, User user1)
        {
            User user2;
            using (UnitOfWork unit = new UnitOfWork())
            {
                UserRepository userRep = new UserRepository(unit.DataContext);
                user2 = userRep.GetUserById(new Guid(id));
            }
            return user1.UserId == user2.UserId;
        }
    }
}