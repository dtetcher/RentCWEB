using Microsoft.Ajax.Utilities;
using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace RentC.WebUI.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        private Repository<Car> _car_repository;
        private Repository<Reservation> _rent_repository;

        public HomeController(Repository<Car> repository, Repository<Reservation> rent_repository)
        {
            _car_repository = repository;
            _rent_repository = rent_repository; 
        }

        [Route("")]
        public ActionResult Index(string selector)
        {
            if (Request.IsAuthenticated && selector != null)
            {
                ViewBag.Header = (selector == "recent") ? "Recently rented cars"
                             : (selector == "popular") ? "Popular this month"
                             : (selector == "notpopular") ? "Cars you may like"
                             : null;

                ViewBag.Cars = (selector == "recent") ? MostRecent()
                             : (selector == "popular") ? MorePopular()
                             : (selector == "notpopular") ? LessPopular() 
                             : null;
            }
            return View();
        }

        [NonAction]
        public List<Car> MostRecent()
        {
            var Cars = _rent_repository.All
                    .OrderByDescending(rent => rent.StartDate)
                    .DistinctBy(rent => rent.CarID)
                    .Select(rent => rent.Car)
                    .Take(10)
                    .ToList();

            return Cars;
        }
        [NonAction]
        public List<Car> MorePopular()
        {
            var date = DateTime.Now.AddMonths(-1);
            var Cars = _car_repository.All
                        .OrderByDescending(car => car.Reservations
                            .Select(rent =>
                                    rent.StartDate > date
                                 && rent.StartDate <= DateTime.Now)
                            .Count())
                            .Take(10)
                            .ToList();

            Cars.ForEach(car => Debug.WriteLine("{0} |||| {1}", car.Plate, car.CarID));

            return Cars;
        }

        [NonAction]
        public List<Car> LessPopular()
        {
            var month_ago = DateTime.Now.AddMonths(-1);
            var Cars = _car_repository.All
                        .OrderBy(car => car.Reservations
                            .Select(rent =>
                                    rent.StartDate > month_ago
                                 && rent.StartDate <= DateTime.Now)
                            .Count())
                            .Take(10)
                            .ToList();

            Cars.ForEach(car => Debug.WriteLine("{0} |||| {1}", car.Plate, car.CarID));

            return Cars;
        }

        [Route("about")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}