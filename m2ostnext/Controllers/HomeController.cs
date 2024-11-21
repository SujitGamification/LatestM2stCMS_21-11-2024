using System;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult About()
        {
            ((dynamic)base.ViewBag).Message = "Your application description page.";
            return base.View();
        }

        public ActionResult Contact()
        {
            ((dynamic)base.ViewBag).Message = "Your contact page.";
            return base.View();
        }

        public ActionResult Index()
        {
            return base.View();
        }

        public ActionResult Logout(string session)
        {
            base.ViewData["type"] = session;
            return base.View();
        }
    }
}