using Microsoft.Ajax.Utilities;
using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            IEnumerable<Car> Cars;
            MostRecent();
            if (Request.IsAuthenticated)
            {

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

        //[NonAction]
        //public List<Car> MorePopular()
        //{
        //    var Cars = _rent_repository.All.Where(rent => rent.StartDate > DateTime.Now.AddMonths(-2)).GroupBy(rent => rent.CarID)
        //        .Count((id, rent) => ')
        //}

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