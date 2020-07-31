using RentC.WebUI.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Text;
using Unity.Injection;

namespace RentC.WebUI.Helpers.Extensions
{
    public static class Extensions
    {
        public static bool IsInRole(User user, string roleName)
        {
            return roleName
                .Equals(user.Role.Name, 
                StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAdmin(this User user)
        {
            return IsInRole(user, "administrator");
        }

        public static bool IsManager(this User user)
        {
            return IsInRole(user, "manager");
        }

        public static bool IsSales(this User user)
        {
            return IsInRole(user, "salesperson");
        }

    }
}