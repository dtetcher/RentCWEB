using RentC.WebUI.Models.DAL;
using RentC.WebUI.Infrastructure.Abstract;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Routing;
using RentC.WebUI.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using RentC.WebUI.Helpers;
using System.Web.Security;
using System;
using System.Web;
using System.Threading.Tasks;

namespace RentC.WebUI.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : Controller
    {
        private Repository<User> _user_repository;
        private IAuthProvider _auth_provider;

        public AuthController(Repository<User> u_repo,
            IAuthProvider provider)
        {
            _user_repository = u_repo;
            _auth_provider = provider;
        }

        [Route("")]
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(new UserLoginVM());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public ActionResult Login_(UserLoginVM model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _user_repository.All
                    .SingleOrDefault(user =>
                                user.UserID == model.UserID
                             && user.Password == model.Password);

                if (user != null)
                {
                    if (user.Enabled == false)
                    {
                        return RedirectToAction("Error", "Status", new
                        {
                            message = "This account currently is disabled.",
                            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(),
                            actionName = this.ControllerContext.RouteData.Values["action"].ToString()
                        });
                    }

                    var ticket = new FormsAuthenticationTicket(
                            1,
                            model.UserID.ToString(),
                            DateTime.Now,
                            DateTime.Now.AddHours(2),
                            model.RememberMe,
                            user.Role.Name,
                            "/"
                    );

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                    Session["USER_ID"] = user.UserID;

                    Response.SetCookie(cookie);

                    return RedirectToAction("Index", "Home");
                }

                TempData["ErrorMessage"] = "Access denied";
                ModelState.AddModelError("", "Please check your login and password and then try again.");
                return View("Login", new UserLoginVM());
            }
            return View("Login", model);
        }

        [Route("logout")]
        public ActionResult LogOut()
        {
            _auth_provider.LogOut();
            TempData["LogoutFunction"] = JS.GenFunctionName("RemoveFromStorage", "permissions"); // "RemoveFromStorage('permissions');";
            Session.Remove("permissions");
            return RedirectToAction(nameof(AuthController.Login));
        }
    }
}