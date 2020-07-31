using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    [Authorize]
    [RoutePrefix("reservation")]
    public class ReservationController : Controller
    {
        private Repository<Location> _location_repository;
        private Repository<Reservation> _reservation_repository;
        private Repository<ReservationStatus> _reservation_status_repository;
        private Repository<Coupon> _coupon_repository;
        private Repository<Car> _car_repository;
        private Repository<Customer> _customer_repository;

        public ReservationController(
                Repository<Location> location_repo,
                Repository<Reservation> _reservation_repo,
                Repository<ReservationStatus> _reservation_status_repo,
                Repository<Coupon> _coupon_repo,
                Repository<Car> _car_repo,
                Repository<Customer> _customer_repo)
        {
            _location_repository = location_repo;
            _reservation_repository = _reservation_repo;
            _reservation_status_repository = _reservation_status_repo;
            _coupon_repository = _coupon_repo;
            _car_repository = _car_repo;
            _customer_repository = _customer_repo;
        }


        [Authorize(Roles = "Administrator,Manager,Salesperson")]
        [Route("register")]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View(new ReservationRegisterVM()
            {
                Locations = new SelectList(_location_repository.All, "Name", "Name"),
            });
        }

        [Authorize(Roles = "Administrator,Manager,Salesperson")]
        [Route("register")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Register(ReservationRegisterVM model, string returnUrl)
        {
            model.Locations = new SelectList(_location_repository.All, "Name", "Name");
            if (ModelState.IsValid)
            {
                Reservation NewReservation = new Reservation();

                // Check if car not rented and exist
                var CarAvailable = _car_repository.All.Any(car => car.CarID == model.CarID
                                &&
                                    car.Reservations
                                    .Where(rent => rent.ReservStatsID == 1).Count() == 0);

                if (!CarAvailable)
                {
                    ModelState.AddModelError("CarID", "Car rented or does not exist");
                    return View(model);
                }

                // Check if customer exists
                var CustomerExistsAndHaveNoRents = _customer_repository.All
                            .Any(cust => 
                                    cust.CustomerID == model.CustomerID 
                                 && cust.Reservations.Where(rent => rent.ReservStatsID == 1).Count() == 0);

                if (!CustomerExistsAndHaveNoRents)
                {
                    ModelState.AddModelError("CustomerID", "Customer does not exist or have an open rent.");
                    return View(model);
                }
                    

                // Check if coupon code exists
                if (model.CouponCode != null)
                {
                    var Coupon = _coupon_repository.All
                            .SingleOrDefault(coupon => coupon.CouponCode == model.CouponCode);

                    if (Coupon == null)
                    {
                        ModelState.AddModelError("CouponCode", "Coupon code doesn't exist");
                        return View(model);
                    }
                        
                }

                _reservation_repository.Add(new Reservation()
                {
                    CarID = model.CarID.Value,
                    CustomerID = model.CustomerID.Value,
                    ReservStatsID = 1,
                    StartDate = model.StartDate.Value,
                    EndDate = model.EndDate.Value,
                    Location = model.Location.ToString(),
                    CouponCode = model.CouponCode ?? null
                });

                await _reservation_repository.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new
                {
                    message = "Reservation registered.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator,Manager,Salesperson")]
        [Route("list")]
        [Route("")]
        public ActionResult List()
        {
            return View(_reservation_repository.All.ToList());
        }

        [Authorize(Roles = "Administrator,Manager")]
        [Route("delete/{cust_id:int?}/{car_id:int?}/{status}/{startDate:datetime?}/{endDate:datetime?}")]
        public ActionResult Delete(int? cust_id, int? car_id, string status, DateTime? startDate, DateTime? endDate, string returnUrl)
        {
            if (cust_id != null && car_id != null && status != null && startDate != null && endDate != null)
            {
                var RentModel = _reservation_repository.All
                        .SingleOrDefault(rent =>
                                        rent.CustomerID == cust_id
                                     && rent.CarID == car_id
                                     && rent.ReservationStatus.Name == status
                                     && rent.StartDate == startDate
                                     && rent.EndDate == endDate
                );

                if (RentModel == null)
                    return HttpNotFound();

                ViewBag.ReturnUrl = returnUrl;
                return View(RentModel);
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Administrator,Manager,Salesperson")]
        [Route("delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Delete_(Reservation model, string returnUrl)
        {
            var Reserv = _reservation_repository.All
                        .SingleOrDefault(rent =>
                                   rent.CustomerID == model.CustomerID
                                && rent.CarID == model.CarID
                                && rent.ReservStatsID == model.ReservStatsID
                                && rent.StartDate == model.StartDate
                                && rent.EndDate == model.EndDate);

            if (Reserv != null)
            {
                _reservation_repository.Remove(Reserv);

                await _reservation_repository.SaveChangesAsync();

                return RedirectToAction("Success", "Status", new
                {
                    message = "Reservation removed.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }

            return RedirectToAction("Error", "Status", new
            {
                message = "Error during deletion.",
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
            });

        }

        [Authorize(Roles = "Administrator,Manager,Salesperson")]
        [Route("edit/{cust_id:int?}/{car_id:int?}/{status}/{startDate:datetime?}/{endDate:datetime?}")]
        public ActionResult Edit(int? cust_id, int? car_id, string status, DateTime? startDate, DateTime? endDate, string returnUrl)
        {
            if (cust_id != null && car_id != null && status != null && startDate != null && endDate != null)
            {
                var SelectedModel = _reservation_repository.All
                        .SingleOrDefault(rent =>
                                        rent.CustomerID == cust_id
                                     && rent.CarID == car_id 
                                     && rent.ReservationStatus.Name == status
                                     && rent.StartDate == startDate
                                     && rent.EndDate == endDate);

                if (SelectedModel != null)
                {
                    ViewBag.ReturnUrl = returnUrl;

                    return View(new ReservationEditVM()
                    {
                        CarID = SelectedModel.CarID,
                        CustomerID = SelectedModel.CustomerID,
                        CouponCode = SelectedModel.CouponCode,
                        StartDate = SelectedModel.StartDate,
                        EndDate = SelectedModel.EndDate,

                        Locations = new SelectList(_location_repository.All,
                                    "Name", "Name"),

                        ReservStatuses = new SelectList(_reservation_status_repository.All,
                                    "ReservStatsID", "Name", SelectedModel.ReservStatsID),

                        StartDateCopy = SelectedModel.StartDate,
                        EndDateCopy = SelectedModel.EndDate,
                        StatusCopy = SelectedModel.ReservStatsID
                    });
                }
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Administrator,Manager,Salesperson")]
        [Route("edit")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit_(ReservationEditVM model,  string returnUrl)
        {
            model.Locations = new SelectList(_location_repository.All, "Name", "Name");

            model.ReservStatuses = new SelectList(_reservation_status_repository.All,
                        "ReservStatsID", "Name", model.Status);

            if (ModelState.IsValid)
            {
                var Record = _reservation_repository.All
                                    .SingleOrDefault(rent =>
                                            rent.CustomerID == model.CustomerID
                                         && rent.CarID == model.CarID
                                         && rent.ReservStatsID == model.StatusCopy
                                         && rent.StartDate == model.StartDateCopy
                                         && rent.EndDate == model.EndDateCopy);

                if (model.CouponCode != null)
                {
                    var CouponExists = _coupon_repository.All
                                .Any(coupon => coupon.CouponCode == model.CouponCode);

                    if (CouponExists)
                    {
                        Record.ReservStatsID = model.Status;
                        Record.StartDate = model.StartDate.Value;
                        Record.EndDate = model.EndDate.Value;
                        Record.Location = model.Location;
                        Record.CouponCode = model.CouponCode;

                        await _reservation_repository.SaveChangesAsync();

                        return RedirectToAction("Success", "Status", new
                        {
                            message = "Reservation updated.",
                            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                        });
                    }
                    ModelState.AddModelError("CouponCode", "Coupon code does not exist");
                }
                else
                {
                    Record.ReservStatsID = model.Status;
                    Record.StartDate = model.StartDate.Value;
                    Record.EndDate = model.EndDate.Value;
                    Record.Location = model.Location;

                    await _reservation_repository.SaveChangesAsync();

                    return RedirectToAction("Success", "Status", new
                    {
                        message = "Reservation updated.",
                        controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                    });
                }
            }
            return View("Edit", model);
        }

        //public ActionResult EditStatus(Reservation model)

    }
}