using m2ostnext.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static m2ostnext.Models.KPIDataLog;
using static m2ostnext.Models.ProgramReassigement;

namespace m2ostnext.Controllers
{
    public class LeaderBoardController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        // GET: LeaderBoard
        public ActionResult LeaderBoardIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            string orgid1 = userSession.id_ORGANIZATION;
            string id_user = userSession.ID_USER;
            string Page1 = "Leader Board";
            UserLogDetails userLogDetails = new UserLogDetails();
            userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
            return View();
        }

        [HttpPost]
        public JsonResult Getassesment()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            int orgid1 = Convert.ToInt32(userSession.id_ORGANIZATION);


            var list1 = this.db.tbl_assessment
                         .Where(t => t.status == "A" && t.id_organization == orgid1)
                         .OrderByDescending(t => t.id_organization)
                         .Select(t => new { t.id_assessment, t.assessment_title })
                         .ToList();

            if (list1.Count == 0)
            {
                return Json(list1);
            }
            string json1 = JsonConvert.SerializeObject(list1);
            return Json(json1);
        }

        [HttpPost]
        public JsonResult GetassesmentID(int program, int assesmentID, string startDate, string expiryDate)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            string orgid1 = userSession.id_ORGANIZATION;

            if (program == 1)
            {
                KPIDataLogt_service getPoint = new KPIDataLogt_service();
                System.Data.DataTable GetList = getPoint.GetPontlist(orgid1, program, assesmentID,startDate, expiryDate);

                if(GetList.Rows.Count == 0)
                {
                    int Data = 0;
                    return Json(Data);
                }
                else
                {
                    int Data = 1;
                    return Json(Data);
                }
            }
            else
            {
                KPIDataLogt_service getPoint = new KPIDataLogt_service();
                System.Data.DataTable GetList = getPoint.GetCoinlist(orgid1, program, assesmentID, startDate, expiryDate);

                if (GetList.Rows.Count == 0)
                {
                    int Data = 0;
                    return Json(Data);
                }
                else
                {
                    int Data = 1;
                    return Json(Data);
                }
            }

        }
    }
}