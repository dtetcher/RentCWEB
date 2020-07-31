using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RentC.Data;
using RentC.Data.Receivers;
using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using RentC.Data.Options;

namespace RentC
{
    class Program
    {
        delegate int KeyToInt(ConsoleKeyInfo key);
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            OptionProxy proxy = new OptionProxy(
                new RegisterNewRent("Register new Car Rent"),
                new UpdateCartRent("Update Car Rent"),
                new ListRents("List Rents"),
                new ListCars("List Available Cars"),
                new RegisterNewCustomer("Register new Customer"),
                new UpdateCustomer("Update Customer"),
                new ListCustomers("List Customers"),
                new ExitOption("Quit")
            );
            menu.SetProxy(proxy);

            menu.Build();

            string welcome_message = "Welcome to RentC, your brand new solution to\n" +
                "manage and control your company's data\n" +
                "without missing anything.";

            if (menu.Welcome(welcome_message))
            {
                menu.SpawnMenu();
            }   
        }
    }
}



