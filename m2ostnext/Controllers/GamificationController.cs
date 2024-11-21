// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.GamificationController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  [UserFilter]
  public class GamificationController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult dashboard()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      this.ViewData["master"] = (object) new GamificationModel().getGameList("" + " SELECT a.id_game, a.game_description,a.id_organisation, a.id_game_creator, a.game_title, b.USERNAME game_creator, " + " CASE WHEN a.STATUS = 'A' THEN 'Active' WHEN a.STATUS = 'D' THEN 'Draft' END game_stutus, " + " CASE WHEN a.game_type = '1' THEN 'Standalone' WHEN a.game_type = '2' THEN 'Journey' END game_type, " + " CASE WHEN a.game_mode = '1' THEN 'Open' WHEN a.game_mode = '2' THEN 'Close' END game_mode, " + " CASE WHEN a.player_type = '1' THEN 'Solo Game' WHEN a.player_type = '2' THEN 'Group Game' END player_type, " + " a.gameid,  DATE_FORMAT(a.game_start_date, '%d-%m-%Y') start_date,    DATE_FORMAT(a.game_expiry_date, '%d-%m-%Y') end_date " + (" FROM tbl_game_creation a LEFT JOIN tbl_cms_users b ON a.id_game_creator = b.ID_USER WHERE a.id_organisation =  " + (object) int32));
      return (ActionResult) this.View();
    }

    public ActionResult group_dashboard()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      this.ViewData["group"] = (object) new GamificationModel().getGroupData("SELECT  a.id_game_group, a.group_name,sum(case when id_game_group_participatant>0 then 1 else 0 end) group_count FROM tbl_game_group a  LEFT JOIN  tbl_game_group_participatant b ON a.id_game_group = b.id_game_group where a.id_organization=" + (object) int32 + " GROUP BY a.id_game_group");
      return (ActionResult) this.View();
    }

    public ActionResult game_dashboard(string gids)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_game_creation tblGameCreation = this.db.tbl_game_creation.SqlQuery("select * from tbl_game_creation where gameid='" + gids + "'").FirstOrDefault<tbl_game_creation>();
      if (tblGameCreation == null)
        return (ActionResult) this.View();
      this.ViewData["elements"] = (object) new GamificationModel().getGameElementsList("" + " SELECT a.id_game,a.id_organization,a.sequence_number,a.weightage, " + " CASE WHEN a.id_category > 0 THEN b.CATEGORYNAME WHEN a.id_assessment > 0 THEN c.assessment_title END element_name, " + " CASE WHEN a.element_type = 1 THEN 'Program' WHEN a.element_type = 2 THEN 'Assessment' END element_type, " + " CASE WHEN a.is_mandatory = 1 THEN 'Mandatory' WHEN a.is_mandatory = 0 THEN 'Not Mandatory' END is_mandatory " + " FROM tbl_game_process_path a LEFT JOIN tbl_category b ON a.id_category = b.ID_CATEGORY AND a.id_category > 0 " + " LEFT JOIN tbl_assessment c ON a.id_assessment = c.id_assessment AND a.id_assessment > 0 and a.id_organization=" + (object) int32 + " and a.id_game=" + (object) tblGameCreation.id_game + " order by a.sequence_number");
      this.ViewData["game"] = (object) tblGameCreation;
      return (ActionResult) this.View();
    }

    public ActionResult m2ost_add_game() => (ActionResult) this.View();

    public ActionResult add_m2ost_game()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      tbl_game_creation entity = new tbl_game_creation()
      {
        game_title = this.Request.Form["game-title"].ToString(),
        game_description = this.Request.Form["game-desc"].ToString(),
        game_expiry_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["game-expiry-date"].ToString())),
        game_start_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["game-start-date"].ToString()))
      };
      entity.game_comment = entity.game_description;
      entity.game_creation_datetime = new DateTime?(DateTime.Now);
      entity.game_creator_name = content.Username;
      entity.id_game_creator = new int?(int32_2);
      entity.id_organisation = new int?(int32_1);
      entity.game_mode = this.Request.Form["game-mode"].ToString();
      entity.game_type = this.Request.Form["game-type"].ToString();
      entity.player_type = this.Request.Form["player-type"].ToString();
      entity.game_phase = "1";
      entity.status = "D";
      entity.updated_date_time = new DateTime?(DateTime.Now);
      this.db.tbl_game_creation.Add(entity);
      entity.gameid = "M2G" + DateTime.Now.ToString("yyyyMMddHHmm");
      this.db.SaveChanges();
      return entity.id_game > 0 ? (ActionResult) this.RedirectToAction("m2ost_game_elements", "Gamification", (object) new
      {
        gids = entity.gameid
      }) : (ActionResult) this.View();
    }

    public ActionResult m2ost_game_elements(string gids)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_game_creation tblGameCreation = this.db.tbl_game_creation.SqlQuery("select * from tbl_game_creation where gameid='" + gids + "'").FirstOrDefault<tbl_game_creation>();
      int num1 = 0;
      if (tblGameCreation == null)
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification");
      List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where id_organization=" + (object) int32 + " and category_type in (0,1) and status='A'   and ID_CATEGORY in (select distinct id_category from tbl_kpi_program_scoring where ID_ORGANIZATION=" + (object) int32 + ") and ID_CATEGORY not in (select distinct ID_CATEGORY from tbl_game_process_path where ID_CATEGORY >0 and id_game=" + (object) tblGameCreation.id_game + ") order by categoryname").ToList<tbl_category>();
      List<tbl_assessment> list2 = this.db.tbl_assessment.SqlQuery("select * from tbl_assessment where id_organization=" + (object) int32 + "  and status='A' and id_assessment not in (select distinct id_assessment from tbl_game_process_path where id_assessment >0 and id_game=" + (object) tblGameCreation.id_game + ") order by assessment_title").ToList<tbl_assessment>();
      string STR = "" + " SELECT a.id_game,a.id_organization,a.sequence_number,a.weightage, " + " CASE WHEN a.id_category > 0 THEN b.CATEGORYNAME WHEN a.id_assessment > 0 THEN c.assessment_title END element_name, " + " CASE WHEN a.element_type = 1 THEN 'Program' WHEN a.element_type = 2 THEN 'Assessment' END element_type, " + " CASE WHEN a.is_mandatory = 1 THEN 'Mandatory' WHEN a.is_mandatory = 0 THEN 'Not Mandatory' END is_mandatory " + " FROM tbl_game_process_path a LEFT JOIN tbl_category b ON a.id_category = b.ID_CATEGORY AND a.id_category > 0 " + " LEFT JOIN tbl_assessment c ON a.id_assessment = c.id_assessment AND a.id_assessment > 0 where a.id_organization=" + (object) int32 + " and a.id_game=" + (object) tblGameCreation.id_game + " order by a.sequence_number";
      int num2 = 0;
      List<GameElement> gameElementsList = new GamificationModel().getGameElementsList(STR);
      if (gameElementsList.Count > 0)
      {
        num1 = Convert.ToInt32(gameElementsList.Max<GameElement>((Func<GameElement, int>) (t => t.sequence_number)));
        num2 = Convert.ToInt32(gameElementsList.Sum<GameElement>((Func<GameElement, double>) (t => t.weightage)));
      }
      double num3 = gameElementsList.Sum<GameElement>((Func<GameElement, double>) (t => t.weightage));
      int num4 = num1 + 1;
      int num5 = 100;
      this.ViewData["sum"] = (object) num3;
      int num6 = num5 - num2;
      this.ViewData["elements"] = (object) gameElementsList;
      this.ViewData["program"] = (object) list1;
      this.ViewData["assessement"] = (object) list2;
      this.ViewData["game"] = (object) tblGameCreation;
      this.ViewData["steps"] = (object) num4;
      this.ViewData["max_val"] = (object) num6;
      return (ActionResult) this.View();
    }

    public ActionResult m2ost_game_publish(string gids)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int OID = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_game_creation tblGameCreation = this.db.tbl_game_creation.SqlQuery("select * from tbl_game_creation where id_game='" + gids + "'").FirstOrDefault<tbl_game_creation>();
      if (tblGameCreation == null)
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification");
      List<GameElement> gameElementsList = new GamificationModel().getGameElementsList("" + " SELECT a.id_game,a.id_organization,a.sequence_number,a.weightage, " + " CASE WHEN a.id_category > 0 THEN b.CATEGORYNAME WHEN a.id_assessment > 0 THEN c.assessment_title END element_name, " + " CASE WHEN a.element_type = 1 THEN 'Program' WHEN a.element_type = 2 THEN 'Assessment' END element_type, " + " CASE WHEN a.is_mandatory = 1 THEN 'Mandatory' WHEN a.is_mandatory = 0 THEN 'Not Mandatory' END is_mandatory " + " FROM tbl_game_process_path a LEFT JOIN tbl_category b ON a.id_category = b.ID_CATEGORY AND a.id_category > 0 " + " LEFT JOIN tbl_assessment c ON a.id_assessment = c.id_assessment AND a.id_assessment > 0 and a.id_organization=" + (object) OID + " where a.id_game=" + (object) tblGameCreation.id_game + " order by a.sequence_number");
      double num = gameElementsList.Sum<GameElement>((Func<GameElement, double>) (t => t.weightage));
      List<tbl_game_group> tblGameGroupList = new List<tbl_game_group>();
      if (tblGameCreation.player_type == "1")
        tblGameGroupList = this.db.tbl_game_group.Where<tbl_game_group>((Expression<Func<tbl_game_group, bool>>) (t => t.game_group_type == (int?) 1 && t.id_organization == (int?) OID)).ToList<tbl_game_group>();
      else if (tblGameCreation.player_type == "2")
        tblGameGroupList = this.db.tbl_game_group.Where<tbl_game_group>((Expression<Func<tbl_game_group, bool>>) (t => t.game_group_type == (int?) 2 && t.id_organization == (int?) OID)).ToList<tbl_game_group>();
      this.ViewData["sum"] = (object) num;
      this.ViewData["elements"] = (object) gameElementsList;
      this.ViewData["game"] = (object) tblGameCreation;
      this.ViewData["group"] = (object) tblGameGroupList;
      return (ActionResult) this.View();
    }

    public ActionResult m2ost_game_publish_action()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      int gameid = Convert.ToInt32(this.Request.Form["game-id"].ToString());
      tbl_game_creation tblGameCreation = this.db.tbl_game_creation.Where<tbl_game_creation>((Expression<Func<tbl_game_creation, bool>>) (t => t.id_game == gameid && t.status == "D")).FirstOrDefault<tbl_game_creation>();
      if (tblGameCreation == null)
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification");
      tblGameCreation.status = "A";
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("m2ost_game_publish", "Gamification", (object) new
      {
        gids = tblGameCreation.id_game
      });
    }

    public ActionResult add_m2ost_game_element()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_game_creation tblGameCreation = this.db.tbl_game_creation.SqlQuery("select * from tbl_game_creation where id_game='" + (object) Convert.ToInt32(this.Request.Form["game-id"]) + "'").FirstOrDefault<tbl_game_creation>();
      int int32_2 = Convert.ToInt32(this.Request.Form["game-step"]);
      if (tblGameCreation.status == "A")
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification", (object) new
        {
          gids = tblGameCreation.gameid
        });
      int int32_3 = Convert.ToInt32(this.Request.Form["element-type"]);
      tbl_game_process_path entity = new tbl_game_process_path();
      int int32_4 = Convert.ToInt32(this.Request.Form["element-mandatory"]);
      double num = Convert.ToDouble(this.Request.Form["element-weightage"]);
      if (int32_3 == 1)
      {
        int int32_5 = Convert.ToInt32(this.Request.Form["element-program-data"]);
        entity.id_category = new int?(int32_5);
        entity.element_type = new int?(1);
        entity.id_assessment = new int?(0);
        entity.process_type = new int?(1);
      }
      else
      {
        int int32_6 = Convert.ToInt32(this.Request.Form["element-assessment-data"]);
        entity.id_category = new int?(0);
        entity.element_type = new int?(2);
        entity.id_assessment = new int?(int32_6);
        entity.process_type = new int?(2);
      }
      entity.id_game = new int?(tblGameCreation.id_game);
      entity.id_organization = new int?(int32_1);
      entity.sequence_number = new int?(int32_2);
      entity.is_mandatory = new int?(int32_4);
      entity.weightage = new double?(num);
      entity.status = "A";
      entity.updated_date_time = new DateTime?(DateTime.Now);
      this.db.tbl_game_process_path.Add(entity);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("m2ost_game_elements", "Gamification", (object) new
      {
        gids = tblGameCreation.gameid
      });
    }

    public ActionResult game_group_dashboard() => (ActionResult) this.View();

    public ActionResult add_group() => (ActionResult) this.View();

    public ActionResult add_game_action()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      tbl_game_group entity = new tbl_game_group();
      entity.group_name = this.Request.Form["group-name"].ToString();
      entity.id_creator = new int?(int32_2);
      entity.id_organization = new int?(int32_1);
      entity.status = "A";
      entity.updated_date_time = new DateTime?(DateTime.Now);
      entity.group_image_path = "";
      entity.game_group_type = new int?(2);
      this.db.tbl_game_group.Add(entity);
      this.db.SaveChanges();
      if (entity.id_game_group <= 0)
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification");
      if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
      {
        HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
        string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
        string str = System.Web.HttpContext.Current.Request["id"];
        if (file != null && file.ContentLength > 0)
        {
          if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/USERGROUP/" + content.id_ORGANIZATION + "/")))
            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/USERGROUP/" + content.id_ORGANIZATION + "/"));
          string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/USERGROUP/" + content.id_ORGANIZATION + "/"), entity.id_game_group.ToString() + extension);
          file.SaveAs(filename);
          entity.group_image_path = entity.id_game_group.ToString() + extension;
          this.db.SaveChanges();
        }
      }
      return (ActionResult) this.RedirectToAction("add_game_group", "Gamification", (object) new
      {
        ids = entity.id_game_group
      });
    }

    public ActionResult add_game_group(string ids)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_game_group tblGameGroup = this.db.tbl_game_group.Find(new object[1]
      {
        (object) Convert.ToInt32(ids)
      });
      if (tblGameGroup == null)
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification");
      this.ViewData["RoleList"] = (object) this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object) int32 + " and status='A'").ToList<tbl_csst_role>();
      List<string> userLocation = new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object) int32 + " order by LOCATION ");
      List<string> userFunction = new contentDashboardModel().getUserFunction(" SELECT distinct upper(user_function) user_function FROM tbl_user where ID_ORGANIZATION=" + (object) int32 + " and user_function is not null order by user_function");
      this.ViewData["location"] = (object) userLocation;
      this.ViewData["function"] = (object) userFunction;
      this.ViewData["group"] = (object) tblGameGroup;
      return (ActionResult) this.View();
    }

    public string getUserListForGroup(
      string gid,
      string role,
      string fun,
      string loc,
      string pattern,
      string std,
      string exd)
    {
      int int32_1 = Convert.ToInt32(gid);
      int int32_2 = Convert.ToInt32(role);
      int int32_3 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      pattern = pattern.Replace("'", "''");
      string str1 = "";
      if (int32_2 > 0)
        str1 = " and a.id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + (object) int32_2 + ") ";
      string str2 = "";
      string str3 = "";
      if (loc.Trim().Count<char>() > 0)
        str2 = " and  (upper(b.CITY) like '%" + loc + "%' OR upper(b.LOCATION) like '%" + loc + "%') ";
      if (fun.Trim().Count<char>() > 0)
        str3 = " and  (upper(a.user_function) like '%" + fun + "%' ) ";
      string str4 = "";
      if (!string.IsNullOrEmpty(pattern))
        str4 = " and ( upper(a.USERID) like '%" + pattern + "%'  OR upper(a.EMPLOYEEID) like '%" + pattern + "%' or upper(b.FIRSTNAME) like '%" + pattern + "%' or upper(b.LASTNAME) like '%" + pattern + "%' )  ";
      string STR = "" + " SELECT     a.id_user,a.userid,concat(b.FIRSTNAME,' ',b.LASTNAME) user_name,IF(c.id_game_group_participatant is null, '0', '1') group_status, IF(c.id_game_group_participatant is null, 'N', c.status) user_status,   IF(c.id_game_group is null, 0,c.id_game_group) id_game_group,a.ID_ORGANIZATION" + " FROM    tbl_user a        LEFT JOIN    tbl_profile b ON a.ID_USER = b.id_user     LEFT JOIN    tbl_game_group_participatant c on a.id_user=c.ID_USER and c.id_game_group =" + (object) int32_1 + " " + " where  a.ID_ORGANIZATION=" + (object) int32_3 + " " + str4 + " " + str1 + " " + str2 + " " + str3;
      List<int> intList = new List<int>();
      List<GroupUsers> groupUserList = new GamificationModel().getGroupUserList(STR);
      string str5 = "";
      foreach (GroupUsers groupUsers in groupUserList)
      {
        str5 = str5 + "<tr><td>" + groupUsers.user_name + " ( " + groupUsers.userid + " ) </td>";
        str5 += "<td>";
        if (groupUsers.group_status == "0")
        {
          str5 = str5 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object) groupUsers.id_user + "\">";
          str5 = str5 + "<i style=\"display:none;\" id=\"done_" + (object) groupUsers.id_user + "\" class=\"glyphicon glyphicon-ok\"></i>";
        }
        else if (groupUsers.user_status == "A")
        {
          str5 = str5 + "<i id=\"done_" + (object) groupUsers.id_user + "\" class=\"glyphicon glyphicon-ok\"></i>";
          str5 = str5 + " | <a id=\"link_" + (object) groupUsers.id_user + "\" href=\"javascript:void(0)\" onclick=\"removeUserFromGroup('" + (object) groupUsers.id_user + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a> | ";
        }
        else
        {
          str5 = str5 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object) groupUsers.id_user + "\">";
          str5 = str5 + "<i style=\"display:none;\" id=\"done_" + (object) groupUsers.id_user + "\" class=\"glyphicon glyphicon-ok\"></i>";
        }
        str5 += "</td>";
        str5 += "</tr>";
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>" + "<th  width=\"20%\"><input type = \"checkbox\" id = \"checkAll\" onclick = \"check_all()\"> Select all &nbsp;&nbsp;</th>" + "</thead>" + "<tbody>" + str5 + "</tbody></table>";
    }

    public string getSoloUserListForGame(
      string gid,
      string role,
      string fun,
      string loc,
      string pattern,
      string std,
      string exd)
    {
      int int32_1 = Convert.ToInt32(gid);
      int int32_2 = Convert.ToInt32(role);
      int int32_3 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      pattern = pattern.Replace("'", "''");
      string str1 = "";
      if (int32_2 > 0)
        str1 = " and a.id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + (object) int32_2 + ") ";
      string str2 = "";
      string str3 = "";
      if (loc.Trim().Count<char>() > 0)
        str2 = " and  (upper(b.CITY) like '%" + loc + "%' OR upper(b.LOCATION) like '%" + loc + "%') ";
      if (fun.Trim().Count<char>() > 0)
        str3 = " and  (upper(a.user_function) like '%" + fun + "%' ) ";
      string str4 = "";
      if (!string.IsNullOrEmpty(pattern))
        str4 = " and ( upper(a.USERID) like '%" + pattern + "%'  OR upper(a.EMPLOYEEID) like '%" + pattern + "%' or upper(b.FIRSTNAME) like '%" + pattern + "%' or upper(b.LASTNAME) like '%" + pattern + "%' )  ";
      string STR = "" + " SELECT     a.id_user,a.userid,concat(b.FIRSTNAME,' ',b.LASTNAME) user_name,IF(c.id_game_group_association IS NULL,'0','1') group_status,IF(c.id_game_group_association IS NULL,  'N',c.status) user_status,0 as id_game_group,a.ID_ORGANIZATION" + " FROM    tbl_user a        LEFT JOIN    tbl_profile b ON a.ID_USER = b.id_user     LEFT JOIN    tbl_game_group_association c ON a.id_user = c.ID_USER AND c.association_type = 1 and c.id_organization=" + (object) int32_3 + " and c.id_game=" + (object) int32_1 + " and c.status='A' " + " where  a.ID_ORGANIZATION=" + (object) int32_3 + " " + str4 + " " + str1 + " " + str2 + " " + str3;
      List<int> intList = new List<int>();
      List<GroupUsers> groupUserList = new GamificationModel().getGroupUserList(STR);
      string str5 = "";
      foreach (GroupUsers groupUsers in groupUserList)
      {
        str5 = str5 + "<tr><td>" + groupUsers.user_name + " ( " + groupUsers.userid + " ) </td>";
        str5 += "<td>";
        if (groupUsers.group_status == "0")
          str5 = str5 + "<div class=\"checkbox\"><label> <input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object) groupUsers.id_user + "\"><span class=\"cr\"><i class=\"cr-icon glyphicon glyphicon-ok\"></i></span></label></div>";
        else if (groupUsers.user_status == "A")
          str5 = str5 + "<i id=\"solo-done-" + (object) groupUsers.id_user + "\" class=\"glyphicon glyphicon-ok\"></i>";
        else
          str5 = str5 + "<div class=\"checkbox\"><label> <input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object) groupUsers.id_user + "\"><span class=\"cr\"><i class=\"cr-icon glyphicon glyphicon-ok\"></i></span></label></div>";
        str5 += "</td>";
        str5 += "</tr>";
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>" + "<th  width=\"20%\"> " + "<div class=\"checkbox\"><label> <input type = \"checkbox\" id = \"checkAll\" onclick = \"check_all()\"><span class=\"cr\"><i class=\"cr-icon glyphicon glyphicon-ok\"></i></span>All</label></div>" + "</th></thead>" + "<tbody>" + str5 + "</tbody></table>";
    }

    public string setUserToGroup()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      int[] numArray = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
      int int32_2 = Convert.ToInt32(this.Request.Form["id_game_group"]);
      foreach (int num in numArray)
      {
        this.db.tbl_game_group_participatant.Add(new tbl_game_group_participatant()
        {
          id_game_group = new int?(int32_2),
          id_organization = new int?(int32_1),
          id_user = new int?(num),
          addition_date = new DateTime?(DateTime.Now),
          status = "A",
          updated_date_time = new DateTime?(DateTime.Now),
          participatant_level = new int?(1)
        });
        this.db.SaveChanges();
      }
      return "1";
    }

    public string removeUserFromGroup(string gid, string uid)
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int uids = Convert.ToInt32(uid);
      int gids = Convert.ToInt32(gid);
      tbl_game_group_participatant groupParticipatant = this.db.tbl_game_group_participatant.Where<tbl_game_group_participatant>((Expression<Func<tbl_game_group_participatant, bool>>) (t => t.id_game_group == (int?) gids && t.id_user == (int?) uids && t.status == "A")).FirstOrDefault<tbl_game_group_participatant>();
      if (groupParticipatant == null)
        return "0";
      groupParticipatant.status = "D";
      groupParticipatant.removal_date = new DateTime?(DateTime.Now);
      this.db.SaveChanges();
      return "1";
    }

    public string removeSoloUserFromGame(string gid, string uid)
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int uids = Convert.ToInt32(uid);
      int gids = Convert.ToInt32(gid);
      tbl_game_group_association groupAssociation = this.db.tbl_game_group_association.Where<tbl_game_group_association>((Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) gids && t.id_user == (int?) uids && t.association_type == (int?) 1 && t.status == "A")).FirstOrDefault<tbl_game_group_association>();
      if (groupAssociation == null)
        return "0";
      groupAssociation.status = "D";
      groupAssociation.removed_date = new DateTime?(DateTime.Now);
      this.db.SaveChanges();
      return "1";
    }

    public ActionResult add_game_group_action() => (ActionResult) this.View();

    public ActionResult game_associations(string gid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_game_creation tblGameCreation = this.db.tbl_game_creation.Find(new object[1]
      {
        (object) Convert.ToInt32(gid)
      });
      List<tbl_game_group> tblGameGroupList1 = new List<tbl_game_group>();
      List<tbl_game_group> tblGameGroupList2 = new List<tbl_game_group>();
      List<tbl_csst_role> tblCsstRoleList = new List<tbl_csst_role>();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      if (tblGameCreation == null)
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification");
      if (tblGameCreation.player_type == "1")
      {
        tblCsstRoleList = this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object) int32 + " and status='A'").ToList<tbl_csst_role>();
        stringList2 = new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object) int32 + " order by LOCATION ");
        stringList1 = new contentDashboardModel().getUserFunction(" SELECT distinct upper(user_function) user_function FROM tbl_user where ID_ORGANIZATION=" + (object) int32 + " and user_function is not null order by user_function");
      }
      else if (tblGameCreation.player_type == "2")
      {
        tblGameGroupList1 = this.db.tbl_game_group.SqlQuery("SELECT * FROM tbl_game_group where id_organization=" + (object) int32 + " and status='A' and id_game_group not in (select distinct id_game_group from tbl_game_group_association where id_game=" + (object) tblGameCreation.id_game + ") ").ToList<tbl_game_group>();
        tblGameGroupList2 = this.db.tbl_game_group.SqlQuery("SELECT * FROM tbl_game_group where id_organization=" + (object) int32 + " and status='A' and id_game_group  in (select distinct id_game_group from tbl_game_group_association where id_game=" + (object) tblGameCreation.id_game + ")").ToList<tbl_game_group>();
      }
      this.ViewData["RoleList"] = (object) tblCsstRoleList;
      this.ViewData["location"] = (object) stringList2;
      this.ViewData["function"] = (object) stringList1;
      this.ViewData["game"] = (object) tblGameCreation;
      this.ViewData["group"] = (object) tblGameGroupList1;
      this.ViewData["pregroup"] = (object) tblGameGroupList2;
      return (ActionResult) this.View();
    }

    public string setGroupToGame(string gid, string grid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      int gameid = Convert.ToInt32(gid);
      int groupid = Convert.ToInt32(grid);
      if (this.db.tbl_game_group_association.Where<tbl_game_group_association>((Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) gameid && t.id_game_group == (int?) groupid && t.association_type == (int?) 2 && t.status == "A")).FirstOrDefault<tbl_game_group_association>() != null)
        return "2";
      this.db.tbl_game_group_association.Add(new tbl_game_group_association()
      {
        id_game = new int?(gameid),
        id_game_group = new int?(groupid),
        id_organization = new int?(int32_1),
        status = "A",
        updated_date_time = new DateTime?(DateTime.Now),
        assigned_by = new int?(int32_2),
        association_type = new int?(2),
        id_user = new int?(0),
        creation_date = new DateTime?(DateTime.Now)
      });
      this.db.SaveChanges();
      return "1";
    }

    public string setSoloUserToGame(string gid, string uid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      int gameid = Convert.ToInt32(gid);
      int userid = Convert.ToInt32(uid);
      if (this.db.tbl_game_group_association.Where<tbl_game_group_association>((Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) gameid && t.id_user == (int?) userid && t.association_type == (int?) 1 && t.status == "A")).FirstOrDefault<tbl_game_group_association>() != null)
        return "2";
      this.db.tbl_game_group_association.Add(new tbl_game_group_association()
      {
        id_game = new int?(gameid),
        id_game_group = new int?(0),
        id_organization = new int?(int32_1),
        status = "A",
        updated_date_time = new DateTime?(DateTime.Now),
        assigned_by = new int?(int32_2),
        association_type = new int?(1),
        id_user = new int?(userid),
        creation_date = new DateTime?(DateTime.Now)
      });
      this.db.SaveChanges();
      return "1";
    }

    public string setMultiSoloUserToGame()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      DateTime? nullable1 = new DateTime?();
      DateTime? nullable2 = new DateTime?();
      nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["game-start-date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["game-start-date"], "dd-MM-yyyy HH:mm", (IFormatProvider) CultureInfo.InvariantCulture)) : new DateTime?();
      nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["game-expiry-date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["game-expiry-date"], "dd-MM-yyyy HH:mm", (IFormatProvider) CultureInfo.InvariantCulture)) : new DateTime?();
      int[] numArray = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
      int gameid = Convert.ToInt32(this.Request.Form["id_game_creation"]);
      foreach (int num in numArray)
      {
        int iUser = num;
        if (this.db.tbl_game_group_association.Where<tbl_game_group_association>((Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) gameid && t.id_user == (int?) iUser && t.association_type == (int?) 1 && t.status == "A")).FirstOrDefault<tbl_game_group_association>() == null)
        {
          this.db.tbl_game_group_association.Add(new tbl_game_group_association()
          {
            id_game = new int?(gameid),
            id_game_group = new int?(0),
            id_organization = new int?(int32_1),
            status = "A",
            updated_date_time = new DateTime?(DateTime.Now),
            assigned_by = new int?(int32_2),
            association_type = new int?(1),
            start_date = nullable1,
            expiry_date = nullable2,
            id_user = new int?(iUser),
            creation_date = new DateTime?(DateTime.Now)
          });
          this.db.SaveChanges();
        }
      }
      return "1";
    }

    public string setMultiGroupToGame()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
      int int32_2 = Convert.ToInt32(content.ID_USER);
      DateTime? nullable1 = new DateTime?();
      DateTime? nullable2 = new DateTime?();
      int gameid = Convert.ToInt32(this.Request.Form["id_game_creation"]);
      nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["game-start-date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["game-start-date"], "dd-MM-yyyy HH:mm", (IFormatProvider) CultureInfo.InvariantCulture)) : new DateTime?();
      nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["game-expiry-date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["game-expiry-date"], "dd-MM-yyyy HH:mm", (IFormatProvider) CultureInfo.InvariantCulture)) : new DateTime?();
      string str = this.Request.Form["chk_group"].ToString();
      char[] chArray = new char[1]{ ',' };
      foreach (int num in Array.ConvertAll<string, int>(str.Split(chArray), new Converter<string, int>(int.Parse)))
      {
        int iGrp = num;
        if (this.db.tbl_game_group_association.Where<tbl_game_group_association>((Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) gameid && t.id_game_group == (int?) iGrp && t.association_type == (int?) 2 && t.status == "A")).FirstOrDefault<tbl_game_group_association>() == null)
        {
          this.db.tbl_game_group_association.Add(new tbl_game_group_association()
          {
            id_game = new int?(gameid),
            id_game_group = new int?(iGrp),
            id_organization = new int?(int32_1),
            status = "A",
            updated_date_time = new DateTime?(DateTime.Now),
            assigned_by = new int?(int32_2),
            start_date = nullable1,
            expiry_date = nullable2,
            association_type = new int?(2),
            id_user = new int?(0),
            creation_date = new DateTime?(DateTime.Now)
          });
          this.db.SaveChanges();
        }
      }
      return "1";
    }

    public ActionResult game_summary(string gid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      tbl_game_creation tblGameCreation = this.db.tbl_game_creation.Find(new object[1]
      {
        (object) Convert.ToInt32(gid)
      });
      List<GameGroupSummary> gameGroupSummaryList = new List<GameGroupSummary>();
      List<GameUserSummary> gameUserSummaryList = new List<GameUserSummary>();
      if (tblGameCreation == null)
        return (ActionResult) this.RedirectToAction("dashboard", "Gamification");
      if (tblGameCreation.player_type == "1")
        gameUserSummaryList = new GamificationModel().getGameUserSummary("" + "  SELECT c.id_user,  c.EMPLOYEEID,  c.USERID,  CONCAT(d.FIRSTNAME,  ' ',  d.LASTNAME,  ' - ',  c.EMPLOYEEID) UNAME, " + "   CASE  WHEN d.CITY IS NULL THEN d.LOCATION  ELSE d.city  END AS LOCATION,  c.user_designation, " + "   CASE  WHEN c.STATUS = 'A' THEN 'active'  WHEN c.STATUS = 'D' THEN 'de-active'  ELSE 'NA' " + "   END AS USTATUS,  DATE_FORMAT(a.start_date, '%d-%m-%Y') start_date,  DATE_FORMAT(a.expiry_date, '%d-%m-%Y') expiry_date " + "  FROM  tbl_game_group_association a  LEFT JOIN  tbl_game_creation b ON b.id_game = a.id_game  LEFT JOIN " + "   tbl_user c left join tbl_profile d on d.ID_USER=c.ID_USER ON a.id_user = c.ID_USER AND a.association_type = 1 " + "  WHERE  a.id_game = " + (object) tblGameCreation.id_game + " ");
      else if (tblGameCreation.player_type == "2")
        gameGroupSummaryList = new GamificationModel().getGameGroupSummary("" + "  SELECT  a.id_game_group ,c.group_name, DATE_FORMAT(a.start_date, '%d-%m-%Y') start_date, DATE_FORMAT(a.expiry_date, '%d-%m-%Y') expiry_date " + "  FROM tbl_game_group_association a LEFT JOIN tbl_game_creation b ON b.id_game = a.id_game " + "  LEFT JOIN tbl_game_group c ON a.id_game_group = c.id_game_group AND a.association_type = 2 WHERE a.id_game = " + (object) tblGameCreation.id_game + " ");
      this.ViewData["game"] = (object) tblGameCreation;
      this.ViewData["group"] = (object) gameGroupSummaryList;
      this.ViewData["users"] = (object) gameUserSummaryList;
      return (ActionResult) this.View();
    }

    public ActionResult configure_game(
      int phase = 0,
      int gameId = 0,
      List<assess_title> assessment_title = null,
      List<programs_title> prog = null)
    {
      if (phase == 2)
      {
        List<game_path_components> gamePath = new GamificationModel().get_game_path(gameId);
        List<game_path_components> progReview = new GamificationModel().get_prog_review(gameId);
        string gameName = new GamificationModel().get_game_name(gameId);
        this.ViewData["game_review"] = (object) gamePath;
        this.ViewData["pro_review"] = (object) progReview;
        this.ViewData["game_name"] = (object) gameName;
      }
      object obj1 = this.TempData["MYLIST"];
      object obj2 = this.TempData["pro_list"];
      this.ViewData[nameof (phase)] = (object) phase;
      this.ViewData[nameof (gameId)] = (object) gameId;
      this.ViewData["assess"] = this.TempData["MYLIST"];
      this.ViewData[nameof (prog)] = this.TempData["pro_list"];
      return (ActionResult) this.View();
    }

    public ActionResult Delete_Program(int pro_id, int game_id)
    {
      new GamificationModel().Delete_Program(game_id, pro_id);
      return (ActionResult) this.RedirectToAction("configure_game", "Gamification", (object) new
      {
        phase = 2,
        gameId = game_id
      });
    }

    public ActionResult Delete_Assessment(int ass_id, int game_id)
    {
      new GamificationModel().Delete_Assessment(game_id, ass_id);
      return (ActionResult) this.RedirectToAction("configure_game", "Gamification", (object) new
      {
        phase = 2,
        gameId = game_id
      });
    }

    public ActionResult phase_changing() => (ActionResult) this.RedirectToAction("configure_game", "Gamification", (object) new
    {
      phase = 2,
      gameId = Convert.ToInt32(this.Request.Form["gId"])
    });

    public ActionResult game_creation()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(content.ID_USER);
      int int32_2 = Convert.ToInt32(content.id_ORGANIZATION);
      Tuple<Tempmodel, List<assess_title>> game = new GamificationModel().CreateGame(new GameElements()
      {
        id_game = Convert.ToInt32(this.Request.Form["id_game"]),
        id_organization = int32_2,
        id_game_creator = int32_1,
        game_title = this.Request.Form["game_name"],
        game_description = this.Request.Form["comment"].ToString(),
        game_creator_name = this.Request.Form["game_creator_name"],
        game_expiry_date = Convert.ToDateTime(this.Request.Form["expiry_date"]),
        game_mode = this.Request.Form["game_mode"],
        game_type = this.Request.Form["game_type"],
        id_game_path = Convert.ToInt32(this.Request.Form["id_game_path"]),
        player_type = this.Request.Form["player_type"],
        id_group = Convert.ToInt32(this.Request.Form["id_group"]),
        game_comment = this.Request.Form["comment"],
        game_status = this.Request.Form["game_status"],
        game_creation_datetime = Convert.ToDateTime(this.Request.Form["game_creation_datetime"]),
        game_update_datetime = Convert.ToDateTime(this.Request.Form["game_update_datetime"]),
        game_phase = Convert.ToInt32(this.Request.Form["game_phase"]),
        status = this.Request.Form["status"]
      });
      int phaseflag = game.Item1.phaseflag;
      int idGame = game.Item1.idGame;
      List<assess_title> assessTitleList = new List<assess_title>();
      List<assess_title> source = game.Item2;
      source.Count<assess_title>();
      this.TempData["MYLIST"] = (object) source;
      return (ActionResult) this.RedirectToAction("configure_game", "Gamification", (object) new
      {
        phase = phaseflag,
        gameId = idGame,
        assessment_title = source
      });
    }

    public string get_program()
    {
      string program = "";
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.ID_USER);
      foreach (programs_title programsTitle in new GamificationModel().get_prog(Convert.ToInt32(content.id_ORGANIZATION)))
        program = program + "<option value=" + (object) programsTitle.program_id + ">" + programsTitle.programs_titl + "</option>";
      return program;
    }

    public string get_assess()
    {
      string assess = "";
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.ID_USER);
      foreach (assess_title assessTitle in new GamificationModel().get_assess(Convert.ToInt32(content.id_ORGANIZATION)))
        assess = assess + "<option value=" + (object) assessTitle.assess_id + " >" + assessTitle.assessment_title + "</option>";
      return assess;
    }

    public string attach_assess(string id, string gameIdval)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.ID_USER);
      Convert.ToInt32(content.id_ORGANIZATION);
      List<assess_title> assessTitleList = new GamificationModel().Insert_Assessment(id, gameIdval);
      string str = "";
      foreach (assess_title assessTitle in assessTitleList)
        str = "<tr style=background-color:#cb84cb;color:white><td>" + (object) assessTitle.assess_id + "</td><td>" + assessTitle.assessment_title + "</td><td></td><td></td><td></td><td></td></t > ";
      return str;
    }

    public string attach_Program(string id, string gameIdval)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.ID_USER);
      Convert.ToInt32(content.id_ORGANIZATION);
      List<programs_title> programsTitleList = new GamificationModel().Insert_Program(id, gameIdval);
      string str = "";
      foreach (programs_title programsTitle in programsTitleList)
        str = "<tr style=background-color:#cb84cb;color:white><td>" + (object) programsTitle.program_id + "</td><td>" + programsTitle.programs_titl + "</td><td></td><td></td><td></td><td></td></t > ";
      return str;
    }

    public ActionResult Configure_group()
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      this.ViewData["RoleList"] = (object) this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object) int32 + " and status='A'").ToList<tbl_csst_role>();
      this.ViewData["location"] = (object) new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object) int32 + " order by LOCATION ");
      return (ActionResult) this.View();
    }

    public string insert_group_name(string grp_name)
    {
      int num = new GamificationModel().insert_group_name(grp_name);
      return "<b><h3 style=text-align:center; value=" + (object) num + " id=grp_name_value></h3></b>" + "<div><input type=hidden id=gp_id value=" + (object) num + "/></div>";
    }

    public ActionResult SoloGameAssignment()
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<GameElements> soloGames = new GamificationModel().getSoloGames(int32);
      this.ViewData["RoleList"] = (object) this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object) int32 + " and status='A'").ToList<tbl_csst_role>();
      this.ViewData["location"] = (object) new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object) int32 + " order by LOCATION ");
      this.ViewData["sologames"] = (object) soloGames;
      return (ActionResult) this.View();
    }

    public ActionResult GroupGameAssignment()
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<GameElements> groupGames = new GamificationModel().getGroupGames(int32);
      this.ViewData["RoleList"] = (object) this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object) int32 + " and status='A'").ToList<tbl_csst_role>();
      this.ViewData["location"] = (object) new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object) int32 + " order by LOCATION ");
      this.ViewData["groupgames"] = (object) groupGames;
      return (ActionResult) this.View();
    }
  }
}
