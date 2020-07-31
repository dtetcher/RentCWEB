using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data
{
    public class OptionProxy
    {
        private List<IOption> options = new List<IOption>();
        private int next_id;
        public OptionProxy(params IOption[] options)
        {
            next_id = 0;
            
            foreach(var option in options)
            {
                option.ID = ++next_id;
                this.options.Add(option);
            }
        }

        public int Count()
        {
            return options.Count;
        }
        public List<IOption> Options()
        {
            return options;
        }

        public void AddOption(IOption option)
        {
            option.ID = ++next_id;
            options.Add(option);
        }

        public IOption GetOption(int id)
        {
            try
            {
                return options.Find(o => o.ID == id);

            }
            catch (NullReferenceException)
            {
                
                RentC.Helpers.UIHelpers.IdleScreen("Option does not exist");
            }
            return default;
        }
        public static int KeyToInt(ConsoleKeyInfo input)
        {
            if (char.IsDigit(input.KeyChar))
            {
                return int.Parse(input.KeyChar.ToString());
            }
            return -1;
        }
    }
}
