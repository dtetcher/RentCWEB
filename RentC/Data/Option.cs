using RentC.Data.Models;
using RentC.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Data
{
    public class Option
    {
        private readonly string _description;
        private int _id;

        private int _offset = -15; 
        private string _text_format_pattern = "{0, -15}";
        private string _dtformat = "yyyy-MM-dd";

        public Option()
        {
            _description = this.GetType().Name;
        }

        public Option(string desc)
        {
            _description = desc;
        }

        public int Offset
        {
            set{
                _offset = value;
                _text_format_pattern = string.Concat("{0, ", _offset, "}");
            }
        }

        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public string Description
        {
            get => _description;
        }
        public string DTFormat { get => _dtformat; set => _dtformat = value; }
        public string TextFormatPattern { get => _text_format_pattern; set => _text_format_pattern = value; }

        public enum Status
        {
            OPEN = 1,
            CLOSED = 2,
            CANCELED = 3
        }
        
    }
}
