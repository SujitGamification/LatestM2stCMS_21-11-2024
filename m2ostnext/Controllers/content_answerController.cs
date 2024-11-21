using m2ostnext;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class content_answerController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public content_answerController()
        {
        }

        public ActionResult Create()
        {
            ((dynamic)base.ViewBag).ID_CONTENT = new SelectList(this.db.tbl_content, "ID_CONTENT", "CONTENT_QUESTION");
            return base.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_CONTENT_ANSWER,ID_CONTENT,CONTENT_ANSWER,STATUS,UPDATED_DATE_TIME")] m2ostnext.tbl_content_answer tbl_content_answer)
        {
            if (base.ModelState.IsValid)
            {
                this.db.tbl_content_answer.Add(tbl_content_answer);
                this.db.SaveChanges();
                return base.RedirectToAction("Index");
            }
            ((dynamic)base.ViewBag).ID_CONTENT = new SelectList(this.db.tbl_content, "ID_CONTENT", "CONTENT_QUESTION", (object)tbl_content_answer.ID_CONTENT);
            return base.View(tbl_content_answer);
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[] { id });
            if (tblContentAnswer == null)
            {
                return base.HttpNotFound();
            }
            return base.View(tblContentAnswer);
        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[] { id });
            this.db.tbl_content_answer.Remove(tblContentAnswer);
            this.db.SaveChanges();
            return base.RedirectToAction("display_content_answer", "dashboard");
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[] { id });
            if (tblContentAnswer == null)
            {
                return base.HttpNotFound();
            }
            return base.View(tblContentAnswer);
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
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[] { id });
            if (tblContentAnswer == null)
            {
                return base.HttpNotFound();
            }
            ((dynamic)base.ViewBag).ID_CONTENT = new SelectList(this.db.tbl_content, "ID_CONTENT", "CONTENT_QUESTION", (object)tblContentAnswer.ID_CONTENT);
            return base.View(tblContentAnswer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_CONTENT_ANSWER,ID_CONTENT,CONTENT_ANSWER,STATUS,UPDATED_DATE_TIME")] m2ostnext.tbl_content_answer tbl_content_answer)
        {
            if (base.ModelState.IsValid)
            {
                this.db.Entry<m2ostnext.tbl_content_answer>(tbl_content_answer).State = EntityState.Modified;
                this.db.SaveChanges();
                return base.RedirectToAction("Index");
            }
            ((dynamic)base.ViewBag).ID_CONTENT = new SelectList(this.db.tbl_content, "ID_CONTENT", "CONTENT_QUESTION", (object)tbl_content_answer.ID_CONTENT);
            return base.View(tbl_content_answer);
        }

        public ActionResult Index()
        {
            return base.View();
        }
    }
}