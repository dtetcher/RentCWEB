using RentC.WebUI.Helpers.Extensions;
using RentC.WebUI.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Infrastructure.Concrete
{
    public class RolePermissionManager
    {
        private RentCEntities context = new RentCEntities();

        public RolePermissionManager(RentCEntities context)
        {
            this.context = context;
        }

        public string ResolveRoleName(string name)
        {
            User foundUser = (from user in context.Users
                              where user.UserID.ToString() == name
                              select user).SingleOrDefault();

            if (foundUser == null)
                return null;

            if (foundUser.IsAdmin())
            {
                return "administrator";
            }else if (foundUser.IsManager())
            {
                return "manager";
            }else if (foundUser.IsSales())
            {
                return "salesperson";
            }

            return null;
        }
    }
}