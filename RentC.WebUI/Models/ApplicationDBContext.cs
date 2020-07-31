using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Models
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext() : base("DefaultConnection") { }

        public static ApplicationDBContext Create()
        {
            return new ApplicationDBContext();
        }
    }
}