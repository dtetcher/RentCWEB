using Microsoft.Extensions.Options;
using RentC.Data.Receivers;
using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data
{
    public sealed class Menu
    {
        private static readonly object padlock = new object();
        private static Menu _instance = null;
        private OptionProxy proxy;
        private string menu_content;
        private bool wasBuilded;

        public static Menu Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Menu();
                    }
                    return _instance;
                }
            }
        }

        public string GetMenuContent()
        {
            return Instance.menu_content;
        }

        public int OptionsCount()
        {
            return Instance.proxy.Count();
        }

        public IOption GetOption(int id)
        {
            return Instance.proxy.GetOption(id);
        }

        public void SetProxy(OptionProxy proxy)
        {
            if (Instance.wasBuilded)
            {
                throw new InvalidOperationException("Can't set proxy after menu build");
            }
            Instance.proxy = proxy;
        }


        public void Build()
        {
            if (Instance.wasBuilded)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            Instance.proxy.Options().ForEach(option =>
            {
                sb.AppendFormat("{1} {0}\n", option.Description, option.ID);
            });

            Instance.wasBuilded = true;
            Instance.menu_content = sb.ToString();
        }

        public void SpawnMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine(Instance.GetMenuContent());
                var id = OptionProxy.KeyToInt(UIHelpers.ReadKey());


                if (id == -1 || id > Instance.OptionsCount())
                {
                    UIHelpers.IdleScreen("Option does not exist");
                    continue;
                }
                Console.Clear();
                IOption option = Instance.GetOption(id);
                string separator = String.Concat(Enumerable.Repeat('-', option.Description.Length));

                Console.WriteLine(separator);
                Console.WriteLine(option.Description);
                Console.WriteLine(separator);

                option.Do();

            } while (true);
        }
        public bool Welcome(string message)
        {
            return UIHelpers.ConfirmScreen(message);
        }

    }
}
