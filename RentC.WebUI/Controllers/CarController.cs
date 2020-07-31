using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    [RoutePrefix("car")]
    public class CarController : Controller
    {
        private Repository<Car> _car_repository;
        private Repository<Location> _location_repository;


        public CarController(Repository<Car> car_repo,
                             Repository<Location> loc_repo)
        {
            _car_repository = car_repo;
            _location_repository = loc_repo;
        }

        [Route("register")]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new CarRegisterEditVM()
            {
                Locations = new SelectList(_location_repository.All, "LocationZipCode", "Name")
            });
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CarRegisterEditVM CarModel, string returnUrl)
        {
            CarModel.Locations = new SelectList(_location_repository.All, "LocationZipCode", "Name");
            if (ModelState.IsValid)
            {
                var PlateRegistered = _car_repository.All.Any(car => car.Plate == CarModel.Plate);

                if (PlateRegistered)
                {
                    ModelState.AddModelError("Plate", "Plate registered");
                    return View(CarModel);
                }

                _car_repository.Add(new Car()
                {
                    Plate = CarModel.Plate,
                    Manufacturer = CarModel.Manufacturer,
                    Model = CarModel.Model,
                    PricePerDay = CarModel.DailyPrice.Value,
                    LocationZipCode = CarModel.LocationZipCode
                });

                await _car_repository.SaveChangesAsync();
                return RedirectToAction("Success", "Status", new
                {
                    message = "Car registered.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }
            return View(CarModel);
        }

        [Route("list")]
        [Route("")]
        public ActionResult List()
        {
            return View(_car_repository.All.ToList());
        }

        [Route("delete/{id:int?}")]
        public ActionResult Delete(int? id, string returnUrl)
        {
            if (id != null)
            {
                var Car = _car_repository.All
                        .SingleOrDefault(car => car.CarID == id);

                if (Car != null)
                    return View(Car);
            }
            return HttpNotFound();
        }

        [Route("delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Delete_(int? id, string returnUrl)
        {
            if (id != null)
            {
                var Car = _car_repository.All
                        .Include("Reservations")
                        .SingleOrDefault(car => car.CarID == id);

                if (Car != null)
                {
                    _car_repository.Remove(Car);
                    await _car_repository.SaveChangesAsync();

                    return RedirectToAction("Success", "Status", new
                    {
                        message = "Car Deleted.",
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
            ViewBag.ReturnUrl = returnUrl;
            if(id != null)
            {
                var Car = _car_repository.All
                        .SingleOrDefault(car => car.CarID == id);

                if (Car == null)
                    return HttpNotFound();
                
                return View(new CarRegisterEditVM()
                {
                    CarID = id.Value,
                    Plate = Car.Plate,
                    Manufacturer = Car.Manufacturer,
                    Model = Car.Model,
                    DailyPrice = Car.PricePerDay,
                    Locations = new SelectList(_location_repository.All,
                                    "LocationZipCode", "Name", Car.LocationZipCode)
                });
            }
            return HttpNotFound();
            
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CarRegisterEditVM CarModel, string returnUrl)
        {
            CarModel.Locations = new SelectList(_location_repository.All, "LocationZipCode", "Name");
            if (ModelState.IsValid)
            {
                var PlateRegistered = _car_repository.All.Any(car => car.Plate == CarModel.Plate);

                if (PlateRegistered)
                {
                    ModelState.AddModelError("Plate", "Plate registered");
                    return View(CarModel);
                }

                var Car = _car_repository.All
                        .SingleOrDefault(car => car.CarID == CarModel.CarID);

                if (Car == null)
                {
                    ModelState.AddModelError("", "Car record doesn't exist.");
                    return View(CarModel);
                }

                Car.Plate = CarModel.Plate;
                Car.Manufacturer = CarModel.Manufacturer;
                Car.Model = CarModel.Model;
                Car.PricePerDay = CarModel.DailyPrice.Value;
                Car.LocationZipCode = CarModel.LocationZipCode;

                await _car_repository.SaveChangesAsync();
                return RedirectToAction("Success", "Status", new
                {
                    message = "Car Updated.",
                    controllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }
            return View(CarModel);
        }
    }
}