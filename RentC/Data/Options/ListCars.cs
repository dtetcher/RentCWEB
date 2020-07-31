using RentC.Data.Models;
using RentC.Helpers;
using RentC.RCWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RentC.Data.Options
{
    public class ListCars : ListOption, IOption
    {
        public ListCars() : base() { }
        public ListCars(string desc) : base(desc) { }

        public void Do()
        {

            Offset = -17;
            string BasePattern = CreatePattern(new string[]
            {
                "Car ID",
                "Plate",
                "Manufacturer",
                "Model",
                "Daily Price",
                "Location"
            }, print: true);
            Console.WriteLine();

            var DB = new RentCEntities();
            RCWebService.RCWebService service = new RCWebService.RCWebService();
            var CarsJSON = service.GetCarListJSON();

            Debug.WriteLine(CarsJSON, "ERROOR");
            var Cars = JsonConvert.DeserializeObject<List<Car>>(CarsJSON);

            foreach (var c in Cars)
            {
                Console.WriteLine(BasePattern,
                                c.CarID,
                                c.Plate,
                                c.Manufacturer,
                                c.Model,
                                c.PricePerDay.ToString("C"),
                                c.LocationZipCode
                );
            }

            UIHelpers.IdleScreen();
        }
    }
}
