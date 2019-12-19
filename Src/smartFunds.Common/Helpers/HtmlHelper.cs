using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace smartFunds.Common.Helpers
{
    public static class HtmlHelper
    {
        public static string IsActived(this IHtmlHelper htmlHelper, string controller, string action, string cssClass = "current")
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = controller == routeControl &&
                               action == routeAction;

            return returnActive ? cssClass : "";
        }
    }
}
