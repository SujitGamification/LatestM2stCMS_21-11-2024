using m2ostnext;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class content_metadataController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public content_metadataController()
        {
        }

        public ActionResult Create()
        {
            ((dynamic)base.ViewBag).ID_CONTENT_ANSWER = new SelectList(this.db.tbl_content_answer, "ID_CONTENT_ANSWER", "CONTENT_ANSWER");
            return base.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_CONTENT_METADATA,CONTENT_METADATA,CONTENT_METADATA_COUNTER,ID_CONTENT_ANSWER,STATUS,UPDATED_DATE_TIME")] m2ostnext.tbl_content_metadata tbl_content_metadata)
        {
            if (base.ModelState.IsValid)
            {
                this.db.tbl_content_metadata.Add(tbl_content_metadata);
                this.db.SaveChanges();
                return base.RedirectToAction("Index");
            }
            ((dynamic)base.ViewBag).ID_CONTENT_ANSWER = new SelectList(this.db.tbl_content_answer, "ID_CONTENT_ANSWER", "CONTENT_ANSWER", (object)tbl_content_metadata.ID_CONTENT_ANSWER);
            return base.View(tbl_content_metadata);
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_content_metadata tblContentMetadatum = this.db.tbl_content_metadata.Find(new object[] { id });
            if (tblContentMetadatum == null)
            {
                return base.HttpNotFound();
            }
            return base.View(tblContentMetadatum);
        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_content_metadata tblContentMetadatum = this.db.tbl_content_metadata.Find(new object[] { id });
            this.db.tbl_content_metadata.Remove(tblContentMetadatum);
            this.db.SaveChanges();
            return base.RedirectToAction("display_content_metadata", "dashboard");
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_content_metadata tblContentMetadatum = this.db.tbl_content_metadata.Find(new object[] { id });
            if (tblContentMetadatum == null)
            {
                return base.HttpNotFound();
            }
            return base.View(tblContentMetadatum);
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
            tbl_content_metadata tblContentMetadatum = this.db.tbl_content_metadata.Find(new object[] { id });
            if (tblContentMetadatum == null)
            {
                return base.HttpNotFound();
            }
            ((dynamic)base.ViewBag).ID_CONTENT_ANSWER = new SelectList(this.db.tbl_content_answer, "ID_CONTENT_ANSWER", "CONTENT_ANSWER", (object)tblContentMetadatum.ID_CONTENT_ANSWER);
            return base.View(tblContentMetadatum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_CONTENT_METADATA,CONTENT_METADATA,CONTENT_METADATA_COUNTER,ID_CONTENT_ANSWER,STATUS,UPDATED_DATE_TIME")] m2ostnext.tbl_content_metadata tbl_content_metadata)
        {
            if (base.ModelState.IsValid)
            {
                this.db.Entry<m2ostnext.tbl_content_metadata>(tbl_content_metadata).State = EntityState.Modified;
                this.db.SaveChanges();
                return base.RedirectToAction("Index");
            }
            ((dynamic)base.ViewBag).ID_CONTENT_ANSWER = new SelectList(this.db.tbl_content_answer, "ID_CONTENT_ANSWER", "CONTENT_ANSWER", (object)tbl_content_metadata.ID_CONTENT_ANSWER);
            return base.View(tbl_content_metadata);
        }

        public ActionResult Index()
        {
            IQueryable<tbl_content_metadata> tblContentMetadatas = this.db.tbl_content_metadata.Include<tbl_content_metadata, tbl_content_answer>((tbl_content_metadata t) => t.tbl_content_answer);
            return base.View(tblContentMetadatas.ToList<tbl_content_metadata>());
        }
    }
}