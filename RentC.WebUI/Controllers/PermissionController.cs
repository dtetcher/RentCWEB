using Microsoft.Owin.Security.Provider;
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
    [RoutePrefix("permission")]
    public class PermissionController : Controller
    {
        private Repository<Permission> _permission_repo;

        public PermissionController(Repository<Permission> _p_repo)
        {
            _permission_repo = _p_repo;
        }

        [Route("register")]
        public ActionResult Register()
        {
            return View(new PermissionRegisterEditVM());
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(PermissionRegisterEditVM PermModel)
        {
            if (ModelState.IsValid)
            {
                var PermNameUsed = _permission_repo.All
                        .Any(p => p.Name == PermModel.Name);
                if (!PermNameUsed)
                {
                    _permission_repo.Add(new Permission()
                    {
                        Name = PermModel.Name,
                        Description = PermModel.Description
                    });

                    await _permission_repo.SaveChangesAsync();

                    return RedirectToAction("Success", "Status", new
                    {
                        message = "Permission Registered.",
                        controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                    });
                }
                ModelState.AddModelError("Name", "Name is used");
            }
            return View(PermModel);
        }

        [Route("")]
        [Route("list")]
        public ActionResult List()
        {
            return View(_permission_repo.All.ToList());
        }

        [Route("delete/{id:int?}")]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var Permission = _permission_repo.All.SingleOrDefault(p => p.PermissionID == id);

                if (Permission != null)
                {
                    return View(Permission);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public async Task<ActionResult> Delete_(int id)
        {
            var Permission = _permission_repo.All
                    .Include("Roles")
                    .SingleOrDefault(p => p.PermissionID == id);

            if(Permission != null)
            {
                _permission_repo.Remove(Permission);
                await _permission_repo.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new
                {
                    message = "Permission Deleted.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }

            return RedirectToAction("Error", "Status", new
            {
                message = "Error during deletion.",
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
            });
        }

        [Route("edit/{id:int?}")]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var Permission = _permission_repo.All
                    .SingleOrDefault(p => p.PermissionID == id);

                if (Permission != null)
                {
                    return View(new PermissionRegisterEditVM() 
                    {
                        ID = Permission.PermissionID,
                        Name = Permission.Name,
                        Description = Permission.Description
                    });
                }
            }
            return HttpNotFound();
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PermissionRegisterEditVM Perm_model)
        {
            if (ModelState.IsValid)
            {
                var PermNameUsed = _permission_repo.All
                        .Any(p => p.Name == Perm_model.Name && p.PermissionID != Perm_model.ID);

                var Permission = _permission_repo.All
                                .SingleOrDefault(p => p.PermissionID == Perm_model.ID);

                if (!PermNameUsed && Permission != null)
                {
                    Permission.Name = Perm_model.Name;
                    Permission.Description = Perm_model.Description;

                    await _permission_repo.SaveChangesAsync();

                    return RedirectToAction("Success", "Status", new
                    {
                        message = "Permission Updated.",
                        controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                    });
                }
                ModelState.AddModelError("Name", "Name is used");
            }
            return View(Perm_model);
        }
    }
}