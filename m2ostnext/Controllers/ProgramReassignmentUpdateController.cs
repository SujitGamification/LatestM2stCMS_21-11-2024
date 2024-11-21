using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static m2ostnext.Models.ProgramReassigement;

namespace m2ostnext.Controllers
{
    public class ProgramReassignmentUpdateController : Controller
    {
        // GET: ProgramReassignmentUpdate
        public ActionResult Index()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            string orgid1 = userSession.id_ORGANIZATION;
            string id_user = userSession.ID_USER;
            string Page1 = "Program Movement";
            UserLogDetails userLogDetails = new UserLogDetails();
            userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
            return View();
        }

        [HttpPost]
        public JsonResult GetCategorylistInsert(int program, int title, int category, string startDate, string expiryDate)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            string orgid1 = userSession.id_ORGANIZATION;


            ProgramReassigement_service getProgramReassigement = new ProgramReassigement_service();
            System.Data.DataTable GetgameList = getProgramReassigement.GetCategorylistInsert(orgid1, program, title, category, startDate, expiryDate);

            if (GetgameList.Rows.Count == 0)
            {
                int Data = 0;
                return Json(Data);
            }
            else
            {
                int Data = 1;
                return Json(Data);
            }
            //string json = JsonConvert.SerializeObject(GetgameList, Formatting.Indented);

            //// Serialize the list to JSON and return
            //return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}