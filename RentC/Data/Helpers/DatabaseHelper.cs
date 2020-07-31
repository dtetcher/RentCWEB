using RentC.Data;
using RentC.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Helpers
{
    public static class DatabaseHelper
    {
        public static void TrueOrSpawnMenu(bool condition, string message)
        {
            if (!condition)
            {
                UIHelpers.IdleScreen(message);
                new Menu().SpawnMenu();
            }
        }
        public static void GetOrSpawnMenu(Action action, string message)
        {
            try
            {
                    action();
            }
            catch (InvalidOperationException)
            {
                UIHelpers.IdleScreen(message);
                new Menu().SpawnMenu();
            }
        }
    }
}
