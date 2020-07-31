using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data
{
    public abstract class AlterOption : Option
    {
        public AlterOption() : base() { }
        public AlterOption(string desc) : base(desc) { }

        public abstract void ReadData();
        public abstract void ValidateData();
        public abstract void SaveData();

        public int ReadInteger(string input)
        {
            int output = default;
            try
            {
                output = Convert.ToInt32(input);
            }
            catch (FormatException)
            {
                UIHelpers.IdleScreen("Please input only natural number");
                new Menu().SpawnMenu();
            }
            Console.WriteLine();
            return output;
        }

        public DateTime ReadDateTime(string input, string dtformat)
        {
            DateTime output = default;
            try
            {
                output = DateTime.ParseExact(input, dtformat,
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                UIHelpers.IdleScreen($"Incorrect date format. Try '{dtformat}'");
                new Menu().SpawnMenu();
            }
            Console.WriteLine();
            return output;
        }

        public string ReadString(string input)
        {
            if (input.Length == 0)
            {
                UIHelpers.IdleScreen("Field can't be empty.");
                new Menu().SpawnMenu();
            }
            Console.WriteLine();
            return input;
        }
    }
}
