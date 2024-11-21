// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.ScoringController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  [UserFilter]
  public class ScoringController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int OID = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      List<tbl_game_creation> list = this.db.tbl_game_creation.SqlQuery("SELECT * FROM tbl_game_creation where id_organisation= " + (object) OID + " and status='A'").ToList<tbl_game_creation>();
      this.db.tbl_game_creation.SqlQuery("SELECT * FROM tbl_game_creation where id_organisation= " + (object) OID + " and status='A' and id_game not in (select distinct id_game from sc_report_game_process_score )").ToList<tbl_game_creation>();
      new Thread((ThreadStart) (() => this.gameAutoEvel(OID))).Start();
      this.ViewData["master"] = (object) list;
      return (ActionResult) this.View();
    }

    public void ganeAutoEvel(List<tbl_game_creation> games)
    {
      foreach (tbl_game_creation game in games)
        this.GamePhase2(game);
    }

    public void gameAutoEvel(int oid)
    {
      foreach (tbl_game_creation tblGameCreation in this.db.tbl_game_creation.SqlQuery("SELECT * FROM tbl_game_creation where status='A' and id_organisation=" + (object) oid).ToList<tbl_game_creation>())
        this.setGameScore(tblGameCreation.id_game.ToString());
    }

    public string getGameResult(string gids)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      string str1 = "";
      if (!(gids == "0"))
        str1 = " and a.id_game=" + gids + " ";
      List<GameReport> gameReport1 = new GamificationModel().getGameReport("" + " SELECT c.id_user,  a.id_game,a.game_title, d.USERID, d.EMPLOYEEID, d.user_department, d.user_designation, d.user_function, " + " CASE  WHEN d.STATUS = 'A' THEN 'active'  WHEN d.STATUS = 'D' THEN 'de-active'  ELSE 'NA' END uStatus,concat(e.FIRSTNAME,' ',e.LASTNAME) UNAME,  " + "  e.city  LOCATION,   TRUNCATE(sum(c.kpi_weightage*b.weightage/100),2) final_weightage ,CASE WHEN a.game_type = '1' THEN 'Standalone' WHEN a.game_type = '2' THEN 'Journey' END game_type, CASE WHEN a.game_mode = '1' THEN 'Open' WHEN a.game_mode = '2' THEN 'Close' END game_mode, CASE WHEN a.player_type = '1' THEN 'Solo Game' WHEN a.player_type = '2' THEN 'Group Game' END player_type" + " FROM tbl_game_creation a, tbl_game_process_path b, sc_report_game_process_score c, tbl_user d left join tbl_profile e on d.ID_USER=e.id_user  " + " WHERE  a.player_type=1 and a.id_game = b.id_game  AND a.id_game = c.id_game  AND b.id_game = c.id_game  AND b.id_category = c.id_category  and c.id_user=d.id_user  and a.id_organisation=" + (object) int32 + " " + str1 + " " + " GROUP BY    a.id_game, c.id_user,UNAME,e.city");
      List<GameReport> gameReport2 = new GamificationModel().getGameReport("" + " SELECT '0' id_user,  a.id_game,a.game_title, 'NA' USERID, 'NA' EMPLOYEEID, 'NA' user_department, 'NA' user_designation, 'NA' user_function, " + "  'NA' uStatus,'NA'  UNAME,  " + " 'NA'  LOCATION,   TRUNCATE(sum(c.kpi_weightage*b.weightage/100),2) final_weightage ,CASE WHEN a.game_type = '1' THEN 'Standalone' WHEN a.game_type = '2' THEN 'Journey' END game_type, CASE WHEN a.game_mode = '1' THEN 'Open' WHEN a.game_mode = '2' THEN 'Close' END game_mode, CASE WHEN a.player_type = '1' THEN 'Solo Game' WHEN a.player_type = '2' THEN 'Group Game' END player_type" + " FROM tbl_game_creation a, tbl_game_process_path b, sc_report_game_process_score c, tbl_user d left join tbl_profile e on d.ID_USER=e.id_user  " + " WHERE  a.player_type=2 and a.id_game = b.id_game  AND a.id_game = c.id_game  AND b.id_game = c.id_game  AND b.id_category = c.id_category  and c.id_user=d.id_user  and a.id_organisation=" + (object) int32 + " " + str1 + " " + " GROUP BY a.id_game ");
      gameReport1.AddRange((IEnumerable<GameReport>) gameReport2);
      List<GameReport> list = gameReport1.OrderByDescending<GameReport, int>((Func<GameReport, int>) (t => t.id_game)).ToList<GameReport>();
      string str2 = "";
      foreach (GameReport gameReport3 in list)
      {
        str2 += "<tr>";
        str2 = str2 + "<td>" + gameReport3.game_title + " </td>";
        str2 = str2 + "<td>" + gameReport3.player_type + " </td>";
        str2 = str2 + "<td>" + gameReport3.game_type + " </td>";
        str2 = str2 + "<td>" + gameReport3.UNAME + " </td>";
        str2 = str2 + "<td>" + gameReport3.LOCATION + " </td>";
        str2 = str2 + "<td>" + gameReport3.user_department + " </td>";
        str2 = str2 + "<td>" + gameReport3.user_designation + " </td>";
        str2 = str2 + "<td>" + gameReport3.user_function + " </td>";
        str2 = str2 + "<td>" + (object) gameReport3.final_weightage + " </td>";
        if (gameReport3.player_type == "Group Game")
          str2 = str2 + "<td><a  href=\"javascript:void(0)\" onclick=\"getDetailsReport('" + (object) gameReport3.id_game + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a></td>";
        else
          str2 = str2 + "<td><a  href=\"javascript:void(0)\" onclick=\"getDetailsUserReport('" + (object) gameReport3.id_game + "','" + (object) gameReport3.id_user + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a></td>";
        str2 += "</tr>";
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th>Game</th>" + " <th>Game Type</th>" + " <th>Play Type</th>" + " <th>Name</th>" + " <th>Location</th>" + " <th>Department</th>" + " <th>Designation</th>" + " <th>Function</th>" + " <th>Overall Score</th>" + "<th></th>" + "</thead>" + "<tbody>" + str2 + "</tbody></table>";
    }

    public GameProgramSummary getGameProgramUserResult(
      int? id_category,
      int? id_user,
      int? id_organization)
    {
      GameProgramSummary programUserResult = new GameProgramSummary();
      tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => (int?) t.ID_CATEGORY == id_category && t.STATUS == "A")).FirstOrDefault<tbl_category>();
      if (tblCategory != null)
      {
        tbl_user tblUser = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>) (t => (int?) t.ID_USER == id_user)).FirstOrDefault<tbl_user>();
        tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>) (t => (int?) t.ID_USER == id_user)).FirstOrDefault<tbl_profile>();
        programUserResult.id_user = tblUser.ID_USER;
        programUserResult.id_category = tblCategory.ID_CATEGORY;
        programUserResult.category_name = tblCategory.CATEGORYNAME;
        programUserResult.user_name = tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME;
        programUserResult.userid = tblUser.USERID;
        programUserResult.employeeid = tblUser.EMPLOYEEID;
        sc_program_content_summary programContentSummary = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_user == id_user && t.id_category == id_category && t.id_organization == id_organization)).FirstOrDefault<sc_program_content_summary>();
        if (programContentSummary != null)
        {
          programUserResult.content_score = Convert.ToDouble((object) programContentSummary.percentage);
          programUserResult.content_weightage = Convert.ToDouble((object) programContentSummary.content_weightage);
        }
        this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_category == id_category && t.id_user == id_user && t.id_organization == id_organization)).ToList<sc_program_assessment_scoring>();
        List<scoring_matrix> scoringMatrixList = new List<scoring_matrix>();
        List<scoring_matrix> scoreMatrix1 = new ProgramScoringModel().getScoreMatrix("" + " SELECT     a.id_assessment id_element,    a.assessment_title element_name,    assessment_score vk_score,    assessment_wieghtage vk_weightage " + " FROM    tbl_assessment a ,    sc_program_assessment_scoring b    where a.id_assessment=b.id_assessment " + " and b.id_category=" + (object) id_category + "    and b.id_user=" + (object) id_user + " ");
        programUserResult.assessment_score = scoreMatrix1;
        List<scoring_matrix> scoreMatrix2 = new ProgramScoringModel().getScoreMatrix("" + " SELECT a.id_kpi_master id_element, a.kpi_name element_name, case when score is null then 0 else score end vk_score, case when kpi_wieghtage is null then 0 else kpi_wieghtage end vk_weightage " + " FROM tbl_kpi_master a LEFT JOIN tbl_kpi_program_scoring b ON a.id_kpi_master = b.id_kpi_master " + " LEFT JOIN sc_program_kpi_weightage c ON a.id_kpi_master = c.id_kpi_master and c.id_user=" + (object) id_user + " " + " WHERE  b.id_category = c.id_category AND b.kpi_type = 3 AND b.id_category = " + (object) id_category + " ");
        programUserResult.kpi_score = scoreMatrix2;
      }
      return programUserResult;
    }

    public ActionResult ProgramResult()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      this.ViewData["RoleList"] = (object) this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object) int32 + " and status='A'").ToList<tbl_csst_role>();
      List<string> userLocation = new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object) int32 + " order by LOCATION ");
      List<string> userFunction = new contentDashboardModel().getUserFunction(" SELECT distinct upper(user_function) user_function FROM tbl_user where ID_ORGANIZATION=" + (object) int32 + " and user_function is not null order by user_function");
      this.ViewData["location"] = (object) userLocation;
      this.ViewData["function"] = (object) userFunction;
      return (ActionResult) this.View();
    }

    public string getGameProgramResult(string gid, string uid)
    {
      int gids = Convert.ToInt32(gid);
      tbl_game_creation game = this.db.tbl_game_creation.Where<tbl_game_creation>((Expression<Func<tbl_game_creation, bool>>) (t => t.id_game == gids)).FirstOrDefault<tbl_game_creation>();
      string gameProgramResult = "";
      if (game != null)
      {
        this.GamePhase2(game);
        List<tbl_game_process_path> list1 = this.db.tbl_game_process_path.Where<tbl_game_process_path>((Expression<Func<tbl_game_process_path, bool>>) (t => t.id_game == (int?) game.id_game)).OrderBy<tbl_game_process_path, int?>((Expression<Func<tbl_game_process_path, int?>>) (t => t.sequence_number)).ToList<tbl_game_process_path>();
        if (game.player_type == "1")
        {
          int int32 = Convert.ToInt32(uid);
          int num1 = 1;
          foreach (tbl_game_process_path tblGameProcessPath in list1)
          {
            int num2 = 0;
            GameProgramSummary programUserResult = this.getGameProgramUserResult(tblGameProcessPath.id_category, new int?(int32), game.id_organisation);
            int num3 = 0;
            double num4 = 0.0;
            if (num2 == 0)
            {
              gameProgramResult = gameProgramResult + "<table id=\"gpsdisplay" + (object) num1 + "\" class=\"gpsdisplay table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\">";
              int num5 = num2 + 1;
              gameProgramResult += "<thead><tr>";
              gameProgramResult += "<th>Name</th>";
              gameProgramResult += "<th>Program</th>";
              gameProgramResult += "<th>Content Access</th>";
              foreach (scoring_matrix scoringMatrix in programUserResult.assessment_score)
                gameProgramResult = gameProgramResult + "<th>" + scoringMatrix.element_name + "</th>";
              foreach (scoring_matrix scoringMatrix in programUserResult.kpi_score)
                gameProgramResult = gameProgramResult + "<th>" + scoringMatrix.element_name + "</th>";
              gameProgramResult += "<th>Overall Score</th>";
              gameProgramResult += "</tr></thead>";
              gameProgramResult += "<tbody>";
            }
            gameProgramResult += "<tr>";
            gameProgramResult = gameProgramResult + "<td>" + programUserResult.user_name + "</td>";
            gameProgramResult = gameProgramResult + "<td>" + programUserResult.category_name + "</td>";
            gameProgramResult = gameProgramResult + "<td>" + (object) programUserResult.content_weightage + "</td>";
            double num6 = num4 + programUserResult.content_weightage;
            int num7 = num3 + 1;
            foreach (scoring_matrix scoringMatrix in programUserResult.assessment_score)
            {
              gameProgramResult = gameProgramResult + "<td>" + (object) scoringMatrix.vk_weightage + "</td>";
              num6 += scoringMatrix.vk_weightage;
              ++num7;
            }
            foreach (scoring_matrix scoringMatrix in programUserResult.kpi_score)
            {
              gameProgramResult = gameProgramResult + "<td>" + (object) scoringMatrix.vk_weightage + "</td>";
              num6 += scoringMatrix.vk_weightage;
              ++num7;
            }
            gameProgramResult = gameProgramResult + "<td>" + (object) num6 + "</td>";
            gameProgramResult += "</tr>";
            gameProgramResult += "</tbody></table><br/><hr/>";
            ++num1;
          }
        }
        else
        {
          DbSet<tbl_game_group_association> groupAssociation1 = this.db.tbl_game_group_association;
          Expression<Func<tbl_game_group_association, bool>> predicate = (Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) game.id_game);
          foreach (tbl_game_group_association groupAssociation2 in groupAssociation1.Where<tbl_game_group_association>(predicate).ToList<tbl_game_group_association>())
          {
            tbl_game_group_association iAsso = groupAssociation2;
            List<tbl_game_group_participatant> list2 = this.db.tbl_game_group_participatant.Where<tbl_game_group_participatant>((Expression<Func<tbl_game_group_participatant, bool>>) (t => t.id_game_group == iAsso.id_game_group)).ToList<tbl_game_group_participatant>();
            int num8 = 1;
            foreach (tbl_game_process_path tblGameProcessPath in list1)
            {
              int num9 = 0;
              foreach (tbl_game_group_participatant groupParticipatant in list2)
              {
                GameProgramSummary programUserResult = this.getGameProgramUserResult(tblGameProcessPath.id_category, groupParticipatant.id_user, game.id_organisation);
                int num10 = 0;
                double num11 = 0.0;
                if (num9 == 0)
                {
                  gameProgramResult = gameProgramResult + "<table id=\"gpsdisplay" + (object) num8 + "\" class=\"gpsdisplay table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\">";
                  ++num9;
                  gameProgramResult += "<thead><tr>";
                  gameProgramResult += "<th>Name</th>";
                  gameProgramResult += "<th>Program</th>";
                  gameProgramResult += "<th>Content Access</th>";
                  foreach (scoring_matrix scoringMatrix in programUserResult.assessment_score)
                    gameProgramResult = gameProgramResult + "<th>" + scoringMatrix.element_name + "</th>";
                  foreach (scoring_matrix scoringMatrix in programUserResult.kpi_score)
                    gameProgramResult = gameProgramResult + "<th>" + scoringMatrix.element_name + "</th>";
                  gameProgramResult += "<th>Overall Score</th>";
                  gameProgramResult += "</tr></thead>";
                  gameProgramResult += "<tbody>";
                }
                gameProgramResult += "<tr>";
                gameProgramResult = gameProgramResult + "<td>" + programUserResult.user_name + "</td>";
                gameProgramResult = gameProgramResult + "<td>" + programUserResult.category_name + "</td>";
                gameProgramResult = gameProgramResult + "<td>" + (object) programUserResult.content_weightage + "</td>";
                double num12 = num11 + programUserResult.content_weightage;
                int num13 = num10 + 1;
                foreach (scoring_matrix scoringMatrix in programUserResult.assessment_score)
                {
                  gameProgramResult = gameProgramResult + "<td>" + (object) scoringMatrix.vk_weightage + "</td>";
                  num12 += scoringMatrix.vk_weightage;
                  ++num13;
                }
                foreach (scoring_matrix scoringMatrix in programUserResult.kpi_score)
                {
                  gameProgramResult = gameProgramResult + "<td>" + (object) scoringMatrix.vk_weightage + "</td>";
                  num12 += scoringMatrix.vk_weightage;
                  ++num13;
                }
                gameProgramResult = gameProgramResult + "<td>" + (object) (num12 / (double) num13) + "</td>";
                gameProgramResult += "</tr>";
              }
              gameProgramResult += "</tbody></table><br/><hr/>";
              ++num8;
            }
          }
        }
      }
      return gameProgramResult;
    }

    public string getGameReport(string gids)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int orgid = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      string str1 = "";
      if (!(gids == "0"))
        str1 = " and a.id_game=" + gids + " ";
      List<GameReport> gameReport1 = new GamificationModel().getGameReport("" + " SELECT c.id_user,  a.id_game,  a.game_title,  d.USERID,  d.EMPLOYEEID, d.user_department,  d.user_designation,  d.user_function, " + " CASE WHEN d.STATUS = 'A' THEN 'active' WHEN d.STATUS = 'D' THEN 'de-active' ELSE 'NA'  END uStatus,  CONCAT(e.FIRSTNAME, ' ', e.LASTNAME) UNAME,  e.city LOCATION, " + " CASE WHEN a.game_type = '1' THEN 'Standalone' WHEN a.game_type = '2' THEN 'Journey'  END game_type,  CASE WHEN a.game_mode = '1' THEN 'Open' WHEN a.game_mode = '2' THEN 'Close' " + " END game_mode,  CASE WHEN a.player_type = '1' THEN 'Solo Game' WHEN a.player_type = '2' THEN 'Group Game'  END player_type,  TRUNCATE(SUM(c.kpi_weightage), 2) final_weightage,  COUNT(DISTINCT c.id_user) ucount " + " FROM  tbl_game_creation a,  tbl_game_process_path b,  sc_report_game_process_score c,  tbl_user d LEFT JOIN  tbl_profile e ON d.ID_USER = e.id_user " + " WHERE  a.status='A' " + str1 + " and a.id_organisation=" + (object) orgid + " and a.player_type = 1 AND a.id_game = b.id_game AND a.id_game = c.id_game AND b.id_game = c.id_game AND b.id_category = c.id_category " + " AND c.id_user = d.id_user GROUP BY a.id_game , c.id_user ,UNAME,e.city");
      if (str1 != "")
      {
        List<GameReport> gameReport2 = new GamificationModel().getGameReport("" + " SELECT g.id_game_group id_user, a.id_game, a.game_title,   'NA' USERID, 'NA' EMPLOYEEID, 'NA' user_department, 'NA' user_designation, 'NA' user_function, 'NA' uStatus, g.group_name UNAME, 'NA' LOCATION,  " + " CASE WHEN a.game_type = '1' THEN 'Standalone' WHEN a.game_type = '2' THEN 'Journey' END game_type, " + " CASE WHEN a.game_mode = '1' THEN 'Open' WHEN a.game_mode = '2' THEN 'Close' END game_mode, " + " CASE WHEN a.player_type = '1' THEN 'Solo Game' WHEN a.player_type = '2' THEN 'Group Game' END player_type, " + " TRUNCATE(SUM(c.kpi_weightage), 2) final_weightage, COUNT(DISTINCT c.id_user) ucount " + " FROM tbl_game_creation a, tbl_game_group g, tbl_game_group_participatant gp, tbl_game_process_path b, sc_report_game_process_score c " + " WHERE  a.status='A' and a.id_organisation=" + (object) orgid + " and a.player_type = 2 and a.id_game = b.id_game AND a.id_game = c.id_game AND b.id_game = c.id_game AND b.id_category = c.id_category AND g.id_game_group = gp.id_game_group AND gp.id_user = c.id_user " + " " + str1 + " GROUP BY a.id_game , g.id_game_group ");
        gameReport1.AddRange((IEnumerable<GameReport>) gameReport2);
      }
      else
      {
        DbSet<tbl_game_creation> tblGameCreation1 = this.db.tbl_game_creation;
        Expression<Func<tbl_game_creation, bool>> predicate = (Expression<Func<tbl_game_creation, bool>>) (t => t.player_type == "2" && t.status == "A" && t.id_organisation == (int?) orgid);
        foreach (tbl_game_creation tblGameCreation2 in tblGameCreation1.Where<tbl_game_creation>(predicate).ToList<tbl_game_creation>())
        {
          List<GameReport> gameReport3 = new GamificationModel().getGameReport("" + " SELECT g.id_game_group id_user, a.id_game, a.game_title,   'NA' USERID, 'NA' EMPLOYEEID, 'NA' user_department, 'NA' user_designation, 'NA' user_function, 'NA' uStatus, g.group_name UNAME, 'NA' LOCATION,  " + " CASE WHEN a.game_type = '1' THEN 'Standalone' WHEN a.game_type = '2' THEN 'Journey' END game_type, " + " CASE WHEN a.game_mode = '1' THEN 'Open' WHEN a.game_mode = '2' THEN 'Close' END game_mode, " + " CASE WHEN a.player_type = '1' THEN 'Solo Game' WHEN a.player_type = '2' THEN 'Group Game' END player_type, " + " TRUNCATE(SUM(c.kpi_weightage), 2) final_weightage, COUNT(DISTINCT c.id_user) ucount " + " FROM tbl_game_creation a, tbl_game_group g, tbl_game_group_participatant gp, tbl_game_process_path b, sc_report_game_process_score c " + " WHERE  a.status='A' and a.id_organisation=" + (object) orgid + " and a.player_type = 2 and a.id_game = b.id_game AND a.id_game = c.id_game AND b.id_game = c.id_game AND b.id_category = c.id_category AND g.id_game_group = gp.id_game_group AND gp.id_user = c.id_user " + " AND a.id_game = " + (object) tblGameCreation2.id_game + " GROUP BY a.id_game , g.id_game_group ");
          gameReport1.AddRange((IEnumerable<GameReport>) gameReport3);
        }
      }
      List<GameReport> list = gameReport1.OrderByDescending<GameReport, int>((Func<GameReport, int>) (t => t.id_game)).ToList<GameReport>();
      string str2 = "";
      foreach (GameReport gameReport4 in list)
      {
        str2 += "<tr>";
        str2 = str2 + "<td>" + gameReport4.game_title + " </td>";
        str2 = str2 + "<td>" + gameReport4.player_type + " </td>";
        str2 = str2 + "<td>" + gameReport4.game_type + " </td>";
        str2 = str2 + "<td>" + gameReport4.UNAME + " </td>";
        str2 = str2 + "<td>" + gameReport4.LOCATION + " </td>";
        str2 = str2 + "<td>" + gameReport4.user_department + " </td>";
        str2 = str2 + "<td>" + gameReport4.user_designation + " </td>";
        str2 = str2 + "<td>" + gameReport4.user_function + " </td>";
        str2 = str2 + "<td>" + (object) Math.Round(gameReport4.final_score, 2) + " </td>";
        if (gameReport4.player_type == "Group Game")
          str2 = str2 + "<td><a  href=\"javascript:void(0)\" onclick=\"getDetailsReport('" + (object) gameReport4.id_game + "','" + (object) gameReport4.id_user + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a></td>";
        else
          str2 = str2 + "<td><a  href=\"javascript:void(0)\" onclick=\"getDetailsUserReport('" + (object) gameReport4.id_game + "','" + (object) gameReport4.id_user + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a></td>";
        str2 += "</tr>";
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th>Game</th>" + " <th>Game Type</th>" + " <th>Play Type</th>" + " <th>Name</th>" + " <th>Location</th>" + " <th>Department</th>" + " <th>Designation</th>" + " <th>Function</th>" + " <th>Overall Score</th>" + "<th></th>" + "</thead>" + "<tbody>" + str2 + "</tbody></table>";
    }

    public string getGameProgramReport(string gid, string uid)
    {
      int gids = Convert.ToInt32(gid);
      tbl_game_creation game = this.db.tbl_game_creation.Where<tbl_game_creation>((Expression<Func<tbl_game_creation, bool>>) (t => t.id_game == gids)).FirstOrDefault<tbl_game_creation>();
      string str1 = "";
      string gameProgramReport = "";
      int uids = Convert.ToInt32(uid);
      if (game != null)
      {
        List<tbl_game_process_path> list1 = this.db.tbl_game_process_path.Where<tbl_game_process_path>((Expression<Func<tbl_game_process_path, bool>>) (t => t.id_game == (int?) game.id_game)).OrderBy<tbl_game_process_path, int?>((Expression<Func<tbl_game_process_path, int?>>) (t => t.sequence_number)).ToList<tbl_game_process_path>();
        if (game.player_type == "1")
        {
          string str2 = gameProgramReport + "<table id=\"gpsdisplay1\" class=\"gpsdisplay table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\">" + "<thead><tr>" + "<th><div class=\"row\"><div class=\"col-md-4\">Program</div><div class=\"col-md-6\"><div class=\"row\"><div class=\"col-md-4\">KPI</div><div class=\"col-md-2\">score in %</div><div class=\"col-md-2\">Grid Score</div><div class=\"col-md-2\">Weightage</div><div class=\"col-md-2\">Score</div></div></div><div class=\"col-md-1\">Program Scroe</div><div class=\"col-md-1\">Program Weightage</div></div></th>" + "<th>Overall Score</th>" + "</tr></thead>" + "<tbody>" + "<tr>";
          double? nullable1 = new double?(0.0);
          string str3 = "";
          foreach (tbl_game_process_path tblGameProcessPath in list1)
          {
            double num1 = 0.0;
            GameProgramSummary programUserResult = this.getGameProgramUserResult(tblGameProcessPath.id_category, new int?(uids), game.id_organisation);
            string str4 = "";
            string str5 = " FROM sc_program_content_summary a, tbl_kpi_program_scoring b WHERE a.id_category = b.id_category and a.id_category=" + (object) tblGameProcessPath.id_category + " AND b.kpi_type = 1 and a.id_user = " + (object) uids;
            foreach (ProgramReport programReport in new ProgramScoringModel().getProgramReport("SELECT a.id_user, TRUNCATE(a.percentage, 2) percentage,a.content_weightage weightage,b.ps_weightage pweightage,(ps_weightage * a.content_weightage/100) score " + str5))
            {
              num1 += programReport.score;
              str4 = str4 + "<div class=\"row div-border\"><div class=\"col-md-4 div-border\">Content KPI</div><div class=\"col-md-2\">" + (object) programReport.percentage + "</div><div class=\"col-md-2\">" + (object) programReport.weightage + "</div><div class=\"col-md-2\">" + (object) programReport.pweightage + "</div><div class=\"col-md-2\">" + (object) programReport.score + "</div></div>";
            }
            foreach (scoring_matrix scoringMatrix in programUserResult.assessment_score)
            {
              foreach (ProgramReport programReport in new ProgramScoringModel().getProgramReport("" + "SELECT  a.id_user,    TRUNCATE(a.assessment_score, 2) percentage,    a.assessment_wieghtage weightage,    b.ps_weightage pweightage,    (ps_weightage * a.assessment_wieghtage/100) score FROM    sc_program_assessment_scoring a,    tbl_kpi_program_scoring b " + (" WHERE a.id_category = b.id_category and a.id_category=" + (object) tblGameProcessPath.id_category + " AND b.kpi_type = 2 and a.id_assessment = " + (object) scoringMatrix.id_element + " and a.id_assessment = b.id_assessment and a.id_user = " + (object) uids)))
              {
                num1 += programReport.score;
                str4 = str4 + "<div class=\"row\"><div class=\"col-md-4\">" + scoringMatrix.element_name + "</div><div class=\"col-md-2\">" + (object) programReport.percentage + "</div><div class=\"col-md-2\">" + (object) programReport.weightage + "</div><div class=\"col-md-2\">" + (object) programReport.pweightage + "</div><div class=\"col-md-2\">" + (object) programReport.score + "</div></div>";
              }
            }
            foreach (scoring_matrix scoringMatrix in programUserResult.kpi_score)
            {
              foreach (ProgramReport programReport in new ProgramScoringModel().getProgramReport("" + "  SELECT a.id_user, TRUNCATE(a.score, 2) percentage,  a.kpi_wieghtage weightage, b.ps_weightage pweightage, (ps_weightage * a.kpi_wieghtage / 100) score FROM  sc_program_kpi_weightage a, tbl_kpi_program_scoring b " + ("  WHERE a.id_category = b.id_category  and a.id_category=" + (object) tblGameProcessPath.id_category + "  AND b.kpi_type = 3 and a.id_kpi_master =" + (object) scoringMatrix.id_element + " and a.id_kpi_master = b.id_kpi_master and a.id_user = " + (object) uids)))
              {
                num1 += programReport.score;
                str4 = str4 + "<div class=\"row\"><div class=\"col-md-4\">" + scoringMatrix.element_name + "</div><div class=\"col-md-2\">" + (object) programReport.percentage + "</div><div class=\"col-md-2\">" + (object) programReport.weightage + "</div><div class=\"col-md-2\">" + (object) programReport.pweightage + "</div><div class=\"col-md-2\">" + (object) programReport.score + "</div></div>";
              }
            }
            double num2 = num1;
            double? nullable2 = tblGameProcessPath.weightage;
            double? nullable3 = nullable2.HasValue ? new double?(num2 * nullable2.GetValueOrDefault()) : new double?();
            double num3 = 100.0;
            double? nullable4;
            if (!nullable3.HasValue)
            {
              nullable2 = new double?();
              nullable4 = nullable2;
            }
            else
              nullable4 = new double?(nullable3.GetValueOrDefault() / num3);
            double? nullable5 = nullable4;
            str3 = str3 + "<div class=\"row dp-border\"><div class=\"col-md-4\">" + programUserResult.category_name + "</div><div class=\"col-md-6\">" + str4 + "</div><div class=\"col-md-1\">" + (object) num1 + "</div><div class=\"col-md-1\">" + (object) nullable5 + "</div></div>";
            double? nullable6 = nullable1;
            nullable2 = nullable5;
            nullable1 = nullable6.HasValue & nullable2.HasValue ? new double?(nullable6.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new double?();
          }
          gameProgramReport = str2 + "<td vertical-align=\"middle\">" + str3 + "</td>" + "<td vertical-align=\"middle\">" + (object) nullable1 + "</td>" + "</tr>" + "</tbody></table><br/><hr/>";
        }
        else
        {
          DbSet<tbl_game_group_association> groupAssociation1 = this.db.tbl_game_group_association;
          Expression<Func<tbl_game_group_association, bool>> predicate = (Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) game.id_game && t.id_game_group == (int?) uids);
          foreach (tbl_game_group_association groupAssociation2 in groupAssociation1.Where<tbl_game_group_association>(predicate).ToList<tbl_game_group_association>())
          {
            tbl_game_group_association iAsso = groupAssociation2;
            tbl_game_group tblGameGroup = this.db.tbl_game_group.Where<tbl_game_group>((Expression<Func<tbl_game_group, bool>>) (t => (int?) t.id_game_group == iAsso.id_game_group)).FirstOrDefault<tbl_game_group>();
            List<tbl_game_group_participatant> list2 = this.db.tbl_game_group_participatant.Where<tbl_game_group_participatant>((Expression<Func<tbl_game_group_participatant, bool>>) (t => t.id_game_group == iAsso.id_game_group)).ToList<tbl_game_group_participatant>();
            gameProgramReport = gameProgramReport + "<div class=\"row\"><div class=\"col-md-12\"><center><h3>" + tblGameGroup.group_name + "</h3></center></div></div></div></hr>";
            foreach (tbl_game_group_participatant groupParticipatant in list2)
            {
              gameProgramReport += "<table id=\"gpsdisplay1\" class=\"gpsdisplay table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\">";
              gameProgramReport += "<thead><tr>";
              gameProgramReport += "<th>User</th>";
              gameProgramReport += "<th><div class=\"row\"><div class=\"col-md-4\">Program</div><div class=\"col-md-6\"><div class=\"row\"><div class=\"col-md-4\">KPI</div><div class=\"col-md-2\">score in %</div><div class=\"col-md-2\">Grid Score</div><div class=\"col-md-2\">Weightage</div><div class=\"col-md-2\">Score</div></div></div><div class=\"col-md-1\">Program Scroe</div><div class=\"col-md-1\">Program Weightage</div></div></th>";
              gameProgramReport += "<th>Overall Score</th>";
              gameProgramReport += "</tr></thead>";
              gameProgramReport += "<tbody>";
              gameProgramReport += "<tr>";
              double? nullable7 = new double?(0.0);
              string str6 = "";
              string str7 = "";
              foreach (tbl_game_process_path tblGameProcessPath in list1)
              {
                double num4 = 0.0;
                GameProgramSummary programUserResult = this.getGameProgramUserResult(tblGameProcessPath.id_category, groupParticipatant.id_user, game.id_organisation);
                string str8 = "";
                str7 = programUserResult.user_name;
                string str9 = " FROM sc_program_content_summary a, tbl_kpi_program_scoring b WHERE a.id_category = b.id_category and a.id_category=" + (object) tblGameProcessPath.id_category + " AND b.kpi_type = 1 and a.id_user = " + (object) groupParticipatant.id_user;
                foreach (ProgramReport programReport in new ProgramScoringModel().getProgramReport("SELECT a.id_user, TRUNCATE(a.percentage, 2) percentage,a.content_weightage weightage,b.ps_weightage pweightage,(ps_weightage * a.content_weightage/100) score " + str9))
                {
                  num4 += programReport.score;
                  str8 = str8 + "<div class=\"row dp-border\"><div class=\"col-md-4\">Content KPI</div><div class=\"col-md-2\">" + (object) programReport.percentage + "</div><div class=\"col-md-2\">" + (object) programReport.weightage + "</div><div class=\"col-md-2\">" + (object) programReport.pweightage + "</div><div class=\"col-md-2\">" + (object) programReport.score + "</div></div>";
                }
                foreach (scoring_matrix scoringMatrix in programUserResult.assessment_score)
                {
                  foreach (ProgramReport programReport in new ProgramScoringModel().getProgramReport("" + "SELECT  a.id_user,    TRUNCATE(a.assessment_score, 2) percentage,    a.assessment_wieghtage weightage,    b.ps_weightage pweightage,    (ps_weightage * a.assessment_wieghtage/100) score FROM    sc_program_assessment_scoring a,    tbl_kpi_program_scoring b " + (" WHERE a.id_category = b.id_category and a.id_category=" + (object) tblGameProcessPath.id_category + " AND b.kpi_type = 2 and a.id_assessment = " + (object) scoringMatrix.id_element + " and a.id_assessment = b.id_assessment and a.id_user = " + (object) groupParticipatant.id_user)))
                  {
                    num4 += programReport.score;
                    str8 = str8 + "<div class=\"row dp-border\"><div class=\"col-md-4\">" + scoringMatrix.element_name + "</div><div class=\"col-md-2\">" + (object) programReport.percentage + "</div><div class=\"col-md-2\">" + (object) programReport.weightage + "</div><div class=\"col-md-2\">" + (object) programReport.pweightage + "</div><div class=\"col-md-2\">" + (object) programReport.score + "</div></div>";
                  }
                }
                foreach (scoring_matrix scoringMatrix in programUserResult.kpi_score)
                {
                  foreach (ProgramReport programReport in new ProgramScoringModel().getProgramReport("" + "  SELECT a.id_user, TRUNCATE(a.score, 2) percentage,  a.kpi_wieghtage weightage, b.ps_weightage pweightage, (ps_weightage * a.kpi_wieghtage / 100) score FROM  sc_program_kpi_weightage a, tbl_kpi_program_scoring b " + ("  WHERE a.id_category = b.id_category  and a.id_category=" + (object) tblGameProcessPath.id_category + "  AND b.kpi_type = 3 and a.id_kpi_master =" + (object) scoringMatrix.id_element + " and a.id_kpi_master = b.id_kpi_master and a.id_user = " + (object) groupParticipatant.id_user)))
                  {
                    num4 += programReport.score;
                    str8 = str8 + "<div class=\"row dp-border\"><div class=\"col-md-4\">" + scoringMatrix.element_name + "</div><div class=\"col-md-2\">" + (object) programReport.percentage + "</div><div class=\"col-md-2\">" + (object) programReport.weightage + "</div><div class=\"col-md-2\">" + (object) programReport.pweightage + "</div><div class=\"col-md-2\">" + (object) programReport.score + "</div></div>";
                  }
                }
                double num5 = num4;
                double? nullable8 = tblGameProcessPath.weightage;
                double? nullable9 = nullable8.HasValue ? new double?(num5 * nullable8.GetValueOrDefault()) : new double?();
                double num6 = 100.0;
                double? nullable10;
                if (!nullable9.HasValue)
                {
                  nullable8 = new double?();
                  nullable10 = nullable8;
                }
                else
                  nullable10 = new double?(nullable9.GetValueOrDefault() / num6);
                double? nullable11 = nullable10;
                str6 = str6 + "<div class=\"row dp-border\"><div class=\"col-md-4\">" + programUserResult.category_name + "</div><div class=\"col-md-6\">" + str8 + "</div><div class=\"col-md-1\">" + (object) num4 + "</div><div class=\"col-md-1\">" + (object) nullable11 + "</div></div>";
                double? nullable12 = nullable7;
                nullable8 = nullable11;
                nullable7 = nullable12.HasValue & nullable8.HasValue ? new double?(nullable12.GetValueOrDefault() + nullable8.GetValueOrDefault()) : new double?();
              }
              gameProgramReport = gameProgramReport + "<td vertical-align=\"middle\">" + str7 + "</td>";
              gameProgramReport = gameProgramReport + "<td vertical-align=\"middle\">" + str6 + "</td>";
              gameProgramReport = gameProgramReport + "<td vertical-align=\"middle\">" + (object) nullable7 + "</td>";
              gameProgramReport += "</tr>";
              gameProgramReport += "</tbody></table><br/><hr/>";
              str1 += gameProgramReport;
            }
          }
        }
      }
      return gameProgramReport;
    }

    public ActionResult scoreAssessment()
    {
      DbSet<tbl_assessmnt_log> tblAssessmntLog1 = this.db.tbl_assessmnt_log;
      Expression<Func<tbl_assessmnt_log, bool>> predicate = (Expression<Func<tbl_assessmnt_log, bool>>) (t => t.attempt_no == 1);
      foreach (tbl_assessmnt_log tblAssessmntLog2 in tblAssessmntLog1.Where<tbl_assessmnt_log>(predicate).ToList<tbl_assessmnt_log>())
      {
        tbl_assessmnt_log log = tblAssessmntLog2;
        Convert.ToInt32((object) log.id_organization);
        if (this.db.tbl_rs_type_qna.Where<tbl_rs_type_qna>((Expression<Func<tbl_rs_type_qna, bool>>) (t => t.id_assessment_log == (int?) log.id_assessmnt_log)).FirstOrDefault<tbl_rs_type_qna>() == null)
        {
          tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_assessment_sheet == log.id_assessment_sheet)).FirstOrDefault<tbl_assessment_sheet>();
          tbl_assessment assessment = this.db.tbl_assessment.Find(new object[1]
          {
            (object) sheet.id_assesment
          });
          int? idAssessmentTheme = sheet.id_assessment_theme;
          int num1 = 1;
          if ((idAssessmentTheme.GetValueOrDefault() == num1 ? (idAssessmentTheme.HasValue ? 1 : 0) : 0) != 0)
          {
            if (this.db.tbl_rs_type_qna.Where<tbl_rs_type_qna>((Expression<Func<tbl_rs_type_qna, bool>>) (t => t.id_assessment == (int?) assessment.id_assessment && t.id_user == (int?) log.id_user && t.attempt_number == (int?) log.attempt_no)).FirstOrDefault<tbl_rs_type_qna>() == null)
            {
              tbl_rs_type_qna entity = new tbl_rs_type_qna();
              entity.id_user = new int?(log.id_user);
              entity.id_assessment = new int?(assessment.id_assessment);
              entity.id_assessment_log = new int?(log.id_assessmnt_log);
              entity.id_assessment_sheet = new int?(sheet.id_assessment_sheet);
              entity.id_organization = assessment.id_organization;
              entity.attempt_number = new int?(log.attempt_no);
              entity.status = "A";
              entity.updated_date_time = new DateTime?(DateTime.Now);
              List<tbl_assessment_audit> list = this.db.tbl_assessment_audit.Where<tbl_assessment_audit>((Expression<Func<tbl_assessment_audit, bool>>) (t => t.id_assessment == (int?) sheet.id_assesment && t.attempt_no == (int?) log.attempt_no && t.id_user == (int?) log.id_user)).ToList<tbl_assessment_audit>();
              int num2 = 0;
              int num3 = 0;
              double num4 = 0.0;
              foreach (tbl_assessment_audit tblAssessmentAudit in list)
              {
                tbl_assessment_question assessmentQuestion = this.db.tbl_assessment_question.Find(new object[1]
                {
                  (object) tblAssessmentAudit.id_assessment_question
                });
                if (assessmentQuestion != null)
                {
                  tbl_assessment_answer assessmentAnswer = this.db.tbl_assessment_answer.Find(new object[1]
                  {
                    (object) tblAssessmentAudit.id_assessment_answer
                  });
                  if (assessmentAnswer != null && assessmentQuestion.aq_answer == assessmentAnswer.id_assessment_answer.ToString())
                    ++num2;
                }
                ++num3;
              }
              if (num2 != 0)
                num4 = Math.Round((double) num2 / (double) list.Count * 100.0, 2);
              entity.total_question = new int?(num3);
              entity.right_answer_count = new int?(num2);
              entity.wrong_answer_count = new int?(num3 - num2);
              entity.result_in_percentage = new double?(num4);
              this.db.tbl_rs_type_qna.Add(entity);
              this.db.SaveChanges();
            }
          }
          else
          {
            idAssessmentTheme = sheet.id_assessment_theme;
            int num5 = 2;
            if ((idAssessmentTheme.GetValueOrDefault() == num5 ? (idAssessmentTheme.HasValue ? 1 : 0) : 0) == 0)
            {
              idAssessmentTheme = sheet.id_assessment_theme;
              int num6 = 3;
              if ((idAssessmentTheme.GetValueOrDefault() == num6 ? (idAssessmentTheme.HasValue ? 1 : 0) : 0) == 0)
              {
                idAssessmentTheme = sheet.id_assessment_theme;
                int num7 = 4;
                int num8 = idAssessmentTheme.GetValueOrDefault() == num7 ? (idAssessmentTheme.HasValue ? 1 : 0) : 0;
              }
            }
          }
        }
      }
      return (ActionResult) this.RedirectToAction("Index", "Dashboard");
    }

    public ActionResult program_scoring()
    {
      DbSet<tbl_assessmnt_log> tblAssessmntLog1 = this.db.tbl_assessmnt_log;
      Expression<Func<tbl_assessmnt_log, bool>> predicate = (Expression<Func<tbl_assessmnt_log, bool>>) (t => t.attempt_no == 1);
      foreach (tbl_assessmnt_log tblAssessmntLog2 in tblAssessmntLog1.Where<tbl_assessmnt_log>(predicate).ToList<tbl_assessmnt_log>())
      {
        tbl_assessmnt_log log = tblAssessmntLog2;
        int uid = log.id_user;
        int num1 = 1;
        string str = log.updated_date_time.Value.ToString("yyyy-MM-dd HH:mm:ss");
        int oid = Convert.ToInt32((object) log.id_organization);
        if (this.db.tbl_rs_type_qna.Where<tbl_rs_type_qna>((Expression<Func<tbl_rs_type_qna, bool>>) (t => t.id_assessment_log == (int?) log.id_assessmnt_log)).FirstOrDefault<tbl_rs_type_qna>() == null)
        {
          tbl_assessment tblAssessment = this.db.tbl_assessment.Find(new object[1]
          {
            (object) this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_assessment_sheet == log.id_assessment_sheet)).FirstOrDefault<tbl_assessment_sheet>().id_assesment
          });
          int aid = tblAssessment.id_assessment;
          sc_game_element_weightage elementWeightage = new sc_game_element_weightage();
          foreach (tbl_category tblCategory in this.db.tbl_category.SqlQuery("select * from tbl_category where id_organization=" + (object) oid + " and id_category in (select distinct id_category from tbl_assessment_categoty_mapping where id_assessment=" + (object) aid + " )").ToList<tbl_category>())
          {
            tbl_category item = tblCategory;
            if (new ProgramScoringModel().checkProgramComplition(item.ID_CATEGORY, uid, oid) == "0")
            {
              sc_program_content_summary programContentSummary = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == (int?) item.ID_CATEGORY && t.id_user == (int?) uid && t.id_organization == (int?) oid)).FirstOrDefault<sc_program_content_summary>();
              if (programContentSummary == null)
              {
                sc_program_content_summary entity = new sc_program_content_summary();
                entity.id_category = new int?(item.ID_CATEGORY);
                entity.id_organization = new int?(oid);
                entity.id_user = new int?(uid);
                int recordCount1 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) item.ID_CATEGORY ?? "");
                int num2 = 0;
                if (recordCount1 > 0)
                {
                  int recordCount2 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) item.ID_CATEGORY + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) uid + ")");
                  num2 = recordCount1 - recordCount2;
                }
                entity.totoal_count = new int?(recordCount1);
                entity.completed_count = new int?(num2);
                double num3 = 0.0;
                if (recordCount1 > 0 && num2 > 0)
                  num3 = (double) num2 / (double) recordCount1 * 100.0;
                entity.percentage = new double?(num3);
                entity.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(new int?(item.ID_CATEGORY), entity.percentage));
                entity.log_datetime = new DateTime?(DateTime.Now);
                entity.status = "A";
                entity.updated_date_time = new DateTime?(DateTime.Now);
                this.db.sc_program_content_summary.Add(entity);
                this.db.SaveChanges();
              }
              else
              {
                int recordCount3 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) item.ID_CATEGORY ?? "");
                int num4 = 0;
                if (recordCount3 > 0)
                {
                  int recordCount4 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) item.ID_CATEGORY + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) uid + " and updated_date_time<='" + str + "')");
                  num4 = recordCount3 - recordCount4;
                }
                programContentSummary.totoal_count = new int?(recordCount3);
                programContentSummary.completed_count = new int?(num4);
                double num5 = 0.0;
                if (recordCount3 > 0 && num4 > 0)
                  num5 = (double) num4 / (double) recordCount3 * 100.0;
                programContentSummary.percentage = new double?(num5);
                programContentSummary.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(new int?(item.ID_CATEGORY), programContentSummary.percentage));
                programContentSummary.log_datetime = new DateTime?(DateTime.Now);
                programContentSummary.status = "A";
                programContentSummary.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
              int? assessGroup = tblAssessment.assess_group;
              int num6 = 1;
              if ((assessGroup.GetValueOrDefault() == num6 ? (assessGroup.HasValue ? 1 : 0) : 0) != 0)
              {
                tbl_rs_type_qna tblRsTypeQna = this.db.tbl_rs_type_qna.SqlQuery("select * from tbl_rs_type_qna where id_assessment=" + (object) aid + " and id_user=" + (object) uid + " and id_organization=" + (object) oid + " and attempt_number=" + (object) num1 + " ").FirstOrDefault<tbl_rs_type_qna>();
                if (tblRsTypeQna != null)
                {
                  sc_program_assessment_scoring assessmentScoring = this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == (int?) aid && t.id_category == (int?) item.ID_CATEGORY && t.id_user == (int?) uid && t.id_organization == (int?) oid)).FirstOrDefault<sc_program_assessment_scoring>();
                  if (assessmentScoring == null)
                  {
                    this.db.sc_program_assessment_scoring.Add(new sc_program_assessment_scoring()
                    {
                      id_assessment = new int?(aid),
                      id_user = new int?(uid),
                      id_category = new int?(item.ID_CATEGORY),
                      id_organization = new int?(oid),
                      assessment_score = tblRsTypeQna.result_in_percentage,
                      assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(new int?(aid), new int?(item.ID_CATEGORY), tblRsTypeQna.result_in_percentage)),
                      attempt_number = new int?(num1),
                      log_datetime = new DateTime?(DateTime.Now),
                      status = "A",
                      updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                  }
                  else
                  {
                    assessmentScoring.assessment_score = tblRsTypeQna.result_in_percentage;
                    assessmentScoring.assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(new int?(aid), new int?(item.ID_CATEGORY), tblRsTypeQna.result_in_percentage));
                    assessmentScoring.attempt_number = new int?(num1);
                    assessmentScoring.log_datetime = new DateTime?(DateTime.Now);
                    assessmentScoring.status = "A";
                    assessmentScoring.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                  }
                }
              }
            }
          }
        }
      }
      return (ActionResult) this.RedirectToAction("Index", "Dashboard");
    }

    public ActionResult getProgramScoring()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      GameProgramSummary gameProgramSummary = new GameProgramSummary();
      return (ActionResult) this.View();
    }

    public string GamePhase2(tbl_game_creation game)
    {
      int orgid = Convert.ToInt32((object) game.id_organisation);
      List<tbl_game_process_path> list1 = this.db.tbl_game_process_path.Where<tbl_game_process_path>((Expression<Func<tbl_game_process_path, bool>>) (t => t.id_game == (int?) game.id_game)).ToList<tbl_game_process_path>();
      if (game.player_type == "1")
      {
        DbSet<tbl_game_group_association> groupAssociation1 = this.db.tbl_game_group_association;
        Expression<Func<tbl_game_group_association, bool>> predicate1 = (Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) game.id_game && t.id_organization == (int?) orgid && t.association_type == (int?) 1 && t.status == "A");
        foreach (tbl_game_group_association groupAssociation2 in groupAssociation1.Where<tbl_game_group_association>(predicate1).ToList<tbl_game_group_association>())
        {
          tbl_game_group_association item = groupAssociation2;
          int num1 = 1;
          foreach (tbl_game_process_path tblGameProcessPath in list1)
          {
            tbl_game_process_path path = tblGameProcessPath;
            int? nullable = path.element_type;
            int num2 = 1;
            if ((nullable.GetValueOrDefault() == num2 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            {
              double num3 = 0.0;
              double num4 = 0.0;
              foreach (tbl_assessment assesment in this.getAssesmentList(path.id_category, orgid))
              {
                tbl_assessment assessment = assesment;
                tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_assesment == assessment.id_assessment)).FirstOrDefault<tbl_assessment_sheet>();
                tbl_assessmnt_log log = this.db.tbl_assessmnt_log.SqlQuery("select * from tbl_assessmnt_log where id_user=" + (object) item.id_user + " and id_assessment_sheet =" + (object) sheet.id_assessment_sheet + " and attempt_no=1").FirstOrDefault<tbl_assessmnt_log>();
                int aid = assessment.id_assessment;
                if (log == null)
                {
                  if (this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == (int?) aid && t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.attempt_number == (int?) 1)).FirstOrDefault<sc_program_assessment_scoring>() == null)
                  {
                    this.db.sc_program_assessment_scoring.Add(new sc_program_assessment_scoring()
                    {
                      id_assessment = new int?(aid),
                      id_user = item.id_user,
                      id_category = path.id_category,
                      id_organization = new int?(orgid),
                      assessment_score = new double?(0.0),
                      assessment_wieghtage = new double?(0.0),
                      attempt_number = new int?(1),
                      log_datetime = new DateTime?(DateTime.Now),
                      status = "A",
                      updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                  }
                }
                else
                {
                  int oid = Convert.ToInt32((object) log.id_organization);
                  if (this.db.tbl_rs_type_qna.Where<tbl_rs_type_qna>((Expression<Func<tbl_rs_type_qna, bool>>) (t => t.id_assessment_log == (int?) log.id_assessmnt_log)).FirstOrDefault<tbl_rs_type_qna>() == null)
                  {
                    nullable = sheet.id_assessment_theme;
                    int num5 = 1;
                    if ((nullable.GetValueOrDefault() == num5 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                    {
                      if (this.db.tbl_rs_type_qna.Where<tbl_rs_type_qna>((Expression<Func<tbl_rs_type_qna, bool>>) (t => t.id_assessment == (int?) assessment.id_assessment && t.id_user == (int?) log.id_user && t.attempt_number == (int?) log.attempt_no)).FirstOrDefault<tbl_rs_type_qna>() == null)
                      {
                        tbl_rs_type_qna entity = new tbl_rs_type_qna();
                        entity.id_user = new int?(log.id_user);
                        entity.id_assessment = new int?(assessment.id_assessment);
                        entity.id_assessment_log = new int?(log.id_assessmnt_log);
                        entity.id_assessment_sheet = new int?(sheet.id_assessment_sheet);
                        entity.id_organization = assessment.id_organization;
                        entity.attempt_number = new int?(log.attempt_no);
                        entity.status = "A";
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        List<tbl_assessment_audit> list2 = this.db.tbl_assessment_audit.Where<tbl_assessment_audit>((Expression<Func<tbl_assessment_audit, bool>>) (t => t.id_assessment == (int?) sheet.id_assesment && t.attempt_no == (int?) log.attempt_no && t.id_user == (int?) log.id_user)).ToList<tbl_assessment_audit>();
                        int num6 = 0;
                        int num7 = 0;
                        double num8 = 0.0;
                        foreach (tbl_assessment_audit tblAssessmentAudit in list2)
                        {
                          tbl_assessment_question assessmentQuestion = this.db.tbl_assessment_question.Find(new object[1]
                          {
                            (object) tblAssessmentAudit.id_assessment_question
                          });
                          if (assessmentQuestion != null)
                          {
                            tbl_assessment_answer assessmentAnswer = this.db.tbl_assessment_answer.Find(new object[1]
                            {
                              (object) tblAssessmentAudit.id_assessment_answer
                            });
                            if (assessmentAnswer != null && assessmentQuestion.aq_answer == assessmentAnswer.id_assessment_answer.ToString())
                              ++num6;
                          }
                          ++num7;
                        }
                        if (num6 != 0)
                          num8 = Math.Round((double) num6 / (double) list2.Count * 100.0, 2);
                        entity.total_question = new int?(num7);
                        entity.right_answer_count = new int?(num6);
                        entity.wrong_answer_count = new int?(num7 - num6);
                        entity.result_in_percentage = new double?(num8);
                        this.db.tbl_rs_type_qna.Add(entity);
                        this.db.SaveChanges();
                      }
                    }
                    else
                    {
                      nullable = sheet.id_assessment_theme;
                      int num9 = 2;
                      if ((nullable.GetValueOrDefault() == num9 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                      {
                        nullable = sheet.id_assessment_theme;
                        int num10 = 3;
                        if ((nullable.GetValueOrDefault() == num10 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                        {
                          nullable = sheet.id_assessment_theme;
                          int num11 = 4;
                          int num12 = nullable.GetValueOrDefault() == num11 ? (nullable.HasValue ? 1 : 0) : 0;
                        }
                      }
                    }
                  }
                  nullable = assessment.assess_group;
                  int num13 = 1;
                  if ((nullable.GetValueOrDefault() == num13 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                  {
                    tbl_rs_type_qna tblRsTypeQna = this.db.tbl_rs_type_qna.SqlQuery("select * from tbl_rs_type_qna where id_assessment=" + (object) aid + " and id_user=" + (object) log.id_user + " and id_organization=" + (object) oid + " and attempt_number=" + (object) log.attempt_no + " ").FirstOrDefault<tbl_rs_type_qna>();
                    if (tblRsTypeQna != null)
                    {
                      sc_program_assessment_scoring assessmentScoring = this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == (int?) aid && t.id_category == path.id_category && t.id_user == (int?) log.id_user && t.id_organization == (int?) oid)).FirstOrDefault<sc_program_assessment_scoring>();
                      if (assessmentScoring == null)
                      {
                        this.db.sc_program_assessment_scoring.Add(new sc_program_assessment_scoring()
                        {
                          id_assessment = new int?(aid),
                          id_user = new int?(log.id_user),
                          id_category = path.id_category,
                          id_organization = new int?(oid),
                          assessment_score = tblRsTypeQna.result_in_percentage,
                          assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(new int?(aid), path.id_category, tblRsTypeQna.result_in_percentage)),
                          attempt_number = new int?(log.attempt_no),
                          log_datetime = new DateTime?(DateTime.Now),
                          status = "A",
                          updated_date_time = new DateTime?(DateTime.Now)
                        });
                        this.db.SaveChanges();
                      }
                      else
                      {
                        assessmentScoring.assessment_score = tblRsTypeQna.result_in_percentage;
                        assessmentScoring.assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(new int?(aid), path.id_category, tblRsTypeQna.result_in_percentage));
                        assessmentScoring.attempt_number = new int?(log.attempt_no);
                        assessmentScoring.log_datetime = new DateTime?(DateTime.Now);
                        assessmentScoring.status = "A";
                        assessmentScoring.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.SaveChanges();
                      }
                    }
                  }
                }
              }
              sc_program_content_summary programContentSummary1 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
              if (programContentSummary1 == null)
              {
                sc_program_content_summary entity = new sc_program_content_summary();
                entity.id_category = path.id_category;
                entity.id_organization = new int?(orgid);
                entity.id_user = item.id_user;
                int recordCount1 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                int num14 = 0;
                if (recordCount1 > 0)
                {
                  int recordCount2 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                  num14 = recordCount1 - recordCount2;
                }
                entity.totoal_count = new int?(recordCount1);
                entity.completed_count = new int?(num14);
                double num15 = 0.0;
                if (recordCount1 > 0 && num14 > 0)
                  num15 = (double) num14 / (double) recordCount1 * 100.0;
                entity.percentage = new double?(num15);
                entity.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, entity.percentage));
                entity.log_datetime = new DateTime?(DateTime.Now);
                entity.status = "A";
                entity.updated_date_time = new DateTime?(DateTime.Now);
                this.db.sc_program_content_summary.Add(entity);
                this.db.SaveChanges();
              }
              else
              {
                int recordCount3 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                int num16 = 0;
                if (recordCount3 > 0)
                {
                  int recordCount4 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                  num16 = recordCount3 - recordCount4;
                }
                programContentSummary1.totoal_count = new int?(recordCount3);
                programContentSummary1.completed_count = new int?(num16);
                double num17 = 0.0;
                if (recordCount3 > 0 && num16 > 0)
                  num17 = (double) num16 / (double) recordCount3 * 100.0;
                programContentSummary1.percentage = new double?(num17);
                programContentSummary1.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, programContentSummary1.percentage));
                programContentSummary1.status = "A";
                programContentSummary1.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
              double num18 = 0.0;
              sc_program_content_summary programContentSummary2 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
              if (programContentSummary2 != null)
              {
                Convert.ToDouble((object) programContentSummary2.percentage);
                num18 = Convert.ToDouble((object) programContentSummary2.content_weightage);
              }
              sc_game_element_weightage elementWeightage1 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 1)).FirstOrDefault<sc_game_element_weightage>();
              double weightageContent = new ProgramScoringModel().getPhase2WeightageContent(path.id_category, new double?(num18));
              if (elementWeightage1 == null)
              {
                this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                {
                  id_assessment = new int?(0),
                  id_category = path.id_category,
                  id_game = new int?(game.id_game),
                  id_kpi_master = new int?(0),
                  id_organization = new int?(orgid),
                  id_user = item.id_user,
                  element_score = new double?(num18),
                  element_type = new int?(1),
                  element_weightage = new double?(weightageContent),
                  status = "A",
                  updated_darte_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
              }
              else
              {
                elementWeightage1.element_score = new double?(num18);
                elementWeightage1.element_weightage = new double?(weightageContent);
                elementWeightage1.updated_darte_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
              double num19 = num4 + num18;
              double num20 = num3 + weightageContent;
              DbSet<tbl_kpi_program_scoring> kpiProgramScoring1 = this.db.tbl_kpi_program_scoring;
              Expression<Func<tbl_kpi_program_scoring, bool>> predicate2 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 2);
              foreach (tbl_kpi_program_scoring kpiProgramScoring2 in kpiProgramScoring1.Where<tbl_kpi_program_scoring>(predicate2).ToList<tbl_kpi_program_scoring>())
              {
                tbl_kpi_program_scoring kaitem = kpiProgramScoring2;
                ++num1;
                sc_program_assessment_scoring assessmentScoring = this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_assessment == kaitem.id_assessment && t.attempt_number == (int?) 1)).FirstOrDefault<sc_program_assessment_scoring>();
                double num21 = 0.0;
                if (assessmentScoring != null)
                {
                  Convert.ToDouble((object) assessmentScoring.assessment_score);
                  num21 = Convert.ToDouble((object) assessmentScoring.assessment_wieghtage);
                }
                else
                {
                  tbl_rs_type_qna tblRsTypeQna = this.db.tbl_rs_type_qna.SqlQuery("select * from tbl_rs_type_qna where id_assessment=" + (object) kaitem.id_assessment + " and id_user=" + (object) item.id_user + " and id_organization=" + (object) orgid + " and attempt_number=1 ").FirstOrDefault<tbl_rs_type_qna>();
                  if (tblRsTypeQna != null)
                  {
                    sc_program_assessment_scoring entity;
                    if (this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_assessment_scoring>() == null)
                    {
                      entity = new sc_program_assessment_scoring();
                      entity.id_assessment = kaitem.id_assessment;
                      entity.id_user = item.id_user;
                      entity.id_category = path.id_category;
                      entity.id_organization = new int?(orgid);
                      entity.assessment_score = tblRsTypeQna.result_in_percentage;
                      entity.assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(kaitem.id_assessment, path.id_category, tblRsTypeQna.result_in_percentage));
                      entity.attempt_number = new int?(1);
                      entity.log_datetime = new DateTime?(DateTime.Now);
                      entity.status = "A";
                      entity.updated_date_time = new DateTime?(DateTime.Now);
                      this.db.sc_program_assessment_scoring.Add(entity);
                      this.db.SaveChanges();
                    }
                    else
                    {
                      entity = new sc_program_assessment_scoring();
                      entity.id_assessment = kaitem.id_assessment;
                      entity.id_user = item.id_user;
                      entity.id_category = path.id_category;
                      entity.id_organization = new int?(orgid);
                      entity.assessment_score = new double?(0.0);
                      entity.assessment_wieghtage = new double?(0.0);
                      entity.attempt_number = new int?(1);
                      entity.log_datetime = new DateTime?(DateTime.Now);
                      entity.status = "A";
                      entity.updated_date_time = new DateTime?(DateTime.Now);
                      this.db.sc_program_assessment_scoring.Add(entity);
                      this.db.SaveChanges();
                    }
                    Convert.ToDouble((object) entity.assessment_score);
                    num21 = Convert.ToDouble((object) entity.assessment_wieghtage);
                  }
                }
                sc_game_element_weightage elementWeightage2 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 2)).FirstOrDefault<sc_game_element_weightage>();
                double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, kaitem.id_assessment, new double?(num21));
                if (elementWeightage2 == null)
                {
                  this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                  {
                    id_assessment = kaitem.id_assessment,
                    id_category = path.id_category,
                    id_game = new int?(game.id_game),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    id_kpi_master = new int?(0),
                    element_score = new double?(num21),
                    element_type = new int?(2),
                    element_weightage = new double?(weightageAssessment),
                    status = "A",
                    updated_darte_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  elementWeightage2.element_score = new double?(num21);
                  elementWeightage2.element_type = new int?(2);
                  elementWeightage2.element_weightage = new double?(weightageAssessment);
                  elementWeightage2.updated_darte_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                num19 += num21;
                num20 += weightageAssessment;
              }
              DbSet<tbl_kpi_program_scoring> kpiProgramScoring3 = this.db.tbl_kpi_program_scoring;
              Expression<Func<tbl_kpi_program_scoring, bool>> predicate3 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 3);
              foreach (tbl_kpi_program_scoring kpiProgramScoring4 in kpiProgramScoring3.Where<tbl_kpi_program_scoring>(predicate3).ToList<tbl_kpi_program_scoring>())
              {
                tbl_kpi_program_scoring nkpi = kpiProgramScoring4;
                ++num1;
                double num22 = 0.0;
                sc_program_kpi_weightage programKpiWeightage = this.db.sc_program_kpi_weightage.Where<sc_program_kpi_weightage>((Expression<Func<sc_program_kpi_weightage, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_kpi_master == nkpi.id_kpi_master)).FirstOrDefault<sc_program_kpi_weightage>();
                if (programKpiWeightage != null)
                {
                  Convert.ToDouble((object) programKpiWeightage.score);
                  num22 = Convert.ToDouble((object) programKpiWeightage.kpi_wieghtage);
                }
                else
                {
                  this.db.sc_program_kpi_weightage.Add(new sc_program_kpi_weightage()
                  {
                    id_category = path.id_category,
                    id_kpi_master = nkpi.id_kpi_master,
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    score = new double?(0.0),
                    kpi_wieghtage = new double?(0.0),
                    log_datetime = new DateTime?(DateTime.Now),
                    updated_date_time = new DateTime?(DateTime.Now),
                    status = "A"
                  });
                  this.db.SaveChanges();
                }
                sc_game_element_weightage elementWeightage3 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_kpi_master == nkpi.id_kpi_master && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 3)).FirstOrDefault<sc_game_element_weightage>();
                double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, nkpi.id_kpi_master, new double?(num22));
                if (elementWeightage3 == null)
                {
                  this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                  {
                    id_assessment = new int?(0),
                    id_category = path.id_category,
                    id_game = new int?(game.id_game),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    element_score = new double?(num22),
                    id_kpi_master = nkpi.id_kpi_master,
                    element_type = new int?(3),
                    element_weightage = new double?(weightageAssessment),
                    status = "A",
                    updated_darte_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  elementWeightage3.element_score = new double?(num22);
                  elementWeightage3.element_type = new int?(3);
                  elementWeightage3.element_weightage = new double?(weightageAssessment);
                  elementWeightage3.updated_darte_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                num19 += num22;
                num20 += weightageAssessment;
              }
              double phase3Weightage = new ProgramScoringModel().getPhase3Weightage(new int?(game.id_game), path.id_category, new double?(num20));
              sc_report_game_process_score gameProcessScore = this.db.sc_report_game_process_score.Where<sc_report_game_process_score>((Expression<Func<sc_report_game_process_score, bool>>) (t => t.id_user == item.id_user && t.id_game == (int?) game.id_game && t.id_category == path.id_category && t.element_type == path.element_type && t.sequence_number == path.sequence_number)).FirstOrDefault<sc_report_game_process_score>();
              if (gameProcessScore == null)
              {
                this.db.sc_report_game_process_score.Add(new sc_report_game_process_score()
                {
                  id_game = new int?(game.id_game),
                  id_kpi_master = new int?(0),
                  id_organization = new int?(orgid),
                  id_user = item.id_user,
                  id_assessment = new int?(0),
                  id_category = path.id_category,
                  element_type = path.element_type,
                  kpi_weightage = new double?(phase3Weightage),
                  score = new double?(num20),
                  sequence_number = path.sequence_number,
                  status = "A",
                  updated_date_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
              }
              else
              {
                gameProcessScore.kpi_weightage = new double?(phase3Weightage);
                gameProcessScore.score = new double?(num20);
                gameProcessScore.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
            }
            else
            {
              nullable = path.element_type;
              int num23 = 2;
              int num24 = nullable.GetValueOrDefault() == num23 ? (nullable.HasValue ? 1 : 0) : 0;
            }
          }
        }
      }
      else if (game.player_type == "2")
      {
        DbSet<tbl_game_group_association> groupAssociation3 = this.db.tbl_game_group_association;
        Expression<Func<tbl_game_group_association, bool>> predicate4 = (Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) game.id_game && t.id_organization == (int?) orgid && t.association_type == (int?) 2 && t.status == "A");
        foreach (tbl_game_group_association groupAssociation4 in groupAssociation3.Where<tbl_game_group_association>(predicate4).ToList<tbl_game_group_association>())
        {
          tbl_game_group_association group = groupAssociation4;
          DbSet<tbl_game_group_participatant> groupParticipatant1 = this.db.tbl_game_group_participatant;
          Expression<Func<tbl_game_group_participatant, bool>> predicate5 = (Expression<Func<tbl_game_group_participatant, bool>>) (t => t.id_game_group == group.id_game_group && t.id_organization == (int?) orgid && t.status == "A");
          foreach (tbl_game_group_participatant groupParticipatant2 in groupParticipatant1.Where<tbl_game_group_participatant>(predicate5).ToList<tbl_game_group_participatant>())
          {
            tbl_game_group_participatant item = groupParticipatant2;
            int num25 = 1;
            foreach (tbl_game_process_path tblGameProcessPath in list1)
            {
              tbl_game_process_path path = tblGameProcessPath;
              int? nullable = path.element_type;
              int num26 = 1;
              if ((nullable.GetValueOrDefault() == num26 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
              {
                foreach (tbl_assessment assesment in this.getAssesmentList(path.id_category, orgid))
                {
                  tbl_assessment assessment = assesment;
                  tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_assesment == assessment.id_assessment)).FirstOrDefault<tbl_assessment_sheet>();
                  tbl_assessmnt_log log = this.db.tbl_assessmnt_log.SqlQuery("select * from tbl_assessmnt_log where id_user=" + (object) item.id_user + " and id_assessment_sheet =" + (object) sheet.id_assessment_sheet + " and attempt_no=1").FirstOrDefault<tbl_assessmnt_log>();
                  int aid = assessment.id_assessment;
                  if (log == null)
                  {
                    if (this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == (int?) aid && t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.attempt_number == (int?) 1)).FirstOrDefault<sc_program_assessment_scoring>() == null)
                    {
                      this.db.sc_program_assessment_scoring.Add(new sc_program_assessment_scoring()
                      {
                        id_assessment = new int?(aid),
                        id_user = item.id_user,
                        id_category = path.id_category,
                        id_organization = new int?(orgid),
                        assessment_score = new double?(0.0),
                        assessment_wieghtage = new double?(0.0),
                        attempt_number = new int?(1),
                        log_datetime = new DateTime?(DateTime.Now),
                        status = "A",
                        updated_date_time = new DateTime?(DateTime.Now)
                      });
                      this.db.SaveChanges();
                    }
                  }
                  else
                  {
                    int oid = Convert.ToInt32((object) log.id_organization);
                    if (this.db.tbl_rs_type_qna.Where<tbl_rs_type_qna>((Expression<Func<tbl_rs_type_qna, bool>>) (t => t.id_assessment_log == (int?) log.id_assessmnt_log)).FirstOrDefault<tbl_rs_type_qna>() == null)
                    {
                      nullable = sheet.id_assessment_theme;
                      int num27 = 1;
                      if ((nullable.GetValueOrDefault() == num27 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                      {
                        if (this.db.tbl_rs_type_qna.Where<tbl_rs_type_qna>((Expression<Func<tbl_rs_type_qna, bool>>) (t => t.id_assessment == (int?) assessment.id_assessment && t.id_user == (int?) log.id_user && t.attempt_number == (int?) log.attempt_no)).FirstOrDefault<tbl_rs_type_qna>() == null)
                        {
                          tbl_rs_type_qna entity = new tbl_rs_type_qna();
                          entity.id_user = new int?(log.id_user);
                          entity.id_assessment = new int?(assessment.id_assessment);
                          entity.id_assessment_log = new int?(log.id_assessmnt_log);
                          entity.id_assessment_sheet = new int?(sheet.id_assessment_sheet);
                          entity.id_organization = assessment.id_organization;
                          entity.attempt_number = new int?(log.attempt_no);
                          entity.status = "A";
                          entity.updated_date_time = new DateTime?(DateTime.Now);
                          List<tbl_assessment_audit> list3 = this.db.tbl_assessment_audit.Where<tbl_assessment_audit>((Expression<Func<tbl_assessment_audit, bool>>) (t => t.id_assessment == (int?) sheet.id_assesment && t.attempt_no == (int?) log.attempt_no && t.id_user == (int?) log.id_user)).ToList<tbl_assessment_audit>();
                          int num28 = 0;
                          int num29 = 0;
                          double num30 = 0.0;
                          foreach (tbl_assessment_audit tblAssessmentAudit in list3)
                          {
                            tbl_assessment_question assessmentQuestion = this.db.tbl_assessment_question.Find(new object[1]
                            {
                              (object) tblAssessmentAudit.id_assessment_question
                            });
                            if (assessmentQuestion != null)
                            {
                              tbl_assessment_answer assessmentAnswer = this.db.tbl_assessment_answer.Find(new object[1]
                              {
                                (object) tblAssessmentAudit.id_assessment_answer
                              });
                              if (assessmentAnswer != null && assessmentQuestion.aq_answer == assessmentAnswer.id_assessment_answer.ToString())
                                ++num28;
                            }
                            ++num29;
                          }
                          if (num28 != 0)
                            num30 = Math.Round((double) num28 / (double) list3.Count * 100.0, 2);
                          entity.total_question = new int?(num29);
                          entity.right_answer_count = new int?(num28);
                          entity.wrong_answer_count = new int?(num29 - num28);
                          entity.result_in_percentage = new double?(num30);
                          this.db.tbl_rs_type_qna.Add(entity);
                          this.db.SaveChanges();
                        }
                      }
                      else
                      {
                        nullable = sheet.id_assessment_theme;
                        int num31 = 2;
                        if ((nullable.GetValueOrDefault() == num31 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                        {
                          nullable = sheet.id_assessment_theme;
                          int num32 = 3;
                          if ((nullable.GetValueOrDefault() == num32 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                          {
                            nullable = sheet.id_assessment_theme;
                            int num33 = 4;
                            int num34 = nullable.GetValueOrDefault() == num33 ? (nullable.HasValue ? 1 : 0) : 0;
                          }
                        }
                      }
                    }
                    nullable = assessment.assess_group;
                    int num35 = 1;
                    if ((nullable.GetValueOrDefault() == num35 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                    {
                      tbl_rs_type_qna tblRsTypeQna = this.db.tbl_rs_type_qna.SqlQuery("select * from tbl_rs_type_qna where id_assessment=" + (object) aid + " and id_user=" + (object) log.id_user + " and id_organization=" + (object) oid + " and attempt_number=" + (object) log.attempt_no + " ").FirstOrDefault<tbl_rs_type_qna>();
                      if (tblRsTypeQna != null)
                      {
                        sc_program_assessment_scoring assessmentScoring = this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == (int?) aid && t.id_category == path.id_category && t.id_user == (int?) log.id_user && t.id_organization == (int?) oid)).FirstOrDefault<sc_program_assessment_scoring>();
                        if (assessmentScoring == null)
                        {
                          this.db.sc_program_assessment_scoring.Add(new sc_program_assessment_scoring()
                          {
                            id_assessment = new int?(aid),
                            id_user = new int?(log.id_user),
                            id_category = path.id_category,
                            id_organization = new int?(oid),
                            assessment_score = tblRsTypeQna.result_in_percentage,
                            assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(new int?(aid), path.id_category, tblRsTypeQna.result_in_percentage)),
                            attempt_number = new int?(log.attempt_no),
                            log_datetime = new DateTime?(DateTime.Now),
                            status = "A",
                            updated_date_time = new DateTime?(DateTime.Now)
                          });
                          this.db.SaveChanges();
                        }
                        else
                        {
                          assessmentScoring.assessment_score = tblRsTypeQna.result_in_percentage;
                          assessmentScoring.assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(new int?(aid), path.id_category, tblRsTypeQna.result_in_percentage));
                          assessmentScoring.attempt_number = new int?(log.attempt_no);
                          assessmentScoring.log_datetime = new DateTime?(DateTime.Now);
                          assessmentScoring.status = "A";
                          assessmentScoring.updated_date_time = new DateTime?(DateTime.Now);
                          this.db.SaveChanges();
                        }
                      }
                    }
                  }
                }
                sc_program_content_summary programContentSummary3 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
                if (programContentSummary3 == null)
                {
                  sc_program_content_summary entity = new sc_program_content_summary();
                  entity.id_category = path.id_category;
                  entity.id_organization = new int?(orgid);
                  entity.id_user = item.id_user;
                  int recordCount5 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                  int num36 = 0;
                  if (recordCount5 > 0)
                  {
                    int recordCount6 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                    num36 = recordCount5 - recordCount6;
                  }
                  entity.totoal_count = new int?(recordCount5);
                  entity.completed_count = new int?(num36);
                  double num37 = 0.0;
                  if (recordCount5 > 0 && num36 > 0)
                    num37 = (double) num36 / (double) recordCount5 * 100.0;
                  entity.percentage = new double?(num37);
                  entity.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, entity.percentage));
                  entity.log_datetime = new DateTime?(DateTime.Now);
                  entity.status = "A";
                  entity.updated_date_time = new DateTime?(DateTime.Now);
                  this.db.sc_program_content_summary.Add(entity);
                  this.db.SaveChanges();
                }
                else
                {
                  int recordCount7 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                  int num38 = 0;
                  if (recordCount7 > 0)
                  {
                    int recordCount8 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                    num38 = recordCount7 - recordCount8;
                  }
                  programContentSummary3.totoal_count = new int?(recordCount7);
                  programContentSummary3.completed_count = new int?(num38);
                  double num39 = 0.0;
                  if (recordCount7 > 0 && num38 > 0)
                    num39 = (double) num38 / (double) recordCount7 * 100.0;
                  programContentSummary3.percentage = new double?(num39);
                  programContentSummary3.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, programContentSummary3.percentage));
                  programContentSummary3.status = "A";
                  programContentSummary3.updated_date_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                double num40 = 0.0;
                double num41 = 0.0;
                double num42 = 0.0;
                sc_program_content_summary programContentSummary4 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
                if (programContentSummary4 != null)
                {
                  Convert.ToDouble((object) programContentSummary4.percentage);
                  num42 = Convert.ToDouble((object) programContentSummary4.content_weightage);
                }
                sc_game_element_weightage elementWeightage4 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 1)).FirstOrDefault<sc_game_element_weightage>();
                double weightageContent = new ProgramScoringModel().getPhase2WeightageContent(path.id_category, new double?(num42));
                if (elementWeightage4 == null)
                {
                  this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                  {
                    id_assessment = new int?(0),
                    id_category = path.id_category,
                    id_game = new int?(game.id_game),
                    id_kpi_master = new int?(0),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    element_score = new double?(num42),
                    element_type = new int?(1),
                    element_weightage = new double?(weightageContent),
                    status = "A",
                    updated_darte_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  elementWeightage4.element_score = new double?(num42);
                  elementWeightage4.element_weightage = new double?(weightageContent);
                  elementWeightage4.updated_darte_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                double num43 = num41 + num42;
                double num44 = num40 + weightageContent;
                DbSet<tbl_kpi_program_scoring> kpiProgramScoring5 = this.db.tbl_kpi_program_scoring;
                Expression<Func<tbl_kpi_program_scoring, bool>> predicate6 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 2);
                foreach (tbl_kpi_program_scoring kpiProgramScoring6 in kpiProgramScoring5.Where<tbl_kpi_program_scoring>(predicate6).ToList<tbl_kpi_program_scoring>())
                {
                  tbl_kpi_program_scoring kaitem = kpiProgramScoring6;
                  ++num25;
                  sc_program_assessment_scoring assessmentScoring = this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_assessment == kaitem.id_assessment && t.attempt_number == (int?) 1)).FirstOrDefault<sc_program_assessment_scoring>();
                  double num45 = 0.0;
                  if (assessmentScoring != null)
                  {
                    Convert.ToDouble((object) assessmentScoring.assessment_score);
                    num45 = Convert.ToDouble((object) assessmentScoring.assessment_wieghtage);
                  }
                  else
                  {
                    tbl_rs_type_qna tblRsTypeQna = this.db.tbl_rs_type_qna.SqlQuery("select * from tbl_rs_type_qna where id_assessment=" + (object) kaitem.id_assessment + " and id_user=" + (object) item.id_user + " and id_organization=" + (object) orgid + " and attempt_number=1 ").FirstOrDefault<tbl_rs_type_qna>();
                    if (tblRsTypeQna != null)
                    {
                      sc_program_assessment_scoring entity;
                      if (this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_assessment_scoring>() == null)
                      {
                        entity = new sc_program_assessment_scoring();
                        entity.id_assessment = kaitem.id_assessment;
                        entity.id_user = item.id_user;
                        entity.id_category = path.id_category;
                        entity.id_organization = new int?(orgid);
                        entity.assessment_score = tblRsTypeQna.result_in_percentage;
                        entity.assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(kaitem.id_assessment, path.id_category, tblRsTypeQna.result_in_percentage));
                        entity.attempt_number = new int?(1);
                        entity.log_datetime = new DateTime?(DateTime.Now);
                        entity.status = "A";
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.sc_program_assessment_scoring.Add(entity);
                        this.db.SaveChanges();
                      }
                      else
                      {
                        entity = new sc_program_assessment_scoring();
                        entity.id_assessment = kaitem.id_assessment;
                        entity.id_user = item.id_user;
                        entity.id_category = path.id_category;
                        entity.id_organization = new int?(orgid);
                        entity.assessment_score = new double?(0.0);
                        entity.assessment_wieghtage = new double?(0.0);
                        entity.attempt_number = new int?(1);
                        entity.log_datetime = new DateTime?(DateTime.Now);
                        entity.status = "A";
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.sc_program_assessment_scoring.Add(entity);
                        this.db.SaveChanges();
                      }
                      Convert.ToDouble((object) entity.assessment_score);
                      num45 = Convert.ToDouble((object) entity.assessment_wieghtage);
                    }
                  }
                  sc_game_element_weightage elementWeightage5 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 2)).FirstOrDefault<sc_game_element_weightage>();
                  double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, kaitem.id_assessment, new double?(num45));
                  if (elementWeightage5 == null)
                  {
                    this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                    {
                      id_assessment = kaitem.id_assessment,
                      id_category = path.id_category,
                      id_game = new int?(game.id_game),
                      id_organization = new int?(orgid),
                      id_user = item.id_user,
                      id_kpi_master = new int?(0),
                      element_score = new double?(num45),
                      element_type = new int?(2),
                      element_weightage = new double?(weightageAssessment),
                      status = "A",
                      updated_darte_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                  }
                  else
                  {
                    elementWeightage5.element_score = new double?(num45);
                    elementWeightage5.element_type = new int?(2);
                    elementWeightage5.element_weightage = new double?(weightageAssessment);
                    elementWeightage5.updated_darte_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                  }
                  num43 += num45;
                  num44 += weightageAssessment;
                }
                DbSet<tbl_kpi_program_scoring> kpiProgramScoring7 = this.db.tbl_kpi_program_scoring;
                Expression<Func<tbl_kpi_program_scoring, bool>> predicate7 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 3);
                foreach (tbl_kpi_program_scoring kpiProgramScoring8 in kpiProgramScoring7.Where<tbl_kpi_program_scoring>(predicate7).ToList<tbl_kpi_program_scoring>())
                {
                  tbl_kpi_program_scoring nkpi = kpiProgramScoring8;
                  ++num25;
                  double num46 = 0.0;
                  sc_program_kpi_weightage programKpiWeightage = this.db.sc_program_kpi_weightage.Where<sc_program_kpi_weightage>((Expression<Func<sc_program_kpi_weightage, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_kpi_master == nkpi.id_kpi_master)).FirstOrDefault<sc_program_kpi_weightage>();
                  if (programKpiWeightage != null)
                  {
                    Convert.ToDouble((object) programKpiWeightage.score);
                    num46 = Convert.ToDouble((object) programKpiWeightage.kpi_wieghtage);
                  }
                  else
                  {
                    this.db.sc_program_kpi_weightage.Add(new sc_program_kpi_weightage()
                    {
                      id_category = path.id_category,
                      id_kpi_master = nkpi.id_kpi_master,
                      id_organization = new int?(orgid),
                      id_user = item.id_user,
                      score = new double?(0.0),
                      kpi_wieghtage = new double?(0.0),
                      log_datetime = new DateTime?(DateTime.Now),
                      updated_date_time = new DateTime?(DateTime.Now),
                      status = "A"
                    });
                    this.db.SaveChanges();
                  }
                  sc_game_element_weightage elementWeightage6 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_kpi_master == nkpi.id_kpi_master && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 3)).FirstOrDefault<sc_game_element_weightage>();
                  double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, nkpi.id_kpi_master, new double?(num46));
                  if (elementWeightage6 == null)
                  {
                    this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                    {
                      id_assessment = new int?(0),
                      id_category = path.id_category,
                      id_game = new int?(game.id_game),
                      id_organization = new int?(orgid),
                      id_user = item.id_user,
                      element_score = new double?(num46),
                      id_kpi_master = nkpi.id_kpi_master,
                      element_type = new int?(3),
                      element_weightage = new double?(weightageAssessment),
                      status = "A",
                      updated_darte_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                  }
                  else
                  {
                    elementWeightage6.element_score = new double?(num46);
                    elementWeightage6.element_type = new int?(3);
                    elementWeightage6.element_weightage = new double?(weightageAssessment);
                    elementWeightage6.updated_darte_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                  }
                  num43 += num46;
                  num44 += weightageAssessment;
                }
                double phase3Weightage = new ProgramScoringModel().getPhase3Weightage(new int?(game.id_game), path.id_category, new double?(num44));
                sc_report_game_process_score gameProcessScore = this.db.sc_report_game_process_score.Where<sc_report_game_process_score>((Expression<Func<sc_report_game_process_score, bool>>) (t => t.id_user == item.id_user && t.id_game == (int?) game.id_game && t.id_category == path.id_category && t.element_type == path.element_type && t.sequence_number == path.sequence_number)).FirstOrDefault<sc_report_game_process_score>();
                if (gameProcessScore == null)
                {
                  this.db.sc_report_game_process_score.Add(new sc_report_game_process_score()
                  {
                    id_game = new int?(game.id_game),
                    id_kpi_master = new int?(0),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    id_assessment = new int?(0),
                    id_category = path.id_category,
                    element_type = path.element_type,
                    kpi_weightage = new double?(phase3Weightage),
                    score = new double?(num44),
                    sequence_number = path.sequence_number,
                    status = "A",
                    updated_date_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  gameProcessScore.kpi_weightage = new double?(phase3Weightage);
                  gameProcessScore.score = new double?(num44);
                  gameProcessScore.updated_date_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
              }
              else
              {
                nullable = path.element_type;
                int num47 = 2;
                int num48 = nullable.GetValueOrDefault() == num47 ? (nullable.HasValue ? 1 : 0) : 0;
              }
            }
          }
        }
      }
      return "1";
    }

    public List<tbl_assessment> getAssesmentList(int? CID, int OID)
    {
      DateTime now = DateTime.Now;
      return this.db.tbl_assessment.SqlQuery("" + " SELECT p.* FROM tbl_assessment p left join  tbl_assessment_sheet a LEFT JOIN tbl_assessment_categoty_mapping b ON a.id_assessment_sheet = b.id_assessment_sheet " + " on p.id_assessment=a.id_assesment WHERE a.status = 'A' and a.id_organization=" + (object) OID + " AND b.id_category = " + (object) CID + " ").ToList<tbl_assessment>();
    }

    public ActionResult ProgramResultDownload()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32 = Convert.ToInt32(content.id_ORGANIZATION);
      Convert.ToInt32(content.ID_USER);
      this.ViewData["master"] = (object) this.db.tbl_game_creation.SqlQuery("SELECT * FROM tbl_game_creation where id_organisation= " + (object) int32 + " and status='A'").ToList<tbl_game_creation>();
      return (ActionResult) this.View();
    }

    public string getGameProgramScore(string gid)
    {
      int gids = Convert.ToInt32(gid);
      tbl_game_creation game = this.db.tbl_game_creation.Where<tbl_game_creation>((Expression<Func<tbl_game_creation, bool>>) (t => t.id_game == gids)).FirstOrDefault<tbl_game_creation>();
      string gameProgramScore = "";
      if (game != null)
      {
        string str = gameProgramScore + "<table id=\"report-table\" class=\"gpsdisplay table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\">" + "<thead><tr>" + "<th>Program</th>" + "<th>User ID</th>" + "<th>EMPLOYEE ID</th>" + "<th>User Name</th>" + "<th>Score</th>" + "<th>Percentile</th>" + "</tr></thead>" + "<tbody>";
        IQueryable<tbl_game_process_path> source = this.db.tbl_game_process_path.Where<tbl_game_process_path>((Expression<Func<tbl_game_process_path, bool>>) (t => t.id_game == (int?) game.id_game));
        Expression<Func<tbl_game_process_path, int?>> keySelector = (Expression<Func<tbl_game_process_path, int?>>) (t => t.sequence_number);
        foreach (tbl_game_process_path tblGameProcessPath in source.OrderBy<tbl_game_process_path, int?>(keySelector).ToList<tbl_game_process_path>())
        {
          List<ProgramPercentile> programPercentile1 = new ProgramScoringModel().getProgramPercentile("" + " SELECT  u.EMPLOYEEID,u.USERID, CONCAT(p.FIRSTNAME, ' ', p.LASTNAME) UNAME, c.id_user AS id_user, b.CATEGORYNAME AS CATEGORYNAME, a.game_title AS game_title, " + " TRUNCATE(AVG(c.score), 2) AS IndavgScore " + " FROM sc_report_game_process_score c  LEFT JOIN tbl_game_creation a ON a.id_game = c.id_game AND a.status = 'A'  LEFT JOIN tbl_category b ON b.ID_CATEGORY = c.id_category  LEFT JOIN tbl_user u  LEFT JOIN " + " tbl_profile p ON p.ID_USER = u.id_user ON u.id_user = c.id_user " + " WHERE u.id_organization=" + (object) game.id_organisation + " and c.id_category = " + (object) tblGameProcessPath.id_category + " and c.element_type = 1 GROUP BY c.id_category , c.id_user ");
          double num1 = programPercentile1.Max<ProgramPercentile>((Func<ProgramPercentile, double>) (t => t.IndavgScore));
          double num2 = programPercentile1.Min<ProgramPercentile>((Func<ProgramPercentile, double>) (t => t.IndavgScore));
          double num3 = num1 - num2;
          foreach (ProgramPercentile programPercentile2 in programPercentile1)
          {
            str += "<tr>";
            str = str + "<td>" + programPercentile2.CATEGORYNAME + "</td>";
            str = str + "<td>" + programPercentile2.USERID + " </td>";
            str = str + "<td>" + programPercentile2.EMPLOYEEID + " </td>";
            str = str + "<td>" + programPercentile2.UNAME + " </td>";
            str = str + "<td>" + (object) programPercentile2.IndavgScore + "</td>";
            double num4 = (programPercentile2.IndavgScore - num2) / num3 * 100.0;
            str = str + "<td>" + (object) Math.Round(num4, 0) + "</td>";
            str += "</tr>";
          }
        }
        gameProgramScore = str + "</tbody></table>";
      }
      return gameProgramScore;
    }

    public string setGameScore(string gid)
    {
      int gids = Convert.ToInt32(gid);
      tbl_game_creation game = this.db.tbl_game_creation.Where<tbl_game_creation>((Expression<Func<tbl_game_creation, bool>>) (t => t.id_game == gids)).FirstOrDefault<tbl_game_creation>();
      int orgid = Convert.ToInt32((object) game.id_organisation);
      List<tbl_game_process_path> list = this.db.tbl_game_process_path.Where<tbl_game_process_path>((Expression<Func<tbl_game_process_path, bool>>) (t => t.id_game == (int?) game.id_game)).ToList<tbl_game_process_path>();
      if (game.player_type == "1")
      {
        DbSet<tbl_game_group_association> groupAssociation1 = this.db.tbl_game_group_association;
        Expression<Func<tbl_game_group_association, bool>> predicate1 = (Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) game.id_game && t.id_organization == (int?) orgid && t.association_type == (int?) 1 && t.status == "A");
        foreach (tbl_game_group_association groupAssociation2 in groupAssociation1.Where<tbl_game_group_association>(predicate1).ToList<tbl_game_group_association>())
        {
          tbl_game_group_association item = groupAssociation2;
          int num1 = 1;
          foreach (tbl_game_process_path tblGameProcessPath in list)
          {
            tbl_game_process_path path = tblGameProcessPath;
            int? elementType = path.element_type;
            int num2 = 1;
            if ((elementType.GetValueOrDefault() == num2 ? (elementType.HasValue ? 1 : 0) : 0) != 0)
            {
              double num3 = 0.0;
              double num4 = 0.0;
              sc_program_content_summary programContentSummary1 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
              if (programContentSummary1 == null)
              {
                sc_program_content_summary entity = new sc_program_content_summary();
                entity.id_category = path.id_category;
                entity.id_organization = new int?(orgid);
                entity.id_user = item.id_user;
                int recordCount1 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                int num5 = 0;
                if (recordCount1 > 0)
                {
                  int recordCount2 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                  num5 = recordCount1 - recordCount2;
                }
                entity.totoal_count = new int?(recordCount1);
                entity.completed_count = new int?(num5);
                double num6 = 0.0;
                if (recordCount1 > 0 && num5 > 0)
                  num6 = (double) num5 / (double) recordCount1 * 100.0;
                entity.percentage = new double?(num6);
                entity.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, entity.percentage));
                entity.log_datetime = new DateTime?(DateTime.Now);
                entity.status = "A";
                entity.updated_date_time = new DateTime?(DateTime.Now);
                this.db.sc_program_content_summary.Add(entity);
                this.db.SaveChanges();
              }
              else
              {
                int recordCount3 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                int num7 = 0;
                if (recordCount3 > 0)
                {
                  int recordCount4 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                  num7 = recordCount3 - recordCount4;
                }
                programContentSummary1.totoal_count = new int?(recordCount3);
                programContentSummary1.completed_count = new int?(num7);
                double num8 = 0.0;
                if (recordCount3 > 0 && num7 > 0)
                  num8 = (double) num7 / (double) recordCount3 * 100.0;
                programContentSummary1.percentage = new double?(num8);
                programContentSummary1.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, programContentSummary1.percentage));
                programContentSummary1.status = "A";
                programContentSummary1.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
              double num9 = 0.0;
              sc_program_content_summary programContentSummary2 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
              if (programContentSummary2 != null)
              {
                Convert.ToDouble((object) programContentSummary2.percentage);
                num9 = Convert.ToDouble((object) programContentSummary2.content_weightage);
              }
              sc_game_element_weightage elementWeightage1 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 1)).FirstOrDefault<sc_game_element_weightage>();
              double weightageContent = new ProgramScoringModel().getPhase2WeightageContent(path.id_category, new double?(num9));
              if (elementWeightage1 == null)
              {
                this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                {
                  id_assessment = new int?(0),
                  id_category = path.id_category,
                  id_game = new int?(game.id_game),
                  id_kpi_master = new int?(0),
                  id_organization = new int?(orgid),
                  id_user = item.id_user,
                  element_score = new double?(num9),
                  element_type = new int?(1),
                  element_weightage = new double?(weightageContent),
                  status = "A",
                  updated_darte_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
              }
              else
              {
                elementWeightage1.element_score = new double?(num9);
                elementWeightage1.element_weightage = new double?(weightageContent);
                elementWeightage1.updated_darte_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
              double num10 = num4 + num9;
              double num11 = num3 + weightageContent;
              DbSet<tbl_kpi_program_scoring> kpiProgramScoring1 = this.db.tbl_kpi_program_scoring;
              Expression<Func<tbl_kpi_program_scoring, bool>> predicate2 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 2);
              foreach (tbl_kpi_program_scoring kpiProgramScoring2 in kpiProgramScoring1.Where<tbl_kpi_program_scoring>(predicate2).ToList<tbl_kpi_program_scoring>())
              {
                tbl_kpi_program_scoring kaitem = kpiProgramScoring2;
                ++num1;
                sc_program_assessment_scoring assessmentScoring = this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_assessment == kaitem.id_assessment && t.attempt_number == (int?) 1)).FirstOrDefault<sc_program_assessment_scoring>();
                double num12 = 0.0;
                if (assessmentScoring != null)
                {
                  Convert.ToDouble((object) assessmentScoring.assessment_score);
                  num12 = new ProgramScoringModel().getAssessmentWeightage(kaitem.id_assessment, path.id_category, assessmentScoring.assessment_score);
                  assessmentScoring.assessment_wieghtage = new double?(num12);
                  this.db.SaveChanges();
                }
                else
                {
                  tbl_rs_type_qna tblRsTypeQna = this.db.tbl_rs_type_qna.SqlQuery("select * from tbl_rs_type_qna where id_assessment=" + (object) kaitem.id_assessment + " and id_user=" + (object) item.id_user + " and id_organization=" + (object) orgid + " and attempt_number=1 ").FirstOrDefault<tbl_rs_type_qna>();
                  if (tblRsTypeQna != null)
                  {
                    sc_program_assessment_scoring entity;
                    if (this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_assessment_scoring>() == null)
                    {
                      entity = new sc_program_assessment_scoring();
                      entity.id_assessment = kaitem.id_assessment;
                      entity.id_user = item.id_user;
                      entity.id_category = path.id_category;
                      entity.id_organization = new int?(orgid);
                      entity.assessment_score = tblRsTypeQna.result_in_percentage;
                      entity.assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(kaitem.id_assessment, path.id_category, tblRsTypeQna.result_in_percentage));
                      entity.attempt_number = new int?(1);
                      entity.log_datetime = new DateTime?(DateTime.Now);
                      entity.status = "A";
                      entity.updated_date_time = new DateTime?(DateTime.Now);
                      this.db.sc_program_assessment_scoring.Add(entity);
                      this.db.SaveChanges();
                    }
                    else
                    {
                      entity = new sc_program_assessment_scoring();
                      entity.id_assessment = kaitem.id_assessment;
                      entity.id_user = item.id_user;
                      entity.id_category = path.id_category;
                      entity.id_organization = new int?(orgid);
                      entity.assessment_score = new double?(0.0);
                      entity.assessment_wieghtage = new double?(0.0);
                      entity.attempt_number = new int?(1);
                      entity.log_datetime = new DateTime?(DateTime.Now);
                      entity.status = "A";
                      entity.updated_date_time = new DateTime?(DateTime.Now);
                      this.db.sc_program_assessment_scoring.Add(entity);
                      this.db.SaveChanges();
                    }
                    Convert.ToDouble((object) entity.assessment_score);
                    num12 = Convert.ToDouble((object) entity.assessment_wieghtage);
                  }
                }
                sc_game_element_weightage elementWeightage2 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 2)).FirstOrDefault<sc_game_element_weightage>();
                double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, kaitem.id_assessment, new double?(num12));
                if (elementWeightage2 == null)
                {
                  this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                  {
                    id_assessment = kaitem.id_assessment,
                    id_category = path.id_category,
                    id_game = new int?(game.id_game),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    id_kpi_master = new int?(0),
                    element_score = new double?(num12),
                    element_type = new int?(2),
                    element_weightage = new double?(weightageAssessment),
                    status = "A",
                    updated_darte_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  elementWeightage2.element_score = new double?(num12);
                  elementWeightage2.element_type = new int?(2);
                  elementWeightage2.element_weightage = new double?(weightageAssessment);
                  elementWeightage2.updated_darte_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                num10 += num12;
                num11 += weightageAssessment;
              }
              DbSet<tbl_kpi_program_scoring> kpiProgramScoring3 = this.db.tbl_kpi_program_scoring;
              Expression<Func<tbl_kpi_program_scoring, bool>> predicate3 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 3);
              foreach (tbl_kpi_program_scoring kpiProgramScoring4 in kpiProgramScoring3.Where<tbl_kpi_program_scoring>(predicate3).ToList<tbl_kpi_program_scoring>())
              {
                tbl_kpi_program_scoring nkpi = kpiProgramScoring4;
                ++num1;
                double num13 = 0.0;
                sc_program_kpi_weightage programKpiWeightage = this.db.sc_program_kpi_weightage.Where<sc_program_kpi_weightage>((Expression<Func<sc_program_kpi_weightage, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_kpi_master == nkpi.id_kpi_master)).FirstOrDefault<sc_program_kpi_weightage>();
                if (programKpiWeightage != null)
                {
                  Convert.ToDouble((object) programKpiWeightage.score);
                  num13 = Convert.ToDouble((object) programKpiWeightage.kpi_wieghtage);
                }
                else
                {
                  this.db.sc_program_kpi_weightage.Add(new sc_program_kpi_weightage()
                  {
                    id_category = path.id_category,
                    id_kpi_master = nkpi.id_kpi_master,
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    score = new double?(0.0),
                    kpi_wieghtage = new double?(0.0),
                    log_datetime = new DateTime?(DateTime.Now),
                    updated_date_time = new DateTime?(DateTime.Now),
                    status = "A"
                  });
                  this.db.SaveChanges();
                }
                sc_game_element_weightage elementWeightage3 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_kpi_master == nkpi.id_kpi_master && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 3)).FirstOrDefault<sc_game_element_weightage>();
                double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, nkpi.id_kpi_master, new double?(num13));
                if (elementWeightage3 == null)
                {
                  this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                  {
                    id_assessment = new int?(0),
                    id_category = path.id_category,
                    id_game = new int?(game.id_game),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    element_score = new double?(num13),
                    id_kpi_master = nkpi.id_kpi_master,
                    element_type = new int?(3),
                    element_weightage = new double?(weightageAssessment),
                    status = "A",
                    updated_darte_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  elementWeightage3.element_score = new double?(num13);
                  elementWeightage3.element_type = new int?(3);
                  elementWeightage3.element_weightage = new double?(weightageAssessment);
                  elementWeightage3.updated_darte_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                num10 += num13;
                num11 += weightageAssessment;
              }
              double phase3Weightage = new ProgramScoringModel().getPhase3Weightage(new int?(game.id_game), path.id_category, new double?(num11));
              sc_report_game_process_score gameProcessScore = this.db.sc_report_game_process_score.Where<sc_report_game_process_score>((Expression<Func<sc_report_game_process_score, bool>>) (t => t.id_user == item.id_user && t.id_game == (int?) game.id_game && t.id_category == path.id_category && t.element_type == path.element_type && t.sequence_number == path.sequence_number)).FirstOrDefault<sc_report_game_process_score>();
              if (gameProcessScore == null)
              {
                this.db.sc_report_game_process_score.Add(new sc_report_game_process_score()
                {
                  id_game = new int?(game.id_game),
                  id_kpi_master = new int?(0),
                  id_organization = new int?(orgid),
                  id_user = item.id_user,
                  id_assessment = new int?(0),
                  id_category = path.id_category,
                  element_type = path.element_type,
                  kpi_weightage = new double?(phase3Weightage),
                  score = new double?(num11),
                  sequence_number = path.sequence_number,
                  status = "A",
                  updated_date_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
              }
              else
              {
                gameProcessScore.kpi_weightage = new double?(phase3Weightage);
                gameProcessScore.score = new double?(num11);
                gameProcessScore.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
            }
            else
            {
              elementType = path.element_type;
              int num14 = 2;
              int num15 = elementType.GetValueOrDefault() == num14 ? (elementType.HasValue ? 1 : 0) : 0;
            }
          }
        }
      }
      else if (game.player_type == "2")
      {
        DbSet<tbl_game_group_association> groupAssociation3 = this.db.tbl_game_group_association;
        Expression<Func<tbl_game_group_association, bool>> predicate4 = (Expression<Func<tbl_game_group_association, bool>>) (t => t.id_game == (int?) game.id_game && t.id_organization == (int?) orgid && t.association_type == (int?) 2 && t.status == "A");
        foreach (tbl_game_group_association groupAssociation4 in groupAssociation3.Where<tbl_game_group_association>(predicate4).ToList<tbl_game_group_association>())
        {
          tbl_game_group_association group = groupAssociation4;
          DbSet<tbl_game_group_participatant> groupParticipatant1 = this.db.tbl_game_group_participatant;
          Expression<Func<tbl_game_group_participatant, bool>> predicate5 = (Expression<Func<tbl_game_group_participatant, bool>>) (t => t.id_game_group == group.id_game_group && t.id_organization == (int?) orgid && t.status == "A");
          foreach (tbl_game_group_participatant groupParticipatant2 in groupParticipatant1.Where<tbl_game_group_participatant>(predicate5).ToList<tbl_game_group_participatant>())
          {
            tbl_game_group_participatant item = groupParticipatant2;
            int num16 = 1;
            foreach (tbl_game_process_path tblGameProcessPath in list)
            {
              tbl_game_process_path path = tblGameProcessPath;
              int? elementType = path.element_type;
              int num17 = 1;
              if ((elementType.GetValueOrDefault() == num17 ? (elementType.HasValue ? 1 : 0) : 0) != 0)
              {
                sc_program_content_summary programContentSummary3 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
                if (programContentSummary3 == null)
                {
                  sc_program_content_summary entity = new sc_program_content_summary();
                  entity.id_category = path.id_category;
                  entity.id_organization = new int?(orgid);
                  entity.id_user = item.id_user;
                  int recordCount5 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                  int num18 = 0;
                  if (recordCount5 > 0)
                  {
                    int recordCount6 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                    num18 = recordCount5 - recordCount6;
                  }
                  entity.totoal_count = new int?(recordCount5);
                  entity.completed_count = new int?(num18);
                  double num19 = 0.0;
                  if (recordCount5 > 0 && num18 > 0)
                    num19 = (double) num18 / (double) recordCount5 * 100.0;
                  entity.percentage = new double?(num19);
                  entity.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, entity.percentage));
                  entity.log_datetime = new DateTime?(DateTime.Now);
                  entity.status = "A";
                  entity.updated_date_time = new DateTime?(DateTime.Now);
                  this.db.sc_program_content_summary.Add(entity);
                  this.db.SaveChanges();
                }
                else
                {
                  int recordCount7 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category ?? "");
                  int num20 = 0;
                  if (recordCount7 > 0)
                  {
                    int recordCount8 = new ContentReportModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object) path.id_category + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object) item.id_user + " )");
                    num20 = recordCount7 - recordCount8;
                  }
                  programContentSummary3.totoal_count = new int?(recordCount7);
                  programContentSummary3.completed_count = new int?(num20);
                  double num21 = 0.0;
                  if (recordCount7 > 0 && num20 > 0)
                    num21 = (double) num20 / (double) recordCount7 * 100.0;
                  programContentSummary3.percentage = new double?(num21);
                  programContentSummary3.content_weightage = new double?(new ProgramScoringModel().getContentWeightage(path.id_category, programContentSummary3.percentage));
                  programContentSummary3.status = "A";
                  programContentSummary3.updated_date_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                double num22 = 0.0;
                double num23 = 0.0;
                double num24 = 0.0;
                sc_program_content_summary programContentSummary4 = this.db.sc_program_content_summary.Where<sc_program_content_summary>((Expression<Func<sc_program_content_summary, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_content_summary>();
                if (programContentSummary4 != null)
                {
                  Convert.ToDouble((object) programContentSummary4.percentage);
                  num24 = Convert.ToDouble((object) programContentSummary4.content_weightage);
                }
                sc_game_element_weightage elementWeightage4 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 1)).FirstOrDefault<sc_game_element_weightage>();
                double weightageContent = new ProgramScoringModel().getPhase2WeightageContent(path.id_category, new double?(num24));
                if (elementWeightage4 == null)
                {
                  this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                  {
                    id_assessment = new int?(0),
                    id_category = path.id_category,
                    id_game = new int?(game.id_game),
                    id_kpi_master = new int?(0),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    element_score = new double?(num24),
                    element_type = new int?(1),
                    element_weightage = new double?(weightageContent),
                    status = "A",
                    updated_darte_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  elementWeightage4.element_score = new double?(num24);
                  elementWeightage4.element_weightage = new double?(weightageContent);
                  elementWeightage4.updated_darte_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
                double num25 = num23 + num24;
                double num26 = num22 + weightageContent;
                DbSet<tbl_kpi_program_scoring> kpiProgramScoring5 = this.db.tbl_kpi_program_scoring;
                Expression<Func<tbl_kpi_program_scoring, bool>> predicate6 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 2);
                foreach (tbl_kpi_program_scoring kpiProgramScoring6 in kpiProgramScoring5.Where<tbl_kpi_program_scoring>(predicate6).ToList<tbl_kpi_program_scoring>())
                {
                  tbl_kpi_program_scoring kaitem = kpiProgramScoring6;
                  ++num16;
                  sc_program_assessment_scoring assessmentScoring = this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_assessment == kaitem.id_assessment && t.attempt_number == (int?) 1)).FirstOrDefault<sc_program_assessment_scoring>();
                  double num27 = 0.0;
                  if (assessmentScoring != null)
                  {
                    Convert.ToDouble((object) assessmentScoring.assessment_score);
                    num27 = new ProgramScoringModel().getAssessmentWeightage(kaitem.id_assessment, path.id_category, assessmentScoring.assessment_score);
                    assessmentScoring.assessment_wieghtage = new double?(num27);
                    this.db.SaveChanges();
                  }
                  else
                  {
                    tbl_rs_type_qna tblRsTypeQna = this.db.tbl_rs_type_qna.SqlQuery("select * from tbl_rs_type_qna where id_assessment=" + (object) kaitem.id_assessment + " and id_user=" + (object) item.id_user + " and id_organization=" + (object) orgid + " and attempt_number=1 ").FirstOrDefault<tbl_rs_type_qna>();
                    if (tblRsTypeQna != null)
                    {
                      sc_program_assessment_scoring entity;
                      if (this.db.sc_program_assessment_scoring.Where<sc_program_assessment_scoring>((Expression<Func<sc_program_assessment_scoring, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid)).FirstOrDefault<sc_program_assessment_scoring>() == null)
                      {
                        entity = new sc_program_assessment_scoring();
                        entity.id_assessment = kaitem.id_assessment;
                        entity.id_user = item.id_user;
                        entity.id_category = path.id_category;
                        entity.id_organization = new int?(orgid);
                        entity.assessment_score = tblRsTypeQna.result_in_percentage;
                        entity.assessment_wieghtage = new double?(new ProgramScoringModel().getAssessmentWeightage(kaitem.id_assessment, path.id_category, tblRsTypeQna.result_in_percentage));
                        entity.attempt_number = new int?(1);
                        entity.log_datetime = new DateTime?(DateTime.Now);
                        entity.status = "A";
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.sc_program_assessment_scoring.Add(entity);
                        this.db.SaveChanges();
                      }
                      else
                      {
                        entity = new sc_program_assessment_scoring();
                        entity.id_assessment = kaitem.id_assessment;
                        entity.id_user = item.id_user;
                        entity.id_category = path.id_category;
                        entity.id_organization = new int?(orgid);
                        entity.assessment_score = new double?(0.0);
                        entity.assessment_wieghtage = new double?(0.0);
                        entity.attempt_number = new int?(1);
                        entity.log_datetime = new DateTime?(DateTime.Now);
                        entity.status = "A";
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.sc_program_assessment_scoring.Add(entity);
                        this.db.SaveChanges();
                      }
                      Convert.ToDouble((object) entity.assessment_score);
                      num27 = Convert.ToDouble((object) entity.assessment_wieghtage);
                    }
                  }
                  sc_game_element_weightage elementWeightage5 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == kaitem.id_assessment && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 2)).FirstOrDefault<sc_game_element_weightage>();
                  double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, kaitem.id_assessment, new double?(num27));
                  if (elementWeightage5 == null)
                  {
                    this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                    {
                      id_assessment = kaitem.id_assessment,
                      id_category = path.id_category,
                      id_game = new int?(game.id_game),
                      id_organization = new int?(orgid),
                      id_user = item.id_user,
                      id_kpi_master = new int?(0),
                      element_score = new double?(num27),
                      element_type = new int?(2),
                      element_weightage = new double?(weightageAssessment),
                      status = "A",
                      updated_darte_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                  }
                  else
                  {
                    elementWeightage5.element_score = new double?(num27);
                    elementWeightage5.element_type = new int?(2);
                    elementWeightage5.element_weightage = new double?(weightageAssessment);
                    elementWeightage5.updated_darte_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                  }
                  num25 += num27;
                  num26 += weightageAssessment;
                }
                DbSet<tbl_kpi_program_scoring> kpiProgramScoring7 = this.db.tbl_kpi_program_scoring;
                Expression<Func<tbl_kpi_program_scoring, bool>> predicate7 = (Expression<Func<tbl_kpi_program_scoring, bool>>) (t => t.id_category == path.id_category && t.id_organization == (int?) orgid && t.kpi_type == (int?) 3);
                foreach (tbl_kpi_program_scoring kpiProgramScoring8 in kpiProgramScoring7.Where<tbl_kpi_program_scoring>(predicate7).ToList<tbl_kpi_program_scoring>())
                {
                  tbl_kpi_program_scoring nkpi = kpiProgramScoring8;
                  ++num16;
                  double num28 = 0.0;
                  sc_program_kpi_weightage programKpiWeightage = this.db.sc_program_kpi_weightage.Where<sc_program_kpi_weightage>((Expression<Func<sc_program_kpi_weightage, bool>>) (t => t.id_category == path.id_category && t.id_user == item.id_user && t.id_organization == (int?) orgid && t.id_kpi_master == nkpi.id_kpi_master)).FirstOrDefault<sc_program_kpi_weightage>();
                  if (programKpiWeightage != null)
                  {
                    Convert.ToDouble((object) programKpiWeightage.score);
                    num28 = Convert.ToDouble((object) programKpiWeightage.kpi_wieghtage);
                  }
                  else
                  {
                    this.db.sc_program_kpi_weightage.Add(new sc_program_kpi_weightage()
                    {
                      id_category = path.id_category,
                      id_kpi_master = nkpi.id_kpi_master,
                      id_organization = new int?(orgid),
                      id_user = item.id_user,
                      score = new double?(0.0),
                      kpi_wieghtage = new double?(0.0),
                      log_datetime = new DateTime?(DateTime.Now),
                      updated_date_time = new DateTime?(DateTime.Now),
                      status = "A"
                    });
                    this.db.SaveChanges();
                  }
                  sc_game_element_weightage elementWeightage6 = this.db.sc_game_element_weightage.Where<sc_game_element_weightage>((Expression<Func<sc_game_element_weightage, bool>>) (t => t.id_assessment == (int?) 0 && t.id_kpi_master == nkpi.id_kpi_master && t.id_category == path.id_category && t.id_game == (int?) game.id_game && t.id_user == item.id_user && t.element_type == (int?) 3)).FirstOrDefault<sc_game_element_weightage>();
                  double weightageAssessment = new ProgramScoringModel().getPhase2WeightageAssessment(path.id_category, nkpi.id_kpi_master, new double?(num28));
                  if (elementWeightage6 == null)
                  {
                    this.db.sc_game_element_weightage.Add(new sc_game_element_weightage()
                    {
                      id_assessment = new int?(0),
                      id_category = path.id_category,
                      id_game = new int?(game.id_game),
                      id_organization = new int?(orgid),
                      id_user = item.id_user,
                      element_score = new double?(num28),
                      id_kpi_master = nkpi.id_kpi_master,
                      element_type = new int?(3),
                      element_weightage = new double?(weightageAssessment),
                      status = "A",
                      updated_darte_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                  }
                  else
                  {
                    elementWeightage6.element_score = new double?(num28);
                    elementWeightage6.element_type = new int?(3);
                    elementWeightage6.element_weightage = new double?(weightageAssessment);
                    elementWeightage6.updated_darte_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                  }
                  num25 += num28;
                  num26 += weightageAssessment;
                }
                double phase3Weightage = new ProgramScoringModel().getPhase3Weightage(new int?(game.id_game), path.id_category, new double?(num26));
                sc_report_game_process_score gameProcessScore = this.db.sc_report_game_process_score.Where<sc_report_game_process_score>((Expression<Func<sc_report_game_process_score, bool>>) (t => t.id_user == item.id_user && t.id_game == (int?) game.id_game && t.id_category == path.id_category && t.element_type == path.element_type && t.sequence_number == path.sequence_number)).FirstOrDefault<sc_report_game_process_score>();
                if (gameProcessScore == null)
                {
                  this.db.sc_report_game_process_score.Add(new sc_report_game_process_score()
                  {
                    id_game = new int?(game.id_game),
                    id_kpi_master = new int?(0),
                    id_organization = new int?(orgid),
                    id_user = item.id_user,
                    id_assessment = new int?(0),
                    id_category = path.id_category,
                    element_type = path.element_type,
                    kpi_weightage = new double?(phase3Weightage),
                    score = new double?(num26),
                    sequence_number = path.sequence_number,
                    status = "A",
                    updated_date_time = new DateTime?(DateTime.Now)
                  });
                  this.db.SaveChanges();
                }
                else
                {
                  gameProcessScore.kpi_weightage = new double?(phase3Weightage);
                  gameProcessScore.score = new double?(num26);
                  gameProcessScore.updated_date_time = new DateTime?(DateTime.Now);
                  this.db.SaveChanges();
                }
              }
              else
              {
                elementType = path.element_type;
                int num29 = 2;
                int num30 = elementType.GetValueOrDefault() == num29 ? (elementType.HasValue ? 1 : 0) : 0;
              }
            }
          }
        }
      }
      return "1";
    }
  }
}
