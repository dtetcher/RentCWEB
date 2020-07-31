using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data.Receivers
{
    public class ExitOption : Option, IOption
    {
        public ExitOption() : base() { }
        public ExitOption(string desc) : base(desc) { }

        public void Do()
        {
            if (UIHelpers.ConfirmScreen("Do you really want to exit?"))
            {
                Environment.Exit(0);
            }
        }
    }
}
