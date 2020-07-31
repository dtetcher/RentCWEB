using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace RentC.WebUI.Infrastructure.Concrete
{
    public class WebAuthProvider : IAuthProvider
    {
        private MembershipProvider m_provider;

        public WebAuthProvider(MembershipProvider provider)
        {
            m_provider = provider;
        }
        private string UserName { get; set; }
        public bool Authenticate(string name, string password)
        {
            UserName = name;
            var result = m_provider.ValidateUser(name, password);
            Debug.WriteLine(result.ToString());

            if (result)
            {
                FormsAuthentication.SetAuthCookie(name, false);
            }

            return result;
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public void RedirectFromLogin()
        {
            FormsAuthentication.RedirectFromLoginPage(UserName, false, FormsAuthentication.FormsCookiePath);
        }
    }
}