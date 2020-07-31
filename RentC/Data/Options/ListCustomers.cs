using RentC.Data.Models;
using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data.Options
{
    public class ListCustomers : ListOption, IOption
    {
        public ListCustomers() : base() { }
        public ListCustomers(string desc) : base(desc) { }

        public void Do()
        {
            Offset = -22;
            string BasePattern = CreatePattern(new string[]
            {
                "Client ID",
                "Client Name",
                "Birth Date",
                "Location"
            }, print: true);
            Console.WriteLine();

            var DB = new RentCEntities();
            var Dataset = DB.Customers.ToList();

            Dataset.ForEach(q =>
            {
                Console.WriteLine(BasePattern,
                                 q.CustomerID,
                                 q.Name,
                                 q.BirthDate.ToString(DTFormat),
                                 q.Location);
            });

            UIHelpers.IdleScreen();
        }
    }
}
