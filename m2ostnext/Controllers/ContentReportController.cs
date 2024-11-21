// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.ContentReportController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class ContentReportController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index()
    {
      this.ViewData["select-organization"] = (object) this.db.tbl_organization.ToList<tbl_organization>();
      List<string> stringList = new List<string>();
      return (ActionResult) this.View();
    }

    public ActionResult ContentReportDetails(string org_id, string user_id)
    {
      try
      {
        string str1 = " Select e.USERID,c.USERNAME,d.ID_ORGANIZATION,b.ID_CONTENT,b.ID_USER,b.CONTENT_QUESTION,b.UPDATED_DATE_TIME,b.EXPIRY_DATE ,b.CONTENT_COUNTER ,d.ORGANIZATION_NAME " + " from tbl_report_content  a, tbl_content b ,tbl_cms_users c ,tbl_organization d , tbl_user e where  a.status='A' " + " AND a.ID_USER = e.ID_USER AND a.ID_ORGANIZATION = d.ID_ORGANIZATION  AND a.ID_CONTENT = b.ID_CONTENT AND  b.ID_USER = c.ID_USER ";
        string str2 = "";
        string str3 = "";
        if (org_id != "ALL" && !string.IsNullOrEmpty(org_id))
          str2 = str2 + " AND a.ID_ORGANIZATION = " + org_id + "  ";
        if (user_id != "ALL" && !string.IsNullOrEmpty(user_id))
          str2 = str2 + " AND a.ID_USER  = " + user_id + "  ";
        this.ViewData["ContentReport"] = (object) new ContentReportModel1().getContentReportfilterlist(str1 + str2 + str3);
      }
      catch (Exception ex)
      {
      }
      return (ActionResult) this.View();
    }

    public ActionResult ContentGraphReportIndex() => (ActionResult) this.View();

    public ActionResult ContentCounterIndex()
    {
      this.ViewData["select-organization"] = (object) this.db.tbl_organization.ToList<tbl_organization>();
      return (ActionResult) this.View();
    }

    public ActionResult ContentReport_Counter(string org_id, string user_id, string cnt)
    {
      try
      {
        string str1 = " Select e.USERID,d.ID_ORGANIZATION,b.ID_CONTENT,b.ID_USER,b.CONTENT_QUESTION,b.UPDATED_DATE_TIME,b.EXPIRY_DATE ,b.CONTENT_COUNTER ,d.ORGANIZATION_NAME " + " from tbl_report_content  a, tbl_content b ,tbl_organization d , tbl_user e where  a.status='A' " + " AND a.ID_USER = e.ID_USER AND a.ID_ORGANIZATION = d.ID_ORGANIZATION  AND a.ID_CONTENT = b.ID_CONTENT  ";
        string str2 = "";
        string str3 = "";
        if (org_id != "ALL" && !string.IsNullOrEmpty(org_id))
          str2 = str2 + " AND a.ID_ORGANIZATION = " + org_id + "  ";
        if (user_id != "ALL" && !string.IsNullOrEmpty(user_id))
          str2 = str2 + " AND a.ID_USER  = " + user_id + "  ";
        if (cnt == 0.ToString())
          str2 = str2 + " AND b.CONTENT_COUNTER  = " + cnt + "  ";
        else if (cnt != "ALL" && !string.IsNullOrEmpty(cnt))
          str2 = str2 + " AND b.CONTENT_COUNTER  >= " + cnt + "  ";
        this.ViewData["ContentReport"] = (object) new ContentReportModel1().getContentReportfilterlist(str1 + str2 + str3);
      }
      catch (Exception ex)
      {
      }
      return (ActionResult) this.View();
    }

    public ActionResult ContentOptionReport()
    {
      this.ViewData["select-organization"] = (object) this.db.tbl_organization.ToList<tbl_organization>();
      return (ActionResult) this.View();
    }

    public ActionResult ContentReportOptionDetails(string org_id, string user_id, string opt)
    {
      try
      {
        string str1 = " SELECT COUNT(a.flag) AS `count`, b.ID_USER,b.USERID,c.CONTENT_QUESTION,a.flag ,c.UPDATED_DATE_TIME,c.EXPIRY_DATE " + " FROM tbl_content_counters a , tbl_user b ,tbl_content c WHERE  c.STATUS='A'  AND a.id_user =  b.ID_USER AND a.id_content = c.id_content   ";
        string str2 = "";
        string str3 = "";
        if (org_id != "ALL")
          string.IsNullOrEmpty(org_id);
        if (user_id != "ALL" && !string.IsNullOrEmpty(user_id))
          str2 = str2 + " AND b.ID_USER  = " + user_id + "  ";
        if (opt != "ALL" && !string.IsNullOrEmpty(opt))
          str2 = str2 + " AND a.flag  = " + opt + "  ";
        this.ViewData["ContentReport"] = (object) new ContentReportModel1().getContentOptionfilterlist(str1 + str2 + str3 + "   GROUP BY c.CONTENT_QUESTION ");
      }
      catch (Exception ex)
      {
      }
      return (ActionResult) this.View();
    }

    public ActionResult ContentTopUSersIndex()
    {
      try
      {
        string str = " SELECT * from tbl_profile group by LOCATION";
        new ContentReportModel1().getContentLoc(str);
        List<tbl_profile> list = this.db.tbl_profile.SqlQuery(str).ToList<tbl_profile>();
        List<locationbyuser> locationbyuserList = new List<locationbyuser>();
        foreach (tbl_profile tblProfile1 in list)
        {
          locationbyuser locationbyuser = new locationbyuser();
          List<usercount> usercountList = new List<usercount>();
          foreach (usersdetails usersdetails in new ContentReportModel1().getContentTopUser("  SELECT COUNT(a.ID_CONTENT) AS `Lcount`,a.ID_USER" + " FROM tbl_report_content  a, tbl_profile b " + " WHERE b.LOCATION LIKE  '" + tblProfile1.LOCATION + "' AND a.ID_USER = b.ID_USER  GROUP BY  ID_USER ORDER BY Lcount DESC LIMIT 20  "))
          {
            usersdetails uitems = usersdetails;
            usercount usercount = new usercount();
            tbl_user tblUser = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => t.ID_USER == uitems.ID_USER)).FirstOrDefault<tbl_user>();
            tbl_profile tblProfile2 = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == uitems.ID_USER)).FirstOrDefault<tbl_profile>();
            usercount.user = tblUser;
            usercount.profile = tblProfile2;
            usercount.count = uitems.count;
            usercountList.Add(usercount);
          }
          locationbyuser.location = tblProfile1.LOCATION;
          locationbyuser.counter = usercountList;
          locationbyuserList.Add(locationbyuser);
        }
        this.ViewData["usersdetails"] = (object) locationbyuserList;
      }
      catch (Exception ex)
      {
        return (ActionResult) this.RedirectToAction("Errorbody", "ErrorView");
      }
      return (ActionResult) this.View();
    }

    public ActionResult ContentTopUSersGMIndex()
    {
      try
      {
        string str = " SELECT * from tbl_profile group by LOCATION";
        new ContentReportModel1().getContentLoc(str);
        List<tbl_profile> list = this.db.tbl_profile.SqlQuery(str).ToList<tbl_profile>();
        List<locationbyuser> locationbyuserList = new List<locationbyuser>();
        foreach (tbl_profile tblProfile1 in list)
        {
          locationbyuser locationbyuser = new locationbyuser();
          List<usercount> usercountList = new List<usercount>();
          foreach (usersdetails usersdetails in new ContentReportModel1().getContentTopUser("  SELECT COUNT(a.ID_CONTENT) AS `Lcount`,a.ID_USER" + " FROM tbl_report_content  a, tbl_profile b " + " WHERE b.LOCATION LIKE  '" + tblProfile1.LOCATION + "' AND a.ID_USER = b.ID_USER  GROUP BY  ID_USER ORDER BY Lcount DESC LIMIT 20  "))
          {
            usersdetails uitems = usersdetails;
            usercount usercount = new usercount();
            tbl_user tblUser = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => t.ID_USER == uitems.ID_USER)).FirstOrDefault<tbl_user>();
            tbl_profile tblProfile2 = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => t.ID_USER == uitems.ID_USER)).FirstOrDefault<tbl_profile>();
            usercount.user = tblUser;
            usercount.profile = tblProfile2;
            usercount.count = uitems.count;
            usercountList.Add(usercount);
          }
          locationbyuser.location = tblProfile1.LOCATION;
          locationbyuser.counter = usercountList;
          locationbyuserList.Add(locationbyuser);
        }
        this.ViewData["usersdetails"] = (object) locationbyuserList;
      }
      catch (Exception ex)
      {
        return (ActionResult) this.RedirectToAction("Errorbody", "ErrorView");
      }
      return (ActionResult) this.View();
    }

    public string get_user(string org_id)
    {
      string str = "";
      List<tbl_user> userOrganization = new ContentReportModel1().get_user_organization(org_id);
      string user;
      if (userOrganization.Count > 0)
      {
        user = str + "<option value =ALL>Select User </option>";
        foreach (tbl_user tblUser in userOrganization)
          user = user + "<option value=" + (object) tblUser.ID_USER + ">" + tblUser.USERID + "</option>";
      }
      else
        user = str + "<option value='0' disabled selected >No Users</option>";
      return user;
    }
  }
}
