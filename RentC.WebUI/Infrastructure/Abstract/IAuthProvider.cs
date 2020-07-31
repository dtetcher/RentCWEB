using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        public bool Authenticate(string name, string password);
        public void LogOut();
        public void RedirectFromLogin();
    }
}
