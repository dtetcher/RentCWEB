using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.Json;

namespace RentC.WebUI.Controllers
{
    [Authorize]
    public class ProxyController : Controller
    {
        private IRequestTransmitter _transmitter;

        public ProxyController(IRequestTransmitter transmitter)
        {
            _transmitter = transmitter;
        }


        public ActionResult Index(string permission_code)
        {
            if (!_transmitter.Determine(permission_code))
            {
                return HttpNotFound();
            }
            return RedirectToAction(_transmitter.Action, _transmitter.Controller);
        }

        [HttpPost]
        public void RenderPermissions(string Permissions, string returnUrl)
        {
            var permission_objects = JsonSerializer.Deserialize<List<Permission>>(Permissions);
            Session["permissions"] = permission_objects;
        }
    }
}