// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.contentDashboardController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Z.EntityFramework.Extensions;
using static m2ostnext.Controllers.contentDashboardController;
using static m2ostnext.Controllers.UploadUtilityController;
using static m2ostnext.Models.NageUserList;

namespace m2ostnext.Controllers
{
    [UserFilter]
    public class contentDashboardController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public ActionResult Index() => (ActionResult)this.View();

        [RoleAccessController(KEY = 5)]
        public ActionResult contentCreation()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add Content";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

            if (this.TempData["LINKTYPE"] != null)
            {
                int id = Convert.ToInt32(this.TempData["LINKTYPE"]);
                this.ViewData["LINKTYPE"] = (object)id;
                tbl_content tblContent = this.db.tbl_content.Find(new object[1]
                {
          (object) id
                });
                this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == id && t.id_organization == orgid)).FirstOrDefault<tbl_content_organization_mapping>();
                List<tbl_category> list = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
                this.ViewData["parent-content"] = (object)tblContent;
                this.ViewData["parent-category"] = (object)list;
            }
            else
            {
                this.ViewData["LINKTYPE"] = (object)0;
                this.ViewData["parent-content"] = (object)null;
                this.ViewData["parent-category"] = (object)null;
            }
            List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            List<tbl_content_level> list2 = this.db.tbl_content_level.ToList<tbl_content_level>();
            List<tbl_csst_role> list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            if (list3.Count == 0)
            {
                this.db.tbl_csst_role.Add(new tbl_csst_role()
                {
                    id_organization = new int?(orgid),
                    csst_role = "User",
                    status = "A",
                    updated_dated_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
                list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            }
            this.ViewData["select-cscc-role"] = (object)list3;
            this.ViewData["select-category"] = (object)list1;
            this.ViewData["select-level"] = (object)list2;
            return (ActionResult)this.View();
        }

        [RoleAccessController(KEY = 5)]
        public ActionResult linkContentCreation(string id)
        {
            this.TempData["LINKTYPE"] = (object)id;
            return (ActionResult)this.RedirectToAction("contentCreation");
        }

        [RoleAccessController(KEY = 5)]
        public ActionResult createIndustry()
        {
            this.db.tbl_industry.Add(new tbl_industry()
            {
                INDUSTRYNAME = this.Request.Form["industry-name"].ToString(),
                DESCRIPTION = this.Request.Form["industry-desc"].ToString(),
                STATUS = "A",
                UPDATED_DATE_TIME = DateTime.Now
            });
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index", "industry");
        }

        [RoleAccessController(KEY = 5)]
        public ActionResult editIndustry()
        {
            tbl_industry tblIndustry = this.db.tbl_industry.Find(new object[1]
            {
        (object) Convert.ToInt32(this.Request.Form["id_industry"])
            });
            tblIndustry.INDUSTRYNAME = this.Request.Form["industry-name"].ToString();
            tblIndustry.DESCRIPTION = this.Request.Form["industry-desc"].ToString();
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index", "industry");
        }

        public ActionResult DeleteAction(string id)
        {
            Convert.ToInt32(id);
            this.db.tbl_industry.Remove(this.db.tbl_industry.Find(new object[1]
            {
        (object) id
            }));
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index", "industry");
        }

        public ActionResult createBusinesstype()
        {
            this.db.tbl_business_type.Add(new tbl_business_type()
            {
                BUSINESS_TYPE_NAME = this.Request.Form["businesstype-name"].ToString(),
                DESCRIPTION = this.Request.Form["businesstype-desc"].ToString(),
                STATUS = "A",
                UPDATED_DATE_TIME = DateTime.Now
            });
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Create", "tbl_business_type");
        }

        public ActionResult createOrganizationAction()
        {
            tbl_organization entity = new tbl_organization();
            entity.ID_INDUSTRY = Convert.ToInt32(this.Request.Form["select-industry"].ToString());
            entity.ID_BUSINESS_TYPE = Convert.ToInt32(this.Request.Form["select-businuss-type"].ToString());
            entity.ORGANIZATION_NAME = this.Request.Form["organisation-name"].ToString();
            entity.DESCRIPTION = this.Request.Form["organisation-desc"].ToString();
            entity.CONTACT_NAME = this.Request.Form["organisation-contact"].ToString();
            entity.CONTACTEMAIL = this.Request.Form["organisation-email"].ToString();
            entity.CONTACTNUMBER = this.Request.Form["organisation-contact-no"].ToString();
            entity.STATUS = "A";
            entity.UPDATED_DATE_TIME = DateTime.Now;
            this.db.tbl_organization.Add(entity);
            this.db.SaveChanges();
            try
            {
                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["logo-btn"];
                    string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["logo-btn"].FileName);
                    string str = System.Web.HttpContext.Current.Request["id"];
                    if (file != null)
                    {
                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/")))
                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"));
                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"), entity.ID_ORGANIZATION.ToString() + extension);
                        file.SaveAs(filename);
                    }
                    entity.LOGO = entity.ID_ORGANIZATION.ToString() + extension;
                    this.db.SaveChanges();
                }
                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["banner-btn"];
                    string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["banner-btn"].FileName);
                    string str = System.Web.HttpContext.Current.Request["id"];
                    if (file != null)
                    {
                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/")))
                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"));
                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"), entity.ID_ORGANIZATION.ToString() + extension);
                        file.SaveAs(filename);
                    }
                    this.db.tbl_organisation_banner.Add(new tbl_organisation_banner()
                    {
                        banner_name = entity.ID_ORGANIZATION.ToString() + extension,
                        Banner_path = entity.ID_ORGANIZATION.ToString() + extension,
                        id_organisation = new int?(entity.ID_ORGANIZATION),
                        updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                }
                if (entity.ID_ORGANIZATION > 0)
                {
                    this.db.tbl_cms_roles.Add(new tbl_cms_roles()
                    {
                        ID_ORGANIZATION = entity.ID_ORGANIZATION,
                        ROLENAME = "Admin_" + (object)entity.ID_ORGANIZATION,
                        STATUS = "A",
                        UPDATED_DATE_TIME = DateTime.Now,
                        DESCRIPTION = "administrator"
                    });
                    this.db.SaveChanges();
                    this.db.tbl_cms_roles.Add(new tbl_cms_roles()
                    {
                        ID_ORGANIZATION = entity.ID_ORGANIZATION,
                        ROLENAME = "User_" + (object)entity.ID_ORGANIZATION,
                        STATUS = "A",
                        UPDATED_DATE_TIME = DateTime.Now,
                        DESCRIPTION = "User"
                    });
                    this.db.SaveChanges();
                    this.db.tbl_role.Add(new tbl_role()
                    {
                        ID_ORGANIZATION = entity.ID_ORGANIZATION,
                        ROLENAME = "Admin_" + (object)entity.ID_ORGANIZATION,
                        STATUS = "A",
                        UPDATEDTIME = DateTime.Now,
                        DESCRIPTION = "administrator"
                    });
                    this.db.SaveChanges();
                    this.db.tbl_role.Add(new tbl_role()
                    {
                        ID_ORGANIZATION = entity.ID_ORGANIZATION,
                        ROLENAME = "User_" + (object)entity.ID_ORGANIZATION,
                        STATUS = "A",
                        UPDATEDTIME = DateTime.Now,
                        DESCRIPTION = "User"
                    });
                    this.db.SaveChanges();
                    this.db.tbl_csst_role.Add(new tbl_csst_role()
                    {
                        csst_role = "App User",
                        id_organization = new int?(entity.ID_ORGANIZATION),
                        status = "A",
                        updated_dated_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
            return (ActionResult)this.RedirectToAction("Create", "organization");
        }

        public ActionResult editOrganizationAction()
        {
            tbl_organization organisation = this.db.tbl_organization.Find(new object[1]
            {
        (object) Convert.ToInt32(this.Request.Form["id_organization"])
            });
            organisation.ID_INDUSTRY = Convert.ToInt32(this.Request.Form["select-industry"].ToString());
            organisation.ID_BUSINESS_TYPE = Convert.ToInt32(this.Request.Form["select-businuss-type"].ToString());
            organisation.ORGANIZATION_NAME = this.Request.Form["organisation-name"].ToString();
            organisation.DESCRIPTION = this.Request.Form["organisation-desc"].ToString();
            organisation.CONTACT_NAME = this.Request.Form["organisation-contact"].ToString();
            organisation.CONTACTEMAIL = this.Request.Form["organisation-email"].ToString();
            organisation.CONTACTNUMBER = this.Request.Form["organisation-contact-no"].ToString();
            organisation.STATUS = "A";
            organisation.UPDATED_DATE_TIME = DateTime.Now;
            this.db.SaveChanges();
            try
            {
                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["logo-btn"];
                    string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["logo-btn"].FileName);
                    string str = System.Web.HttpContext.Current.Request["id"];
                    if (file != null)
                    {
                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/")))
                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"));
                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/ORGLOGO/"), organisation.ID_ORGANIZATION.ToString() + extension);
                        file.SaveAs(filename);
                        organisation.LOGO = organisation.ID_ORGANIZATION.ToString() + extension;
                        this.db.SaveChanges();
                    }
                }
                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["banner-btn"];
                    string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["banner-btn"].FileName);
                    string str = System.Web.HttpContext.Current.Request["id"];
                    if (file != null)
                    {
                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/")))
                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"));
                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/BANNERIMG/"), organisation.ID_ORGANIZATION.ToString() + extension);
                        file.SaveAs(filename);
                        tbl_organisation_banner organisationBanner = this.db.tbl_organisation_banner.Where<tbl_organisation_banner>((Expression<Func<tbl_organisation_banner, bool>>)(t => t.id_organisation == (int?)organisation.ID_ORGANIZATION)).FirstOrDefault<tbl_organisation_banner>();
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
            catch (Exception ex)
            {
            }
            return (ActionResult)this.RedirectToAction("displayOrganization", "contentDashboard");
        }

        public ActionResult feedbackCreation() => (ActionResult)this.View();

        public ActionResult createFeedback()
        {
            string str1 = this.Request.Form["btn_submit"].ToString();
            string str2 = this.Request.Form["feedback-title"].ToString();
            string str3 = this.Request.Form["feedback-question"].ToString();
            int int32 = Convert.ToInt32(this.Request.Form["step-count"].ToString());
            List<string> values = new List<string>();
            for (int index = 1; index <= int32; ++index)
            {
                if (!string.IsNullOrEmpty(this.Request.Form["choice-" + (object)index].ToString().Trim()))
                    values.Add("[" + this.Request.Form["choice-" + (object)index].ToString().Trim() + "]");
            }
            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
            {
                HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
                string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["uploadBtn"].FileName);
                tbl_feedback_bank entity = new tbl_feedback_bank();
                entity.FEEDBACK_NAME = str2;
                entity.FEEDBACK_QUESTION = str3;
                entity.FEEDBACK_CHOICES = string.Join("|", (IEnumerable<string>)values);
                entity.FEEDBACK_IMAGE = "";
                entity.STATUS = "A";
                entity.UPDATED_DATE_TIME = DateTime.Now;
                this.db.tbl_feedback_bank.Add(entity);
                this.db.SaveChanges();
                int idFeedbackBank = entity.ID_FEEDBACK_BANK;
                if (file != null)
                {
                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/FEEDBACK/" + (object)idFeedbackBank + "/")))
                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/FEEDBACK/" + (object)idFeedbackBank + "/"));
                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/FEEDBACK/" + (object)idFeedbackBank + "/"), idFeedbackBank.ToString() + "-1" + extension);
                    file.SaveAs(filename);
                }
                entity.FEEDBACK_IMAGE = idFeedbackBank.ToString() + extension;
                this.db.tbl_feedback_bank.Attach(entity);
                this.db.Entry<tbl_feedback_bank>(entity).Property<string>((Expression<Func<tbl_feedback_bank, string>>)(x => x.FEEDBACK_IMAGE)).IsModified = true;
                this.db.SaveChanges();
            }
            return str1.Equals("Save and Exit") ? (ActionResult)this.RedirectToAction("displayFeedback") : (ActionResult)this.RedirectToAction("feedbackCreation");
        }

        public ActionResult displayFeedback()
        {
            this.ViewData["list-feeds"] = (object)this.db.tbl_feedback_bank.ToList<tbl_feedback_bank>();
            return (ActionResult)this.View();
        }

        public ActionResult associateFeedback(string id)
        {
            tbl_feedback_bank tblFeedbackBank = this.db.tbl_feedback_bank.Find(new object[1]
            {
        (object) Convert.ToInt32(id)
            });
            List<FeddbackAssosiation> feddbackAssosiationList = new List<FeddbackAssosiation>();
            foreach (tbl_content_answer tblContentAnswer in this.db.tbl_content_answer.ToList<tbl_content_answer>())
            {
                tbl_content_answer ans = tblContentAnswer;
                FeddbackAssosiation feddbackAssosiation = new FeddbackAssosiation();
                feddbackAssosiation.Answers = ans;
                List<tbl_content_answer_steps> list = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == ans.ID_CONTENT_ANSWER)).ToList<tbl_content_answer_steps>();
                feddbackAssosiation.AnswerStep = list;
                feddbackAssosiationList.Add(feddbackAssosiation);
            }
            this.ViewData["parent"] = (object)tblFeedbackBank;
            this.ViewData["feedback"] = (object)feddbackAssosiationList;
            return (ActionResult)this.View();
        }

        public string attachFeedback(List<feeds> contents, string feedid)
        {
            if (contents != null)
            {
                foreach (feeds content in contents)
                {
                    if (!string.IsNullOrEmpty(content.rows) || !string.IsNullOrEmpty(content.colids))
                    {
                        tbl_feedback_bank_link entity = new tbl_feedback_bank_link();
                        entity.ID_CONTENT_ANSWER = !(content.rows == "0") ? Convert.ToInt32(content.rows) : 0;
                        entity.ID_ANSWER_ASSOCIATION = !string.IsNullOrEmpty(content.colids) ? content.colids : "";
                        if (entity.ID_CONTENT_ANSWER > 0 || entity.ID_ANSWER_ASSOCIATION.Length > 0)
                        {
                            entity.STATUS = "A";
                            entity.UPDATED_DATE_TIME = DateTime.Now;
                            entity.ID_FEEDBACK_BANK = Convert.ToInt32(feedid);
                            this.db.tbl_feedback_bank_link.Add(entity);
                            this.db.SaveChanges();
                        }
                    }
                }
            }
            return (string)null;
        }

        [RoleAccessController(KEY = 6)]
        public ActionResult createContent()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add New Content";
                string Id_assessment = null;
                string Id_category = this.Request.Form["hid-category"];
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            int cid = 0;
            tbl_content entity1 = (tbl_content)null;
            string str1 = this.Request.Form["role_check"];
            string str2 = this.Request.Form["btn_submit"].ToString();
            string str3 = this.Request.Form["select-link-type"].ToString();
            int int32_1 = Convert.ToInt32(this.Request.Form["link-parent"]);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int OID = Convert.ToInt32(content.id_ORGANIZATION);
            int? cmdUserType = content.USER.cmd_user_type;
            int num1 = 0;
            string str4 = (cmdUserType.GetValueOrDefault() == num1 ? (cmdUserType.HasValue ? 1 : 0) : 0) == 0 ? "P" : "P";
            if (int32_1 > 0)
                entity1 = this.db.tbl_content.Find(new object[1]
                {
          (object) int32_1
                });
            int int32_2 = Convert.ToInt32(this.Request.Form["select-theme"].ToString());
            List<string> stringList = new List<string>();
            int int32_3 = Convert.ToInt32(this.Request.Form["select-level"].ToString());
            DateTime now = DateTime.Now;
            DateTime datetime = new Utility().StringToDatetime(this.Request.Form["content-expiry"].ToString());
            int content_id = 0;
            int num2 = 0;
            tbl_content entity2 = new tbl_content();
            tbl_content_answer entity3 = new tbl_content_answer();
            int int32_4 = Convert.ToInt32(this.Request.Form["step-count"].ToString());
            try
            {
                entity2.CONTENT_HEADER = this.Request.Form["t-header"].ToString();
                entity2.CONTENT_QUESTION = this.Request.Form["t-quetion"].ToString();
                entity2.CONTENT_TITLE = this.Request.Form["t-title"].ToString();
                entity2.ID_THEME = int32_2;
                entity2.ID_CONTENT_LEVEL = int32_3;
                entity2.ID_USER = Convert.ToInt32(content.ID_USER);
                entity2.UPDATED_DATE_TIME = DateTime.Now;
                entity2.CONTENT_COUNTER = 0;
                entity2.EXPIRY_DATE = new DateTime?(datetime);
                entity2.STATUS = str4;
                entity2.CONTENT_OWNER = OID;
                entity2.IS_PRIMARY = int32_1 != 0 ? 0 : 1;
                this.db.tbl_content.Add(entity2);
                this.db.SaveChanges();
                content_id = entity2.ID_CONTENT;
                cid = content_id;

                //test


                //int contentId;

                //using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                //{
                //    connection.Open();

                //    using (MySqlCommand command = new MySqlCommand())
                //    {
                //        command.Connection = connection;
                //        command.CommandText = @"
                //        INSERT INTO tbl_content 
                //        (CONTENT_HEADER, CONTENT_QUESTION, CONTENT_TITLE, ID_THEME, ID_CONTENT_LEVEL, ID_USER, UPDATED_DATE_TIME, CONTENT_COUNTER, EXPIRY_DATE, STATUS, CONTENT_OWNER, IS_PRIMARY)
                //        VALUES 
                //        (@CONTENT_HEADER, @CONTENT_QUESTION, @CONTENT_TITLE, @ID_THEME, @ID_CONTENT_LEVEL, @ID_USER, @UPDATED_DATE_TIME, @CONTENT_COUNTER, @EXPIRY_DATE, @STATUS, @CONTENT_OWNER, @IS_PRIMARY);
                //        SELECT LAST_INSERT_ID();";

                //        command.Parameters.AddWithValue("@CONTENT_HEADER", this.Request.Form["t-header"].ToString());
                //        command.Parameters.AddWithValue("@CONTENT_QUESTION", this.Request.Form["t-quetion"].ToString());
                //        command.Parameters.AddWithValue("@CONTENT_TITLE", this.Request.Form["t-title"].ToString());
                //        command.Parameters.AddWithValue("@ID_THEME", int32_2);
                //        command.Parameters.AddWithValue("@ID_CONTENT_LEVEL", int32_3);
                //        command.Parameters.AddWithValue("@ID_USER", Convert.ToInt32(content.ID_USER));
                //        command.Parameters.AddWithValue("@UPDATED_DATE_TIME", DateTime.Now);
                //        command.Parameters.AddWithValue("@CONTENT_COUNTER", 0);
                //        command.Parameters.AddWithValue("@EXPIRY_DATE", datetime == null ? (object)DBNull.Value : datetime);
                //        command.Parameters.AddWithValue("@STATUS", str4);
                //        command.Parameters.AddWithValue("@CONTENT_OWNER", OID);
                //        command.Parameters.AddWithValue("@IS_PRIMARY", int32_1 != 0 ? 0 : 1);

                //        contentId = Convert.ToInt32(command.ExecuteScalar());
                //    }
                //}

                //content_id = contentId;
                //cid = content_id;


                if (content_id > 0)
                {
                    int int32_5 = Convert.ToInt32(this.Request.Form["hid-category"]);
                    for (int index = 1; index <= int32_5; ++index)
                    {
                        string str5 = Convert.ToString(this.Request.Form["add-con-category-" + (object)index]);
                        if (!string.IsNullOrEmpty(str5))
                        {
                            int citd = Convert.ToInt32(str5);
                            if (this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_category == citd && t.id_content == content_id && t.id_organization == OID)).FirstOrDefault<tbl_content_organization_mapping>() == null)
                            {
                                this.db.tbl_content_organization_mapping.Add(new tbl_content_organization_mapping()
                                {
                                    id_organization = Convert.ToInt32(content.id_ORGANIZATION),
                                    id_category = citd,
                                    id_content = entity2.ID_CONTENT,
                                    //id_content = content_id,
                                    status = "A",
                                    updated_date_time = new DateTime?(DateTime.Now)
                                });
                                this.db.SaveChanges();
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(str1))
                {
                    string str6 = str1;
                    char[] chArray = new char[1] { ',' };
                    foreach (string str7 in str6.Split(chArray))
                    {
                        this.db.tbl_content_role_mapping.Add(new tbl_content_role_mapping()
                        {
                            id_content = new int?(content_id),
                            id_csst_role = new int?(Convert.ToInt32(str7)),
                            updated_date_time = new DateTime?(DateTime.Now),
                            status = "A"
                        });
                        this.db.SaveChanges();
                    }
                }
                switch (int32_2)
                {
                    case 1:
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                        entity3.CONTENT_ANSWER2 = "";
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG1 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 2:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file1 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                            HttpPostedFile file2 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                            if (file2 != null)
                            {
                                string extension = Path.GetExtension(file2.FileName);
                                if (file1.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                    file1.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                                }
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                        entity3.CONTENT_ANSWER2 = "";
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 3:
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG1 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 4:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file3 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                            HttpPostedFile file4 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                            if (file4 != null)
                            {
                                string extension = Path.GetExtension(file4.FileName);
                                if (file3.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                    file3.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                                }
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 5:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file5 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                            HttpPostedFile file6 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                            if (file6 != null)
                            {
                                string extension = Path.GetExtension(file6.FileName);
                                if (file5.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                    file5.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                                }
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 6:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file7 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            HttpPostedFile file8 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            if (file8 != null)
                            {
                                string extension = Path.GetExtension(file8.FileName);
                                if (file7.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                    file7.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                                }
                            }
                            HttpPostedFile file9 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            HttpPostedFile file10 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            if (file10 != null)
                            {
                                string extension = Path.GetExtension(file10.FileName);
                                if (file9.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                    file9.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                                }
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 7:
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                        entity3.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG1 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 8:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file11 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            HttpPostedFile file12 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            if (file12 != null)
                            {
                                string extension = Path.GetExtension(file12.FileName);
                                if (file11.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                    file11.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                                }
                            }
                            HttpPostedFile file13 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            HttpPostedFile file14 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            if (file14 != null)
                            {
                                string extension = Path.GetExtension(file14.FileName);
                                if (file13.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                    file13.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                                }
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 9:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file15 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            HttpPostedFile file16 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            if (file16 != null)
                            {
                                string extension = Path.GetExtension(file16.FileName);
                                if (file15.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                    file15.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                                }
                            }
                            HttpPostedFile file17 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            HttpPostedFile file18 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            if (file18 != null)
                            {
                                string extension = Path.GetExtension(file18.FileName);
                                if (file17.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                    file17.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                                }
                            }
                            HttpPostedFile file19 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            HttpPostedFile file20 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            if (file20 != null)
                            {
                                string extension = Path.GetExtension(file20.FileName);
                                if (file19.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                                    file19.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                                }
                            }
                            HttpPostedFile file21 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            HttpPostedFile file22 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            if (file22 != null)
                            {
                                string extension = Path.GetExtension(file22.FileName);
                                if (file21.ContentLength > 0)
                                {
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                                    file21.SaveAs(filename);
                                    entity3.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                                }
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                        entity3.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 10:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file23 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            HttpPostedFile file24 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            if (file24 != null && file23.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file24.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file23.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                            HttpPostedFile file25 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            HttpPostedFile file26 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            if (file26 != null && file25.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file26.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                file25.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                            }
                            HttpPostedFile file27 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            HttpPostedFile file28 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            if (file28 != null && file27.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file28.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                                file27.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                            }
                            HttpPostedFile file29 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            HttpPostedFile file30 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            if (file30 != null && file29.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file30.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                                file29.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                            }
                            HttpPostedFile file31 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                            HttpPostedFile file32 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                            if (file32 != null && file31.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file32.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-5" + extension);
                                file31.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG5 = content_id.ToString() + "-5" + extension;
                            }
                            HttpPostedFile file33 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                            HttpPostedFile file34 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                            if (file34 != null && file33.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file34.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-6" + extension);
                                file33.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG6 = content_id.ToString() + "-6" + extension;
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                        entity3.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                        entity3.CONTENT_ANSWER5 = this.Request.Form["t-content-5"].ToString();
                        entity3.CONTENT_ANSWER6 = this.Request.Form["t-content-6"].ToString();
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 11:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file35 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            HttpPostedFile file36 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            if (file36 != null && file35.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file36.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file35.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                            HttpPostedFile file37 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            HttpPostedFile file38 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            if (file38 != null && file37.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file38.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                file37.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                            }
                            HttpPostedFile file39 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            HttpPostedFile file40 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            if (file40 != null && file39.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file40.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                                file39.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                            }
                            HttpPostedFile file41 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            HttpPostedFile file42 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            if (file42 != null && file41.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file42.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                                file41.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                            }
                            HttpPostedFile file43 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                            HttpPostedFile file44 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                            if (file44 != null && file43.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file44.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-5" + extension);
                                file43.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG5 = content_id.ToString() + "-5" + extension;
                            }
                            HttpPostedFile file45 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                            HttpPostedFile file46 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                            if (file46 != null && file45.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file46.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-6" + extension);
                                file45.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG6 = content_id.ToString() + "-6" + extension;
                            }
                            HttpPostedFile file47 = System.Web.HttpContext.Current.Request.Files["t-btn-7"];
                            HttpPostedFile file48 = System.Web.HttpContext.Current.Request.Files["t-btn-7"];
                            if (file48 != null && file47.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file48.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-7" + extension);
                                file47.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG7 = content_id.ToString() + "-7" + extension;
                            }
                            HttpPostedFile file49 = System.Web.HttpContext.Current.Request.Files["t-btn-8"];
                            HttpPostedFile file50 = System.Web.HttpContext.Current.Request.Files["t-btn-8"];
                            if (file50 != null && file49.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file50.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-8" + extension);
                                file49.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG8 = content_id.ToString() + "-8" + extension;
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                        entity3.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                        entity3.CONTENT_ANSWER5 = this.Request.Form["t-content-5"].ToString();
                        entity3.CONTENT_ANSWER6 = this.Request.Form["t-content-6"].ToString();
                        entity3.CONTENT_ANSWER7 = this.Request.Form["t-content-7"].ToString();
                        entity3.CONTENT_ANSWER8 = this.Request.Form["t-content-8"].ToString();
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 12:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file51 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            HttpPostedFile file52 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            if (file52 != null && file51.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file52.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file51.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                            HttpPostedFile file53 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            HttpPostedFile file54 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            if (file54 != null && file53.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file54.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                file53.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                            }
                            HttpPostedFile file55 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            HttpPostedFile file56 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            if (file56 != null && file55.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file56.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                                file55.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 13:
                        if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                        {
                            HttpPostedFile file57 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            HttpPostedFile file58 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                            if (file58 != null && file57.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file58.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file57.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                            HttpPostedFile file59 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            HttpPostedFile file60 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                            if (file60 != null && file59.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file60.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                file59.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                            }
                            HttpPostedFile file61 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            HttpPostedFile file62 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                            if (file62 != null && file61.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file62.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                                file61.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                            }
                            HttpPostedFile file63 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            HttpPostedFile file64 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                            if (file64 != null && file63.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file64.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                                file63.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                            }
                            HttpPostedFile file65 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                            HttpPostedFile file66 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                            if (file66 != null && file65.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file66.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-5" + extension);
                                file65.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG5 = content_id.ToString() + "-5" + extension;
                            }
                            HttpPostedFile file67 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                            HttpPostedFile file68 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                            if (file68 != null && file67.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(file68.FileName);
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-6" + extension);
                                file67.SaveAs(filename);
                                entity3.CONTENT_ANSWER_IMG6 = content_id.ToString() + "-6" + extension;
                            }
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                        entity3.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                        entity3.CONTENT_ANSWER5 = this.Request.Form["t-content-5"].ToString();
                        entity3.CONTENT_ANSWER6 = this.Request.Form["t-content-6"].ToString();
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 14:
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                        entity3.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                        entity3.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                        entity3.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG1 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 16:
                        switch (Convert.ToInt32(this.Request.Form["video_type"].ToString()))
                        {
                            case 1:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file69 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                                    HttpPostedFile file70 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                                    if (file70 != null)
                                    {
                                        string extension = Path.GetExtension(file70.FileName);
                                        if (file69.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                            file69.SaveAs(filename);
                                            entity3.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                                            break;
                                        }
                                        break;
                                    }
                                    break;
                                }
                                break;
                            case 2:
                                entity3.CONTENT_ANSWER_IMG1 = this.Request.Form["t-ans-url"].ToString();
                                break;
                        }
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                        entity3.CONTENT_ANSWER2 = "";
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                    case 17:
                        entity3.CONTENT_ANSWER_IMG1 = this.Request.Form["t-btn"].ToString();
                        entity3.ID_CONTENT = content_id;
                        entity3.CONTENT_ANSWER_TITLE = "";
                        entity3.CONTENT_ANSWER_COUNTER = 0;
                        entity3.CONTENT_ANSWER_HEADER = entity2.CONTENT_HEADER;
                        entity3.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                        entity3.CONTENT_ANSWER2 = "";
                        entity3.CONTENT_ANSWER3 = "";
                        entity3.CONTENT_ANSWER4 = "";
                        entity3.CONTENT_ANSWER5 = "";
                        entity3.CONTENT_ANSWER6 = "";
                        entity3.CONTENT_ANSWER7 = "";
                        entity3.CONTENT_ANSWER8 = "";
                        entity3.CONTENT_ANSWER9 = "";
                        entity3.CONTENT_ANSWER10 = "";
                        entity3.CONTENT_ANSWER_IMG2 = "";
                        entity3.CONTENT_ANSWER_IMG3 = "";
                        entity3.CONTENT_ANSWER_IMG4 = "";
                        entity3.CONTENT_ANSWER_IMG5 = "";
                        entity3.CONTENT_ANSWER_IMG6 = "";
                        entity3.CONTENT_ANSWER_IMG7 = "";
                        entity3.CONTENT_ANSWER_IMG8 = "";
                        entity3.CONTENT_ANSWER_IMG9 = "";
                        entity3.CONTENT_ANSWER_IMG10 = "";
                        entity3.STATUS = "A";
                        entity3.UPDATEDTIME = DateTime.Now;
                        this.db.tbl_content_answer.Add(entity3);
                        this.db.SaveChanges();
                        num2 = entity3.ID_CONTENT_ANSWER;
                        break;
                }
                string str8 = this.Request.Form["t-metadata"].ToString();
                char[] chArray1 = new char[1] { ',' };
                foreach (string str9 in str8.Split(chArray1))
                {
                    if (!string.IsNullOrEmpty(str9) && !string.IsNullOrWhiteSpace(str9))
                    {
                        this.db.tbl_content_metadata.Add(new tbl_content_metadata()
                        {
                            CONTENT_METADATA = str9,
                            CONTENT_METADATA_COUNTER = 0,
                            ID_CONTENT_ANSWER = num2,
                            STATUS = "A",
                            UPDATEDTIME = DateTime.Now
                        });
                        this.db.SaveChanges();
                    }
                }
                int num3 = 1;
                for (int index = 1; index <= int32_4; ++index)
                {
                    switch (Convert.ToInt32(this.Request.Form["select-theme-step-" + (object)index].ToString()))
                    {
                        case 8:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = num2,
                                STEPNO = num3,
                                ID_THEME = new int?(8),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 9:
                            tbl_content_answer_steps entity4 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file71 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file72 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file72 != null)
                                {
                                    string extension = Path.GetExtension(file72.FileName);
                                    if (file71.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)index + extension);
                                        file71.SaveAs(filename);
                                        entity4.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity4.ID_CONTENT_ANSWER = num2;
                            entity4.STEPNO = num3;
                            entity4.ID_THEME = new int?(9);
                            entity4.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity4.ANSWER_STEPS_PART2 = "";
                            entity4.ANSWER_STEPS_PART3 = "";
                            entity4.ANSWER_STEPS_PART4 = "";
                            entity4.ANSWER_STEPS_PART5 = "";
                            entity4.ANSWER_STEPS_PART6 = "";
                            entity4.ANSWER_STEPS_PART7 = "";
                            entity4.ANSWER_STEPS_PART8 = "";
                            entity4.ANSWER_STEPS_PART9 = "";
                            entity4.ANSWER_STEPS_PART10 = "";
                            entity4.ANSWER_STEPS_IMG2 = "";
                            entity4.ANSWER_STEPS_IMG3 = "";
                            entity4.ANSWER_STEPS_IMG4 = "";
                            entity4.ANSWER_STEPS_IMG5 = "";
                            entity4.ANSWER_STEPS_IMG6 = "";
                            entity4.ANSWER_STEPS_IMG7 = "";
                            entity4.ANSWER_STEPS_IMG8 = "";
                            entity4.ANSWER_STEPS_IMG9 = "";
                            entity4.ANSWER_STEPS_IMG10 = "";
                            entity4.STATUS = "A";
                            entity4.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity4);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 10:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = num2,
                                STEPNO = num3,
                                ID_THEME = new int?(10),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part3"].ToString(),
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 11:
                            tbl_content_answer_steps entity5 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file73 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file74 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file74 != null)
                                {
                                    string extension = Path.GetExtension(file74.FileName);
                                    if (file73.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)index + extension);
                                        file73.SaveAs(filename);
                                        entity5.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity5.ID_CONTENT_ANSWER = num2;
                            entity5.STEPNO = num3;
                            entity5.ID_THEME = new int?(11);
                            entity5.ANSWER_STEPS_PART1 = "";
                            entity5.ANSWER_STEPS_PART2 = "";
                            entity5.ANSWER_STEPS_PART3 = "";
                            entity5.ANSWER_STEPS_PART4 = "";
                            entity5.ANSWER_STEPS_PART5 = "";
                            entity5.ANSWER_STEPS_PART6 = "";
                            entity5.ANSWER_STEPS_PART7 = "";
                            entity5.ANSWER_STEPS_PART8 = "";
                            entity5.ANSWER_STEPS_PART9 = "";
                            entity5.ANSWER_STEPS_PART10 = "";
                            entity5.ANSWER_STEPS_IMG2 = "";
                            entity5.ANSWER_STEPS_IMG3 = "";
                            entity5.ANSWER_STEPS_IMG4 = "";
                            entity5.ANSWER_STEPS_IMG5 = "";
                            entity5.ANSWER_STEPS_IMG6 = "";
                            entity5.ANSWER_STEPS_IMG7 = "";
                            entity5.ANSWER_STEPS_IMG8 = "";
                            entity5.ANSWER_STEPS_IMG9 = "";
                            entity5.ANSWER_STEPS_IMG10 = "";
                            entity5.STATUS = "A";
                            entity5.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity5);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 12:
                            tbl_content_answer_steps entity6 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file75 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file76 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file76 != null && file75.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file76.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file75.SaveAs(filename);
                                    entity6.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file77 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file78 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file78 != null && file77.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file78.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file77.SaveAs(filename);
                                    entity6.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity6.ID_CONTENT_ANSWER = num2;
                            entity6.STEPNO = num3;
                            entity6.ID_THEME = new int?(12);
                            entity6.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity6.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity6.ANSWER_STEPS_PART3 = "";
                            entity6.ANSWER_STEPS_PART4 = "";
                            entity6.ANSWER_STEPS_PART5 = "";
                            entity6.ANSWER_STEPS_PART6 = "";
                            entity6.ANSWER_STEPS_PART7 = "";
                            entity6.ANSWER_STEPS_PART8 = "";
                            entity6.ANSWER_STEPS_PART9 = "";
                            entity6.ANSWER_STEPS_PART10 = "";
                            entity6.ANSWER_STEPS_IMG3 = "";
                            entity6.ANSWER_STEPS_IMG4 = "";
                            entity6.ANSWER_STEPS_IMG5 = "";
                            entity6.ANSWER_STEPS_IMG6 = "";
                            entity6.ANSWER_STEPS_IMG7 = "";
                            entity6.ANSWER_STEPS_IMG8 = "";
                            entity6.ANSWER_STEPS_IMG9 = "";
                            entity6.ANSWER_STEPS_IMG10 = "";
                            entity6.STATUS = "A";
                            entity6.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity6);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 13:
                            tbl_content_answer_steps entity7 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file79 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file80 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file80 != null && file79.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file80.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file79.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file81 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file82 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file82 != null && file81.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file82.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file81.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file83 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file84 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file84 != null && file83.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file84.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file83.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file85 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file86 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file86 != null && file85.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file86.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file85.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity7.ID_CONTENT_ANSWER = num2;
                            entity7.STEPNO = num3;
                            entity7.ID_THEME = new int?(13);
                            entity7.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity7.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity7.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity7.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity7.ANSWER_STEPS_PART5 = "";
                            entity7.ANSWER_STEPS_PART6 = "";
                            entity7.ANSWER_STEPS_PART7 = "";
                            entity7.ANSWER_STEPS_PART8 = "";
                            entity7.ANSWER_STEPS_PART9 = "";
                            entity7.ANSWER_STEPS_PART10 = "";
                            entity7.ANSWER_STEPS_IMG5 = "";
                            entity7.ANSWER_STEPS_IMG6 = "";
                            entity7.ANSWER_STEPS_IMG7 = "";
                            entity7.ANSWER_STEPS_IMG8 = "";
                            entity7.ANSWER_STEPS_IMG9 = "";
                            entity7.ANSWER_STEPS_IMG10 = "";
                            entity7.STATUS = "A";
                            entity7.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity7);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 14:
                            tbl_content_answer_steps entity8 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file87 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file88 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file88 != null && file87.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file88.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file87.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file89 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file90 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file90 != null && file89.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file90.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file89.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file91 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file92 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file92 != null && file91.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file92.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file91.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file93 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file94 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file94 != null && file93.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file94.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file93.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file95 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                HttpPostedFile file96 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                if (file96 != null && file95.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file96.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)index + extension);
                                    file95.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file97 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                HttpPostedFile file98 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                if (file98 != null && file97.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file98.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)index + extension);
                                    file97.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity8.ID_CONTENT_ANSWER = num2;
                            entity8.STEPNO = num3;
                            entity8.ID_THEME = new int?(14);
                            entity8.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity8.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity8.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity8.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity8.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)index + "-part-5"].ToString();
                            entity8.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)index + "-part-6"].ToString();
                            entity8.ANSWER_STEPS_PART7 = "";
                            entity8.ANSWER_STEPS_PART8 = "";
                            entity8.ANSWER_STEPS_PART9 = "";
                            entity8.ANSWER_STEPS_PART10 = "";
                            entity8.ANSWER_STEPS_IMG7 = "";
                            entity8.ANSWER_STEPS_IMG8 = "";
                            entity8.ANSWER_STEPS_IMG9 = "";
                            entity8.ANSWER_STEPS_IMG10 = "";
                            entity8.STATUS = "A";
                            entity8.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity8);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 15:
                            tbl_content_answer_steps entity9 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file99 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file100 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file100 != null && file99.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file100.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file99.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file101 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file102 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file102 != null && file101.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file102.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file101.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file103 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file104 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file104 != null && file103.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file104.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file103.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file105 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file106 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file106 != null && file105.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file106.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file105.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file107 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                HttpPostedFile file108 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                if (file108 != null && file107.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file108.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)index + extension);
                                    file107.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file109 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                HttpPostedFile file110 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                if (file110 != null && file109.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file110.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)index + extension);
                                    file109.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file111 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-7"];
                                HttpPostedFile file112 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-7"];
                                if (file112 != null && file111.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file112.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-7-" + (object)content_id + (object)index + extension);
                                    file111.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG7 = "step-7-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file113 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-8"];
                                HttpPostedFile file114 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-8"];
                                if (file114 != null && file113.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file114.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-8-" + (object)content_id + (object)index + extension);
                                    file113.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG8 = "step-8-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity9.ID_CONTENT_ANSWER = num2;
                            entity9.STEPNO = num3;
                            entity9.ID_THEME = new int?(15);
                            entity9.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity9.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity9.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity9.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity9.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)index + "-part-5"].ToString();
                            entity9.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)index + "-part-6"].ToString();
                            entity9.ANSWER_STEPS_PART7 = this.Request.Form["step-" + (object)index + "-part-7"].ToString();
                            entity9.ANSWER_STEPS_PART8 = this.Request.Form["step-" + (object)index + "-part-8"].ToString();
                            entity9.ANSWER_STEPS_PART9 = "";
                            entity9.ANSWER_STEPS_PART10 = "";
                            entity9.ANSWER_STEPS_IMG9 = "";
                            entity9.ANSWER_STEPS_IMG10 = "";
                            entity9.STATUS = "A";
                            entity9.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity9);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 16:
                            tbl_content_answer_steps entity10 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file115 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file116 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file116 != null && file115.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file116.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file115.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file117 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file118 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file118 != null && file117.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file118.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file117.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file119 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file120 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file120 != null && file119.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file120.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file119.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity10.ID_CONTENT_ANSWER = num2;
                            entity10.STEPNO = num3;
                            entity10.ID_THEME = new int?(16);
                            entity10.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity10.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity10.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity10.ANSWER_STEPS_PART4 = "";
                            entity10.ANSWER_STEPS_PART5 = "";
                            entity10.ANSWER_STEPS_PART6 = "";
                            entity10.ANSWER_STEPS_PART7 = "";
                            entity10.ANSWER_STEPS_PART8 = "";
                            entity10.ANSWER_STEPS_PART9 = "";
                            entity10.ANSWER_STEPS_PART10 = "";
                            entity10.ANSWER_STEPS_IMG4 = "";
                            entity10.ANSWER_STEPS_IMG5 = "";
                            entity10.ANSWER_STEPS_IMG6 = "";
                            entity10.ANSWER_STEPS_IMG7 = "";
                            entity10.ANSWER_STEPS_IMG8 = "";
                            entity10.ANSWER_STEPS_IMG9 = "";
                            entity10.ANSWER_STEPS_IMG10 = "";
                            entity10.STATUS = "A";
                            entity10.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity10);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 17:
                            tbl_content_answer_steps entity11 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file121 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file122 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file122 != null && file121.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file122.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file121.SaveAs(filename);
                                    entity11.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file123 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file124 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file124 != null && file123.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file124.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file123.SaveAs(filename);
                                    entity11.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file125 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file126 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file126 != null && file125.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file126.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file125.SaveAs(filename);
                                    entity11.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file127 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file128 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file128 != null && file127.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file128.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file127.SaveAs(filename);
                                    entity11.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file129 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                HttpPostedFile file130 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                if (file130 != null && file129.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file130.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)index + extension);
                                    file129.SaveAs(filename);
                                    entity11.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file131 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                HttpPostedFile file132 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                if (file132 != null && file131.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file132.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)index + extension);
                                    file131.SaveAs(filename);
                                    entity11.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity11.ID_CONTENT_ANSWER = num2;
                            entity11.STEPNO = num3;
                            entity11.ID_THEME = new int?(17);
                            entity11.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity11.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity11.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity11.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity11.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)index + "-part-5"].ToString();
                            entity11.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)index + "-part-6"].ToString();
                            entity11.ANSWER_STEPS_PART7 = "";
                            entity11.ANSWER_STEPS_PART8 = "";
                            entity11.ANSWER_STEPS_PART9 = "";
                            entity11.ANSWER_STEPS_PART10 = "";
                            entity11.ANSWER_STEPS_IMG7 = "";
                            entity11.ANSWER_STEPS_IMG8 = "";
                            entity11.ANSWER_STEPS_IMG9 = "";
                            entity11.ANSWER_STEPS_IMG10 = "";
                            entity11.STATUS = "A";
                            entity11.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity11);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 18:
                            tbl_content_answer_steps entity12 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file133 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file134 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file134 != null)
                                {
                                    string extension = Path.GetExtension(file134.FileName);
                                    if (file133.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                        file133.SaveAs(filename);
                                        entity12.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity12.ID_CONTENT_ANSWER = num2;
                            entity12.STEPNO = num3;
                            entity12.ID_THEME = new int?(18);
                            entity12.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity12.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString();
                            entity12.ANSWER_STEPS_PART3 = "";
                            entity12.ANSWER_STEPS_PART4 = "";
                            entity12.ANSWER_STEPS_PART5 = "";
                            entity12.ANSWER_STEPS_PART6 = "";
                            entity12.ANSWER_STEPS_PART7 = "";
                            entity12.ANSWER_STEPS_PART8 = "";
                            entity12.ANSWER_STEPS_PART9 = "";
                            entity12.ANSWER_STEPS_PART10 = "";
                            entity12.ANSWER_STEPS_IMG2 = "";
                            entity12.ANSWER_STEPS_IMG3 = "";
                            entity12.ANSWER_STEPS_IMG4 = "";
                            entity12.ANSWER_STEPS_IMG5 = "";
                            entity12.ANSWER_STEPS_IMG6 = "";
                            entity12.ANSWER_STEPS_IMG7 = "";
                            entity12.ANSWER_STEPS_IMG8 = "";
                            entity12.ANSWER_STEPS_IMG9 = "";
                            entity12.ANSWER_STEPS_IMG10 = "";
                            entity12.STATUS = "A";
                            entity12.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity12);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 19:
                            tbl_content_answer_steps entity13 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file135 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file136 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file136 != null)
                                {
                                    string extension = Path.GetExtension(file136.FileName);
                                    if (file135.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                        file135.SaveAs(filename);
                                        entity13.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                    }
                                }
                                HttpPostedFile file137 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file138 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file138 != null)
                                {
                                    string extension = Path.GetExtension(file138.FileName);
                                    if (file137.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                        file137.SaveAs(filename);
                                        entity13.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity13.ID_CONTENT_ANSWER = num2;
                            entity13.STEPNO = num3;
                            entity13.ID_THEME = new int?(19);
                            entity13.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity13.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString();
                            entity13.ANSWER_STEPS_PART3 = "";
                            entity13.ANSWER_STEPS_PART4 = "";
                            entity13.ANSWER_STEPS_PART5 = "";
                            entity13.ANSWER_STEPS_PART6 = "";
                            entity13.ANSWER_STEPS_PART7 = "";
                            entity13.ANSWER_STEPS_PART8 = "";
                            entity13.ANSWER_STEPS_PART9 = "";
                            entity13.ANSWER_STEPS_PART10 = "";
                            entity13.ANSWER_STEPS_IMG3 = "";
                            entity13.ANSWER_STEPS_IMG4 = "";
                            entity13.ANSWER_STEPS_IMG5 = "";
                            entity13.ANSWER_STEPS_IMG6 = "";
                            entity13.ANSWER_STEPS_IMG7 = "";
                            entity13.ANSWER_STEPS_IMG8 = "";
                            entity13.ANSWER_STEPS_IMG9 = "";
                            entity13.ANSWER_STEPS_IMG10 = "";
                            entity13.STATUS = "A";
                            entity13.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity13);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 20:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = num2,
                                STEPNO = num3,
                                ID_THEME = new int?(20),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 21:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = num2,
                                STEPNO = num3,
                                ID_THEME = new int?(21),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 22:
                            tbl_content_answer_steps entity14 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file139 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file140 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file140 != null)
                                {
                                    string extension = Path.GetExtension(file140.FileName);
                                    if (file139.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                        file139.SaveAs(filename);
                                        entity14.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity14.ID_CONTENT_ANSWER = num2;
                            entity14.STEPNO = num3;
                            entity14.ID_THEME = new int?(22);
                            entity14.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity14.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString();
                            entity14.ANSWER_STEPS_PART3 = "";
                            entity14.ANSWER_STEPS_PART4 = "";
                            entity14.ANSWER_STEPS_PART5 = "";
                            entity14.ANSWER_STEPS_PART6 = "";
                            entity14.ANSWER_STEPS_PART7 = "";
                            entity14.ANSWER_STEPS_PART8 = "";
                            entity14.ANSWER_STEPS_PART9 = "";
                            entity14.ANSWER_STEPS_PART10 = "";
                            entity14.ANSWER_STEPS_IMG2 = "";
                            entity14.ANSWER_STEPS_IMG3 = "";
                            entity14.ANSWER_STEPS_IMG4 = "";
                            entity14.ANSWER_STEPS_IMG5 = "";
                            entity14.ANSWER_STEPS_IMG6 = "";
                            entity14.ANSWER_STEPS_IMG7 = "";
                            entity14.ANSWER_STEPS_IMG8 = "";
                            entity14.ANSWER_STEPS_IMG9 = "";
                            entity14.ANSWER_STEPS_IMG10 = "";
                            entity14.STATUS = "A";
                            entity14.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity14);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 23:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = num2,
                                STEPNO = num3,
                                ID_THEME = new int?(23),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 24:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = num2,
                                STEPNO = num3,
                                ID_THEME = new int?(24),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = "",
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 25:
                            tbl_content_answer_steps entity15 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file141 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file142 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file142 != null)
                                {
                                    string extension = Path.GetExtension(file142.FileName);
                                    if (file141.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)index + extension);
                                        file141.SaveAs(filename);
                                        entity15.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity15.ID_CONTENT_ANSWER = num2;
                            entity15.STEPNO = num3;
                            entity15.ID_THEME = new int?(25);
                            entity15.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity15.ANSWER_STEPS_PART2 = "";
                            entity15.ANSWER_STEPS_PART3 = "";
                            entity15.ANSWER_STEPS_PART4 = "";
                            entity15.ANSWER_STEPS_PART5 = "";
                            entity15.ANSWER_STEPS_PART6 = "";
                            entity15.ANSWER_STEPS_PART7 = "";
                            entity15.ANSWER_STEPS_PART8 = "";
                            entity15.ANSWER_STEPS_PART9 = "";
                            entity15.ANSWER_STEPS_PART10 = "";
                            entity15.ANSWER_STEPS_IMG2 = "";
                            entity15.ANSWER_STEPS_IMG3 = "";
                            entity15.ANSWER_STEPS_IMG4 = "";
                            entity15.ANSWER_STEPS_IMG5 = "";
                            entity15.ANSWER_STEPS_IMG6 = "";
                            entity15.ANSWER_STEPS_IMG7 = "";
                            entity15.ANSWER_STEPS_IMG8 = "";
                            entity15.ANSWER_STEPS_IMG9 = "";
                            entity15.ANSWER_STEPS_IMG10 = "";
                            entity15.STATUS = "A";
                            entity15.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity15);
                            this.db.SaveChanges();
                            ++num3;
                            break;
                        case 26:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ANSWER_STEPS_IMG1 = this.Request.Form["step-" + (object)index + "-image"].ToString(),
                                ID_CONTENT_ANSWER = num2,
                                STEPNO = num3,
                                ID_THEME = new int?(26),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = "",
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num3;
                            break;
                    }
                }
                if (!str3.Equals("0"))
                {
                    this.db.tbl_content_link.Add(new tbl_content_link()
                    {
                        ID_CONTENT_CHILD = content_id,
                        ID_CONTENT_PARENT = int32_1,
                        ID_LINK_TYPE = Convert.ToInt32(str3),
                        STATUS = "A",
                        UPDATED_DATE_TIME = DateTime.Now
                    });
                    this.db.SaveChanges();
                    if (int32_1 > 0)
                    {
                        ++entity1.LINK_COUNT;
                        this.db.tbl_content.Attach(entity1);
                        this.db.Entry<tbl_content>(entity1).Property<int>((Expression<Func<tbl_content, int>>)(x => x.LINK_COUNT)).IsModified = true;
                        this.db.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                if (cid == 0)
                    return (ActionResult)this.RedirectToAction("display_content", "dashboard");
                if (cid != 0)
                {
                    new addCMS_CategoryModel().delete_content(cid);
                    return (ActionResult)this.RedirectToAction("ContentError", "contentDashboard");
                }
            }
            switch (str2)
            {
                case "Save and Exit":
                    return (ActionResult)this.RedirectToAction("LoadContent", "contentDashboard", (object)new
                    {
                        id = content_id
                    });
                case "Save and Add Lead-out to Parent":
                    return (ActionResult)this.RedirectToAction("linkContentCreation", (object)new
                    {
                        id = int32_1
                    });
                default:
                    return (ActionResult)this.RedirectToAction("linkContentCreation", (object)new
                    {
                        id = content_id
                    });
            }
        }

        [RoleAccessController(KEY = 6)]
        [ValidateInput(false)]
        public ActionResult editContent(int id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content content = this.db.tbl_content.Find(new object[1]
            {
        (object) id
            });
            tbl_content_answer answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
            List<tbl_content_answer_steps> list1 = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).OrderBy<tbl_content_answer_steps, int>((Expression<Func<tbl_content_answer_steps, int>>)(t => t.STEPNO)).ToList<tbl_content_answer_steps>();
            List<tbl_content_metadata> list2 = this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_content_metadata>();
            List<tbl_content_level> list3 = this.db.tbl_content_level.ToList<tbl_content_level>();
            List<tbl_content_organization_mapping> list4 = this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == content.ID_CONTENT && t.id_organization == orgid)).ToList<tbl_content_organization_mapping>();
            this.ViewData["CONTENT"] = (object)content;
            this.ViewData["ANSWER"] = (object)answer;
            this.ViewData["STEPS"] = (object)list1;
            this.ViewData["METADATA"] = (object)list2;
            this.ViewData["select-level"] = (object)list3;
            this.ViewData["select-category"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            this.ViewData["mapping"] = (object)list4;
            return content.ID_THEME != 15 ? (ActionResult)this.View() : (ActionResult)this.RedirectToAction("rich_text_edit", "contentDashboard", (object)new
            {
                id = id
            });
        }

        public ActionResult rich_text_edit(int id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content content = this.db.tbl_content.Find(new object[1]
            {
        (object) id
            });
            tbl_content_answer answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
            List<tbl_content_answer_steps> list1 = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).OrderBy<tbl_content_answer_steps, int>((Expression<Func<tbl_content_answer_steps, int>>)(t => t.STEPNO)).ToList<tbl_content_answer_steps>();
            List<tbl_content_metadata> list2 = this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_content_metadata>();
            List<tbl_content_level> list3 = this.db.tbl_content_level.ToList<tbl_content_level>();
            List<tbl_content_organization_mapping> list4 = this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == content.ID_CONTENT && t.id_organization == orgid)).ToList<tbl_content_organization_mapping>();
            this.ViewData["CONTENT"] = (object)content;
            this.ViewData["ANSWER"] = (object)answer;
            this.ViewData["STEPS"] = (object)list1;
            this.ViewData["METADATA"] = (object)list2;
            this.ViewData["select-level"] = (object)list3;
            this.ViewData["select-category"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            this.ViewData["conId"] = (object)id;
            this.ViewData["mapping"] = (object)list4;
            return (ActionResult)this.View();
        }

        [HttpPost]
        public ActionResult updateContent()
        {
            int cid = 0;
            ActionResult action;
            try
            {
                UserSession content1 = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int OID = Convert.ToInt32(content1.id_ORGANIZATION);
                int content_id = Convert.ToInt32(this.Request.Form["content-id"]);
                tbl_content content = this.db.tbl_content.Find(new object[1]
                {
          (object) content_id
                });
                content.CONTENT_HEADER = this.Request.Form["t-header"].ToString();
                content.CONTENT_QUESTION = this.Request.Form["t-quetion"].ToString();
                content.CONTENT_TITLE = this.Request.Form["t-title"].ToString();
                content.ID_CONTENT_LEVEL = Convert.ToInt32(this.Request.Form["select-level"].ToString());
                string dateString = this.Request.Form["content-expiry"].ToString();
                content.EXPIRY_DATE = new DateTime?(new Utility().StringToDatetime(dateString));
                DateTime now = DateTime.Now;
                this.db.SaveChanges();
                Convert.ToInt32(content1.id_ORGANIZATION);
                cid = content_id;
                int int32_1 = Convert.ToInt32(this.Request.Form["hid-category"]);
                for (int index = 1; index <= int32_1; ++index)
                {
                    string str = Convert.ToString(this.Request.Form["add-con-category-" + (object)index]);
                    if (!string.IsNullOrEmpty(str))
                    {
                        int citd = Convert.ToInt32(str);
                        if (this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_category == citd && t.id_content == content_id && t.id_organization == OID)).FirstOrDefault<tbl_content_organization_mapping>() == null)
                        {
                            this.db.tbl_content_organization_mapping.Add(new tbl_content_organization_mapping()
                            {
                                id_organization = Convert.ToInt32(content1.id_ORGANIZATION),
                                id_category = citd,
                                id_content = content.ID_CONTENT,
                                status = "A",
                                updated_date_time = new DateTime?(DateTime.Now)
                            });
                            this.db.SaveChanges();
                        }
                    }
                }
                int idTheme = content.ID_THEME;
                tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == content.ID_CONTENT)).FirstOrDefault<tbl_content_answer>();
                if (idTheme == 1)
                {
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                    tblContentAnswer.CONTENT_ANSWER_IMG1 = "";
                    tblContentAnswer.CONTENT_ANSWER2 = "";
                    tblContentAnswer.CONTENT_ANSWER3 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG2 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG3 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 2)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file1 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        HttpPostedFile file2 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        if (file2 != null && file1.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file2.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                            file1.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                        }
                    }
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 15)
                {
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Unvalidated.Form["content_ans"].ToString();
                    tblContentAnswer.CONTENT_ANSWER_IMG1 = "";
                    tblContentAnswer.CONTENT_ANSWER2 = "";
                    tblContentAnswer.CONTENT_ANSWER3 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG2 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG3 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 16)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file3 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        HttpPostedFile file4 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        if (file4 != null && file3.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file4.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                            file3.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                        }
                    }
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 17)
                {
                    tblContentAnswer.CONTENT_ANSWER_IMG1 = this.Request.Form["t-btn"].ToString();
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 3)
                {
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 4)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file5 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        HttpPostedFile file6 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        if (file6 != null)
                        {
                            string extension = Path.GetExtension(file6.FileName);
                            if (file5.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file5.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                        }
                    }
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 5)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file7 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        HttpPostedFile file8 = System.Web.HttpContext.Current.Request.Files["t-btn"];
                        if (file8 != null)
                        {
                            string extension = Path.GetExtension(file8.FileName);
                            if (file7.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file7.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                        }
                    }
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 6)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file9 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        HttpPostedFile file10 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        if (file10 != null && file9.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file10.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                            file9.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                        }
                        HttpPostedFile file11 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        HttpPostedFile file12 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        if (file12 != null && file11.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file12.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                            file11.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                        }
                    }
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 7)
                {
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                    tblContentAnswer.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                    this.db.SaveChanges();
                }
                if (idTheme == 8)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file13 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        HttpPostedFile file14 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        if (file14 != null)
                        {
                            string extension = Path.GetExtension(file14.FileName);
                            if (file13.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file13.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                        }
                        HttpPostedFile file15 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        HttpPostedFile file16 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        if (file16 != null)
                        {
                            string extension = Path.GetExtension(file16.FileName);
                            if (file15.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                file15.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                            }
                        }
                    }
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = "";
                    tblContentAnswer.CONTENT_ANSWER4 = "";
                    tblContentAnswer.CONTENT_ANSWER5 = "";
                    tblContentAnswer.CONTENT_ANSWER6 = "";
                    tblContentAnswer.CONTENT_ANSWER7 = "";
                    tblContentAnswer.CONTENT_ANSWER8 = "";
                    tblContentAnswer.CONTENT_ANSWER9 = "";
                    tblContentAnswer.CONTENT_ANSWER10 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG3 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG4 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG5 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG6 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG7 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG8 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG9 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG10 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 9)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file17 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        HttpPostedFile file18 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        if (file18 != null)
                        {
                            string extension = Path.GetExtension(file18.FileName);
                            if (file17.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                                file17.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                            }
                        }
                        HttpPostedFile file19 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        HttpPostedFile file20 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        if (file20 != null)
                        {
                            string extension = Path.GetExtension(file20.FileName);
                            if (file19.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                                file19.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                            }
                        }
                        HttpPostedFile file21 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        HttpPostedFile file22 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        if (file22 != null)
                        {
                            string extension = Path.GetExtension(file22.FileName);
                            if (file21.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                                file21.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                            }
                        }
                        HttpPostedFile file23 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        HttpPostedFile file24 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        if (file24 != null)
                        {
                            string extension = Path.GetExtension(file24.FileName);
                            if (file23.ContentLength > 0)
                            {
                                if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                    Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                                file23.SaveAs(filename);
                                tblContentAnswer.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                            }
                        }
                    }
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                    tblContentAnswer.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                    tblContentAnswer.CONTENT_ANSWER5 = "";
                    tblContentAnswer.CONTENT_ANSWER6 = "";
                    tblContentAnswer.CONTENT_ANSWER7 = "";
                    tblContentAnswer.CONTENT_ANSWER8 = "";
                    tblContentAnswer.CONTENT_ANSWER9 = "";
                    tblContentAnswer.CONTENT_ANSWER10 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG5 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG6 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG7 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG8 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG9 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG10 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 10)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file25 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        HttpPostedFile file26 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        if (file26 != null && file25.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file26.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                            file25.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                        }
                        HttpPostedFile file27 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        HttpPostedFile file28 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        if (file28 != null && file27.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file28.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                            file27.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                        }
                        HttpPostedFile file29 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        HttpPostedFile file30 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        if (file30 != null && file29.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file30.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                            file29.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                        }
                        HttpPostedFile file31 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        HttpPostedFile file32 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        if (file32 != null && file31.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file32.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                            file31.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                        }
                        HttpPostedFile file33 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                        HttpPostedFile file34 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                        if (file34 != null && file33.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file34.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-5" + extension);
                            file33.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG5 = content_id.ToString() + "-5" + extension;
                        }
                        HttpPostedFile file35 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                        HttpPostedFile file36 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                        if (file36 != null && file35.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file36.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-6" + extension);
                            file35.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG6 = content_id.ToString() + "-6" + extension;
                        }
                    }
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                    tblContentAnswer.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                    tblContentAnswer.CONTENT_ANSWER5 = this.Request.Form["t-content-5"].ToString();
                    tblContentAnswer.CONTENT_ANSWER6 = this.Request.Form["t-content-6"].ToString();
                    tblContentAnswer.CONTENT_ANSWER7 = "";
                    tblContentAnswer.CONTENT_ANSWER8 = "";
                    tblContentAnswer.CONTENT_ANSWER9 = "";
                    tblContentAnswer.CONTENT_ANSWER10 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG7 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG8 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG9 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG10 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 11)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file37 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        HttpPostedFile file38 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        if (file38 != null && file37.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file38.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                            file37.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                        }
                        HttpPostedFile file39 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        HttpPostedFile file40 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        if (file40 != null && file39.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file40.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                            file39.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                        }
                        HttpPostedFile file41 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        HttpPostedFile file42 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        if (file42 != null && file41.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file42.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                            file41.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                        }
                        HttpPostedFile file43 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        HttpPostedFile file44 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        if (file44 != null && file43.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file44.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                            file43.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                        }
                        HttpPostedFile file45 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                        HttpPostedFile file46 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                        if (file46 != null && file45.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file46.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-5" + extension);
                            file45.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG5 = content_id.ToString() + "-5" + extension;
                        }
                        HttpPostedFile file47 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                        HttpPostedFile file48 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                        if (file48 != null && file47.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file48.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-6" + extension);
                            file47.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG6 = content_id.ToString() + "-6" + extension;
                        }
                        HttpPostedFile file49 = System.Web.HttpContext.Current.Request.Files["t-btn-7"];
                        HttpPostedFile file50 = System.Web.HttpContext.Current.Request.Files["t-btn-7"];
                        if (file50 != null && file49.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file50.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-7" + extension);
                            file49.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG7 = content_id.ToString() + "-7" + extension;
                        }
                        HttpPostedFile file51 = System.Web.HttpContext.Current.Request.Files["t-btn-8"];
                        HttpPostedFile file52 = System.Web.HttpContext.Current.Request.Files["t-btn-8"];
                        if (file52 != null && file51.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file52.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-8" + extension);
                            file51.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG8 = content_id.ToString() + "-8" + extension;
                        }
                    }
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                    tblContentAnswer.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                    tblContentAnswer.CONTENT_ANSWER5 = this.Request.Form["t-content-5"].ToString();
                    tblContentAnswer.CONTENT_ANSWER6 = this.Request.Form["t-content-6"].ToString();
                    tblContentAnswer.CONTENT_ANSWER7 = this.Request.Form["t-content-7"].ToString();
                    tblContentAnswer.CONTENT_ANSWER8 = this.Request.Form["t-content-8"].ToString();
                    tblContentAnswer.CONTENT_ANSWER9 = "";
                    tblContentAnswer.CONTENT_ANSWER10 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG9 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG10 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 12)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file53 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        HttpPostedFile file54 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        if (file54 != null && file53.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file54.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                            file53.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                        }
                        HttpPostedFile file55 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        HttpPostedFile file56 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        if (file56 != null && file55.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file56.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                            file55.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                        }
                        HttpPostedFile file57 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        HttpPostedFile file58 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        if (file58 != null && file57.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file58.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                            file57.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                        }
                    }
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                    tblContentAnswer.CONTENT_ANSWER4 = "";
                    tblContentAnswer.CONTENT_ANSWER5 = "";
                    tblContentAnswer.CONTENT_ANSWER6 = "";
                    tblContentAnswer.CONTENT_ANSWER7 = "";
                    tblContentAnswer.CONTENT_ANSWER8 = "";
                    tblContentAnswer.CONTENT_ANSWER9 = "";
                    tblContentAnswer.CONTENT_ANSWER10 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG4 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG5 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG6 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG7 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG8 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG9 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG10 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 13)
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file59 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        HttpPostedFile file60 = System.Web.HttpContext.Current.Request.Files["t-btn-1"];
                        if (file60 != null && file59.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file60.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-1" + extension);
                            file59.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG1 = content_id.ToString() + "-1" + extension;
                        }
                        HttpPostedFile file61 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        HttpPostedFile file62 = System.Web.HttpContext.Current.Request.Files["t-btn-2"];
                        if (file62 != null && file61.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file62.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-2" + extension);
                            file61.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG2 = content_id.ToString() + "-2" + extension;
                        }
                        HttpPostedFile file63 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        HttpPostedFile file64 = System.Web.HttpContext.Current.Request.Files["t-btn-3"];
                        if (file64 != null && file63.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file64.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-3" + extension);
                            file63.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG3 = content_id.ToString() + "-3" + extension;
                        }
                        HttpPostedFile file65 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        HttpPostedFile file66 = System.Web.HttpContext.Current.Request.Files["t-btn-4"];
                        if (file66 != null && file65.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file66.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-4" + extension);
                            file65.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG4 = content_id.ToString() + "-4" + extension;
                        }
                        HttpPostedFile file67 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                        HttpPostedFile file68 = System.Web.HttpContext.Current.Request.Files["t-btn-5"];
                        if (file68 != null && file67.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file68.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-5" + extension);
                            file67.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG5 = content_id.ToString() + "-5" + extension;
                        }
                        HttpPostedFile file69 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                        HttpPostedFile file70 = System.Web.HttpContext.Current.Request.Files["t-btn-6"];
                        if (file70 != null && file69.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file70.FileName);
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), content_id.ToString() + "-6" + extension);
                            file69.SaveAs(filename);
                            tblContentAnswer.CONTENT_ANSWER_IMG6 = content_id.ToString() + "-6" + extension;
                        }
                    }
                    tblContentAnswer.ID_CONTENT = content_id;
                    tblContentAnswer.CONTENT_ANSWER_TITLE = "";
                    tblContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                    tblContentAnswer.CONTENT_ANSWER_HEADER = content.CONTENT_HEADER;
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                    tblContentAnswer.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                    tblContentAnswer.CONTENT_ANSWER5 = this.Request.Form["t-content-5"].ToString();
                    tblContentAnswer.CONTENT_ANSWER6 = this.Request.Form["t-content-6"].ToString();
                    tblContentAnswer.CONTENT_ANSWER7 = "";
                    tblContentAnswer.CONTENT_ANSWER8 = "";
                    tblContentAnswer.CONTENT_ANSWER9 = "";
                    tblContentAnswer.CONTENT_ANSWER10 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG7 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG8 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG9 = "";
                    tblContentAnswer.CONTENT_ANSWER_IMG10 = "";
                    tblContentAnswer.STATUS = "A";
                    tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    this.db.SaveChanges();
                }
                if (idTheme == 14)
                {
                    tblContentAnswer.CONTENT_ANSWER1 = this.Request.Form["t-content-1"].ToString();
                    tblContentAnswer.CONTENT_ANSWER2 = this.Request.Form["t-content-2"].ToString();
                    tblContentAnswer.CONTENT_ANSWER3 = this.Request.Form["t-content-3"].ToString();
                    tblContentAnswer.CONTENT_ANSWER4 = this.Request.Form["t-content-4"].ToString();
                    this.db.SaveChanges();
                }
                int answer_id = tblContentAnswer.ID_CONTENT_ANSWER;
                string[] strArray = this.Request.Form["t-metadata"].ToString().Split(',');
                new addCMS_CategoryModel().delete_meta(answer_id);
                foreach (string str1 in strArray)
                {
                    string str = str1;
                    this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.CONTENT_METADATA.ToLower().Contains(str.ToLower()) && t.ID_CONTENT_ANSWER == answer_id)).FirstOrDefault<tbl_content_metadata>();
                    if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
                    {
                        this.db.tbl_content_metadata.Add(new tbl_content_metadata()
                        {
                            CONTENT_METADATA = str,
                            CONTENT_METADATA_COUNTER = 0,
                            ID_CONTENT_ANSWER = answer_id,
                            STATUS = "A",
                            UPDATEDTIME = DateTime.Now
                        });
                        this.db.SaveChanges();
                    }
                }
                int int32_2 = Convert.ToInt32(this.Request.Form["step-count"].ToString());
                int num = 1;
                for (int i = 1; i <= int32_2; i++)
                {
                    tbl_content_answer_steps contentAnswerSteps = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.STEPNO == i && t.ID_CONTENT_ANSWER == answer_id)).FirstOrDefault<tbl_content_answer_steps>();
                    int int32_3 = Convert.ToInt32(this.Request.Form["select-theme-step-" + (object)i].ToString());
                    if (contentAnswerSteps != null)
                    {
                        switch (int32_3)
                        {
                            case 8:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(8);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 9:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file71 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file72 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file72 != null)
                                    {
                                        string extension = Path.GetExtension(file72.FileName);
                                        if (file71.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)i + extension);
                                            file71.SaveAs(filename);
                                            contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(9);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 10:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(10);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part3"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 11:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file73 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file74 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file74 != null)
                                    {
                                        string extension = Path.GetExtension(file74.FileName);
                                        if (file73.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)i + extension);
                                            file73.SaveAs(filename);
                                            contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(11);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART2 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 12:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file75 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file76 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file76 != null && file75.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file76.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file75.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file77 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file78 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file78 != null && file77.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file78.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file77.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(12);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 13:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file79 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file80 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file80 != null && file79.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file80.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file79.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file81 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file82 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file82 != null && file81.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file82.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file81.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file83 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file84 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file84 != null && file83.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file84.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file83.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file85 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file86 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file86 != null && file85.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file86.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file85.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(13);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 14:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file87 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file88 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file88 != null && file87.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file88.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file87.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file89 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file90 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file90 != null && file89.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file90.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file89.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file91 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file92 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file92 != null && file91.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file92.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file91.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file93 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file94 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file94 != null && file93.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file94.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file93.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file95 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    HttpPostedFile file96 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    if (file96 != null && file95.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file96.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)i + extension);
                                        file95.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file97 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    HttpPostedFile file98 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    if (file98 != null && file97.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file98.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)i + extension);
                                        file97.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(14);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)i + "-part-5"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)i + "-part-6"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 15:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file99 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file100 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file100 != null && file99.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file100.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file99.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file101 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file102 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file102 != null && file101.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file102.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file101.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file103 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file104 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file104 != null && file103.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file104.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file103.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file105 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file106 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file106 != null && file105.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file106.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file105.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file107 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    HttpPostedFile file108 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    if (file108 != null && file107.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file108.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)i + extension);
                                        file107.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file109 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    HttpPostedFile file110 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    if (file110 != null && file109.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file110.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)i + extension);
                                        file109.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file111 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-7"];
                                    HttpPostedFile file112 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-7"];
                                    if (file112 != null && file111.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file112.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-7-" + (object)content_id + (object)i + extension);
                                        file111.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG7 = "step-7-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file113 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-8"];
                                    HttpPostedFile file114 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-8"];
                                    if (file114 != null && file113.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file114.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-8-" + (object)content_id + (object)i + extension);
                                        file113.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG8 = "step-8-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(15);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)i + "-part-5"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)i + "-part-6"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART7 = this.Request.Form["step-" + (object)i + "-part-7"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART8 = this.Request.Form["step-" + (object)i + "-part-8"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 16:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file115 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file116 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file116 != null && file115.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file116.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file115.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file117 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file118 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file118 != null && file117.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file118.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file117.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file119 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file120 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file120 != null && file119.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file120.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file119.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(16);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 17:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file121 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file122 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file122 != null && file121.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file122.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file121.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file123 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file124 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file124 != null && file123.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file124.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file123.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file125 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file126 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file126 != null && file125.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file126.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file125.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file127 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file128 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file128 != null && file127.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file128.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file127.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file129 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    HttpPostedFile file130 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    if (file130 != null && file129.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file130.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)i + extension);
                                        file129.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file131 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    HttpPostedFile file132 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    if (file132 != null && file131.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file132.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)i + extension);
                                        file131.SaveAs(filename);
                                        contentAnswerSteps.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(17);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)i + "-part-5"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)i + "-part-6"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 18:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(18);
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file133 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file134 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file134 != null)
                                    {
                                        string extension = Path.GetExtension(file134.FileName);
                                        if (file133.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                            file133.SaveAs(filename);
                                            contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 19:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(19);
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file135 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file136 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file136 != null)
                                    {
                                        string extension = Path.GetExtension(file136.FileName);
                                        if (file135.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                            file135.SaveAs(filename);
                                            contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                    HttpPostedFile file137 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file138 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file138 != null)
                                    {
                                        string extension = Path.GetExtension(file138.FileName);
                                        if (file137.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                            file137.SaveAs(filename);
                                            contentAnswerSteps.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 20:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(20);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 21:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(21);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 22:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(22);
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file139 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file140 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file140 != null)
                                    {
                                        string extension = Path.GetExtension(file140.FileName);
                                        if (file139.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                            file139.SaveAs(filename);
                                            contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 23:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(23);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 24:
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(24);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 25:
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file141 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file142 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file142 != null)
                                    {
                                        string extension = Path.GetExtension(file142.FileName);
                                        if (file141.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)i + extension);
                                            file141.SaveAs(filename);
                                            contentAnswerSteps.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(25);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 26:
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = this.Request.Form["step-" + (object)i + "-image"].ToString();
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(26);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 27:
                                contentAnswerSteps.ANSWER_STEPS_IMG1 = "";
                                contentAnswerSteps.ID_CONTENT_ANSWER = answer_id;
                                contentAnswerSteps.STEPNO = num;
                                contentAnswerSteps.ID_THEME = new int?(27);
                                contentAnswerSteps.ANSWER_STEPS_PART1 = this.Request.Unvalidated.Form["step-" + (object)i + "-part1"].ToString();
                                contentAnswerSteps.ANSWER_STEPS_PART2 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART3 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART4 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART5 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART6 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART7 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART8 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART9 = "";
                                contentAnswerSteps.ANSWER_STEPS_PART10 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG2 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG3 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG4 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG5 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG6 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG7 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG8 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG9 = "";
                                contentAnswerSteps.ANSWER_STEPS_IMG10 = "";
                                contentAnswerSteps.STATUS = "A";
                                contentAnswerSteps.UPDATEDDATETIME = DateTime.Now;
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            default:
                                continue;
                        }
                    }
                    else
                    {
                        switch (int32_3)
                        {
                            case 8:
                                this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                                {
                                    ID_CONTENT_ANSWER = answer_id,
                                    STEPNO = num,
                                    ID_THEME = new int?(8),
                                    ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString(),
                                    ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString(),
                                    ANSWER_STEPS_PART3 = "",
                                    ANSWER_STEPS_PART4 = "",
                                    ANSWER_STEPS_PART5 = "",
                                    ANSWER_STEPS_PART6 = "",
                                    ANSWER_STEPS_PART7 = "",
                                    ANSWER_STEPS_PART8 = "",
                                    ANSWER_STEPS_PART9 = "",
                                    ANSWER_STEPS_PART10 = "",
                                    ANSWER_STEPS_IMG1 = "",
                                    ANSWER_STEPS_IMG2 = "",
                                    ANSWER_STEPS_IMG3 = "",
                                    ANSWER_STEPS_IMG4 = "",
                                    ANSWER_STEPS_IMG5 = "",
                                    ANSWER_STEPS_IMG6 = "",
                                    ANSWER_STEPS_IMG7 = "",
                                    ANSWER_STEPS_IMG8 = "",
                                    ANSWER_STEPS_IMG9 = "",
                                    ANSWER_STEPS_IMG10 = "",
                                    STATUS = "A",
                                    UPDATEDDATETIME = DateTime.Now
                                });
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 9:
                                tbl_content_answer_steps entity1 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file143 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file144 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file144 != null)
                                    {
                                        string extension = Path.GetExtension(file144.FileName);
                                        if (file143.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)i + extension);
                                            file143.SaveAs(filename);
                                            entity1.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                entity1.ID_CONTENT_ANSWER = answer_id;
                                entity1.STEPNO = num;
                                entity1.ID_THEME = new int?(9);
                                entity1.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                entity1.ANSWER_STEPS_PART2 = "";
                                entity1.ANSWER_STEPS_PART3 = "";
                                entity1.ANSWER_STEPS_PART4 = "";
                                entity1.ANSWER_STEPS_PART5 = "";
                                entity1.ANSWER_STEPS_PART6 = "";
                                entity1.ANSWER_STEPS_PART7 = "";
                                entity1.ANSWER_STEPS_PART8 = "";
                                entity1.ANSWER_STEPS_PART9 = "";
                                entity1.ANSWER_STEPS_PART10 = "";
                                entity1.ANSWER_STEPS_IMG2 = "";
                                entity1.ANSWER_STEPS_IMG3 = "";
                                entity1.ANSWER_STEPS_IMG4 = "";
                                entity1.ANSWER_STEPS_IMG5 = "";
                                entity1.ANSWER_STEPS_IMG6 = "";
                                entity1.ANSWER_STEPS_IMG7 = "";
                                entity1.ANSWER_STEPS_IMG8 = "";
                                entity1.ANSWER_STEPS_IMG9 = "";
                                entity1.ANSWER_STEPS_IMG10 = "";
                                entity1.STATUS = "A";
                                entity1.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity1);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 10:
                                this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                                {
                                    ID_CONTENT_ANSWER = answer_id,
                                    STEPNO = num,
                                    ID_THEME = new int?(10),
                                    ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString(),
                                    ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString(),
                                    ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part3"].ToString(),
                                    ANSWER_STEPS_PART4 = "",
                                    ANSWER_STEPS_PART5 = "",
                                    ANSWER_STEPS_PART6 = "",
                                    ANSWER_STEPS_PART7 = "",
                                    ANSWER_STEPS_PART8 = "",
                                    ANSWER_STEPS_PART9 = "",
                                    ANSWER_STEPS_PART10 = "",
                                    ANSWER_STEPS_IMG1 = "",
                                    ANSWER_STEPS_IMG2 = "",
                                    ANSWER_STEPS_IMG3 = "",
                                    ANSWER_STEPS_IMG4 = "",
                                    ANSWER_STEPS_IMG5 = "",
                                    ANSWER_STEPS_IMG6 = "",
                                    ANSWER_STEPS_IMG7 = "",
                                    ANSWER_STEPS_IMG8 = "",
                                    ANSWER_STEPS_IMG9 = "",
                                    ANSWER_STEPS_IMG10 = "",
                                    STATUS = "A",
                                    UPDATEDDATETIME = DateTime.Now
                                });
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 11:
                                tbl_content_answer_steps entity2 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file145 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file146 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file146 != null)
                                    {
                                        string extension = Path.GetExtension(file146.FileName);
                                        if (file145.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)i + extension);
                                            file145.SaveAs(filename);
                                            entity2.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                entity2.ID_CONTENT_ANSWER = answer_id;
                                entity2.STEPNO = num;
                                entity2.ID_THEME = new int?(11);
                                entity2.ANSWER_STEPS_PART1 = "";
                                entity2.ANSWER_STEPS_PART2 = "";
                                entity2.ANSWER_STEPS_PART3 = "";
                                entity2.ANSWER_STEPS_PART4 = "";
                                entity2.ANSWER_STEPS_PART5 = "";
                                entity2.ANSWER_STEPS_PART6 = "";
                                entity2.ANSWER_STEPS_PART7 = "";
                                entity2.ANSWER_STEPS_PART8 = "";
                                entity2.ANSWER_STEPS_PART9 = "";
                                entity2.ANSWER_STEPS_PART10 = "";
                                entity2.ANSWER_STEPS_IMG2 = "";
                                entity2.ANSWER_STEPS_IMG3 = "";
                                entity2.ANSWER_STEPS_IMG4 = "";
                                entity2.ANSWER_STEPS_IMG5 = "";
                                entity2.ANSWER_STEPS_IMG6 = "";
                                entity2.ANSWER_STEPS_IMG7 = "";
                                entity2.ANSWER_STEPS_IMG8 = "";
                                entity2.ANSWER_STEPS_IMG9 = "";
                                entity2.ANSWER_STEPS_IMG10 = "";
                                entity2.STATUS = "A";
                                entity2.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity2);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 12:
                                tbl_content_answer_steps entity3 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file147 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file148 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file148 != null && file147.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file148.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file147.SaveAs(filename);
                                        entity3.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file149 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file150 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file150 != null && file149.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file150.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file149.SaveAs(filename);
                                        entity3.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                entity3.ID_CONTENT_ANSWER = answer_id;
                                entity3.STEPNO = num;
                                entity3.ID_THEME = new int?(12);
                                entity3.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                entity3.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                entity3.ANSWER_STEPS_PART3 = "";
                                entity3.ANSWER_STEPS_PART4 = "";
                                entity3.ANSWER_STEPS_PART5 = "";
                                entity3.ANSWER_STEPS_PART6 = "";
                                entity3.ANSWER_STEPS_PART7 = "";
                                entity3.ANSWER_STEPS_PART8 = "";
                                entity3.ANSWER_STEPS_PART9 = "";
                                entity3.ANSWER_STEPS_PART10 = "";
                                entity3.ANSWER_STEPS_IMG3 = "";
                                entity3.ANSWER_STEPS_IMG4 = "";
                                entity3.ANSWER_STEPS_IMG5 = "";
                                entity3.ANSWER_STEPS_IMG6 = "";
                                entity3.ANSWER_STEPS_IMG7 = "";
                                entity3.ANSWER_STEPS_IMG8 = "";
                                entity3.ANSWER_STEPS_IMG9 = "";
                                entity3.ANSWER_STEPS_IMG10 = "";
                                entity3.STATUS = "A";
                                entity3.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity3);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 13:
                                tbl_content_answer_steps entity4 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file151 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file152 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file152 != null && file151.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file152.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file151.SaveAs(filename);
                                        entity4.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file153 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file154 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file154 != null && file153.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file154.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file153.SaveAs(filename);
                                        entity4.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file155 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file156 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file156 != null && file155.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file156.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file155.SaveAs(filename);
                                        entity4.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file157 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file158 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file158 != null && file157.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file158.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file157.SaveAs(filename);
                                        entity4.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                entity4.ID_CONTENT_ANSWER = answer_id;
                                entity4.STEPNO = num;
                                entity4.ID_THEME = new int?(13);
                                entity4.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                entity4.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                entity4.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                entity4.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                entity4.ANSWER_STEPS_PART5 = "";
                                entity4.ANSWER_STEPS_PART6 = "";
                                entity4.ANSWER_STEPS_PART7 = "";
                                entity4.ANSWER_STEPS_PART8 = "";
                                entity4.ANSWER_STEPS_PART9 = "";
                                entity4.ANSWER_STEPS_PART10 = "";
                                entity4.ANSWER_STEPS_IMG5 = "";
                                entity4.ANSWER_STEPS_IMG6 = "";
                                entity4.ANSWER_STEPS_IMG7 = "";
                                entity4.ANSWER_STEPS_IMG8 = "";
                                entity4.ANSWER_STEPS_IMG9 = "";
                                entity4.ANSWER_STEPS_IMG10 = "";
                                entity4.STATUS = "A";
                                entity4.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity4);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 14:
                                tbl_content_answer_steps entity5 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file159 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file160 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file160 != null && file159.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file160.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file159.SaveAs(filename);
                                        entity5.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file161 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file162 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file162 != null && file161.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file162.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file161.SaveAs(filename);
                                        entity5.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file163 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file164 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file164 != null && file163.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file164.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file163.SaveAs(filename);
                                        entity5.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file165 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file166 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file166 != null && file165.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file166.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file165.SaveAs(filename);
                                        entity5.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file167 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    HttpPostedFile file168 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    if (file168 != null && file167.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file168.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)i + extension);
                                        file167.SaveAs(filename);
                                        entity5.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file169 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    HttpPostedFile file170 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    if (file170 != null && file169.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file170.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)i + extension);
                                        file169.SaveAs(filename);
                                        entity5.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                entity5.ID_CONTENT_ANSWER = answer_id;
                                entity5.STEPNO = num;
                                entity5.ID_THEME = new int?(14);
                                entity5.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                entity5.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                entity5.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                entity5.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                entity5.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)i + "-part-5"].ToString();
                                entity5.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)i + "-part-6"].ToString();
                                entity5.ANSWER_STEPS_PART7 = "";
                                entity5.ANSWER_STEPS_PART8 = "";
                                entity5.ANSWER_STEPS_PART9 = "";
                                entity5.ANSWER_STEPS_PART10 = "";
                                entity5.ANSWER_STEPS_IMG7 = "";
                                entity5.ANSWER_STEPS_IMG8 = "";
                                entity5.ANSWER_STEPS_IMG9 = "";
                                entity5.ANSWER_STEPS_IMG10 = "";
                                entity5.STATUS = "A";
                                entity5.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity5);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 15:
                                tbl_content_answer_steps entity6 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file171 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file172 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file172 != null && file171.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file172.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file171.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file173 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file174 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file174 != null && file173.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file174.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file173.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file175 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file176 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file176 != null && file175.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file176.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file175.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file177 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file178 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file178 != null && file177.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file178.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file177.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file179 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    HttpPostedFile file180 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    if (file180 != null && file179.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file180.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)i + extension);
                                        file179.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file181 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    HttpPostedFile file182 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    if (file182 != null && file181.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file182.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)i + extension);
                                        file181.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file183 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-7"];
                                    HttpPostedFile file184 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-7"];
                                    if (file184 != null && file183.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file184.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-7-" + (object)content_id + (object)i + extension);
                                        file183.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG7 = "step-7-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file185 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-8"];
                                    HttpPostedFile file186 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-8"];
                                    if (file186 != null && file185.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file186.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-8-" + (object)content_id + (object)i + extension);
                                        file185.SaveAs(filename);
                                        entity6.ANSWER_STEPS_IMG8 = "step-8-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                entity6.ID_CONTENT_ANSWER = answer_id;
                                entity6.STEPNO = num;
                                entity6.ID_THEME = new int?(15);
                                entity6.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                entity6.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                entity6.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                entity6.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                entity6.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)i + "-part-5"].ToString();
                                entity6.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)i + "-part-6"].ToString();
                                entity6.ANSWER_STEPS_PART7 = this.Request.Form["step-" + (object)i + "-part-7"].ToString();
                                entity6.ANSWER_STEPS_PART8 = this.Request.Form["step-" + (object)i + "-part-8"].ToString();
                                entity6.ANSWER_STEPS_PART9 = "";
                                entity6.ANSWER_STEPS_PART10 = "";
                                entity6.ANSWER_STEPS_IMG9 = "";
                                entity6.ANSWER_STEPS_IMG10 = "";
                                entity6.STATUS = "A";
                                entity6.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity6);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 16:
                                tbl_content_answer_steps entity7 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file187 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file188 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file188 != null && file187.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file188.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file187.SaveAs(filename);
                                        entity7.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file189 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file190 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file190 != null && file189.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file190.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file189.SaveAs(filename);
                                        entity7.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file191 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file192 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file192 != null && file191.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file192.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file191.SaveAs(filename);
                                        entity7.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                entity7.ID_CONTENT_ANSWER = answer_id;
                                entity7.STEPNO = num;
                                entity7.ID_THEME = new int?(16);
                                entity7.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                entity7.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                entity7.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                entity7.ANSWER_STEPS_PART4 = "";
                                entity7.ANSWER_STEPS_PART5 = "";
                                entity7.ANSWER_STEPS_PART6 = "";
                                entity7.ANSWER_STEPS_PART7 = "";
                                entity7.ANSWER_STEPS_PART8 = "";
                                entity7.ANSWER_STEPS_PART9 = "";
                                entity7.ANSWER_STEPS_PART10 = "";
                                entity7.ANSWER_STEPS_IMG4 = "";
                                entity7.ANSWER_STEPS_IMG5 = "";
                                entity7.ANSWER_STEPS_IMG6 = "";
                                entity7.ANSWER_STEPS_IMG7 = "";
                                entity7.ANSWER_STEPS_IMG8 = "";
                                entity7.ANSWER_STEPS_IMG9 = "";
                                entity7.ANSWER_STEPS_IMG10 = "";
                                entity7.STATUS = "A";
                                entity7.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity7);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 17:
                                tbl_content_answer_steps entity8 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file193 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file194 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file194 != null && file193.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file194.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                        file193.SaveAs(filename);
                                        entity8.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file195 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file196 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file196 != null && file195.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file196.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                        file195.SaveAs(filename);
                                        entity8.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file197 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    HttpPostedFile file198 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-3"];
                                    if (file198 != null && file197.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file198.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)i + extension);
                                        file197.SaveAs(filename);
                                        entity8.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file199 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    HttpPostedFile file200 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-4"];
                                    if (file200 != null && file199.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file200.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)i + extension);
                                        file199.SaveAs(filename);
                                        entity8.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file201 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    HttpPostedFile file202 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-5"];
                                    if (file202 != null && file201.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file202.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)i + extension);
                                        file201.SaveAs(filename);
                                        entity8.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)i + extension;
                                    }
                                    HttpPostedFile file203 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    HttpPostedFile file204 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-6"];
                                    if (file204 != null && file203.ContentLength > 0)
                                    {
                                        string extension = Path.GetExtension(file204.FileName);
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)i + extension);
                                        file203.SaveAs(filename);
                                        entity8.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)i + extension;
                                    }
                                }
                                entity8.ID_CONTENT_ANSWER = answer_id;
                                entity8.STEPNO = num;
                                entity8.ID_THEME = new int?(17);
                                entity8.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part-1"].ToString();
                                entity8.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part-2"].ToString();
                                entity8.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)i + "-part-3"].ToString();
                                entity8.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)i + "-part-4"].ToString();
                                entity8.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)i + "-part-5"].ToString();
                                entity8.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)i + "-part-6"].ToString();
                                entity8.ANSWER_STEPS_PART7 = "";
                                entity8.ANSWER_STEPS_PART8 = "";
                                entity8.ANSWER_STEPS_PART9 = "";
                                entity8.ANSWER_STEPS_PART10 = "";
                                entity8.ANSWER_STEPS_IMG7 = "";
                                entity8.ANSWER_STEPS_IMG8 = "";
                                entity8.ANSWER_STEPS_IMG9 = "";
                                entity8.ANSWER_STEPS_IMG10 = "";
                                entity8.STATUS = "A";
                                entity8.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity8);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 18:
                                tbl_content_answer_steps entity9 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file205 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file206 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file206 != null)
                                    {
                                        string extension = Path.GetExtension(file206.FileName);
                                        if (file205.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                            file205.SaveAs(filename);
                                            entity9.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                entity9.ID_CONTENT_ANSWER = answer_id;
                                entity9.STEPNO = num;
                                entity9.ID_THEME = new int?(18);
                                entity9.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                entity9.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                entity9.ANSWER_STEPS_PART3 = "";
                                entity9.ANSWER_STEPS_PART4 = "";
                                entity9.ANSWER_STEPS_PART5 = "";
                                entity9.ANSWER_STEPS_PART6 = "";
                                entity9.ANSWER_STEPS_PART7 = "";
                                entity9.ANSWER_STEPS_PART8 = "";
                                entity9.ANSWER_STEPS_PART9 = "";
                                entity9.ANSWER_STEPS_PART10 = "";
                                entity9.ANSWER_STEPS_IMG2 = "";
                                entity9.ANSWER_STEPS_IMG3 = "";
                                entity9.ANSWER_STEPS_IMG4 = "";
                                entity9.ANSWER_STEPS_IMG5 = "";
                                entity9.ANSWER_STEPS_IMG6 = "";
                                entity9.ANSWER_STEPS_IMG7 = "";
                                entity9.ANSWER_STEPS_IMG8 = "";
                                entity9.ANSWER_STEPS_IMG9 = "";
                                entity9.ANSWER_STEPS_IMG10 = "";
                                entity9.STATUS = "A";
                                entity9.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity9);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 19:
                                tbl_content_answer_steps entity10 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file207 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file208 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file208 != null)
                                    {
                                        string extension = Path.GetExtension(file208.FileName);
                                        if (file207.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                            file207.SaveAs(filename);
                                            entity10.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                    HttpPostedFile file209 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    HttpPostedFile file210 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-2"];
                                    if (file210 != null)
                                    {
                                        string extension = Path.GetExtension(file210.FileName);
                                        if (file209.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)i + extension);
                                            file209.SaveAs(filename);
                                            entity10.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                entity10.ID_CONTENT_ANSWER = answer_id;
                                entity10.STEPNO = num;
                                entity10.ID_THEME = new int?(19);
                                entity10.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                entity10.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                entity10.ANSWER_STEPS_PART3 = "";
                                entity10.ANSWER_STEPS_PART4 = "";
                                entity10.ANSWER_STEPS_PART5 = "";
                                entity10.ANSWER_STEPS_PART6 = "";
                                entity10.ANSWER_STEPS_PART7 = "";
                                entity10.ANSWER_STEPS_PART8 = "";
                                entity10.ANSWER_STEPS_PART9 = "";
                                entity10.ANSWER_STEPS_PART10 = "";
                                entity10.ANSWER_STEPS_IMG3 = "";
                                entity10.ANSWER_STEPS_IMG4 = "";
                                entity10.ANSWER_STEPS_IMG5 = "";
                                entity10.ANSWER_STEPS_IMG6 = "";
                                entity10.ANSWER_STEPS_IMG7 = "";
                                entity10.ANSWER_STEPS_IMG8 = "";
                                entity10.ANSWER_STEPS_IMG9 = "";
                                entity10.ANSWER_STEPS_IMG10 = "";
                                entity10.STATUS = "A";
                                entity10.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity10);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 20:
                                this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                                {
                                    ID_CONTENT_ANSWER = answer_id,
                                    STEPNO = num,
                                    ID_THEME = new int?(20),
                                    ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString(),
                                    ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString(),
                                    ANSWER_STEPS_PART3 = "",
                                    ANSWER_STEPS_PART4 = "",
                                    ANSWER_STEPS_PART5 = "",
                                    ANSWER_STEPS_PART6 = "",
                                    ANSWER_STEPS_PART7 = "",
                                    ANSWER_STEPS_PART8 = "",
                                    ANSWER_STEPS_PART9 = "",
                                    ANSWER_STEPS_PART10 = "",
                                    ANSWER_STEPS_IMG1 = "",
                                    ANSWER_STEPS_IMG2 = "",
                                    ANSWER_STEPS_IMG3 = "",
                                    ANSWER_STEPS_IMG4 = "",
                                    ANSWER_STEPS_IMG5 = "",
                                    ANSWER_STEPS_IMG6 = "",
                                    ANSWER_STEPS_IMG7 = "",
                                    ANSWER_STEPS_IMG8 = "",
                                    ANSWER_STEPS_IMG9 = "",
                                    ANSWER_STEPS_IMG10 = "",
                                    STATUS = "A",
                                    UPDATEDDATETIME = DateTime.Now
                                });
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 21:
                                this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                                {
                                    ID_CONTENT_ANSWER = answer_id,
                                    STEPNO = num,
                                    ID_THEME = new int?(21),
                                    ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString(),
                                    ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString(),
                                    ANSWER_STEPS_PART3 = "",
                                    ANSWER_STEPS_PART4 = "",
                                    ANSWER_STEPS_PART5 = "",
                                    ANSWER_STEPS_PART6 = "",
                                    ANSWER_STEPS_PART7 = "",
                                    ANSWER_STEPS_PART8 = "",
                                    ANSWER_STEPS_PART9 = "",
                                    ANSWER_STEPS_PART10 = "",
                                    ANSWER_STEPS_IMG1 = "",
                                    ANSWER_STEPS_IMG2 = "",
                                    ANSWER_STEPS_IMG3 = "",
                                    ANSWER_STEPS_IMG4 = "",
                                    ANSWER_STEPS_IMG5 = "",
                                    ANSWER_STEPS_IMG6 = "",
                                    ANSWER_STEPS_IMG7 = "",
                                    ANSWER_STEPS_IMG8 = "",
                                    ANSWER_STEPS_IMG9 = "",
                                    ANSWER_STEPS_IMG10 = "",
                                    STATUS = "A",
                                    UPDATEDDATETIME = DateTime.Now
                                });
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 22:
                                tbl_content_answer_steps entity11 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file211 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    HttpPostedFile file212 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn-1"];
                                    if (file212 != null)
                                    {
                                        string extension = Path.GetExtension(file212.FileName);
                                        if (file211.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)i + extension);
                                            file211.SaveAs(filename);
                                            entity11.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                entity11.ID_CONTENT_ANSWER = answer_id;
                                entity11.STEPNO = num;
                                entity11.ID_THEME = new int?(22);
                                entity11.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                entity11.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString();
                                entity11.ANSWER_STEPS_PART3 = "";
                                entity11.ANSWER_STEPS_PART4 = "";
                                entity11.ANSWER_STEPS_PART5 = "";
                                entity11.ANSWER_STEPS_PART6 = "";
                                entity11.ANSWER_STEPS_PART7 = "";
                                entity11.ANSWER_STEPS_PART8 = "";
                                entity11.ANSWER_STEPS_PART9 = "";
                                entity11.ANSWER_STEPS_PART10 = "";
                                entity11.ANSWER_STEPS_IMG2 = "";
                                entity11.ANSWER_STEPS_IMG3 = "";
                                entity11.ANSWER_STEPS_IMG4 = "";
                                entity11.ANSWER_STEPS_IMG5 = "";
                                entity11.ANSWER_STEPS_IMG6 = "";
                                entity11.ANSWER_STEPS_IMG7 = "";
                                entity11.ANSWER_STEPS_IMG8 = "";
                                entity11.ANSWER_STEPS_IMG9 = "";
                                entity11.ANSWER_STEPS_IMG10 = "";
                                entity11.STATUS = "A";
                                entity11.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity11);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 23:
                                this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                                {
                                    ID_CONTENT_ANSWER = answer_id,
                                    STEPNO = num,
                                    ID_THEME = new int?(23),
                                    ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString(),
                                    ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)i + "-part2"].ToString(),
                                    ANSWER_STEPS_PART3 = "",
                                    ANSWER_STEPS_PART4 = "",
                                    ANSWER_STEPS_PART5 = "",
                                    ANSWER_STEPS_PART6 = "",
                                    ANSWER_STEPS_PART7 = "",
                                    ANSWER_STEPS_PART8 = "",
                                    ANSWER_STEPS_PART9 = "",
                                    ANSWER_STEPS_PART10 = "",
                                    ANSWER_STEPS_IMG1 = "",
                                    ANSWER_STEPS_IMG2 = "",
                                    ANSWER_STEPS_IMG3 = "",
                                    ANSWER_STEPS_IMG4 = "",
                                    ANSWER_STEPS_IMG5 = "",
                                    ANSWER_STEPS_IMG6 = "",
                                    ANSWER_STEPS_IMG7 = "",
                                    ANSWER_STEPS_IMG8 = "",
                                    ANSWER_STEPS_IMG9 = "",
                                    ANSWER_STEPS_IMG10 = "",
                                    STATUS = "A",
                                    UPDATEDDATETIME = DateTime.Now
                                });
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 24:
                                this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                                {
                                    ID_CONTENT_ANSWER = answer_id,
                                    STEPNO = num,
                                    ID_THEME = new int?(24),
                                    ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString(),
                                    ANSWER_STEPS_PART2 = "",
                                    ANSWER_STEPS_PART3 = "",
                                    ANSWER_STEPS_PART4 = "",
                                    ANSWER_STEPS_PART5 = "",
                                    ANSWER_STEPS_PART6 = "",
                                    ANSWER_STEPS_PART7 = "",
                                    ANSWER_STEPS_PART8 = "",
                                    ANSWER_STEPS_PART9 = "",
                                    ANSWER_STEPS_PART10 = "",
                                    ANSWER_STEPS_IMG1 = "",
                                    ANSWER_STEPS_IMG2 = "",
                                    ANSWER_STEPS_IMG3 = "",
                                    ANSWER_STEPS_IMG4 = "",
                                    ANSWER_STEPS_IMG5 = "",
                                    ANSWER_STEPS_IMG6 = "",
                                    ANSWER_STEPS_IMG7 = "",
                                    ANSWER_STEPS_IMG8 = "",
                                    ANSWER_STEPS_IMG9 = "",
                                    ANSWER_STEPS_IMG10 = "",
                                    STATUS = "A",
                                    UPDATEDDATETIME = DateTime.Now
                                });
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 25:
                                tbl_content_answer_steps entity12 = new tbl_content_answer_steps();
                                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                                {
                                    HttpPostedFile file213 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    HttpPostedFile file214 = System.Web.HttpContext.Current.Request.Files["step-" + (object)i + "-btn"];
                                    if (file214 != null)
                                    {
                                        string extension = Path.GetExtension(file214.FileName);
                                        if (file213.ContentLength > 0)
                                        {
                                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content1.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)i + extension);
                                            file213.SaveAs(filename);
                                            entity12.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)i + extension;
                                        }
                                    }
                                }
                                entity12.ID_CONTENT_ANSWER = answer_id;
                                entity12.STEPNO = num;
                                entity12.ID_THEME = new int?(9);
                                entity12.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)i + "-part1"].ToString();
                                entity12.ANSWER_STEPS_PART2 = "";
                                entity12.ANSWER_STEPS_PART3 = "";
                                entity12.ANSWER_STEPS_PART4 = "";
                                entity12.ANSWER_STEPS_PART5 = "";
                                entity12.ANSWER_STEPS_PART6 = "";
                                entity12.ANSWER_STEPS_PART7 = "";
                                entity12.ANSWER_STEPS_PART8 = "";
                                entity12.ANSWER_STEPS_PART9 = "";
                                entity12.ANSWER_STEPS_PART10 = "";
                                entity12.ANSWER_STEPS_IMG2 = "";
                                entity12.ANSWER_STEPS_IMG3 = "";
                                entity12.ANSWER_STEPS_IMG4 = "";
                                entity12.ANSWER_STEPS_IMG5 = "";
                                entity12.ANSWER_STEPS_IMG6 = "";
                                entity12.ANSWER_STEPS_IMG7 = "";
                                entity12.ANSWER_STEPS_IMG8 = "";
                                entity12.ANSWER_STEPS_IMG9 = "";
                                entity12.ANSWER_STEPS_IMG10 = "";
                                entity12.STATUS = "A";
                                entity12.UPDATEDDATETIME = DateTime.Now;
                                this.db.tbl_content_answer_steps.Add(entity12);
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            case 27:
                                this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                                {
                                    ID_CONTENT_ANSWER = answer_id,
                                    STEPNO = num,
                                    ID_THEME = new int?(27),
                                    ANSWER_STEPS_PART1 = this.Request.Unvalidated.Form["step-" + (object)i + "-part1"].ToString(),
                                    ANSWER_STEPS_PART2 = "",
                                    ANSWER_STEPS_PART3 = "",
                                    ANSWER_STEPS_PART4 = "",
                                    ANSWER_STEPS_PART5 = "",
                                    ANSWER_STEPS_PART6 = "",
                                    ANSWER_STEPS_PART7 = "",
                                    ANSWER_STEPS_PART8 = "",
                                    ANSWER_STEPS_PART9 = "",
                                    ANSWER_STEPS_PART10 = "",
                                    ANSWER_STEPS_IMG1 = "",
                                    ANSWER_STEPS_IMG2 = "",
                                    ANSWER_STEPS_IMG3 = "",
                                    ANSWER_STEPS_IMG4 = "",
                                    ANSWER_STEPS_IMG5 = "",
                                    ANSWER_STEPS_IMG6 = "",
                                    ANSWER_STEPS_IMG7 = "",
                                    ANSWER_STEPS_IMG8 = "",
                                    ANSWER_STEPS_IMG9 = "",
                                    ANSWER_STEPS_IMG10 = "",
                                    STATUS = "A",
                                    UPDATEDDATETIME = DateTime.Now
                                });
                                this.db.SaveChanges();
                                ++num;
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (cid == 0)
                {
                    action = (ActionResult)this.RedirectToAction("display_content", "dashboard");
                    goto label_581;
                }
                else if (cid != 0)
                {
                    new addCMS_CategoryModel().delete_content(cid);
                    action = (ActionResult)this.RedirectToAction("ContentError", "contentDashboard");
                    goto label_581;
                }
            }
            return (ActionResult)this.RedirectToAction("display_content", "dashboard");
        label_581:
            return action;
        }

        public string removeCategoryLink(string id, string cnid)
        {
            int cid = Convert.ToInt32(id);
            int cntid = Convert.ToInt32(cnid);
            if (cntid > 0 && cid > 0)
            {
                int OID = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
                tbl_content_organization_mapping entity = this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_category == cid && t.id_content == cntid && t.id_organization == OID)).FirstOrDefault<tbl_content_organization_mapping>();
                if (entity != null)
                {
                    this.db.tbl_content_organization_mapping.Remove(entity);
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        [RoleAccessController(KEY = 5)]
        public ActionResult LoadContent(string id)
        {
            UserSession content1 = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content1.id_ORGANIZATION);
            int int32_1 = Convert.ToInt32(content1.ID_USER);
            int int32_2 = Convert.ToInt32(id);
            tbl_content content = this.db.tbl_content.Find(new object[1]
            {
        (object) int32_2
            });
            List<tbl_content_answer_steps> contentAnswerStepsList = new List<tbl_content_answer_steps>();
            List<tbl_content_metadata> tblContentMetadataList = new List<tbl_content_metadata>();
            List<tbl_content> tblContentList = new List<tbl_content>();
            tbl_content_answer content_answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == content.ID_CONTENT)).FirstOrDefault<tbl_content_answer>();
            List<tbl_content_answer_steps> list1 = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_answer_steps>();
            if (list1.Count > 0)
                contentAnswerStepsList = list1;
            List<tbl_content_metadata> list2 = this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_metadata>();
            List<tbl_content> list3 = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select ID_CONTENT_CHILD from tbl_content_link where ID_CONTENT_PARENT=" + (object)int32_2 + ") and content_owner=" + (object)orgid + "   ").ToList<tbl_content>();
            if (list2.Count > 0)
                tblContentMetadataList = list2;
            List<tbl_content_organization_mapping> list4 = this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == content.ID_CONTENT && t.id_organization == orgid)).ToList<tbl_content_organization_mapping>();
            List<tbl_category> tblCategoryList = new List<tbl_category>();
            foreach (tbl_content_organization_mapping organizationMapping in list4)
            {
                tbl_content_organization_mapping item = organizationMapping;
                tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>)(t => t.ID_CATEGORY == item.id_category && t.STATUS == "A")).FirstOrDefault<tbl_category>();
                tblCategoryList.Add(tblCategory);
            }
            List<tbl_content_type_link> list5 = this.db.tbl_content_type_link.Where<tbl_content_type_link>((Expression<Func<tbl_content_type_link, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_type_link>();
            string str1 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Banner/";
            tbl_content_banner cbanner = this.db.tbl_content_banner.Where<tbl_content_banner>((Expression<Func<tbl_content_banner, bool>>)(t => t.id_content == content.ID_CONTENT)).FirstOrDefault<tbl_content_banner>();
            tbl_banner tblBanner = new tbl_banner();
            if (cbanner != null)
                tblBanner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == cbanner.id_banner && t.status == "A")).FirstOrDefault<tbl_banner>();
            List<tbl_assessment_mapping> list6 = this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_content == (int?)content.ID_CONTENT && t.id_organization == (int?)orgid)).ToList<tbl_assessment_mapping>();
            List<tbl_assessment> tblAssessmentList = new List<tbl_assessment>();
            foreach (tbl_assessment_mapping assessmentMapping in list6)
            {
                tbl_assessment_mapping item = assessmentMapping;
                tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => (int?)t.id_assessment_sheet == item.id_assessment_sheet)).FirstOrDefault<tbl_assessment_sheet>();
                if (sheet != null)
                {
                    tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == sheet.id_assesment)).FirstOrDefault<tbl_assessment>();
                    if (tblAssessment != null)
                        tblAssessmentList.Add(tblAssessment);
                }
            }
            string str2 = ConfigurationManager.AppSettings["SERVERPATH"].ToString();
            new RoleBasedAccess().checkAccess(content1.action, 6);
            bool flag;
            if (content.ID_USER != int32_1)
            {
                int? cmdUserType = content1.USER.cmd_user_type;
                int num = 0;
                if ((cmdUserType.GetValueOrDefault() == num ? (cmdUserType.HasValue ? 1 : 0) : 0) == 0)
                {
                    flag = false;
                    goto label_22;
                }
            }
            flag = true;
        label_22:
            this.ViewData["accessflag"] = (object)flag;
            this.ViewData["banner"] = (object)tblBanner;
            this.ViewData["urls"] = (object)str1;
            this.ViewData["imgurl"] = (object)str2;
            this.ViewData["content_links"] = (object)list5;
            this.ViewData["users"] = (object)content1;
            this.ViewData["linked"] = (object)list3;
            this.ViewData["assessments"] = (object)tblAssessmentList;
            this.ViewData["content"] = (object)content;
            this.ViewData["content_answer"] = (object)content_answer;
            this.ViewData["answer_step"] = (object)contentAnswerStepsList;
            this.ViewData["metadata"] = (object)tblContentMetadataList;
            this.ViewData["category"] = (object)tblCategoryList;
            return (ActionResult)this.View();
        }

        [RoleAccessController(KEY = 7)]
        public ActionResult ContentApproval(string id)
        {
            UserSession content1 = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content1.id_ORGANIZATION);
            int int32 = Convert.ToInt32(id);
            tbl_content content = this.db.tbl_content.Find(new object[1]
            {
        (object) int32
            });
            List<tbl_content_answer_steps> contentAnswerStepsList = new List<tbl_content_answer_steps>();
            List<tbl_content_metadata> tblContentMetadataList = new List<tbl_content_metadata>();
            List<tbl_content> tblContentList = new List<tbl_content>();
            tbl_content_answer content_answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == content.ID_CONTENT)).FirstOrDefault<tbl_content_answer>();
            List<tbl_content_answer_steps> list1 = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_answer_steps>();
            if (list1.Count > 0)
                contentAnswerStepsList = list1;
            List<tbl_content_metadata> list2 = this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_metadata>();
            List<tbl_content> list3 = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select ID_CONTENT_CHILD from tbl_content_link where ID_CONTENT_PARENT=" + (object)int32 + ")  and content_owner=" + (object)orgid + "  ").ToList<tbl_content>();
            if (list2.Count > 0)
                tblContentMetadataList = list2;
            List<tbl_content_type_link> list4 = this.db.tbl_content_type_link.Where<tbl_content_type_link>((Expression<Func<tbl_content_type_link, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_type_link>();
            string str = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Banner/";
            tbl_content_banner cbanner = this.db.tbl_content_banner.Where<tbl_content_banner>((Expression<Func<tbl_content_banner, bool>>)(t => t.id_content == content.ID_CONTENT)).FirstOrDefault<tbl_content_banner>();
            tbl_banner tblBanner = new tbl_banner();
            if (cbanner != null)
                tblBanner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == cbanner.id_banner && t.status == "A")).FirstOrDefault<tbl_banner>();
            List<tbl_assessment_mapping> list5 = this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_content == (int?)content.ID_CONTENT && t.id_organization == (int?)orgid)).ToList<tbl_assessment_mapping>();
            List<tbl_assessment> tblAssessmentList = new List<tbl_assessment>();
            foreach (tbl_assessment_mapping assessmentMapping in list5)
            {
                tbl_assessment_mapping item = assessmentMapping;
                tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => (int?)t.id_assessment_sheet == item.id_assessment_sheet)).FirstOrDefault<tbl_assessment_sheet>();
                if (sheet != null)
                {
                    tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == sheet.id_assesment)).FirstOrDefault<tbl_assessment>();
                    if (tblAssessment != null)
                        tblAssessmentList.Add(tblAssessment);
                }
            }
            this.ViewData["banner"] = (object)tblBanner;
            this.ViewData["urls"] = (object)str;
            this.ViewData["content_links"] = (object)list4;
            this.ViewData["users"] = (object)content1;
            this.ViewData["linked"] = (object)list3;
            this.ViewData["assessments"] = (object)tblAssessmentList;
            this.ViewData["content"] = (object)content;
            this.ViewData["content_answer"] = (object)content_answer;
            this.ViewData["answer_step"] = (object)contentAnswerStepsList;
            this.ViewData["metadata"] = (object)tblContentMetadataList;
            return (ActionResult)this.View();
        }

        public ActionResult DeleteContentAll(int id)
        {
            this.Deletelinks(id);
            return (ActionResult)this.RedirectToAction("display_content", "dashboard");
        }

        public void Deletelinks(int id)
        {
            List<tbl_content_link> list1 = this.db.tbl_content_link.Where<tbl_content_link>((Expression<Func<tbl_content_link, bool>>)(t => t.ID_CONTENT_PARENT == id)).ToList<tbl_content_link>();
            if (list1.Count > 0)
            {
                foreach (tbl_content_link entity in list1.ToList<tbl_content_link>())
                {
                    this.db.tbl_content_link.Remove(entity);
                    this.db.SaveChanges();
                }
            }
            List<tbl_content_link> list2 = this.db.tbl_content_link.Where<tbl_content_link>((Expression<Func<tbl_content_link, bool>>)(t => t.ID_CONTENT_CHILD == id)).ToList<tbl_content_link>();
            if (list2 != null)
            {
                foreach (tbl_content_link entity in list2.ToList<tbl_content_link>())
                {
                    this.db.tbl_content_link.Remove(entity);
                    this.db.SaveChanges();
                }
            }
            this.DeleteContentId(id);
        }

        public void DeleteContentId(int id)
        {
            tbl_content content = this.db.tbl_content.Find(new object[1]
            {
        (object) id
            });
            if (content == null)
                return;
            tbl_content_answer answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
            if (answer != null)
            {
                List<tbl_content_metadata> list1 = this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_content_metadata>();
                if (list1 != null)
                {
                    list1.ForEach((Action<tbl_content_metadata>)(t => t.STATUS = "D"));
                    this.db.SaveChanges();
                }
                List<tbl_content_answer_steps> list2 = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_content_answer_steps>();
                if (list2 != null)
                {
                    list2.ForEach((Action<tbl_content_answer_steps>)(t => t.STATUS = "D"));
                    this.db.SaveChanges();
                }
                List<tbl_content_type_link> list3 = this.db.tbl_content_type_link.Where<tbl_content_type_link>((Expression<Func<tbl_content_type_link, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_content_type_link>();
                if (list3 != null)
                {
                    list3.ForEach((Action<tbl_content_type_link>)(t => t.STATUS = "D"));
                    this.db.SaveChanges();
                }
                List<tbl_feedback_bank_link> list4 = this.db.tbl_feedback_bank_link.Where<tbl_feedback_bank_link>((Expression<Func<tbl_feedback_bank_link, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_feedback_bank_link>();
                if (list4 != null)
                {
                    list4.ForEach((Action<tbl_feedback_bank_link>)(t => t.STATUS = "D"));
                    this.db.SaveChanges();
                }
                answer.STATUS = "D";
                this.db.SaveChanges();
            }
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content_organization_mapping organizationMapping = this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == content.ID_CONTENT && t.id_organization == oid)).FirstOrDefault<tbl_content_organization_mapping>();
            if (organizationMapping != null)
            {
                organizationMapping.status = "D";
                this.db.SaveChanges();
            }
            content.STATUS = "D";
            this.db.SaveChanges();
        }

        [RoleAccessController(KEY = 5)]
        public ActionResult AssociatedContent(int? aid)
        {
            if (!aid.HasValue)
                return (ActionResult)this.RedirectToAction("display_content", "dashboard");
            tbl_content tbl_content = this.db.tbl_content.Find(new object[1]
            {
        (object) aid
            });
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == tbl_content.ID_CONTENT)).FirstOrDefault<tbl_content_answer>();
            if (tblContentAnswer == null)
                return (ActionResult)this.RedirectToAction("display_content", "dashboard");
            return (ActionResult)this.View((object)new associated_answer_content()
            {
                Answer = tblContentAnswer,
                Type = new contentDashboardModel().get_tbl_content_type()
            });
        }

        public ActionResult add_answer_content(FormCollection collection)
        {
            string str1 = this.Request.Form["btn_submit"];
            int int32_1 = Convert.ToInt32(this.Request.Form["ID_CONTENT_ANSWER"]);
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[1]
            {
        (object) int32_1
            });
            int int32_2 = Convert.ToInt32(this.Request.Form["select_type"]);
            string str2 = this.Request.Form["description"].ToString();
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            if (int32_2 == 3)
            {
                string str3 = this.Request.Form["WebLink"];
                this.db.tbl_content_type_link.Add(new tbl_content_type_link()
                {
                    ID_CONTENT_ANSWER = int32_1,
                    ID_CONTENT_TYPE = 3,
                    LINK_VALUE = str3,
                    DESCRIPTION = str2,
                    STATUS = "A",
                    UPDATED_DATE_TIME = DateTime.Now
                });
                this.db.SaveChanges();
            }
            else
            {
                string path2 = this.Request.Form["FileName"];
                try
                {
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        string str4 = ConfigurationManager.AppSettings["SERVER_PATH"].ToString();
                        HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["uploadBtn"];
                        if (file != null)
                        {
                            if (!Directory.Exists(this.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/" + (object)int32_1 + "/")))
                                Directory.CreateDirectory(this.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/" + (object)int32_1 + "/"));
                            string filename = Path.Combine(this.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/" + (object)int32_1 + "/"), path2);
                            file.SaveAs(filename);
                            string str5 = str4 + "Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/" + (object)int32_1 + "/" + path2;
                            this.db.tbl_content_type_link.Add(new tbl_content_type_link()
                            {
                                ID_CONTENT_ANSWER = int32_1,
                                ID_CONTENT_TYPE = int32_2,
                                LINK_VALUE = str5,
                                STATUS = "A",
                                DESCRIPTION = str2,
                                UPDATED_DATE_TIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    new contentDashboardModel().exception_log(ex);
                }
            }
            return (ActionResult)this.RedirectToAction("AssociatedContent", "dashboard", (object)new
            {
                aid = int32_1
            });
        }

        public ActionResult displayOrganisation()
        {
            this.ViewData["select-organization"] = (object)this.db.tbl_organization.ToList<tbl_organization>();
            return (ActionResult)this.View();
        }

        public ActionResult ProgramAssignment()
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string orgid = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "ProgramAssignment";


                UserLogDetails userLogDetails = new UserLogDetails();
                userLogDetails.AddUserDataLog(id_user, orgid, Page1);
            }
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " and category_type=0 and status='A'").ToList<tbl_category>();
            return (ActionResult)this.View();
        }

        public string getProgramContent(string id, string pattern)
        {
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern.Replace("'", "''");
            string sql = "select * from tbl_content where id_content not in (select id_content from tbl_content_role_mapping) and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32_1 + ") and content_owner=" + (object)int32_2 + "  and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<int> intList = new List<int>();
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery(sql).ToList<tbl_content>();
            foreach (tbl_content tblContent in list1)
            {
                tbl_content tbl_view = tblContent;
                List<tbl_subscriptions> list2 = this.db.tbl_subscriptions.Where<tbl_subscriptions>((Expression<Func<tbl_subscriptions, bool>>)(t => t.ID_CONTENT == tbl_view.ID_CONTENT)).ToList<tbl_subscriptions>();
                intList.Add(list2.Count);
            }
            string str = "";
            foreach (tbl_content tblContent in list1)
            {
                str = str + "<tr><td>" + tblContent.CONTENT_QUESTION + " (" + (object)tblContent.ID_CONTENT + ") </td>";
                str = str + "<td><a class=\"btn btn-primary btn-sm\" target=\"_blank\" href=" + this.Url.Action("ProgramUserList", "contentDashboard", (object)new
                {
                    id = tblContent.ID_CONTENT
                }) + ">continue</a></td>";
                str += "</tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th>Content</th>" + "<th></th>" + "</thead>" + "<tbody>" + str + "</tbody></table>";
        }

        public ActionResult ProgramUserList(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            this.ViewData["program"] = (object)this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>)(t => t.ID_CATEGORY == ids)).FirstOrDefault<tbl_category>();
            this.ViewData["category-list"] = (object)this.db.tbl_category_tiles.SqlQuery("select * from tbl_category_tiles where id_organization=" + (object)orgid + " and category_theme in (1,2)").ToList<tbl_category_tiles>();
            tbl_content_organization_mapping organizationMapping = new tbl_content_organization_mapping();
            this.ViewData["content"] = (object)this.db.tbl_content.SqlQuery("select * from tbl_content where id_content  in (select id_content from tbl_content_organization_mapping where id_category=" + (object)ids + " and id_organization=" + (object)orgid + ")").ToList<tbl_content>();
            this.ViewData["RoleList"] = (object)this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object)orgid + " and status='A'").ToList<tbl_csst_role>();
            this.ViewData["notification"] = (object)this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_organization == orgid && t.status == "A" && t.notification_type == 3)).ToList<tbl_notification>();
            this.ViewData["location"] = (object)new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object)orgid + " order by LOCATION ");
            this.ViewData["function"] = (object)new contentDashboardModel().getUserFunction(" select distinct case when user_function is null then UPPER(user_function) else UPPER(user_function) end as user_function " + " from tbl_user where ID_ORGANIZATION=" + (object)orgid + " and user_function is not null order by user_function ");
            return (ActionResult)this.View();
        }

        public class UserIdDataUplodeModel
        {
            public string userId { get; set; }
            public string idprogram { get; set; }
            public string idrole { get; set; }
            public string ctid { get; set; }
            public string chid { get; set; }
            public string std { get; set; }
            public string exd { get; set; }
            public string ncid { get; set; }
        }


        [HttpPost]
        public ActionResult uplodeUserID(List<UserIdDataUplodeModel> userIdStatusArray, bool isLastChunk)
        {
            //Example
            List<string> nonExistentUserIds = Session["NonExistentUserIds"] as List<string> ?? new List<string>();
            List<string> allreadyUserIds = Session["AllreadyUserIds"] as List<string> ?? new List<string>();
            bool hasUpdates = false;

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession == null)
            {
                return Json(new { success = false, message = "User session is not available." });
            }

            string orgId = userSession.id_ORGANIZATION;
            string userIdSession = userSession.ID_USER;
            string page = "PROGRAM ASSIGNMENT Bulk UploadUserID";
            string idProgram = Request.Form["id_program"];
            string idOperation = "Save";

            // Log the user action
            UserLogDetails userLogDetails = new UserLogDetails();
            userLogDetails.AddUserDataLogopration(userIdSession, orgId, page, idProgram, null, idOperation);

            foreach (var userStatus in userIdStatusArray)
            {
                string userId = userStatus.userId;
                int orgIdInt = Convert.ToInt32(userSession.id_ORGANIZATION);

                // Check if user exists
                var tblUser = this.db.tbl_user.FirstOrDefault(t => t.USERID == userId);
                if (tblUser != null)
                {
                    int userRoleId;
                    if (!int.TryParse(userStatus.idrole, out userRoleId))
                    {
                        return Json(new { success = false, message = "Invalid role ID for user " + userId });
                    }

                    int userContentId = tblUser.ID_USER;
                   
                    var roleMapping = this.db.tbl_role_user_mapping
                        .FirstOrDefault(t => t.id_csst_role == userRoleId && t.id_user == userContentId);
                    if (roleMapping != null)
                    {
                        int idProgram_1 = Convert.ToInt16(userStatus.idprogram);
                        var program_mapping = this.db.tbl_content_program_mapping
                            .FirstOrDefault(c => c.id_category == idProgram_1 && c.id_user == userContentId);
                       // var program_mapping = this.db.tbl_content_program_mapping.FirstOrDefault(c => c.id_category == Convert.ToInt16(userStatus.idprogram) && c.id_user == userContentId);

                        if (program_mapping == null)
                        {
                            DateTime datetime1 = new Utility().StringToDatetime(userStatus.std);
                            DateTime datetime2 = new Utility().StringToDatetime(userStatus.exd);


                            // Create a new mapping entry
                            tbl_content_program_mapping entity = new tbl_content_program_mapping
                            {
                                map_type = 1,
                                id_role = userRoleId,
                                id_user = userContentId,
                                id_organization = orgIdInt,
                                id_category = Convert.ToInt16(userStatus.idprogram),
                                status = "A",
                                option_type = 0,
                                start_date = new DateTime?(datetime1),
                                expiry_date = new DateTime?(datetime2),
                                id_category_tile = Convert.ToInt16(userStatus.ctid),
                                id_category_heading = Convert.ToInt16(userStatus.chid),
                                id_assessment_sheet = 0,
                                updated_date_time = DateTime.Now
                            };


                            this.db.tbl_content_program_mapping.Add(entity);
                            hasUpdates = true;
                            //activeUserIds.Add(userId);
                        }
                        else
                        {

                            allreadyUserIds.Add(userId);
                        }
                    }
                    else
                    {
                        nonExistentUserIds.Add(userId);
                    }
                }
                else
                {
                    nonExistentUserIds.Add(userId);
                }
            }

            Session["NonExistentUserIds"] = nonExistentUserIds;
            Session["AllreadyUserIds"] = allreadyUserIds;

            if (hasUpdates)
            {
                this.db.SaveChanges();
                //return Json(new { success = true, message = "Status changed successfully.", nonExistentUserIds, allreadyUserIds });
            }
            //else
            //{
            //    return Json(new { success = false, message = "No users were updated.", nonExistentUserIds, allreadyUserIds });
            //}
            if (isLastChunk)
            {

                return Json(new { success = true, message = "Status changed successfully" });
            }

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult GetNonExistentUserIdsProgram()
        {
            var nonExistentUserIds = Session["NonExistentUserIds"] as List<string> ?? new List<string>();
            var AddExistentUserIds = Session["AllreadyUserIds"] as List<string> ?? new List<string>();
            Session.Remove("NonExistentUserIds");
            Session.Remove("AllreadyUserIds");
            return Json(new { success = true, nonExistentUserIds, AddExistentUserIds }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult uplodeUserID(List<UserIdDataUplodeModel> userIdStatusArray)
        //{
        //    List<string> nonExistentUserIds = new List<string>();
        //    List<string> activeUserIds = new List<string>();
        //    bool hasUpdates = false;

        //   //var parsedFormData = HttpUtility.ParseQueryString(form_data);

        //   // string fieldName = parsedFormData["id_program"];

        //    UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
        //    if (userSession != null)
        //    {
        //        string orgid1 = userSession.id_ORGANIZATION;
        //        string id_user = userSession.ID_USER;
        //        string Page1 = "PROGRAM ASSIGMENT Bulk uplodeUserID";
        //        string Id_assessment = this.Request.Form["id_program"];
        //        string Id_category = null;
        //        string Id_operation = "Save";
        //        UserLogDetails userLogDetails = new UserLogDetails();

        //        userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
        //        // Now you have orgid and id_user available for further processing
        //    }

        //    int id_config = 0;
        //    string multiUser = "1";


        //    //string ids =this.Request.Form["id_program"];
        //    //int idrole = Convert.ToInt32(this.Request.Form["role-Id"]);
        //    //int categoryType = Convert.ToInt32(this.Request.Form["CATEGORY_TYPE"]);

        //    //int int32_1 = Convert.ToInt32(this.Request.Form["category_tile"]);
        //    //int int32_2 = Convert.ToInt32(this.Request.Form["category_heading"]);

        //    //UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    //int orgid = Convert.ToInt32(content.id_ORGANIZATION);

        //    //int int32_3 = Convert.ToInt32(content.ID_USER);
        //    //int int32_4 = Convert.ToInt32(this.Request.Form["notification_req"]);




        //    int nids = 0;
        //    tbl_notification note = null;
        //    if (ncid == 1)
        //    {
        //        if (!int.TryParse(this.Request.Form["notification_id"], out nids))
        //        {
        //            throw new FormatException("notification_id is not in a correct format.");
        //        }

        //        note = this.db.tbl_notification.FirstOrDefault(t => t.id_notification == nids && t.status == "A");
        //    }



        //    foreach (var userStatus in userIdStatusArray)
        //    {
        //        // Extract user data from the model
        //        string userId = userStatus.userId;

        //        UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //        int orgid = Convert.ToInt32(content.id_ORGANIZATION);


        //        // Check if user exists in tbl_user
        //        var tbluser = this.db.tbl_user.FirstOrDefault(t => t.USERID == userId);

        //        if (tbluser != null)
        //        {

        //            int ID_user = tbluser.ID_USER;

        //            var tbl_role_user_mapping = this.db.tbl_role_user_mapping.FirstOrDefault(t => t.id_csst_role == idrole && t.id_user == ID_user);

        //            if (tbl_role_user_mapping.id_csst_role > 0 )
        //            {
        //                tbl_content_program_mapping entity = new tbl_content_program_mapping();

        //                entity.map_type = 1;
        //                entity.id_role = idrole;
        //                entity.id_user = ID_user;
        //                entity.id_organization = orgid;
        //                entity.id_category = idprogram;
        //                entity.status = "A";
        //                entity.option_type = new int?(0);
        //                entity.start_date = std;
        //                entity.expiry_date = exd;
        //                entity.id_category_tile = ctid;
        //                entity.id_category_heading = chid;
        //                entity.id_assessment_sheet = new int?(0);
        //                entity.updated_date_time = new DateTime?(DateTime.Now);
        //                this.db.tbl_content_program_mapping.Add(entity);
        //                this.db.SaveChanges();
        //                hasUpdates = true;


        //                activeUserIds.Add(userId);
        //            }
        //            /// insert the data////

        //        }
        //        else
        //        {

        //            nonExistentUserIds.Add(userId);
        //        }
        //    }

        //    // Save changes only if updates were made
        //    if (hasUpdates)
        //    {
        //        this.db.SaveChanges();
        //        return Json(new { success = true, message = "Status changed successfully", nonExistentUserIds });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, message = "No users were updated because none were found.", nonExistentUserIds });
        //    }
        //}


        //



        public string getSubHeader(string id)
        {
            string subHeader = "";
            List<tbl_category_heading> list = this.db.tbl_category_heading.SqlQuery("SELECT distinct * FROM tbl_category_heading where id_category_tiles =" + (object)Convert.ToInt32(id) + " and status='A'  order by heading_order ").ToList<tbl_category_heading>();
            if (list.Count > 0)
            {
                foreach (tbl_category_heading tblCategoryHeading in list)
                    subHeader = subHeader + " <option value=\"" + (object)tblCategoryHeading.id_category_heading + "\" >" + tblCategoryHeading.Heading_title + "</option> ";
            }
            else
                subHeader = "0";
            return subHeader;
        }

        public string setProgramToUser(
          string cid,
          string value,
          string type,
          string cdt,
          string edt,
          string ctid,
          string chid,
          string nid)
        {
            string user = "0";
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            int opts = Convert.ToInt32(value);
            int int32_1 = Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            int int32_2 = Convert.ToInt32(ctid);
            int int32_3 = Convert.ToInt32(chid);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_4 = Convert.ToInt32(content.ID_USER);
            tbl_content_program_mapping contentProgramMapping1 = new tbl_content_program_mapping();
            tbl_notification tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            if (tblNotification != null)
            {
                if (int32_1 == 1)
                {
                    if (this.db.tbl_assignment_role_program.Where<tbl_assignment_role_program>((Expression<Func<tbl_assignment_role_program, bool>>)(t => t.id_organization == (int?)orgid && t.id_role == (int?)opts && t.id_program == (int?)ids)).FirstOrDefault<tbl_assignment_role_program>() == null)
                    {
                        this.db.tbl_assignment_role_program.Add(new tbl_assignment_role_program()
                        {
                            id_organization = new int?(orgid),
                            id_program = new int?(ids),
                            id_role = new int?(opts),
                            start_datetime = new DateTime?(datetime1),
                            end_datetime = new DateTime?(datetime2),
                            id_category_tile = new int?(int32_2),
                            id_category_heading = new int?(int32_3),
                            status = "A",
                            updated_date_time = new DateTime?(DateTime.Now)
                        });
                        this.db.SaveChanges();
                    }
                    object[] objArray = new object[5]
                    {
            (object) "select * from tbl_user where id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=",
            (object) opts,
            (object) ")  and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=",
            (object) orgid,
            (object) ")) "
                    };
                    foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery(string.Concat(objArray)).ToList<tbl_user>())
                    {
                        tbl_user item = tblUser;
                        tbl_content_program_mapping contentProgramMapping2 = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
                        if (contentProgramMapping2 == null)
                        {
                            tbl_content_program_mapping entity = new tbl_content_program_mapping();
                            entity.map_type = new int?(1);
                            entity.id_role = new int?(opts);
                            entity.id_user = new int?(item.ID_USER);
                            entity.id_organization = new int?(orgid);
                            entity.id_category = new int?(ids);
                            entity.status = "A";
                            entity.option_type = new int?(0);
                            entity.start_date = new DateTime?(datetime1);
                            entity.expiry_date = new DateTime?(datetime2);
                            entity.id_category_tile = new int?(int32_2);
                            entity.id_category_heading = new int?(int32_3);
                            entity.id_assessment_sheet = new int?(0);
                            entity.updated_date_time = new DateTime?(DateTime.Now);
                            this.db.tbl_content_program_mapping.Add(entity);
                            this.db.SaveChanges();
                            if (entity.id_content_program_mapping > 0)
                            {
                                this.db.tbl_notification_config.Add(new tbl_notification_config()
                                {
                                    id_assessment = new int?(0),
                                    id_category = new int?(ids),
                                    id_content = new int?(0),
                                    id_creater = new int?(int32_4),
                                    id_notification = new int?(nids),
                                    id_user = new int?(item.ID_USER),
                                    start_date = new DateTime?(datetime1),
                                    end_date = new DateTime?(datetime2),
                                    notification_key = tblNotification.notification_key,
                                    notification_action_type = "PRO",
                                    read_timestamp = new DateTime?(DateTime.Now),
                                    updated_date_time = new DateTime?(DateTime.Now),
                                    user_type = "1",
                                    status = "P"
                                });
                                this.db.SaveChanges();
                            }
                        }
                        else
                        {
                            contentProgramMapping2.start_date = new DateTime?(datetime1);
                            contentProgramMapping2.expiry_date = new DateTime?(datetime2);
                            contentProgramMapping2.updated_date_time = new DateTime?(DateTime.Now);
                            this.db.SaveChanges();
                        }
                    }
                    user = "1";
                }
                else if (this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)opts)).FirstOrDefault<tbl_content_program_mapping>() == null)
                {
                    tbl_content_program_mapping entity = new tbl_content_program_mapping();
                    entity.map_type = new int?(1);
                    entity.id_role = new int?(0);
                    entity.id_assessment_sheet = new int?(0);
                    entity.id_user = new int?(opts);
                    entity.id_organization = new int?(orgid);
                    entity.id_category = new int?(ids);
                    entity.status = "A";
                    entity.start_date = new DateTime?(datetime1);
                    entity.expiry_date = new DateTime?(datetime2);
                    entity.id_category_tile = new int?(int32_2);
                    entity.id_category_heading = new int?(int32_3);
                    entity.option_type = new int?(0);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_content_program_mapping.Add(entity);
                    this.db.SaveChanges();
                    user = "1";
                    if (entity.id_content_program_mapping > 0 && tblNotification != null)
                    {
                        this.db.tbl_notification_config.Add(new tbl_notification_config()
                        {
                            id_assessment = new int?(0),
                            id_category = new int?(ids),
                            id_content = new int?(0),
                            id_notification = new int?(nids),
                            id_user = new int?(opts),
                            id_creater = new int?(int32_4),
                            start_date = new DateTime?(datetime1),
                            end_date = new DateTime?(datetime2),
                            notification_key = tblNotification.notification_key,
                            notification_action_type = "PRO",
                            read_timestamp = new DateTime?(DateTime.Now),
                            updated_date_time = new DateTime?(DateTime.Now),
                            user_type = "1",
                            status = "P"
                        });
                        this.db.SaveChanges();
                    }
                }
            }
            return user;
        }


        /// <summary>
        //AAAAA
        //public string setProgramToAllUser(
        //  string cid,
        //  string value,
        //  string type,
        //  string cdt,
        //  string edt,
        //  string ctid,
        //  string chid,
        //  string nid)
        //{
        //    string allUser = "0";
        //    int ids = Convert.ToInt32(cid);
        //    int nids = Convert.ToInt32(nid);
        //    Convert.ToInt32(value);
        //    Convert.ToInt32(type);
        //    DateTime datetime1 = new Utility().StringToDatetime(cdt);
        //    DateTime datetime2 = new Utility().StringToDatetime(edt);
        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int orgid = Convert.ToInt32(content.id_ORGANIZATION);
        //    int int32_1 = Convert.ToInt32(ctid);
        //    int int32_2 = Convert.ToInt32(chid);
        //    int int32_3 = Convert.ToInt32(content.ID_USER);
        //    List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();
        //    tbl_notification tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();



        //    const int batchSize = 1000;  // You can adjust the batch size as needed
        //    int totalUsers = list.Count;

        //    List<tbl_content_program_mapping> entities1 = new List<tbl_content_program_mapping>();

        //    for (int i = 0; i < totalUsers; i += batchSize)
        //    {
        //        var batch = list.Skip(i).Take(batchSize).ToList();


        //        foreach (tbl_user tblUser in batch)
        //        {

        //            tbl_user item = tblUser;

        //            //tbl_content_program_mapping contentProgramMapping = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
        //            tbl_content_program_mapping contentProgramMapping = null;
        //            if (contentProgramMapping == null)
        //            {


        //                //entities1.Add(new tbl_content_program_mapping
        //                //{
        //                //    map_type = new int?(1),
        //                //    id_role = new int?(0),
        //                //    id_user = new int?(item.ID_USER),
        //                //    id_organization = new int?(orgid),
        //                //    id_category = new int?(ids),
        //                //    status = "A",
        //                //    option_type = new int?(0),
        //                //    start_date = new DateTime?(datetime1),
        //                //    expiry_date = new DateTime?(datetime2),
        //                //    id_category_tile = new int?(int32_1),
        //                //    id_category_heading = new int?(int32_2),
        //                //    id_assessment_sheet = new int?(0),
        //                //    updated_date_time = new DateTime?(DateTime.Now)
        //                //});
        //                //if (entities1.Any())
        //                //{
        //                //    this.db.BulkInsert(entities1);
        //                //    entities1.Clear(); // Clear the list for the next batch
        //                //}


        //                // Assuming item, orgid, ids, datetime1, datetime2, int32_1, and int32_2 are defined somewhere

        //                entities1.Add(new tbl_content_program_mapping
        //                {
        //                    map_type = new int?(1),
        //                    id_role = new int?(0),
        //                    id_user = new int?(item.ID_USER),
        //                    id_organization = new int?(orgid),
        //                    id_category = new int?(ids),
        //                    status = "A",
        //                    option_type = new int?(0),
        //                    start_date = new DateTime?(datetime1),
        //                    expiry_date = new DateTime?(datetime2),
        //                    id_category_tile = new int?(int32_1),
        //                    id_category_heading = new int?(int32_2),
        //                    id_assessment_sheet = new int?(0),
        //                    updated_date_time = new DateTime?(DateTime.Now)
        //                });

        //                foreach (var entity4 in entities1)
        //                {
        //                    this.db.tbl_content_program_mapping.Add(entity4);
        //                }

        //                this.db.SaveChanges();
        //                entities1.Clear();


        //                tbl_content_program_mapping entity = new tbl_content_program_mapping();


        //                if (entity.id_content_program_mapping > 0 && tblNotification != null)
        //                {
        //                    this.db.tbl_notification_config.Add(new tbl_notification_config()
        //                    {
        //                        id_assessment = new int?(0),
        //                        id_category = new int?(ids),
        //                        id_content = new int?(0),
        //                        id_creater = new int?(int32_3),
        //                        id_notification = new int?(nids),
        //                        id_user = new int?(item.ID_USER),
        //                        start_date = new DateTime?(datetime1),
        //                        end_date = new DateTime?(datetime2),
        //                        notification_key = tblNotification.notification_key,
        //                        notification_action_type = "PRO",
        //                        read_timestamp = new DateTime?(DateTime.Now),
        //                        updated_date_time = new DateTime?(DateTime.Now),
        //                        user_type = "1",
        //                        status = "P"
        //                    });
        //                    this.db.SaveChanges();
        //                }
        //            }
        //            else
        //            {
        //                contentProgramMapping.start_date = new DateTime?(datetime1);
        //                contentProgramMapping.expiry_date = new DateTime?(datetime2);
        //                contentProgramMapping.updated_date_time = new DateTime?(DateTime.Now);
        //                this.db.SaveChanges();
        //            }
        //        }
        //    }
        //    return allUser;

        //}

        public string setProgramToAllUser(
             string cid,
             string value,
             string type,
             string cdt,
             string edt,
             string ctid,
             string chid,
             string nid)
        {
            string allUser = "0";
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            Convert.ToInt32(value);
            Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_1 = Convert.ToInt32(ctid);
            int int32_2 = Convert.ToInt32(chid);
            int int32_3 = Convert.ToInt32(content.ID_USER);
           // List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where STATUS = 'A' && id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();
            tbl_notification tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();



            const int batchSize = 1000;  // You can adjust the batch size as needed
            int totalUsers = list.Count;

            List<tbl_content_program_mapping> entities1 = new List<tbl_content_program_mapping>();

            for (int i = 0; i < totalUsers; i += batchSize)
            {
                var batch = list.Skip(i).Take(batchSize).ToList();


                foreach (tbl_user tblUser in batch)
                {

                    tbl_user item = tblUser;

                    //tbl_content_program_mapping contentProgramMapping = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
                    tbl_content_program_mapping contentProgramMapping = null;
                    if (contentProgramMapping == null)
                    {


                        entities1.Add(new tbl_content_program_mapping
                        {
                            map_type = new int?(1),
                            id_role = new int?(0),
                            id_user = new int?(item.ID_USER),
                            id_organization = new int?(orgid),
                            id_category = new int?(ids),
                            status = "A",
                            option_type = new int?(0),
                            start_date = new DateTime?(datetime1),
                            expiry_date = new DateTime?(datetime2),
                            id_category_tile = new int?(int32_1),
                            id_category_heading = new int?(int32_2),
                            id_assessment_sheet = new int?(0),
                            updated_date_time = new DateTime?(DateTime.Now)
                        });
                        if (entities1.Any())
                        {
                            this.db.BulkInsert(entities1);
                            entities1.Clear(); // Clear the list for the next batch
                        }


                        tbl_content_program_mapping entity = new tbl_content_program_mapping();


                        if (entity.id_content_program_mapping > 0 && tblNotification != null)
                        {
                            this.db.tbl_notification_config.Add(new tbl_notification_config()
                            {
                                id_assessment = new int?(0),
                                id_category = new int?(ids),
                                id_content = new int?(0),
                                id_creater = new int?(int32_3),
                                id_notification = new int?(nids),
                                id_user = new int?(item.ID_USER),
                                start_date = new DateTime?(datetime1),
                                end_date = new DateTime?(datetime2),
                                notification_key = tblNotification.notification_key,
                                notification_action_type = "PRO",
                                read_timestamp = new DateTime?(DateTime.Now),
                                updated_date_time = new DateTime?(DateTime.Now),
                                user_type = "1",
                                status = "P"
                            });
                            this.db.SaveChanges();
                        }
                    }
                    else
                    {
                        contentProgramMapping.start_date = new DateTime?(datetime1);
                        contentProgramMapping.expiry_date = new DateTime?(datetime2);
                        contentProgramMapping.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.SaveChanges();
                    }
                }
            }
            return allUser;

        }




        public string removeProgramToUser(string cid, string value, string type)
        {
            string user = "0";
            int ids = Convert.ToInt32(cid);
            int opts = Convert.ToInt32(value);
            int int32 = Convert.ToInt32(type);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (int32 == 1)
            {
                DbSet<tbl_content_program_mapping> contentProgramMapping = this.db.tbl_content_program_mapping;
                Expression<Func<tbl_content_program_mapping, bool>> predicate = (Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_role == (int?)opts);
                foreach (tbl_content_program_mapping entity in contentProgramMapping.Where<tbl_content_program_mapping>(predicate).ToList<tbl_content_program_mapping>())
                {
                    this.db.tbl_content_program_mapping.Remove(entity);
                    this.db.SaveChanges();
                    user = "1";
                }
            }
            else
            {
                tbl_content_program_mapping entity = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)opts)).FirstOrDefault<tbl_content_program_mapping>();
                if (entity != null)
                {
                    this.db.tbl_content_program_mapping.Remove(entity);
                    this.db.SaveChanges();
                    user = "1";
                }
            }
            return user;
        }


        [HttpPost]
        public string removeProgramToUserall(string cid, List<string> values, string type)
        {
            string user = "0";
            int ids = Convert.ToInt32(cid);
            int int32 = Convert.ToInt32(type);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

            foreach (var value in values)
            {
                int opts = Convert.ToInt32(value);

                if (int32 == 1)
                {
                    DbSet<tbl_content_program_mapping> contentProgramMapping = this.db.tbl_content_program_mapping;
                    Expression<Func<tbl_content_program_mapping, bool>> predicate = (Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_role == (int?)opts);
                    foreach (tbl_content_program_mapping entity in contentProgramMapping.Where<tbl_content_program_mapping>(predicate).ToList<tbl_content_program_mapping>())
                    {
                        this.db.tbl_content_program_mapping.Remove(entity);
                        this.db.SaveChanges();
                        user = "1";
                    }
                }
                else
                {
                    tbl_content_program_mapping entity = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)opts)).FirstOrDefault<tbl_content_program_mapping>();
                    if (entity != null)
                    {
                        this.db.tbl_content_program_mapping.Remove(entity);
                        this.db.SaveChanges();
                        user = "1";
                    }
                }
            }

            return user;
        }


        public string setProgramToAllUserString(
          string cid,
          string value,
          string type,
          string cdt,
          string edt,
          string ctid,
          string chid,
          string nid)
        {
            string allUserString = "1";
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            int int32_1 = Convert.ToInt32(ctid);
            int int32_2 = Convert.ToInt32(chid);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            value = value.Replace("'", "''");
            string sql = "select * from tbl_user where  id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + value + "%' or upper(LASTNAME) like '%" + value + "%')  OR upper(USERID) like '%" + value + "%'  OR upper(EMPLOYEEID) like '%" + value + "%'   and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) ";
            List<int> intList = new List<int>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            int int32_3 = Convert.ToInt32(content.ID_USER);
            tbl_notification tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_content_program_mapping contentProgramMapping = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
                if (contentProgramMapping == null)
                {
                    tbl_content_program_mapping entity = new tbl_content_program_mapping();
                    entity.map_type = new int?(1);
                    entity.id_role = new int?(0);
                    entity.id_user = new int?(item.ID_USER);
                    entity.id_organization = new int?(orgid);
                    entity.id_category = new int?(ids);
                    entity.status = "A";
                    entity.option_type = new int?(0);
                    entity.start_date = new DateTime?(datetime1);
                    entity.expiry_date = new DateTime?(datetime2);
                    entity.id_category_tile = new int?(int32_1);
                    entity.id_category_heading = new int?(int32_2);
                    entity.id_assessment_sheet = new int?(0);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_content_program_mapping.Add(entity);
                    this.db.SaveChanges();
                    if (entity.id_content_program_mapping > 0 && tblNotification != null)
                    {
                        this.db.tbl_notification_config.Add(new tbl_notification_config()
                        {
                            id_assessment = new int?(0),
                            id_category = new int?(ids),
                            id_content = new int?(0),
                            id_notification = new int?(nids),
                            id_user = new int?(item.ID_USER),
                            id_creater = new int?(int32_3),
                            start_date = new DateTime?(datetime1),
                            end_date = new DateTime?(datetime2),
                            notification_key = tblNotification.notification_key,
                            notification_action_type = "PRO",
                            read_timestamp = new DateTime?(DateTime.Now),
                            updated_date_time = new DateTime?(DateTime.Now),
                            user_type = "1",
                            status = "P"
                        });
                        this.db.SaveChanges();
                    }
                }
                else
                {
                    contentProgramMapping.start_date = new DateTime?(datetime1);
                    contentProgramMapping.expiry_date = new DateTime?(datetime2);
                    contentProgramMapping.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            return allUserString;
        }

        public ActionResult UserBasedContentAssignment()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = " Content Assignment";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            return (ActionResult)this.View();
        }

        public string getUserBasedContent(string id, string pattern)
        {
            if (id == "")
                id = "0";
            int int32_1 = Convert.ToInt32(id);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_2 = Convert.ToInt32(content.id_ORGANIZATION);
            bool flag = new RoleBasedAccess().checkAccess(content.action, 6);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string str1 = "select * from tbl_content where status in ('A') and content_owner=" + (object)int32_2 + "  ";
            if (int32_1 == 0)
                //Rani Changes date 16-6-2023
                // str1 += " and id_content not in (select id_content from tbl_content_organization_mapping )";
                str1 += "";
            //end changes
            if (!string.IsNullOrEmpty(pattern))
                str1 = str1 + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery(str1 + " order by CONTENT_QUESTION").Take<tbl_content>(100).ToList<tbl_content>();
            List<int> intList = new List<int>();
            foreach (tbl_content tblContent in list1)
            {
                tbl_content tbl_view = tblContent;
                List<tbl_subscriptions> list2 = this.db.tbl_subscriptions.Where<tbl_subscriptions>((Expression<Func<tbl_subscriptions, bool>>)(t => t.ID_CONTENT == tbl_view.ID_CONTENT)).ToList<tbl_subscriptions>();
                intList.Add(list2.Count);
            }
            string str2 = "";
            foreach (tbl_content tblContent in list1)
            {
                tbl_content item = tblContent;
                str2 = str2 + "<tr><td>" + item.CONTENT_QUESTION + "(" + (object)item.ID_CONTENT + ")</td>";
                tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_cms_users>();
                str2 += "<td>";
                str2 = tblCmsUsers == null ? str2 ?? "" : str2 + (tblCmsUsers.USERNAME ?? "");
                str2 += "</td>";
                str2 = str2 + "<td>" + item.UPDATED_DATE_TIME.ToShortDateString() + "</td>";
                if (flag)
                {
                    str2 += "<td> ";
                    str2 = str2 + "<a class=\"btn btn-primary btn-sm\" target=\"_blank\" href=" + this.Url.Action("ContenToUserList", "contentDashboard", (object)new
                    {
                        id = item.ID_CONTENT
                    }) + ">continue</a>";
                    str2 += "</td> ";
                }
                str2 += "</tr>";
            }
            string str3 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"70%\">Content</th><th width=\"10%\">Creator </th><th width=\"10%\" align=\"center\">Created Date</th>";
            if (flag)
                str3 += "<th width=\"10%\">Action</th>";
            return str3 + "</tr></thead>" + "<tbody>" + str2 + "</tbody></table>";
        }

        public ActionResult ContenToUserList(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            this.ViewData["program"] = (object)this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == ids)).FirstOrDefault<tbl_content>();
            this.ViewData["category-list"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            tbl_content_organization_mapping organizationMapping = new tbl_content_organization_mapping();
            this.ViewData["RoleList"] = (object)this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object)orgid + " and status='A'").ToList<tbl_csst_role>();
            this.ViewData["location"] = (object)new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object)orgid + " order by LOCATION ");
            this.ViewData["function"] = (object)new contentDashboardModel().getUserFunction(" select distinct case when user_function is null then UPPER(user_function) else UPPER(user_function) end as user_function " + " from tbl_user where ID_ORGANIZATION=" + (object)orgid + " and user_function is not null order by user_function ");
            this.ViewData["notification"] = (object)this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_organization == orgid && t.status == "A" && t.notification_type == 3)).ToList<tbl_notification>();
            return (ActionResult)this.View();
        }

        public string setUserbasedContentToUser(
          string cid,
          string value,
          string type,
          string cdt,
          string edt,
          string ctid,
          string nid)
        {
            int id_config = 0;
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            int opts = Convert.ToInt32(value);
            int int32_1 = Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(ctid);
            int int32_3 = Convert.ToInt32(content.ID_USER);
            tbl_content_user_assisgnment contentUserAssisgnment1 = new tbl_content_user_assisgnment();
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            if (int32_1 == 1)
            {
                object[] objArray = new object[5]
                {
          (object) "select * from tbl_user where id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=",
          (object) opts,
          (object) ")   and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=",
          (object) orgid,
          (object) ")) "
                };
                foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery(string.Concat(objArray)).ToList<tbl_user>())
                {
                    tbl_user item = tblUser;
                    tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(item.ID_USER);
                    tbl_content_user_assisgnment contentUserAssisgnment2 = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_user_assisgnment>();
                    if (contentUserAssisgnment2 == null)
                    {
                        tbl_content_user_assisgnment contentUserAssisgnment3 = new tbl_content_user_assisgnment();
                        contentUserAssisgnment3.id_user = new int?(item.ID_USER);
                        contentUserAssisgnment3.id_organization = new int?(orgid);
                        contentUserAssisgnment3.id_content = new int?(ids);
                        contentUserAssisgnment3.status = "A";
                        contentUserAssisgnment3.start_date = new DateTime?(datetime1);
                        contentUserAssisgnment3.expiry_date = new DateTime?(datetime2);
                        contentUserAssisgnment3.id_category = new int?(int32_2);
                        contentUserAssisgnment3.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment3);
                        this.db.SaveChanges();
                        if (contentUserAssisgnment3.id_content_user_assisgnment > 0)
                        {
                            if (note != null)
                            {
                                tbl_notification_config entity = new tbl_notification_config();
                                entity.id_assessment = new int?(0);
                                entity.id_category = new int?(0);
                                entity.id_creater = new int?(int32_3);
                                entity.id_content = new int?(ids);
                                entity.id_notification = new int?(nids);
                                entity.id_user = new int?(item.ID_USER);
                                entity.start_date = new DateTime?(datetime1);
                                entity.end_date = new DateTime?(datetime2);
                                entity.notification_key = note.notification_key;
                                entity.notification_action_type = "CON";
                                entity.read_timestamp = new DateTime?(DateTime.Now);
                                entity.updated_date_time = new DateTime?(DateTime.Now);
                                entity.user_type = "1";
                                entity.status = "P";
                                this.db.tbl_notification_config.Add(entity);
                                this.db.SaveChanges();
                                id_config = entity.id_notification_config;
                            }
                            if (mailId.EMAIL != "" && note != null)
                                new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment3, orgid);
                        }
                    }
                    else
                    {
                        contentUserAssisgnment2.start_date = new DateTime?(datetime1);
                        contentUserAssisgnment2.expiry_date = new DateTime?(datetime2);
                        contentUserAssisgnment2.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.SaveChanges();
                    }
                }
            }
            else
            {
                tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(opts);
                tbl_content_user_assisgnment contentUserAssisgnment4 = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)opts)).FirstOrDefault<tbl_content_user_assisgnment>();
                if (contentUserAssisgnment4 == null)
                {
                    tbl_content_user_assisgnment contentUserAssisgnment5 = new tbl_content_user_assisgnment();
                    contentUserAssisgnment5.id_user = new int?(opts);
                    contentUserAssisgnment5.id_organization = new int?(orgid);
                    contentUserAssisgnment5.id_content = new int?(ids);
                    contentUserAssisgnment5.status = "A";
                    contentUserAssisgnment5.start_date = new DateTime?(datetime1);
                    contentUserAssisgnment5.expiry_date = new DateTime?(datetime2);
                    contentUserAssisgnment5.id_category = new int?(int32_2);
                    contentUserAssisgnment5.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment5);
                    this.db.SaveChanges();
                    if (contentUserAssisgnment5.id_content_user_assisgnment > 0 && note != null)
                    {
                        this.db.tbl_notification_config.Add(new tbl_notification_config()
                        {
                            id_assessment = new int?(0),
                            id_category = new int?(0),
                            id_content = new int?(ids),
                            id_notification = new int?(nids),
                            id_creater = new int?(int32_3),
                            id_user = new int?(opts),
                            start_date = new DateTime?(datetime1),
                            end_date = new DateTime?(datetime2),
                            notification_key = note.notification_key,
                            notification_action_type = "CON",
                            read_timestamp = new DateTime?(DateTime.Now),
                            updated_date_time = new DateTime?(DateTime.Now),
                            user_type = "1",
                            status = "P"
                        });
                        this.db.SaveChanges();
                    }
                    new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment5, orgid);
                }
                else
                {
                    contentUserAssisgnment4.start_date = new DateTime?(datetime1);
                    contentUserAssisgnment4.expiry_date = new DateTime?(datetime2);
                    contentUserAssisgnment4.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            return "1";
        }

        public string removeUserbasedContentToUser(string cid, string value, string type)
        {
            string user = "0";
            int ids = Convert.ToInt32(cid);
            int opts = Convert.ToInt32(value);
            int int32 = Convert.ToInt32(type);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (int32 == 1)
            {
                DbSet<tbl_content_user_assisgnment> contentUserAssisgnment = this.db.tbl_content_user_assisgnment;
                Expression<Func<tbl_content_user_assisgnment, bool>> predicate = (Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid);
                foreach (tbl_content_user_assisgnment entity in contentUserAssisgnment.Where<tbl_content_user_assisgnment>(predicate).ToList<tbl_content_user_assisgnment>())
                {
                    this.db.tbl_content_user_assisgnment.Remove(entity);
                    this.db.SaveChanges();
                    user = "1";
                }
            }
            else
            {
                tbl_content_user_assisgnment entity = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)opts)).FirstOrDefault<tbl_content_user_assisgnment>();
                if (entity != null)
                {
                    this.db.tbl_content_user_assisgnment.Remove(entity);
                    this.db.SaveChanges();
                    user = "1";
                }
            }
            return user;
        }

        public string setUserbasedContentToAllUserString(
          string cid,
          string value,
          string type,
          string cdt,
          string edt,
          string ctid,
          string chid,
          string nid)
        {
            string allUserString = "1";
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            int int32_1 = Convert.ToInt32(ctid);
            Convert.ToInt32(chid);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            value = value.Replace("'", "''");
            string sql = "select * from tbl_user where  id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + value + "%' or upper(LASTNAME) like '%" + value + "%')  OR upper(USERID) like '%" + value + "%'   AND id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) ";
            List<int> intList = new List<int>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            tbl_notification tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_content_user_assisgnment contentUserAssisgnment = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_user_assisgnment>();
                if (contentUserAssisgnment == null)
                {
                    tbl_content_user_assisgnment entity = new tbl_content_user_assisgnment();
                    entity.id_user = new int?(item.ID_USER);
                    entity.id_organization = new int?(orgid);
                    entity.id_content = new int?(ids);
                    entity.id_category = new int?(int32_1);
                    entity.status = "A";
                    entity.start_date = new DateTime?(datetime1);
                    entity.expiry_date = new DateTime?(datetime2);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_content_user_assisgnment.Add(entity);
                    this.db.SaveChanges();
                    if (entity.id_content_user_assisgnment > 0 && tblNotification != null)
                    {
                        this.db.tbl_notification_config.Add(new tbl_notification_config()
                        {
                            id_assessment = new int?(0),
                            id_category = new int?(0),
                            id_content = new int?(ids),
                            id_notification = new int?(nids),
                            id_user = new int?(item.ID_USER),
                            start_date = new DateTime?(datetime1),
                            id_creater = new int?(int32_2),
                            end_date = new DateTime?(datetime2),
                            notification_key = tblNotification.notification_key,
                            notification_action_type = "CON",
                            read_timestamp = new DateTime?(DateTime.Now),
                            updated_date_time = new DateTime?(DateTime.Now),
                            user_type = "1",
                            status = "P"
                        });
                        this.db.SaveChanges();
                    }
                }
                else
                {
                    contentUserAssisgnment.start_date = new DateTime?(datetime1);
                    contentUserAssisgnment.expiry_date = new DateTime?(datetime2);
                    contentUserAssisgnment.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            this.sendContentNotification(cid);
            return allUserString;
        }

        //public string setUserbasedContentToAllUser(
        //  string cid,
        //  string value,
        //  string type,
        //  string cdt,
        //  string edt,
        //  string ctid,
        //  string chid,
        //  string nid)
        //{
        //    int ids = Convert.ToInt32(cid);
        //    int nids = Convert.ToInt32(nid);
        //    Convert.ToInt32(value);
        //    Convert.ToInt32(type);
        //    DateTime datetime1 = new Utility().StringToDatetime(cdt);
        //    DateTime datetime2 = new Utility().StringToDatetime(edt);
        //    int int32_1 = Convert.ToInt32(ctid);
        //    Convert.ToInt32(chid);
        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int orgid = Convert.ToInt32(content.id_ORGANIZATION);
        //    int int32_2 = Convert.ToInt32(content.ID_USER);
        //    List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();
        //    //List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user  where id_organization=" + (object)orgid +" and STATUS = 'A')").ToList<tbl_user>();
        //    tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
        //    foreach (tbl_user tblUser in list)
        //    {
        //        tbl_user item = tblUser;

        //        int id_config = 0;
        //        tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(item.ID_USER);

        //        tbl_content_user_assisgnment contentUserAssisgnment1 = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_user_assisgnment>();

        //        if (contentUserAssisgnment1 == null)
        //        {
        //            tbl_content_user_assisgnment contentUserAssisgnment2 = new tbl_content_user_assisgnment();
        //            contentUserAssisgnment2.id_user = new int?(item.ID_USER);
        //            contentUserAssisgnment2.id_organization = new int?(orgid);
        //            contentUserAssisgnment2.id_content = new int?(ids);
        //            contentUserAssisgnment2.id_category = new int?(int32_1);
        //            contentUserAssisgnment2.status = "A";
        //            contentUserAssisgnment2.start_date = new DateTime?(datetime1);
        //            contentUserAssisgnment2.expiry_date = new DateTime?(datetime2);
        //            contentUserAssisgnment2.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment2);
        //            this.db.SaveChanges();
        //            if (contentUserAssisgnment2.id_content_user_assisgnment > 0 && note != null)
        //            {
        //                tbl_notification_config entity = new tbl_notification_config();
        //                entity.id_assessment = new int?(0);
        //                entity.id_category = new int?(0);
        //                entity.id_content = new int?(ids);
        //                entity.id_notification = new int?(nids);
        //                entity.start_date = new DateTime?(datetime1);
        //                entity.id_creater = new int?(int32_2);
        //                entity.end_date = new DateTime?(datetime2);
        //                entity.id_user = new int?(item.ID_USER);
        //                entity.notification_key = note.notification_key;
        //                entity.notification_action_type = "CON";
        //                entity.read_timestamp = new DateTime?(DateTime.Now);
        //                entity.updated_date_time = new DateTime?(DateTime.Now);
        //                entity.user_type = "1";
        //                entity.status = "P";
        //                this.db.tbl_notification_config.Add(entity);
        //                this.db.SaveChanges();
        //                id_config = entity.id_notification_config;
        //            }
        //            if (mailId.EMAIL != "" && note != null)
        //                new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment2, orgid);
        //        }
        //        else
        //        {
        //            contentUserAssisgnment1.start_date = new DateTime?(datetime1);
        //            contentUserAssisgnment1.expiry_date = new DateTime?(datetime2);
        //            contentUserAssisgnment1.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.SaveChanges();
        //        }
        //    }
        //    this.sendContentNotification(cid);
        //    return "1";
        //}

        /// <summary>
        //public string setUserbasedContentToAllUser(
        // string cid,
        // string value,
        // string type,
        // string cdt,
        // string edt,
        // string ctid,
        // string chid,
        // string nid)
        //{
        //    int ids = Convert.ToInt32(cid);
        //    int nids = Convert.ToInt32(nid);
        //    Convert.ToInt32(value);
        //    Convert.ToInt32(type);
        //    DateTime datetime1 = new Utility().StringToDatetime(cdt);
        //    DateTime datetime2 = new Utility().StringToDatetime(edt);
        //    int int32_1 = Convert.ToInt32(ctid);
        //    Convert.ToInt32(chid);
        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int orgid = Convert.ToInt32(content.id_ORGANIZATION);
        //    int int32_2 = Convert.ToInt32(content.ID_USER);
        //    List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();


        //    //List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user  where id_organization=" + (object)orgid +" and STATUS = 'A')").ToList<tbl_user>();
        //    tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();


        //    const int batchSize = 1000;  // You can adjust the batch size as needed
        //    int totalUsers = list.Count;

        //    List<tbl_content_user_assisgnment> entities1 = new List<tbl_content_user_assisgnment>();

        //    for (int i = 0; i < totalUsers; i += batchSize)
        //    {
        //        var batch = list.Skip(i).Take(batchSize).ToList();


        //        foreach (tbl_user tblUser in batch)
        //        {
        //            tbl_user item = tblUser;

        //            int id_config = 0;
        //            tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(item.ID_USER);
        //            //tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment
        //            //    .Where(t => t.id_assessment == ids && t.id_organization == orgid && t.id_user == tblUser.ID_USER)
        //            //    .FirstOrDefault();
        //            //Direct all entry 

        //            tbl_content_user_assisgnment contentUserAssisgnment1 = null;

        //            if (contentUserAssisgnment1 == null)
        //            {
        //                entities1.Add(new tbl_content_user_assisgnment
        //                {
        //                    id_user = item.ID_USER,
        //                    id_organization = orgid,
        //                    id_content = ids,
        //                    id_category = int32_1,
        //                    status = "A",
        //                    start_date = datetime1,
        //                    expiry_date = datetime2,
        //                    updated_date_time = DateTime.Now
        //                });
        //                if (entities1.Any())
        //                {
        //                    this.db.tbl_content_user_assisgnment.AddRange(entities1);
        //                    this.db.SaveChanges();
        //                    entities1.Clear();
        //                }
        //                /////



        //                tbl_content_user_assisgnment contentUserAssisgnment2 = new tbl_content_user_assisgnment();
        //                if (contentUserAssisgnment2.id_content_user_assisgnment > 0 && note != null)
        //                {
        //                    tbl_notification_config entity = new tbl_notification_config();
        //                    entity.id_assessment = new int?(0);
        //                    entity.id_category = new int?(0);
        //                    entity.id_content = new int?(ids);
        //                    entity.id_notification = new int?(nids);
        //                    entity.start_date = new DateTime?(datetime1);
        //                    entity.id_creater = new int?(int32_2);
        //                    entity.end_date = new DateTime?(datetime2);
        //                    entity.id_user = new int?(item.ID_USER);
        //                    entity.notification_key = note.notification_key;
        //                    entity.notification_action_type = "CON";
        //                    entity.read_timestamp = new DateTime?(DateTime.Now);
        //                    entity.updated_date_time = new DateTime?(DateTime.Now);
        //                    entity.user_type = "1";
        //                    entity.status = "P";
        //                    this.db.tbl_notification_config.Add(entity);
        //                    this.db.SaveChanges();
        //                    id_config = entity.id_notification_config;
        //                }
        //                if (mailId.EMAIL != "" && note != null)
        //                    new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment2, orgid);

        //            }


        //            else
        //            {
        //                contentUserAssisgnment1.start_date = datetime1;
        //                contentUserAssisgnment1.expiry_date = datetime2;
        //                contentUserAssisgnment1.updated_date_time = DateTime.Now;
        //                this.db.SaveChanges();
        //            }
        //        }

        //    }
        //    this.sendContentNotification(cid);
        //    return "1";
        //}



        public string setUserbasedContentToAllUser(
               string cid,
               string value,
               string type,
               string cdt,
               string edt,
               string ctid,
               string chid,
               string nid)
        {
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            Convert.ToInt32(value);
            Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            int int32_1 = Convert.ToInt32(ctid);
            Convert.ToInt32(chid);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();


            //List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user  where id_organization=" + (object)orgid +" and STATUS = 'A')").ToList<tbl_user>();
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();


            const int batchSize = 1000;  // You can adjust the batch size as needed
            int totalUsers = list.Count;

            List<tbl_content_user_assisgnment> entities1 = new List<tbl_content_user_assisgnment>();

            for (int i = 0; i < totalUsers; i += batchSize)
            {
                var batch = list.Skip(i).Take(batchSize).ToList();


                foreach (tbl_user tblUser in batch)
                {
                    tbl_user item = tblUser;

                    int id_config = 0;
                    tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(item.ID_USER);

                    //tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment
                    //    .Where(t => t.id_assessment == ids && t.id_organization == orgid && t.id_user == tblUser.ID_USER)
                    //    .FirstOrDefault();
                    //Direct all entry 

                    tbl_content_user_assisgnment contentUserAssisgnment1 = null;

                    if (contentUserAssisgnment1 == null)
                    {
                        entities1.Add(new tbl_content_user_assisgnment
                        {
                            id_user = item.ID_USER,
                            id_organization = orgid,
                            id_content = ids,
                            id_category = int32_1,
                            status = "A",
                            start_date = datetime1,
                            expiry_date = datetime2,
                            updated_date_time = DateTime.Now
                        });
                        if (entities1.Any())
                        {

                            this.db.BulkInsert(entities1);
                            entities1.Clear();
                        }
                        /////



                        tbl_content_user_assisgnment contentUserAssisgnment2 = new tbl_content_user_assisgnment();
                        if (contentUserAssisgnment2.id_content_user_assisgnment > 0 && note != null)
                        {
                            tbl_notification_config entity = new tbl_notification_config();
                            entity.id_assessment = new int?(0);
                            entity.id_category = new int?(0);
                            entity.id_content = new int?(ids);
                            entity.id_notification = new int?(nids);
                            entity.start_date = new DateTime?(datetime1);
                            entity.id_creater = new int?(int32_2);
                            entity.end_date = new DateTime?(datetime2);
                            entity.id_user = new int?(item.ID_USER);
                            entity.notification_key = note.notification_key;
                            entity.notification_action_type = "CON";
                            entity.read_timestamp = new DateTime?(DateTime.Now);
                            entity.updated_date_time = new DateTime?(DateTime.Now);
                            entity.user_type = "1";
                            entity.status = "P";
                            this.db.tbl_notification_config.Add(entity);
                            this.db.SaveChanges();
                            id_config = entity.id_notification_config;
                        }
                        if (mailId.EMAIL != "" && note != null)
                            new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment2, orgid);

                    }


                    else
                    {
                        contentUserAssisgnment1.start_date = datetime1;
                        contentUserAssisgnment1.expiry_date = datetime2;
                        contentUserAssisgnment1.updated_date_time = DateTime.Now;
                        this.db.SaveChanges();
                    }
                }

            }
            this.sendContentNotification(cid);
            return "1";
        }

        /// </summary>
        /// <returns></returns>



        public ActionResult UserBasedAssessmentAssignment()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Assessment Assignment";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            this.ViewData["AssessmentList"] = (object)this.db.tbl_assessment.SqlQuery("select * from tbl_assessment where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " AND status='A' order by assessment_title").ToList<tbl_assessment>();
            return (ActionResult)this.View();
        }

        public ActionResult UserBasedAssessmentToUserList(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            this.ViewData["program"] = (object)this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == ids && t.status == "A")).FirstOrDefault<tbl_assessment>();
            this.ViewData["category-list"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            tbl_content_organization_mapping organizationMapping = new tbl_content_organization_mapping();
            this.ViewData["RoleList"] = (object)this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object)orgid + " and status='A'").ToList<tbl_csst_role>();
            this.ViewData["notification"] = (object)this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_organization == orgid && t.status == "A" && t.notification_type == 3)).ToList<tbl_notification>();
            this.ViewData["location"] = (object)new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object)orgid + " order by LOCATION ");
            this.ViewData["function"] = (object)new contentDashboardModel().getUserFunction(" select distinct case when user_function is null then UPPER(user_function) else UPPER(user_function) end as user_function " + " from tbl_user where ID_ORGANIZATION=" + (object)orgid + " and user_function is not null order by user_function ");
            return (ActionResult)this.View();
        }

        public string setUserBasedAssessmentToUser(
          string cid,
          string value,
          string type,
          string cdt,
          string edt,
          string ctid,
          string nid)
        {
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            int int32_1 = Convert.ToInt32(value);
            int int32_2 = Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            int int32_3 = Convert.ToInt32(ctid);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            tbl_assessment_user_assignment assessmentUserAssignment1 = new tbl_assessment_user_assignment();
            string str = int32_2 != 1 ? value : "select id_user from tbl_role_user_mapping  where id_csst_role=" + (object)int32_1 ?? "";
            int int32_4 = Convert.ToInt32(content.ID_USER);
            List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (" + str + ")   and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) ").ToList<tbl_user>();
            tbl_notification tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_assessment_user_assignment assessmentUserAssignment2 = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_assessment_user_assignment>();
                if (assessmentUserAssignment2 == null)
                {
                    tbl_assessment_user_assignment entity = new tbl_assessment_user_assignment();
                    entity.id_user = new int?(item.ID_USER);
                    entity.id_organization = new int?(orgid);
                    entity.id_assessment = new int?(ids);
                    entity.status = "A";
                    entity.start_date = new DateTime?(datetime1);
                    entity.expire_date = new DateTime?(datetime2);
                    entity.id_category = new int?(int32_3);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_user_assignment.Add(entity);
                    this.db.SaveChanges();
                    if (entity.id_assessment_user_assignment > 0 && tblNotification != null)
                    {
                        this.db.tbl_notification_config.Add(new tbl_notification_config()
                        {
                            id_assessment = new int?(ids),
                            id_category = new int?(0),
                            id_content = new int?(0),
                            id_notification = new int?(nids),
                            id_user = new int?(item.ID_USER),
                            id_creater = new int?(int32_4),
                            start_date = new DateTime?(datetime1),
                            end_date = new DateTime?(datetime2),
                            notification_key = tblNotification.notification_key,
                            notification_action_type = "ASS",
                            read_timestamp = new DateTime?(DateTime.Now),
                            updated_date_time = new DateTime?(DateTime.Now),
                            user_type = "1",
                            status = "P"
                        });
                        this.db.SaveChanges();
                    }
                }
                else
                {
                    assessmentUserAssignment2.start_date = new DateTime?(datetime1);
                    assessmentUserAssignment2.expire_date = new DateTime?(datetime2);
                    assessmentUserAssignment2.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            this.sendAssessmentNotification(cid);
            return "1";
        }

        public string removeUserBasedAssessmentToUser(string cid, string value, string type)
        {
            string user = "0";
            int ids = Convert.ToInt32(cid);
            int opts = Convert.ToInt32(value);
            int int32 = Convert.ToInt32(type);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (int32 == 1)
            {
                DbSet<tbl_assessment_user_assignment> assessmentUserAssignment = this.db.tbl_assessment_user_assignment;
                Expression<Func<tbl_assessment_user_assignment, bool>> predicate = (Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid);
                foreach (tbl_assessment_user_assignment entity in assessmentUserAssignment.Where<tbl_assessment_user_assignment>(predicate).ToList<tbl_assessment_user_assignment>())
                {
                    this.db.tbl_assessment_user_assignment.Remove(entity);
                    this.db.SaveChanges();
                    user = "1";
                }
            }
            else
            {
                tbl_assessment_user_assignment entity = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)opts)).FirstOrDefault<tbl_assessment_user_assignment>();
                if (entity != null)
                {
                    this.db.tbl_assessment_user_assignment.Remove(entity);
                    this.db.SaveChanges();
                    user = "1";
                }
            }
            return user;
        }


        [HttpPost]
        public string removeProgramToUserall_1(string cid, List<string> values, string type)
        {

            string user = "0";
            int ids = Convert.ToInt32(cid);
          
            int int32 = Convert.ToInt32(type);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

            foreach (var value in values)
            {
                int opts = Convert.ToInt32(value);
                if (int32 == 1)
                {
                    DbSet<tbl_assessment_user_assignment> assessmentUserAssignment = this.db.tbl_assessment_user_assignment;
                    Expression<Func<tbl_assessment_user_assignment, bool>> predicate = (Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid);
                    foreach (tbl_assessment_user_assignment entity in assessmentUserAssignment.Where<tbl_assessment_user_assignment>(predicate).ToList<tbl_assessment_user_assignment>())
                    {
                        this.db.tbl_assessment_user_assignment.Remove(entity);
                        this.db.SaveChanges();
                        user = "1";
                    }
                }
                else
                {
                    tbl_assessment_user_assignment entity = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)opts)).FirstOrDefault<tbl_assessment_user_assignment>();
                    if (entity != null)
                    {
                        this.db.tbl_assessment_user_assignment.Remove(entity);
                        this.db.SaveChanges();
                        user = "1";
                    }
                }
            }
            return user;
        }





        public string setUserBasedAssessmentToAllUserString(
          string cid,
          string value,
          string type,
          string cdt,
          string edt,
          string ctid,
          string chid,
          string nid)
        {
            string allUserString = "1";
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            int int32_1 = Convert.ToInt32(ctid);
            Convert.ToInt32(chid);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            value = value.Replace("'", "''");
            int int32_2 = Convert.ToInt32(content.ID_USER);
            string sql = "select * from tbl_user where  id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + value + "%' or upper(LASTNAME) like '%" + value + "%')  OR upper(USERID) like '%" + value + "%' OR upper(EMPLOYEEID) like '%" + value + "%'  and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) ";
            List<int> intList = new List<int>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            tbl_notification tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_assessment_user_assignment>();
                if (assessmentUserAssignment == null)
                {
                    tbl_assessment_user_assignment entity = new tbl_assessment_user_assignment();
                    entity.id_user = new int?(item.ID_USER);
                    entity.id_organization = new int?(orgid);
                    entity.id_assessment = new int?(ids);
                    entity.id_category = new int?(int32_1);
                    entity.status = "A";
                    entity.start_date = new DateTime?(datetime1);
                    entity.expire_date = new DateTime?(datetime2);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_user_assignment.Add(entity);
                    this.db.SaveChanges();
                    if (entity.id_assessment_user_assignment > 0 && tblNotification != null)
                    {
                        this.db.tbl_notification_config.Add(new tbl_notification_config()
                        {
                            id_assessment = new int?(ids),
                            id_category = new int?(0),
                            id_content = new int?(0),
                            id_notification = new int?(nids),
                            start_date = new DateTime?(datetime1),
                            end_date = new DateTime?(datetime2),
                            id_user = new int?(item.ID_USER),
                            id_creater = new int?(int32_2),
                            notification_key = tblNotification.notification_key,
                            notification_action_type = "ASS",
                            read_timestamp = new DateTime?(DateTime.Now),
                            updated_date_time = new DateTime?(DateTime.Now),
                            user_type = "1",
                            status = "P"
                        });
                        this.db.SaveChanges();
                    }
                }
                else
                {
                    assessmentUserAssignment.start_date = new DateTime?(datetime1);
                    assessmentUserAssignment.expire_date = new DateTime?(datetime2);
                    assessmentUserAssignment.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            this.sendAssessmentNotification(cid);
            return allUserString;
        }

        //Added for the Assessmennt for all user
        public string setUserBasedAssessmentToAllUser(
        string cid,
        string value,
        string type,
        string cdt,
        string edt,
        string ctid,
        string chid,
        string nid)
        {
            int ids = Convert.ToInt32(cid);
            int nids = Convert.ToInt32(nid);
            Convert.ToInt32(value);
            Convert.ToInt32(type);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_1 = Convert.ToInt32(ctid);
            Convert.ToInt32(chid);
            int int32_2 = Convert.ToInt32(content.ID_USER);

            List<tbl_user> list = this.db.tbl_user.SqlQuery("SELECT * FROM tbl_user WHERE STATUS='A' AND id_user IN (SELECT DISTINCT id_user FROM tbl_role_user_mapping WHERE id_csst_role IN (SELECT id_csst_role FROM tbl_csst_role WHERE id_organization=" + (object)orgid + "))").ToList<tbl_user>();

            const int batchSize = 1000;  // You can adjust the batch size as needed
            int totalUsers = list.Count;

            List<tbl_assessment_user_assignment> entities1 = new List<tbl_assessment_user_assignment>();

            for (int i = 0; i < totalUsers; i += batchSize)
            {
                var batch = list.Skip(i).Take(batchSize).ToList();

                foreach (tbl_user tblUser in batch)
                {
                    //tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment
                    //    .Where(t => t.id_assessment == ids && t.id_organization == orgid && t.id_user == tblUser.ID_USER)
                    //    .FirstOrDefault();
                    //Direct all entry 

                    tbl_assessment_user_assignment assessmentUserAssignment = null;

                    if (assessmentUserAssignment == null)
                    {
                        entities1.Add(new tbl_assessment_user_assignment
                        {
                            id_user = tblUser.ID_USER,
                            id_organization = orgid,
                            id_assessment = ids,
                            id_category = int32_1,
                            status = "A",
                            start_date = datetime1,
                            expire_date = datetime2,
                            updated_date_time = DateTime.Now
                        });
                        if (entities1.Any())
                        {
                            this.db.BulkInsert(entities1);
                            entities1.Clear(); // Clear the list for the next batch
                        }

                        //this.db.tbl_assessment_user_assignment.AddRange(entities1);

                        tbl_assessment_user_assignment entity = new tbl_assessment_user_assignment();

                        if (entity.id_assessment_user_assignment > 0)
                        {
                            tbl_notification tblNotification = this.db.tbl_notification
                                .Where(t => t.id_notification == nids && t.status == "A")
                                .FirstOrDefault();

                            if (tblNotification != null)
                            {
                                this.db.tbl_notification_config.Add(new tbl_notification_config()
                                {
                                    id_assessment = ids,
                                    id_category = 0,
                                    id_content = 0,
                                    id_creater = int32_2,
                                    id_notification = nids,
                                    id_user = tblUser.ID_USER,
                                    start_date = datetime1,
                                    end_date = datetime2,
                                    notification_key = tblNotification.notification_key,
                                    notification_action_type = "ASS",
                                    read_timestamp = DateTime.Now,
                                    updated_date_time = DateTime.Now,
                                    user_type = "1",
                                    status = "P"
                                });
                                this.db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        assessmentUserAssignment.start_date = datetime1;
                        assessmentUserAssignment.expire_date = datetime2;
                        assessmentUserAssignment.updated_date_time = DateTime.Now;
                        this.db.SaveChanges();
                    }
                }
            }

            this.sendAssessmentNotification(cid);
            return "1";
        }



        public string sendContentNotification(string cid)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.id_ORGANIZATION);
            int cids = Convert.ToInt32(cid);
            int uid = Convert.ToInt32(content.ID_USER);
            tbl_content tblContent = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == cids && t.STATUS == "A")).FirstOrDefault<tbl_content>();
            if (tblContent != null)
            {
                DbSet<tbl_notification_config> notificationConfig1 = this.db.tbl_notification_config;
                Expression<Func<tbl_notification_config, bool>> predicate = (Expression<Func<tbl_notification_config, bool>>)(t => t.id_content == (int?)cids && t.id_creater == (int?)uid && t.status == "P");
                foreach (tbl_notification_config notificationConfig2 in notificationConfig1.Where<tbl_notification_config>(predicate).ToList<tbl_notification_config>())
                {
                    tbl_notification_config item = notificationConfig2;
                    tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => (int?)t.id_notification == item.id_notification && t.status == "A")).FirstOrDefault<tbl_notification>();
                    if (note != null)
                    {
                        string msg = "";
                        int userid = Convert.ToInt32((object)item.id_user);
                        msg = note.notification_name + " - " + tblContent.CONTENT_QUESTION;
                        IQueryable<tbl_user_gcm_log> source = this.db.tbl_user_gcm_log.Where<tbl_user_gcm_log>((Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)userid));
                        Expression<Func<tbl_user_gcm_log, int>> keySelector = (Expression<Func<tbl_user_gcm_log, int>>)(t => t.id_user_gcm_log);
                        foreach (tbl_user_gcm_log tblUserGcmLog in source.OrderByDescending<tbl_user_gcm_log, int>(keySelector).Take<tbl_user_gcm_log>(2).ToList<tbl_user_gcm_log>())
                        {
                            tbl_user_gcm_log gcm = tblUserGcmLog;
                            if (!string.IsNullOrEmpty(gcm.GCMID))
                            {
                                Notification notification;
                                new Thread((ThreadStart)(() => notification = new Notification(gcm.GCMID, msg, note.notification_name))).Start();
                            }
                        }
                        item.status = "A";
                        this.db.SaveChanges();
                    }
                }
            }
            return "1";
        }

        public string sendAssessmentNotification(string cid)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.id_ORGANIZATION);
            int cids = Convert.ToInt32(cid);
            int uid = Convert.ToInt32(content.ID_USER);
            tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == cids && t.status == "A")).FirstOrDefault<tbl_assessment>();
            if (tblAssessment != null)
            {
                DbSet<tbl_notification_config> notificationConfig1 = this.db.tbl_notification_config;
                Expression<Func<tbl_notification_config, bool>> predicate = (Expression<Func<tbl_notification_config, bool>>)(t => t.id_assessment == (int?)cids && t.id_creater == (int?)uid && t.status == "P");
                foreach (tbl_notification_config notificationConfig2 in notificationConfig1.Where<tbl_notification_config>(predicate).ToList<tbl_notification_config>())
                {
                    tbl_notification_config item = notificationConfig2;
                    tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => (int?)t.id_notification == item.id_notification && t.status == "A")).FirstOrDefault<tbl_notification>();
                    if (note != null)
                    {
                        string msg = "";
                        int userid = Convert.ToInt32((object)item.id_user);
                        msg = note.notification_name + " - " + tblAssessment.assessment_title;
                        IQueryable<tbl_user_gcm_log> source = this.db.tbl_user_gcm_log.Where<tbl_user_gcm_log>((Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)userid));
                        Expression<Func<tbl_user_gcm_log, int>> keySelector = (Expression<Func<tbl_user_gcm_log, int>>)(t => t.id_user_gcm_log);
                        foreach (tbl_user_gcm_log tblUserGcmLog in source.OrderByDescending<tbl_user_gcm_log, int>(keySelector).Take<tbl_user_gcm_log>(2).ToList<tbl_user_gcm_log>())
                        {
                            tbl_user_gcm_log gcm = tblUserGcmLog;
                            Notification notification;
                            new Thread((ThreadStart)(() => notification = new Notification(gcm.GCMID, msg, note.notification_name))).Start();
                        }
                        item.status = "A";
                        this.db.SaveChanges();
                    }
                }
            }
            return "1";
        }

        public string sendProgramNotification(string cid)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.id_ORGANIZATION);
            int cids = Convert.ToInt32(cid);
            int uid = Convert.ToInt32(content.ID_USER);
            tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>)(t => t.ID_CATEGORY == cids && t.CATEGORY_TYPE == (int?)0 && t.STATUS == "A")).FirstOrDefault<tbl_category>();
            if (tblCategory != null)
            {
                DbSet<tbl_notification_config> notificationConfig1 = this.db.tbl_notification_config;
                Expression<Func<tbl_notification_config, bool>> predicate = (Expression<Func<tbl_notification_config, bool>>)(t => t.id_category == (int?)cids && t.id_creater == (int?)uid && t.status == "P");
                foreach (tbl_notification_config notificationConfig2 in notificationConfig1.Where<tbl_notification_config>(predicate).ToList<tbl_notification_config>())
                {
                    tbl_notification_config item = notificationConfig2;
                    tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => (int?)t.id_notification == item.id_notification && t.status == "A")).FirstOrDefault<tbl_notification>();
                    if (note != null)
                    {
                        string msg = "";
                        int userid = Convert.ToInt32((object)item.id_user);
                        msg = note.notification_name + " - " + tblCategory.CATEGORYNAME;
                        IQueryable<tbl_user_gcm_log> source = this.db.tbl_user_gcm_log.Where<tbl_user_gcm_log>((Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)userid));
                        Expression<Func<tbl_user_gcm_log, int>> keySelector = (Expression<Func<tbl_user_gcm_log, int>>)(t => t.id_user_gcm_log);
                        foreach (tbl_user_gcm_log tblUserGcmLog in source.OrderByDescending<tbl_user_gcm_log, int>(keySelector).Take<tbl_user_gcm_log>(2).ToList<tbl_user_gcm_log>())
                        {
                            tbl_user_gcm_log gcm = tblUserGcmLog;
                            Notification notification;
                            new Thread((ThreadStart)(() => notification = new Notification(gcm.GCMID, msg, note.notification_name))).Start();
                        }
                        item.status = "A";
                        this.db.SaveChanges();
                    }
                }
            }
            return "1";
        }

        public string assignProgramToRole(
          string pid,
          string rid,
          string opt,
          string cdt,
          string edt,
          string nid)
        {
            int pids = Convert.ToInt32(pid);
            int nids = Convert.ToInt32(nid);
            Convert.ToInt32(rid);
            int opts = Convert.ToInt32(opt);
            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32 = Convert.ToInt32(content.ID_USER);
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            if (this.db.tbl_assignment_role_program.Where<tbl_assignment_role_program>((Expression<Func<tbl_assignment_role_program, bool>>)(t => t.id_organization == (int?)orgid && t.id_role == (int?)opts && t.id_program == (int?)pids)).FirstOrDefault<tbl_assignment_role_program>() == null)
            {
                tbl_assignment_role_program entity = new tbl_assignment_role_program();
                entity.id_organization = new int?(orgid);
                entity.id_program = new int?(pids);
                entity.id_role = new int?(opts);
                entity.status = "A";
                entity.updated_date_time = new DateTime?(DateTime.Now);
                this.db.tbl_assignment_role_program.Add(entity);
                this.db.SaveChanges();
                if (entity.id_assignment_role_program > 0)
                {
                    object[] objArray = new object[4]
                    {
            (object) "select * from tbl_user where id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=",
            (object) opts,
            (object) ")  and id_organization=",
            (object) orgid
                    };
                    foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery(string.Concat(objArray) ?? "").ToList<tbl_user>())
                    {
                        tbl_user item = tblUser;
                        if (this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)pids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>() == null)
                        {
                            this.db.tbl_content_program_mapping.Add(new tbl_content_program_mapping()
                            {
                                map_type = new int?(1),
                                id_role = new int?(opts),
                                id_user = new int?(item.ID_USER),
                                id_organization = new int?(orgid),
                                id_category = new int?(pids),
                                status = "A",
                                option_type = new int?(0),
                                start_date = new DateTime?(datetime1),
                                expiry_date = new DateTime?(datetime2),
                                id_category_tile = new int?(0),
                                id_category_heading = new int?(0),
                                id_assessment_sheet = new int?(0),
                                updated_date_time = new DateTime?(DateTime.Now)
                            });
                            this.db.SaveChanges();
                            if (note != null)
                            {
                                if (this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_category == (int?)pids && t.id_user == (int?)item.ID_USER && t.id_notification == (int?)note.id_notification)).FirstOrDefault<tbl_notification_config>() == null)
                                {
                                    this.db.tbl_notification_config.Add(new tbl_notification_config()
                                    {
                                        id_assessment = new int?(0),
                                        id_category = new int?(pids),
                                        id_content = new int?(0),
                                        id_creater = new int?(int32),
                                        id_notification = new int?(nids),
                                        id_user = new int?(item.ID_USER),
                                        start_date = new DateTime?(datetime1),
                                        end_date = new DateTime?(datetime2),
                                        notification_key = note.notification_key,
                                        notification_action_type = "PRO",
                                        read_timestamp = new DateTime?(DateTime.Now),
                                        updated_date_time = new DateTime?(DateTime.Now),
                                        user_type = "1",
                                        status = "P"
                                    });
                                    this.db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }

        public string removeProgramRoleAsignment(string pid, string rid)
        {
            int pids = Convert.ToInt32(pid);
            int rids = Convert.ToInt32(rid);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            Convert.ToInt32(content.ID_USER);
            tbl_assignment_role_program entity = this.db.tbl_assignment_role_program.Where<tbl_assignment_role_program>((Expression<Func<tbl_assignment_role_program, bool>>)(t => t.id_organization == (int?)orgid && t.id_role == (int?)rids && t.id_program == (int?)pids)).FirstOrDefault<tbl_assignment_role_program>();
            if (entity != null)
            {
                this.db.tbl_assignment_role_program.Remove(entity);
                this.db.SaveChanges();
            }
            return "1";
        }

        /// <summary>
        //BulkInsert
        public string setUserbasedContentToMultipleUser()
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "USER BASED CONTENT ASSIGMENT";
                string Id_assessment = null;
                string Id_category = this.Request.Form["category"].ToString();
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }

            int batchSize = 1000; // Number of records to process in each batch
            int id_config = 0;
            int[] source = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
            DateTime? nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : null;
            DateTime? nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : null;
            int ids = Convert.ToInt32(this.Request.Form["id_content"].ToString());
            int int32_1 = Convert.ToInt32(this.Request.Form["category"].ToString());
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            int int32_3 = Convert.ToInt32(this.Request.Form["notification_req"].ToString());
            int nids = 0;
            if (int32_3 == 1)
                nids = Convert.ToInt32(this.Request.Form["notification_id"].ToString());
            tbl_notification note = null;
            if (int32_3 == 1)
                note = this.db.tbl_notification.FirstOrDefault(t => t.id_notification == nids && t.status == "A");

            for (int i = 0; i < source.Length; i += batchSize)
            {
                var batch = source.Skip(i).Take(batchSize);

                foreach (int temp in batch)
                {
                    tbl_content_user_assisgnment contentUserAssisgnment1 = this.db.tbl_content_user_assisgnment.FirstOrDefault(t => t.id_content == ids && t.id_organization == orgid && t.id_user == temp);
                    tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(temp);
                    if (contentUserAssisgnment1 == null)
                    {
                        tbl_content_user_assisgnment contentUserAssisgnment2 = new tbl_content_user_assisgnment();
                        contentUserAssisgnment2.id_user = temp;
                        contentUserAssisgnment2.id_organization = orgid;
                        contentUserAssisgnment2.id_content = ids;
                        contentUserAssisgnment2.id_category = int32_1;
                        contentUserAssisgnment2.status = "A";
                        contentUserAssisgnment2.start_date = nullable1;
                        contentUserAssisgnment2.expiry_date = nullable2;
                        contentUserAssisgnment2.updated_date_time = DateTime.Now;
                        this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment2);
                        this.db.SaveChanges();
                        if (contentUserAssisgnment2.id_content_user_assisgnment > 0 && note != null)
                        {
                            tbl_notification_config entity = new tbl_notification_config();
                            entity.id_assessment = 0;
                            entity.id_category = 0;
                            entity.id_content = ids;
                            entity.id_notification = nids;
                            entity.start_date = nullable1;
                            entity.end_date = nullable2;
                            entity.id_user = temp;
                            entity.notification_key = note.notification_key;
                            entity.notification_action_type = "CON";
                            entity.read_timestamp = DateTime.Now;
                            entity.updated_date_time = DateTime.Now;
                            entity.user_type = "1";
                            entity.status = "P";
                            entity.id_creater = int32_2;
                            this.db.tbl_notification_config.Add(entity);
                            this.db.SaveChanges();
                            id_config = entity.id_notification_config;
                        }
                        if (!string.IsNullOrEmpty(mailId.EMAIL) && note != null)
                            new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment2, orgid);
                    }
                    else
                    {
                        contentUserAssisgnment1.start_date = nullable1;
                        contentUserAssisgnment1.expiry_date = nullable2;
                        contentUserAssisgnment1.updated_date_time = DateTime.Now;
                        this.db.SaveChanges();
                    }
                }
            }
            this.sendContentNotification(ids.ToString());
            return "1";
        }


        /// <returns></returns>
        //public string setUserbasedContentToMultipleUser()
        //{
        //    int id_config = 0;
        //    int[] source = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
        //    DateTime? nullable1 = new DateTime?();
        //    DateTime? nullable2 = new DateTime?();
        //    nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
        //    nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
        //    int ids = Convert.ToInt32(this.Request.Form["id_content"].ToString());
        //    int int32_1 = Convert.ToInt32(this.Request.Form["category"].ToString());
        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int orgid = Convert.ToInt32(content.id_ORGANIZATION);
        //    int int32_2 = Convert.ToInt32(content.ID_USER);
        //    int int32_3 = Convert.ToInt32(this.Request.Form["notification_req"].ToString());
        //    int nids = 0;
        //    if (int32_3 == 1)
        //        nids = Convert.ToInt32(this.Request.Form["notification_id"].ToString());
        //    tbl_notification note = (tbl_notification)null;
        //    if (int32_3 == 1)
        //        note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
        //    for (int index = 0; index < ((IEnumerable<int>)source).Count<int>(); ++index)
        //    {
        //        int temp = source[index];
        //        tbl_content_user_assisgnment contentUserAssisgnment1 = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)temp)).FirstOrDefault<tbl_content_user_assisgnment>();
        //        tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(temp);
        //        if (contentUserAssisgnment1 == null)
        //        {
        //            tbl_content_user_assisgnment contentUserAssisgnment2 = new tbl_content_user_assisgnment();
        //            contentUserAssisgnment2.id_user = new int?(source[index]);
        //            contentUserAssisgnment2.id_organization = new int?(orgid);
        //            contentUserAssisgnment2.id_content = new int?(ids);
        //            contentUserAssisgnment2.id_category = new int?(int32_1);
        //            contentUserAssisgnment2.status = "A";
        //            contentUserAssisgnment2.start_date = nullable1;
        //            contentUserAssisgnment2.expiry_date = nullable2;
        //            contentUserAssisgnment2.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment2);
        //            this.db.SaveChanges();
        //            if (contentUserAssisgnment2.id_content_user_assisgnment > 0 && note != null)
        //            {
        //                tbl_notification_config entity = new tbl_notification_config();
        //                entity.id_assessment = new int?(0);
        //                entity.id_category = new int?(0);
        //                entity.id_content = new int?(ids);
        //                entity.id_notification = new int?(nids);
        //                entity.start_date = nullable1;
        //                entity.end_date = nullable2;
        //                entity.id_user = new int?(source[index]);
        //                entity.notification_key = note.notification_key;
        //                entity.notification_action_type = "CON";
        //                entity.read_timestamp = new DateTime?(DateTime.Now);
        //                entity.updated_date_time = new DateTime?(DateTime.Now);
        //                entity.user_type = "1";
        //                entity.status = "P";
        //                entity.id_creater = new int?(int32_2);
        //                this.db.tbl_notification_config.Add(entity);
        //                this.db.SaveChanges();
        //                id_config = entity.id_notification_config;
        //            }
        //            if (mailId.EMAIL != "" && note != null)
        //                new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment2, orgid);
        //        }
        //        else
        //        {
        //            contentUserAssisgnment1.start_date = nullable1;
        //            contentUserAssisgnment1.expiry_date = nullable2;
        //            contentUserAssisgnment1.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.SaveChanges();
        //        }
        //    }
        //    this.sendContentNotification(ids.ToString());
        //    return "1";
        //}


        //public string setProgramToMultiUser()
        //{
        //    int id_config = 0;
        //    int[] source = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
        //    DateTime? nullable1 = new DateTime?();
        //    DateTime? nullable2 = new DateTime?();
        //    nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
        //    nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
        //    string multiUser = "1";
        //    int ids = Convert.ToInt32(this.Request.Form["id_program"].ToString());
        //    Convert.ToInt32(this.Request.Form["role-type"].ToString());
        //    Convert.ToInt32(this.Request.Form["CATEGORY_TYPE"].ToString());
        //    int int32_1 = Convert.ToInt32(this.Request.Form["category_tile"].ToString());
        //    int int32_2 = Convert.ToInt32(this.Request.Form["category_heading"].ToString());
        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int orgid = Convert.ToInt32(content.id_ORGANIZATION);
        //    int int32_3 = Convert.ToInt32(content.ID_USER);
        //    int int32_4 = Convert.ToInt32(this.Request.Form["notification_req"].ToString());
        //    int nids = 0;
        //    if (int32_4 == 1)
        //        nids = Convert.ToInt32(this.Request.Form["notification_id"].ToString());
        //    tbl_notification note = (tbl_notification)null;
        //    if (int32_4 == 1)
        //        note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
        //    for (int index = 0; index < ((IEnumerable<int>)source).Count<int>(); ++index)
        //    {
        //        int user_idn = source[index];
        //        tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(user_idn);
        //        tbl_content_program_mapping contentProgramMapping1 = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)user_idn)).FirstOrDefault<tbl_content_program_mapping>();
        //        if (contentProgramMapping1 == null)
        //        {
        //            tbl_content_program_mapping contentProgramMapping2 = new tbl_content_program_mapping();
        //            contentProgramMapping2.map_type = new int?(1);
        //            contentProgramMapping2.id_role = new int?(0);
        //            contentProgramMapping2.id_user = new int?(source[index]);
        //            contentProgramMapping2.id_organization = new int?(orgid);
        //            contentProgramMapping2.id_category = new int?(ids);
        //            contentProgramMapping2.status = "A";
        //            contentProgramMapping2.option_type = new int?(0);
        //            contentProgramMapping2.start_date = nullable1;
        //            contentProgramMapping2.expiry_date = nullable2;
        //            contentProgramMapping2.id_category_tile = new int?(int32_1);
        //            contentProgramMapping2.id_category_heading = new int?(int32_2);
        //            contentProgramMapping2.id_assessment_sheet = new int?(0);
        //            contentProgramMapping2.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.tbl_content_program_mapping.Add(contentProgramMapping2);
        //            this.db.SaveChanges();
        //            if (contentProgramMapping2.id_content_program_mapping > 0 && note != null)
        //            {
        //                tbl_notification_config entity = new tbl_notification_config();
        //                entity.id_assessment = new int?(0);
        //                entity.id_category = new int?(ids);
        //                entity.id_content = new int?(0);
        //                entity.id_notification = new int?(nids);
        //                entity.id_user = new int?(source[index]);
        //                entity.id_creater = new int?(int32_3);
        //                entity.start_date = nullable1;
        //                entity.end_date = nullable2;
        //                entity.notification_key = note.notification_key;
        //                entity.notification_action_type = "PRO";
        //                entity.read_timestamp = new DateTime?(DateTime.Now);
        //                entity.updated_date_time = new DateTime?(DateTime.Now);
        //                entity.user_type = "1";
        //                entity.status = "P";
        //                this.db.tbl_notification_config.Add(entity);
        //                this.db.SaveChanges();
        //                id_config = entity.id_notification_config;
        //            }
        //            if (mailId.EMAIL != "" && note != null)
        //                new addCMS_CategoryModel().sendmail_program(mailId, note, id_config, contentProgramMapping2, orgid);
        //        }
        //        else
        //        {
        //            contentProgramMapping1.start_date = nullable1;
        //            contentProgramMapping1.expiry_date = nullable2;
        //            contentProgramMapping1.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.SaveChanges();
        //        }
        //    }
        //    this.sendProgramNotification(ids.ToString());
        //    return multiUser;
        //}


        /// <summary>
        //Bulkinsert
        public string setProgramToMultiUser()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "PROGRAM ASSIGMENT";
                string Id_assessment = this.Request.Form["id_program"];
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }

            int id_config = 0;
            string multiUser = "1";

            // Safely parse and handle exceptions for id_program
            if (!int.TryParse(this.Request.Form["id_program"], out int ids))
            {
                throw new FormatException("id_program is not in a correct format.");
            }

            string idrole = this.Request.Form["role-Id"];

            // Safely parse and handle exceptions for category_type
            if (!int.TryParse(this.Request.Form["CATEGORY_TYPE"], out int categoryType))
            {
                throw new FormatException("CATEGORY_TYPE is not in a correct format.");
            }

            // Safely parse and handle exceptions for category_tile
            if (!int.TryParse(this.Request.Form["category_tile"], out int int32_1))
            {
                throw new FormatException("category_tile is not in a correct format.");
            }

            // Safely parse and handle exceptions for category_heading
            if (!int.TryParse(this.Request.Form["category_heading"], out int int32_2))
            {
                throw new FormatException("category_heading is not in a correct format.");
            }

            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            if (!int.TryParse(content.id_ORGANIZATION, out int orgid))
            {
                throw new FormatException("id_ORGANIZATION is not in a correct format.");
            }

            if (!int.TryParse(content.ID_USER, out int int32_3))
            {
                throw new FormatException("ID_USER is not in a correct format.");
            }

            if (!int.TryParse(this.Request.Form["notification_req"], out int int32_4))
            {
                throw new FormatException("notification_req is not in a correct format.");
            }

            int nids = 0;
            tbl_notification note = null;
            if (int32_4 == 1)
            {
                if (!int.TryParse(this.Request.Form["notification_id"], out nids))
                {
                    throw new FormatException("notification_id is not in a correct format.");
                }

                note = this.db.tbl_notification.FirstOrDefault(t => t.id_notification == nids && t.status == "A");
            }

            // Safely parse and handle exceptions for chk_user
            int[] source;
            try
            {
                source = Array.ConvertAll(this.Request.Form["chk_user"].Split(','), int.Parse);
            }
            catch (FormatException)
            {
                throw new FormatException("chk_user contains values that are not in a correct format.");
            }

            DateTime? nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"])
                ? DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
                : (DateTime?)null;

            DateTime? nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"])
                ? DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
                : (DateTime?)null;
            //int id_config = 0;
            //string multiUser = "1";
            //int ids = Convert.ToInt32(this.Request.Form["id_program"]);
            //string idrole =  this.Request.Form["role-Id"];
            ////Convert.ToInt32(this.Request.Form["role-type"]);
            //Convert.ToInt32(this.Request.Form["CATEGORY_TYPE"]);
            //int int32_1 = Convert.ToInt32(this.Request.Form["category_tile"]);
            //int int32_2 = Convert.ToInt32(this.Request.Form["category_heading"]);
            //UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            //int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            //int int32_3 = Convert.ToInt32(content.ID_USER);
            //int int32_4 = Convert.ToInt32(this.Request.Form["notification_req"]);
            //int nids = 0;
            //if (int32_4 == 1)
            //    nids = Convert.ToInt32(this.Request.Form["notification_id"]);
            //tbl_notification note = null;
            //if (int32_4 == 1)
            //    note = this.db.tbl_notification.FirstOrDefault(t => t.id_notification == nids && t.status == "A");

            //int[] source = Array.ConvertAll(this.Request.Form["chk_user"].Split(','), int.Parse);
            //DateTime? nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"]) ? DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture) : (DateTime?)null;
            //DateTime? nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"]) ? DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture) : (DateTime?)null;

            for (int i = 0; i < source.Length; i += 1000)
            {
                var batchSource = source.Skip(i).Take(1000).ToList();

                using (var dbContext = new db_m2ostEntities()) // Create your DbContext here
                {
                    foreach (int user_idn in batchSource)
                    {
                        tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(user_idn);
                        tbl_content_program_mapping contentProgramMapping1 = dbContext.tbl_content_program_mapping.FirstOrDefault(t => t.id_category == ids && t.id_organization == orgid && t.id_user == user_idn);

                        if (contentProgramMapping1 == null)
                        {
                            tbl_content_program_mapping contentProgramMapping2 = new tbl_content_program_mapping
                            {
                                map_type = 1,
                                id_role = int.TryParse(idrole, out int roleId) ? roleId : throw new FormatException("role-Id is not in a correct format."),
                                id_user = user_idn,
                                id_organization = orgid,
                                id_category = ids,
                                status = "A",
                                option_type = 0,
                                start_date = nullable1,
                                expiry_date = nullable2,
                                id_category_tile = int32_1,
                                id_category_heading = int32_2,
                                id_assessment_sheet = 0,
                                updated_date_time = DateTime.Now
                                //map_type = 1,
                                //id_role = Convert.ToInt32(idrole),
                                //id_user = user_idn,
                                //id_organization = orgid,
                                //id_category = ids,
                                //status = "A",
                                //option_type = 0,
                                //start_date = nullable1,
                                //expiry_date = nullable2,
                                //id_category_tile = int32_1,
                                //id_category_heading = int32_2,
                                //id_assessment_sheet = 0,
                                //updated_date_time = DateTime.Now
                            };

                            dbContext.tbl_content_program_mapping.Add(contentProgramMapping2);
                            dbContext.SaveChanges();

                            if (contentProgramMapping2.id_content_program_mapping > 0 && note != null)
                            {
                                tbl_notification_config entity = new tbl_notification_config
                                {
                                    id_assessment = 0,
                                    id_category = ids,
                                    id_content = 0,
                                    id_notification = nids,
                                    id_user = user_idn,
                                    id_creater = int32_3,
                                    start_date = nullable1,
                                    end_date = nullable2,
                                    notification_key = note.notification_key,
                                    notification_action_type = "PRO",
                                    read_timestamp = DateTime.Now,
                                    updated_date_time = DateTime.Now,
                                    user_type = "1",
                                    status = "P"
                                };

                                dbContext.tbl_notification_config.Add(entity);
                                dbContext.SaveChanges();
                                id_config = entity.id_notification_config;
                            }

                            if (!string.IsNullOrEmpty(mailId.EMAIL) && note != null)
                                new addCMS_CategoryModel().sendmail_program(mailId, note, id_config, contentProgramMapping2, orgid);
                        }
                        else
                        {
                            contentProgramMapping1.start_date = nullable1;
                            contentProgramMapping1.expiry_date = nullable2;
                            contentProgramMapping1.updated_date_time = DateTime.Now;
                            dbContext.SaveChanges();
                        }
                    }
                }
            }

            this.sendProgramNotification(ids.ToString());
            return multiUser;
        }

        /// <returns></returns>

        //public string setUserBasedAssessmentToMultiUser()
        //{
        //    int id_config = 0;
        //    int[] source = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
        //    DateTime? nullable1 = new DateTime?();
        //    DateTime? nullable2 = new DateTime?();
        //    nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
        //    nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
        //    int ids = Convert.ToInt32(this.Request.Form["id_content"].ToString());
        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int orgid = Convert.ToInt32(content.id_ORGANIZATION);
        //    int int32_1 = Convert.ToInt32(content.ID_USER);
        //    int int32_2 = Convert.ToInt32(this.Request.Form["category"].ToString());
        //    int[] numArray = source;
        //    this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();
        //    int int32_3 = Convert.ToInt32(this.Request.Form["notification_req"].ToString());
        //    int nids = 0;
        //    if (int32_3 == 1)
        //        nids = Convert.ToInt32(this.Request.Form["notification_id"].ToString());
        //    tbl_notification note = (tbl_notification)null;
        //    if (int32_3 == 1)
        //        note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
        //    for (int index = 0; index < ((IEnumerable<int>)source).Count<int>(); ++index)
        //    {
        //        int user_id = numArray[index];
        //        tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(user_id);
        //        tbl_assessment_user_assignment assessmentUserAssignment1 = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)user_id)).FirstOrDefault<tbl_assessment_user_assignment>();
        //        if (assessmentUserAssignment1 == null)
        //        {
        //            tbl_assessment_user_assignment assessmentUserAssignment2 = new tbl_assessment_user_assignment();
        //            assessmentUserAssignment2.id_user = new int?(numArray[index]);
        //            assessmentUserAssignment2.id_organization = new int?(orgid);
        //            assessmentUserAssignment2.id_assessment = new int?(ids);
        //            assessmentUserAssignment2.id_category = new int?(int32_2);
        //            assessmentUserAssignment2.status = "A";
        //            assessmentUserAssignment2.start_date = nullable1;
        //            assessmentUserAssignment2.expire_date = nullable2;
        //            assessmentUserAssignment2.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.tbl_assessment_user_assignment.Add(assessmentUserAssignment2);
        //            this.db.SaveChanges();
        //            if (assessmentUserAssignment2.id_assessment_user_assignment > 0 && note != null)
        //            {
        //                tbl_notification_config entity = new tbl_notification_config();
        //                entity.id_assessment = new int?(ids);
        //                entity.id_category = new int?(0);
        //                entity.id_content = new int?(0);
        //                entity.id_creater = new int?(int32_1);
        //                entity.id_notification = new int?(nids);
        //                entity.id_user = new int?(numArray[index]);
        //                entity.start_date = nullable1;
        //                entity.end_date = nullable2;
        //                entity.notification_key = note.notification_key;
        //                entity.notification_action_type = "ASS";
        //                entity.read_timestamp = new DateTime?(DateTime.Now);
        //                entity.updated_date_time = new DateTime?(DateTime.Now);
        //                entity.user_type = "1";
        //                entity.status = "P";
        //                this.db.tbl_notification_config.Add(entity);
        //                this.db.SaveChanges();
        //                id_config = entity.id_notification_config;
        //            }
        //            if (mailId.EMAIL != "" && note != null)
        //                new addCMS_CategoryModel().sendmail_assessment(mailId, note, id_config, assessmentUserAssignment2, orgid);
        //        }
        //        else
        //        {
        //            assessmentUserAssignment1.start_date = nullable1;
        //            assessmentUserAssignment1.expire_date = nullable2;
        //            assessmentUserAssignment1.updated_date_time = new DateTime?(DateTime.Now);
        //            this.db.SaveChanges();
        //        }
        //    }
        //    this.sendAssessmentNotification(ids.ToString());
        //    return "1";
        //}

        /// <summary>
        //Bulkinsert
        public string setUserBasedAssessmentToMultiUser()
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "USER BASED ASSESSMENT ASSIGMENT";
                string Id_assessment = null;
                string Id_category = this.Request.Form["category"].ToString();
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }


            int batchSize = 1000; // Number of records to process in each batch
            int id_config = 0;
            int[] source = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
            DateTime? nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : null;
            DateTime? nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : null;
            int ids = Convert.ToInt32(this.Request.Form["id_content"].ToString());
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_1 = Convert.ToInt32(content.ID_USER);
            int int32_2 = Convert.ToInt32(this.Request.Form["category"].ToString());
            int int32_3 = Convert.ToInt32(this.Request.Form["notification_req"].ToString());
            int nids = 0;
            if (int32_3 == 1)
                nids = Convert.ToInt32(this.Request.Form["notification_id"].ToString());
            tbl_notification note = null;
            if (int32_3 == 1)
                note = this.db.tbl_notification.FirstOrDefault(t => t.id_notification == nids && t.status == "A");

            for (int i = 0; i < source.Length; i += batchSize)
            {
                var batch = source.Skip(i).Take(batchSize);

                foreach (int user_id in batch)
                {
                    tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(user_id);
                    tbl_assessment_user_assignment assessmentUserAssignment1 = this.db.tbl_assessment_user_assignment.FirstOrDefault(t => t.id_assessment == ids && t.id_organization == orgid && t.id_user == user_id);
                    if (assessmentUserAssignment1 == null)
                    {
                        tbl_assessment_user_assignment assessmentUserAssignment2 = new tbl_assessment_user_assignment();
                        assessmentUserAssignment2.id_user = user_id;
                        assessmentUserAssignment2.id_organization = orgid;
                        assessmentUserAssignment2.id_assessment = ids;
                        assessmentUserAssignment2.id_category = int32_2;
                        assessmentUserAssignment2.status = "A";
                        assessmentUserAssignment2.start_date = nullable1;
                        assessmentUserAssignment2.expire_date = nullable2;
                        assessmentUserAssignment2.updated_date_time = DateTime.Now;
                        this.db.tbl_assessment_user_assignment.Add(assessmentUserAssignment2);
                        this.db.SaveChanges();
                        if (assessmentUserAssignment2.id_assessment_user_assignment > 0 && note != null)
                        {
                            tbl_notification_config entity = new tbl_notification_config();
                            entity.id_assessment = ids;
                            entity.id_category = 0;
                            entity.id_content = 0;
                            entity.id_creater = int32_1;
                            entity.id_notification = nids;
                            entity.id_user = user_id;
                            entity.start_date = nullable1;
                            entity.end_date = nullable2;
                            entity.notification_key = note.notification_key;
                            entity.notification_action_type = "ASS";
                            entity.read_timestamp = DateTime.Now;
                            entity.updated_date_time = DateTime.Now;
                            entity.user_type = "1";
                            entity.status = "P";
                            this.db.tbl_notification_config.Add(entity);
                            this.db.SaveChanges();
                            id_config = entity.id_notification_config;
                        }
                        if (!string.IsNullOrEmpty(mailId.EMAIL) && note != null)
                            new addCMS_CategoryModel().sendmail_assessment(mailId, note, id_config, assessmentUserAssignment2, orgid);
                    }
                    else
                    {
                        assessmentUserAssignment1.start_date = nullable1;
                        assessmentUserAssignment1.expire_date = nullable2;
                        assessmentUserAssignment1.updated_date_time = DateTime.Now;
                        this.db.SaveChanges();
                    }
                }
            }
            this.sendAssessmentNotification(ids.ToString());
            return "1";
        }



        /// <returns></returns>
        public class UserIdDataUplodeModelAssessment
        {

            public string userId { get; set; }
            public string idassessment { get; set; }
            public string std { get; set; }
            public string exd { get; set; }
            public string idcategory { get; set; }
            public string ncid { get; set; }
            public string idrole { get; set; }
           
            public string notification_req { get; set; }

        }

        // For the Uplode File Assessment

        [HttpPost]
        public ActionResult uplodeUserIDAssessment(List<UserIdDataUplodeModelAssessment> userIdStatusArray, bool isLastChunk)
        {
            List<string> nonExistentUserIds = Session["NonExistentUserIds"] as List<string> ?? new List<string>();
            List<string> allreadyUserIds = Session["AllreadyUserIds"] as List<string> ?? new List<string>();
            bool hasUpdates = false;

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession == null)
            {
                return Json(new { success = false, message = "User session is not available." });
            }

            string orgId = userSession.id_ORGANIZATION;
            string userIdSession = userSession.ID_USER;
            string page = "PROGRAM ASSIGNMENT Bulk UploadUserID";
            string idProgram = Request.Form["id_program"];
            string idOperation = "Save";

            // Log the user action
            UserLogDetails userLogDetails = new UserLogDetails();
            userLogDetails.AddUserDataLogopration(userIdSession, orgId, page, idProgram, null, idOperation);



            int id_config = 0;
            tbl_notification note = null;

            foreach (var userStatus in userIdStatusArray)
            {
                string userId = userStatus.userId;
                int orgIdInt = Convert.ToInt32(userSession.id_ORGANIZATION);
                string ncid = userStatus.ncid;

                // Check if user exists
                var tblUser = this.db.tbl_user.FirstOrDefault(t => t.USERID == userId);
                if (tblUser != null)
                {
                    int userRoleId;
                    if (!int.TryParse(userStatus.idrole, out userRoleId))
                    {
                        return Json(new { success = false, message = "Invalid role ID for user " + userId });
                    }

                    int userContentId = tblUser.ID_USER;
                    int assessment = int.Parse(userStatus.idassessment);

                    var roleMapping = this.db.tbl_role_user_mapping
                        .FirstOrDefault(t => t.id_csst_role == userRoleId && t.id_user == userContentId);
                    if (roleMapping != null)
                    {
                        int idcategory_1 = Convert.ToInt16(userStatus.idcategory);
                        var user_assignment = this.db.tbl_assessment_user_assignment
                            .FirstOrDefault(c => c.id_category == idcategory_1 && c.id_user == userContentId && c.id_assessment == assessment);
                        // var program_mapping = this.db.tbl_content_program_mapping.FirstOrDefault(c => c.id_category == Convert.ToInt16(userStatus.idprogram) && c.id_user == userContentId);

                        if (user_assignment == null)
                        {
                            DateTime datetime1 = new Utility().StringToDatetime(userStatus.std);
                            DateTime datetime2 = new Utility().StringToDatetime(userStatus.exd);


                            // Create a new mapping entry
                            tbl_assessment_user_assignment entity = new tbl_assessment_user_assignment
                            {                                
                            
                                id_user = userContentId,
                                id_organization = orgIdInt,
                                id_category = Convert.ToInt16(userStatus.idcategory),
                                id_assessment = Convert.ToInt16(userStatus.idassessment),
                                status = "A",
                                start_date = new DateTime?(datetime1),
                                expire_date = new DateTime?(datetime2),
                                updated_date_time = DateTime.Now
                            };


                            this.db.tbl_assessment_user_assignment.Add(entity);
                           
                            int int32_3 = Convert.ToInt32(userStatus.notification_req);
                            int nids = 0;
                            if (int32_3 == 1)
                                nids = Convert.ToInt32(userStatus.ncid);
                       
                            if (int32_3 == 1)
                                note = this.db.tbl_notification.FirstOrDefault(t => t.id_notification == nids && t.status == "A");


                            if (int32_3 == 1)
                            {
                                note = this.db.tbl_notification.FirstOrDefault(t => t.id_notification == int32_3 && t.status == "A");

                                tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(userContentId);

                                if (entity.id_assessment_user_assignment > 0 && userStatus.ncid != null)
                                {
                                    tbl_notification_config entity_1 = new tbl_notification_config();
                                    entity_1.id_assessment = Convert.ToInt16(userStatus.idassessment);
                                    entity_1.id_category = 0;
                                    entity_1.id_content = 0;
                                    entity_1.id_creater = userContentId;
                                    entity_1.id_notification = nids;
                                    entity_1.id_user = userContentId;
                                    entity_1.start_date = new DateTime?(datetime1);
                                    entity_1.end_date = new DateTime?(datetime2);
                                    entity_1.notification_key = note.notification_key;
                                    entity_1.notification_action_type = "ASS";
                                    entity_1.read_timestamp = DateTime.Now;
                                    entity_1.updated_date_time = DateTime.Now;
                                    entity_1.user_type = "1";
                                    entity_1.status = "P";
                                    this.db.tbl_notification_config.Add(entity_1);
                                    this.db.SaveChanges();
                                    id_config = entity_1.id_notification_config;
                                }
                                if (!string.IsNullOrEmpty(mailId.EMAIL) && userStatus.ncid != null)
                                    new addCMS_CategoryModel().sendmail_assessment(mailId, note, id_config, entity, orgIdInt);
                            }


                            hasUpdates = true;
                            //activeUserIds.Add(userId);
                        }
                        else
                        {

                            allreadyUserIds.Add(userId);
                        }
                    }
                    else
                    {
                        nonExistentUserIds.Add(userId);
                    }
                }
                else
                {
                    nonExistentUserIds.Add(userId);
                }
            }

            Session["NonExistentUserIds"] = nonExistentUserIds;
            Session["AllreadyUserIds"] = allreadyUserIds;

            if (hasUpdates)
            {
                this.db.SaveChanges();
               //return Json(new { success = true, message = "Status changed successfully.", nonExistentUserIds, allreadyUserIds });
            }
            //else
            //{
            //    return Json(new { success = false, message = "No users were updated.", nonExistentUserIds, allreadyUserIds });
            //}
            if (isLastChunk)
            {

                return Json(new { success = true, message = "Status changed successfully" });
            }

            return new EmptyResult();
        }
        [HttpGet]
        public ActionResult GetNonExistentUserIds()
        {
            var nonExistentUserIds = Session["NonExistentUserIds"] as List<string> ?? new List<string>();
            var AddExistentUserIds = Session["AllreadyUserIds"] as List<string> ?? new List<string>();
            Session.Remove("NonExistentUserIds");
            Session.Remove("AllreadyUserIds");
            return Json(new { success = true, nonExistentUserIds, AddExistentUserIds }, JsonRequestBehavior.AllowGet);
        }



        public string getUserListForProgram(string id, string pattern, string cid, string type)
        {
            int int32 = Convert.ToInt32(id);
            int cids = Convert.ToInt32(cid);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern.Replace("'", "''");
            string strfunction = "";
            string str1 = "";
            if (type == "1")
                str1 = " and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + (object)int32 + ") ";
            string str2 = "";
            if (type == "2")
            {
                string[] strArray = pattern.Split('|');
                string str3 = strArray[0];
                pattern = strArray[1];
                str2 = " and id_user in (select id_user from tbl_profile where (upper(CITY) like '%" + str3 + "%' OR upper(LOCATION) like '%" + str3 + "%')) ";
            }
            string str4 = "";
            string str5 = "";
            if (type == "5")
            {
                string[] strArray = pattern.Split('|');
                string str3 = strArray[0];
                pattern = strArray[0];
                strfunction = " user_function='" + pattern + "' and ";
            }
            else
            {
                if (!string.IsNullOrEmpty(pattern))
                {
                    str5 = " ( upper(USERID) like '%" + pattern + "%'  OR upper(EMPLOYEEID) like '%" + pattern + "%'  ) and ";
                    str4 = " and id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + pattern + "%' or upper(LASTNAME) like '%" + pattern + "%') ";
                }
            }
            string sql = "select * from tbl_user where  STATUS = 'A' AND " + strfunction + "" + str5 + " id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) " + str4 + str1 + str2;
            List<int> intList = new List<int>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            bool flag = false;
            string str6 = "";
            string str7 = "";
            string str8 = "";
            string str9 = "";
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
                if (tblProfile != null)
                {
                    str8 = str8 + "<tr><td>" + tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME + " (" + item.USERID + ") </td>";
                    str8 = str8 + "<td>"  + item.UPDATEDTIME + " </td>";
                    str8 = str8 + "<td>" + item.user_grade + " </td>";
                    

                    str8 += "<td>";
                    if (flag)
                    {
                        str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";

                        str8 += "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"additional_chk_user\" class=\"additionalCheckbox\"  value=\"" + (object)item.ID_USER + "\">";

                    }
                    else
                    {
                        // i want one more checkbox in one  row
                        tbl_content_program_mapping contentProgramMapping = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
                        if (contentProgramMapping == null)
                        {
                            str8 = str8 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object)item.ID_USER + "\">";
                            str8 = str8 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        }
                        else
                        {
                            str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                            str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a> | ";
                            string[] strArray = new string[6]
                            {
                str8,
                " ( ",
                null,
                null,
                null,
                null
                            };
                            DateTime? nullable = contentProgramMapping.start_date;
                            DateTime dateTime = nullable.Value;
                            strArray[2] = dateTime.ToShortDateString();
                            strArray[3] = " to ";
                            nullable = contentProgramMapping.expiry_date;
                            dateTime = nullable.Value;
                            strArray[4] = dateTime.ToShortDateString();
                            strArray[5] = " )";
                            str8 = string.Concat(strArray);
                        }
                    }
                    // test

                    str8 += "<td>";
                    if (flag)
                    {
                        str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";

                        str8 += "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"additional_chk_user\" class=\"additionalCheckbox\"  value=\"" + (object)item.ID_USER + "\">";

                    }
                    else
                    {

                        tbl_content_program_mapping contentProgramMapping = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
                        if (contentProgramMapping != null)
                        {
                            str8 = str8 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox_D\"  value=\"" + (object)item.ID_USER + "\">";
                            str8 = str8 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        }


                    }
                   //
                    str8 += "</td>";
                    str8 += "</tr>";
                }
            }
            string userListForProgram = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>" + " <th width=\"80%\">Update Date </th>" + " <th width=\"80%\">User Grade</th>" + "<th  width=\"20%\"><input type = \"checkbox\" id = \"checkAll\" onclick = \"check_all()\"> Select all &nbsp;&nbsp;</th>" + "<th  width=\"20%\"><input type = \"checkbox\" id = \"checkAll1\" onclick = \"check_Deactive()\"> Select all Unassign  <button type=\"button\" class=\"btn btn-primary\" onclick=\"Deactive()\">Unassign</button> </th>" + "</thead>" + "<tbody>" + str8 + "</tbody></table>";
            if (flag)
            {
                userListForProgram = " <div class=\"row\" id=\"div-remove\" >   <div class=\"col-md-12\">   <div class=\"alert alert-info alert-dismissable\">   <input id=\"program-assignment\" type=\"button\" class=\"btn btn-primary btn-sm\" value=\"Remove Program From Role\" onclick=\"removeProgramToRole(0)\" /><strong>&nbsp;&nbsp; Click to Remove Role from  Program  </strong>   </div>   </div>   </div><hr/>" + userListForProgram;
            }

            userListForProgram += "<div class='row'>  <div class='form-group'> <div class='col-md-2'><label class='control-label'> Start Date_1</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker1' name='start-date' value='" + str6 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div> <div class='col-md-2'><label class='control-label'>Expiry Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker2' name='expiry-date'  value='" + str7 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div>  </div>   </div>";

            return userListForProgram;
        }

        public string getUserListForContentBased(string id, string pattern, string cid, string type)
        {
            int IsModified = 0;
            int int32 = Convert.ToInt32(id);
            int cids = Convert.ToInt32(cid);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern.Replace("'", "''");
            string strfunction = "";

            string str1 = "";
            if (type == "1")
                str1 = " and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + (object)int32 + ") ";
            string str2 = "";
            if (type == "2")
            {
                string[] strArray = pattern.Split('|');
                string str3 = strArray[0];
                pattern = strArray[1];
                str2 = " and id_user in (select id_user from tbl_profile where (upper(CITY) like '%" + str3 + "%' OR upper(LOCATION) like '%" + str3 + "%')) ";
            }
            string str4 = "";
            string str5 = "";
            if (type == "5")
            {
                string[] strArray = pattern.Split('|');
                string str3 = strArray[0];
                pattern = strArray[0];
                strfunction = " user_function='" + pattern + "' and ";
            }
            else
            {
                if (!string.IsNullOrEmpty(pattern))
                {
                    IsModified = 1;
                    str5 = " (upper(USERID) like '%" + pattern + "%'  OR upper(EMPLOYEEID) like '%" + pattern + "%'  ) ";
                    str4 = " OR id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + pattern + "%' or upper(LASTNAME) like '%" + pattern + "%')  ";

                }
            }
            string sql = "";

            if (IsModified==1)
            {
                sql = "select * from tbl_user where (" + strfunction + "" + str5 + str4 + ") AND id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) " + str1 + str2;
            }
            else
            {
                sql = "select * from tbl_user where " + strfunction + "" + str5 + "  id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) " + str4 + str1 + str2;
            }
            List<int> intList = new List<int>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            bool flag = false;
            string str6 = "";
            string str7 = "";
            string str8 = "";
            if (type == "5")
            {

                string sql1 = "";
                list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            }
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
                if (tblProfile != null)
                {
                    str8 = str8 + "<tr><td>" + tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME + " (" + item.USERID + ") </td>";
                    str8 += "<td>";
                    if (flag)
                    {
                        str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                    }
                    else
                    {
                        tbl_content_user_assisgnment contentUserAssisgnment = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_user_assisgnment>();
                        if (contentUserAssisgnment == null)
                        {
                            str8 = str8 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object)item.ID_USER + "\">";
                            str8 = str8 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        }
                        else
                        {
                            str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                            str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeContentToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a> | ";
                            string[] strArray = new string[6]
                            {
                str8,
                " ( ",
                null,
                null,
                null,
                null
                            };
                            DateTime? nullable = contentUserAssisgnment.start_date;
                            DateTime dateTime = nullable.Value;
                            strArray[2] = dateTime.ToShortDateString();
                            strArray[3] = " to ";
                            nullable = contentUserAssisgnment.expiry_date;
                            dateTime = nullable.Value;
                            strArray[4] = dateTime.ToShortDateString();
                            strArray[5] = " )";
                            str8 = string.Concat(strArray);
                        }
                    }
                    str8 += "</td>";
                    str8 += "</tr>";
                }
            }
            string str9 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>" + "<th  width=\"20%\"><input type = \"checkbox\" id = \"checkAll\" onclick = \"check_all()\"> Select all &nbsp;&nbsp;</th>" + "</thead>" + "<tbody>" + str8 + "</tbody></table>";
            if (flag)
            {
                str9 = " <div class=\"row\" id=\"div-remove\" >   <div class=\"col-md-12\">   <div class=\"alert alert-info alert-dismissable\">   <input id=\"program-assignment\" type=\"button\" class=\"btn btn-primary btn-sm\" value=\"Remove Program From Role\" onclick=\"removeProgramToRole(0)\" /><strong>&nbsp;&nbsp; Click to Remove Role from  Program  </strong>   </div>   </div>   </div><hr/>" + str9;
            }

            str9 += "<div class='row'>  <div class='form-group'> <div class='col-md-2'><label class='control-label'> Start Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker1' name='start-date' value='" + str6 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div> <div class='col-md-2'><label class='control-label'>Expiry Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker2' name='expiry-date'  value='" + str7 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div>  </div>   </div>";
            return "<hr/>" + str9;
        }


        public string getUserListForUserBasedAssessment(
        string id,
        string pattern,
        string cid,
        string type)
        {
            int int32 = Convert.ToInt32(id);
            int cids = Convert.ToInt32(cid);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern.Replace("'", "''");
            string str1 = "";
            string strfunction = "";
            if (type == "1")
                str1 = " and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + (object)int32 + ") ";
            string str2 = "";
            if (type == "2")
            {
                string[] strArray = pattern.Split('|');
                string str3 = strArray[0];
                pattern = strArray[1];
                str2 = " and id_user in (select id_user from tbl_profile where (upper(CITY) like '%" + str3 + "%' OR upper(LOCATION) like '%" + str3 + "%')) ";
            }
            string str4 = "";
            string str5 = "";
            if (type == "5")
            {
                string[] strArray = pattern.Split('|');
                string str3 = strArray[0];
                pattern = strArray[0];
                strfunction = " user_function='" + pattern + "' and ";
            }
            else
            {
                if (!string.IsNullOrEmpty(pattern))
                {
                    str5 = " ( upper(USERID) like '%" + pattern + "%'  OR upper(EMPLOYEEID) like '%" + pattern + "%'  ) and ";
                    str4 = " and id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + pattern + "%' or upper(LASTNAME) like '%" + pattern + "%') ";
                }
            }
            string sql = "select * from tbl_user where STATUS = 'A' AND " + strfunction + "" + str5 + " id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) " + str4 + str1 + str2;
            //
            // string NageList = NgageForUserBasedAssesment(id,pattern,cid,type);

            //
            List<int> intList = new List<int>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            bool flag = false;
            string str6 = "";
            string str7 = "";
            string str8 = "";
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
                if (tblProfile != null)
                {
                    str8 = str8 + "<tr><td>" + tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME + " (" + item.USERID + ") </td>";
                    str8 = str8 + "<td>" + item.UPDATEDTIME + "</td>";
                    str8 = str8 + "<td>" + item.user_grade + "</td>";
                    //str8 = str8 + "<td>";
                    str8 += "<td>";
                    if (flag)
                    {
                        str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                    }
                    else
                    {
                        tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_assessment_user_assignment>();
                        if (assessmentUserAssignment == null)
                        {
                            str8 = str8 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object)item.ID_USER + "\">";
                            str8 = str8 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        }
                        else
                        {
                            str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                            str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeContentToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a> | ";
                            string[] strArray = new string[6]
                            {
                str8,
                " ( ",
                null,
                null,
                null,
                null
                            };
                            DateTime? nullable = assessmentUserAssignment.start_date;
                            DateTime dateTime = nullable.Value;
                            strArray[2] = dateTime.ToShortDateString();
                            strArray[3] = " to ";
                            nullable = assessmentUserAssignment.expire_date;
                            dateTime = nullable.Value;
                            strArray[4] = dateTime.ToShortDateString();
                            strArray[5] = " )";
                            str8 = string.Concat(strArray);
                        }
                    }
                    str8 += "</td>";

                    // Test for checkbox
                    str8 += "<td>";


                    if (flag)
                    {
                        str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";

                        str8 += "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"additional_chk_user\" class=\"additionalCheckbox\"  value=\"" + (object)item.ID_USER + "\">";

                    }
                    else
                    {

                        tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_assessment_user_assignment>();
                        if (assessmentUserAssignment != null)
                        {
                            str8 = str8 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox_D\"  value=\"" + (object)item.ID_USER + "\">";
                            str8 = str8 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                        }


                    }
                    str8 += "</td>";
                    //


                    str8 += "</tr>";
                }
            }
            string str9 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" +
              "<th width=\"80%\">User Info</th>" +
              "<th width=\"80%\">Create Date</th>" +  // Corrected closing tag for "Create Date"
              "<th width=\"80%\">User Grade</th>" +  // Corrected closing tag for "User Grade"
              "<th width=\"20%\"><input type=\"checkbox\" id=\"checkAll\" onclick=\"check_all()\"> Select all &nbsp;&nbsp;</th>" +
              "<th width=\"20%\"><input type = \"checkbox\" id = \"checkAll1\" onclick = \"check_Deactive()\"> Select all Unassign  <button type=\"button\" class=\"btn btn-primary\" onclick=\"Deactive()\">Unassign</button> </th>" +
              "</tr></thead><tbody>" + str8 + "</tbody></table>";

            // string str9 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>"+ " <th width=\"80%\">Create Date<th>" + " <th width=\"80%\">User Grade<th>" + "<th  width=\"20%\"><input type = \"checkbox\" id = \"checkAll\" onclick = \"check_all()\"> Select all &nbsp;&nbsp;</th>" + "</thead>" + "<tbody>" + str8 + "</tbody></table>";
            if (flag)
                str9 = " <div class=\"row\" id=\"div-remove\" >   <div class=\"col-md-12\">   <div class=\"alert alert-info alert-dismissable\">   <input id=\"program-assignment\" type=\"button\" class=\"btn btn-primary btn-sm\" value=\"Remove Program From Role\" onclick=\"removeProgramToRole(0)\" /><strong>&nbsp;&nbsp; Click to Remove Role from  Program  </strong>   </div>   </div>   </div><hr/>" + str9;
            string str10 = "<div class='row'>  <div class='form-group'> <div class='col-md-2'><label class='control-label'> Start Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker1' name='start-date' value='" + str6 + "' /> " + "  <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div> <div class='col-md-2'><label class='control-label'>Expiry Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker2' name='expiry-date'  value='" + str7 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div>  </div>   </div>";
            return "<hr/>" + str9;
        }


        //public string getUserListForUserBasedAssessment(
        //  string id,
        //  string pattern,
        //  string cid,
        //  string type)
        //{
        //    int int32 = Convert.ToInt32(id);
        //    int cids = Convert.ToInt32(cid);
        //    int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
        //    pattern = pattern.Replace("'", "''");
        //    string str1 = "";
        //    string strfunction = "";
        //    if (type == "1")
        //        str1 = " and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + (object)int32 + ") ";
        //    string str2 = "";
        //    if (type == "2")
        //    {
        //        string[] strArray = pattern.Split('|');
        //        string str3 = strArray[0];
        //        pattern = strArray[1];
        //        str2 = " and id_user in (select id_user from tbl_profile where (upper(CITY) like '%" + str3 + "%' OR upper(LOCATION) like '%" + str3 + "%')) ";
        //    }
        //    string str4 = "";
        //    string str5 = "";
        //    if (type == "5")
        //    {
        //        string[] strArray = pattern.Split('|');
        //        string str3 = strArray[0];
        //        pattern = strArray[0];
        //        strfunction = " user_function='" + pattern + "' and ";
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(pattern))
        //        {
        //            str5 = " ( upper(USERID) like '%" + pattern + "%'  OR upper(EMPLOYEEID) like '%" + pattern + "%'  ) and ";
        //            str4 = " and id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + pattern + "%' or upper(LASTNAME) like '%" + pattern + "%') ";
        //        }
        //    }
        //    string sql = "select * from tbl_user where STATUS='A' AND " + strfunction + "" + str5 + " id_user in (select id_user from tbl_role_user_mapping  where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) " + str4 + str1 + str2;
        //    //
        //   // string NageList = NgageForUserBasedAssesment(id,pattern,cid,type);

        //    //
        //    List<int> intList = new List<int>();
        //    List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
        //    bool flag = false;
        //    string str6 = "";
        //    string str7 = "";
        //    string str8 = "";
        //    foreach (tbl_user tblUser in list)
        //    {
        //        tbl_user item = tblUser;
        //        tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
        //        if (tblProfile != null)
        //        {
        //            str8 = str8 + "<tr><td>" + tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME + " (" + item.USERID + ") </td>";
        //            str8 = str8 + "<td>"+ item.UPDATEDTIME + "</td>";
        //            str8 = str8 + "<td>" + item.user_grade + "</td>";
        //            //str8 = str8 + "<td>";
        //            str8 += "<td>";
        //            if (flag)
        //            {
        //                str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
        //                str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";

        //               /// str8 += "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"additional_chk_user\" class=\"additionalCheckbox\"  value=\"" + (object)item.ID_USER + "\">";


        //            }
        //            else
        //            {
        //                tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_assessment_user_assignment>();
        //                if (assessmentUserAssignment == null)
        //                {
        //                    str8 = str8 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox\"  value=\"" + (object)item.ID_USER + "\">";
        //                    str8 = str8 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
        //                }
        //                else
        //                {
        //                    str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
        //                    str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeContentToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a> | ";
        //                    string[] strArray = new string[6]
        //                    {
        //        str8,
        //        " ( ",
        //        null,
        //        null,
        //        null,
        //        null
        //                    };
        //                    DateTime? nullable = assessmentUserAssignment.start_date;
        //                    DateTime dateTime = nullable.Value;
        //                    strArray[2] = dateTime.ToShortDateString();
        //                    strArray[3] = " to ";
        //                    nullable = assessmentUserAssignment.expire_date;
        //                    dateTime = nullable.Value;
        //                    strArray[4] = dateTime.ToShortDateString();
        //                    strArray[5] = " )";
        //                    str8 = string.Concat(strArray);
        //                }
        //            }
        //            str8 += "</td>";


        //            // Test for checkbox
        //            str8 += "<td>";


        //            if (flag)
        //            {
        //                str8 = str8 + "<i id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
        //                str8 = str8 + " | <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"removeProgramToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";

        //                str8 += "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"additional_chk_user\" class=\"additionalCheckbox\"  value=\"" + (object)item.ID_USER + "\">";

        //            }
        //            else
        //            {

        //                tbl_assessment_user_assignment assessmentUserAssignment = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_assessment_user_assignment>();
        //                if (assessmentUserAssignment != null)
        //                {
        //                    str8 = str8 + "<input style=\"margin-left:5%\" type=\"checkbox\" name=\"chk_user\" class=\"myCheckbox_D\"  value=\"" + (object)item.ID_USER + "\">";
        //                    str8 = str8 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
        //                }


        //            }
        //            str8 += "</td>";
        //            //

        //            str8 += "<td>";
        //            str8 += "</tr>";
        //        }
        //    }
        //    string str9 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" +
        //      "<th width=\"80%\">User Info</th>" +
        //      "<th width=\"80%\">Create Date</th>" +  // Corrected closing tag for "Create Date"
        //      "<th width=\"80%\">User Grade</th>" +  // Corrected closing tag for "User Grade"
        //      "<th width=\"20%\"><input type=\"checkbox\" id=\"checkAll\" onclick=\"check_all()\"> Select all &nbsp;&nbsp;</th>" +
        //      "<th width=\"20%\"><input type = \"checkbox\" id = \"checkAll1\" onclick = \"check_Deactive()\"> Select all Unassign  <button type=\"button\" class=\"btn btn-primary\" onclick=\"Deactive()\">Unassign</button> </th>" +
        //      "</tr></thead><tbody>" + str8 + "</tbody></table>";

        //   // string str9 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>"+ " <th width=\"80%\">Create Date<th>" + " <th width=\"80%\">User Grade<th>" + "<th  width=\"20%\"><input type = \"checkbox\" id = \"checkAll\" onclick = \"check_all()\"> Select all &nbsp;&nbsp;</th>" + "</thead>" + "<tbody>" + str8 + "</tbody></table>";
        //    if (flag)
        //        str9 = " <div class=\"row\" id=\"div-remove\" >   <div class=\"col-md-12\">   <div class=\"alert alert-info alert-dismissable\">   <input id=\"program-assignment\" type=\"button\" class=\"btn btn-primary btn-sm\" value=\"Remove Program From Role\" onclick=\"removeProgramToRole(0)\" /><strong>&nbsp;&nbsp; Click to Remove Role from  Program  </strong>   </div>   </div>   </div><hr/>" + str9;
        //    string str10 = "<div class='row'>  <div class='form-group'> <div class='col-md-2'><label class='control-label'> Start Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker1' name='start-date' value='" + str6 + "' /> " + "  <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div> <div class='col-md-2'><label class='control-label'>Expiry Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker2' name='expiry-date'  value='" + str7 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div>  </div>   </div>";
        //    return "<hr/>" + str9;
        //}


        //public string NgageForUserBasedAssesment(
        //    string id,
        //  string pattern,
        //  string cid,
        //  string type)
        //{
        //    int int32 = Convert.ToInt32(id);
        //    int cids = Convert.ToInt32(cid);
        //    int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
        //    List<Ngagetbl_user> users = NageUserList.Ngagetbl_user(orgid);
        //    return "";
        //}

        public ActionResult contentCreationRichText()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add Content with Rich Text";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (this.TempData["LINKTYPE"] != null)
            {
                int id = Convert.ToInt32(this.TempData["LINKTYPE"]);
                this.ViewData["LINKTYPE"] = (object)id;
                tbl_content tblContent = this.db.tbl_content.Find(new object[1]
                {
          (object) id
                });
                this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == id && t.id_organization == orgid)).FirstOrDefault<tbl_content_organization_mapping>();
                List<tbl_category> list = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
                this.ViewData["parent-content"] = (object)tblContent;
                this.ViewData["parent-category"] = (object)list;
            }
            else
            {
                this.ViewData["LINKTYPE"] = (object)0;
                this.ViewData["parent-content"] = (object)null;
                this.ViewData["parent-category"] = (object)null;
            }
            List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            List<tbl_content_level> list2 = this.db.tbl_content_level.ToList<tbl_content_level>();
            List<tbl_csst_role> list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            if (list3.Count == 0)
            {
                this.db.tbl_csst_role.Add(new tbl_csst_role()
                {
                    id_organization = new int?(orgid),
                    csst_role = "User",
                    status = "A",
                    updated_dated_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
                list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            }
            this.ViewData["select-cscc-role"] = (object)list3;
            this.ViewData["select-category"] = (object)list1;
            this.ViewData["select-level"] = (object)list2;
            return (ActionResult)this.View();
        }

        public ActionResult contentCreationPPS()
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (this.TempData["LINKTYPE"] != null)
            {
                int id = Convert.ToInt32(this.TempData["LINKTYPE"]);
                this.ViewData["LINKTYPE"] = (object)id;
                tbl_content tblContent = this.db.tbl_content.Find(new object[1]
                {
          (object) id
                });
                this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == id && t.id_organization == orgid)).FirstOrDefault<tbl_content_organization_mapping>();
                List<tbl_category> list = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
                this.ViewData["parent-content"] = (object)tblContent;
                this.ViewData["parent-category"] = (object)list;
            }
            else
            {
                this.ViewData["LINKTYPE"] = (object)0;
                this.ViewData["parent-content"] = (object)null;
                this.ViewData["parent-category"] = (object)null;
            }
            List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            List<tbl_content_level> list2 = this.db.tbl_content_level.ToList<tbl_content_level>();
            List<tbl_csst_role> list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            if (list3.Count == 0)
            {
                this.db.tbl_csst_role.Add(new tbl_csst_role()
                {
                    id_organization = new int?(orgid),
                    csst_role = "User",
                    status = "A",
                    updated_dated_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
                list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            }
            this.ViewData["select-cscc-role"] = (object)list3;
            this.ViewData["select-category"] = (object)list1;
            this.ViewData["select-level"] = (object)list2;
            return (ActionResult)this.View();
        }

        public bool InsertContent(ContentRichText obContentRichText)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.id_ORGANIZATION);
            Convert.ToInt32(content.ID_USER);
            return true;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createRichTextContent()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Content Creation Rich Text";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            int cid = 0;
            int num1 = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int OID = Convert.ToInt32(content.id_ORGANIZATION);
            int? cmdUserType = content.USER.cmd_user_type;
            int num2 = 0;
            string str1 = (cmdUserType.GetValueOrDefault() == num2 ? (cmdUserType.HasValue ? 1 : 0) : 0) == 0 ? "P" : "P";
            if (num1 > 0)
                this.db.tbl_content.Find(new object[1]
                {
          (object) num1
                });
            int num3 = 15;
            List<string> stringList = new List<string>();
            int int32_1 = Convert.ToInt32(this.Request.Form["select-level"].ToString());
            DateTime now = DateTime.Now;
            int content_id = 0;
            int answer_id = 0;
            tbl_content entity1 = new tbl_content();
            tbl_content_answer entity2 = new tbl_content_answer();
            try
            {
                entity1.CONTENT_HEADER = this.Request.Form["t-header"].ToString();
                entity1.CONTENT_QUESTION = this.Request.Form["t-quetion"].ToString();
                entity1.CONTENT_TITLE = this.Request.Form["t-title"].ToString();
                entity1.ID_THEME = num3;
                entity1.ID_CONTENT_LEVEL = int32_1;
                entity1.ID_USER = Convert.ToInt32(content.ID_USER);
                entity1.UPDATED_DATE_TIME = DateTime.Now;
                entity1.CONTENT_COUNTER = 0;
                entity1.EXPIRY_DATE = new DateTime?(now);
                entity1.STATUS = str1;
                entity1.CONTENT_OWNER = OID;
                entity1.IS_PRIMARY = num1 != 0 ? 0 : 1;
                this.db.tbl_content.Add(entity1);
                this.db.SaveChanges();
                content_id = entity1.ID_CONTENT;
                cid = content_id;
                if (content_id > 0)
                {
                    int int32_2 = Convert.ToInt32(this.Request.Form["hid-category"]);
                    for (int index = 1; index <= int32_2; ++index)
                    {
                        string str2 = Convert.ToString(this.Request.Form["add-con-category-" + (object)index]);
                        if (!string.IsNullOrEmpty(str2))
                        {
                            int citd = Convert.ToInt32(str2);
                            if (this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_category == citd && t.id_content == content_id && t.id_organization == OID)).FirstOrDefault<tbl_content_organization_mapping>() == null)
                            {
                                this.db.tbl_content_organization_mapping.Add(new tbl_content_organization_mapping()
                                {
                                    id_organization = Convert.ToInt32(content.id_ORGANIZATION),
                                    id_category = citd,
                                    id_content = entity1.ID_CONTENT,
                                    status = "A",
                                    updated_date_time = new DateTime?(DateTime.Now)
                                });
                                this.db.SaveChanges();
                            }
                        }
                    }
                }
                if (entity1.ID_CONTENT > 0)
                {
                    entity2.ID_CONTENT = content_id;
                    entity2.CONTENT_ANSWER_TITLE = "";
                    entity2.CONTENT_ANSWER_COUNTER = 0;
                    entity2.CONTENT_ANSWER_HEADER = entity1.CONTENT_HEADER;
                    entity2.CONTENT_ANSWER1 = this.Request.Unvalidated.Form["content_rt"].ToString();
                    entity2.CONTENT_ANSWER2 = "";
                    entity2.CONTENT_ANSWER3 = "";
                    entity2.CONTENT_ANSWER4 = "";
                    entity2.CONTENT_ANSWER5 = "";
                    entity2.CONTENT_ANSWER6 = "";
                    entity2.CONTENT_ANSWER7 = "";
                    entity2.CONTENT_ANSWER8 = "";
                    entity2.CONTENT_ANSWER9 = "";
                    entity2.CONTENT_ANSWER10 = "";
                    entity2.CONTENT_ANSWER_IMG1 = "";
                    entity2.CONTENT_ANSWER_IMG2 = "";
                    entity2.CONTENT_ANSWER_IMG3 = "";
                    entity2.CONTENT_ANSWER_IMG4 = "";
                    entity2.CONTENT_ANSWER_IMG5 = "";
                    entity2.CONTENT_ANSWER_IMG6 = "";
                    entity2.CONTENT_ANSWER_IMG7 = "";
                    entity2.CONTENT_ANSWER_IMG8 = "";
                    entity2.CONTENT_ANSWER_IMG9 = "";
                    entity2.CONTENT_ANSWER_IMG10 = "";
                    entity2.STATUS = "A";
                    entity2.UPDATEDTIME = DateTime.Now;
                    this.db.tbl_content_answer.Add(entity2);
                    this.db.SaveChanges();
                }
                answer_id = entity2.ID_CONTENT_ANSWER;
                int int32_3 = Convert.ToInt32(this.Request.Form["step-count"].ToString());
                int num4 = 1;
                for (int index = 1; index <= int32_3; ++index)
                {
                    switch (Convert.ToInt32(this.Request.Form["select-theme-step-" + (object)index].ToString()))
                    {
                        case 8:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(8),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 9:
                            tbl_content_answer_steps entity3 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file1 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file2 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file2 != null)
                                {
                                    string extension = Path.GetExtension(file2.FileName);
                                    if (file1.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)index + extension);
                                        file1.SaveAs(filename);
                                        entity3.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity3.ID_CONTENT_ANSWER = answer_id;
                            entity3.STEPNO = num4;
                            entity3.ID_THEME = new int?(9);
                            entity3.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity3.ANSWER_STEPS_PART2 = "";
                            entity3.ANSWER_STEPS_PART3 = "";
                            entity3.ANSWER_STEPS_PART4 = "";
                            entity3.ANSWER_STEPS_PART5 = "";
                            entity3.ANSWER_STEPS_PART6 = "";
                            entity3.ANSWER_STEPS_PART7 = "";
                            entity3.ANSWER_STEPS_PART8 = "";
                            entity3.ANSWER_STEPS_PART9 = "";
                            entity3.ANSWER_STEPS_PART10 = "";
                            entity3.ANSWER_STEPS_IMG2 = "";
                            entity3.ANSWER_STEPS_IMG3 = "";
                            entity3.ANSWER_STEPS_IMG4 = "";
                            entity3.ANSWER_STEPS_IMG5 = "";
                            entity3.ANSWER_STEPS_IMG6 = "";
                            entity3.ANSWER_STEPS_IMG7 = "";
                            entity3.ANSWER_STEPS_IMG8 = "";
                            entity3.ANSWER_STEPS_IMG9 = "";
                            entity3.ANSWER_STEPS_IMG10 = "";
                            entity3.STATUS = "A";
                            entity3.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity3);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 10:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(10),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part3"].ToString(),
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 11:
                            tbl_content_answer_steps entity4 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file3 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file4 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file4 != null)
                                {
                                    string extension = Path.GetExtension(file4.FileName);
                                    if (file3.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)index + extension);
                                        file3.SaveAs(filename);
                                        entity4.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity4.ID_CONTENT_ANSWER = answer_id;
                            entity4.STEPNO = num4;
                            entity4.ID_THEME = new int?(11);
                            entity4.ANSWER_STEPS_PART1 = "";
                            entity4.ANSWER_STEPS_PART2 = "";
                            entity4.ANSWER_STEPS_PART3 = "";
                            entity4.ANSWER_STEPS_PART4 = "";
                            entity4.ANSWER_STEPS_PART5 = "";
                            entity4.ANSWER_STEPS_PART6 = "";
                            entity4.ANSWER_STEPS_PART7 = "";
                            entity4.ANSWER_STEPS_PART8 = "";
                            entity4.ANSWER_STEPS_PART9 = "";
                            entity4.ANSWER_STEPS_PART10 = "";
                            entity4.ANSWER_STEPS_IMG2 = "";
                            entity4.ANSWER_STEPS_IMG3 = "";
                            entity4.ANSWER_STEPS_IMG4 = "";
                            entity4.ANSWER_STEPS_IMG5 = "";
                            entity4.ANSWER_STEPS_IMG6 = "";
                            entity4.ANSWER_STEPS_IMG7 = "";
                            entity4.ANSWER_STEPS_IMG8 = "";
                            entity4.ANSWER_STEPS_IMG9 = "";
                            entity4.ANSWER_STEPS_IMG10 = "";
                            entity4.STATUS = "A";
                            entity4.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity4);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 12:
                            tbl_content_answer_steps entity5 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file5 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file6 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file6 != null && file5.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file6.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file5.SaveAs(filename);
                                    entity5.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file7 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file8 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file8 != null && file7.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file8.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file7.SaveAs(filename);
                                    entity5.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity5.ID_CONTENT_ANSWER = answer_id;
                            entity5.STEPNO = num4;
                            entity5.ID_THEME = new int?(12);
                            entity5.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity5.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity5.ANSWER_STEPS_PART3 = "";
                            entity5.ANSWER_STEPS_PART4 = "";
                            entity5.ANSWER_STEPS_PART5 = "";
                            entity5.ANSWER_STEPS_PART6 = "";
                            entity5.ANSWER_STEPS_PART7 = "";
                            entity5.ANSWER_STEPS_PART8 = "";
                            entity5.ANSWER_STEPS_PART9 = "";
                            entity5.ANSWER_STEPS_PART10 = "";
                            entity5.ANSWER_STEPS_IMG3 = "";
                            entity5.ANSWER_STEPS_IMG4 = "";
                            entity5.ANSWER_STEPS_IMG5 = "";
                            entity5.ANSWER_STEPS_IMG6 = "";
                            entity5.ANSWER_STEPS_IMG7 = "";
                            entity5.ANSWER_STEPS_IMG8 = "";
                            entity5.ANSWER_STEPS_IMG9 = "";
                            entity5.ANSWER_STEPS_IMG10 = "";
                            entity5.STATUS = "A";
                            entity5.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity5);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 13:
                            tbl_content_answer_steps entity6 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file9 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file10 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file10 != null && file9.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file10.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file9.SaveAs(filename);
                                    entity6.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file11 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file12 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file12 != null && file11.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file12.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file11.SaveAs(filename);
                                    entity6.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file13 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file14 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file14 != null && file13.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file14.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file13.SaveAs(filename);
                                    entity6.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file15 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file16 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file16 != null && file15.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file16.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file15.SaveAs(filename);
                                    entity6.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity6.ID_CONTENT_ANSWER = answer_id;
                            entity6.STEPNO = num4;
                            entity6.ID_THEME = new int?(13);
                            entity6.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity6.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity6.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity6.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity6.ANSWER_STEPS_PART5 = "";
                            entity6.ANSWER_STEPS_PART6 = "";
                            entity6.ANSWER_STEPS_PART7 = "";
                            entity6.ANSWER_STEPS_PART8 = "";
                            entity6.ANSWER_STEPS_PART9 = "";
                            entity6.ANSWER_STEPS_PART10 = "";
                            entity6.ANSWER_STEPS_IMG5 = "";
                            entity6.ANSWER_STEPS_IMG6 = "";
                            entity6.ANSWER_STEPS_IMG7 = "";
                            entity6.ANSWER_STEPS_IMG8 = "";
                            entity6.ANSWER_STEPS_IMG9 = "";
                            entity6.ANSWER_STEPS_IMG10 = "";
                            entity6.STATUS = "A";
                            entity6.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity6);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 14:
                            tbl_content_answer_steps entity7 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file17 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file18 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file18 != null && file17.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file18.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file17.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file19 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file20 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file20 != null && file19.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file20.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file19.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file21 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file22 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file22 != null && file21.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file22.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file21.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file23 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file24 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file24 != null && file23.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file24.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file23.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file25 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                HttpPostedFile file26 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                if (file26 != null && file25.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file26.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)index + extension);
                                    file25.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file27 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                HttpPostedFile file28 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                if (file28 != null && file27.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file28.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)index + extension);
                                    file27.SaveAs(filename);
                                    entity7.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity7.ID_CONTENT_ANSWER = answer_id;
                            entity7.STEPNO = num4;
                            entity7.ID_THEME = new int?(14);
                            entity7.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity7.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity7.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity7.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity7.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)index + "-part-5"].ToString();
                            entity7.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)index + "-part-6"].ToString();
                            entity7.ANSWER_STEPS_PART7 = "";
                            entity7.ANSWER_STEPS_PART8 = "";
                            entity7.ANSWER_STEPS_PART9 = "";
                            entity7.ANSWER_STEPS_PART10 = "";
                            entity7.ANSWER_STEPS_IMG7 = "";
                            entity7.ANSWER_STEPS_IMG8 = "";
                            entity7.ANSWER_STEPS_IMG9 = "";
                            entity7.ANSWER_STEPS_IMG10 = "";
                            entity7.STATUS = "A";
                            entity7.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity7);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 15:
                            tbl_content_answer_steps entity8 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file29 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file30 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file30 != null && file29.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file30.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file29.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file31 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file32 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file32 != null && file31.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file32.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file31.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file33 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file34 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file34 != null && file33.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file34.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file33.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file35 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file36 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file36 != null && file35.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file36.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file35.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file37 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                HttpPostedFile file38 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                if (file38 != null && file37.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file38.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)index + extension);
                                    file37.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file39 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                HttpPostedFile file40 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                if (file40 != null && file39.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file40.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)index + extension);
                                    file39.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file41 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-7"];
                                HttpPostedFile file42 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-7"];
                                if (file42 != null && file41.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file42.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-7-" + (object)content_id + (object)index + extension);
                                    file41.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG7 = "step-7-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file43 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-8"];
                                HttpPostedFile file44 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-8"];
                                if (file44 != null && file43.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file44.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-8-" + (object)content_id + (object)index + extension);
                                    file43.SaveAs(filename);
                                    entity8.ANSWER_STEPS_IMG8 = "step-8-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity8.ID_CONTENT_ANSWER = answer_id;
                            entity8.STEPNO = num4;
                            entity8.ID_THEME = new int?(15);
                            entity8.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity8.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity8.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity8.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity8.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)index + "-part-5"].ToString();
                            entity8.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)index + "-part-6"].ToString();
                            entity8.ANSWER_STEPS_PART7 = this.Request.Form["step-" + (object)index + "-part-7"].ToString();
                            entity8.ANSWER_STEPS_PART8 = this.Request.Form["step-" + (object)index + "-part-8"].ToString();
                            entity8.ANSWER_STEPS_PART9 = "";
                            entity8.ANSWER_STEPS_PART10 = "";
                            entity8.ANSWER_STEPS_IMG9 = "";
                            entity8.ANSWER_STEPS_IMG10 = "";
                            entity8.STATUS = "A";
                            entity8.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity8);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 16:
                            tbl_content_answer_steps entity9 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file45 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file46 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file46 != null && file45.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file46.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file45.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file47 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file48 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file48 != null && file47.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file48.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file47.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file49 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file50 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file50 != null && file49.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file50.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file49.SaveAs(filename);
                                    entity9.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity9.ID_CONTENT_ANSWER = answer_id;
                            entity9.STEPNO = num4;
                            entity9.ID_THEME = new int?(16);
                            entity9.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity9.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity9.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity9.ANSWER_STEPS_PART4 = "";
                            entity9.ANSWER_STEPS_PART5 = "";
                            entity9.ANSWER_STEPS_PART6 = "";
                            entity9.ANSWER_STEPS_PART7 = "";
                            entity9.ANSWER_STEPS_PART8 = "";
                            entity9.ANSWER_STEPS_PART9 = "";
                            entity9.ANSWER_STEPS_PART10 = "";
                            entity9.ANSWER_STEPS_IMG4 = "";
                            entity9.ANSWER_STEPS_IMG5 = "";
                            entity9.ANSWER_STEPS_IMG6 = "";
                            entity9.ANSWER_STEPS_IMG7 = "";
                            entity9.ANSWER_STEPS_IMG8 = "";
                            entity9.ANSWER_STEPS_IMG9 = "";
                            entity9.ANSWER_STEPS_IMG10 = "";
                            entity9.STATUS = "A";
                            entity9.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity9);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 17:
                            tbl_content_answer_steps entity10 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file51 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file52 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file52 != null && file51.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file52.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                    file51.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file53 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file54 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file54 != null && file53.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file54.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                    file53.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file55 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                HttpPostedFile file56 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-3"];
                                if (file56 != null && file55.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file56.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-3-" + (object)content_id + (object)index + extension);
                                    file55.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG3 = "step-3-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file57 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                HttpPostedFile file58 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-4"];
                                if (file58 != null && file57.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file58.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-4-" + (object)content_id + (object)index + extension);
                                    file57.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG4 = "step-4-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file59 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                HttpPostedFile file60 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-5"];
                                if (file60 != null && file59.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file60.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-5-" + (object)content_id + (object)index + extension);
                                    file59.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG5 = "step-5-" + (object)content_id + (object)index + extension;
                                }
                                HttpPostedFile file61 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                HttpPostedFile file62 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-6"];
                                if (file62 != null && file61.ContentLength > 0)
                                {
                                    string extension = Path.GetExtension(file62.FileName);
                                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-6-" + (object)content_id + (object)index + extension);
                                    file61.SaveAs(filename);
                                    entity10.ANSWER_STEPS_IMG6 = "step-6-" + (object)content_id + (object)index + extension;
                                }
                            }
                            entity10.ID_CONTENT_ANSWER = answer_id;
                            entity10.STEPNO = num4;
                            entity10.ID_THEME = new int?(17);
                            entity10.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part-1"].ToString();
                            entity10.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part-2"].ToString();
                            entity10.ANSWER_STEPS_PART3 = this.Request.Form["step-" + (object)index + "-part-3"].ToString();
                            entity10.ANSWER_STEPS_PART4 = this.Request.Form["step-" + (object)index + "-part-4"].ToString();
                            entity10.ANSWER_STEPS_PART5 = this.Request.Form["step-" + (object)index + "-part-5"].ToString();
                            entity10.ANSWER_STEPS_PART6 = this.Request.Form["step-" + (object)index + "-part-6"].ToString();
                            entity10.ANSWER_STEPS_PART7 = "";
                            entity10.ANSWER_STEPS_PART8 = "";
                            entity10.ANSWER_STEPS_PART9 = "";
                            entity10.ANSWER_STEPS_PART10 = "";
                            entity10.ANSWER_STEPS_IMG7 = "";
                            entity10.ANSWER_STEPS_IMG8 = "";
                            entity10.ANSWER_STEPS_IMG9 = "";
                            entity10.ANSWER_STEPS_IMG10 = "";
                            entity10.STATUS = "A";
                            entity10.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity10);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 18:
                            tbl_content_answer_steps entity11 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file63 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file64 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file64 != null)
                                {
                                    string extension = Path.GetExtension(file64.FileName);
                                    if (file63.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                        file63.SaveAs(filename);
                                        entity11.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity11.ID_CONTENT_ANSWER = answer_id;
                            entity11.STEPNO = num4;
                            entity11.ID_THEME = new int?(18);
                            entity11.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity11.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString();
                            entity11.ANSWER_STEPS_PART3 = "";
                            entity11.ANSWER_STEPS_PART4 = "";
                            entity11.ANSWER_STEPS_PART5 = "";
                            entity11.ANSWER_STEPS_PART6 = "";
                            entity11.ANSWER_STEPS_PART7 = "";
                            entity11.ANSWER_STEPS_PART8 = "";
                            entity11.ANSWER_STEPS_PART9 = "";
                            entity11.ANSWER_STEPS_PART10 = "";
                            entity11.ANSWER_STEPS_IMG2 = "";
                            entity11.ANSWER_STEPS_IMG3 = "";
                            entity11.ANSWER_STEPS_IMG4 = "";
                            entity11.ANSWER_STEPS_IMG5 = "";
                            entity11.ANSWER_STEPS_IMG6 = "";
                            entity11.ANSWER_STEPS_IMG7 = "";
                            entity11.ANSWER_STEPS_IMG8 = "";
                            entity11.ANSWER_STEPS_IMG9 = "";
                            entity11.ANSWER_STEPS_IMG10 = "";
                            entity11.STATUS = "A";
                            entity11.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity11);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 19:
                            tbl_content_answer_steps entity12 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file65 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file66 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file66 != null)
                                {
                                    string extension = Path.GetExtension(file66.FileName);
                                    if (file65.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                        file65.SaveAs(filename);
                                        entity12.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                    }
                                }
                                HttpPostedFile file67 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                HttpPostedFile file68 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-2"];
                                if (file68 != null)
                                {
                                    string extension = Path.GetExtension(file68.FileName);
                                    if (file67.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-2-" + (object)content_id + (object)index + extension);
                                        file67.SaveAs(filename);
                                        entity12.ANSWER_STEPS_IMG2 = "step-2-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity12.ID_CONTENT_ANSWER = answer_id;
                            entity12.STEPNO = num4;
                            entity12.ID_THEME = new int?(19);
                            entity12.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity12.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString();
                            entity12.ANSWER_STEPS_PART3 = "";
                            entity12.ANSWER_STEPS_PART4 = "";
                            entity12.ANSWER_STEPS_PART5 = "";
                            entity12.ANSWER_STEPS_PART6 = "";
                            entity12.ANSWER_STEPS_PART7 = "";
                            entity12.ANSWER_STEPS_PART8 = "";
                            entity12.ANSWER_STEPS_PART9 = "";
                            entity12.ANSWER_STEPS_PART10 = "";
                            entity12.ANSWER_STEPS_IMG3 = "";
                            entity12.ANSWER_STEPS_IMG4 = "";
                            entity12.ANSWER_STEPS_IMG5 = "";
                            entity12.ANSWER_STEPS_IMG6 = "";
                            entity12.ANSWER_STEPS_IMG7 = "";
                            entity12.ANSWER_STEPS_IMG8 = "";
                            entity12.ANSWER_STEPS_IMG9 = "";
                            entity12.ANSWER_STEPS_IMG10 = "";
                            entity12.STATUS = "A";
                            entity12.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity12);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 20:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(20),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 21:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(21),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 22:
                            tbl_content_answer_steps entity13 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file69 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                HttpPostedFile file70 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn-1"];
                                if (file70 != null)
                                {
                                    string extension = Path.GetExtension(file70.FileName);
                                    if (file69.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-1-" + (object)content_id + (object)index + extension);
                                        file69.SaveAs(filename);
                                        entity13.ANSWER_STEPS_IMG1 = "step-1-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity13.ID_CONTENT_ANSWER = answer_id;
                            entity13.STEPNO = num4;
                            entity13.ID_THEME = new int?(22);
                            entity13.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity13.ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString();
                            entity13.ANSWER_STEPS_PART3 = "";
                            entity13.ANSWER_STEPS_PART4 = "";
                            entity13.ANSWER_STEPS_PART5 = "";
                            entity13.ANSWER_STEPS_PART6 = "";
                            entity13.ANSWER_STEPS_PART7 = "";
                            entity13.ANSWER_STEPS_PART8 = "";
                            entity13.ANSWER_STEPS_PART9 = "";
                            entity13.ANSWER_STEPS_PART10 = "";
                            entity13.ANSWER_STEPS_IMG2 = "";
                            entity13.ANSWER_STEPS_IMG3 = "";
                            entity13.ANSWER_STEPS_IMG4 = "";
                            entity13.ANSWER_STEPS_IMG5 = "";
                            entity13.ANSWER_STEPS_IMG6 = "";
                            entity13.ANSWER_STEPS_IMG7 = "";
                            entity13.ANSWER_STEPS_IMG8 = "";
                            entity13.ANSWER_STEPS_IMG9 = "";
                            entity13.ANSWER_STEPS_IMG10 = "";
                            entity13.STATUS = "A";
                            entity13.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity13);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 23:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(23),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = this.Request.Form["step-" + (object)index + "-part2"].ToString(),
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 24:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(24),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = "",
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 25:
                            tbl_content_answer_steps entity14 = new tbl_content_answer_steps();
                            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                            {
                                HttpPostedFile file71 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                HttpPostedFile file72 = System.Web.HttpContext.Current.Request.Files["step-" + (object)index + "-btn"];
                                if (file72 != null)
                                {
                                    string extension = Path.GetExtension(file72.FileName);
                                    if (file71.ContentLength > 0)
                                    {
                                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/")))
                                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"));
                                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)content_id + "/"), "step-" + (object)content_id + (object)index + extension);
                                        file71.SaveAs(filename);
                                        entity14.ANSWER_STEPS_IMG1 = "step-" + (object)content_id + (object)index + extension;
                                    }
                                }
                            }
                            entity14.ID_CONTENT_ANSWER = answer_id;
                            entity14.STEPNO = num4;
                            entity14.ID_THEME = new int?(25);
                            entity14.ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString();
                            entity14.ANSWER_STEPS_PART2 = "";
                            entity14.ANSWER_STEPS_PART3 = "";
                            entity14.ANSWER_STEPS_PART4 = "";
                            entity14.ANSWER_STEPS_PART5 = "";
                            entity14.ANSWER_STEPS_PART6 = "";
                            entity14.ANSWER_STEPS_PART7 = "";
                            entity14.ANSWER_STEPS_PART8 = "";
                            entity14.ANSWER_STEPS_PART9 = "";
                            entity14.ANSWER_STEPS_PART10 = "";
                            entity14.ANSWER_STEPS_IMG2 = "";
                            entity14.ANSWER_STEPS_IMG3 = "";
                            entity14.ANSWER_STEPS_IMG4 = "";
                            entity14.ANSWER_STEPS_IMG5 = "";
                            entity14.ANSWER_STEPS_IMG6 = "";
                            entity14.ANSWER_STEPS_IMG7 = "";
                            entity14.ANSWER_STEPS_IMG8 = "";
                            entity14.ANSWER_STEPS_IMG9 = "";
                            entity14.ANSWER_STEPS_IMG10 = "";
                            entity14.STATUS = "A";
                            entity14.UPDATEDDATETIME = DateTime.Now;
                            this.db.tbl_content_answer_steps.Add(entity14);
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 26:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ANSWER_STEPS_IMG1 = this.Request.Form["step-" + (object)index + "-image"].ToString(),
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(26),
                                ANSWER_STEPS_PART1 = this.Request.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = "",
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                        case 27:
                            this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                            {
                                ID_CONTENT_ANSWER = answer_id,
                                STEPNO = num4,
                                ID_THEME = new int?(27),
                                ANSWER_STEPS_PART1 = this.Request.Unvalidated.Form["step-" + (object)index + "-part1"].ToString(),
                                ANSWER_STEPS_PART2 = "",
                                ANSWER_STEPS_PART3 = "",
                                ANSWER_STEPS_PART4 = "",
                                ANSWER_STEPS_PART5 = "",
                                ANSWER_STEPS_PART6 = "",
                                ANSWER_STEPS_PART7 = "",
                                ANSWER_STEPS_PART8 = "",
                                ANSWER_STEPS_PART9 = "",
                                ANSWER_STEPS_PART10 = "",
                                ANSWER_STEPS_IMG1 = "",
                                ANSWER_STEPS_IMG2 = "",
                                ANSWER_STEPS_IMG3 = "",
                                ANSWER_STEPS_IMG4 = "",
                                ANSWER_STEPS_IMG5 = "",
                                ANSWER_STEPS_IMG6 = "",
                                ANSWER_STEPS_IMG7 = "",
                                ANSWER_STEPS_IMG8 = "",
                                ANSWER_STEPS_IMG9 = "",
                                ANSWER_STEPS_IMG10 = "",
                                STATUS = "A",
                                UPDATEDDATETIME = DateTime.Now
                            });
                            this.db.SaveChanges();
                            ++num4;
                            break;
                    }
                }
                string str3 = this.Request.Form["t-metadata"].ToString();
                char[] chArray = new char[1] { ',' };
                foreach (string str4 in str3.Split(chArray))
                {
                    string str = str4;
                    if (this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.CONTENT_METADATA.ToLower().Contains(str.ToLower()) && t.ID_CONTENT_ANSWER == answer_id)).FirstOrDefault<tbl_content_metadata>() == null && !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
                    {
                        this.db.tbl_content_metadata.Add(new tbl_content_metadata()
                        {
                            CONTENT_METADATA = str,
                            CONTENT_METADATA_COUNTER = 0,
                            ID_CONTENT_ANSWER = answer_id,
                            STATUS = "A",
                            UPDATEDTIME = DateTime.Now
                        });
                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                if (cid == 0)
                    return (ActionResult)this.RedirectToAction("display_content", "dashboard");
                if (cid != 0)
                {
                    new addCMS_CategoryModel().delete_content(cid);
                    return (ActionResult)this.RedirectToAction("ContentError", "contentDashboard");
                }
            }
            return (ActionResult)this.RedirectToAction("display_content", "dashboard");
        }

        [HttpPost]
        [ValidateInput(false)]
        public string createPresentationContent(ContentRichText idata)
        {
            int num1 = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int OID = Convert.ToInt32(content.id_ORGANIZATION);
            int? cmdUserType = content.USER.cmd_user_type;
            int num2 = 0;
            string str1 = (cmdUserType.GetValueOrDefault() == num2 ? (cmdUserType.HasValue ? 1 : 0) : 0) == 0 ? "P" : "P";
            if (num1 > 0)
                this.db.tbl_content.Find(new object[1]
                {
          (object) num1
                });
            int num3 = 15;
            List<string> stringList = new List<string>();
            int selectLevel = idata.select_level;
            DateTime now = DateTime.Now;
            int content_id = 0;
            int answer_id = 0;
            tbl_content entity1 = new tbl_content();
            tbl_content_answer entity2 = new tbl_content_answer();
            try
            {
                entity1.CONTENT_HEADER = idata.t_header;
                entity1.CONTENT_QUESTION = idata.t_question;
                entity1.CONTENT_TITLE = idata.t_question;
                entity1.ID_THEME = num3;
                entity1.ID_CONTENT_LEVEL = selectLevel;
                entity1.ID_USER = Convert.ToInt32(content.ID_USER);
                entity1.UPDATED_DATE_TIME = DateTime.Now;
                entity1.CONTENT_COUNTER = 0;
                entity1.EXPIRY_DATE = new DateTime?(idata.content_expiry);
                entity1.STATUS = str1;
                entity1.CONTENT_OWNER = OID;
                entity1.IS_PRIMARY = num1 != 0 ? 0 : 1;
                this.db.tbl_content.Add(entity1);
                this.db.SaveChanges();
                content_id = entity1.ID_CONTENT;
                if (content_id > 0)
                {
                    string categoryList = idata.category_list;
                    char[] chArray = new char[1] { '|' };
                    foreach (string str2 in categoryList.Split(chArray))
                    {
                        int citd = Convert.ToInt32(str2);
                        if (citd > 0)
                        {
                            if (this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_category == citd && t.id_content == content_id && t.id_organization == OID)).FirstOrDefault<tbl_content_organization_mapping>() == null)
                            {
                                this.db.tbl_content_organization_mapping.Add(new tbl_content_organization_mapping()
                                {
                                    id_organization = Convert.ToInt32(content.id_ORGANIZATION),
                                    id_category = citd,
                                    id_content = entity1.ID_CONTENT,
                                    status = "A",
                                    updated_date_time = new DateTime?(DateTime.Now)
                                });
                                this.db.SaveChanges();
                            }
                        }
                    }
                }
                if (entity1.ID_CONTENT > 0)
                {
                    entity2.ID_CONTENT = content_id;
                    entity2.CONTENT_ANSWER_TITLE = "";
                    entity2.CONTENT_ANSWER_COUNTER = 0;
                    entity2.CONTENT_ANSWER_HEADER = entity1.CONTENT_HEADER;
                    entity2.CONTENT_ANSWER1 = idata.content_answer;
                    entity2.CONTENT_ANSWER2 = "";
                    entity2.CONTENT_ANSWER3 = "";
                    entity2.CONTENT_ANSWER4 = "";
                    entity2.CONTENT_ANSWER5 = "";
                    entity2.CONTENT_ANSWER6 = "";
                    entity2.CONTENT_ANSWER7 = "";
                    entity2.CONTENT_ANSWER8 = "";
                    entity2.CONTENT_ANSWER9 = "";
                    entity2.CONTENT_ANSWER10 = "";
                    entity2.CONTENT_ANSWER_IMG1 = "";
                    entity2.CONTENT_ANSWER_IMG2 = "";
                    entity2.CONTENT_ANSWER_IMG3 = "";
                    entity2.CONTENT_ANSWER_IMG4 = "";
                    entity2.CONTENT_ANSWER_IMG5 = "";
                    entity2.CONTENT_ANSWER_IMG6 = "";
                    entity2.CONTENT_ANSWER_IMG7 = "";
                    entity2.CONTENT_ANSWER_IMG8 = "";
                    entity2.CONTENT_ANSWER_IMG9 = "";
                    entity2.CONTENT_ANSWER_IMG10 = "";
                    entity2.STATUS = "A";
                    entity2.UPDATEDTIME = DateTime.Now;
                    this.db.tbl_content_answer.Add(entity2);
                    this.db.SaveChanges();
                }
                answer_id = entity2.ID_CONTENT_ANSWER;
                string tMetadata = idata.t_metadata;
                char[] chArray1 = new char[1] { ',' };
                foreach (string str3 in tMetadata.Split(chArray1))
                {
                    string str = str3;
                    if (this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.CONTENT_METADATA.ToLower().Contains(str.ToLower()) && t.ID_CONTENT_ANSWER == answer_id)).FirstOrDefault<tbl_content_metadata>() == null && !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
                    {
                        this.db.tbl_content_metadata.Add(new tbl_content_metadata()
                        {
                            CONTENT_METADATA = str,
                            CONTENT_METADATA_COUNTER = 0,
                            ID_CONTENT_ANSWER = answer_id,
                            STATUS = "A",
                            UPDATEDTIME = DateTime.Now
                        });
                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        [HttpPost]
        public string updateRichTextContent(ContentRichText idata)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            new addCMS_CategoryModel().updat_content(idata, int32_1, int32_2);
            return "";
        }

        public ActionResult ContentError() => (ActionResult)this.View();

        public ActionResult contentCreationTrial()
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (this.TempData["LINKTYPE"] != null)
            {
                int id = Convert.ToInt32(this.TempData["LINKTYPE"]);
                this.ViewData["LINKTYPE"] = (object)id;
                tbl_content tblContent = this.db.tbl_content.Find(new object[1]
                {
          (object) id
                });
                this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == id && t.id_organization == orgid)).FirstOrDefault<tbl_content_organization_mapping>();
                List<tbl_category> list = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
                this.ViewData["parent-content"] = (object)tblContent;
                this.ViewData["parent-category"] = (object)list;
            }
            else
            {
                this.ViewData["LINKTYPE"] = (object)0;
                this.ViewData["parent-content"] = (object)null;
                this.ViewData["parent-category"] = (object)null;
            }
            List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            List<tbl_content_level> list2 = this.db.tbl_content_level.ToList<tbl_content_level>();
            List<tbl_csst_role> list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            if (list3.Count == 0)
            {
                this.db.tbl_csst_role.Add(new tbl_csst_role()
                {
                    id_organization = new int?(orgid),
                    csst_role = "User",
                    status = "A",
                    updated_dated_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
                list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            }
            this.ViewData["select-cscc-role"] = (object)list3;
            this.ViewData["select-category"] = (object)list1;
            this.ViewData["select-level"] = (object)list2;
            return (ActionResult)this.View();
        }

        public ActionResult contentCreationRichTextTrial()
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (this.TempData["LINKTYPE"] != null)
            {
                int id = Convert.ToInt32(this.TempData["LINKTYPE"]);
                this.ViewData["LINKTYPE"] = (object)id;
                tbl_content tblContent = this.db.tbl_content.Find(new object[1]
                {
          (object) id
                });
                this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == id && t.id_organization == orgid)).FirstOrDefault<tbl_content_organization_mapping>();
                List<tbl_category> list = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
                this.ViewData["parent-content"] = (object)tblContent;
                this.ViewData["parent-category"] = (object)list;
            }
            else
            {
                this.ViewData["LINKTYPE"] = (object)0;
                this.ViewData["parent-content"] = (object)null;
                this.ViewData["parent-category"] = (object)null;
            }
            List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            List<tbl_content_level> list2 = this.db.tbl_content_level.ToList<tbl_content_level>();
            List<tbl_csst_role> list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            if (list3.Count == 0)
            {
                this.db.tbl_csst_role.Add(new tbl_csst_role()
                {
                    id_organization = new int?(orgid),
                    csst_role = "User",
                    status = "A",
                    updated_dated_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
                list3 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            }
            this.ViewData["select-cscc-role"] = (object)list3;
            this.ViewData["select-category"] = (object)list1;
            this.ViewData["select-level"] = (object)list2;
            return (ActionResult)this.View();
        }

        public string getPrograms()
        {
            List<tbl_category> list = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " and category_type=0 and status='A'").ToList<tbl_category>();
            string programs = "";
            foreach (tbl_category tblCategory in list)
                programs = "<option value=" + (object)tblCategory.ID_CATEGORY + ">" + tblCategory.CATEGORYNAME + "</option>";
            return programs;
        }

        public ActionResult NotificationAttachCon(int id)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            this.ViewData["not"] = (object)this.db.tbl_notification.SqlQuery("select * from tbl_notification where id_notification=" + (object)id + " AND status='A'").FirstOrDefault<tbl_notification>();
            tbl_notification_pre_config preConfigNot = new BuisinessLogic().getPreConfigNot("select * from tbl_notification_pre_config where id_notification=" + (object)id);
            this.ViewData["RoleList"] = (object)this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object)int32 + " and status='A'").ToList<tbl_csst_role>();
            this.ViewData["location"] = (object)new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object)int32 + " order by LOCATION ");
            this.ViewData["function"] = (object)new contentDashboardModel().getUserFunction(" select distinct case when user_function is null then UPPER(user_function) else UPPER(user_function) end as user_function " + " from tbl_user where ID_ORGANIZATION=" + (object)int32 + " and user_function is not null order by user_function ");
            this.ViewData["conf"] = (object)preConfigNot;
            return (ActionResult)this.View();
        }

        public string NotificationAttachConAction()
        {
            int id_config = 0;
            int[] source = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
            DateTime? nullable1 = new DateTime?();
            DateTime? nullable2 = new DateTime?();
            nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
            nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
            int ids = Convert.ToInt32(this.Request.Form["id_content"].ToString());
            int int32_1 = Convert.ToInt32(this.Request.Form["category"].ToString());
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            int int32_3 = Convert.ToInt32(this.Request.Form["notification_req"].ToString());
            int nids = 0;
            if (int32_3 == 1)
                nids = Convert.ToInt32(this.Request.Form["notification_id"].ToString());
            tbl_notification note = (tbl_notification)null;
            if (int32_3 == 1)
                note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            for (int index = 0; index < ((IEnumerable<int>)source).Count<int>(); ++index)
            {
                int temp = source[index];
                tbl_content_user_assisgnment contentUserAssisgnment1 = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)temp)).FirstOrDefault<tbl_content_user_assisgnment>();
                tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(temp);
                if (contentUserAssisgnment1 == null)
                {
                    tbl_content_user_assisgnment contentUserAssisgnment2 = new tbl_content_user_assisgnment();
                    contentUserAssisgnment2.id_user = new int?(source[index]);
                    contentUserAssisgnment2.id_organization = new int?(orgid);
                    contentUserAssisgnment2.id_content = new int?(ids);
                    contentUserAssisgnment2.id_category = new int?(int32_1);
                    contentUserAssisgnment2.status = "A";
                    contentUserAssisgnment2.start_date = nullable1;
                    contentUserAssisgnment2.expiry_date = nullable2;
                    contentUserAssisgnment2.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment2);
                    this.db.SaveChanges();
                    if (contentUserAssisgnment2.id_content_user_assisgnment > 0 && note != null)
                    {
                        tbl_notification_config entity = new tbl_notification_config();
                        entity.id_assessment = new int?(0);
                        entity.id_category = new int?(0);
                        entity.id_content = new int?(ids);
                        entity.id_notification = new int?(nids);
                        entity.start_date = nullable1;
                        entity.end_date = nullable2;
                        entity.id_user = new int?(source[index]);
                        entity.notification_key = note.notification_key;
                        entity.notification_action_type = "CON";
                        entity.read_timestamp = new DateTime?(DateTime.Now);
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        entity.user_type = "1";
                        entity.status = "P";
                        entity.id_creater = new int?(int32_2);
                        this.db.tbl_notification_config.Add(entity);
                        this.db.SaveChanges();
                        id_config = entity.id_notification_config;
                    }
                    if (mailId.EMAIL != "" && note != null)
                        new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment2, orgid);
                }
                else
                {
                    contentUserAssisgnment1.start_date = nullable1;
                    contentUserAssisgnment1.expiry_date = nullable2;
                    contentUserAssisgnment1.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            this.sendContentNotification(ids.ToString());
            return "1";
        }

        public ActionResult NotificationAttachAss(int id)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            this.ViewData["not"] = (object)this.db.tbl_notification.SqlQuery("select * from tbl_notification where id_notification=" + (object)id + " AND status='A'").FirstOrDefault<tbl_notification>();
            tbl_notification_pre_config preConfigNot = new BuisinessLogic().getPreConfigNot("select * from tbl_notification_pre_config where id_notification=" + (object)id);
            this.ViewData["RoleList"] = (object)this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object)int32 + " and status='A'").ToList<tbl_csst_role>();
            this.ViewData["location"] = (object)new contentDashboardModel().getUserLocation(" select distinct case when a.CITY is null then UPPER(a.LOCATION) else UPPER(a.city) end as LOCATION " + " from tbl_profile a left join tbl_user b ON a.id_user = b.id_user where b.ID_ORGANIZATION=" + (object)int32 + " order by LOCATION ");
            this.ViewData["function"] = (object)new contentDashboardModel().getUserFunction(" select distinct case when user_function is null then UPPER(user_function) else UPPER(user_function) end as user_function " + " from tbl_user where ID_ORGANIZATION=" + (object)int32 + " and user_function is not null order by user_function ");
            this.ViewData["conf"] = (object)preConfigNot;
            return (ActionResult)this.View();
        }

        public string NotificationAttachAssAction()
        {
            int id_config = 0;
            int[] source = Array.ConvertAll<string, int>(this.Request.Form["chk_user"].ToString().Split(','), new Converter<string, int>(int.Parse));
            DateTime? nullable1 = new DateTime?();
            DateTime? nullable2 = new DateTime?();
            nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
            nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"].ToString()) ? new DateTime?(DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", (IFormatProvider)CultureInfo.InvariantCulture)) : new DateTime?();
            int ids = Convert.ToInt32(this.Request.Form["id_content"].ToString());
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_1 = Convert.ToInt32(content.ID_USER);
            int int32_2 = Convert.ToInt32(this.Request.Form["category"].ToString());
            int[] numArray = source;
            this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();
            int int32_3 = Convert.ToInt32(this.Request.Form["notification_req"].ToString());
            int nids = 0;
            if (int32_3 == 1)
                nids = Convert.ToInt32(this.Request.Form["notification_id"].ToString());
            tbl_notification note = (tbl_notification)null;
            if (int32_3 == 1)
                note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            for (int index = 0; index < ((IEnumerable<int>)source).Count<int>(); ++index)
            {
                int user_id = numArray[index];
                tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(user_id);
                tbl_assessment_user_assignment assessmentUserAssignment1 = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == (int?)ids && t.id_organization == (int?)orgid && t.id_user == (int?)user_id)).FirstOrDefault<tbl_assessment_user_assignment>();
                if (assessmentUserAssignment1 == null)
                {
                    tbl_assessment_user_assignment assessmentUserAssignment2 = new tbl_assessment_user_assignment();
                    assessmentUserAssignment2.id_user = new int?(numArray[index]);
                    assessmentUserAssignment2.id_organization = new int?(orgid);
                    assessmentUserAssignment2.id_assessment = new int?(ids);
                    assessmentUserAssignment2.id_category = new int?(int32_2);
                    assessmentUserAssignment2.status = "A";
                    assessmentUserAssignment2.start_date = nullable1;
                    assessmentUserAssignment2.expire_date = nullable2;
                    assessmentUserAssignment2.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_user_assignment.Add(assessmentUserAssignment2);
                    this.db.SaveChanges();
                    if (assessmentUserAssignment2.id_assessment_user_assignment > 0 && note != null)
                    {
                        tbl_notification_config entity = new tbl_notification_config();
                        entity.id_assessment = new int?(ids);
                        entity.id_category = new int?(0);
                        entity.id_content = new int?(0);
                        entity.id_creater = new int?(int32_1);
                        entity.id_notification = new int?(nids);
                        entity.id_user = new int?(numArray[index]);
                        entity.start_date = nullable1;
                        entity.end_date = nullable2;
                        entity.notification_key = note.notification_key;
                        entity.notification_action_type = "ASS";
                        entity.read_timestamp = new DateTime?(DateTime.Now);
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        entity.user_type = "1";
                        entity.status = "P";
                        this.db.tbl_notification_config.Add(entity);
                        this.db.SaveChanges();
                        id_config = entity.id_notification_config;
                    }
                    if (mailId.EMAIL != "" && note != null)
                        new addCMS_CategoryModel().sendmail_assessment(mailId, note, id_config, assessmentUserAssignment2, orgid);
                }
                else
                {
                    assessmentUserAssignment1.start_date = nullable1;
                    assessmentUserAssignment1.expire_date = nullable2;
                    assessmentUserAssignment1.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            this.sendAssessmentNotification(ids.ToString());
            return "1";
        }
    }
}
