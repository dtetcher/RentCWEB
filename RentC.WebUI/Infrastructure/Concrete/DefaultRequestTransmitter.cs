using RentC.WebUI.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebGrease.Css.ImageAssemblyAnalysis.LogModel;

namespace RentC.WebUI.Infrastructure.Concrete
{
    public class DefaultRequestTransmitter : IRequestTransmitter
    {
        public Dictionary<string, string> Kword_ControllerDictionary { get; set; }
        public Dictionary<string, string> Kword_ActionDictionary { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }

        public bool Determine(string optionName)
        {
            foreach(var controller in Kword_ControllerDictionary)
            {
                if (optionName.StartsWith(controller.Key))
                {
                    foreach(var action in Kword_ActionDictionary)
                    {
                        if (optionName.EndsWith(action.Key))
                        {
                            Controller = controller.Value;
                            Action = action.Value;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}