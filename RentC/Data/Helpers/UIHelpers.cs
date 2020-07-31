using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RentC.Data;

namespace RentC.Helpers
{
    static class UIHelpers
    {
        public static bool ConfirmScreen(string message = null)
        {
            string welcome_message = (message ??
                "<place for your message>") +
                "\n\n\n\nPress ENTER to confirm or ESC to quit.";
            ConsoleKeyInfo input;

            do
            {
                Console.Clear();
                Console.WriteLine(welcome_message);

                input = UIHelpers.ReadKey();

                if (input.Key == ConsoleKey.Enter)
                    return true;

            } while (input.Key != ConsoleKey.Escape);

            return false;
        }

        public static void IdleScreen(string message = null)
        {
            
            string info_message = "\n\n" + (message ?? "") +
                "\n\nPress ENTER to continue";
            ConsoleKeyInfo input;

            Console.WriteLine(info_message);
            do
            { 
                input = UIHelpers.ReadKey();

            } while (input.Key != ConsoleKey.Enter);

        }

        public static ConsoleKeyInfo ReadKey()
        {
            var key = Console.ReadKey();
            Console.Write("\b");
            return key;
        }
    }
}
