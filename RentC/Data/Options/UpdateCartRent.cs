using Microsoft.Extensions.Options;
using RentC.Data.Models;
using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data.Options
{
    public class UpdateCartRent : AlterOption, IOption
    {
        private string Plate;
        private int ClientID;
        private DateTime StartDate;
        private DateTime EndDate;
        private string City;
        Car Car;

        public UpdateCartRent() : base() { }
        public UpdateCartRent(string desc) : base(desc) { }

        public void Do()
        {
            ReadData();
            ValidateData();
            SaveData();
        }

        public override void ReadData()
        {
            Console.Write(TextFormatPattern, "Cart Plate");
            Plate = base.ReadString(Console.ReadLine());

            Console.Write(TextFormatPattern, "Client ID");
            ClientID = base.ReadInteger(Console.ReadLine());

            Console.Write(TextFormatPattern, "Start Date");
            StartDate = base.ReadDateTime(Console.ReadLine(), DTFormat);

            Console.Write(TextFormatPattern, "End Date");
            EndDate = base.ReadDateTime(Console.ReadLine(), DTFormat);

            Console.Write(TextFormatPattern, "City");
            City = base.ReadString(Console.ReadLine());
        }
        public override void ValidateData()
        {
            using var db = new RentCEntities();


            DatabaseHelper.GetOrSpawnMenu(() =>
            {
                Car = db.Cars.Single(car => car.Plate == Plate);

            }, "Car with this plate does not exist");

            
            DatabaseHelper.GetOrSpawnMenu(() =>
            {
                db.Reservations.Single(rent =>
                            rent.CarID == Car.CarID &&
                            rent.CustomerID == ClientID &&
                            rent.ReservStatsID == (int)Status.OPEN);

            }, "Car reservation is not yours. Or car not rented.");


            DatabaseHelper.GetOrSpawnMenu(() =>
            {
                db.Customers.Single(r =>
                            r.CustomerID == ClientID);

            }, "Client not found");

            DatabaseHelper.TrueOrSpawnMenu(
                StartDate >= DateTime.Now,
                "Start date not valid");

            DatabaseHelper.TrueOrSpawnMenu(
                EndDate >= StartDate,
                "End date can't be less than start date");

            //DatabaseHelper.TrueOrSpawnMenu(
            //    Car.Location.Name == City,
            //    "Car not available in your city");
        }

        public override async void SaveData()
        {
            using var db = new RentCEntities();

            Reservation Reservation = db.Reservations
                .Single(rent =>
                        rent.CarID == Car.CarID &&
                        rent.CustomerID == ClientID &&
                        rent.ReservStatsID == (int)Status.OPEN);

            Reservation.StartDate = this.StartDate;
            Reservation.EndDate = this.EndDate;
            Reservation.Location = City;

            await db.SaveChangesAsync();

            UIHelpers.IdleScreen("Your data submitted.");
        }
    }
}
