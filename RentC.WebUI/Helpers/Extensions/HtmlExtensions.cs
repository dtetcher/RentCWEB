using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RentC.WebUI.Helpers.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString ImageLink(this HtmlHelper helper, string LinkText, string ActionName,
            string ControllerName, object routeValues, object htmlAttributes, string ImagePath)
        {
            var UrlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var img = new TagBuilder("img");
            img.Attributes.Add("src", VirtualPathUtility.ToAbsolute(ImagePath));

            var anchor = new TagBuilder("a")
            {
                InnerHtml = img.ToString(TagRenderMode.SelfClosing)
            };
            anchor.Attributes["href"] = UrlHelper.Action(ActionName, ControllerName, routeValues);
            anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(anchor.ToString());
        }
    }
}