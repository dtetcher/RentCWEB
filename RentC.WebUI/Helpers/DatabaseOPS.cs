using RentC.WebUI.Models.DAL;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Helpers
{
    public static class DatabaseOPS
    {
        public static User isUserExists(User model)
        {
            using var ctx = new RentCEntities();
            
            User user = ctx.Users
                    .SingleOrDefault(user => user.UserID == model.UserID 
                    && user.Password == model.Password);

            return user == null ? null : user;
        }
    }
}