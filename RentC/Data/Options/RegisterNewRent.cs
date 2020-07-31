using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Odbc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RentC.Data.Models;
using System.Diagnostics;
using System.Data.Entity.Validation;
using Microsoft.SqlServer.Server;
using RentC.Helpers;
using RentC.Data.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RentC.Data.Receivers
{
    public class RegisterNewRent : AlterOption, IOption
    {
        private int CarID;
        private int ClientID;
        private string Plate;
        private DateTime StartDate;
        private DateTime EndDate;
        private string City;

        public RegisterNewRent() : base(){ }
        public RegisterNewRent(string desc) : base(desc){ }

        public void Do()
        {
            ReadData();
            ValidateData();
            SaveData();
        }

        public override void ReadData()
        {
            Console.Write(TextFormatPattern, "Cart Plate");
            Plate = ReadString(Console.ReadLine());

            Console.Write(TextFormatPattern, "Client ID");
            ClientID = ReadInteger(Console.ReadLine());

            Console.Write(TextFormatPattern, "Start Date");
            StartDate = ReadDateTime(Console.ReadLine(), DTFormat);

            Console.Write(TextFormatPattern, "End Date");
            EndDate = ReadDateTime(Console.ReadLine(), DTFormat);

            Console.Write(TextFormatPattern, "City");
            City = ReadString(Console.ReadLine());
        }
        public override void ValidateData()
        {
            Customer Customer = default;
            Car Car = default;
            Reservation Reservation = default;

            using var db = new RentCEntities();
            DatabaseHelper.GetOrSpawnMenu(() =>
            {
                Customer = db.Customers.Single(r =>
                            r.CustomerID == ClientID);

            }, "Client not found");

            DatabaseHelper.GetOrSpawnMenu(() =>
            {
                Car = db.Cars.Single(car => car.Plate == Plate);   
            }, "No car with specified plate");

            Reservation = db.Reservations.SingleOrDefault(rent =>
            rent.CarID == Car.CarID &&
            rent.ReservStatsID == (int)Status.OPEN);

            //DatabaseHelper.TrueOrSpawnMenu(
            //    Car.Location.Name == City,
            //    "Car not available in your city"
            //);
            
            DatabaseHelper.TrueOrSpawnMenu(
                    Reservation == null,
                    "Car already reserved");

            DatabaseHelper.TrueOrSpawnMenu(
                StartDate >= DateTime.Now,
                "Start date not valid");

            DatabaseHelper.TrueOrSpawnMenu(
                EndDate >= StartDate,
                "End date can't be less then start date.");

            CarID = Car.CarID;
        }

        public override async void SaveData()
        {
            var new_rent = new Reservation()
            {
                CarID = this.CarID,
                CustomerID = ClientID,
                ReservStatsID = (int)Status.OPEN,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Location = City
            };

            using var db = new RentCEntities();

            db.Reservations.Add(new_rent);
            await db.SaveChangesAsync();

            UIHelpers.IdleScreen("Your data submitted.");
        }
    }
}
