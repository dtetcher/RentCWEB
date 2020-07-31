using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    [RoutePrefix("coupon")]
    public class CouponController : Controller
    {
        private Repository<Coupon> _coupon_repository;

        public CouponController(Repository<Coupon> coupon_repo)
        {
            _coupon_repository = coupon_repo;
        }

        [Route("register")]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new CouponRegisterEditVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<ActionResult> Register(CouponRegisterEditVM CouponModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var CouponExist = _coupon_repository.All
                    .Any(coupon => 
                            coupon.CouponCode == CouponModel.CouponCode);

                if (CouponExist)
                {
                    ModelState.AddModelError("CouponCode", "Coupon code exists");
                    return View(CouponModel);
                }

                _coupon_repository.Add(new Coupon()
                {
                    CouponCode = CouponModel.CouponCode,
                    Description = CouponModel.Description,
                    Discount = CouponModel.Discount.Value
                });

                await _coupon_repository.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new
                {
                    message = "Coupon Registered.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }
            return View(CouponModel);
        }

        [Route("")]
        [Route("list")]
        public ActionResult List()
        {
            return View(_coupon_repository.All.ToList());
        }

        [Route("delete/{code}")]
        public ActionResult Delete(string code)
        {
            if (code != null)
            {
                var Coupon = _coupon_repository.All
                    .SingleOrDefault(c => c.CouponCode == code);

                if (Coupon != null)
                    return View(Coupon);
               
            }
            return HttpNotFound();
        }

        [Route("delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete_(string CouponCode)
        {
            var Coupon = _coupon_repository.All
                        .SingleOrDefault(c => c.CouponCode == CouponCode);
                
            if (Coupon != null)
            {
                _coupon_repository.Remove(Coupon);
                await _coupon_repository.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new
                {
                    message = "Coupon Deleted.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }

            return RedirectToAction("Error", "Status", new
            {
                message = "Error during deletion.",
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
            });
        }

        [Route("edit/{code}")]
        [HttpGet]
        public ActionResult Edit(string code)
        {
            if (code != null)
            {
                var Coupon = _coupon_repository.All.SingleOrDefault(c => c.CouponCode == code);
                if (Coupon != null)
                {
                    return View(new CouponRegisterEditVM()
                    {
                        CouponCode = Coupon.CouponCode,
                        Description = Coupon.Description,
                        Discount = Coupon.Discount
                    });
                }
            }
            return HttpNotFound();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public async Task<ActionResult> Edit_(CouponRegisterEditVM CouponModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var Coupon = _coupon_repository.All
                        .SingleOrDefault(c => c.CouponCode == CouponModel.CouponCode);

                if (Coupon != null)
                {
                    Coupon.Description = CouponModel.Description;
                    Coupon.Discount = CouponModel.Discount.Value;

                    await _coupon_repository.SaveChangesAsync();

                    return RedirectToAction("Success", "Status", new
                    {
                        message = "Coupon Updated.",
                        controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                    });
                }
                ModelState.AddModelError("CouponCode", "Coupon with such code not found");
            }
            return View(CouponModel);
        }

        [Route("details/{code}")]
        public ActionResult Details(string code, string returnUrl)
        {
            if (code != null)
            {
                var Coupon = _coupon_repository.All
                    .SingleOrDefault(c => c.CouponCode == code);

                if (Coupon != null)
                {
                    return View(Coupon);
                }
            }
            return HttpNotFound();
        }
    }
}