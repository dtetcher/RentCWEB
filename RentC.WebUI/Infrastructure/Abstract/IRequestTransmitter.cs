using RentC.WebUI.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.WebUI.Infrastructure.Abstract
{
    public interface IRequestTransmitter
    {
        public Dictionary<string, string> Kword_ControllerDictionary { get; set; }
        public Dictionary<string, string> Kword_ActionDictionary { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }

        public bool Determine(string optionName);
    }
}
