using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    public class StatusController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success(string message, string controllerName, string actionName = "List")
        {
            ViewBag.StatusMessage = message;
            ViewBag.ControllerName = controllerName;
            ViewBag.ActionName = actionName;

            return View();
        }

        public ActionResult Error(string message, string controllerName, string actionName = "List")
        {
            ViewBag.StatusMessage = message;
            ViewBag.ControllerName = controllerName;
            ViewBag.ActionName = actionName;

            return View();
        }
    }
}