// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.tbl_business_typeController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class tbl_business_typeController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index() => (ActionResult) this.View((object) this.db.tbl_business_type.ToList<tbl_business_type>());

    public ActionResult Details(int? id)
    {
      if (!id.HasValue)
        return (ActionResult) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      tbl_business_type model = this.db.tbl_business_type.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    public ActionResult Create() => (ActionResult) this.View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "ID_BUSINESS_TYPE,BUSINESS_TYPE_NAME,DESCRIPTION,STATUS,UPDATED_DATE_TIME")] tbl_business_type tbl_business_type)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) tbl_business_type);
      this.db.tbl_business_type.Add(tbl_business_type);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Edit(int? id)
    {
      if (!id.HasValue)
        return (ActionResult) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      tbl_business_type model = this.db.tbl_business_type.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "ID_BUSINESS_TYPE,BUSINESS_TYPE_NAME,DESCRIPTION,STATUS,UPDATED_DATE_TIME")] tbl_business_type tbl_business_type)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) tbl_business_type);
      this.db.Entry<tbl_business_type>(tbl_business_type).State = EntityState.Modified;
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Delete(int? id)
    {
      if (!id.HasValue)
        return (ActionResult) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      tbl_business_type model = this.db.tbl_business_type.Find(new object[1]
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
      this.db.tbl_business_type.Remove(this.db.tbl_business_type.Find(new object[1]
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
