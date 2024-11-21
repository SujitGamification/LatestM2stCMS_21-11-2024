// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.AdministratorController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  [UserFilter]
  public class AdministratorController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

        public class UserIdStatusModel
        {
            public string UserId { get; set; }
            public string NewStatus { get; set; }
        }


        public ActionResult Index() => (ActionResult) this.View();

    public ActionResult content_association()
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<tbl_organization> list1 = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      List<tbl_category> list2 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object) list1[0].ID_ORGANIZATION + " and category_type in (0,1) AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
      this.ViewData["orgs"] = (object) list1;
      this.ViewData["orgid"] = (object) int32;
      this.ViewData["CategoryList"] = (object) list2;
      return (ActionResult) this.View();
    }

    public ActionResult assessment_association()
    {
      int orgid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<AssessmentSheet> assessmentSheetList = new List<AssessmentSheet>();
      List<tbl_organization> list = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      DbSet<tbl_assessment_sheet> tblAssessmentSheet1 = this.db.tbl_assessment_sheet;
      Expression<Func<tbl_assessment_sheet, bool>> predicate = (Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_organization == (int?) orgid);
      foreach (tbl_assessment_sheet tblAssessmentSheet2 in tblAssessmentSheet1.Where<tbl_assessment_sheet>(predicate).ToList<tbl_assessment_sheet>())
      {
        tbl_assessment_sheet item = tblAssessmentSheet2;
        tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => t.id_assessment == item.id_assesment && t.status == "A")).FirstOrDefault<tbl_assessment>();
        if (tblAssessment != null)
          assessmentSheetList.Add(new AssessmentSheet()
          {
            Assessment = tblAssessment,
            Sheet = item
          });
      }
      this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => t.id_organization == (int?) orgid && t.status != "D")).OrderBy<tbl_assessment, string>((Expression<Func<tbl_assessment, string>>) (t => t.assessment_title)).ToList<tbl_assessment>();
      this.ViewData["orgs"] = (object) list;
      this.ViewData["orgid"] = (object) orgid;
      this.ViewData["AssessmentList"] = (object) assessmentSheetList;
      return (ActionResult) this.View();
    }

    public string getAssessmentAssociationList(string osid, string odid)
    {
      int orgid = Convert.ToInt32(osid);
      int desid = Convert.ToInt32(odid);
      List<AssessmentSheet> assessmentSheetList = new List<AssessmentSheet>();
      this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      DbSet<tbl_assessment_sheet> tblAssessmentSheet1 = this.db.tbl_assessment_sheet;
      Expression<Func<tbl_assessment_sheet, bool>> predicate1 = (Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_organization == (int?) orgid);
      foreach (tbl_assessment_sheet tblAssessmentSheet2 in tblAssessmentSheet1.Where<tbl_assessment_sheet>(predicate1).ToList<tbl_assessment_sheet>())
      {
        tbl_assessment_sheet item = tblAssessmentSheet2;
        tbl_assessment asMnt = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => t.id_assessment == item.id_assesment && t.status != "D")).FirstOrDefault<tbl_assessment>();
        AssessmentSheet assessmentSheet = new AssessmentSheet();
        assessmentSheet.Assessment = asMnt;
        assessmentSheet.Sheet = item;
        DbSet<tbl_assessment_sheet> tblAssessmentSheet3 = this.db.tbl_assessment_sheet;
        Expression<Func<tbl_assessment_sheet, bool>> predicate2 = (Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_organization == (int?) desid && t.id_assesment == asMnt.id_assessment);
        assessmentSheet.FLAG = tblAssessmentSheet3.Where<tbl_assessment_sheet>(predicate2).FirstOrDefault<tbl_assessment_sheet>() != null;
        assessmentSheetList.Add(assessmentSheet);
      }
      string str = "";
      tbl_organization tblOrganization = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_ORGANIZATION == orgid)).FirstOrDefault<tbl_organization>();
      foreach (AssessmentSheet assessmentSheet in assessmentSheetList)
      {
        str += "<tr>";
        str = str + "<td>" + assessmentSheet.Assessment.assessment_title + "</td>";
        str = str + "<td>" + tblOrganization.ORGANIZATION_NAME + "</td>";
        if (assessmentSheet.FLAG)
          str = str + "<td><a id=\"tag_" + (object) assessmentSheet.Assessment.id_assessment + "\" href=\"#\" onclick=\"removeAssessmentAssociation('" + (object) assessmentSheet.Assessment.id_assessment + "','" + odid + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a></td>";
        else
          str = str + "<td><a id=\"link_" + (object) assessmentSheet.Assessment.id_assessment + "\" href=\"#\" onclick=\"addAssessmentToOrganization('" + (object) assessmentSheet.Assessment.id_assessment + "','" + odid + "')\"><i class=\"glyphicon glyphicon-plus\"></i></a></td>";
        str += "</tr>";
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th>Assessment </th><th>Organization</th><th></th></thead>" + "<tbody>" + str + "</tbody></table>";
    }

    public ActionResult content_right_association()
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<tbl_organization> list1 = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).ToList<tbl_organization>();
      List<tbl_category> list2 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object) list1[0].ID_ORGANIZATION + " and category_type in (0,1) AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
      this.ViewData["orgs"] = (object) list1;
      this.ViewData["CategoryList"] = (object) list2;
      return (ActionResult) this.View();
    }

    public string getCategoryFromOrganization(string id)
    {
      string fromOrganization = "";
      List<tbl_category> list = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + id + " AND CATEGORY_TYPE in (0,1) AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
      if (list.Count > 0)
      {
        foreach (tbl_category tblCategory in list)
          fromOrganization = fromOrganization + "<option value=\"" + (object) tblCategory.ID_CATEGORY + "\">" + tblCategory.CATEGORYNAME + "</option>";
      }
      else
        fromOrganization = "<option disabled selected>No Category Present</option>";
      return fromOrganization;
    }

    public string getContentOrgReport(string id, string oid, string pattern, string tcid)
    {
      if (id == "")
        id = "0";
      int int32_1 = Convert.ToInt32(id);
      int tcids = Convert.ToInt32(tcid);
      int int32_2 = Convert.ToInt32(oid);
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(int32_2);
      pattern = pattern == null ? "" : pattern.Replace("'", "''");
      string str1 = "select * from tbl_content where true  ";
      string sql;
      if (int32_1 > 0)
        sql = str1 + " and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object) int32_1 + ") ";
      else
        sql = str1 + "and id_content in (select id_content from tbl_content_organization_mapping where id_organization=" + (object) int32_2 + ") ";
      if (!string.IsNullOrEmpty(pattern))
        sql = sql + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
      List<tbl_content> list = this.db.tbl_content.SqlQuery(sql).OrderBy<tbl_content, string>((Func<tbl_content, string>) (t => t.CONTENT_QUESTION)).ToList<tbl_content>();
      string str2 = "";
      foreach (tbl_content tblContent in list)
      {
        tbl_content item = tblContent;
        tbl_content_right_association rightAssociation = this.db.tbl_content_right_association.Where<tbl_content_right_association>((Expression<Func<tbl_content_right_association, bool>>) (t => t.id_organization == tcids && t.id_content == item.ID_CONTENT)).FirstOrDefault<tbl_content_right_association>();
        str2 = str2 + "<tr><td><a target=\"_blank\" href=" + this.Url.Action("LoadContent", "contentDashboard", (object) new
        {
          id = item.ID_CONTENT
        }) + ">" + item.CONTENT_QUESTION + "(" + (object) item.ID_CONTENT + ")</a></td>";
        str2 += "</td><td> ";
        if (rightAssociation == null)
        {
          str2 = str2 + "  <a href=\"#\" onclick=\"addContentToOrganization(" + (object) item.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-plus\"></i></a>";
        }
        else
        {
          str2 += "Added";
          str2 = str2 + " | <a id=\"link_" + (object) item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeContentToOrganization('" + (object) item.ID_CONTENT + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
        }
        str2 += "</td></tr>";
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"70%\">Content</th><th width=\"30%\">Click (<i class=\"glyphicon glyphicon-plus\"></i>) to Add </th></tr></thead>" + "<tbody>" + str2 + "</tbody></table>";
    }

    public string addContentToOrganization(string oid, string cid)
    {
      string organization = "0";
      int cids = Convert.ToInt32(cid);
      int oids = Convert.ToInt32(oid);
      tbl_content tblContent = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == cids && t.STATUS == "A")).FirstOrDefault<tbl_content>();
      if (tblContent != null)
      {
        if (this.db.tbl_content_right_association.Where<tbl_content_right_association>((Expression<Func<tbl_content_right_association, bool>>) (t => t.id_organization == oids && t.id_content == cids && t.status == "R")).FirstOrDefault<tbl_content_right_association>() == null)
        {
          tbl_content_right_association entity = new tbl_content_right_association();
          entity.id_organization = oids;
          entity.id_content = cids;
          entity.status = "R";
          entity.updated_date_time = new DateTime?(DateTime.Now);
          DateTime? nullable = new DateTime?(DateTime.Now);
          nullable.Value.AddMonths(12);
          entity.content_expiry_date = nullable;
          entity.real_content_id = new int?(tblContent.ID_CONTENT);
          entity.real_organization_id = new int?(tblContent.CONTENT_OWNER);
          entity.transfered_date = new DateTime?(DateTime.Now);
          this.db.tbl_content_right_association.Add(entity);
          this.db.SaveChanges();
          organization = "1";
        }
      }
      return organization;
    }

    public string removeContentToOrganization(string oid, string cid)
    {
      string organization = "0";
      int cids = Convert.ToInt32(cid);
      int oids = Convert.ToInt32(oid);
      if (this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == cids && t.STATUS == "A")).FirstOrDefault<tbl_content>() != null)
      {
        tbl_content_right_association entity = this.db.tbl_content_right_association.Where<tbl_content_right_association>((Expression<Func<tbl_content_right_association, bool>>) (t => t.id_organization == oids && t.id_content == cids && t.status == "R")).FirstOrDefault<tbl_content_right_association>();
        if (entity != null)
        {
          this.db.tbl_content_right_association.Remove(entity);
          this.db.SaveChanges();
          organization = "1";
        }
      }
      return organization;
    }

    public string addAssessmentToOrganization(string oid, string cid)
    {
      int cids = Convert.ToInt32(cid);
      int oids = Convert.ToInt32(oid);
      tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => t.id_assessment == cids && t.status == "A")).FirstOrDefault<tbl_assessment>();
      string organization = "";
      if (tblAssessment != null)
      {
        if (this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_organization == (int?) oids && t.id_assesment == cids)).FirstOrDefault<tbl_assessment_sheet>() == null)
        {
          this.db.tbl_assessment_sheet.Add(new tbl_assessment_sheet()
          {
            id_organization = new int?(oids),
            id_assesment = cids,
            status = "R",
            updated_date_time = new DateTime?(DateTime.Now),
            id_assessment_theme = tblAssessment.assess_group
          });
          this.db.SaveChanges();
          organization = "1";
        }
        else
          organization = "0";
      }
      return organization;
    }

    public string removeAssessmentAssociation(string oid, string cid)
    {
      int cids = Convert.ToInt32(cid);
      int oids = Convert.ToInt32(oid);
      tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => t.id_assessment == cids && t.status == "A")).FirstOrDefault<tbl_assessment>();
      string str = "";
      if (tblAssessment != null)
      {
        tbl_assessment_sheet entity = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>) (t => t.id_organization == (int?) oids && t.id_assesment == cids)).FirstOrDefault<tbl_assessment_sheet>();
        if (entity == null)
        {
          str = "0";
        }
        else
        {
          this.db.tbl_assessment_sheet.Remove(entity);
          this.db.SaveChanges();
          str = "1";
        }
      }
      return str;
    }

    public string TransferContentToOrganization(string oid, string cid) => "";

    public ActionResult data_association()
    {
      int orgid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<ContentOwner> contentOwnerList = new List<ContentOwner>();
      List<tbl_content> tblContentList = new List<tbl_content>();
      List<tbl_content_right_association> list1 = this.db.tbl_content_right_association.Where<tbl_content_right_association>((Expression<Func<tbl_content_right_association, bool>>) (t => t.id_organization == orgid)).ToList<tbl_content_right_association>();
      List<tbl_category> list2 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object) orgid + " AND CATEGORY_TYPE in (0,1) AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
      tbl_organization organisation = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).FirstOrDefault<tbl_organization>();
      lazy_response lazyResponse = new lazy_response();
      List<string> stringList = new List<string>();
      foreach (tbl_content_right_association rightAssociation in list1)
      {
        tbl_content_right_association item = rightAssociation;
        tbl_content tblContent = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == item.id_content && t.STATUS == "A")).FirstOrDefault<tbl_content>();
        if (tblContent != null)
        {
          ContentOwner contentOwner = new ContentOwner();
          contentOwner.Content = tblContent;
          contentOwner.Organization = organisation;
          if (item.status == "R")
            contentOwner.flag = "R";
          else if (item.status == "O")
            contentOwner.flag = "O";
          List<tbl_content_organization_mapping> list3 = this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>) (t => t.id_organization == organisation.ID_ORGANIZATION && t.id_content == item.id_content)).ToList<tbl_content_organization_mapping>();
          if (list3.Count == 0)
          {
            contentOwner.used = 0;
            contentOwnerList.Add(contentOwner);
          }
          else
            contentOwner.used = list3.Count;
        }
      }
      this.ViewData["organization"] = (object) organisation;
      this.ViewData["category"] = (object) list2;
      this.ViewData["content_owner"] = (object) contentOwnerList;
      return (ActionResult) this.View();
    }

    public ActionResult data_display_association()
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Associated Content List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int orgid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<ContentAssociation> contentAssociationList = new List<ContentAssociation>();
      List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object) orgid + " and category_type in (0,1) AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
      List<tbl_content> list2 = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select distinct id_content from tbl_content_organization_mapping where id_organization=" + (object) orgid + ")  ").OrderBy<tbl_content, string>((Func<tbl_content, string>) (t => t.CONTENT_QUESTION)).ToList<tbl_content>();
      tbl_organization tblOrganization = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).FirstOrDefault<tbl_organization>();
      List<tbl_content> tblContentList = new List<tbl_content>();
      this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>) (t => t.id_organization == orgid)).ToList<tbl_content_organization_mapping>();
      foreach (tbl_content tblContent in list2)
      {
        tbl_content item = tblContent;
        if (this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == item.ID_CONTENT && t.STATUS == "A")).FirstOrDefault<tbl_content>() != null)
        {
          List<tbl_category> list3 = this.db.tbl_category.SqlQuery("select * from tbl_category where id_category in (select distinct id_category from tbl_content_organization_mapping where id_content=" + (object) item.ID_CONTENT + " AND id_organization=" + (object) orgid + ")").OrderBy<tbl_category, string>((Func<tbl_category, string>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
          contentAssociationList.Add(new ContentAssociation()
          {
            Content = item,
            Category = list3
          });
        }
      }
      this.ViewData["organization"] = (object) tblOrganization;
      this.ViewData["category"] = (object) list1;
      this.ViewData["contentList"] = (object) contentAssociationList;
      return (ActionResult) this.View();
    }

    public string getDataResponse(string oid, string pattern, string cid)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int orgid = Convert.ToInt32(oid);
      int cids = Convert.ToInt32(cid);
      string str1 = "";
      string str2 = "";
      if (!string.IsNullOrEmpty(pattern))
        str2 = " and id_content in (select id_content from tbl_content where CONTENT_QUESTION like '%" + pattern + "%' and status='A') ";
      List<tbl_content_right_association> list = this.db.tbl_content_right_association.SqlQuery("select * from tbl_content_right_association where id_organization=" + (object) orgid + " " + str2 + " ").ToList<tbl_content_right_association>();
      this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).OrderBy<tbl_category, string>((Expression<Func<tbl_category, string>>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
      this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).FirstOrDefault<tbl_organization>();
      lazy_response lazyResponse = new lazy_response();
      List<List<string>> stringListList = new List<List<string>>();
      foreach (tbl_content_right_association rightAssociation in list)
      {
        tbl_content_right_association item = rightAssociation;
        tbl_content con = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == item.id_content && t.STATUS == "A")).FirstOrDefault<tbl_content>();
        if (con != null)
        {
          str1 += "<tr>";
          str1 = str1 + "<td>" + con.CONTENT_QUESTION + "(" + (object) item.id_content + ")</td>";
          if (item.status == "R")
            str1 += "<td>Has Right to Use</td>";
          else if (item.status == "O")
            str1 += "<td>Own Content</td>";
          if (this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>) (t => t.id_organization == orgid && t.id_category == cids && t.id_content == con.ID_CONTENT)).FirstOrDefault<tbl_content_organization_mapping>() == null)
            str1 = str1 + "<td><a  id=\"add_" + (object) con.ID_CONTENT + "\"  href=\"#\" onclick=\"addContentToCategory('" + (object) con.ID_CONTENT + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a></td>";
          else
            str1 = str1 + "<td><a id=\"done_" + (object) con.ID_CONTENT + "\"  href=\"#\" onclick=\"removeContentToCategory('" + (object) con.ID_CONTENT + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a></td>";
          str1 += "</tr>";
        }
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><th width=\"70%\">Content</th><th width=\"15%\">Status</th><th width=\"5%\">Add</th></tr></thead><tbody>" + str1 + "</tbody></table>";
    }

    public string getAssociatedResponse(string oid, string pattern)
    {
      string str = "";
      int orgid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<ContentAssociation> contentAssociationList = new List<ContentAssociation>();
      this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).ToList<tbl_category>();
      List<tbl_content> list1 = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select distinct id_content from tbl_content_organization_mapping where id_organization=" + (object) orgid + ") and CONTENT_QUESTION like'%" + pattern + "%' Limit 100").OrderBy<tbl_content, string>((Func<tbl_content, string>) (t => t.CONTENT_QUESTION)).ToList<tbl_content>();
      this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).FirstOrDefault<tbl_organization>();
      List<tbl_content> tblContentList = new List<tbl_content>();
      this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>) (t => t.id_organization == orgid)).ToList<tbl_content_organization_mapping>();
      foreach (tbl_content tblContent1 in list1)
      {
        tbl_content item = tblContent1;
        tbl_content tblContent2 = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == item.ID_CONTENT && t.STATUS == "A")).FirstOrDefault<tbl_content>();
        if (tblContent2 != null)
        {
          List<tbl_category> list2 = this.db.tbl_category.SqlQuery("select * from tbl_category where id_category in (select distinct id_category from tbl_content_organization_mapping where id_content=" + (object) item.ID_CONTENT + " AND id_organization=" + (object) orgid + ")").OrderBy<tbl_category, string>((Func<tbl_category, string>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
          str += "<tr>";
          str = str + "<td>" + tblContent2.CONTENT_QUESTION + "</td>";
          str += "<td>";
          foreach (tbl_category tblCategory in list2)
            str += tblCategory.CATEGORYNAME;
          str += "</td>";
          str = str + "<td><a href=\"#\" onclick=\"addContentToCategory('" + (object) item.ID_CONTENT + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a></td>";
          str += "</tr>";
        }
      }
      return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><th width=\"50%\">Content</th><th width=\"45%\">Category</th><th width=\"5%\">Add</th></tr></thead><tbody>" + str + "</tbody></table>";
    }

    public string addContentAssociation(string id, string cid, string oid)
    {
      int cids = Convert.ToInt32(cid);
      int oids = Convert.ToInt32(oid);
      int ids = Convert.ToInt32(id);
      string str = "0";
      tbl_content tblContent = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == ids && t.STATUS == "A")).FirstOrDefault<tbl_content>();
      if (tblContent != null)
      {
        tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_CATEGORY == cids && t.ID_ORGANIZATION == oids && t.STATUS == "A")).FirstOrDefault<tbl_category>();
        tbl_organization tblOrganization = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_ORGANIZATION == oids && t.STATUS == "A")).FirstOrDefault<tbl_organization>();
        if (tblCategory != null && tblOrganization != null)
        {
          if (this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>) (t => t.id_organization == oids && t.id_category == cids && t.id_content == ids)).FirstOrDefault<tbl_content_organization_mapping>() == null)
          {
            str = "N";
            this.db.tbl_content_organization_mapping.Add(new tbl_content_organization_mapping()
            {
              id_content = tblContent.ID_CONTENT,
              id_category = tblCategory.ID_CATEGORY,
              id_organization = tblOrganization.ID_ORGANIZATION,
              status = "A",
              updated_date_time = new DateTime?(DateTime.Now)
            });
            this.db.SaveChanges();
          }
          else
            str = "P";
        }
      }
      return str;
    }

    public string removeContentAssociation(string id, string cid, string oid)
    {
      int cids = Convert.ToInt32(cid);
      int oids = Convert.ToInt32(oid);
      int ids = Convert.ToInt32(id);
      string str = "0";
      if (this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>) (t => t.ID_CONTENT == ids && t.STATUS == "A")).FirstOrDefault<tbl_content>() != null)
      {
        tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_CATEGORY == cids && t.ID_ORGANIZATION == oids && t.STATUS == "A")).FirstOrDefault<tbl_category>();
        tbl_organization tblOrganization = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_ORGANIZATION == oids && t.STATUS == "A")).FirstOrDefault<tbl_organization>();
        if (tblCategory != null && tblOrganization != null)
        {
          tbl_content_organization_mapping entity = this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>) (t => t.id_organization == oids && t.id_category == cids && t.id_content == ids)).FirstOrDefault<tbl_content_organization_mapping>();
          if (entity != null)
          {
            this.db.tbl_content_organization_mapping.Remove(entity);
            this.db.SaveChanges();
            str = "1";
          }
        }
      }
      return str;
    }

    public string CopyContent(int id, int userid, int catid, int orgid, int soid)
    {
      tbl_content content = this.db.tbl_content.Find(new object[1]
      {
        (object) id
      });
      tbl_content entity1 = new tbl_content();
      entity1.CONTENT_HEADER = content.CONTENT_HEADER;
      entity1.CONTENT_QUESTION = content.CONTENT_QUESTION;
      entity1.CONTENT_TITLE = content.CONTENT_TITLE;
      entity1.ID_THEME = content.ID_THEME;
      entity1.ID_CONTENT_LEVEL = content.ID_CONTENT_LEVEL;
      entity1.ID_USER = userid;
      entity1.UPDATED_DATE_TIME = content.UPDATED_DATE_TIME;
      entity1.CONTENT_COUNTER = 0;
      entity1.EXPIRY_DATE = content.EXPIRY_DATE;
      entity1.STATUS = content.STATUS;
      entity1.IS_PRIMARY = content.IS_PRIMARY;
      this.db.tbl_content.Add(entity1);
      this.db.SaveChanges();
      this.db.tbl_content_right_association.Add(new tbl_content_right_association()
      {
        id_organization = orgid,
        id_content = entity1.ID_CONTENT,
        status = "O",
        updated_date_time = new DateTime?(DateTime.Now),
        content_expiry_date = new DateTime?(DateTime.Now.AddMonths(12))
      });
      this.db.SaveChanges();
      DbSet<tbl_content_answer> tblContentAnswer1 = this.db.tbl_content_answer;
      Expression<Func<tbl_content_answer, bool>> predicate1 = (Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == content.ID_CONTENT);
      foreach (tbl_content_answer tblContentAnswer2 in tblContentAnswer1.Where<tbl_content_answer>(predicate1).ToList<tbl_content_answer>())
      {
        tbl_content_answer answer = tblContentAnswer2;
        tbl_content_answer entity2 = new tbl_content_answer();
        entity2.ID_CONTENT = entity1.ID_CONTENT;
        entity2.CONTENT_ANSWER_TITLE = answer.CONTENT_ANSWER_TITLE;
        entity2.CONTENT_ANSWER_COUNTER = 0;
        entity2.CONTENT_ANSWER_HEADER = answer.CONTENT_ANSWER_HEADER;
        entity2.CONTENT_ANSWER1 = answer.CONTENT_ANSWER1;
        entity2.CONTENT_ANSWER_IMG1 = answer.CONTENT_ANSWER_IMG1;
        entity2.CONTENT_ANSWER2 = answer.CONTENT_ANSWER2;
        entity2.CONTENT_ANSWER3 = answer.CONTENT_ANSWER3;
        entity2.CONTENT_ANSWER4 = answer.CONTENT_ANSWER4;
        entity2.CONTENT_ANSWER_IMG2 = answer.CONTENT_ANSWER_IMG2;
        entity2.CONTENT_ANSWER_IMG3 = answer.CONTENT_ANSWER_IMG3;
        entity2.STATUS = answer.STATUS;
        entity2.UPDATEDTIME = DateTime.Now;
        this.db.tbl_content_answer.Add(entity2);
        this.db.SaveChanges();
        DbSet<tbl_content_answer_steps> contentAnswerSteps1 = this.db.tbl_content_answer_steps;
        Expression<Func<tbl_content_answer_steps, bool>> predicate2 = (Expression<Func<tbl_content_answer_steps, bool>>) (t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER);
        foreach (tbl_content_answer_steps contentAnswerSteps2 in contentAnswerSteps1.Where<tbl_content_answer_steps>(predicate2).ToList<tbl_content_answer_steps>())
        {
          this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
          {
            ID_CONTENT_ANSWER = entity2.ID_CONTENT_ANSWER,
            STEPNO = contentAnswerSteps2.STEPNO,
            ID_THEME = contentAnswerSteps2.ID_THEME,
            ANSWER_STEPS_IMG1 = contentAnswerSteps2.ANSWER_STEPS_IMG1,
            ANSWER_STEPS_IMG2 = contentAnswerSteps2.ANSWER_STEPS_IMG2,
            ANSWER_STEPS_IMG3 = contentAnswerSteps2.ANSWER_STEPS_IMG3,
            ANSWER_STEPS_PART1 = contentAnswerSteps2.ANSWER_STEPS_PART1,
            ANSWER_STEPS_PART2 = contentAnswerSteps2.ANSWER_STEPS_PART2,
            ANSWER_STEPS_PART3 = contentAnswerSteps2.ANSWER_STEPS_PART3,
            STATUS = contentAnswerSteps2.STATUS,
            UPDATEDDATETIME = DateTime.Now
          });
          this.db.SaveChanges();
        }
        DbSet<tbl_content_type_link> tblContentTypeLink1 = this.db.tbl_content_type_link;
        Expression<Func<tbl_content_type_link, bool>> predicate3 = (Expression<Func<tbl_content_type_link, bool>>) (t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER);
        foreach (tbl_content_type_link tblContentTypeLink2 in tblContentTypeLink1.Where<tbl_content_type_link>(predicate3).ToList<tbl_content_type_link>())
        {
          this.db.tbl_content_type_link.Add(new tbl_content_type_link()
          {
            ID_CONTENT_ANSWER = answer.ID_CONTENT_ANSWER,
            ID_CONTENT_TYPE = tblContentTypeLink2.ID_CONTENT_TYPE,
            LINK_VALUE = tblContentTypeLink2.LINK_VALUE,
            STATUS = tblContentTypeLink2.STATUS,
            UPDATED_DATE_TIME = DateTime.Now,
            DESCRIPTION = tblContentTypeLink2.DESCRIPTION
          });
          this.db.SaveChanges();
        }
        DbSet<tbl_content_metadata> tblContentMetadata1 = this.db.tbl_content_metadata;
        Expression<Func<tbl_content_metadata, bool>> predicate4 = (Expression<Func<tbl_content_metadata, bool>>) (t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER);
        foreach (tbl_content_metadata tblContentMetadata2 in tblContentMetadata1.Where<tbl_content_metadata>(predicate4).ToList<tbl_content_metadata>())
        {
          this.db.tbl_content_metadata.Add(new tbl_content_metadata()
          {
            CONTENT_METADATA = tblContentMetadata2.CONTENT_METADATA,
            CONTENT_METADATA_COUNTER = 0,
            ID_CONTENT_ANSWER = entity2.ID_CONTENT_ANSWER,
            UPDATEDTIME = DateTime.Now,
            STATUS = tblContentMetadata2.STATUS
          });
          this.db.SaveChanges();
        }
      }
      string str = "~/Content/SKILLMUNI_DATA/" + (object) soid + "/";
      string path1 = "~/Content/SKILLMUNI_DATA/" + (object) orgid + "/";
      if (!Directory.Exists(this.HttpContext.Server.MapPath(path1)))
        Directory.CreateDirectory(this.HttpContext.Server.MapPath(path1));
      string path2 = "~/Content/SKILLMUNI_DATA/" + (object) soid + "/" + (object) content.ID_CONTENT + "/";
      string path3 = "~/Content/SKILLMUNI_DATA/" + (object) orgid + "/" + (object) entity1.ID_CONTENT + "/";
      if (!Directory.Exists(this.HttpContext.Server.MapPath(path3)))
        Directory.CreateDirectory(this.HttpContext.Server.MapPath(path3));
      if (Directory.Exists(this.HttpContext.Server.MapPath(path3)))
      {
        foreach (string file in Directory.GetFiles(this.HttpContext.Server.MapPath(path2), "*.*", SearchOption.AllDirectories))
          System.IO.File.Copy(file, file.Replace(this.HttpContext.Server.MapPath(path2), this.HttpContext.Server.MapPath(path3)), true);
      }
      return "";
    }

    public ActionResult cms_display()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int orgid = Convert.ToInt32(content.id_ORGANIZATION);
      int uid = Convert.ToInt32(content.ID_USER);
      tbl_cms_users tblCmsUsers1 = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_cms_users>();
      List<CMSUser> cmsUserList = new List<CMSUser>();
      List<tbl_cms_roles> tblCmsRolesList = new List<tbl_cms_roles>();
      int? cmdUserType = tblCmsUsers1.cmd_user_type;
      int num = 0;
      List<tbl_cms_roles> list1;
      if ((cmdUserType.GetValueOrDefault() == num ? (cmdUserType.HasValue ? 1 : 0) : 0) != 0)
        list1 = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_cms_roles, string>((Expression<Func<tbl_cms_roles, string>>) (t => t.ROLENAME)).ToList<tbl_cms_roles>();
      else
        list1 = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).OrderBy<tbl_cms_roles, string>((Expression<Func<tbl_cms_roles, string>>) (t => t.ROLENAME)).ToList<tbl_cms_roles>();
      List<tbl_organization> list2 = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      foreach (tbl_cms_roles tblCmsRoles in list1)
      {
        tbl_cms_roles item = tblCmsRoles;
        IQueryable<tbl_cms_users> source = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_ROLE == item.ID_ROLE && t.STATUS == "A"));
        Expression<Func<tbl_cms_users, string>> keySelector = (Expression<Func<tbl_cms_users, string>>) (t => t.USERNAME);
        foreach (tbl_cms_users tblCmsUsers2 in source.OrderBy<tbl_cms_users, string>(keySelector).ToList<tbl_cms_users>())
          cmsUserList.Add(new CMSUser()
          {
            user = tblCmsUsers2,
            role = item
          });
      }
      this.ViewData["cms"] = (object) cmsUserList;
      this.ViewData["orglist"] = (object) list2;
      return (ActionResult) this.View();
    }

    public ActionResult cms_user_create()
    {
      int orgid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<tbl_cms_roles> list1 = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ORGANIZATION == orgid)).ToList<tbl_cms_roles>();
      List<tbl_organization> list2 = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      this.ViewData["rolelist"] = (object) list1;
      this.ViewData["orglist"] = (object) list2;
      if (orgid == 16)
        this.ViewData["type_flag"] = (object) "1";
      else
        this.ViewData["type_flag"] = (object) "0";
      return (ActionResult) this.View();
    }

    public string getRolesFromOrg(string id)
    {
      int orgid = Convert.ToInt32(id);
      List<tbl_cms_roles> list = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ORGANIZATION == orgid)).OrderBy<tbl_cms_roles, string>((Expression<Func<tbl_cms_roles, string>>) (t => t.ROLENAME)).ToList<tbl_cms_roles>();
      string rolesFromOrg = "";
      foreach (tbl_cms_roles tblCmsRoles in list)
        rolesFromOrg = rolesFromOrg + "<option value=\"" + (object) tblCmsRoles.ID_ROLE + "\">" + tblCmsRoles.ROLENAME + "</option>";
      return rolesFromOrg;
    }

    public ActionResult cms_user_edit(string id)
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int ids = Convert.ToInt32(id);
      tbl_cms_users users = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == ids)).FirstOrDefault<tbl_cms_users>();
      List<tbl_organization> list = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      if (users == null)
        return (ActionResult) this.RedirectToAction("cms_display");
      if (int32 == 16)
        this.ViewData["type_flag"] = (object) "1";
      else
        this.ViewData["type_flag"] = (object) "0";
      tbl_cms_roles crole = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ROLE == users.ID_ROLE)).FirstOrDefault<tbl_cms_roles>();
      this.ViewData["crole"] = (object) crole;
      this.ViewData["orglist"] = (object) list;
      this.ViewData["roles"] = (object) this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ORGANIZATION == crole.ID_ORGANIZATION)).OrderBy<tbl_cms_roles, string>((Expression<Func<tbl_cms_roles, string>>) (t => t.ROLENAME)).ToList<tbl_cms_roles>();
      this.ViewData["cms_user"] = (object) users;
      return (ActionResult) this.View();
    }

    public ActionResult create_cms_user()
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      string str1 = this.Request.Form["user-name"];
      string str2 = this.Request.Form["pass-word"];
      int int32 = Convert.ToInt32(this.Request.Form["select-role"]);
      tbl_cms_users entity = new tbl_cms_users();
      entity.ID_ROLE = int32;
      entity.USERNAME = str1;
      entity.PASSWORD = str2;
      entity.STATUS = "A";
      string str3 = this.Request.Form["first-name"];
      string str4 = this.Request.Form["employee-id"];
      entity.employee_name = str3;
      entity.employee_id = str4;
      entity.cmd_user_type = new int?(1);
      entity.UPDATED_DATE_TIME = DateTime.Now;
      this.db.tbl_cms_users.Add(entity);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("cms_display");
    }

    public ActionResult edit_cms_user()
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int id_user = Convert.ToInt32(this.Request.Form["id_user"]);
      string str1 = this.Request.Form["user-name"];
      string str2 = this.Request.Form["pass-word"];
      string str3 = this.Request.Form["employee-name"];
      string str4 = this.Request.Form["employee-id"];
      int int32 = Convert.ToInt32(this.Request.Form["select-role"]);
      tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == id_user)).FirstOrDefault<tbl_cms_users>();
      if (tblCmsUsers != null)
      {
        tblCmsUsers.ID_ROLE = int32;
        tblCmsUsers.USERNAME = str1;
        tblCmsUsers.PASSWORD = str2;
        tblCmsUsers.STATUS = "A";
        tblCmsUsers.employee_name = str3;
        tblCmsUsers.employee_id = str4;
        tblCmsUsers.cmd_user_type = new int?(1);
        tblCmsUsers.UPDATED_DATE_TIME = DateTime.Now;
        this.db.SaveChanges();
      }
      return (ActionResult) this.RedirectToAction("cms_display");
    }

      


        public ActionResult app_user_display()
        {
            try
            {
                var alertMessage = TempData["AlertMessage"] as string;

                ViewBag.AlertMessage = alertMessage;

                int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
                List<AppUser> appUserList = new List<AppUser>();
                this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>)(t => t.csst_role)).ToList<tbl_csst_role>();

                //test 
                //var users = this.db.tbl_user.SqlQuery(
                //        "SELECT * FROM tbl_user WHERE id_user IN (" +
                //        "SELECT id_user FROM tbl_role_user_mapping WHERE id_csst_role IN (" +
                //        "SELECT id_csst_role FROM tbl_csst_role WHERE id_organization=" + (object)orgid + "))")
                //        .OrderBy(t => t.USERID)
                //        .ToList();

                //foreach (tbl_user tblUser in users)
                //{
                //    tbl_user item = tblUser;
                //    List<tbl_csst_role> list = this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where id_csst_role in (select id_csst_role from tbl_role_user_mapping where id_user=" + (object)item.ID_USER + ") and status='A'").OrderBy<tbl_csst_role, string>((Func<tbl_csst_role, string>)(t => t.csst_role)).ToList<tbl_csst_role>();
                //    AppUser appUser = new AppUser();
                //    appUser.User = item;
                //    appUser.Role = list;

                //    tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();

                //    appUser.Profile = tblProfile;
                //    appUserList.Add(appUser);

                //}
                //this.ViewData["userlist"] = (object)appUserList;
                //

                foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").OrderBy<tbl_user, string>((Func<tbl_user, string>)(t => t.USERID)).ToList<tbl_user>())
                {
                    tbl_user item = tblUser;
                    List<tbl_csst_role> list = this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where id_csst_role in (select id_csst_role from tbl_role_user_mapping where id_user=" + (object)item.ID_USER + ") and status='A'").OrderBy<tbl_csst_role, string>((Func<tbl_csst_role, string>)(t => t.csst_role)).ToList<tbl_csst_role>();
                    AppUser appUser = new AppUser();
                    appUser.User = item;
                    appUser.Role = list;

                    tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();

                    appUser.Profile = tblProfile;
                    appUserList.Add(appUser);

                }
                this.ViewData["userlist"] = (object)appUserList;

                //          int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

                //          var usersWithRolesAndProfiles = db.tbl_user.SqlQuery(
                //              @"SELECT u.*, r.*, p.*
                //FROM tbl_user u
                //LEFT JOIN tbl_role_user_mapping rum ON u.id_user = rum.id_user
                //LEFT JOIN tbl_csst_role r ON rum.id_csst_role = r.id_csst_role
                //LEFT JOIN tbl_profile p ON u.id_user = p.id_user
                //WHERE r.id_organization = @p0 AND rum.status = 'A'", orgid).ToList();

                //          List<AppUser> appUserList = new List<AppUser>();

                //          foreach (var user in usersWithRolesAndProfiles)
                //          {
                //              var roles = usersWithRolesAndProfiles
                //                  .Where(r => r.ID_USER == user.ID_USER)
                //                  .Select(r => r.ID_ROLE)
                //                  .ToList();

                //              var profile = usersWithRolesAndProfiles
                //                  .Where(p => p.ID_USER == user.ID_USER)
                //                  .Select(p => p.Profile)
                //                  .FirstOrDefault();

                //              AppUser appUser = new AppUser
                //              {
                //                  User = user,
                //                  Roles = roles,
                //                  Profile = profile
                //              };

                //              appUserList.Add(appUser);
                //          }

                //          this.ViewData["userlist"] = appUserList;








            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (ActionResult)this.View();
        }

        //For testing 
        //public async Task<ActionResult> app_user_display()
        //{
        //    try
        //    {
        //        var alertMessage = TempData["AlertMessage"] as string;
        //        ViewBag.AlertMessage = alertMessage;

        //        int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

        //        var query = from user in this.db.tbl_user
        //                    join mapping in this.db.tbl_role_user_mapping on user.ID_USER equals mapping.id_user
        //                    join role in this.db.tbl_csst_role on mapping.id_csst_role equals role.id_csst_role
        //                    where role.status == "A" && role.id_organization == orgid
        //                    orderby user.USERID
        //                    select new
        //                    {
        //                        User = user,
        //                        Role = role
        //                    };

        //        var results = await query.ToListAsync();

        //        var appUserList = new List<AppUser>();

        //        foreach (var result in results.GroupBy(r => r.User.ID_USER))
        //        {
        //            var appUser = new AppUser();
        //            appUser.User = result.First().User;
        //            appUser.Role = result.Select(r => r.Role).ToList();

        //            appUser.Profile = await this.db.tbl_profile.FirstOrDefaultAsync(p => p.ID_USER == appUser.User.ID_USER);

        //            appUserList.Add(appUser);
        //        }

        //        this.ViewData["userlist"] = appUserList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return View();
        //}



        [HttpPost]
   public ActionResult app_ChangeStatus(string userId, string newStatus)
        {
            int User_id = Convert.ToInt32(userId);
            tbl_user tbluser = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_USER == User_id)).FirstOrDefault<tbl_user>();
            if (tbluser != null)
            {

                tbluser.STATUS = newStatus;

                this.db.SaveChanges();
            }
           
            return Json(new { success = true, message = "Status changed successfully" });


        }

        [HttpPost]
        public ActionResult app_ChangeStatusfile(List<UserIdStatusModel> userIdStatusArray)
        {
            List<string> nonExistentUserIds = new List<string>();
            List<string> activeUserIds = new List<string>();

            foreach (var userStatus in userIdStatusArray)
            {
                string userId = userStatus.UserId;
                string newStatus = userStatus.NewStatus;

                bool userExists = this.db.tbl_user.Any(t => t.USERID == userId);

                if (userExists)
                {

                    if (newStatus == "D")
                    {
                   
                        tbl_user tbluser = this.db.tbl_user
                                                  .Where(t => t.USERID == userId)
                                                  .FirstOrDefault();

                        if (tbluser != null)
                        {
                            tbluser.STATUS = newStatus;
                        }
                   
                     }
                    else
                    {
                        activeUserIds.Add(userId);
                    }
                }
                else
                {
                    nonExistentUserIds.Add(userId);
                }


            }

            this.db.SaveChanges();

            return Json(new { success = true, message = "Status changed successfully", nonExistentUserIds, activeUserIds });
        }

        [HttpPost]
        public ActionResult app_ChangeStatusfileActive(List<UserIdStatusModel> userIdStatusArray)
        {
            List<string> nonExistentUserIds = new List<string>();
            List<string> activeUserIds = new List<string>();

            foreach (var userStatus in userIdStatusArray)
            {
                string userId = userStatus.UserId;
                string newStatus = userStatus.NewStatus;
                bool userExists = this.db.tbl_user.Any(t => t.USERID == userId);
                if (userExists)
                {
                    if (newStatus == "A")
                    {
                   
                        tbl_user tbluser = this.db.tbl_user
                                                  .Where(t => t.USERID == userId)
                                                  .FirstOrDefault();

                        if (tbluser != null)
                        {
                            tbluser.STATUS = newStatus;
                        }
                    }
                    else
                    {
                        activeUserIds.Add(userId);
                    }

                }
                else
                {
                    nonExistentUserIds.Add(userId);
                }


            }

            this.db.SaveChanges();

            return Json(new { success = true, message = "Status changed successfully", nonExistentUserIds, activeUserIds });
        }

        public ActionResult add_app_user()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int orgid = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_csst_role> list1 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>) (t => t.id_organization == (int?) orgid && t.status == "A")).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>) (t => t.csst_role)).ToList<tbl_csst_role>();
      List<tbl_organization> list2 = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      List<tbl_user> list3 = this.db.tbl_user.SqlQuery("select * from tbl_user where is_reporting=1 and id_organization = " + (object) orgid + " and status='A'").ToList<tbl_user>();
      this.ViewData["orglist"] = (object) list2;
      this.ViewData["rmlist"] = (object) list3;
      if (orgid == 16)
        this.ViewData["type_flag"] = (object) "1";
      else
        this.ViewData["type_flag"] = (object) "0";
      this.ViewData["roles"] = (object) list1;
      if (!(content.org_status == "F") && !(content.org_status == "S") && !(content.org_status == "H"))
        return (ActionResult) this.View();
      return new addCMS_CategoryModel().check_appuser_count(orgid) < 10 ? (ActionResult) this.View() : (ActionResult) this.RedirectToAction("App_user_status");
    }

    public ActionResult App_user_status() => (ActionResult) this.View();

    public string getAppRolesFromOrg(string id)
    {
      int orgid = Convert.ToInt32(id);
      List<tbl_role> list = this.db.tbl_role.Where<tbl_role>((Expression<Func<tbl_role, bool>>) (t => t.ID_ORGANIZATION == orgid)).OrderBy<tbl_role, string>((Expression<Func<tbl_role, string>>) (t => t.ROLENAME)).ToList<tbl_role>();
      string appRolesFromOrg = "";
      foreach (tbl_role tblRole in list)
        appRolesFromOrg = appRolesFromOrg + "<option value=\"" + (object) tblRole.ID_ROLE + "\">" + tblRole.ROLENAME + "</option>";
      return appRolesFromOrg;
    }

    public ActionResult add_app_user_action()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int orgid = Convert.ToInt32(content.id_ORGANIZATION);
      string str1 = this.Request.Form["role_check"];
      Convert.ToInt32(content.ID_USER);
      string str2 = this.Request.Form["user-first-name"];
      string str3 = this.Request.Form["user-last-name"];
      string str4 = this.Request.Form["user-name"];
      string inputString = this.Request.Form["pass-word"];
      string dateString1 = this.Request.Form["user-dob"];
      string dateString2 = this.Request.Form["user-doj"];
      string str5 = this.Request.Form["employee-id"];
      int int32_1 = Convert.ToInt32(this.Request.Form["user-age"]);
      List<tbl_role> list = this.db.tbl_role.Where<tbl_role>((Expression<Func<tbl_role, bool>>) (t => t.ID_ORGANIZATION == orgid)).OrderBy<tbl_role, string>((Expression<Func<tbl_role, string>>) (t => t.ROLENAME)).ToList<tbl_role>();
      tbl_cms_users tblCmsUsers = new tbl_cms_users();
      string str6 = Convert.ToInt32(this.Request.Form["select-gender"]) != 1 ? "F" : "M";
      string str7 = this.Request.Form["user-mobile"];
      string str8 = this.Request.Form["user-email"];
      string str9 = this.Request.Form["user-loc"];
      string str10 = this.Request.Form["user-designation"];
      string str11 = this.Request.Form["office-address"];
      string str12 = this.Request.Form["user-doj"];
      string str13 = this.Request.Form["user-rm"];
      int int32_2 = Convert.ToInt32(this.Request.Form["user-is-rm"]);
      string str14 = this.Request.Form["user-department"];
      string str15 = this.Request.Form["user-function"];
      string str16 = this.Request.Form["user-grade"];
      tbl_user user = new tbl_user();
      user.USERID = str4.Trim();
      string md5Hash = inputString.ToMD5Hash();
      user.PASSWORD = md5Hash;
      user.ID_ORGANIZATION = new int?(orgid);
      user.ID_ROLE = list[0].ID_ROLE;
      user.FBSOCIALID = "";
      user.GPSOCIALID = "";
      user.EXPIRY_DATE = new DateTime?(DateTime.Now);
      user.STATUS = "A";
      user.is_reporting = new int?(int32_2);
      user.UPDATEDTIME = DateTime.Now;
      user.ID_CODE = 1;
      user.EMPLOYEEID = str5;
      user.reporting_manager = new int?(Convert.ToInt32(str13));
      user.user_department = str14;
      user.user_designation = str10;
      user.user_function = str15;
      user.user_grade = str16;
      user.user_status = "A";
      this.db.tbl_user.Add(user);
      this.db.SaveChanges();
      this.db.tbl_profile.Add(new tbl_profile()
      {
        FIRSTNAME = str2,
        LASTNAME = str3,
        EMAIL = str8,
        MOBILE = str7,
        AGE = new int?(int32_1),
        CITY = str9,
        LOCATION = str9,
        DATE_OF_BIRTH = new DateTime?(new Utility().StringToDatetime(dateString1)),
        DATE_OF_JOINING = new DateTime?(new Utility().StringToDatetime(dateString2)),
        DESIGNATION = str10,
        GENDER = str6,
        ID_USER = user.ID_USER,
        OFFICE_ADDRESS = str11,
        REPORTING_MANAGER = str13
      });
      this.db.SaveChanges();
      if (!string.IsNullOrEmpty(str1))
      {
        string str17 = str1;
        char[] chArray = new char[1]{ ',' };
        foreach (string str18 in str17.Split(chArray))
        {
          int rid = Convert.ToInt32(str18);
          this.db.tbl_role_user_mapping.Add(new tbl_role_user_mapping()
          {
            id_user = new int?(user.ID_USER),
            id_csst_role = new int?(rid),
            updated_date_time = new DateTime?(DateTime.Now),
            status = "A"
          });
          this.db.SaveChanges();
          DbSet<tbl_assignment_role_program> assignmentRoleProgram1 = this.db.tbl_assignment_role_program;
          Expression<Func<tbl_assignment_role_program, bool>> predicate = (Expression<Func<tbl_assignment_role_program, bool>>) (t => t.id_role == (int?) rid && t.id_organization == (int?) orgid);
          foreach (tbl_assignment_role_program assignmentRoleProgram2 in assignmentRoleProgram1.Where<tbl_assignment_role_program>(predicate).ToList<tbl_assignment_role_program>())
          {
            tbl_assignment_role_program rpItem = assignmentRoleProgram2;
            if (this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>) (t => t.id_category == rpItem.id_program && t.id_organization == (int?) orgid && t.id_user == (int?) user.ID_USER)).FirstOrDefault<tbl_content_program_mapping>() == null)
            {
              this.db.tbl_content_program_mapping.Add(new tbl_content_program_mapping()
              {
                map_type = new int?(1),
                id_role = rpItem.id_role,
                id_user = new int?(user.ID_USER),
                id_organization = new int?(orgid),
                id_category = rpItem.id_program,
                status = "A",
                option_type = new int?(0),
                start_date = rpItem.start_datetime,
                expiry_date = rpItem.end_datetime,
                id_category_tile = rpItem.id_category_tile,
                id_category_heading = rpItem.id_category_heading,
                id_assessment_sheet = new int?(0),
                updated_date_time = new DateTime?(DateTime.Now)
              });
              this.db.SaveChanges();
            }
          }
        }
      }
      return (ActionResult) this.RedirectToAction("app_user_display");
    }

    public string check_user_name(string uname)
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      uname = uname.Trim();
      return this.db.tbl_user.SqlQuery("select * from tbl_user where UPPER(USERID) LIKE upper('" + uname + "')   and id_organization=" + (object) int32 + " ").FirstOrDefault<tbl_user>() != null ? "0" : "1";
    }

        public ActionResult edit_app_user1(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            List<tbl_csst_role> tblCsstRoleList = new List<tbl_csst_role>();
            List<tbl_csst_role> list1 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid && t.status == "A")).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>)(t => t.csst_role)).ToList<tbl_csst_role>();
            this.ViewData["roles"] = (object)list1;
            string str1 = "";
            AppUser appUser = new AppUser();

            tbl_user tblUser = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_USER == ids && t.ID_ORGANIZATION == (int?)orgid)).FirstOrDefault<tbl_user>();


            if (tblUser == null)
                return (ActionResult)this.RedirectToAction("app_user_display");
            List<tbl_organization> list2 = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>)(t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>)(t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
            List<tbl_user> list3 = this.db.tbl_user.SqlQuery("select * from tbl_user where is_reporting=1 and id_organization = " + (object)orgid + " and status='A'").ToList<tbl_user>();
            this.ViewData["orglist"] = (object)list2;
            this.ViewData["rmlist"] = (object)list3;
            if (orgid == 16)
                this.ViewData["type_flag"] = (object)"1";
            else
                this.ViewData["type_flag"] = (object)"0";
            List<tbl_csst_role> list4 = this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where id_csst_role in (select id_csst_role from tbl_role_user_mapping where id_user=" + (object)tblUser.ID_USER + " )").OrderBy<tbl_csst_role, string>((Func<tbl_csst_role, string>)(t => t.csst_role)).ToList<tbl_csst_role>();
            tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == ids)).FirstOrDefault<tbl_profile>();
            appUser.User = tblUser;
            appUser.Profile = tblProfile;
            appUser.Role = list4;
            this.ViewData["appUser"] = (object)appUser;
            List<csstRole> csstRoleList = new List<csstRole>();
            foreach (tbl_csst_role tblCsstRole1 in list1)
            {
                csstRole csstRole = new csstRole();
                string str2 = "";
                foreach (tbl_csst_role tblCsstRole2 in appUser.Role)
                {
                    if (tblCsstRole1.id_csst_role == tblCsstRole2.id_csst_role)
                    {
                        str2 = " checked";
                        csstRole.check = " checked";
                    }
                    else
                        csstRole.check = " ";
                }
                csstRole.id_csst_role = tblCsstRole1.id_csst_role;
                csstRole.csst_role = tblCsstRole1.csst_role;
                str1 = str1 + "<div class=\"checkbox-inline\" style=\"padding-right:15px;\" ><input type=\"checkbox\" value=\"" + (object)tblCsstRole1.id_csst_role + "\" name=\"role_check\" " + str2 + " />" + tblCsstRole1.csst_role + "</div>";
                csstRoleList.Add(csstRole);
            }
            this.ViewData["role-option"] = (object)str1;
            this.ViewData["rol"] = (object)csstRoleList;
            return (ActionResult)this.View();
        }


        // test

        public ActionResult edit_app_user(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            string str1 = "";
            List<tbl_csst_role> list1 = this.db.tbl_csst_role
                .Where(t => t.id_organization == orgid && t.status == "A")
                .OrderBy(t => t.csst_role)
                .ToList();
            this.ViewData["roles"] = list1;

            AppUserList appUser = new AppUserList();

            User newUser = GetUserByIdAndOrganization(ids, orgid);
            if (newUser == null)
            {
                return RedirectToAction("app_user_display");
            }

            List<tbl_organization> list2 = this.db.tbl_organization
                .Where(t => t.STATUS == "A")
                .OrderBy(t => t.ORGANIZATION_NAME)
                .ToList();
            List<tbl_user> list3 = this.db.tbl_user
                .SqlQuery("SELECT * FROM tbl_user WHERE is_reporting=1 AND id_organization = @p0 AND status='A'", orgid)
                .ToList();
            this.ViewData["orglist"] = list2;
            this.ViewData["rmlist"] = list3;

            this.ViewData["type_flag"] = orgid == 16 ? "1" : "0";

            List<tbl_csst_role> list4 = this.db.tbl_csst_role
                .SqlQuery("SELECT * FROM tbl_csst_role WHERE id_csst_role IN (SELECT id_csst_role FROM tbl_role_user_mapping WHERE id_user = @p0)", newUser.ID_USER)
                .OrderBy(t => t.csst_role)
                .ToList();

            tbl_profile tblProfile = this.db.tbl_profile
                .FirstOrDefault(t => t.ID_USER == ids);
            appUser.User = newUser;
            appUser.Profile = tblProfile;
            appUser.Role = list4;

            this.ViewData["appUser"] = appUser;

            List<csstRole> csstRoleList = new List<csstRole>();
            foreach (tbl_csst_role tblCsstRole1 in list1)
            {
                csstRole csstRole = new csstRole();
                string str2 = "";
                foreach (tbl_csst_role tblCsstRole2 in appUser.Role)
                {
                    if (tblCsstRole1.id_csst_role == tblCsstRole2.id_csst_role)
                    {
                        str2 = " checked";
                        csstRole.check = " checked";
                    }
                    else
                    {
                        csstRole.check = " ";
                    }
                }
                csstRole.id_csst_role = tblCsstRole1.id_csst_role;
                csstRole.csst_role = tblCsstRole1.csst_role;
                str1 += $"<div class=\"checkbox-inline\" style=\"padding-right:15px;\" ><input type=\"checkbox\" value=\"{tblCsstRole1.id_csst_role}\" name=\"role_check\" {str2} />{tblCsstRole1.csst_role}</div>";
                csstRoleList.Add(csstRole);
            }
            this.ViewData["role-option"] = str1;
            this.ViewData["rol"] = csstRoleList;
            return View();
        }

        public User GetUserByIdAndOrganization(int userId, int? organizationId)
        {
            User user = null;

            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbl_user WHERE ID_USER = @ID_USER" +
                               (organizationId.HasValue ? " AND ID_ORGANIZATION = @ID_ORGANIZATION" : "");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_USER", userId);

                    if (organizationId.HasValue)
                    {
                        command.Parameters.AddWithValue("@ID_ORGANIZATION", organizationId.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                ID_USER = reader.IsDBNull(reader.GetOrdinal("ID_USER")) ? (int?)null : reader.GetInt32("ID_USER"),
                                ID_CODE = reader.IsDBNull(reader.GetOrdinal("ID_CODE")) ? (int?)null : reader.GetInt32("ID_CODE"),
                                ID_ORGANIZATION = reader.IsDBNull(reader.GetOrdinal("ID_ORGANIZATION")) ? (int?)null : reader.GetInt32("ID_ORGANIZATION"),
                                ID_ROLE = reader.IsDBNull(reader.GetOrdinal("ID_ROLE")) ? (int?)null : reader.GetInt32("ID_ROLE"),
                                USERID = reader.IsDBNull(reader.GetOrdinal("USERID")) ? null : reader.GetString("USERID"),
                                PASSWORD = reader.IsDBNull(reader.GetOrdinal("PASSWORD")) ? null : reader.GetString("PASSWORD"),
                                FBSOCIALID = reader.IsDBNull(reader.GetOrdinal("FBSOCIALID")) ? null : reader.GetString("FBSOCIALID"),
                                GPSOCIALID = reader.IsDBNull(reader.GetOrdinal("GPSOCIALID")) ? null : reader.GetString("GPSOCIALID"),
                                STATUS = reader.IsDBNull(reader.GetOrdinal("STATUS")) ? (char?)null : reader.GetChar("STATUS"),
                                UPDATEDTIME = reader.IsDBNull(reader.GetOrdinal("UPDATEDTIME")) ? (DateTime?)null : reader.GetDateTime("UPDATEDTIME"),
                                EXPIRY_DATE = reader.IsDBNull(reader.GetOrdinal("EXPIRY_DATE")) ? (DateTime?)null : reader.GetDateTime("EXPIRY_DATE"),
                                EMPLOYEEID = reader.IsDBNull(reader.GetOrdinal("EMPLOYEEID")) ? null : reader.GetString("EMPLOYEEID"),
                                user_department = reader.IsDBNull(reader.GetOrdinal("user_department")) ? null : reader.GetString("user_department"),
                                user_designation = reader.IsDBNull(reader.GetOrdinal("user_designation")) ? null : reader.GetString("user_designation"),
                                user_function = reader.IsDBNull(reader.GetOrdinal("user_function")) ? null : reader.GetString("user_function"),
                                user_grade = reader.IsDBNull(reader.GetOrdinal("user_grade")) ? null : reader.GetString("user_grade"),
                                user_status = reader.IsDBNull(reader.GetOrdinal("user_status")) ? null : reader.GetString("user_status"),
                                reporting_manager = reader.IsDBNull(reader.GetOrdinal("reporting_manager")) ? (int?)null : reader.GetInt32("reporting_manager"),
                                is_reporting = reader.IsDBNull(reader.GetOrdinal("is_reporting")) ? (int?)null : reader.GetInt32("is_reporting"),
                                created_datetime = reader.IsDBNull(reader.GetOrdinal("created_datetime")) ? (DateTime?)null : reader.GetDateTime("created_datetime"),
                                last_modified_datetime = reader.IsDBNull(reader.GetOrdinal("last_modified_datetime")) ? (DateTime?)null : reader.GetDateTime("last_modified_datetime"),
                                L4 = reader.IsDBNull(reader.GetOrdinal("L4")) ? null : reader.GetString("L4"),
                                L3 = reader.IsDBNull(reader.GetOrdinal("L3")) ? null : reader.GetString("L3"),
                                L2 = reader.IsDBNull(reader.GetOrdinal("L2")) ? null : reader.GetString("L2"),
                                L1 = reader.IsDBNull(reader.GetOrdinal("L1")) ? null : reader.GetString("L1"),
                                Spectator = reader.IsDBNull(reader.GetOrdinal("Spectator")) ? null : reader.GetString("Spectator")
                            };
                        }
                    }
                }
            }

            return user;
        }

        public ActionResult edit_app_user_action()
        {
            int id_user = Convert.ToInt32(this.Request.Form["id_user"]);
            if (id_user > 0)
            {
                tbl_user user = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_USER == id_user && t.STATUS == "A")).FirstOrDefault<tbl_user>();
                if (user == null)
                    return (ActionResult)this.RedirectToAction("app_user_display");
                string str1 = this.Request.Form["user-first-name"];
                string str2 = this.Request.Form["user-last-name"];
                string str3 = this.Request.Form["user-name"];
                string inputString = this.Request.Form["pass-word"];
                string dateString1 = this.Request.Form["user-dob"];
                string dateString2 = this.Request.Form["user-doj"];
                int int32_1 = Convert.ToInt32(this.Request.Form["user-age"]);
                Convert.ToInt32(this.Request.Form["select-role"]);
                int int32_2 = Convert.ToInt32(this.Request.Form["select-gender"]);
                string str4 = this.Request.Form["employee-id"];
                string str5 = int32_2 != 1 ? "F" : "M";
                string str6 = this.Request.Form["user-mobile"];
                string str7 = this.Request.Form["user-email"];
                string str8 = this.Request.Form["user-loc"];
                string str9 = this.Request.Form["user-designation"];
                string str10 = this.Request.Form["office-address"];
                string str11 = this.Request.Form["user-doj"];
                string str12 = this.Request.Form["user-rm"];
                string str13 = this.Request.Form["user-department"];
                string str14 = this.Request.Form["user-function"];
                string str15 = this.Request.Form["user-grade"];
                int int32_3 = Convert.ToInt32(this.Request.Form["user-is-rm"]);

                string str20 = this.Request.Form["user-L4"];
                string str21 = this.Request.Form["user-L3"];
                string str22 = this.Request.Form["user-L2"];
                string str23 = this.Request.Form["user-L1"];
                string str24 = this.Request.Form["user-Spectator"];

                user.USERID = str3.Trim();
                if (!string.IsNullOrEmpty(inputString))
                {
                    string md5Hash = inputString.ToMD5Hash();
                    user.PASSWORD = md5Hash;
                }
                user.FBSOCIALID = "";
                user.GPSOCIALID = "";
                user.EXPIRY_DATE = new DateTime?(DateTime.Now);
                user.STATUS = "A";
                user.EMPLOYEEID = str4;
                user.UPDATEDTIME = DateTime.Now;
                user.ID_CODE = 1;
                user.reporting_manager = new int?(Convert.ToInt32(str12));
                user.user_department = str13;
                user.user_designation = str9;
                user.user_function = str14;
                user.is_reporting = new int?(int32_3);
                user.user_grade = str15;
                user.user_status = "A";
                this.db.SaveChanges();

                string strrole = this.Request.Form["role_check"];
                string[] strArraystrrole = null;
                if (strrole != null)
                    strArraystrrole = strrole.Split(',');

                //Assuming you want to store a comma - separated string of roles
                string rolesAsString = string.Join(",", strArraystrrole[0]);

                UserService userService = new UserService();
                bool isUpdated = userService.updatethetable_user1(id_user, str20, str21, str22, str23, str24, rolesAsString);

                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == user.ID_USER)).FirstOrDefault<tbl_profile>();
                if (tblProfile != null)
                {
                    tblProfile.FIRSTNAME = str1;
                    tblProfile.LASTNAME = str2;
                    tblProfile.EMAIL = str7;
                    tblProfile.MOBILE = str6;
                    tblProfile.AGE = new int?(int32_1);
                    tblProfile.CITY = str8;
                    tblProfile.LOCATION = str8;
                    tblProfile.DATE_OF_BIRTH = new DateTime?(new Utility().StringToDatetime(dateString1));
                    tblProfile.DATE_OF_JOINING = new DateTime?(new Utility().StringToDatetime(dateString2));
                    tblProfile.DESIGNATION = str9;
                    tblProfile.GENDER = str5;
                    tblProfile.ID_USER = user.ID_USER;
                    tblProfile.OFFICE_ADDRESS = str10;
                    tblProfile.REPORTING_MANAGER = str12;
                    this.db.SaveChanges();
                }
                string str16 = this.Request.Form["role_check"];
                string[] strArray = (string[])null;
                if (str16 != null)
                    strArray = str16.Split(',');
                DbSet<tbl_role_user_mapping> tblRoleUserMapping = this.db.tbl_role_user_mapping;
                Expression<Func<tbl_role_user_mapping, bool>> predicate = (Expression<Func<tbl_role_user_mapping, bool>>)(t => t.id_user == (int?)user.ID_USER);
                foreach (tbl_role_user_mapping entity in tblRoleUserMapping.Where<tbl_role_user_mapping>(predicate).ToList<tbl_role_user_mapping>())
                {
                    bool flag = true;
                    if (strArray != null)
                    {
                        foreach (string str17 in strArray)
                        {
                            int int32_4 = Convert.ToInt32(str17);
                            int? idCsstRole = entity.id_csst_role;
                            int valueOrDefault = idCsstRole.GetValueOrDefault();
                            if ((int32_4 == valueOrDefault ? (idCsstRole.HasValue ? 1 : 0) : 0) != 0)
                                flag = false;
                        }
                    }
                    if (flag)
                    {
                        this.db.tbl_role_user_mapping.Remove(entity);
                        this.db.SaveChanges();
                    }
                }
                if (strArray != null)
                {
                    foreach (string str18 in strArray)
                    {
                        int rid = Convert.ToInt32(str18);
                        if (this.db.tbl_role_user_mapping.Where<tbl_role_user_mapping>((Expression<Func<tbl_role_user_mapping, bool>>)(t => t.id_user == (int?)user.ID_USER && t.id_csst_role == (int?)rid)).FirstOrDefault<tbl_role_user_mapping>() == null)
                        {
                            this.db.tbl_role_user_mapping.Add(new tbl_role_user_mapping()
                            {
                                id_user = new int?(user.ID_USER),
                                id_csst_role = new int?(rid),
                                updated_date_time = new DateTime?(DateTime.Now),
                                status = "A"
                            });
                            this.db.SaveChanges();
                        }
                    }
                }



            }
            TempData["AlertMessage"] = "Data is Updated";
            return RedirectToAction("edit_app_user", new { id = id_user });

            //return (ActionResult)this.RedirectToAction("app_user_display");
        }


        //

        public ActionResult edit_app_user_action1()
        {
            int id_user = Convert.ToInt32(this.Request.Form["id_user"]);
            if (id_user > 0)
            {
                tbl_user user = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_USER == id_user && t.STATUS == "A")).FirstOrDefault<tbl_user>();
                if (user == null)
                    return (ActionResult)this.RedirectToAction("app_user_display");
                string str1 = this.Request.Form["user-first-name"];
                string str2 = this.Request.Form["user-last-name"];
                string str3 = this.Request.Form["user-name"];
                string inputString = this.Request.Form["pass-word"];
                string dateString1 = this.Request.Form["user-dob"];
                string dateString2 = this.Request.Form["user-doj"];
                int int32_1 = Convert.ToInt32(this.Request.Form["user-age"]);
                Convert.ToInt32(this.Request.Form["select-role"]);
                int int32_2 = Convert.ToInt32(this.Request.Form["select-gender"]);
                string str4 = this.Request.Form["employee-id"];
                string str5 = int32_2 != 1 ? "F" : "M";
                string str6 = this.Request.Form["user-mobile"];
                string str7 = this.Request.Form["user-email"];
                string str8 = this.Request.Form["user-loc"];
                string str9 = this.Request.Form["user-designation"];
                string str10 = this.Request.Form["office-address"];
                string str11 = this.Request.Form["user-doj"];
                string str12 = this.Request.Form["user-rm"];
                string str13 = this.Request.Form["user-department"];
                string str14 = this.Request.Form["user-function"];
                string str15 = this.Request.Form["user-grade"];
                int int32_3 = Convert.ToInt32(this.Request.Form["user-is-rm"]);
                user.USERID = str3.Trim();
                if (!string.IsNullOrEmpty(inputString))
                {
                    string md5Hash = inputString.ToMD5Hash();
                    user.PASSWORD = md5Hash;
                }
                user.FBSOCIALID = "";
                user.GPSOCIALID = "";
                user.EXPIRY_DATE = new DateTime?(DateTime.Now);
                user.STATUS = "A";
                user.EMPLOYEEID = str4;
                user.UPDATEDTIME = DateTime.Now;
                user.ID_CODE = 1;
                user.reporting_manager = new int?(Convert.ToInt32(str12));
                user.user_department = str13;
                user.user_designation = str9;
                user.user_function = str14;
                user.is_reporting = new int?(int32_3);
                user.user_grade = str15;
                user.user_status = "A";
                this.db.SaveChanges();
                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == user.ID_USER)).FirstOrDefault<tbl_profile>();
                if (tblProfile != null)
                {
                    tblProfile.FIRSTNAME = str1;
                    tblProfile.LASTNAME = str2;
                    tblProfile.EMAIL = str7;
                    tblProfile.MOBILE = str6;
                    tblProfile.AGE = new int?(int32_1);
                    tblProfile.CITY = str8;
                    tblProfile.LOCATION = str8;
                    tblProfile.DATE_OF_BIRTH = new DateTime?(new Utility().StringToDatetime(dateString1));
                    tblProfile.DATE_OF_JOINING = new DateTime?(new Utility().StringToDatetime(dateString2));
                    tblProfile.DESIGNATION = str9;
                    tblProfile.GENDER = str5;
                    tblProfile.ID_USER = user.ID_USER;
                    tblProfile.OFFICE_ADDRESS = str10;
                    tblProfile.REPORTING_MANAGER = str12;
                    this.db.SaveChanges();
                }
                string str16 = this.Request.Form["role_check"];
                string[] strArray = (string[])null;
                if (str16 != null)
                    strArray = str16.Split(',');
                DbSet<tbl_role_user_mapping> tblRoleUserMapping = this.db.tbl_role_user_mapping;
                Expression<Func<tbl_role_user_mapping, bool>> predicate = (Expression<Func<tbl_role_user_mapping, bool>>)(t => t.id_user == (int?)user.ID_USER);
                foreach (tbl_role_user_mapping entity in tblRoleUserMapping.Where<tbl_role_user_mapping>(predicate).ToList<tbl_role_user_mapping>())
                {
                    bool flag = true;
                    if (strArray != null)
                    {
                        foreach (string str17 in strArray)
                        {
                            int int32_4 = Convert.ToInt32(str17);
                            int? idCsstRole = entity.id_csst_role;
                            int valueOrDefault = idCsstRole.GetValueOrDefault();
                            if ((int32_4 == valueOrDefault ? (idCsstRole.HasValue ? 1 : 0) : 0) != 0)
                                flag = false;
                        }
                    }
                    if (flag)
                    {
                        this.db.tbl_role_user_mapping.Remove(entity);
                        this.db.SaveChanges();
                    }
                }
                if (strArray != null)
                {
                    foreach (string str18 in strArray)
                    {
                        int rid = Convert.ToInt32(str18);
                        if (this.db.tbl_role_user_mapping.Where<tbl_role_user_mapping>((Expression<Func<tbl_role_user_mapping, bool>>)(t => t.id_user == (int?)user.ID_USER && t.id_csst_role == (int?)rid)).FirstOrDefault<tbl_role_user_mapping>() == null)
                        {
                            this.db.tbl_role_user_mapping.Add(new tbl_role_user_mapping()
                            {
                                id_user = new int?(user.ID_USER),
                                id_csst_role = new int?(rid),
                                updated_date_time = new DateTime?(DateTime.Now),
                                status = "A"
                            });
                            this.db.SaveChanges();
                        }
                    }
                }
            }
            return (ActionResult)this.RedirectToAction("app_user_display");
        }

        public ActionResult display_cms_role()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int orgid = Convert.ToInt32(content.id_ORGANIZATION);
      int uid = Convert.ToInt32(content.ID_USER);
      tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>) (t => t.ID_USER == uid)).FirstOrDefault<tbl_cms_users>();
      List<CMSRole> cmsRoleList = new List<CMSRole>();
      List<tbl_cms_roles> tblCmsRolesList = new List<tbl_cms_roles>();
      string str = "";
      int? cmdUserType = tblCmsUsers.cmd_user_type;
      int num = 0;
      List<tbl_cms_roles> list1;
      if ((cmdUserType.GetValueOrDefault() == num ? (cmdUserType.HasValue ? 1 : 0) : 0) != 0)
      {
        list1 = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_cms_roles, string>((Expression<Func<tbl_cms_roles, string>>) (t => t.ROLENAME)).ToList<tbl_cms_roles>();
      }
      else
      {
        list1 = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ORGANIZATION == orgid && t.STATUS == "A")).OrderBy<tbl_cms_roles, string>((Expression<Func<tbl_cms_roles, string>>) (t => t.ROLENAME)).ToList<tbl_cms_roles>();
        str = " AND id_organization=" + (object) orgid + " ";
      }
      List<tbl_organization> list2 = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.STATUS == "A")).OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      foreach (tbl_cms_roles tblCmsRoles in list1)
      {
        CMSRole cmsRole = new CMSRole();
        List<tbl_cms_role_action> list3 = this.db.tbl_cms_role_action.SqlQuery("select * from tbl_cms_role_action where id_cms_role_action in (select id_cms_role_action from tbl_cms_role_action_mapping where id_cms_role=" + (object) tblCmsRoles.ID_ROLE + " " + str + " )").ToList<tbl_cms_role_action>();
        cmsRole.ROLE = tblCmsRoles;
        cmsRole.Action = list3;
        cmsRoleList.Add(cmsRole);
      }
      this.ViewData["cmsrole"] = (object) cmsRoleList;
      this.ViewData["orglist"] = (object) list2;
      if (orgid == 16)
        this.ViewData["type_flag"] = (object) "1";
      else
        this.ViewData["type_flag"] = (object) "0";
      this.ViewData["roles"] = (object) list1;

            //UserSession content1 = (UserSession)this.HttpContext.Session.Contents["UserSession"];

            //if (content1 != null)
            //{
            //    // Extract ID_USER and id_ORGANIZATION from the userSession object
            //    string ID_USER = content1.ID_USER;
            //    string id_ORGANIZATION = content1.id_ORGANIZATION;
            //    string Page = "display_cms_role";
            //    // Call AddUserDataLog method with extracted values
            //    new UserLogDetails().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
            //} 


            return (ActionResult) this.View();
    }

    public ActionResult add_cms_role()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      Convert.ToInt32(content.id_ORGANIZATION);
      this.ViewData["orglist"] = (object) this.db.tbl_organization.OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      List<tbl_cms_role_action> list = this.db.tbl_cms_role_action.Where<tbl_cms_role_action>((Expression<Func<tbl_cms_role_action, bool>>) (t => t.status == "A")).ToList<tbl_cms_role_action>();
      this.ViewData["sessionUser"] = (object) content;
      this.ViewData["cms_roles"] = (object) list;
      return (ActionResult) this.View();
    }

    public ActionResult add_cms_role_action()
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      string str1 = this.Request.Form["cms_role"];
      string str2 = this.Request.Form["description"];
      int int32_1 = Convert.ToInt32(this.Request.Form["select-org"]);
      string[] strArray = (string[]) null;
      string str3 = this.Request.Form["role_check"].ToString();
      tbl_cms_roles entity1 = new tbl_cms_roles();
      entity1.ROLENAME = str1;
      entity1.DESCRIPTION = str2;
      entity1.STATUS = "A";
      entity1.ID_ORGANIZATION = int32_1;
      entity1.UPDATED_DATE_TIME = DateTime.Now;
      this.db.tbl_cms_roles.Add(entity1);
      this.db.SaveChanges();
      if (!string.IsNullOrEmpty(str3))
        strArray = str3.Split(',');
      foreach (string str4 in strArray)
      {
        if (str4 != null)
        {
          tbl_cms_role_action_mapping entity2 = new tbl_cms_role_action_mapping();
          int int32_2 = Convert.ToInt32(str4);
          entity2.id_cms_role = new int?(entity1.ID_ROLE);
          entity2.id_cms_role_action = new int?(int32_2);
          entity2.id_organization = new int?(int32_1);
          entity2.status = "A";
          entity2.updated_date_time = new DateTime?(DateTime.Now);
          this.db.tbl_cms_role_action_mapping.Add(entity2);
          this.db.SaveChanges();
        }
      }
      return (ActionResult) this.RedirectToAction("display_cms_role");
    }

    public ActionResult edit_cms_role(string id)
    {
      int ids = Convert.ToInt32(id);
      CMSRole cmsRole = new CMSRole();
      tbl_cms_roles tblCmsRoles = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ROLE == ids)).FirstOrDefault<tbl_cms_roles>();
      cmsRole.ROLE = tblCmsRoles;
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      this.ViewData["orglist"] = (object) this.db.tbl_organization.OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>) (t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
      List<tbl_cms_role_action> list1 = this.db.tbl_cms_role_action.SqlQuery("select * from tbl_cms_role_action where id_cms_role_action in (select id_cms_role_action from tbl_cms_role_action_mapping where id_organization=" + (object) tblCmsRoles.ID_ORGANIZATION + " AND id_cms_role=" + (object) tblCmsRoles.ID_ROLE + " )").ToList<tbl_cms_role_action>();
      cmsRole.Action = list1;
      List<tbl_cms_role_action> list2 = this.db.tbl_cms_role_action.Where<tbl_cms_role_action>((Expression<Func<tbl_cms_role_action, bool>>) (t => t.status == "A")).ToList<tbl_cms_role_action>();
      this.ViewData["cmsrole"] = (object) cmsRole;
      this.ViewData["cms_roles"] = (object) list2;
      return (ActionResult) this.View();
    }

    public ActionResult updated_cms_role_action()
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int ids = Convert.ToInt32(this.Request.Form["id_role"]);
      int oids = Convert.ToInt32(this.Request.Form["select-org"]);
      string str1 = this.Request.Form["cms_role"];
      string str2 = this.Request.Form["description"];
      string[] strArray = (string[]) null;
      string str3 = this.Request.Form["role_check"].ToString();
      tbl_cms_roles roles = this.db.tbl_cms_roles.Where<tbl_cms_roles>((Expression<Func<tbl_cms_roles, bool>>) (t => t.ID_ROLE == ids)).FirstOrDefault<tbl_cms_roles>();
      if (roles != null)
      {
        roles.ROLENAME = str1;
        roles.DESCRIPTION = str2;
        roles.STATUS = "A";
        roles.ID_ORGANIZATION = oids;
        roles.UPDATED_DATE_TIME = DateTime.Now;
        this.db.SaveChanges();
        if (!string.IsNullOrEmpty(str3) && str3 != "0")
          strArray = str3.Split(',');
        DbSet<tbl_cms_role_action_mapping> roleActionMapping1 = this.db.tbl_cms_role_action_mapping;
        Expression<Func<tbl_cms_role_action_mapping, bool>> predicate1 = (Expression<Func<tbl_cms_role_action_mapping, bool>>) (t => t.id_cms_role == (int?) ids && t.id_organization == (int?) oids);
        foreach (tbl_cms_role_action_mapping roleActionMapping2 in roleActionMapping1.Where<tbl_cms_role_action_mapping>(predicate1).ToList<tbl_cms_role_action_mapping>())
        {
          roleActionMapping2.status = "C";
          this.db.SaveChanges();
        }
        if (strArray != null)
        {
          foreach (string str4 in strArray)
          {
            if (str4 != null)
            {
              int mno = Convert.ToInt32(str4);
              tbl_cms_role_action_mapping roleActionMapping3 = this.db.tbl_cms_role_action_mapping.Where<tbl_cms_role_action_mapping>((Expression<Func<tbl_cms_role_action_mapping, bool>>) (t => t.id_cms_role == (int?) roles.ID_ROLE && t.id_cms_role_action == (int?) mno && t.id_organization == (int?) oids)).FirstOrDefault<tbl_cms_role_action_mapping>();
              if (roleActionMapping3 != null)
              {
                roleActionMapping3.id_cms_role = new int?(roles.ID_ROLE);
                roleActionMapping3.id_cms_role_action = new int?(mno);
                roleActionMapping3.id_organization = new int?(oids);
                roleActionMapping3.status = "A";
                roleActionMapping3.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
              }
              else
              {
                this.db.tbl_cms_role_action_mapping.Add(new tbl_cms_role_action_mapping()
                {
                  id_cms_role = new int?(roles.ID_ROLE),
                  id_cms_role_action = new int?(mno),
                  id_organization = new int?(oids),
                  status = "A",
                  updated_date_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
              }
            }
          }
        }
        DbSet<tbl_cms_role_action_mapping> roleActionMapping4 = this.db.tbl_cms_role_action_mapping;
        Expression<Func<tbl_cms_role_action_mapping, bool>> predicate2 = (Expression<Func<tbl_cms_role_action_mapping, bool>>) (t => t.id_cms_role == (int?) roles.ID_ROLE && t.id_organization == (int?) oids && t.status == "C");
        foreach (tbl_cms_role_action_mapping entity in roleActionMapping4.Where<tbl_cms_role_action_mapping>(predicate2).ToList<tbl_cms_role_action_mapping>())
        {
          this.db.tbl_cms_role_action_mapping.Remove(entity);
          this.db.SaveChanges();
        }
      }
      return (ActionResult) this.RedirectToAction("display_cms_role");
    }

    public string check_cms_role_name(string uname)
    {
      uname = uname.Trim();
      return this.db.tbl_cms_roles.SqlQuery("select * from tbl_cms_roles where UPPER(ROLENAME) LIKE upper('" + uname + "')").FirstOrDefault<tbl_cms_roles>() != null ? "0" : "1";
    }

    public ActionResult createOrganization()
    {
      this.ViewData["select-industry"] = (object) this.db.tbl_industry.ToList<tbl_industry>();
      this.ViewData["select-business-type"] = (object) this.db.tbl_business_type.ToList<tbl_business_type>();
      return (ActionResult) this.View();
    }

    public ActionResult createOrganizationAction()
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<tbl_menu> men = new List<tbl_menu>();
      tbl_organization entity1 = new tbl_organization();
      entity1.ID_INDUSTRY = Convert.ToInt32(this.Request.Form["select-industry"].ToString());
      entity1.ID_BUSINESS_TYPE = Convert.ToInt32(this.Request.Form["select-businuss-type"].ToString());
      entity1.ORGANIZATION_NAME = this.Request.Form["organisation-name"].ToString();
      entity1.DESCRIPTION = this.Request.Form["organisation-desc"].ToString();
      entity1.CONTACT_NAME = this.Request.Form["organisation-contact"].ToString();
      entity1.CONTACTEMAIL = this.Request.Form["organisation-email"].ToString();
      entity1.CONTACTNUMBER = this.Request.Form["organisation-contact-no"].ToString();
      entity1.DEFAULT_EMAIL = this.Request.Form["default-email"].ToString();
      entity1.STATUS = "A";
      entity1.UPDATED_DATE_TIME = DateTime.Now;
      this.db.tbl_organization.Add(entity1);
      this.db.SaveChanges();
      int idOrganization1 = entity1.ID_ORGANIZATION;
      switch (Convert.ToInt32(this.Request.Form["menu_typ"].ToString()))
      {
        case 1:
          new addCMS_CategoryModel().insert_menu_type(idOrganization1, 1);
          break;
        case 2:
          int int32 = Convert.ToInt32(this.Request.Form["menu_coun"].ToString());
          for (int index = 1; index <= int32; ++index)
          {
            tbl_menu tblMenu = new tbl_menu();
            tblMenu.menu_name = this.Request.Form["menu_name" + (object) index].ToString();
            if (tblMenu.menu_name != null && !(tblMenu.menu_name == "") && tblMenu.menu_name != null)
            {
              tblMenu.menu_url = this.Request.Form["menu_url" + (object) index].ToString();
              if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
              {
                HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["menu_icon" + (object) index];
                string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["menu_icon" + (object) index].FileName);
                string str = System.Web.HttpContext.Current.Request["id"];
                if (file != null)
                {
                  if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/")))
                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/"));
                  string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/"), entity1.ID_ORGANIZATION.ToString() + "icon" + (object) index + extension);
                  file.SaveAs(filename);
                  tblMenu.menu_icon = entity1.ID_ORGANIZATION.ToString() + "icon" + (object) index + extension;
                }
                men.Add(tblMenu);
              }
            }
          }
          if (men != null)
          {
            new addCMS_CategoryModel().insert_menu(men, idOrganization1);
            new addCMS_CategoryModel().insert_menu_type(idOrganization1, 2);
            break;
          }
          break;
      }
      int idOrganization2 = entity1.ID_ORGANIZATION;
      List<ColorConfig> color = new List<ColorConfig>();
      switch (Convert.ToInt32(this.Request.Form["color_config_type"].ToString()))
      {
        case 0:
          new addCMS_CategoryModel().insert_color_config_flag(idOrganization1, 0);
          break;
        case 1:
          for (int index = 1; index <= 4; ++index)
          {
            ColorConfig colorConfig = new ColorConfig();
            colorConfig.config_type = index;
            colorConfig.id_organisation = idOrganization1;
            if (index == 1)
            {
              colorConfig.grid1_bk_color = this.Request.Form["1_grid1_bk_color"].ToString();
              colorConfig.grid1_text_color = this.Request.Form["1_grid1_text_color"].ToString();
            }
            if (index == 2)
            {
              colorConfig.grid1_bk_color = this.Request.Form["2_grid1_bk_color"].ToString();
              colorConfig.grid1_text_color = this.Request.Form["2_grid1_text_color"].ToString();
            }
            if (index == 3)
            {
              colorConfig.grid1_bk_color = this.Request.Form["3_grid1_bk_color"].ToString();
              colorConfig.grid1_text_color = this.Request.Form["3_grid1_text_color"].ToString();
              colorConfig.grid2_bk_color = this.Request.Form["3_grid2_bk_color"].ToString();
              colorConfig.grid2_text_color = this.Request.Form["3_grid2_text_color"].ToString();
            }
            if (index == 4)
            {
              colorConfig.grid1_bk_color = this.Request.Form["4_grid1_bk_color"].ToString();
              colorConfig.grid1_text_color = this.Request.Form["4_grid1_text_color"].ToString();
              colorConfig.grid2_bk_color = this.Request.Form["4_grid2_bk_color"].ToString();
              colorConfig.grid2_text_color = this.Request.Form["4_grid2_text_color"].ToString();
            }
            color.Add(colorConfig);
          }
          new addCMS_CategoryModel().insert_color_config(color);
          new addCMS_CategoryModel().insert_color_config_flag(idOrganization1, 1);
          break;
      }
      try
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["logo-btn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["logo-btn"].FileName);
          string str = System.Web.HttpContext.Current.Request["id"];
          if (file != null)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"), entity1.ID_ORGANIZATION.ToString() + extension);
            file.SaveAs(filename);
            entity1.LOGO = entity1.ID_ORGANIZATION.ToString() + extension;
            this.db.SaveChanges();
          }
        }
        WelcomeGif gif = new WelcomeGif();
        if (Convert.ToInt32(this.Request.Form["gif_type"].ToString()) == 0)
        {
          gif.gif = "default.gif";
          gif.flag = 0;
          gif.status = this.Request.Form["gif_status"].ToString();
        }
        else if (Convert.ToInt32(this.Request.Form["gif_type"].ToString()) == 1)
        {
          gif.flag = 1;
          gif.status = this.Request.Form["gif_status"].ToString();
          if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
          {
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["welcome-btn"];
            string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["welcome-btn"].FileName);
            string str = System.Web.HttpContext.Current.Request["id"];
            if (file != null && file.ContentLength > 0)
            {
              if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/WelcomeGif/")))
                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/WelcomeGif/"));
              string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/WelcomeGif/"), entity1.ID_ORGANIZATION.ToString() + extension);
              file.SaveAs(filename);
              gif.gif = entity1.ID_ORGANIZATION.ToString() + extension;
            }
          }
        }
        gif.id_org = idOrganization1;
        gif.created_date = DateTime.Now;
        gif.updated_date = DateTime.Now;
        new addCMS_CategoryModel().insert_welcomegif(gif);
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["banner-btn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["banner-btn"].FileName);
          string str1 = System.Web.HttpContext.Current.Request["id"];
          if (file != null && file.ContentLength > 0)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"), entity1.ID_ORGANIZATION.ToString() + extension);
            file.SaveAs(filename);
            tbl_organisation_banner entity2 = new tbl_organisation_banner();
            tbl_organisation_banner organisationBanner1 = entity2;
            int idOrganization3 = entity1.ID_ORGANIZATION;
            string str2 = idOrganization3.ToString() + extension;
            organisationBanner1.banner_name = str2;
            tbl_organisation_banner organisationBanner2 = entity2;
            idOrganization3 = entity1.ID_ORGANIZATION;
            string str3 = idOrganization3.ToString() + extension;
            organisationBanner2.Banner_path = str3;
            entity2.id_organisation = new int?(entity1.ID_ORGANIZATION);
            entity2.updated_date_time = new DateTime?(DateTime.Now);
            this.db.tbl_organisation_banner.Add(entity2);
            this.db.SaveChanges();
          }
        }
        if (entity1.ID_ORGANIZATION > 0)
        {
          this.db.tbl_cms_roles.Add(new tbl_cms_roles()
          {
            ID_ORGANIZATION = entity1.ID_ORGANIZATION,
            ROLENAME = "Admin_" + (object) entity1.ID_ORGANIZATION,
            STATUS = "A",
            UPDATED_DATE_TIME = DateTime.Now,
            DESCRIPTION = "administrator"
          });
          this.db.SaveChanges();
          this.db.tbl_cms_roles.Add(new tbl_cms_roles()
          {
            ID_ORGANIZATION = entity1.ID_ORGANIZATION,
            ROLENAME = "User_" + (object) entity1.ID_ORGANIZATION,
            STATUS = "A",
            UPDATED_DATE_TIME = DateTime.Now,
            DESCRIPTION = "User"
          });
          this.db.SaveChanges();
          this.db.tbl_role.Add(new tbl_role()
          {
            ID_ORGANIZATION = entity1.ID_ORGANIZATION,
            ROLENAME = "Admin_" + (object) entity1.ID_ORGANIZATION,
            STATUS = "A",
            UPDATEDTIME = DateTime.Now,
            DESCRIPTION = "administrator"
          });
          this.db.SaveChanges();
          this.db.tbl_role.Add(new tbl_role()
          {
            ID_ORGANIZATION = entity1.ID_ORGANIZATION,
            ROLENAME = "User_" + (object) entity1.ID_ORGANIZATION,
            STATUS = "A",
            UPDATEDTIME = DateTime.Now,
            DESCRIPTION = "User"
          });
          this.db.SaveChanges();
          this.db.tbl_csst_role.Add(new tbl_csst_role()
          {
            csst_role = "App User",
            id_organization = new int?(entity1.ID_ORGANIZATION),
            status = "A",
            updated_dated_time = new DateTime?(DateTime.Now)
          });
          this.db.SaveChanges();
        }
      }
      catch (Exception ex)
      {
      }
      return (ActionResult) this.RedirectToAction("displayOrganization", "Administrator");
    }

    public ActionResult editOrganization(string id)
    {
      if (id == null)
        return (ActionResult) this.RedirectToAction("Index");
      int int32_1 = Convert.ToInt32(id);
      tbl_organization tblOrganization = this.db.tbl_organization.Find(new object[1]
      {
        (object) int32_1
      });
      if (tblOrganization == null)
        return (ActionResult) this.RedirectToAction("DisplayOrganization", "Administrator");
      List<ColorConfig> colorConfigList = new List<ColorConfig>();
      WelcomeGif welcomeGif = new WelcomeGif();
      int int32_2 = Convert.ToInt32(id);
      int colorConfigFlag = new addCMS_CategoryModel().get_color_config_flag(int32_2);
      int gifFlag = new addCMS_CategoryModel().get_gif_flag(int32_2);
      if (colorConfigFlag == 1)
        colorConfigList = new addCMS_CategoryModel().get_color_config(int32_2);
      if (gifFlag == 1 || gifFlag == 0)
        welcomeGif = new addCMS_CategoryModel().get_welcomeGif(int32_2);
      this.ViewData["organization"] = (object) tblOrganization;
      this.ViewData["select-industry"] = (object) this.db.tbl_industry.ToList<tbl_industry>();
      List<tbl_business_type> list = this.db.tbl_business_type.ToList<tbl_business_type>();
      int int32_3 = Convert.ToInt32(id);
      List<tbl_menu> menu = new addCMS_CategoryModel().get_menu(int32_3);
      int menuType = new addCMS_CategoryModel().get_menu_type(int32_3);
      this.ViewData["color"] = (object) colorConfigList;
      this.ViewData["gif"] = (object) welcomeGif;
      this.ViewData["color_flag"] = (object) colorConfigFlag;
      this.ViewData["gif_flag"] = (object) gifFlag;
      this.ViewData["select-business-type"] = (object) list;
      this.ViewData["Logos"] = (object) new contentDashboardModel().getOrgLogo(int32_1);
      this.ViewData["Banner"] = (object) new contentDashboardModel().getOrgBanner(int32_1);
      this.ViewData["menu_type"] = (object) menuType;
      this.ViewData["menu"] = (object) menu;
      return (ActionResult) this.View();
    }

    public bool deleteMenu(int id)
    {
      new addCMS_CategoryModel().delete_Menu(id);
      return true;
    }

    public ActionResult editOrganizationAction()
    {
      tbl_organization organisation = this.db.tbl_organization.Find(new object[1]
      {
        (object) Convert.ToInt32(this.Request.Form["id_organization"])
      });
      int idOrganization = organisation.ID_ORGANIZATION;
      organisation.ID_INDUSTRY = Convert.ToInt32(this.Request.Form["select-industry"].ToString());
      organisation.ID_BUSINESS_TYPE = Convert.ToInt32(this.Request.Form["select-businuss-type"].ToString());
      organisation.ORGANIZATION_NAME = this.Request.Form["organisation-name"].ToString();
      organisation.DESCRIPTION = this.Request.Form["organisation-desc"].ToString();
      organisation.CONTACT_NAME = this.Request.Form["organisation-contact"].ToString();
      organisation.CONTACTEMAIL = this.Request.Form["organisation-email"].ToString();
      organisation.DEFAULT_EMAIL = this.Request.Form["default-email"].ToString();
      organisation.CONTACTNUMBER = this.Request.Form["organisation-contact-no"].ToString();
      organisation.STATUS = "A";
      organisation.UPDATED_DATE_TIME = DateTime.Now;
      this.db.SaveChanges();
      int int32_1 = Convert.ToInt32(this.Request.Form["color_flag"].ToString());
      int int32_2 = Convert.ToInt32(this.Request.Form["gif_flag"].ToString());
      List<ColorConfig> color = new List<ColorConfig>();
      if (int32_1 == 1 || int32_1 == 0)
      {
        switch (Convert.ToInt32(this.Request.Form["color_config_type"].ToString()))
        {
          case 0:
            new addCMS_CategoryModel().update_color_config_flag(idOrganization, 0);
            break;
          case 1:
            for (int index = 1; index <= 4; ++index)
            {
              ColorConfig colorConfig = new ColorConfig();
              colorConfig.id_organisation = idOrganization;
              colorConfig.config_type = index;
              if (index == 1)
              {
                colorConfig.grid1_bk_color = this.Request.Form[index.ToString() + "-grid1_bk_color"].ToString();
                colorConfig.grid1_text_color = this.Request.Form[index.ToString() + "-grid1_text_color"].ToString();
              }
              if (index == 2)
              {
                colorConfig.grid1_bk_color = this.Request.Form[index.ToString() + "-grid1_bk_color"].ToString();
                colorConfig.grid1_text_color = this.Request.Form[index.ToString() + "-grid1_text_color"].ToString();
              }
              if (index == 3)
              {
                colorConfig.grid1_bk_color = this.Request.Form[index.ToString() + "-grid1_bk_color"].ToString();
                colorConfig.grid1_text_color = this.Request.Form[index.ToString() + "-grid1_text_color"].ToString();
                colorConfig.grid2_bk_color = this.Request.Form[index.ToString() + "-grid2_bk_color"].ToString();
                colorConfig.grid2_text_color = this.Request.Form[index.ToString() + "-grid2_text_color"].ToString();
              }
              if (index == 4)
              {
                colorConfig.grid1_bk_color = this.Request.Form[index.ToString() + "-grid1_bk_color"].ToString();
                colorConfig.grid1_text_color = this.Request.Form[index.ToString() + "-grid1_text_color"].ToString();
                colorConfig.grid2_bk_color = this.Request.Form[index.ToString() + "-grid2_bk_color"].ToString();
                colorConfig.grid2_text_color = this.Request.Form[index.ToString() + "-grid2_text_color"].ToString();
              }
              color.Add(colorConfig);
            }
            new addCMS_CategoryModel().update_color_confif(color, idOrganization, 1);
            break;
        }
      }
      int int32_3 = Convert.ToInt32(this.Request.Form["menu_typ"].ToString());
      List<tbl_menu> menu = new List<tbl_menu>();
      switch (int32_3)
      {
        case 1:
          new addCMS_CategoryModel().update_menu_type(idOrganization, 1);
          new addCMS_CategoryModel().update_menu(idOrganization);
          break;
        case 2:
          int int32_4 = Convert.ToInt32(this.Request.Form["menu_coun"].ToString());
          int int32_5 = Convert.ToInt32(this.Request.Form["current_menu_count"].ToString());
          for (int index = 1; index <= int32_4; ++index)
          {
            tbl_menu tblMenu = new tbl_menu();
            tblMenu.menu_name = this.Request.Form["menu_name_" + (object) index].ToString();
            if (tblMenu.menu_name != null && !(tblMenu.menu_name == "") && tblMenu.menu_name != null)
            {
              tblMenu.menu_url = this.Request.Form["menu_url_" + (object) index].ToString();
              if (index > int32_5)
              {
                if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                  HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["menu_icon_" + (object) index];
                  string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["menu_icon_" + (object) index].FileName);
                  string str = System.Web.HttpContext.Current.Request["id"];
                  if (file != null)
                  {
                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/")))
                      Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/"));
                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/"), organisation.ID_ORGANIZATION.ToString() + "icon" + (object) index + extension);
                    file.SaveAs(filename);
                    tblMenu.menu_icon = organisation.ID_ORGANIZATION.ToString() + "icon" + (object) index + extension;
                  }
                }
              }
              else
              {
                if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                  HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["menu_icon_" + (object) index];
                  string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["menu_icon_" + (object) index].FileName);
                  string str = System.Web.HttpContext.Current.Request["id"];
                  if (file != null)
                  {
                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/")))
                      Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/"));
                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/MENUICON/"), organisation.ID_ORGANIZATION.ToString() + "icon" + (object) index + extension);
                    file.SaveAs(filename);
                    if (extension != "")
                      tblMenu.menu_icon = organisation.ID_ORGANIZATION.ToString() + "icon" + (object) index + extension;
                    else
                      tblMenu.menu_icon = "null";
                  }
                }
                tblMenu.id_menu = Convert.ToInt32(this.Request.Form["men_" + (object) index].ToString());
              }
              menu.Add(tblMenu);
            }
          }
          new addCMS_CategoryModel().update_menu(menu, idOrganization, int32_5);
          break;
      }
      try
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["logo-btn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["logo-btn"].FileName);
          string str = System.Web.HttpContext.Current.Request["id"];
          if (file != null && file.ContentLength > 0)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"), organisation.ID_ORGANIZATION.ToString() + extension);
            file.SaveAs(filename);
            organisation.LOGO = organisation.ID_ORGANIZATION.ToString() + extension;
            this.db.SaveChanges();
          }
        }
        WelcomeGif gif = new WelcomeGif();
        if (int32_2 == 1 || int32_2 == 0)
        {
          int int32_6 = Convert.ToInt32(this.Request.Form["gif_type"].ToString());
          string str1 = this.Request.Form["gif_status"].ToString();
          gif.id_org = idOrganization;
          gif.updated_date = DateTime.Now;
          if (int32_6 == 1)
          {
            if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
            {
              HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["welcome-btn"];
              string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["welcome-btn"].FileName);
              string str2 = System.Web.HttpContext.Current.Request["id"];
              if (file != null && file.ContentLength > 0)
              {
                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/WelcomeGif/")))
                  Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/WelcomeGif/"));
                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/WelcomeGif/"), organisation.ID_ORGANIZATION.ToString() + extension);
                file.SaveAs(filename);
                gif.gif = organisation.ID_ORGANIZATION.ToString() + extension;
              }
            }
            gif.flag = 1;
            gif.status = str1;
            new addCMS_CategoryModel().update_gif_flag(gif, idOrganization, 1);
          }
          if (int32_6 == 0)
          {
            gif.gif = "default.gif";
            gif.flag = 0;
            gif.status = str1;
            new addCMS_CategoryModel().update_gif_flag(gif, idOrganization, 0);
          }
        }
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["banner-btn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["banner-btn"].FileName);
          string str = System.Web.HttpContext.Current.Request["id"];
          if (file != null)
          {
            if (file.ContentLength > 0)
            {
              if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/")))
                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"));
              string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"), organisation.ID_ORGANIZATION.ToString() + extension);
              file.SaveAs(filename);
              tbl_organisation_banner organisationBanner = this.db.tbl_organisation_banner.Where<tbl_organisation_banner>((Expression<Func<tbl_organisation_banner, bool>>) (t => t.id_organisation == (int?) organisation.ID_ORGANIZATION)).FirstOrDefault<tbl_organisation_banner>();
              if (organisationBanner != null)
              {
                organisationBanner.banner_name = organisation.ID_ORGANIZATION.ToString() + extension;
                organisationBanner.Banner_path = organisation.ID_ORGANIZATION.ToString() + extension;
                organisationBanner.id_organisation = new int?(organisation.ID_ORGANIZATION);
                this.db.SaveChanges();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return (ActionResult) this.RedirectToAction("displayOrganization", "Administrator");
    }

    public ActionResult DisplayOrganization()
    {
      this.ViewData["select-organization"] = (object) this.db.tbl_organization.ToList<tbl_organization>();
      return (ActionResult) this.View();
    }
  }
}
