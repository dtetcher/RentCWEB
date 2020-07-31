using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data
{
    public class ListOption : Option
    {
        public ListOption() : base() { }
        public ListOption(string desc) : base(desc) { }

        public string CreatePattern(string[] headers, bool print = false)
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0; i<headers.Length; i++)
            {
                string p = TextFormatPattern.Replace("0", i.ToString());
                
                if (print)
                    Console.Write(TextFormatPattern, headers[i]);

                sb.Append(p);
            }
            return sb.ToString();
        }

        public string CreatePattern(int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(TextFormatPattern.Replace("0", i.ToString()));
            }
            return sb.ToString();
        }
    }
}
