using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RentC.Data
{ 
    public interface IOption
    {
        int ID
        {
            get;
            set;
        }
        string Description
        {
            get;
        }

        void Do();
    }
}
