using m2ostnext;
using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class organizationController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public organizationController()
        {
        }

        public ActionResult Create()
        {
            List<tbl_industry> list = this.db.tbl_industry.ToList<tbl_industry>();
            base.ViewData["select-industry"] = list;
            List<tbl_business_type> tblBusinessTypes = this.db.tbl_business_type.ToList<tbl_business_type>();
            base.ViewData["select-business-type"] = tblBusinessTypes;
            return base.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_INDUSTRY,ORGANIZATION_NAME,CONTACT_NAME,CONTACTNUMBER,CONTACTEMAIL")] m2ostnext.tbl_organization tbl_organization)
        {
            if (base.ModelState.IsValid)
            {
                tbl_organization.STATUS = "0";
                tbl_organization.UPDATED_DATE_TIME = DateTime.Now;
                this.db.tbl_organization.Add(tbl_organization);
                this.db.SaveChanges();
                return base.RedirectToAction("display_association", "cms_association");
            }
            ((dynamic)base.ViewBag).ID_INDUSTRY = new SelectList(this.db.tbl_industry, "ID_INDUSTRY", "INDUSTRYNAME", (object)tbl_organization.ID_INDUSTRY);
            return base.View(tbl_organization);
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_organization tblOrganization = this.db.tbl_organization.Find(new object[] { id });
            if (tblOrganization == null)
            {
                return base.HttpNotFound();
            }
            return base.View(tblOrganization);
        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_organization tblOrganization = this.db.tbl_organization.Find(new object[] { id });
            this.db.tbl_organization.Remove(tblOrganization);
            this.db.SaveChanges();
            return base.RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_organization tblOrganization = this.db.tbl_organization.Find(new object[] { id });
            if (tblOrganization == null)
            {
                return base.HttpNotFound();
            }
            return base.View(tblOrganization);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_organization tblOrganization = this.db.tbl_organization.Find(new object[] { id });
            if (tblOrganization == null)
            {
                return base.RedirectToAction("displayOrganization", "Administrator");
            }
            int num = Convert.ToInt32(id);
            base.ViewData["organization"] = tblOrganization;
            List<tbl_industry> list = this.db.tbl_industry.ToList<tbl_industry>();
            base.ViewData["select-industry"] = list;
            List<tbl_business_type> tblBusinessTypes = this.db.tbl_business_type.ToList<tbl_business_type>();
            base.ViewData["select-business-type"] = tblBusinessTypes;
            base.ViewData["Logos"] = (new contentDashboardModel()).getOrgLogo(num);
            base.ViewData["Banner"] = (new contentDashboardModel()).getOrgBanner(num);
            return base.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_ORGANIZATION,ID_INDUSTRY,ORGANIZATION_NAME,CONTACT_NAME,CONTACTNUMBER,CONTACTEMAIL,STATUS,UPDATED_DATE_TIME")] m2ostnext.tbl_organization tbl_organization)
        {
            if (base.ModelState.IsValid)
            {
                this.db.Entry<m2ostnext.tbl_organization>(tbl_organization).State = EntityState.Modified;
                this.db.SaveChanges();
                return base.RedirectToAction("Index");
            }
            ((dynamic)base.ViewBag).ID_INDUSTRY = new SelectList(this.db.tbl_industry, "ID_INDUSTRY", "INDUSTRYNAME", (object)tbl_organization.ID_INDUSTRY);
            return base.View(tbl_organization);
        }

        public ActionResult Index()
        {
            IQueryable<tbl_organization> tblOrganizations = this.db.tbl_organization.Include<tbl_organization, tbl_industry>((tbl_organization t) => t.tbl_industry);
            return base.View(tblOrganizations.ToList<tbl_organization>());
        }
    }
}