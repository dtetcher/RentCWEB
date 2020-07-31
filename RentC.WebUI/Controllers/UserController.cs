using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [RoutePrefix("user")]
    public class UserController : Controller
    {

        private Repository<User> _user_repo;
        private Repository<Role> _role_repo;

        public UserController(Repository<User> _user_repo,
                                    Repository<Role> _role_repo)
        {
            this._user_repo = _user_repo;
            this._role_repo = _role_repo;
        }

        [Route("register")]
        public ActionResult Register()
        {
            return View(new UserRegisterEditVM()
            {
                Roles = new SelectList(_role_repo.All.ToList(), "RoleID", "Name")
            });
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserRegisterEditVM UserModel)
        {
            UserModel.Roles = new SelectList(_role_repo.All.ToList(), "RoleID", "Name");
            if (ModelState.IsValid)
            {
                _user_repo.Add(new User()
                {
                    RoleID = UserModel.RoleID,
                    Password = UserModel.Password
                });

                await _user_repo.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new
                {
                    message = "User Registered.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }
            return View(UserModel);
        }

        [Route("list")]
        [Route("")]
        public ActionResult List()
        {
            return View(_user_repo.All.ToList());
        }

        [Route("alter/{id:int?}")]
        public ActionResult Alter(int? id)
        {
            if (id != null)
            {
                var User = _user_repo.All.SingleOrDefault(user => user.UserID == id);

                if (User != null)
                {
                    return View(User);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("alter")]
        public async Task<ActionResult> Alter_(int id, bool status)
        {
            var User = _user_repo.All
                    .SingleOrDefault(user => user.UserID == id);

            if (User != null)
            {
                User.Enabled = status;
                await _user_repo.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new
                {
                    message = "User Status Updated.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }

            return RedirectToAction("Error", "Status", new
            {
                message = "Status was not changed",
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
            });
        }

        [Route("edit/{id:int?}")]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var User = _user_repo.All
                    .SingleOrDefault(user => user.UserID == id);

                if (User != null)
                {
                    return View(new UserRegisterEditVM()
                    {
                        UserID = User.UserID,
                        Password = User.Password,
                        Roles = new SelectList(_role_repo.All.ToList(), "RoleID", "Name", User.RoleID)
                    });
                }
            }
            return HttpNotFound();
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserRegisterEditVM User_model)
        {
            User_model.Roles = new SelectList(_user_repo.All.ToList(), "UserID", "Name");
            if (ModelState.IsValid)
            {
                var User = _user_repo.All
                    .SingleOrDefault(user => user.UserID == User_model.UserID);

                if (User != null)
                {
                    User.RoleID = User_model.RoleID;
                    User.Password = User_model.Password;

                    await _user_repo.SaveChangesAsync();

                    return RedirectToAction("Success", "Status", new
                    {
                        message = "User Updated.",
                        controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                    });
                }
                ModelState.AddModelError("", "User with this id doesn't exist.");
            }
            return View(User_model);
        }
    }
}