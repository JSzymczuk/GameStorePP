using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Search",
                url: "Search/{pageNumber}/{pageSize}",
                defaults: new
                {
                    controller = "Search",
                    action = "Index",
                    pageSize = 0,
                    pageNumber = 0
                }
            );

            routes.MapRoute(
                name: "Product/Manage",
                url: "Product/Manage/{pageNumber}/{pageSize}",
                defaults: new
                {
                    controller = "Product",
                    action = "Manage",
                    pageSize = 0,
                    pageNumber = 0
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
