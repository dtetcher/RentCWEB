using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace RentC.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager,Salesperson")]
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        private Repository<User> _user_repository;

        public AccountController(Repository<User> repository)
        {
            _user_repository = repository;
        }


        [Route("")]
        public ActionResult Home()
        {
            if (Session["USER_ID"] != null)
            {
                var UserID = (int)Session["USER_ID"];

                var User = _user_repository.All
                    .SingleOrDefault(user => user.UserID == UserID);

                if (User != null)
                    return View(User);
            }
            return RedirectToAction("Login", "Auth");
        }

        [Route("alter/{id:int?}")]
        public ActionResult Alter(int? id)
        {
            if (id == (int)Session["USER_ID"])
            {
                return View(new ChangePasswordVM()
                {
                    UserID = id.Value
                });
            }
            
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("alter")]
        public async Task<ActionResult> Alter_(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != model.OldPassword)
                {
                    var User = _user_repository.All
                                .SingleOrDefault(user =>
                                    user.UserID == model.UserID);
                    if (User != null)
                    {
                        User.Password = model.NewPassword;
                        await _user_repository.SaveChangesAsync();

                        return RedirectToAction("Success", "Status", new
                        {
                            message = "Password successfully changed.",
                            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                        });
                    }

                    return RedirectToAction("Error", "Status", new
                    {
                        message = "An unexpected error occurred.",
                        controllerName = "Account",
                        actionName = "Home"
                    });
                }
                ModelState.AddModelError("NewPassword", "Old and new passwords can't be the same.");
            }
            return View("Alter", model);
        }
    }
}