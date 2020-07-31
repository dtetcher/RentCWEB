using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using WebGrease.Css.Extensions;

namespace RentC.WebUI.Helpers
{
    public static class JS
    {
        public static string GenFunctionName(string fname, params string[] parameters)
        {
            parameters = Array.ConvertAll<string, string>(parameters, str => "'" + str + "'");
            return fname + "(" + string.Join(",", parameters) + ");";
        }
    }
}