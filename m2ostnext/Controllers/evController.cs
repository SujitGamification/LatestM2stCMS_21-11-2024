// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.evController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class evController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index() => (ActionResult) this.View();

    public ActionResult eventApproval(string e, string o, string u, string rm, string a)
    {
      int eid = Convert.ToInt32(e);
      int rmid = Convert.ToInt32(rm);
      int int32 = Convert.ToInt32(o);
      int uid = Convert.ToInt32(u);
      bool flag = false;
      this.ViewData["qList"] = (object) (eid.ToString() + "-" + (object) uid + "-" + (object) int32 + "-" + (object) rmid);
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eid)).FirstOrDefault<tbl_scheduled_event>();
      if (sEvent == null)
        return (ActionResult) this.RedirectToAction("Message", (object) new
        {
          session = nameof (e)
        });
      if (sEvent.status == "X")
        return (ActionResult) this.RedirectToAction("Message", (object) new
        {
          session = "x"
        });
      if (sEvent.status == "A")
      {
        tbl_user user = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_user>();
        if (user != null)
        {
          tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == user.ID_USER)).FirstOrDefault<tbl_profile>();
          int num1 = 0;
          int? eventType = sEvent.event_type;
          int num2 = 1;
          if ((eventType.GetValueOrDefault() == num2 ? (eventType.HasValue ? 1 : 0) : 0) != 0)
          {
            num1 = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.subscription_status == "A")).Count<tbl_scheduled_event_subscription_log>();
            int num3 = num1;
            int? noOfParticipants = sEvent.no_of_participants;
            int valueOrDefault = noOfParticipants.GetValueOrDefault();
            if ((num3 >= valueOrDefault ? (noOfParticipants.HasValue ? 1 : 0) : 0) != 0)
              flag = true;
          }
          tbl_scheduled_event_subscription_log eventSubscriptionLog = this.db.tbl_scheduled_event_subscription_log.Where<tbl_scheduled_event_subscription_log>((Expression<Func<tbl_scheduled_event_subscription_log, bool>>) (t => t.id_scheduled_event == (int?) sEvent.id_scheduled_event && t.id_user == (int?) user.ID_USER)).FirstOrDefault<tbl_scheduled_event_subscription_log>();
          if (eventSubscriptionLog == null)
            return (ActionResult) this.RedirectToAction("Message", (object) new
            {
              session = "i"
            });
          tbl_user tblUser = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => t.ID_USER == rmid)).FirstOrDefault<tbl_user>();
          this.ViewData["org"] = (object) o;
          this.ViewData[nameof (rm)] = (object) tblUser;
          this.ViewData["fullFlag"] = (object) flag;
          this.ViewData["eLog"] = (object) eventSubscriptionLog;
          this.ViewData["event_count"] = (object) num1;
          this.ViewData["user"] = (object) user;
          this.ViewData["profile"] = (object) tblProfile;
          this.ViewData["event"] = (object) sEvent;
        }
      }
      return (ActionResult) this.View();
    }

    public string approveUserRequest()
    {
      string[] strArray = this.Request.Form["qList"].Split('-');
      Convert.ToInt32(strArray[2]);
      int int32 = Convert.ToInt32(strArray[3]);
      int eids = Convert.ToInt32(strArray[0]);
      int uids = Convert.ToInt32(strArray[1]);
      tbl_scheduled_event sEvent = this.db.tbl_scheduled_event.Where<tbl_scheduled_event>((Expression<Func<tbl_scheduled_event, bool>>) (t => t.id_scheduled_event == eids)).FirstOrDefault<tbl_scheduled_event>();
      string str;
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
            str = "a";
          }
          else
            str = "i";
        }
        else
          str = "l";
      }
      else
        str = "e";
      return str;
    }

    public string rejectUserSubscription()
    {
      string[] strArray = this.Request.Form["qList"].Split('-');
      Convert.ToInt32(strArray[2]);
      Convert.ToInt32(strArray[3]);
      int eids = Convert.ToInt32(strArray[0]);
      int uids = Convert.ToInt32(strArray[1]);
      string str1 = "";
      string str2 = this.Request.Form["reject-desc"].ToString();
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
            eventSubscriptionLog.subscription_status = "C";
            eventSubscriptionLog.approved_date = new DateTime?(DateTime.Now);
            eventSubscriptionLog.apporoved_reporting_manager = new int?(uids);
            eventSubscriptionLog.event_user_comment = str2;
            this.db.SaveChanges();
            str1 = "r";
          }
        }
        else
          str1 = "l";
      }
      else
        str1 = "e";
      return str1;
    }

    public ActionResult Message(string session)
    {
      string str;
      switch (session)
      {
        case "a":
          str = "User request for approval is successfull.Status will be updated to User.";
          break;
        case "e":
          str = "Invalid Event.Please contact administrator.";
          break;
        case "l":
          str = "Event participant limit is full.Please check our portal for more information";
          break;
        case "i":
          str = "Invalid user for the event .Please contact administrator";
          break;
        case "r":
          str = "User request has been rejected.Status will be updated to User";
          break;
        case "x":
          str = "Event is Expired.For more information contact Administrator";
          break;
        default:
          str = "Thank you";
          break;
      }
      this.ViewData["message"] = (object) str;
      return (ActionResult) this.View();
    }
  }
}
