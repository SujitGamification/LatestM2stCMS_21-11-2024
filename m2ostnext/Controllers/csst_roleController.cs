// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.csst_roleController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  [UserFilter]
  public class csst_roleController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index()
    {
      int oid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      tbl_organization tblOrganization = this.db.tbl_organization.Find(new object[1]
      {
        (object) oid
      });
      List<tbl_csst_role> list = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>) (t => t.id_organization == (int?) oid)).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>) (t => t.csst_role)).ToList<tbl_csst_role>();
      if (list.Count == 0)
      {
        this.db.tbl_csst_role.Add(new tbl_csst_role()
        {
          id_organization = new int?(oid),
          csst_role = "User",
          status = "A",
          updated_dated_time = new DateTime?(DateTime.Now)
        });
        this.db.SaveChanges();
        list = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>) (t => t.id_organization == (int?) oid)).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>) (t => t.csst_role)).ToList<tbl_csst_role>();
      }
      this.ViewData["csst_role"] = (object) list;
      this.ViewData["cscc_org"] = (object) tblOrganization;
      return (ActionResult) this.View();
    }

    public string editCsstRole(string id, string role)
    {
      tbl_csst_role tblCsstRole = this.db.tbl_csst_role.Find(new object[1]
      {
        (object) Convert.ToInt32(id)
      });
      if (tblCsstRole != null)
      {
        tblCsstRole.csst_role = role;
        tblCsstRole.updated_dated_time = new DateTime?(DateTime.Now);
        this.db.SaveChanges();
      }
      return "1";
    }

    public string insertCsstRole(string role)
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      this.db.tbl_csst_role.Add(new tbl_csst_role()
      {
        id_organization = new int?(int32),
        csst_role = role,
        status = "A",
        updated_dated_time = new DateTime?(DateTime.Now)
      });
      this.db.SaveChanges();
      return "1";
    }

    public string deleteCsstRole(string id)
    {
      int ids = Convert.ToInt32(id);
      DbSet<tbl_content_role_mapping> contentRoleMapping = this.db.tbl_content_role_mapping;
      Expression<Func<tbl_content_role_mapping, bool>> predicate = (Expression<Func<tbl_content_role_mapping, bool>>) (t => t.id_csst_role == (int?) ids);
      foreach (tbl_content_role_mapping entity in contentRoleMapping.Where<tbl_content_role_mapping>(predicate).ToList<tbl_content_role_mapping>())
      {
        this.db.tbl_content_role_mapping.Remove(entity);
        this.db.SaveChanges();
      }
      this.db.tbl_csst_role.Remove(this.db.tbl_csst_role.Find(new object[1]
      {
        (object) ids
      }));
      this.db.SaveChanges();
      return "1";
    }
  }
}
