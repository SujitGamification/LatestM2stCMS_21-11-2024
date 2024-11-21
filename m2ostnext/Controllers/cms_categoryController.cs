// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.cms_categoryController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class cms_categoryController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index() => (ActionResult) this.View();

    public ActionResult add_category(int? id) => (ActionResult) this.View();

    public ActionResult add_cms_category(FormCollection formCollection)
    {
      string str1 = (string) null;
      string str2 = this.Request.Form["Category"];
      string str3 = str2.Replace(" ", "");
      string str4 = this.Request.Form["Description"];
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      tbl_category temp = new tbl_category();
      try
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          str1 = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
          string str5 = System.Web.HttpContext.Current.Request["id"];
          if (file != null)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str3 + str1);
            file.SaveAs(filename);
          }
        }
        temp.CATEGORYNAME = str2;
        temp.DESCRIPTION = str4;
        temp.IMAGE_PATH = str3 + str1;
        temp.STATUS = "A";
        temp.UPDATED_DATE_TIME = DateTime.Now;
        temp.ID_ORGANIZATION = Convert.ToInt32(content.id_ORGANIZATION);
      }
      catch (Exception ex)
      {
        new contentDashboardModel().exception_log(ex);
      }
      return new addCMS_CategoryModel().add_cms_category(temp).Equals("TRUE") ? (ActionResult) this.RedirectToAction("display_category") : (ActionResult) this.RedirectToAction("add_category");
    }

    public ActionResult edit_category(int? id)
    {
          
            if (!id.HasValue)
        return (ActionResult) this.RedirectToAction("display_category");
      tbl_category model = this.db.tbl_category.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.RedirectToAction("display_category") : (ActionResult) this.View((object) model);
    }

    public ActionResult edit_cms_category(FormCollection formCollection)
    {
      int int32 = Convert.ToInt32(this.Request.Form["ID_Category"]);
      string str1 = this.Request.Form["Category"];
      string str2 = str1.Replace(" ", "");
      string str3 = this.Request.Form["Description"];
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      tbl_category tblCategory = this.db.tbl_category.Find(new object[1]
      {
        (object) int32
      });
      tbl_category temp = new tbl_category();
      try
      {
        if (((IEnumerable<string>) System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
        {
          HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
          string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
          string str4 = System.Web.HttpContext.Current.Request["id"];
          if (file != null && file.ContentLength > 0)
          {
            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/")))
              Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"));
            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/" + content.id_ORGANIZATION + "/"), str2 + extension);
            file.SaveAs(filename);
            temp.IMAGE_PATH = str2 + extension;
          }
          else
            temp.IMAGE_PATH = tblCategory.IMAGE_PATH;
        }
        temp.CATEGORYNAME = str1;
        temp.DESCRIPTION = str3;
        temp.STATUS = "A";
        temp.ID_ORGANIZATION = Convert.ToInt32(content.id_ORGANIZATION);
        temp.UPDATED_DATE_TIME = DateTime.Now;
        temp.ID_CATEGORY = int32;
      }
      catch (Exception ex)
      {
        new contentDashboardModel().exception_log(ex);
      }
      return new addCMS_CategoryModel().edit_cms_category(temp).Equals("TRUE") ? (ActionResult) this.RedirectToAction("display_category") : (ActionResult) this.RedirectToAction("add_category");
    }

    public ActionResult display_category()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_category> list1 = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_ORGANIZATION == oid)).ToList<tbl_category>();
      List<int> intList = new List<int>();
      foreach (tbl_category tblCategory in list1)
      {
        List<tbl_content_organization_mapping> list2 = this.db.tbl_content_organization_mapping.SqlQuery("select * from tbl_content_organization_mapping where id_category in (select id_category from tbl_category where id_organization =" + content.id_ORGANIZATION + ")").ToList<tbl_content_organization_mapping>();
        intList.Add(list2.Count);
      }
      this.ViewData["Category"] = (object) list1;
      this.ViewData["CatCount"] = (object) intList;
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

    public string getCategories()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      int oid = Convert.ToInt32(content.id_ORGANIZATION);
      List<tbl_category> list1 = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>) (t => t.ID_ORGANIZATION == oid)).ToList<tbl_category>();
      List<int> intList = new List<int>();
      foreach (tbl_category tblCategory in list1)
      {
        List<tbl_content_organization_mapping> list2 = this.db.tbl_content_organization_mapping.SqlQuery("select * from tbl_content_organization_mapping where id_category in (select id_category from tbl_category where id_organization =" + content.id_ORGANIZATION + ")").ToList<tbl_content_organization_mapping>();
        intList.Add(list2.Count);
      }
      string categories = "";
      foreach (tbl_category tblCategory in list1)
        categories = categories + "<option value = " + (object) tblCategory.ID_CATEGORY + ">" + tblCategory.CATEGORYNAME + "</option>";
      return categories;
    }
  }
}
