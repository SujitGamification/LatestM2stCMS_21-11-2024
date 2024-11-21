// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.csst_categoryController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace m2ostnext.Controllers
{
  public class csst_categoryController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index() => (ActionResult) this.View();

    [RoleAccessController(KEY = 3)]
    public ActionResult add_category(int? id)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add Category";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            //

            //List<tbl_language_master> languageList = new UserLanguageDetails().GetLanguageList();

            //this.ViewData["Language-list"] = languageList;



            //this.ViewData["Language-list"] = (object) this.db.tbl_language_master.SqlQuery("select * from tbl_language_master where id_organization=" + (object) Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " and status='A' ").ToList<tbl_language_master>();
            this.ViewData["category-list"] = (object) this.db.tbl_category_tiles.SqlQuery("select * from tbl_category_tiles where id_organization=" + (object) Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " and category_theme in (1,5,6) and status='A' order by tile_heading ").ToList<tbl_category_tiles>();
      return (ActionResult) this.View();
    }

    [RoleAccessController(KEY = 3)]
    public ActionResult LanguageAdd_category()
     {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Language Add Category";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            ////

            List<tbl_language_master> languageList = new UserLanguageDetails().GetLanguageList();

            // Store the language list in ViewData
            this.ViewData["Language-list"] = languageList;


            this.ViewData["category-list"] = (object)this.db.tbl_category_tiles.SqlQuery("select * from tbl_category_tiles where id_organization=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " and category_theme in (1,5,6) and status='A' order by tile_heading ").ToList<tbl_category_tiles>();

            return (ActionResult)this.View();
    }


        //language
        [RoleAccessController(KEY = 3)]
        public ActionResult add_language_category(FormCollection formCollection)
        {
            string NewUser = this.Request.Form["New_user_Auto"];
            string str1 = (string)null;
            string str2 = this.Request.Form["Category"];
            string str3 = str2.Replace(" ", "").Replace("&", "");
            string str4 = this.Request.Form["Description"];
            string[] strArray = (string[])null;
            int int32_1 = Convert.ToInt32(this.Request.Form["type-category"]);
            int num;
            if (int32_1 != 0)
            {
                num = Convert.ToInt32(this.Request.Form["parent-category"]);
                if (num != 0)
                {
                    string str5 = this.Request.Form["heading-check"].ToString();
                    if (!string.IsNullOrEmpty(str5))
                        strArray = str5.Split(',');
                }
            }
            else
                num = 0;
            int int32_2 = Convert.ToInt32(this.Request.Form["OrderId"]);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            tbl_category entity = new tbl_category();
            try
            {
                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
                    str1 = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
                    string str6 = System.Web.HttpContext.Current.Request["id"];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/")))
                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"));
                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str3 + str1);
                        file.SaveAs(filename);
                    }
                }
                int ID_CATEGORY;

                //using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                //{
                //    connection.Open();

                //    MySqlCommand cmd = new MySqlCommand();
                //    cmd.Connection = connection;
                //    cmd.CommandType = CommandType.Text;

                //    if (num == 0)
                //    {
                //        //cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen)";
                //        //cmd.Parameters.AddWithValue("@ID_PARENT", 0);
                //        //cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                //        cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) " +
                //      "VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen); " +
                //      "SELECT LAST_INSERT_ID();";
                //        cmd.Parameters.AddWithValue("@ID_PARENT", 0);
                //        cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                //    }
                //    else
                //    {
                //        //cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen)" +
                //        //   "SELECT LAST_INSERT_ID();";
                //        //cmd.Parameters.AddWithValue("@ID_PARENT", num);
                //        //cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                //        cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) " +
                //          "VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen); " +
                //          "SELECT LAST_INSERT_ID();";
                //        cmd.Parameters.AddWithValue("@ID_PARENT", num);
                //        cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                //    }

                //    string str7 = this.Request.Form["red_url"];
                //    if (int32_1 == 3)

                //        cmd.Parameters.AddWithValue("@CATEGORY_TYPE", int32_1);
                //    cmd.Parameters.AddWithValue("@ORDERID", int32_2);
                //    cmd.Parameters.AddWithValue("@CATEGORY_TYPE", int32_1);
                //    cmd.Parameters.AddWithValue("@SUB_HEADING", "");
                //    cmd.Parameters.AddWithValue("@CATEGORYNAME", str2);
                //    cmd.Parameters.AddWithValue("@DESCRIPTION", str4);
                //    cmd.Parameters.AddWithValue("@IMAGE_PATH", str3 + str1);
                //    cmd.Parameters.AddWithValue("@IMAGE_URL", str7);
                //    cmd.Parameters.AddWithValue("@STATUS", "A");
                //    cmd.Parameters.AddWithValue("@UPDATED_DATE_TIME", DateTime.Now);
                //    cmd.Parameters.AddWithValue("@ID_ORGANIZATION", Convert.ToInt32(content.id_ORGANIZATION));
                //    cmd.Parameters.AddWithValue("@SEARCH_MAX_COUNT", Convert.ToInt32(this.Request.Form["count-category"]));
                //    cmd.Parameters.AddWithValue("@COUNT_REQUIRED", Convert.ToInt32(this.Request.Form["click-category"]));
                //    cmd.Parameters.AddWithValue("@new_user_auto_assigen", NewUser);
                //    ID_CATEGORY = Convert.ToInt32(cmd.ExecuteScalar());
                //    //cmd.ExecuteNonQuery();
                //}


                //if (num != 0)
                //{
                //    if (strArray != null)
                //    {
                //        foreach (string str8 in strArray)
                //        {
                //            if (str8 != null)
                //            {
                //                this.db.tbl_category_associantion.Add(new tbl_category_associantion()
                //                {
                //                    id_category = new int?(ID_CATEGORY),
                //                    //id_category = new int?(entity.ID_CATEGORY),
                //                    id_category_heading = new int?(Convert.ToInt32(str8)),
                //                    id_category_tile = new int?(num),
                //                    status = "A",
                //                    category_order = new int?(int32_2),
                //                    updated_date_time = new DateTime?(DateTime.Now)
                //                });
                //                this.db.SaveChanges();
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                new contentDashboardModel().exception_log(ex);
            }


            return (ActionResult)this.RedirectToAction("display_category", (object)new
            {
                flag = 1
            });
        }






        [RoleAccessController(KEY = 3)]
    public ActionResult add_cms_category(FormCollection formCollection)
    {
      string NewUser = this.Request.Form["New_user_Auto"];
      string str1 = (string) null;
      string str2 = this.Request.Form["Category"];
      string str3 = str2.Replace(" ", "").Replace("&", "");
      string str4 = this.Request.Form["Description"];
      string[] strArray = (string[]) null;
      int int32_1 = Convert.ToInt32(this.Request.Form["type-category"]);
      int num;
      if (int32_1 != 0)
      {
        num = Convert.ToInt32(this.Request.Form["parent-category"]);
        if (num != 0)
        {
          string str5 = this.Request.Form["heading-check"].ToString();
          if (!string.IsNullOrEmpty(str5))
            strArray = str5.Split(',');
        }
      }
      else
        num = 0;
      int int32_2 = Convert.ToInt32(this.Request.Form["OrderId"]);
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      tbl_category entity = new tbl_category();
      try
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          str1 = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
          string str6 = System.Web.HttpContext.Current.Request["id"];
          if (file != null && file.ContentLength > 0)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str3 + str1);
            file.SaveAs(filename);
          }
        }
                int ID_CATEGORY;

                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    if (num == 0)
                    {
                        //cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen)";
                        //cmd.Parameters.AddWithValue("@ID_PARENT", 0);
                        //cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                        cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) " +
                      "VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen); " +
                      "SELECT LAST_INSERT_ID();";
                        cmd.Parameters.AddWithValue("@ID_PARENT", 0);
                        cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                    }
                    else
                    {
                        //cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen)" +
                        //   "SELECT LAST_INSERT_ID();";
                        //cmd.Parameters.AddWithValue("@ID_PARENT", num);
                        //cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                        cmd.CommandText = "INSERT INTO tbl_category (ID_PARENT, IS_PRIMARY, CATEGORY_TYPE, ORDERID, SUB_HEADING, CATEGORYNAME, DESCRIPTION, IMAGE_PATH, IMAGE_URL, STATUS, UPDATED_DATE_TIME, ID_ORGANIZATION, SEARCH_MAX_COUNT, COUNT_REQUIRED, new_user_auto_assigen) " +
                          "VALUES (@ID_PARENT, @IS_PRIMARY, @CATEGORY_TYPE, @ORDERID, @SUB_HEADING, @CATEGORYNAME, @DESCRIPTION, @IMAGE_PATH, @IMAGE_URL, @STATUS, @UPDATED_DATE_TIME, @ID_ORGANIZATION, @SEARCH_MAX_COUNT, @COUNT_REQUIRED, @new_user_auto_assigen); " +
                          "SELECT LAST_INSERT_ID();";
                        cmd.Parameters.AddWithValue("@ID_PARENT", num);
                        cmd.Parameters.AddWithValue("@IS_PRIMARY", 1);
                    }

                    string str7 = this.Request.Form["red_url"];
                    if (int32_1 == 3)

                    cmd.Parameters.AddWithValue("@CATEGORY_TYPE", int32_1);
                    cmd.Parameters.AddWithValue("@ORDERID", int32_2);
                    cmd.Parameters.AddWithValue("@CATEGORY_TYPE", int32_1);
                    cmd.Parameters.AddWithValue("@SUB_HEADING", "");
                    cmd.Parameters.AddWithValue("@CATEGORYNAME", str2);
                    cmd.Parameters.AddWithValue("@DESCRIPTION", str4);
                    cmd.Parameters.AddWithValue("@IMAGE_PATH", str3 + str1);
                    cmd.Parameters.AddWithValue("@IMAGE_URL", str7);
                    cmd.Parameters.AddWithValue("@STATUS", "A");
                    cmd.Parameters.AddWithValue("@UPDATED_DATE_TIME", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ID_ORGANIZATION", Convert.ToInt32(content.id_ORGANIZATION));
                    cmd.Parameters.AddWithValue("@SEARCH_MAX_COUNT", Convert.ToInt32(this.Request.Form["count-category"]));
                    cmd.Parameters.AddWithValue("@COUNT_REQUIRED", Convert.ToInt32(this.Request.Form["click-category"]));
                    cmd.Parameters.AddWithValue("@new_user_auto_assigen", NewUser);
                    ID_CATEGORY = Convert.ToInt32(cmd.ExecuteScalar());
                    //cmd.ExecuteNonQuery();
                }

                //if (num == 0)
                //{
                //  entity.ID_PARENT = new int?(0);
                //  entity.IS_PRIMARY = new int?(1);
                //}
                //else
                //{
                //  entity.ID_PARENT = new int?(num);
                //  entity.IS_PRIMARY = new int?(1);
                //}
                //string str7 = "";
                //if (int32_1 == 3)
                //  str7 = this.Request.Form["red_url"];
                //entity.CATEGORY_TYPE = new int?(int32_1);
                //entity.ORDERID = new int?(int32_2);
                //entity.SUB_HEADING = "";
                //entity.CATEGORYNAME = str2;
                //entity.DESCRIPTION = str4;
                //entity.IMAGE_PATH = str3 + str1;
                //entity.IMAGE_URL = str7;
                //entity.STATUS = "A";
                //entity.UPDATED_DATE_TIME = DateTime.Now;
                //entity.ID_ORGANIZATION = Convert.ToInt32(content.id_ORGANIZATION);
                //entity.SEARCH_MAX_COUNT = new int?(Convert.ToInt32(this.Request.Form["count-category"]));
                //entity.COUNT_REQUIRED = new int?(Convert.ToInt32(this.Request.Form["click-category"]));
                //entity.new_user_auto_assigen = NewUser;
                //this.db.tbl_category.Add(entity);
                //this.db.SaveChanges();
                if (num != 0)
                {
                    if (strArray != null)
                    {
                        foreach (string str8 in strArray)
                        {
                            if (str8 != null)
                            {
                                this.db.tbl_category_associantion.Add(new tbl_category_associantion()
                                {
                                    id_category = new int?(ID_CATEGORY),
                                    //id_category = new int?(entity.ID_CATEGORY),
                                    id_category_heading = new int?(Convert.ToInt32(str8)),
                                    id_category_tile = new int?(num),
                                    status = "A",
                                    category_order = new int?(int32_2),
                                    updated_date_time = new DateTime?(DateTime.Now)
                                });
                                this.db.SaveChanges();
                            }
                        }
                    }
                }
            }
      catch (Exception ex)
      {
        new contentDashboardModel().exception_log(ex);
      }


      return (ActionResult) this.RedirectToAction("display_category", (object) new
      {
        flag = 1
      });
    }

    [RoleAccessController(KEY = 3)]
    public ActionResult edit_category(int? id)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Edit Category";
                string Id_assessment = null;
                string Id_category = Convert.ToString(id);
                string Id_operation = null;
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            if (!id.HasValue)
        return (ActionResult) this.RedirectToAction("display_category");
      int orgid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      this.ViewData["category-list"] = (object) this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) orgid && t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      tbl_category model = this.db.tbl_category.Find(new object[1]
      {
        (object) id
      });
      List<tbl_category_associantion> list = this.db.tbl_category_associantion.SqlQuery("select * from tbl_category_associantion where id_category=" + (object) model.ID_CATEGORY).ToList<tbl_category_associantion>();
      tbl_category_tiles tblCategoryTiles = new tbl_category_tiles();
      List<tbl_category_heading> tblCategoryHeadingList = new List<tbl_category_heading>();
      foreach (tbl_category_associantion categoryAssociantion in list)
      {
        tbl_category_heading tblCategoryHeading = this.db.tbl_category_heading.Find(new object[1]
        {
          (object) categoryAssociantion.id_category_heading
        });
        tblCategoryTiles = this.db.tbl_category_tiles.Find(new object[1]
        {
          (object) categoryAssociantion.id_category_tile
        });
        tblCategoryHeading.Heading_title = tblCategoryHeading.Heading_title + "( Tile : " + tblCategoryTiles.tile_heading + ")";
        tblCategoryHeadingList.Add(tblCategoryHeading);
      }
      if (model == null)
        return (ActionResult) this.RedirectToAction("display_category");
      this.ViewData["category-tile"] = (object) tblCategoryTiles;
      this.ViewData["category-heading"] = (object) tblCategoryHeadingList;
      return (ActionResult) this.View((object) model);
    }

    [RoleAccessController(KEY = 3)]
    //public ActionResult edit_cms_category(FormCollection formCollection)
    //{
    //  string[] strArray1 = (string[]) null;
    //  string[] strArray2 = (string[]) null;
    //  int categoryid = Convert.ToInt32(this.Request.Form["ID_Category"]);
    //  string str1 = this.Request.Form["Category"];
    //  string str2 = this.Request.Form["Description"];
    //  int int32_1 = Convert.ToInt32(this.Request.Form["OrderId"]);
    //  int int32_2 = Convert.ToInt32(this.Request.Form["type-category"]);
    //  int int32_3 = int32_2 <= 0 ? 0 : Convert.ToInt32(this.Request.Form["parent-category"]);
    //  string str3 = "";
    //  UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
    //  tbl_category temp = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_CATEGORY == categoryid)).FirstOrDefault<tbl_category>();
    //  string str4 = str1.Replace(" ", "").Replace("&", "");
    //  int num = str1 == temp.CATEGORYNAME ? 1 : 0;
    //  try
    //  {
    //    if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
    //    {
    //      HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
    //      string extension = Path.GetExtension(file.FileName);
    //      string str5 = System.Web.HttpContext.Current.Request["id"];
    //      if (file != null && file.ContentLength > 0)
    //      {
    //        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/")))
    //          Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"));
    //        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str4 + extension);
    //        string path = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str4 + extension);
    //        if (System.IO.File.Exists(path))
    //          System.IO.File.Delete(path);
    //        file.SaveAs(filename);
    //        temp.IMAGE_PATH = str4 + extension;
    //      }
    //    }
    //    if (int32_3 != 0)
    //    {
    //      string str6 = Convert.ToString(this.Request.Form["heading-check"]);
    //      if (!string.IsNullOrEmpty(str6))
    //        strArray1 = str6.Split(',');
    //      string str7 = Convert.ToString(this.Request.Form["heading-check-edit"]);
    //      if (!string.IsNullOrEmpty(str7))
    //        strArray2 = str7.Split(',');
    //      List<tbl_category_associantion> list1 = this.db.tbl_category_associantion.SqlQuery("SELECT distinct * FROM tbl_category_associantion where  id_category =" + (object) temp.ID_CATEGORY + " ").ToList<tbl_category_associantion>();
    //      List<string> stringList = new List<string>();
    //      if (list1 != null)
    //      {
    //        foreach (tbl_category_associantion categoryAssociantion in list1)
    //        {
    //          categoryAssociantion.status = "C";
    //          this.db.SaveChanges();
    //        }
    //        foreach (tbl_category_associantion categoryAssociantion in list1)
    //        {
    //          if (strArray2 != null)
    //          {
    //            foreach (string str8 in strArray2)
    //            {
    //              if (str8 == categoryAssociantion.id_category_heading.ToString())
    //              {
    //                categoryAssociantion.status = "A";
    //                this.db.SaveChanges();
    //              }
    //            }
    //          }
    //        }
    //      }
    //      List<tbl_category_associantion> list2 = this.db.tbl_category_associantion.SqlQuery("SELECT distinct * FROM tbl_category_associantion where  id_category =" + (object) temp.ID_CATEGORY + " ").ToList<tbl_category_associantion>();
    //      if (list2 != null)
    //      {
    //        foreach (tbl_category_associantion entity in list2)
    //        {
    //          if (entity.status == "C")
    //          {
    //            this.db.tbl_category_associantion.Remove(entity);
    //            this.db.SaveChanges();
    //          }
    //        }
    //      }
    //      if (strArray1 != null)
    //      {
    //        foreach (string str9 in strArray1)
    //        {
    //          if (str9 != null)
    //          {
    //            this.db.tbl_category_associantion.Add(new tbl_category_associantion()
    //            {
    //              id_category = new int?(temp.ID_CATEGORY),
    //              id_category_heading = new int?(Convert.ToInt32(str9)),
    //              id_category_tile = new int?(int32_3),
    //              status = "A",
    //              category_order = new int?(int32_1),
    //              updated_date_time = new DateTime?(DateTime.Now)
    //            });
    //            this.db.SaveChanges();
    //          }
    //        }
    //      }
    //    }
    //    if (int32_3 == 0)
    //    {
    //      temp.ID_PARENT = new int?(0);
    //      temp.IS_PRIMARY = new int?(1);
    //      DbSet<tbl_category_associantion> categoryAssociantion = this.db.tbl_category_associantion;
    //      Expression<Func<tbl_category_associantion, bool>> predicate = (Expression<Func<tbl_category_associantion, bool>>) (t => t.id_category == (int?) temp.ID_CATEGORY);
    //      foreach (tbl_category_associantion entity in categoryAssociantion.Where<tbl_category_associantion>(predicate).ToList<tbl_category_associantion>())
    //      {
    //        this.db.tbl_category_associantion.Remove(entity);
    //        this.db.SaveChanges();
    //      }
    //    }
    //    else
    //    {
    //      temp.ID_PARENT = new int?(int32_3);
    //      temp.IS_PRIMARY = new int?(1);
    //    }
    //            try
    //            {
    //                if (int32_2 == 3)
    //                 str3 = this.Request.Form["red_url"];
    //                temp.SEARCH_MAX_COUNT = new int?(Convert.ToInt32(this.Request.Form["count-category"]));
    //                temp.COUNT_REQUIRED = new int?(Convert.ToInt32(this.Request.Form["click-category"]));
    //                temp.ORDERID = new int?(int32_1);
    //                temp.CATEGORY_TYPE = new int?(int32_2);
    //                temp.CATEGORYNAME = str1;
    //                temp.DESCRIPTION = str2;
    //                temp.STATUS = "A";
    //                temp.IMAGE_URL = str3;
    //                temp.ID_ORGANIZATION = Convert.ToInt32(content.id_ORGANIZATION);
    //                temp.UPDATED_DATE_TIME = DateTime.Now;
    //                temp.ID_CATEGORY = categoryid;
    //                this.db.SaveChanges();
    //            }
    //            catch (DbEntityValidationException ex)
    //            {
    //                foreach (var entityValidationErrors in ex.EntityValidationErrors)
    //                {
    //                    foreach (var validationError in entityValidationErrors.ValidationErrors)
    //                    {
    //                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
    //                    }
    //                }
    //                throw; // Optionally rethrow the exception if needed
    //            }

    //        }
    //  catch (Exception ex)
    //  {
    //    new contentDashboardModel().exception_log(ex);
    //  }
    //  return (ActionResult) this.RedirectToAction("display_category");
    //}
        public ActionResult edit_cms_category(FormCollection formCollection)
        {
            int categoryId = Convert.ToInt32(Request.Form["ID_Category"]);
            string categoryName = Request.Form["Category"];
            string description = Request.Form["Description"];
            int orderId = Convert.ToInt32(Request.Form["OrderId"]);
            int categoryType = Convert.ToInt32(Request.Form["type-category"]);
            int parentCategoryId = categoryType > 0 ? Convert.ToInt32(Request.Form["parent-category"]) : 0;
            string imageUrl = "";
            string imagePath = "";
            UserSession content = (UserSession)HttpContext.Session["UserSession"];
            string str4 = categoryName.Replace(" ", "").Replace("&", "");

            try
            {
                // Handle file upload
                if (Request.Files.AllKeys.Contains("uploadBtn"))
                {
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];    
                    if (file != null && file.ContentLength > 0)
                    {
                        //string fileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "").Replace("&", "");
                        string extension = Path.GetExtension(file.FileName);
                        //string uploadPath = Server.MapPath($"~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/{content.id_ORGANIZATION}/");
                        //Directory.CreateDirectory(uploadPath);

                        //imagePath = Path.Combine(uploadPath, fileName + extension);
                        //if (System.IO.File.Exists(imagePath))
                        //    System.IO.File.Delete(imagePath);

                        //file.SaveAs(imagePath);
                        //imageUrl = fileName + extension;

                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/")))
                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"));
                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str4 + extension);
                        string path = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str4 + extension);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        file.SaveAs(filename);
                        imageUrl = str4 + extension;
                    }



                }

                // Update database using ADO.NET
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Connection = connection;

                        // Update tbl_category
                        command.CommandText = @"
        UPDATE tbl_category
        SET CATEGORYNAME = @CategoryName,
            DESCRIPTION = @Description,
            ORDERID = @OrderId,
            CATEGORY_TYPE = @CategoryType,
            ID_PARENT = @ParentCategoryId,
            IMAGE_PATH = @ImageUrl,
            SEARCH_MAX_COUNT = @SearchMaxCount,
            COUNT_REQUIRED = @CountRequired,
            UPDATED_DATE_TIME = @UpdatedDateTime,
            STATUS = @Status,
            IMAGE_URL = @ImageUrl,
            ID_ORGANIZATION = @OrganizationId
        WHERE ID_CATEGORY = @CategoryId";

                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                        command.Parameters.AddWithValue("@CategoryName", categoryName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@CategoryType", categoryType);
                        command.Parameters.AddWithValue("@ParentCategoryId", parentCategoryId);
                        command.Parameters.AddWithValue("@ImageUrl", imageUrl);
                        command.Parameters.AddWithValue("@SearchMaxCount", Convert.ToInt32(Request.Form["count-category"]));
                        command.Parameters.AddWithValue("@CountRequired", Convert.ToInt32(Request.Form["click-category"]));
                        command.Parameters.AddWithValue("@UpdatedDateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@Status", "A");
                        command.Parameters.AddWithValue("@OrganizationId", content.id_ORGANIZATION);

                        command.ExecuteNonQuery();
                    }

                    // Update tbl_category_associantion
                    // Clear existing associations
                    using (MySqlCommand deleteCommand = new MySqlCommand())
                    {
                        deleteCommand.Connection = connection;
                        deleteCommand.CommandText = "DELETE FROM tbl_category_associantion WHERE id_category = @CategoryId";
                        deleteCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                        deleteCommand.ExecuteNonQuery();
                    }

                    string[] headingCheck = Request.Form["heading-check"]?.Split(',') ?? Array.Empty<string>();

                    foreach (string headingId in headingCheck)
                    {
                        if (!string.IsNullOrEmpty(headingId))
                        {
                            using (MySqlCommand insertCommand = new MySqlCommand())
                            {
                                insertCommand.Connection = connection;
                                insertCommand.CommandText = @"
                INSERT INTO tbl_category_associantion 
                (id_category, id_category_heading, id_category_tile, status, category_order, updated_date_time)
                VALUES (@CategoryId, @HeadingId, @TileId, @Status, @CategoryOrder, @UpdatedDateTime)";

                                insertCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                                insertCommand.Parameters.AddWithValue("@HeadingId", Convert.ToInt32(headingId));
                                insertCommand.Parameters.AddWithValue("@TileId", parentCategoryId);
                                insertCommand.Parameters.AddWithValue("@Status", "A");
                                insertCommand.Parameters.AddWithValue("@CategoryOrder", orderId);
                                insertCommand.Parameters.AddWithValue("@UpdatedDateTime", DateTime.Now);

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Log the exception (assuming contentDashboardModel().exception_log(ex) exists)
                new contentDashboardModel().exception_log(ex);
            }

            return RedirectToAction("display_category");
        }


        [RoleAccessController(KEY = 4)]
    public ActionResult display_category(int flag = 0)
    {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "List Category";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_category> list = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_ORGANIZATION == oid && t.STATUS == "A")).OrderBy<tbl_category, string>((Expression<Func<tbl_category, string>>) (t => t.CATEGORYNAME)).ToList<tbl_category>();
      string str = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/" + (object) oid + "/";
      this.ViewData["accessflag"] = (object) new RoleBasedAccess().checkAccess(content.action, 3);
      this.ViewData["Category"] = (object) list;
      this.ViewData["CATIMG"] = (object) str;
      this.ViewData[nameof (flag)] = (object) flag;
      return (ActionResult) this.View();
    }

    public ActionResult delete_category(int? id)
    {
      if (!id.HasValue)
        return (ActionResult) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      this.ViewData["category"] = (object) this.db.tbl_category.Find(new object[1]
      {
        (object) id
      });
      return (ActionResult) this.View();
    }

    public ActionResult delete_cms_category(FormCollection formCollection) => new addCMS_CategoryModel().delete_cms_category(this.Request.Form["ID_Category"]).Equals("TRUE") ? (ActionResult) this.RedirectToAction("display_category") : (ActionResult) this.RedirectToAction("delete_category");

    public string getSubHeader(string id)
    {
      string subHeader = "";
      List<tbl_category_heading> list = this.db.tbl_category_heading.SqlQuery("SELECT distinct * FROM tbl_category_heading where id_category_tiles =" + (object) Convert.ToInt32(id) + " and status='A'  order by heading_order ").ToList<tbl_category_heading>();
      if (list.Count > 0)
      {
        foreach (tbl_category_heading tblCategoryHeading in list)
          subHeader = subHeader + " <div class=\"checkbox\"><label><input class=\"checkbox checkbox-inline\" type=\"checkbox\" name=\"heading-check\" value=\"" + (object) tblCategoryHeading.id_category_heading + "\" />" + tblCategoryHeading.Heading_title + "</label></div>  ";
      }
      else
        subHeader = "0";
      return subHeader;
    }

    public string getSubHeaderCheck(string id, string cat)
    {
      string subHeaderCheck = "";
      int int32_1 = Convert.ToInt32(id);
      int int32_2 = Convert.ToInt32(cat);
      List<tbl_category_heading> list1 = this.db.tbl_category_heading.SqlQuery("SELECT distinct * FROM tbl_category_heading where id_category_tiles =" + (object) int32_1 + " and status='A' ").ToList<tbl_category_heading>();
      if (list1.Count > 0)
      {
        foreach (tbl_category_heading tblCategoryHeading in list1)
        {
          List<tbl_category_associantion> list2 = this.db.tbl_category_associantion.SqlQuery("select * from tbl_category_associantion where id_category_tile=" + (object) tblCategoryHeading.id_category_tiles + " AND id_category=" + (object) int32_2).ToList<tbl_category_associantion>();
          if (list2.Count > 0)
          {
            foreach (tbl_category_associantion categoryAssociantion in list2)
            {
              int? nullable = categoryAssociantion.id_category;
              int num1 = int32_2;
              if ((nullable.GetValueOrDefault() == num1 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
              {
                nullable = categoryAssociantion.id_category_tile;
                int num2 = int32_1;
                if ((nullable.GetValueOrDefault() == num2 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                {
                  nullable = categoryAssociantion.id_category_heading;
                  int idCategoryHeading = tblCategoryHeading.id_category_heading;
                  if ((nullable.GetValueOrDefault() == idCategoryHeading ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                  {
                    subHeaderCheck = subHeaderCheck + " <div class=\"checkbox\"><label><input class=\"checkbox checkbox-inline\" checked disabled type=\"checkbox\"  value=\"" + (object) tblCategoryHeading.id_category_heading + "\" />" + tblCategoryHeading.Heading_title + "</label></div>  ";
                    continue;
                  }
                }
              }
              subHeaderCheck = subHeaderCheck + " <div class=\"checkbox\"><label><input class=\"checkbox checkbox-inline\" type=\"checkbox\" name=\"heading-check\" value=\"" + (object) tblCategoryHeading.id_category_heading + "\" />" + tblCategoryHeading.Heading_title + "</label></div>  ";
            }
          }
          else
            subHeaderCheck = subHeaderCheck + " <div class=\"checkbox\"><label><input class=\"checkbox checkbox-inline\" type=\"checkbox\" name=\"heading-check\" value=\"" + (object) tblCategoryHeading.id_category_heading + "\" />" + tblCategoryHeading.Heading_title + "</label></div>  ";
        }
      }
      else
        subHeaderCheck = "0";
      return subHeaderCheck;
    }

    public string getSelectedSubHeader(string id, string cat)
    {
      string selectedSubHeader = "";
      List<tbl_category_heading> list = this.db.tbl_category_heading.SqlQuery("SELECT distinct * FROM tbl_category_heading where  id_category_heading in(select id_category_heading from tbl_category_associantion where id_category_tiles =" + (object) Convert.ToInt32(id) + " and id_category =" + cat + ") and  status='A'  order by heading_order ").ToList<tbl_category_heading>();
      if (list.Count > 0)
      {
        foreach (tbl_category_heading tblCategoryHeading in list)
          selectedSubHeader = selectedSubHeader + " <div class=\"checkbox\"><label><input class=\"checkbox checkbox-inline\" checked type=\"checkbox\" name=\"heading-check-edit\" value=\"" + (object) tblCategoryHeading.id_category_heading + "\" />" + tblCategoryHeading.Heading_title + "</label></div>  ";
      }
      else
        selectedSubHeader = "0";
      return selectedSubHeader;
    }

    [RoleAccessController(KEY = 3)]
    public ActionResult add_category_tiles()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add Landing Page Tiles";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            return (ActionResult)this.View();
        }
            
          

    public ActionResult add_cms_category_tiles(FormCollection col)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      string str1 = this.Request.Form["category-tile"];
      string str2 = this.Request.Form["select-tile-type"];
      int int32 = Convert.ToInt32(this.Request.Form["category-order"]);
      string str3 = "";
      if (str2 == "7")
        str3 = this.Request.Form["red_url"];
      tbl_category_tiles entity = new tbl_category_tiles();
      entity.tile_heading = str1;
      entity.status = "A";
      entity.category_theme = new int?(Convert.ToInt32(str2));
      entity.id_organization = new int?(Convert.ToInt32(content.id_ORGANIZATION));
      entity.updated_date_time = new DateTime?(DateTime.Now);
      entity.image_url = str3;
      entity.category_order = new int?(int32);
      this.db.tbl_category_tiles.Add(entity);
      this.db.SaveChanges();
      try
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
          string str4 = System.Web.HttpContext.Current.Request["id"];
          if (file != null)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/Tiles/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/Tiles/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/Tiles/"), entity.id_category_tiles.ToString() + extension);
            file.SaveAs(filename);
            entity.tile_image = entity.id_category_tiles.ToString() + extension;
            this.db.SaveChanges();
          }
        }
      }
      catch (Exception ex)
      {
        new contentDashboardModel().exception_log(ex);
      }
      return (ActionResult) this.RedirectToAction("display_category_tiles", (object) new
      {
        flag = 1
      });
    }

    [RoleAccessController(KEY = 3)]
    public ActionResult edit_category_tiles(string id)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Edit Landing Page Title";
                string Id_assessment = id;
                string Id_category = null;
                string Id_operation = null; 
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment,Id_category,Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            this.ViewData["tiles"] = (object) this.db.tbl_category_tiles.Find(new object[1]
      {
        (object) Convert.ToInt32(id)
      });
      this.ViewData["vpath"] = (object) (ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/Tiles/");
      return (ActionResult) this.View();
    }

    [RoleAccessController(KEY = 4)]
    public ActionResult display_category_tiles(int flag = 0)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "List Landing Page Tiles";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int orgid = Convert.ToInt32(content.id_ORGANIZATION);
      List<int> intList = new List<int>();
      List<tbl_category_tiles> list = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) orgid && t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      foreach (tbl_category_tiles tblCategoryTiles in list)
      {
        int num = this.db.tbl_category_associantion.SqlQuery("select * from tbl_category_associantion where status='A' AND id_category_tile =" + (object) tblCategoryTiles.id_category_tiles).Count<tbl_category_associantion>();
        intList.Add(num);
      }
      this.ViewData["accessflag"] = (object) new RoleBasedAccess().checkAccess(content.action, 3);
      this.ViewData["counter"] = (object) intList;
      this.ViewData["vpath"] = (object) (ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/Tiles/");
      this.ViewData["category-tile"] = (object) list;
      this.ViewData[nameof (flag)] = (object) flag;
      return (ActionResult) this.View();
    }

    public ActionResult update_cms_category_tiles(FormCollection col)
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int int32_1 = Convert.ToInt32(this.Request.Form["id_category_tiles"]);
      string str1 = this.Request.Form["category-tile"];
      string str2 = this.Request.Form["select-tile-type"];
      string str3 = "";
      if (str2 == "7")
        str3 = this.Request.Form["red_url"];
      tbl_category_tiles tblCategoryTiles = this.db.tbl_category_tiles.Find(new object[1]
      {
        (object) int32_1
      });
      tblCategoryTiles.tile_heading = str1;
      tblCategoryTiles.category_theme = new int?(Convert.ToInt32(str2));
      int int32_2 = Convert.ToInt32(this.Request.Form["category-order"]);
      tblCategoryTiles.category_order = new int?(int32_2);
      tblCategoryTiles.image_url = str3;
      this.db.SaveChanges();
      try
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file1 = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          HttpPostedFile file2 = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          if (file2.ContentLength > 0)
          {
            string extension = Path.GetExtension(file2.FileName);
            FileInfo fileInfo = new FileInfo(Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/Tiles/"), tblCategoryTiles.id_category_tiles.ToString() + extension));
            if (fileInfo.Exists)
              fileInfo.Delete();
            string str4 = System.Web.HttpContext.Current.Request["id"];
            if (file1 != null)
            {
              if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/Tiles/")))
                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/Tiles/"));
              string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/Tiles/"), tblCategoryTiles.id_category_tiles.ToString() + extension);
              file1.SaveAs(filename);
              tblCategoryTiles.tile_image = tblCategoryTiles.id_category_tiles.ToString() + extension;
              this.db.SaveChanges();
            }
          }
        }
      }
      catch (Exception ex)
      {
        new contentDashboardModel().exception_log(ex);
      }
      return (ActionResult) this.RedirectToAction("display_category_tiles");
    }

    public string delete_category_tile(string id)
    {
      int ids = Convert.ToInt32(id);
      tbl_category_tiles tiles = this.db.tbl_category_tiles.Find(new object[1]
      {
        (object) ids
      });
      tbl_category_associantion entity = this.db.tbl_category_associantion.Where<tbl_category_associantion>((Expression<Func<tbl_category_associantion, bool>>) (t => t.id_category_tile == (int?) tiles.id_category_tiles && t.status == "A")).FirstOrDefault<tbl_category_associantion>();
      if (entity != null)
      {
        this.db.tbl_category_associantion.Remove(entity);
        this.db.SaveChanges();
      }
      this.db.tbl_category_tiles.Remove(tiles);
      this.db.SaveChanges();
      string str = "0";
      if (this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_category_tiles == ids)).FirstOrDefault<tbl_category_tiles>() == null)
        str = "1";
      return str;
    }

    [RoleAccessController(KEY = 3)]
    public ActionResult add_category_headers()
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add Category Heading";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int ids = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      this.ViewData["tiles"] = (object) this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) ids & t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      return (ActionResult) this.View();
    }

    public ActionResult add_cms_category_headers()
    {
      Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int int32_1 = Convert.ToInt32(this.Request.Form["select-category-tile"]);
      string str = this.Request.Form["category-heading"].ToString();
      int int32_2 = Convert.ToInt32(this.Request.Form["category-order"]);
      this.db.tbl_category_heading.Add(new tbl_category_heading()
      {
        Heading_title = str,
        id_category_tiles = new int?(int32_1),
        heading_order = new int?(int32_2),
        status = "A",
        updated_date_time = new DateTime?(DateTime.Now)
      });
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("display_category_heading", (object) new
      {
        flag = 1
      });
    }

    [RoleAccessController(KEY = 4)]
    public ActionResult display_category_heading(int flag = 0)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "List Category Heading";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int ids = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_category_tiles> list = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) ids & t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      List<tbl_category_heading> tblCategoryHeadingList = new List<tbl_category_heading>();
      List<int> intList = new List<int>();
      foreach (tbl_category_tiles tblCategoryTiles in list)
      {
        tbl_category_tiles item = tblCategoryTiles;
        IQueryable<tbl_category_heading> source = this.db.tbl_category_heading.Where<tbl_category_heading>((Expression<Func<tbl_category_heading, bool>>) (t => t.id_category_tiles == (int?) item.id_category_tiles));
        Expression<Func<tbl_category_heading, int?>> keySelector = (Expression<Func<tbl_category_heading, int?>>) (t => t.heading_order);
        foreach (tbl_category_heading tblCategoryHeading in source.OrderBy<tbl_category_heading, int?>(keySelector).ToList<tbl_category_heading>())
        {
          int num = this.db.tbl_category_associantion.SqlQuery("select * from tbl_category_associantion where status='A' AND  id_category_heading =" + (object) tblCategoryHeading.id_category_heading).Count<tbl_category_associantion>();
          intList.Add(num);
          tblCategoryHeadingList.Add(tblCategoryHeading);
        }
      }
      this.ViewData["accessflag"] = (object) new RoleBasedAccess().checkAccess(content.action, 3);
      this.ViewData["select-tile"] = (object) list;
      this.ViewData["heading"] = (object) tblCategoryHeadingList;
      this.ViewData["counter"] = (object) intList;
      this.ViewData[nameof (flag)] = (object) flag;
      return (ActionResult) this.View();
    }

    [RoleAccessController(KEY = 3)]
    public ActionResult edit_category_heading(string id)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Edit Category Titles";
                string Id_assessment = null;
                string Id_category = id;
                string Id_operation = null;
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            int ods = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      int int32 = Convert.ToInt32(id);
      List<tbl_category_tiles> list = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) ods && t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      tbl_category_heading tblCategoryHeading = this.db.tbl_category_heading.Find(new object[1]
      {
        (object) int32
      });
      this.ViewData["select-tile"] = (object) list;
      this.ViewData["heading"] = (object) tblCategoryHeading;
      return (ActionResult) this.View();
    }

    public ActionResult update_cms_category_headers()
    {
      tbl_category_heading tblCategoryHeading = this.db.tbl_category_heading.Find(new object[1]
      {
        (object) Convert.ToInt32(this.Request.Form["id_category_heading"])
      });
      int int32 = Convert.ToInt32((object) tblCategoryHeading.heading_order);
      tblCategoryHeading.id_category_tiles = new int?(Convert.ToInt32(this.Request.Form["select-category-tile"]));
      tblCategoryHeading.Heading_title = this.Request.Form["category-heading"];
      tblCategoryHeading.heading_order = new int?(Convert.ToInt32(this.Request.Form["category-order"]));
      this.db.SaveChanges();
      int num1 = int32;
      int? headingOrder = tblCategoryHeading.heading_order;
      int valueOrDefault1 = headingOrder.GetValueOrDefault();
      if ((num1 == valueOrDefault1 ? (headingOrder.HasValue ? 1 : 0) : 0) == 0)
      {
        int num2 = int32;
        headingOrder = tblCategoryHeading.heading_order;
        int valueOrDefault2 = headingOrder.GetValueOrDefault();
        int num3 = num2 < valueOrDefault2 ? (headingOrder.HasValue ? 1 : 0) : 0;
      }
      return (ActionResult) this.RedirectToAction("display_category_heading");
    }

    public ActionResult delete_category_heading(string id)
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "List Category Heading";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int ids = Convert.ToInt32(id);
      tbl_category_associantion entity = this.db.tbl_category_associantion.Where<tbl_category_associantion>((Expression<Func<tbl_category_associantion, bool>>) (t => t.id_category_heading == (int?) ids)).FirstOrDefault<tbl_category_associantion>();
      if (entity != null)
      {
        this.db.tbl_category_associantion.Remove(entity);
        this.db.SaveChanges();
      }
      this.db.tbl_category_heading.Remove(this.db.tbl_category_heading.Find(new object[1]
      {
        (object) ids
      }));
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("display_category_heading");
    }

    public ActionResult SortCategoryTiles()
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Sort Landing Page Tile";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_category_tiles> list = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) oid && t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      this.ViewData["accessflag"] = (object) new RoleBasedAccess().checkAccess(content.action, 3);
      this.ViewData["category_tile"] = (object) list;
      return (ActionResult) this.View();
    }

    public ActionResult SortCategoryHeading()
    {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Sort Category Heading";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_category_tiles> list = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) oid && t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      this.ViewData["accessflag"] = (object) new RoleBasedAccess().checkAccess(content.action, 3);
      this.ViewData["category_tile"] = (object) list;
      return (ActionResult) this.View();
    }

    public ActionResult SortCategory()
    {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Sort Category";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_category_tiles> list = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_organization == (int?) oid && t.status == "A")).OrderBy<tbl_category_tiles, string>((Expression<Func<tbl_category_tiles, string>>) (t => t.tile_heading)).ToList<tbl_category_tiles>();
      this.ViewData["accessflag"] = (object) new RoleBasedAccess().checkAccess(content.action, 3);
      this.ViewData["category_tile"] = (object) list;
      return (ActionResult) this.View();
    }

    public string getCategoryHeadingSelect(string id)
    {
      string categoryHeadingSelect = "<option value=\"0\" selected> Select Category Heading </option>";
      int ctid = Convert.ToInt32(id);
      tbl_category_tiles tile = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_category_tiles == ctid && t.status == "A")).FirstOrDefault<tbl_category_tiles>();
      if (tile != null)
      {
        DbSet<tbl_category_heading> tblCategoryHeading1 = this.db.tbl_category_heading;
        Expression<Func<tbl_category_heading, bool>> predicate = (Expression<Func<tbl_category_heading, bool>>) (t => t.id_category_tiles == (int?) tile.id_category_tiles && t.status == "A");
        foreach (tbl_category_heading tblCategoryHeading2 in tblCategoryHeading1.Where<tbl_category_heading>(predicate).ToList<tbl_category_heading>())
          categoryHeadingSelect = categoryHeadingSelect + "<option value=\"" + (object) tblCategoryHeading2.id_category_heading + "\" > " + tblCategoryHeading2.Heading_title + " </option>";
      }
      return categoryHeadingSelect;
    }

    public string getCategoryHeading(string id)
    {
      string categoryHeading = "";
      int ctid = Convert.ToInt32(id);
      tbl_category_tiles tile = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_category_tiles == ctid && t.status == "A")).FirstOrDefault<tbl_category_tiles>();
      if (tile != null)
      {
        List<tbl_category_heading> list = this.db.tbl_category_heading.Where<tbl_category_heading>((Expression<Func<tbl_category_heading, bool>>) (t => t.id_category_tiles == (int?) tile.id_category_tiles && t.status == "A")).ToList<tbl_category_heading>();
        string str = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\">" + "<thead><tr> <th width=\"70%\">Category Heading </th> <th width=\"20%\">Order </th> <th width=\"10%\">Order </th></tr> </thead>" + "<tbody>";
        foreach (tbl_category_heading tblCategoryHeading in list)
        {
          str += "<tr>";
          str = str + "<td>" + tblCategoryHeading.Heading_title + "</td>";
          str = str + " <td> <input style=\"text-align:center;width:100%;\" type=\"number\" max=\"100\" min=\"1\" id=\"ct_" + (object) tblCategoryHeading.id_category_heading + "\" value=\"" + (object) tblCategoryHeading.heading_order + "\" /></td>";
          str = str + " <td> <input id=\"notification-to-all\" type=\"button\" class=\"btn btn-primary btn-sm pull-right\" value=\"update\" onclick=\"updateOrder('ct_" + (object) tblCategoryHeading.id_category_heading + "','" + (object) tblCategoryHeading.id_category_heading + "')\" /></td>";
          str += " </tr>";
        }
        categoryHeading = str + " </tbody></table>";
      }
      return categoryHeading;
    }

    public string getCategoryList(string cid, string hid)
    {
      string categoryList = "";
      int ctid = Convert.ToInt32(cid);
      int chid = Convert.ToInt32(hid);
      tbl_category_heading heading = this.db.tbl_category_heading.Where<tbl_category_heading>((Expression<Func<tbl_category_heading, bool>>) (t => t.id_category_tiles == (int?) ctid && t.id_category_heading == chid && t.status == "A")).FirstOrDefault<tbl_category_heading>();
      if (heading != null)
      {
        List<tbl_category_associantion> list = this.db.tbl_category_associantion.Where<tbl_category_associantion>((Expression<Func<tbl_category_associantion, bool>>) (t => t.id_category_tile == heading.id_category_tiles && t.id_category_heading == (int?) heading.id_category_heading && t.status == "A")).ToList<tbl_category_associantion>();
        string str = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\">" + "<thead><tr> <th width=\"70%\">Category  </th> <th width=\"20%\">Order </th> <th width=\"10%\">Order </th></tr> </thead>" + "<tbody>";
        foreach (tbl_category_associantion categoryAssociantion in list)
        {
          tbl_category_associantion item = categoryAssociantion;
          tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => (int?) t.ID_CATEGORY == item.id_category && t.STATUS == "A")).FirstOrDefault<tbl_category>();
          str += "<tr>";
          str = str + "<td>" + tblCategory.CATEGORYNAME + "</td>";
          str = str + " <td> <input style=\"text-align:center;width:100%;\" type=\"number\" max=\"100\" min=\"1\" id=\"ct_" + (object) item.id_category_association + "\" value=\"" + (object) item.category_order + "\" /></td>";
          str = str + " <td> <input id=\"notification-to-all\" type=\"button\" class=\"btn btn-primary btn-sm pull-right\" value=\"update\" onclick=\"updateOrder('ct_" + (object) item.id_category_association + "','" + (object) item.id_category_association + "')\" /></td>";
          str += " </tr>";
        }
        categoryList = str + " </tbody></table>";
      }
      return categoryList;
    }

    public string updateTileOrder(string ctid, string ord)
    {
      int ctids = Convert.ToInt32(ctid);
      int int32 = Convert.ToInt32(ord);
      tbl_category_tiles tblCategoryTiles = this.db.tbl_category_tiles.Where<tbl_category_tiles>((Expression<Func<tbl_category_tiles, bool>>) (t => t.id_category_tiles == ctids)).FirstOrDefault<tbl_category_tiles>();
      if (tblCategoryTiles != null)
      {
        tblCategoryTiles.category_order = new int?(int32);
        this.db.SaveChanges();
      }
      return "1";
    }

    public string updateHeadingOrder(string ctid, string ord)
    {
      int ctids = Convert.ToInt32(ctid);
      int int32 = Convert.ToInt32(ord);
      tbl_category_heading tblCategoryHeading = this.db.tbl_category_heading.Where<tbl_category_heading>((Expression<Func<tbl_category_heading, bool>>) (t => t.id_category_heading == ctids)).FirstOrDefault<tbl_category_heading>();
      if (tblCategoryHeading != null)
      {
        tblCategoryHeading.heading_order = new int?(int32);
        this.db.SaveChanges();
      }
      return "1";
    }

    public string updateCategoryOrder(string ctid, string ord)
    {
      int caids = Convert.ToInt32(ctid);
      int int32 = Convert.ToInt32(ord);
      tbl_category_associantion categoryAssociantion = this.db.tbl_category_associantion.Where<tbl_category_associantion>((Expression<Func<tbl_category_associantion, bool>>) (t => t.id_category_association == caids && t.status == "A")).FirstOrDefault<tbl_category_associantion>();
      if (categoryAssociantion != null)
      {
        categoryAssociantion.category_order = new int?(int32);
        this.db.SaveChanges();
      }
      return "1";
    }
  }
}
