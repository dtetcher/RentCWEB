using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Server;
using RentC.Data.Models;
using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RentC.Data.Options
{
    public class RegisterNewCustomer : AlterOption, IOption
    {
        private int ClientID;
        private string ClientName;
        private DateTime BirthDate;
        private Location Location;

        public RegisterNewCustomer() : base() { }
        public RegisterNewCustomer(string desc) : base(desc) { }
        public void Do()
        {
            Location = new Location();

            ReadData();
            ValidateData();
            SaveData();
        }

        public override void ReadData()
        {
            DTFormat = "dd-MM-yyyy";

            Console.Write(TextFormatPattern, "ClientID");
            ClientID = ReadInteger(Console.ReadLine());

            Console.Write(TextFormatPattern, "Client Name");
            ClientName = ReadString(Console.ReadLine());

            Console.Write(TextFormatPattern, "Birth Date");
            BirthDate = ReadDateTime(Console.ReadLine(), DTFormat);

            Console.Write(TextFormatPattern, "ZIP Code");
            Location.LocationZipCode = ReadInteger(Console.ReadLine());
        }

        public override void ValidateData()
        {
            using var db = new RentCEntities();

            Match ZipCodeMatch = Regex.Match(
                Location.LocationZipCode.ToString(), @"^\d{5}$");

            Match CustNameMatch = Regex.Match(
                ClientName, @"^\w{2,20}\s\w{2,20}$");

            DatabaseHelper.TrueOrSpawnMenu(
                ZipCodeMatch.Success,
                "Incorrect Zip Code Format.\n" +
                " Use American (XXXXX) instead.");

            DatabaseHelper.GetOrSpawnMenu(() =>
            {
                Location.Name = db.Locations.Single(r =>
                        r.LocationZipCode == Location.LocationZipCode).Name;
            }, "ZIP Code not registered");
            try
            {
                DatabaseHelper.TrueOrSpawnMenu(
                db.Customers.Single(r =>
                        r.CustomerID == ClientID) == null,
                "Customer with such ID already exists.");
            }
            catch { }

            DatabaseHelper.TrueOrSpawnMenu(
                CustNameMatch.Success,
                "Incorrect username format.\n" +
                " Format: <Name> <Surname>");

            DatabaseHelper.TrueOrSpawnMenu(
                BirthDate <= DateTime.Now.AddYears(-16),
                "You must be at least 16 to rent a car");

            DatabaseHelper.TrueOrSpawnMenu(
                BirthDate > DateTime.Now.AddYears(-90),
                "Invalid date of birth");

            DatabaseHelper.TrueOrSpawnMenu(
                Location.Name != null,
                "No city with such ZIP Code.");
        }

        public override async void SaveData()
        {
            using var db = new RentCEntities();
            
            db.Customers.Add(new Customer()
            {
                CustomerID = ClientID,
                Name = ClientName,
                BirthDate = BirthDate,
                Location = Location.Name
            });
            await db.SaveChangesAsync();

            UIHelpers.IdleScreen("You have been successfully registered.");
        }
    }
}
