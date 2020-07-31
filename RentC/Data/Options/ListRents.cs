using RentC.Data.Models;
using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data.Options
{
    class ListRents : ListOption, IOption
    {
        public ListRents() : base() { }
        public ListRents(string desc) : base(desc) { }

        public void Do()
        {
            string BasePattern= CreatePattern(new string[]{
                "Car ID",
                "Plate",
                "Client ID",
                "Start Date",
                "End Date",
                "Location" }, print: true);
            Console.WriteLine();
            
            var db = new RentCEntities();
            var dataset = db.Reservations.ToList();

            foreach(var q in dataset)
            {
                Console.WriteLine(BasePattern,
                    q.CarID,
                    q.Car.Plate,
                    q.CustomerID, 
                    q.StartDate.ToString("yyyy-MM-dd"), 
                    q.EndDate.ToString("yyyy-MM-dd"), 
                    q.Car.Location.Name);
            }
            UIHelpers.IdleScreen();
        }
    }
}
