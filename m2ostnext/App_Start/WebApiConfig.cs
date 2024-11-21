using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace m2ostnext.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //////// Enable CORS for all origins, headers, and methods.
            //////var cors = new EnableCorsAttribute("*", "*", "*");
            //////config.EnableCors(cors);
            config.MapHttpAttributeRoutes();

            ////config.Routes.MapHttpRoute(
            ////    name: "DefaultApi",
            ////    routeTemplate: "api/{controller}/{id}",
            ////    defaults: new { id = RouteParameter.Optional }
            ////);

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               ////////routeTemplate: "api/{controller}/{id}",
               routeTemplate: "api/{controller}/{actions}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
        }
    }
}