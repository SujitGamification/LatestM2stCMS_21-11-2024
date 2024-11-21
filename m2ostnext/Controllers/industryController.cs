// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.industryController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class industryController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index()
    {
      List<tbl_industry> list = this.db.tbl_industry.ToList<tbl_industry>();
      this.ViewData["industry"] = (object) list;
      List<int> intList = new List<int>();
      foreach (tbl_industry tblIndustry in list)
      {
        tbl_industry cat = tblIndustry;
        tbl_organization tblOrganization = this.db.tbl_organization.Where<tbl_organization>((Expression<Func<tbl_organization, bool>>) (t => t.ID_INDUSTRY == cat.ID_INDUSTRY)).FirstOrDefault<tbl_organization>();
        int num = 0;
        if (tblOrganization != null)
          num = 1;
        intList.Add(num);
      }
      this.ViewData["counter"] = (object) intList;
      return (ActionResult) this.View();
    }

    public ActionResult Create() => (ActionResult) this.View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "INDUSTRYNAME,DESCRIPTION")] tbl_industry tbl_industry)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) tbl_industry);
      tbl_industry.STATUS = "0";
      tbl_industry.UPDATED_DATE_TIME = DateTime.Now;
      this.db.tbl_industry.Add(tbl_industry);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("display_association", "cms_association");
    }

    public ActionResult Edit(string id)
    {
      int int32 = Convert.ToInt32(id);
      if (id == null)
        return (ActionResult) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      this.ViewData["industry"] = (object) this.db.tbl_industry.Find(new object[1]
      {
        (object) int32
      });
      return (ActionResult) this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "ID_INDUSTRY,INDUSTRYNAME,DESCRIPTION,STATUS,UPDATED_DATE_TIME")] tbl_industry tbl_industry)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) tbl_industry);
      this.db.Entry<tbl_industry>(tbl_industry).State = EntityState.Modified;
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Delete(int? id)
    {
      if (!id.HasValue)
        return (ActionResult) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      tbl_industry model = this.db.tbl_industry.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      this.db.tbl_industry.Remove(this.db.tbl_industry.Find(new object[1]
      {
        (object) id
      }));
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }
  }
}
