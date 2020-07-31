 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.Infrastructure.Abstract;
using Unity;

namespace RentC.WebUI
{
    /// <summary>
    /// Summary description for RCWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RCWebService : System.Web.Services.WebService
    {

        private Repository<Car> _car_repository;
        private Repository<Reservation> _reservation_repository;

        public RCWebService()
        {
            _car_repository = UnityConfig.Container.Resolve<Repository<Car>>();
            _reservation_repository = UnityConfig.Container.Resolve<Repository<Reservation>>();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetCarListJSON()
        {

            var ReservedCarsIDs = _reservation_repository
                                    .Filter(rent => rent.ReservStatsID == 1)
                                    .Select(rent => rent.CarID)
                                    .ToList();

            var AvailableCars = _car_repository
                                    .Filter(car => !ReservedCarsIDs
                                    .Contains(car.CarID))
                                    .Select(car =>
                                                    new Car
                                                    {
                                                        CarID = car.CarID,
                                                        Plate = car.Plate,
                                                        Manufacturer = car.Manufacturer,
                                                        Model = car.Model,
                                                        PricePerDay = car.PricePerDay,
                                                        LocationZipCode = car.LocationZipCode
                                                    })
                                    .ToList();

            return new JavaScriptSerializer().Serialize(AvailableCars).ToString();

        }
    }
}
