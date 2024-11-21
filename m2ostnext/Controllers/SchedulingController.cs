// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.SchedulingController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class SchedulingController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index(int flag = 0)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Scheduling DashBoard";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<EventModel> eventStatusList = new ContentReportModel().getEventStatusList("" + " SELECT  b.id_scheduled_event ID_EVENT,    b.event_title EVENT_NAME,  " + "   CASE  WHEN b.event_type = 1 THEN 'OPEN'   ELSE 'CLOSE'  END EVENT_TYPE, count(*) TOTAL_COUNT,  " + " CASE   WHEN  b.event_group_type = 1 THEN 'Face to Face'   WHEN b.event_group_type = 2 THEN 'Online' WHEN b.event_group_type = 3 THEN 'M2OST' ELSE 'NA'  END EVENT_GROUP_TYPE, " + " CONCAT(e.USERNAME, ' - ', e.employee_id) EVENT_CREATOR,  DATE_FORMAT(b.event_start_datetime, '%d-%m-%Y') EVENT_DATE,   " + " CASE   WHEN b.event_type = 1 THEN b.no_of_participants   ELSE 0  END NO_OF_USER,  SUM(CASE   WHEN subscription_status = 'A' THEN 1   ELSE 0  END) APPROVAL_COUNT, " + " SUM(CASE   WHEN subscription_status = 'P' THEN 1   ELSE 0  END) PENDING_COUNT, " + "   SUM(CASE   WHEN subscription_status = 'R' THEN 1   ELSE 0  END) REJECT_COUNT , b.status STATUS " + " FROM  tbl_scheduled_event_subscription_log a,  tbl_scheduled_event b,  tbl_user c,  tbl_profile d,  tbl_cms_users e " + " where b.id_organization=" + (object) int32 + " and a.id_scheduled_event=b.id_scheduled_event   and a.id_user = c.ID_USER   and c.id_user = d.ID_USER   and b.id_event_creator = e.ID_USER group by b.id_scheduled_event ");
      object[] objArray = new object[4]
      {
        (object) ("" + " SELECT  b.id_scheduled_event ID_EVENT,    b.event_title EVENT_NAME,  " + "   CASE  WHEN b.event_type = 1 THEN 'OPEN'   ELSE 'CLOSE'  END EVENT_TYPE, count(*) TOTAL_COUNT,  " + " CASE   WHEN  b.event_group_type = 1 THEN 'Face to Face'   WHEN b.event_group_type = 2 THEN 'Online' WHEN b.event_group_type = 3 THEN 'M2OST' ELSE 'NA'  END EVENT_GROUP_TYPE, " + " CONCAT(e.USERNAME, ' - ', e.employee_id) EVENT_CREATOR,  DATE_FORMAT(b.event_start_datetime, '%d-%m-%Y') EVENT_DATE,   " + " CASE   WHEN b.event_type = 1 THEN b.no_of_participants   ELSE 0  END NO_OF_USER,  0 APPROVAL_COUNT, " + " 0 PENDING_COUNT, 0 REJECT_COUNT , b.status STATUS " + " FROM    tbl_scheduled_event_subscription_log a,    tbl_scheduled_event b,    tbl_user c,    tbl_profile d,    tbl_cms_users e "),
        (object) "  WHERE  b.id_organization=",
        (object) int32,
        (object) "  AND c.id_user = d.ID_USER        AND b.id_event_creator = e.ID_USER       "
      };
      foreach (EventModel emptyEventStatus in new ContentReportModel().getEmptyEventStatusList(string.Concat(objArray) + " and b.id_scheduled_event not in (select distinct id_scheduled_event from tbl_scheduled_event_subscription_log) GROUP BY b.id_scheduled_event "))
        eventStatusList.Add(emptyEventStatus);
      this.ViewData["eModel"] = (object) eventStatusList.OrderByDescending<EventModel, int>((Func<EventModel, int>) (t => t.ID_EVENT)).ToList<EventModel>();
      this.ViewData[nameof (flag)] = (object) flag;
      return (ActionResult) this.View();
    }

    public ActionResult ApprovalIndex()
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Approval DashBoard";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int oid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      this.ViewData["eventlist"] = (object) this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_organization == (int?) oid && t.event_type == (int?) 1 && t.status == "A")).ToList<tbl_scheduled_event>();
      return (ActionResult) this.View();
    }

    public ActionResult createEvent()
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add Event";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int orgid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object) orgid + " AND CATEGORY_TYPE = 0 AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
      List<tbl_category> list2 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object) orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
      List<tbl_assessment> list3 = this.db.tbl_assessment.SqlQuery("select * from tbl_assessment where ID_ORGANIZATION=" + (object) orgid + " AND status='A' order by assessment_title").ToList<tbl_assessment>();
      this.ViewData["designation"] = (object) this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => t.STATUS == "A" && t.ID_ORGANIZATION == (int?) orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>) (p => p.user_designation)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))).Distinct<string>().ToList<string>();
      this.ViewData["program"] = (object) list1;
      this.ViewData["category-list"] = (object) list2;
      this.ViewData["assessment"] = (object) list3;
      this.ViewData["categorytile-list"] = (object) this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) orgid && t.category_theme == (int?) 1)).ToList<tbl_category_tiles>();
      return (ActionResult) this.View();
    }

    public ActionResult createEventAction()
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "ADD EVENT";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      DateTime dateTime1 = new DateTime();
      int num6 = 0;
      string str5 = "2";
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      string str6 = this.Request.Form["event-title"].ToString();
      string str7 = this.Request.Form["event-desc"].ToString();
      string str8 = this.Request.Form["event-objective"].ToString();
      DateTime datetime1 = new Utility().StringToDatetime(this.Request.Form["event-reg-start"].ToString());
      DateTime datetime2 = new Utility().StringToDatetime(this.Request.Form["event-reg-end"].ToString());
      DateTime datetime3 = new Utility().StringToDatetime(this.Request.Form["event-time"].ToString());
      string str9 = "";
      int int32_3 = Convert.ToInt32(this.Request.Form["event-group-type"]);
      string str10 = "";
      string str11 = this.Request.Form["event-program-facilitator"].ToString();
      string str12 = this.Request.Form["event-program-f-org"].ToString();
      int int32_4 = Convert.ToInt32(this.Request.Form["event-duration-flag"]);
      DateTime dateTime2 = new DateTime();
      DateTime dateTime3;
      int months;
      string str13;
      if (int32_4 == 2)
      {
        dateTime3 = new Utility().StringToDatetime(this.Request.Form["event-time"].ToString());
        months = Convert.ToInt32(this.Request.Form["program-duration"]);
        str13 = this.Request.Form["program-duration-unit"].ToString();
        switch (str13)
        {
          case "H":
            dateTime2 = dateTime3.AddHours((double) months);
            str9 = months.ToString() + " Hour";
            break;
          case "D":
            dateTime2 = dateTime3.AddDays((double) months);
            str9 = months.ToString() + " Day";
            break;
          case "W":
            dateTime2 = dateTime3.AddDays((double) (months * 7));
            str9 = months.ToString() + " Week";
            break;
          case "M":
            dateTime2 = dateTime3.AddMonths(months);
            str9 = months.ToString() + " Month";
            break;
        }
      }
      else
      {
        dateTime3 = DateTime.Now;
        months = 0;
        str13 = "NA";
        str9 = "No time Limit.";
      }
      switch (int32_3)
      {
        case 1:
          str1 = this.Request.Form["event-location"].ToString();
          str10 = this.Request.Form["event-venue"].ToString();
          str2 = this.Request.Form["f2f-aditional"].ToString();
          break;
        case 2:
          str3 = this.Request.Form["event-location"].ToString();
          str4 = this.Request.Form["f2f-aditional"].ToString();
          break;
        case 3:
          num6 = Convert.ToInt32(this.Request.Form["event-attachment-type"]);
          switch (num6)
          {
            case 1:
              num2 = Convert.ToInt32(this.Request.Form["tile-category"]);
              num3 = Convert.ToInt32(this.Request.Form["heading-category"]);
              num1 = Convert.ToInt32(this.Request.Form["event-category-program"]);
              num5 = 0;
              break;
            case 2:
              num1 = 0;
              num5 = Convert.ToInt32(this.Request.Form["event-category-assessment"]);
              num4 = Convert.ToInt32(this.Request.Form["event-assessment-category"]);
              break;
          }
          break;
      }
      int int32_5 = Convert.ToInt32(this.Request.Form["event-type-flag"]);
      int num7 = 0;
      string str14 = "";
      switch (int32_5)
      {
        case 1:
          num7 = Convert.ToInt32(this.Request.Form["event-participant"]);
          str14 = this.Request.Form["event-participant-level"].ToString();
          break;
        case 2:
          num7 = 0;
          str14 = "";
          break;
      }
      string str15 = this.Request.Form["event-response-flag"].ToString();
      if (str15 == "1")
        str5 = this.Request.Form["event-approval-flag"].ToString();
      string str16 = this.Request.Form["event-unsubscribe-flag"].ToString();
      tbl_scheduled_event entity = new tbl_scheduled_event();
      entity.event_title = str6;
      entity.id_organization = new int?(int32_1);
      entity.event_description = str7;
      entity.registration_start_date = new DateTime?(datetime1);
      entity.registration_end_date = new DateTime?(datetime2);
      entity.event_start_datetime = new DateTime?(datetime3);
      entity.event_duration = str9;
      entity.event_group_type = new int?(int32_3);
      entity.program_name = str6;
      entity.program_objective = str8;
      entity.program_description = "NA";
      entity.facilitator_name = str11;
      entity.facilitator_organization = str12;
      entity.program_location = str1;
      entity.program_venue = str10;
      entity.attachment_type = new int?(num6);
      entity.is_approval = str5;
      entity.is_response = str15;
      entity.is_unsubscribe = str16;
      entity.event_online_url = "";
      switch (int32_3)
      {
        case 1:
          entity.event_additional_info = str2;
          break;
        case 2:
          entity.event_additional_info = str3;
          entity.event_online_url = this.Request.Form["event-url"].ToString();
          break;
        case 3:
          entity.event_additional_info = "";
          break;
      }
      entity.id_category_tile = new int?(num2);
      entity.id_category_heading = new int?(num3);
      entity.id_category = new int?(num4);
      entity.id_program = new int?(num1);
      entity.id_assessment = new int?(num5);
      entity.event_online_url = str4;
      entity.program_duration_type = new int?(int32_4);
      entity.program_duration = new int?(months);
      entity.program_duration_unit = str13;
      entity.program_start_date = new DateTime?(dateTime3);
      entity.program_end_date = new DateTime?(dateTime2);
      entity.id_event_creator = new int?(int32_2);
      entity.event_type = new int?(int32_5);
      entity.no_of_participants = new int?(num7);
      entity.status = "P";
      entity.updated_date_time = new DateTime?(DateTime.Now);
      entity.participant_level = str14;
      this.db.tbl_scheduled_event.Add(entity);
      this.db.SaveChanges();
      if (entity.id_scheduled_event <= 0)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling", (object) new
        {
          flag = 1
        });
      if (int32_3 != 3)
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
          string str17 = System.Web.HttpContext.Current.Request["id"];
          if (file != null)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/SCHEDULING/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/SCHEDULING/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/SCHEDULING/"), "SEP" + (object) entity.id_scheduled_event + extension);
            file.SaveAs(filename);
            entity.program_image = "SEP" + (object) entity.id_scheduled_event + extension;
            this.db.SaveChanges();
          }
        }
      }
      if (int32_5 == 1)
        this.setOpenEventToParticipants(entity.id_scheduled_event);
      return (ActionResult) this.RedirectToAction("LoadEvent", "Scheduling", (object) new
      {
        id = entity.id_scheduled_event,
        flag = 1
      });
    }

    public ActionResult editEvent(string id)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      int eid = Convert.ToInt32(id);
      int uid = Convert.ToInt32(content.ID_USER);
      string str1 = "";
      int num1 = 0;
      int num2 = 0;
      tbl_category_heading tblCategoryHeading1 = (tbl_category_heading) null;
      tbl_cms_users tblCmsUsers1 = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_cms_users>();
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eid)).FirstOrDefault<tbl_scheduled_event>();
      tbl_cms_users tblCmsUsers2 = (tbl_cms_users) null;
      if (sEvent == null)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      int? idEventCreator = sEvent.id_event_creator;
      int num3 = uid;
      if ((idEventCreator.GetValueOrDefault() == num3 ? (idEventCreator.HasValue ? 1 : 0) : 0) != 0 && sEvent.status == "P")
        num2 = 1;
      int? attachmentType1 = sEvent.attachment_type;
      int num4 = 1;
      if ((attachmentType1.GetValueOrDefault() == num4 ? (attachmentType1.HasValue ? 1 : 0) : 0) != 0)
      {
        tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => (int?) t.ID_CATEGORY == sEvent.id_program && t.STATUS == "A" && t.CATEGORY_TYPE == (int?) 0)).FirstOrDefault<tbl_category>();
        tbl_category_tiles tblCategoryTiles = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => (int?) t.id_category_tiles == sEvent.id_category_tile && t.status == "A")).FirstOrDefault<tbl_category_tiles>();
        tbl_category_heading tblCategoryHeading2 = this.db.tbl_category_heading.Where<tbl_category_heading>((Expression<Func<tbl_category_heading, bool>>) (t => (int?) t.id_category_heading == sEvent.id_category_heading && t.status == "A")).FirstOrDefault<tbl_category_heading>();
        if (tblCategory != null)
        {
          num1 = tblCategory.ID_CATEGORY;
          str1 = tblCategory.CATEGORYNAME + " [Tile : " + tblCategoryTiles.tile_heading + " , Heading : " + tblCategoryHeading2.Heading_title + " ]";
        }
      }
      else
      {
        int? attachmentType2 = sEvent.attachment_type;
        int num5 = 2;
        if ((attachmentType2.GetValueOrDefault() == num5 ? (attachmentType2.HasValue ? 1 : 0) : 0) != 0)
        {
          tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => (int?) t.id_assessment == sEvent.id_assessment && t.status == "A")).FirstOrDefault<tbl_assessment>();
          tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => (int?) t.ID_CATEGORY == sEvent.id_category && t.STATUS == "A")).FirstOrDefault<tbl_category>();
          if (tblAssessment != null)
          {
            num1 = tblAssessment.id_assessment;
            str1 = tblAssessment.assessment_title + " [ " + tblCategory.CATEGORYNAME + " ]";
          }
        }
      }
      this.ViewData["designation"] = (object) this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => t.STATUS == "A" && t.ID_ORGANIZATION == (int?) oid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>) (p => p.user_designation)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))).Distinct<string>().ToList<string>();
      this.ViewData["cHeading"] = (object) tblCategoryHeading1;
      this.ViewData["event"] = (object) sEvent;
      this.ViewData["eventCreator"] = (object) tblCmsUsers2;
      this.ViewData["attachment"] = (object) str1;
      this.ViewData["attachmentid"] = (object) num1;
      this.ViewData["publishFlag"] = (object) num2;
      tbl_user tblUser = this.db.tbl_user.SqlQuery("select * from tbl_user where upper(EMPLOYEEID) like upper('" + tblCmsUsers1.employee_id + "')  limit 1 ").FirstOrDefault<tbl_user>();
      string str2 = "0";
      if (tblUser != null)
        str2 = "1";
      this.ViewData["approval_check"] = (object) str2;
      return (ActionResult) this.View();
    }

    public ActionResult editEventAction()
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      DateTime dateTime1 = new DateTime();
      int num6 = 0;
      string str5 = "0";
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      int int32_1 = Convert.ToInt32(content.ID_USER);
      int id_scheduled_event = Convert.ToInt32(this.Request.Form["id_scheduled_event"]);
      tbl_scheduled_event tblScheduledEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == id_scheduled_event)).FirstOrDefault<tbl_scheduled_event>();
      string str6 = this.Request.Form["event-title"].ToString();
      string str7 = this.Request.Form["event-desc"].ToString();
      string str8 = this.Request.Form["event-objective"].ToString();
      DateTime datetime1 = new Utility().StringToDatetime(this.Request.Form["event-reg-start"].ToString());
      DateTime datetime2 = new Utility().StringToDatetime(this.Request.Form["event-reg-end"].ToString());
      DateTime datetime3 = new Utility().StringToDatetime(this.Request.Form["event-time"].ToString());
      string str9 = "";
      int int32_2 = Convert.ToInt32(this.Request.Form["event-group-type"]);
      string str10 = "";
      string str11 = this.Request.Form["event-program-facilitator"].ToString();
      string str12 = this.Request.Form["event-program-f-org"].ToString();
      int int32_3 = Convert.ToInt32(this.Request.Form["event-duration-flag"]);
      DateTime dateTime2 = new DateTime();
      DateTime dateTime3;
      int months;
      string str13;
      if (int32_3 == 2)
      {
        dateTime3 = new Utility().StringToDatetime(this.Request.Form["event-time"].ToString());
        months = Convert.ToInt32(this.Request.Form["program-duration"]);
        str13 = this.Request.Form["program-duration-unit"].ToString();
        switch (str13)
        {
          case "H":
            dateTime2 = dateTime3.AddHours((double) months);
            str9 = months.ToString() + " Hour";
            break;
          case "D":
            dateTime2 = dateTime3.AddDays((double) months);
            str9 = months.ToString() + " Day";
            break;
          case "W":
            dateTime2 = dateTime3.AddDays((double) (months * 7));
            str9 = months.ToString() + " Week";
            break;
          case "M":
            dateTime2 = dateTime3.AddMonths(months);
            str9 = months.ToString() + " Month";
            break;
        }
      }
      else
      {
        dateTime3 = DateTime.Now;
        months = 0;
        str13 = "NA";
        str9 = "No time Limit.";
      }
      switch (int32_2)
      {
        case 1:
          str1 = this.Request.Form["event-location"].ToString();
          str10 = this.Request.Form["event-venue"].ToString();
          str2 = this.Request.Form["f2f-aditional"].ToString();
          break;
        case 2:
          str3 = this.Request.Form["event-location"].ToString();
          str4 = this.Request.Form["f2f-aditional"].ToString();
          break;
        case 3:
          num6 = Convert.ToInt32(this.Request.Form["event-attachment-type"]);
          switch (num6)
          {
            case 1:
              num2 = Convert.ToInt32(this.Request.Form["tile-category"]);
              num3 = Convert.ToInt32(this.Request.Form["heading-category"]);
              num1 = Convert.ToInt32(this.Request.Form["event-category-program"]);
              num5 = 0;
              break;
            case 2:
              num1 = 0;
              num5 = Convert.ToInt32(this.Request.Form["event-category-assessment"]);
              num4 = Convert.ToInt32(this.Request.Form["event-assessment-category"]);
              break;
          }
          break;
      }
      int int32_4 = Convert.ToInt32(this.Request.Form["event-type-flag"]);
      int num7 = 0;
      string str14 = "";
      switch (int32_4)
      {
        case 1:
          num7 = Convert.ToInt32(this.Request.Form["event-participant"]);
          str14 = this.Request.Form["event-participant-level"].ToString();
          break;
        case 2:
          num7 = 0;
          str14 = "";
          break;
      }
      string str15 = this.Request.Form["event-response-flag"].ToString();
      if (str15 == "1")
        str5 = this.Request.Form["event-approval-flag"].ToString();
      string str16 = this.Request.Form["event-unsubscribe-flag"].ToString();
      tblScheduledEvent.event_title = str6;
      tblScheduledEvent.event_description = str7;
      tblScheduledEvent.registration_start_date = new DateTime?(datetime1);
      tblScheduledEvent.registration_end_date = new DateTime?(datetime2);
      tblScheduledEvent.event_start_datetime = new DateTime?(datetime3);
      tblScheduledEvent.event_duration = str9;
      tblScheduledEvent.event_group_type = new int?(int32_2);
      tblScheduledEvent.program_name = str6;
      tblScheduledEvent.program_objective = str8;
      tblScheduledEvent.program_description = "NA";
      tblScheduledEvent.facilitator_name = str11;
      tblScheduledEvent.facilitator_organization = str12;
      tblScheduledEvent.program_location = str1;
      tblScheduledEvent.program_venue = str10;
      tblScheduledEvent.attachment_type = new int?(num6);
      tblScheduledEvent.is_approval = str5;
      tblScheduledEvent.is_response = str15;
      tblScheduledEvent.is_unsubscribe = str16;
      tblScheduledEvent.event_online_url = "";
      switch (int32_2)
      {
        case 1:
          tblScheduledEvent.event_additional_info = str2;
          break;
        case 2:
          tblScheduledEvent.event_additional_info = str3;
          tblScheduledEvent.event_online_url = this.Request.Form["event-url"].ToString();
          break;
        case 3:
          tblScheduledEvent.event_additional_info = "";
          break;
      }
      tblScheduledEvent.id_category_tile = new int?(num2);
      tblScheduledEvent.id_category_heading = new int?(num3);
      tblScheduledEvent.id_category = new int?(num4);
      tblScheduledEvent.id_program = new int?(num1);
      tblScheduledEvent.id_assessment = new int?(num5);
      tblScheduledEvent.event_online_url = str4;
      tblScheduledEvent.program_duration_type = new int?(int32_3);
      tblScheduledEvent.program_duration = new int?(months);
      tblScheduledEvent.program_duration_unit = str13;
      tblScheduledEvent.program_start_date = new DateTime?(dateTime3);
      tblScheduledEvent.program_end_date = new DateTime?(dateTime2);
      tblScheduledEvent.id_event_creator = new int?(int32_1);
      tblScheduledEvent.event_type = new int?(int32_4);
      tblScheduledEvent.no_of_participants = new int?(num7);
      tblScheduledEvent.updated_date_time = new DateTime?(DateTime.Now);
      tblScheduledEvent.participant_level = str14;
      this.db.SaveChanges();
      if (tblScheduledEvent.id_scheduled_event <= 0)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      if (int32_2 != 3)
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
          string str17 = System.Web.HttpContext.Current.Request["id"];
          if (file != null)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/SCHEDULING/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/SCHEDULING/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/SCHEDULING/"), "SEP" + (object) tblScheduledEvent.id_scheduled_event + extension);
            file.SaveAs(filename);
            tblScheduledEvent.program_image = "SEP" + (object) tblScheduledEvent.id_scheduled_event + extension;
            this.db.SaveChanges();
          }
        }
      }
      return (ActionResult) this.RedirectToAction("LoadEvent", "Scheduling", (object) new
      {
        id = tblScheduledEvent.id_scheduled_event
      });
    }

    public ActionResult ScheduleEvent() => (ActionResult) this.View();

    public ActionResult LoadEvent(string id, int flag = 0)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      int eid = Convert.ToInt32(id);
      int uid = Convert.ToInt32(content.ID_USER);
      string str1 = "";
      int num1 = 0;
      int num2 = 0;
      tbl_cms_users tblCmsUsers1 = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_cms_users>();
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eid)).FirstOrDefault<tbl_scheduled_event>();
      tbl_cms_users tblCmsUsers2 = (tbl_cms_users) null;
      if (sEvent == null)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      tblCmsUsers2 = new tbl_cms_users();
      tbl_cms_users tblCmsUsers3 = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => (int?) t.ID_USER == sEvent.id_event_creator)).FirstOrDefault<tbl_cms_users>();
      int? idEventCreator = sEvent.id_event_creator;
      int num3 = uid;
      if ((idEventCreator.GetValueOrDefault() == num3 ? (idEventCreator.HasValue ? 1 : 0) : 0) != 0 && sEvent.status == "P")
        num2 = 1;
      int? attachmentType1 = sEvent.attachment_type;
      int num4 = 1;
      if ((attachmentType1.GetValueOrDefault() == num4 ? (attachmentType1.HasValue ? 1 : 0) : 0) != 0)
      {
        tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => (int?) t.ID_CATEGORY == sEvent.id_program && t.STATUS == "A" && t.CATEGORY_TYPE == (int?) 0)).FirstOrDefault<tbl_category>();
        tbl_category_tiles tblCategoryTiles = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => (int?) t.id_category_tiles == sEvent.id_category_tile && t.status == "A")).FirstOrDefault<tbl_category_tiles>();
        tbl_category_heading tblCategoryHeading = this.db.tbl_category_heading.Where<tbl_category_heading>((Expression<Func<tbl_category_heading, bool>>) (t => (int?) t.id_category_heading == sEvent.id_category_heading && t.status == "A")).FirstOrDefault<tbl_category_heading>();
        if (tblCategory != null)
        {
          num1 = tblCategory.ID_CATEGORY;
          str1 = tblCategory.CATEGORYNAME + " [Tile : " + tblCategoryTiles.tile_heading + " , Heading : " + tblCategoryHeading.Heading_title + " ]";
        }
      }
      else
      {
        int? attachmentType2 = sEvent.attachment_type;
        int num5 = 2;
        if ((attachmentType2.GetValueOrDefault() == num5 ? (attachmentType2.HasValue ? 1 : 0) : 0) != 0)
        {
          tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => (int?) t.id_assessment == sEvent.id_assessment && t.status == "A")).FirstOrDefault<tbl_assessment>();
          tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => (int?) t.ID_CATEGORY == sEvent.id_category && t.STATUS == "A")).FirstOrDefault<tbl_category>();
          if (tblAssessment != null)
          {
            num1 = tblAssessment.id_assessment;
            str1 = tblAssessment.assessment_title + " [ " + tblCategory.CATEGORYNAME + " ]";
          }
        }
      }
      string str2 = "";
      List<tbl_scheduled_event_subscription_log> list = this.db.tbl_scheduled_event_subscription_log.SqlQuery("select * from tbl_scheduled_event_subscription_log " + " where id_scheduled_event=" + (object) sEvent.id_scheduled_event + " AND  id_user in (select id_user from tbl_user where status='A' and id_organization=" + (object) oid + ")" + " and subscription_status not in ('O')").ToList<tbl_scheduled_event_subscription_log>();
      bool flag1 = false;
      int num6 = 0;
      int? nullable = sEvent.event_type;
      int num7 = 1;
      if ((nullable.GetValueOrDefault() == num7 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
      {
        num6 = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.subscription_status == "A")).Count<tbl_scheduled_event_subscription_log>();
        int num8 = num6;
        nullable = sEvent.no_of_participants;
        int valueOrDefault = nullable.GetValueOrDefault();
        if ((num8 >= valueOrDefault ? (nullable.HasValue ? 1 : 0) : 0) != 0)
          flag1 = true;
      }
      foreach (tbl_scheduled_event_subscription_log eventSubscriptionLog in list)
      {
        tbl_scheduled_event_subscription_log check = eventSubscriptionLog;
        tbl_user item = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => (int?) t.ID_USER == check.id_user && t.ID_ORGANIZATION == (int?) oid)).FirstOrDefault<tbl_user>();
        tbl_profile tblProfile1 = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
        if (item != null)
        {
          string str3 = "";
          if (Convert.ToInt32((object) item.reporting_manager) > 0)
          {
            tbl_user rm = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => (int?) t.ID_USER == item.reporting_manager)).FirstOrDefault<tbl_user>();
            if (rm != null)
            {
              tbl_profile tblProfile2 = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == rm.ID_USER)).FirstOrDefault<tbl_profile>();
              if (tblProfile2 != null)
                str3 = tblProfile2.FIRSTNAME + " " + tblProfile2.LASTNAME + " [ " + rm.USERID + " ]";
              else
                str3 = rm.USERID;
            }
          }
          else
            str3 = "No Reporting Manager.";
          str2 += "<tr><td>";
          if (tblProfile1 != null)
            str2 = str2 + tblProfile1.FIRSTNAME + " " + tblProfile1.LASTNAME + " [ " + item.USERID + " ]";
          else
            str2 += item.USERID;
          str2 += "</td>";
          str2 += "<td>";
          str2 += str3;
          str2 += "</td>";
          str2 += "<td>";
          if (check.subscription_status == "P")
          {
            nullable = sEvent.event_type;
            int num9 = 1;
            str2 = (nullable.GetValueOrDefault() == num9 ? (nullable.HasValue ? 1 : 0) : 0) == 0 ? str2 + "Subscription Not Approved yet by RM" : (!flag1 ? str2 + "Subscription Not Approved yet by RM" : str2 + "Put on Waiting List");
          }
          else if (check.subscription_status == "R")
            str2 += " Rejected by User ";
          else if (check.subscription_status == "C")
            str2 = str2 + " Rejected by RM for '" + check.event_user_comment + "' ";
          else if (check.subscription_status == "A")
            str2 += "Subscribed.";
          else if (check.subscription_status == "L")
            str2 = str2 ?? "";
          str2 += "</td>";
          str2 += "</tr>";
        }
      }
      this.ViewData["tableData"] = (object) ("<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"40%\">User Info</th>" + " <th width=\"30%\">Reporting Manager</th>" + "<th  width=\"30%\">Status</th>" + "</tr></thead>" + "<tbody>" + str2 + "</tbody></table>");
      this.ViewData["event"] = (object) sEvent;
      this.ViewData["eventCreator"] = (object) tblCmsUsers3;
      this.ViewData["fullFlag"] = (object) flag1;
      this.ViewData["event_count"] = (object) num6;
      this.ViewData["attachment"] = (object) str1;
      this.ViewData["attachmentid"] = (object) num1;
      this.ViewData["publishFlag"] = (object) num2;
      tbl_user tblUser = this.db.tbl_user.SqlQuery("select * from tbl_user where upper(EMPLOYEEID) like upper('" + tblCmsUsers1.employee_id + "') limit 1 ").FirstOrDefault<tbl_user>();
      string str4 = "0";
      if (tblUser != null)
        str4 = "1";
      this.ViewData["approval_check"] = (object) str4;
      this.ViewData[nameof (flag)] = (object) flag;
      return (ActionResult) this.View();
    }

    public ActionResult PublishEvent(string id)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      int eid = Convert.ToInt32(id);
      int uid = Convert.ToInt32(content.ID_USER);
      this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_cms_users>();
      tbl_scheduled_event tblScheduledEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eid)).FirstOrDefault<tbl_scheduled_event>();
      if (tblScheduledEvent == null)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      tblScheduledEvent.status = "A";
      tblScheduledEvent.updated_date_time = new DateTime?(DateTime.Now);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("LoadEvent", "Scheduling", (object) new
      {
        id = tblScheduledEvent.id_scheduled_event
      });
    }

    public ActionResult ApprovalDashboard(string id)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      int eid = Convert.ToInt32(id);
      int uid = Convert.ToInt32(content.ID_USER);
      bool flag = false;
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eid && t.status != "X")).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent == null)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_cms_users>();
      tbl_user tblUser = this.db.tbl_user.SqlQuery("select * from tbl_user where status='A' and id_organization=" + (object) oid + " and upper(EMPLOYEEID) like upper('" + tblCmsUsers.employee_id + "')").FirstOrDefault<tbl_user>();
      if (tblUser == null)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      int? eventType = sEvent.event_type;
      int num1 = 1;
      if ((eventType.GetValueOrDefault() == num1 ? (eventType.HasValue ? 1 : 0) : 0) == 0)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      int num2 = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.subscription_status == "A")).Count<tbl_scheduled_event_subscription_log>();
      int num3 = num2;
      int? noOfParticipants = sEvent.no_of_participants;
      int valueOrDefault = noOfParticipants.GetValueOrDefault();
      if ((num3 >= valueOrDefault ? (noOfParticipants.HasValue ? 1 : 0) : 0) != 0)
        flag = true;
      string str1 = "";
      List<tbl_scheduled_event_subscription_log> list = this.db.tbl_scheduled_event_subscription_log.SqlQuery("select * from tbl_scheduled_event_subscription_log " + " where id_scheduled_event=" + (object) sEvent.id_scheduled_event + " AND  id_user in (select id_user from tbl_user where status='A' and id_organization=" + (object) oid + " and reporting_manager=" + (object) tblUser.ID_USER + ")" + " and subscription_status not in ('O')").ToList<tbl_scheduled_event_subscription_log>();
      this.db.tbl_user.SqlQuery("select * from tbl_user where  status='A' and id_organization=" + (object) oid + " and reporting_manager=" + (object) tblUser.ID_USER ?? "").ToList<tbl_user>();
      foreach (tbl_scheduled_event_subscription_log eventSubscriptionLog in list)
      {
        tbl_scheduled_event_subscription_log check = eventSubscriptionLog;
        tbl_user item = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => (int?) t.ID_USER == check.id_user && t.ID_ORGANIZATION == (int?) oid)).FirstOrDefault<tbl_user>();
        tbl_profile tblProfile1 = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
        if (check != null && item != null)
        {
          string str2 = "";
          if (Convert.ToInt32((object) item.reporting_manager) > 0)
          {
            tbl_user rm = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => (int?) t.ID_USER == item.reporting_manager)).FirstOrDefault<tbl_user>();
            if (rm != null)
            {
              tbl_profile tblProfile2 = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == rm.ID_USER)).FirstOrDefault<tbl_profile>();
              if (tblProfile2 != null)
                str2 = tblProfile2.FIRSTNAME + " " + tblProfile2.LASTNAME + " [ " + rm.USERID + " ]";
              else
                str2 = rm.USERID;
            }
          }
          else
            str2 = "No Reporting Manager.";
          str1 += "<tr><td>";
          if (tblProfile1 != null)
            str1 = str1 + tblProfile1.FIRSTNAME + " " + tblProfile1.LASTNAME + " [ " + item.USERID + " ]";
          else
            str1 += item.USERID;
          str1 += "</td>";
          str1 += "<td>";
          str1 += str2;
          str1 += "</td>";
          str1 = str1 + "<td>" + check.event_sent_timestamp.Value.ToString("dd-MM-yyyy HH:mm") + "</td>";
          str1 += "<td>";
          if (check.subscription_status == "P")
          {
            if (flag)
            {
              str1 += "Put on Waiting List";
            }
            else
            {
              str1 = str1 + " <a id=\"linka_" + (object) item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"addUserToEvent('" + (object) item.ID_USER + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a>";
              str1 = str1 + "<i style=\"display:none;\" id=\"done_" + (object) item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
              str1 = str1 + " <a id=\"linkr_" + (object) item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"rejectUserToEventModel('" + (object) item.ID_USER + "','" + item.USERID + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
            }
          }
          else if (check.subscription_status == "O")
            str1 += "Subscription request Not Recieved. ";
          else if (check.subscription_status == "R")
            str1 += " Rejected by User ";
          else if (check.subscription_status == "C")
            str1 = str1 + " Rejected by RM for '" + check.event_user_comment + "' ";
          else if (check.subscription_status == "A")
            str1 += "Subscription Request Approved";
          else if (check.subscription_status == "L")
            str1 = str1 ?? "";
          str1 += "</td>";
          str1 += "</tr>";
        }
      }
      this.ViewData["tableData"] = (object) ("<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"35%\">User Info</th>" + " <th width=\"30%\">Rep Manager</th>" + " <th width=\"15%\">Sent Date-Time</th>" + "<th  width=\"20%\">Status</th>" + "</tr></thead>" + "<tbody>" + str1 + "</tbody></table>");
      this.ViewData["fullFlag"] = (object) flag;
      this.ViewData["event_count"] = (object) num2;
      this.ViewData["event"] = (object) sEvent;
      return (ActionResult) this.View();
    }

    public ActionResult cancleEvent()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      int uid = Convert.ToInt32(content.ID_USER);
      int eid = Convert.ToInt32(this.Request.Form["rejected-event"].ToString());
      string str = this.Request.Form["cancel-desc"].ToString();
      this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_cms_users>();
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eid)).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent == null)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      if (sEvent.status == "A")
      {
        sEvent.status = "X";
        sEvent.event_comment = str;
        sEvent.updated_date_time = new DateTime?(DateTime.Now);
        this.db.SaveChanges();
      }
      else if (sEvent.status == "P")
      {
        DbSet<tbl_scheduled_event_subscription_log> eventSubscriptionLog = this.db.tbl_scheduled_event_subscription_log;
        Expression<Func<tbl_scheduled_event_subscription_log, bool>> predicate = (Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event);
        foreach (tbl_scheduled_event_subscription_log entity in eventSubscriptionLog.Where<tbl_scheduled_event_subscription_log>(predicate).ToList<tbl_scheduled_event_subscription_log>())
        {
          this.db.tbl_scheduled_event_subscription_log.Remove(entity);
          this.db.SaveChanges();
        }
        this.db.tbl_scheduled_event.Remove(sEvent);
        this.db.SaveChanges();
      }
      return (ActionResult) this.RedirectToAction("Index", "Scheduling");
    }

    public string setOpenEventToParticipants(int id)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == id && t.status != "X")).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent == null)
        return "0";
      bool flag = true;
      if (sEvent.is_response == "2")
        flag = false;
      int num = sEvent.is_approval == "2" ? 1 : 0;
      foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery("select * from tbl_user where status='A' and id_organization=" + (object) int32_1 + " ").ToList<tbl_user>())
      {
        tbl_user item = tblUser;
        if (this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.id_user == (int?) item.ID_USER)).FirstOrDefault<tbl_scheduled_event_subscription_log>() == null)
        {
          tbl_scheduled_event_subscription_log entity = new tbl_scheduled_event_subscription_log();
          entity.id_organization = new int?(int32_1);
          entity.id_scheduled_event = new int?(sEvent.id_scheduled_event);
          entity.id_user = new int?(item.ID_USER);
          entity.id_cms_user = new int?(int32_2);
          entity.event_sent_timestamp = new DateTime?(DateTime.Now);
          entity.updated_date_time = new DateTime?(DateTime.Now);
          entity.event_user_comment = "";
          entity.event_user_response = new int?(0);
          if (flag)
          {
            entity.subscription_status = "O";
            entity.status = "S";
          }
          else
          {
            entity.event_user_response = new int?(1);
            entity.subscription_status = "A";
            entity.status = "S";
          }
          this.db.tbl_scheduled_event_subscription_log.Add(entity);
          this.db.SaveChanges();
        }
      }
      return "1";
    }

    public ActionResult EventAssignment(string id)
    {
      int oid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int eid = Convert.ToInt32(id);
      string str = "";
      int num1 = 0;
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eid && t.status != "X")).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent == null)
        return (ActionResult) this.RedirectToAction("Index", "Scheduling");
      int? eventType = sEvent.event_type;
      int num2 = 1;
      if ((eventType.GetValueOrDefault() == num2 ? (eventType.HasValue ? 1 : 0) : 0) != 0)
        return (ActionResult) this.RedirectToAction("LoadEvent", "Scheduling", (object) new
        {
          id = sEvent.id_scheduled_event
        });
      int? attachmentType1 = sEvent.attachment_type;
      int num3 = 1;
      if ((attachmentType1.GetValueOrDefault() == num3 ? (attachmentType1.HasValue ? 1 : 0) : 0) != 0)
      {
        tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => (int?) t.ID_CATEGORY == sEvent.id_program && t.STATUS == "A" && t.CATEGORY_TYPE == (int?) 0)).FirstOrDefault<tbl_category>();
        if (tblCategory != null)
        {
          num1 = tblCategory.ID_CATEGORY;
          str = tblCategory.CATEGORYNAME;
        }
      }
      else
      {
        int? attachmentType2 = sEvent.attachment_type;
        int num4 = 2;
        if ((attachmentType2.GetValueOrDefault() == num4 ? (attachmentType2.HasValue ? 1 : 0) : 0) != 0)
        {
          tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => (int?) t.id_assessment == sEvent.id_assessment && t.status == "A")).FirstOrDefault<tbl_assessment>();
          if (tblAssessment != null)
          {
            num1 = tblAssessment.id_assessment;
            str = tblAssessment.assessment_title;
          }
        }
      }
      this.ViewData["event"] = (object) sEvent;
      this.ViewData["attachment"] = (object) str;
      this.ViewData["attachmentid"] = (object) num1;
      this.ViewData["RoleList"] = (object) this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object) oid + " and status='A'").ToList<tbl_csst_role>();
      this.ViewData["notification"] = (object) this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>) (t => t.id_organization == oid && t.status == "A" && t.notification_type == 3)).ToList<tbl_notification>();
      return (ActionResult) this.View();
    }

    public string getEventscheduledToUser(string eid, string type, string rid, string pattern)
    {
      string str1 = "";
      if (rid == "")
        rid = "0";
      int ids = Convert.ToInt32(eid);
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == ids)).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent == null)
        return "";
      string str2 = "";
      if (type == "1")
        str2 = " and a.id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + rid + ") ";
      string str3 = "";
      string str4 = "";
      if (!string.IsNullOrEmpty(pattern))
      {
        str4 = " and ( upper(a.USERID) like '%" + pattern + "%'  OR upper(a.EMPLOYEEID) like '%" + pattern + "%'  )  ";
        str3 = " and a.id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + pattern + "%' or upper(LASTNAME) like '%" + pattern + "%') ";
      }
      string[] strArray = new string[5]
      {
        "select * from tbl_user a,tbl_profile b where a.id_user=b.id_user ",
        str4,
        str3,
        str2,
        " order by FIRSTNAME,LASTNAME, USERID "
      };
      foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery(string.Concat(strArray)).ToList<tbl_user>())
      {
        tbl_user item = tblUser;
        str1 += "<tr>";
        tbl_scheduled_event_subscription_log eventSubscriptionLog = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.id_user == (int?) item.ID_USER)).FirstOrDefault<tbl_scheduled_event_subscription_log>();
        str1 += "<td>";
        tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
        if (tblProfile != null)
          str1 = str1 + tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME;
        str1 = str1 + " - " + item.USERID + " </td>";
        str1 += "<td>";
        if (eventSubscriptionLog == null)
        {
          str1 = str1 + " <a id=\"link_" + (object) item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"addEventToUser('" + (object) item.ID_USER + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a>";
          str1 = str1 + "<i style=\"display:none;\" id=\"done_" + (object) item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
        }
        else
          str1 = str1 + "<i id=\"done_" + (object) item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
        str1 += "</td>";
        str1 += "</tr>";
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>" + "<th  width=\"20%\"></th>" + "</tr></thead>" + "<tbody>" + str1 + "</tbody></table>";
    }

    public string addEventToUser(string eid, string uid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      int eids = Convert.ToInt32(eid);
      int uids = Convert.ToInt32(uid);
      tbl_scheduled_event tblScheduledEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eids)).FirstOrDefault<tbl_scheduled_event>();
      if (tblScheduledEvent != null)
      {
        bool flag = true;
        if (tblScheduledEvent.is_response == "2")
          flag = false;
        int num = tblScheduledEvent.is_approval == "2" ? 1 : 0;
        if (this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) eids && t.id_user == (int?) uids)).FirstOrDefault<tbl_scheduled_event_subscription_log>() == null)
        {
          tbl_scheduled_event_subscription_log entity = new tbl_scheduled_event_subscription_log();
          entity.id_organization = new int?(int32_1);
          entity.id_scheduled_event = new int?(tblScheduledEvent.id_scheduled_event);
          entity.id_user = new int?(uids);
          entity.id_cms_user = new int?(int32_2);
          entity.event_sent_timestamp = new DateTime?(DateTime.Now);
          entity.updated_date_time = new DateTime?(DateTime.Now);
          entity.event_user_comment = "";
          if (flag)
          {
            entity.subscription_status = "O";
            entity.status = "S";
          }
          else
          {
            entity.event_user_response = new int?(1);
            entity.subscription_status = "A";
            entity.status = "S";
          }
          this.db.tbl_scheduled_event_subscription_log.Add(entity);
          this.db.SaveChanges();
          if (entity.id_scheduled_event_subscription_log > 0)
            return "1";
        }
      }
      return "0";
    }

    public string addUserToEvent(string eid, string uid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      int int32 = Convert.ToInt32(content.ID_USER);
      int eids = Convert.ToInt32(eid);
      int uids = Convert.ToInt32(uid);
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eids)).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent != null)
      {
        int num = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.subscription_status == "A")).Count<tbl_scheduled_event_subscription_log>();
        int? noOfParticipants = sEvent.no_of_participants;
        int valueOrDefault = noOfParticipants.GetValueOrDefault();
        if ((num < valueOrDefault ? (noOfParticipants.HasValue ? 1 : 0) : 0) == 0)
          return "L";
        tbl_scheduled_event_subscription_log eventSubscriptionLog = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) eids && t.id_user == (int?) uids)).FirstOrDefault<tbl_scheduled_event_subscription_log>();
        if (eventSubscriptionLog != null)
        {
          eventSubscriptionLog.subscription_status = "A";
          eventSubscriptionLog.approved_date = new DateTime?(DateTime.Now);
          eventSubscriptionLog.apporoved_reporting_manager = new int?(int32);
          this.db.SaveChanges();
          return "1";
        }
      }
      return "0";
    }

    public ActionResult approveUserRequest(string eid, string uid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(this.Request.Form["id-org"]);
      int int32 = Convert.ToInt32(this.Request.Form["rm-user"]);
      int eids = Convert.ToInt32(this.Request.Form["id-event"]);
      int uids = Convert.ToInt32(this.Request.Form["id-org"]);
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eids)).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent != null)
      {
        int num = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.subscription_status == "A")).Count<tbl_scheduled_event_subscription_log>();
        int? noOfParticipants = sEvent.no_of_participants;
        int valueOrDefault = noOfParticipants.GetValueOrDefault();
        if ((num < valueOrDefault ? (noOfParticipants.HasValue ? 1 : 0) : 0) != 0)
        {
          tbl_scheduled_event_subscription_log eventSubscriptionLog = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) eids && t.id_user == (int?) uids)).FirstOrDefault<tbl_scheduled_event_subscription_log>();
          if (eventSubscriptionLog != null)
          {
            eventSubscriptionLog.subscription_status = "A";
            eventSubscriptionLog.approved_date = new DateTime?(DateTime.Now);
            eventSubscriptionLog.apporoved_reporting_manager = new int?(int32);
            this.db.SaveChanges();
          }
        }
      }
      return (ActionResult) this.View();
    }

    public string rejectUserSubscription(string eid, string uid, string msg)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      int eids = Convert.ToInt32(eid);
      int uids = Convert.ToInt32(uid);
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eids)).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent != null)
      {
        int num = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.subscription_status == "A")).Count<tbl_scheduled_event_subscription_log>();
        int? noOfParticipants = sEvent.no_of_participants;
        int valueOrDefault = noOfParticipants.GetValueOrDefault();
        if ((num < valueOrDefault ? (noOfParticipants.HasValue ? 1 : 0) : 0) == 0)
          return "L";
        tbl_scheduled_event_subscription_log eventSubscriptionLog = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) eids && t.id_user == (int?) uids)).FirstOrDefault<tbl_scheduled_event_subscription_log>();
        if (eventSubscriptionLog != null)
        {
          eventSubscriptionLog.subscription_status = "C";
          eventSubscriptionLog.approved_date = new DateTime?(DateTime.Now);
          eventSubscriptionLog.apporoved_reporting_manager = new int?(uids);
          eventSubscriptionLog.event_user_comment = msg;
          this.db.SaveChanges();
          return "1";
        }
      }
      return "0";
    }
  }
}
