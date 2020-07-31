using RentC.WebUI.Helpers.Extensions;
using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Infrastructure.Concrete;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    [RoutePrefix("customer")]
    public class CustomerController : Controller
    {
        private Repository<Customer> _customer_repository;
        private Repository<Location> _location_repository;

        public CustomerController(Repository<Customer> c_repo,
            Repository<Location> l_repo)
        {
            _customer_repository = c_repo;
            _location_repository = l_repo;
        }

        [Route("register")]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var locations = _location_repository.All.ToList();
            locations.Insert(0, new Location() { Name = null, LocationZipCode = 0 });

            return View(new CustomerRegisterEditVM()
            {
                Locations = new SelectList(locations, "LocationZipCode", "Name")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<ActionResult> Register(CustomerRegisterEditVM model, string returnUrl)
        {
            var locations = _location_repository.All.ToList();
            locations.Insert(0, new Location() { Name = null, LocationZipCode = 0 });
            model.Locations = new SelectList(locations, "LocationZipCode", "Name");

            if (ModelState.IsValid)
            {
                // if zip code entered
                if (model.LocationZipCode != null && model.LocationZipCode != 0)
                {
                    // Get location with entered zip code
                    var CustomerLocation = _location_repository
                        .Filter(location => location.LocationZipCode == model.LocationZipCode)
                        .SingleOrDefault();

                    // Error if entered and not valid
                    if (CustomerLocation != null)
                    {
                        _customer_repository.Add(new Customer()
                        {
                            Name = model.Name,
                            BirthDate = (DateTime)model.BirthDate,
                            Location = CustomerLocation.Name
                        });
                    }
                    ModelState.AddModelError("LocationZipCode", "Customer's city not available.");
                    return View(model);
                }
                else    // if zip code not entered
                {
                    _customer_repository.Add(new Customer()
                    {
                        Name = model.Name,
                        BirthDate = (DateTime)model.BirthDate,
                    });
                }

                await _customer_repository.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new { message = "Customer registered.", 
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString() });
            }
            return View(model);
        }

        [Route("list")]
        [Route("")]
        public ActionResult List()
        {
            return View(_customer_repository.All.ToList());
        }

        [Route("delete/{id:int?}")]
        public ActionResult Delete(int? id, string returnUrl)
        {
            if (id != null)
            {
                var Customer = _customer_repository.All
                                .SingleOrDefault(c => c.CustomerID == id);

                if (Customer != null)
                    return View(Customer);
            }
            return HttpNotFound();
        }

        [Route("delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Delete_(int? id)
        {
            if (id != null)
            {
                var Customer = _customer_repository.All
                                .Include("Reservations")
                                .SingleOrDefault(c => c.CustomerID == id);

                if (Customer != null)
                {
                    _customer_repository.Remove(Customer);
                    await _customer_repository.SaveChangesAsync();

                    return RedirectToAction("Success", "Status", new
                    {
                        message = "Customer deleted.",
                        controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                    });
                }
            }
            return RedirectToAction("Error", "Status", new
            {
                message = "Error during deletion.",
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
            });
        }

        [Route("edit/{id:int?}")]
        public ActionResult Edit(int? id, string returnUrl)
        {
            if (id != null)
            {
                ViewBag.ReturnUrl = returnUrl;
                var customer = _customer_repository.Filter(c => c.CustomerID == id).SingleOrDefault();

                if (customer != null)
                {
                    var locations = _location_repository.All.ToList();
                    locations.Insert(0, new Location() { Name = null, LocationZipCode = 0 });

                    return View(new CustomerRegisterEditVM()
                    {
                        Id = id.Value,
                        BirthDate = customer.BirthDate,
                        Name = customer.Name,
                        Locations = new SelectList(locations, "LocationZipCode", "Name")
                    });
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public async Task<ActionResult> Edit(CustomerRegisterEditVM model, string returnUrl)
        {
            var locations = _location_repository.All.ToList();
            locations.Insert(0, new Location() { Name = null, LocationZipCode = 0 });
            model.Locations = new SelectList(locations, "LocationZipCode", "Name");

            if (ModelState.IsValid)
            {
                Customer customer;
                Location CustomerLocation = default;

                customer = _customer_repository
                        .Filter(c => c.CustomerID == model.Id)
                        .SingleOrDefault();

                if (customer == null)
                    return HttpNotFound();


                if (model.LocationZipCode != null)
                {
                    // Get customer location object if zip code entered
                    CustomerLocation = _location_repository
                        .Filter(location => location.LocationZipCode == model.LocationZipCode)
                        .SingleOrDefault();

                    // Error if entered zip code not valid
                    if (CustomerLocation == null)
                    {
                        ModelState.AddModelError("", "Customer's city not available.");
                        return View(model);
                    }

                    customer.BirthDate = model.BirthDate.Value;
                    customer.Location = CustomerLocation.Name;
                    customer.Name = model.Name;
                }
                else
                {
                    customer.BirthDate = model.BirthDate.Value;
                    customer.Name = model.Name;
                    customer.Location = null;
                }

                await _customer_repository.SaveChangesAsync();
                
                return RedirectToAction("Success", "Status", new { message = "Customer record updated.",
                                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString() });
            }
            return View(model);
        }

        [Route("details/{id:int?}")]
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                var Customer = _customer_repository.All
                    .SingleOrDefault(c => c.CustomerID == id);
             
                if (Customer != null)
                {
                    var month_ago = DateTime.Now.AddMonths(-1);
                    var RentsThisMonth = Customer.Reservations.Select(rent =>
                                                rent.StartDate > month_ago
                                                && rent.StartDate <= DateTime.Now
                                                && (rent.ReservationStatus.Name == "OPEN"
                                                        || rent.ReservationStatus.Name == "CLOSED")
                                                   ).Count();

                    ViewBag.Membership = (RentsThisMonth >= 4) ? "Gold"
                                        : (RentsThisMonth >= 2) ? "Silver"
                                        : null;

                    return View(Customer);
                }
            }
            return HttpNotFound();
        }
    }
}