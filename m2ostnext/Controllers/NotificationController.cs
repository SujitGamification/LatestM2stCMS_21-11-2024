// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.NotificationController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class NotificationController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public ActionResult Index(int flag = 0)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Notification";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            this.ViewData["notification"] = (object)this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_organization == oid)).ToList<tbl_notification>();
            this.ViewData[nameof(flag)] = (object)flag;
            return (ActionResult)this.View();
        }

        public ActionResult createNotification()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add Notification";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            DateTime? sysDate = new DateTime?(DateTime.Now);
            this.ViewData["notification"] = (object)this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_organization == oid && t.notification_type == 4 && t.status == "A" && t.end_date >= sysDate)).ToList<tbl_notification>();
            return (ActionResult)this.View();
        }

        public ActionResult add_notification()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "ADD NOTIFICATION";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(this.Request.Form["notification-type"]);
            int int32_3 = Convert.ToInt32(this.Request.Form["availablity-div"]);
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            if (int32_2 == 4)
            {
                num3 = Convert.ToInt32(this.Request.Form["reminder-timegap"]);
                num4 = Convert.ToInt32(this.Request.Form["reminder-frequency"]);
            }
            tbl_notification entity1 = new tbl_notification();
            entity1.notification_name = this.Request.Form["notification-title"];
            entity1.notification_description = this.Request.Form["notification-desc"];
            entity1.notification_message = this.Request.Form["notification-message"];
            if (int32_2 == 1)
            {
                num1 = Convert.ToInt32(this.Request.Form["gen_notification_ty"]);
                entity1.notification_type = num1 != 1 ? Convert.ToInt32(this.Request.Form["gen_notification_type"]) : int32_2;
            }
            else
                entity1.notification_type = int32_2;
            entity1.notification_action_type = "NA";
            entity1.created_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["notification-created"].ToString()));
            entity1.start_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["notification-started"].ToString()));
            entity1.end_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["notification-ended"].ToString()));
            entity1.reminder_flag = num2;
            entity1.reminder_frequency = num4;
            entity1.reminder_time = num3;
            entity1.id_organization = int32_1;
            entity1.notification_key = this.GenerateTransaction();
            entity1.status = int32_3 != 1 ? "N" : "A";
            entity1.updated_date_time = new DateTime?(DateTime.Now);
            this.db.tbl_notification.Add(entity1);
            this.db.SaveChanges();
            int idNotification = entity1.id_notification;
            if (int32_2 == 4)
            {
                int int32_4 = Convert.ToInt32(this.Request.Form["setup-type"]);
                int num5;
                int num6;
                int num7;
                int num8;
                if (int32_4 == 1)
                {
                    num5 = 0;
                    num6 = 0;
                    num7 = 0;
                    num8 = 0;
                }
                else
                {
                    num5 = Convert.ToInt32(this.Request.Form["select-DH"]);
                    num6 = Convert.ToInt32(this.Request.Form["select-YH"]);
                    num7 = Convert.ToInt32(this.Request.Form["select-YM"]);
                    num8 = Convert.ToInt32(this.Request.Form["select-TM"]);
                }
                this.db.tbl_reminder_notification_config.Add(new tbl_reminder_notification_config()
                {
                    id_notification = new int?(entity1.id_notification),
                    reminder_type = new int?(int32_4),
                    DH = new int?(num5),
                    YH = new int?(num6),
                    YM = new int?(num7),
                    TM = new int?(num8),
                    status = "A",
                    updated_date_time = new DateTime?(DateTime.Now)
                });
                this.db.SaveChanges();
            }
            else if (Convert.ToInt32(this.Request.Form["reminder-flag"]) == 1)
            {
                tbl_notification_reminder entity2 = new tbl_notification_reminder();
                entity2.id_notification = new int?(entity1.id_notification);
                int int32_5 = Convert.ToInt32(this.Request.Form["reminder-notification"]);
                entity2.id_reminder_notification = new int?(int32_5);
                entity2.reminder_frequency = new int?(entity1.reminder_frequency);
                entity2.reminder_timeout = new int?(entity1.reminder_time);
                entity2.status = "A";
                entity2.updated_date_time = new DateTime?(DateTime.Now);
                this.db.tbl_notification_reminder.Add(entity2);
                this.db.SaveChanges();
            }
            if (int32_2 == 1 && num1 == 2)
            {
                if (Convert.ToInt32(this.Request.Form["gen_notification_type"]) == 5)
                    new addCMS_CategoryModel().configcon(new tbl_notification_pre_config()
                    {
                        id_assessment = new int?(0),
                        id_category = new int?(Convert.ToInt32(this.Request.Form["load_cat"])),
                        id_content = new int?(Convert.ToInt32(this.Request.Form["qtn"])),
                        id_notification = new int?(idNotification),
                        start_date = entity1.start_date,
                        end_date = entity1.end_date,
                        notification_key = this.GenerateTransaction(),
                        notification_action_type = "GENCON",
                        read_timestamp = new DateTime?(DateTime.Now),
                        updated_date_time = new DateTime?(DateTime.Now),
                        user_type = "1",
                        status = "P",
                        id_creater = new int?(Convert.ToInt32(content.ID_USER))
                    });
                else if (Convert.ToInt32(this.Request.Form["gen_notification_type"]) == 6)
                    new addCMS_CategoryModel().configcon(new tbl_notification_pre_config()
                    {
                        id_assessment = new int?(0),
                        id_category = new int?(0),
                        id_content = new int?(Convert.ToInt32(this.Request.Form["load_Program"])),
                        id_notification = new int?(idNotification),
                        start_date = entity1.start_date,
                        end_date = entity1.end_date,
                        notification_key = this.GenerateTransaction(),
                        notification_action_type = "GENPRO",
                        read_timestamp = new DateTime?(DateTime.Now),
                        updated_date_time = new DateTime?(DateTime.Now),
                        user_type = "1",
                        status = "P",
                        id_creater = new int?(Convert.ToInt32(content.ID_USER))
                    });
                else if (Convert.ToInt32(this.Request.Form["gen_notification_type"]) == 7)
                    new addCMS_CategoryModel().configcon(new tbl_notification_pre_config()
                    {
                        id_assessment = new int?(Convert.ToInt32(this.Request.Form["ass"])),
                        id_category = new int?(Convert.ToInt32(this.Request.Form["load_cat"])),
                        id_content = new int?(0),
                        id_notification = new int?(idNotification),
                        start_date = entity1.start_date,
                        end_date = entity1.end_date,
                        notification_key = this.GenerateTransaction(),
                        notification_action_type = "GENASS",
                        read_timestamp = new DateTime?(DateTime.Now),
                        updated_date_time = new DateTime?(DateTime.Now),
                        user_type = "1",
                        status = "P",
                        id_creater = new int?(Convert.ToInt32(content.ID_USER))
                    });
            }
            return (ActionResult)this.RedirectToAction("Index", (object)new
            {
                flag = 1
            });
        }

        public string GenerateTransaction()
        {
            int num1 = 999999;
            int num2 = 100000;
            int num3 = new Random().Next(1, 13);
            Math.Round((double)(num3 * (num1 - num2 + 1) + num2));
            string str = DateTime.Now.ToString("yyyyMMddHHmmss");
            return "NTF" + (object)num3 + str;
        }

        public ActionResult editNotification(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_notification current = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == ids)).FirstOrDefault<tbl_notification>();
            this.ViewData["notification"] = (object)current;
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            DateTime? sysDate = new DateTime?(DateTime.Now);
            this.ViewData["notificationList"] = (object)this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_organization == oid && t.notification_type == 4 && t.status == "A" && t.end_date > sysDate)).ToList<tbl_notification>();
            int num = 0;
            tbl_reminder_notification_config notificationConfig = new tbl_reminder_notification_config();
            tbl_notification_pre_config notificationPreConfig = new tbl_notification_pre_config();
            if (current.notification_type == 4)
            {
                tbl_notification_reminder notificationReminder = this.db.tbl_notification_reminder.Where<tbl_notification_reminder>((Expression<Func<tbl_notification_reminder, bool>>)(t => t.id_notification == (int?)current.id_notification)).FirstOrDefault<tbl_notification_reminder>();
                if (notificationReminder != null)
                    num = Convert.ToInt32((object)notificationReminder.id_notification);
                notificationConfig = this.db.tbl_reminder_notification_config.Where<tbl_reminder_notification_config>((Expression<Func<tbl_reminder_notification_config, bool>>)(t => t.id_notification == (int?)current.id_notification)).FirstOrDefault<tbl_reminder_notification_config>();
            }
            if (current.notification_type == 5 || current.notification_type == 7)
            {
                notificationPreConfig = new BuisinessLogic().getPreConfigNot("select * from tbl_notification_pre_config where id_notification=" + (object)ids);
                this.ViewData["cat"] = (object)this.getCategories();
                if (current.notification_type == 5)
                    this.ViewData["con"] = (object)this.getContentedit(notificationPreConfig.id_category.ToString());
                if (current.notification_type == 7)
                    this.ViewData["ass"] = (object)this.getAssessments(notificationPreConfig.id_category.ToString());
            }
            this.ViewData["reminder"] = (object)num;
            this.ViewData["reminderData"] = (object)notificationConfig;
            this.ViewData["pre"] = (object)notificationPreConfig;
            return (ActionResult)this.View();
        }

        public ActionResult edit_notification()
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int id_notification = Convert.ToInt32(this.Request.Form["id_notification"]);
            int int32_2 = Convert.ToInt32(this.Request.Form["notification-type"]);
            int int32_3 = Convert.ToInt32(this.Request.Form["availablity-div"]);
            int int32_4 = Convert.ToInt32(this.Request.Form["reminder-flag"]);
            tbl_notification notify = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == id_notification)).FirstOrDefault<tbl_notification>();
            if (notify != null)
            {
                notify.notification_name = this.Request.Form["notification-title"];
                notify.notification_description = this.Request.Form["notification-desc"];
                notify.notification_message = this.Request.Form["notification-message"];
                notify.notification_type = int32_2;
                notify.created_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["notification-created"].ToString()));
                notify.start_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["notification-started"].ToString()));
                notify.end_date = new DateTime?(new Utility().StringToDatetime(this.Request.Form["notification-ended"].ToString()));
                notify.reminder_flag = int32_4;
                int num1 = 0;
                int num2 = 0;
                notify.reminder_frequency = num2;
                notify.reminder_time = num1;
                notify.id_organization = int32_1;
                notify.status = int32_3 != 1 ? "N" : "A";
                notify.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
                if (int32_2 == 4)
                {
                    int int32_5 = Convert.ToInt32(this.Request.Form["setup-type"]);
                    int num3;
                    int num4;
                    int num5;
                    int num6;
                    if (int32_5 == 1)
                    {
                        num3 = 0;
                        num4 = 0;
                        num5 = 0;
                        num6 = 0;
                    }
                    else
                    {
                        num3 = Convert.ToInt32(this.Request.Form["select-DH"]);
                        num4 = Convert.ToInt32(this.Request.Form["select-YH"]);
                        num5 = Convert.ToInt32(this.Request.Form["select-YM"]);
                        num6 = Convert.ToInt32(this.Request.Form["select-TM"]);
                    }
                    tbl_reminder_notification_config notificationConfig = this.db.tbl_reminder_notification_config.Where<tbl_reminder_notification_config>((Expression<Func<tbl_reminder_notification_config, bool>>)(t => t.id_notification == (int?)notify.id_notification)).FirstOrDefault<tbl_reminder_notification_config>();
                    if (notificationConfig == null)
                    {
                        this.db.tbl_reminder_notification_config.Add(new tbl_reminder_notification_config()
                        {
                            id_notification = new int?(notify.id_notification),
                            reminder_type = new int?(int32_5),
                            DH = new int?(num3),
                            YH = new int?(num4),
                            YM = new int?(num5),
                            TM = new int?(num6),
                            status = "A",
                            updated_date_time = new DateTime?(DateTime.Now)
                        });
                        this.db.SaveChanges();
                    }
                    else
                    {
                        notificationConfig.id_notification = new int?(notify.id_notification);
                        notificationConfig.reminder_type = new int?(int32_5);
                        notificationConfig.DH = new int?(num3);
                        notificationConfig.YH = new int?(num4);
                        notificationConfig.YM = new int?(num5);
                        notificationConfig.TM = new int?(num6);
                        notificationConfig.status = "A";
                        notificationConfig.updated_date_time = new DateTime?(DateTime.Now);
                        this.db.SaveChanges();
                    }
                }
                else if (Convert.ToInt32(this.Request.Form["reminder-flag"]) == 1)
                {
                    tbl_notification_reminder entity = this.db.tbl_notification_reminder.Where<tbl_notification_reminder>((Expression<Func<tbl_notification_reminder, bool>>)(t => t.id_notification == (int?)notify.id_notification)).FirstOrDefault<tbl_notification_reminder>();
                    entity.id_notification = new int?(notify.id_notification);
                    int int32_6 = Convert.ToInt32(this.Request.Form["reminder-notification"]);
                    entity.id_reminder_notification = new int?(int32_6);
                    entity.reminder_frequency = new int?(notify.reminder_frequency);
                    entity.reminder_timeout = new int?(notify.reminder_time);
                    entity.status = "A";
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_notification_reminder.Add(entity);
                    this.db.SaveChanges();
                }
            }
            return (ActionResult)this.RedirectToAction("Index");
        }

        public ActionResult LoadNotification(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_notification notification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == ids)).FirstOrDefault<tbl_notification>();
            this.ViewData["notification"] = (object)notification;
            List<NotificationUserList> notificationUserListList = new List<NotificationUserList>();
            List<tbl_notification_config> list = this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)notification.id_notification)).ToList<tbl_notification_config>();
            if (list != null)
            {
                foreach (tbl_notification_config notificationConfig in list)
                {
                    tbl_notification_config item = notificationConfig;
                    NotificationUserList notificationUserList = new NotificationUserList();
                    this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => (int?)t.ID_USER == item.id_user)).FirstOrDefault<tbl_user>();
                    tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => (int?)t.ID_USER == item.id_user)).FirstOrDefault<tbl_profile>();
                    notificationUserList.USER = tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME;
                    notificationUserList.STATUS = !(item.status == "R") ? "Unread" : "Read";
                    notificationUserListList.Add(notificationUserList);
                }
            }
            this.ViewData["noteList"] = (object)notificationUserListList;
            return (ActionResult)this.View();
        }

        public ActionResult NotificationAttachment(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_notification current = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == ids)).FirstOrDefault<tbl_notification>();
            this.ViewData["notification"] = (object)current;
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_notification tblNotification = (tbl_notification)null;
            if (current.notification_type == 4)
            {
                tbl_notification_reminder remData = this.db.tbl_notification_reminder.Where<tbl_notification_reminder>((Expression<Func<tbl_notification_reminder, bool>>)(t => t.id_notification == (int?)current.id_notification)).FirstOrDefault<tbl_notification_reminder>();
                if (remData != null)
                    tblNotification = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => (int?)t.id_notification == remData.id_reminder_notification)).FirstOrDefault<tbl_notification>();
            }
            this.ViewData["reminder"] = (object)tblNotification;
            this.ViewData["RoleList"] = (object)this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object)int32 + " and status='A'").ToList<tbl_csst_role>();
            return (ActionResult)this.View();
        }

        public string getUserListForNotification(string id, string pattern, string cid, string type)
        {
            int ids = Convert.ToInt32(id);
            int int32_1 = Convert.ToInt32(cid);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern.Replace("'", "''");
            string str1 = "";
            if (type == "1")
                str1 = " and id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=" + (object)int32_1 + ") ";
            string sql = "select * from tbl_user where id_organization=" + (object)int32_2 + " and  id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%" + pattern + "%' or upper(LASTNAME) like '%" + pattern + "%')  and upper(USERID) like '%" + pattern + "%' " + str1;
            List<int> intList = new List<int>();
            List<tbl_user> list = this.db.tbl_user.SqlQuery(sql).ToList<tbl_user>();
            bool flag = false;
            string str2 = "";
            string str3 = "";
            string str4 = "";
            foreach (tbl_user tblUser in list)
            {
                tbl_user item = tblUser;
                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
                if (tblProfile != null)
                {
                    str4 = str4 + "<tr><td>" + tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME + " (" + item.USERID + ") </td>";
                    str4 += "<td>";
                    if (this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)ids & t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_notification_config>() == null)
                    {
                        str4 = str4 + " <a id=\"link_" + (object)item.ID_USER + "\" href=\"javascript:void(0)\" onclick=\"sensNotificationToUser('" + (object)item.ID_USER + "')\"><i class=\"glyphicon glyphicon-send\"></i></a>";
                        str4 = str4 + "<i style=\"display:none;\" id=\"done_" + (object)item.ID_USER + "\" class=\"glyphicon glyphicon-ok\"></i>";
                    }
                    else
                        str4 += "<h5>Notification Sent</h5>";
                    str4 += "</td>";
                    str4 += "</tr>";
                }
            }
            string str5 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th width=\"80%\">User Info</th>" + "<th  width=\"20%\">Send Notification</th>" + "</thead>" + "<tbody>" + str4 + "</tbody></table>";
            if (flag)
            {
                str5 = " <div class=\"row\" id=\"div-remove\" >   <div class=\"col-md-12\">   <div class=\"alert alert-info alert-dismissable\">   <input id=\"program-assignment\" type=\"button\" class=\"btn btn-primary btn-sm\" value=\"Remove Program From Role\" onclick=\"removeProgramToRole(0)\" /><strong>&nbsp;&nbsp; Click to Remove Role from  Program  </strong>   </div>   </div>   </div><hr/>" + str5;
            }
            str5 += "<div class='row'>  <div class='form-group'> <div class='col-md-2'><label class='control-label'> Start Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker1' name='start-date' value='" + str2 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div> <div class='col-md-2'><label class='control-label'>Expiry Date</label></div> <div class='col-md-4'>  <div class='input-group date'> <input type='text' class='form-control validate[required]' id='datetimepicker2' name='expiry-date'  value='" + str3 + "' /> <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span> </span>  </div> </div>  </div>   </div>";
            return "<hr/>" + str5;
        }

        public string sendNotificationToUser(string nid, string uid)
        {
            int id_config = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.id_ORGANIZATION);
            int int32 = Convert.ToInt32(content.ID_USER);
            int suid = Convert.ToInt32(uid);
            int nids = Convert.ToInt32(nid);
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(suid);
            if (note != null)
            {
                if (this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)note.id_notification && t.id_user == (int?)suid && t.id_category == (int?)0 && t.id_content == (int?)0 && t.id_assessment == (int?)0)).FirstOrDefault<tbl_notification_config>() == null)
                {
                    tbl_notification_config entity = new tbl_notification_config();
                    entity.id_assessment = new int?(0);
                    entity.id_category = new int?(0);
                    entity.id_content = new int?(0);
                    entity.id_notification = new int?(nids);
                    entity.id_user = new int?(suid);
                    entity.id_creater = new int?(int32);
                    entity.notification_key = note.notification_key;
                    entity.notification_action_type = "GEN";
                    entity.read_timestamp = new DateTime?(DateTime.Now);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    entity.user_type = "1";
                    entity.status = "A";
                    this.db.tbl_notification_config.Add(entity);
                    this.db.SaveChanges();
                    id_config = entity.id_notification_config;
                    string msg = "";
                    msg = note.notification_message;
                    DbSet<tbl_user_gcm_log> tblUserGcmLog1 = this.db.tbl_user_gcm_log;
                    Expression<Func<tbl_user_gcm_log, bool>> predicate = (Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)suid);
                    foreach (tbl_user_gcm_log tblUserGcmLog2 in tblUserGcmLog1.Where<tbl_user_gcm_log>(predicate).ToList<tbl_user_gcm_log>())
                    {
                        tbl_user_gcm_log gcm = tblUserGcmLog2;
                        if (!string.IsNullOrEmpty(gcm.GCMID))
                            Task.Run<Notification>((Func<Notification>)(() => new Notification(gcm.GCMID, msg, note.notification_name)));
                    }
                }
            }
            if (mailId.EMAIL != "" && note != null)
                new addCMS_CategoryModel().sendmail(mailId, note, id_config);
            return "1";
        }

        public string sendNotificationToRole(string nid, string uid)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            int nids = Convert.ToInt32(nid);
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            if (note != null)
            {
                int id_config = 0;
                object[] objArray = new object[5]
                {
          (object) "select * from tbl_user where id_organization=",
          (object) int32_1,
          (object) " and  id_user in (select id_user from tbl_role_user_mapping  where id_csst_role=",
          (object) uid,
          (object) ") "
                };
                foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery(string.Concat(objArray)).ToList<tbl_user>())
                {
                    tbl_user item = tblUser;
                    tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(item.ID_USER);
                    if (this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)note.id_notification && t.id_user == (int?)item.ID_USER && t.id_category == (int?)0 && t.id_content == (int?)0 && t.id_assessment == (int?)0)).FirstOrDefault<tbl_notification_config>() == null)
                    {
                        tbl_notification_config entity = new tbl_notification_config();
                        entity.id_assessment = new int?(0);
                        entity.id_category = new int?(0);
                        entity.id_content = new int?(0);
                        entity.id_notification = new int?(nids);
                        entity.id_user = new int?(item.ID_USER);
                        entity.id_creater = new int?(int32_2);
                        entity.notification_key = note.notification_key;
                        entity.notification_action_type = "GEN";
                        entity.read_timestamp = new DateTime?(DateTime.Now);
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        entity.user_type = "1";
                        entity.status = "A";
                        this.db.tbl_notification_config.Add(entity);
                        this.db.SaveChanges();
                        id_config = entity.id_notification_config;
                        string msg = "";
                        msg = note.notification_message;
                        DbSet<tbl_user_gcm_log> tblUserGcmLog1 = this.db.tbl_user_gcm_log;
                        Expression<Func<tbl_user_gcm_log, bool>> predicate = (Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)item.ID_USER);
                        foreach (tbl_user_gcm_log tblUserGcmLog2 in tblUserGcmLog1.Where<tbl_user_gcm_log>(predicate).ToList<tbl_user_gcm_log>())
                        {
                            tbl_user_gcm_log gcm = tblUserGcmLog2;
                            if (!string.IsNullOrEmpty(gcm.GCMID))
                                Task.Run<Notification>((Func<Notification>)(() => new Notification(gcm.GCMID, msg, note.notification_name)));
                        }
                    }
                    if (mailId.EMAIL != "" && note != null)
                        new addCMS_CategoryModel().sendmail(mailId, note, id_config);
                }
            }
            return "1";
        }

        public string sensNotificationToUserString(string nid, string value, string type)
        {
            int id_config = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            int nids = Convert.ToInt32(nid);
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            if (note != null)
            {
                object[] objArray = new object[9]
                {
          (object) "select * from tbl_user where id_organization= ",
          (object) int32_1,
          (object) " and id_user in (select id_user from tbl_profile where upper(FIRSTNAME) like '%",
          (object) value,
          (object) "%' or upper(LASTNAME) like '%",
          (object) value,
          (object) "%')  and upper(USERID) like '%",
          (object) value,
          (object) "%' "
                };
                foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery(string.Concat(objArray)).ToList<tbl_user>())
                {
                    tbl_user item = tblUser;
                    tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(item.ID_USER);
                    if (this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)note.id_notification && t.id_user == (int?)item.ID_USER && t.id_category == (int?)0 && t.id_content == (int?)0 && t.id_assessment == (int?)0)).FirstOrDefault<tbl_notification_config>() == null)
                    {
                        tbl_notification_config entity = new tbl_notification_config();
                        entity.id_assessment = new int?(0);
                        entity.id_category = new int?(0);
                        entity.id_content = new int?(0);
                        entity.id_notification = new int?(nids);
                        entity.id_user = new int?(item.ID_USER);
                        entity.id_creater = new int?(int32_2);
                        entity.notification_key = note.notification_key;
                        entity.notification_action_type = "GEN";
                        entity.read_timestamp = new DateTime?(DateTime.Now);
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        entity.user_type = "1";
                        entity.status = "A";
                        this.db.tbl_notification_config.Add(entity);
                        this.db.SaveChanges();
                        id_config = entity.id_notification_config;
                        string msg = "";
                        msg = note.notification_message;
                        DbSet<tbl_user_gcm_log> tblUserGcmLog1 = this.db.tbl_user_gcm_log;
                        Expression<Func<tbl_user_gcm_log, bool>> predicate = (Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)item.ID_USER);
                        foreach (tbl_user_gcm_log tblUserGcmLog2 in tblUserGcmLog1.Where<tbl_user_gcm_log>(predicate).ToList<tbl_user_gcm_log>())
                        {
                            tbl_user_gcm_log gcm = tblUserGcmLog2;
                            if (!string.IsNullOrEmpty(gcm.GCMID))
                                Task.Run<Notification>((Func<Notification>)(() => new Notification(gcm.GCMID, msg, note.notification_name)));
                        }
                    }
                    if (mailId.EMAIL != "" && note != null)
                        new addCMS_CategoryModel().sendmail(mailId, note, id_config);
                }
            }
            return "1";
        }

        public string sendNotificationToAllUser(string nid, string value, string type)
        {
            int id_config = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            int nids = Convert.ToInt32(nid);
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            if (note != null)
            {
                object[] objArray = new object[5]
                {
          (object) "select * from tbl_user where  id_organization= ",
          (object) int32_1,
          (object) " and id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=",
          (object) int32_1,
          (object) "))"
                };
                foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery(string.Concat(objArray)).ToList<tbl_user>())
                {
                    tbl_user item = tblUser;
                    tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(item.ID_USER);
                    if (this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)note.id_notification && t.id_user == (int?)item.ID_USER && t.id_category == (int?)0 && t.id_content == (int?)0 && t.id_assessment == (int?)0)).FirstOrDefault<tbl_notification_config>() == null)
                    {
                        tbl_notification_config entity = new tbl_notification_config();
                        entity.id_assessment = new int?(0);
                        entity.id_category = new int?(0);
                        entity.id_content = new int?(0);
                        entity.id_notification = new int?(nids);
                        entity.id_user = new int?(item.ID_USER);
                        entity.id_creater = new int?(int32_2);
                        entity.notification_key = note.notification_key;
                        entity.notification_action_type = "GEN";
                        entity.read_timestamp = new DateTime?(DateTime.Now);
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        entity.user_type = "1";
                        entity.status = "A";
                        this.db.tbl_notification_config.Add(entity);
                        this.db.SaveChanges();
                        id_config = entity.id_notification_config;
                        string msg = "";
                        msg = note.notification_message;
                        DbSet<tbl_user_gcm_log> tblUserGcmLog1 = this.db.tbl_user_gcm_log;
                        Expression<Func<tbl_user_gcm_log, bool>> predicate = (Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)item.ID_USER);
                        foreach (tbl_user_gcm_log tblUserGcmLog2 in tblUserGcmLog1.Where<tbl_user_gcm_log>(predicate).ToList<tbl_user_gcm_log>())
                        {
                            tbl_user_gcm_log gcm = tblUserGcmLog2;
                            if (!string.IsNullOrEmpty(gcm.GCMID))
                                Task.Run<Notification>((Func<Notification>)(() => new Notification(gcm.GCMID, msg, note.notification_name)));
                        }
                    }
                    if (mailId.EMAIL != "" && note != null)
                        new addCMS_CategoryModel().sendmail(mailId, note, id_config);
                }
            }
            return "1";
        }

        public string getContent(string val)
        {
            UserSession content1 = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content1.id_ORGANIZATION);
            List<tbl_content_organization_mapping> tblContentCat = new contentDashboardModel().get_tbl_content_cat(val, content1.id_ORGANIZATION);
            string content2 = "";
            foreach (tbl_content tblContent in new contentDashboardModel().getcon(tblContentCat))
                content2 = content2 + " <option value=" + (object)tblContent.ID_CONTENT + ">" + tblContent.CONTENT_QUESTION + "</option>";
            return content2;
        }

        public string sendNotificationToUserCont(string nid, string uid)
        {
            int id_config = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            int suid = Convert.ToInt32(uid);
            int nids = Convert.ToInt32(nid);
            tbl_content_user_assisgnment contentUserAssisgnment = new tbl_content_user_assisgnment();
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(suid);
            tbl_notification_pre_config preConfigNot = new BuisinessLogic().getPreConfigNot("select * from tbl_notification_pre_config where id_notification=" + (object)note.id_notification);
            if (note != null)
            {
                tbl_notification_config notificationConfig = this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)note.id_notification && t.id_user == (int?)suid && t.id_category == (int?)0 && t.id_content == (int?)0 && t.id_assessment == (int?)0)).FirstOrDefault<tbl_notification_config>();
                contentUserAssisgnment.id_user = new int?(suid);
                contentUserAssisgnment.id_organization = new int?(int32_1);
                contentUserAssisgnment.id_content = preConfigNot.id_content;
                contentUserAssisgnment.status = "A";
                contentUserAssisgnment.start_date = preConfigNot.start_date;
                contentUserAssisgnment.expiry_date = preConfigNot.end_date;
                contentUserAssisgnment.id_category = preConfigNot.id_category;
                contentUserAssisgnment.updated_date_time = new DateTime?(DateTime.Now);
                this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment);
                this.db.SaveChanges();
                if (notificationConfig == null)
                {
                    tbl_notification_config entity = new tbl_notification_config();
                    entity.id_assessment = new int?(0);
                    entity.id_category = preConfigNot.id_category;
                    entity.id_content = preConfigNot.id_content;
                    entity.id_notification = preConfigNot.id_notification;
                    entity.id_user = new int?(suid);
                    entity.id_creater = new int?(int32_2);
                    entity.notification_key = note.notification_key;
                    entity.notification_action_type = preConfigNot.notification_action_type;
                    entity.read_timestamp = new DateTime?(DateTime.Now);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    entity.user_type = "1";
                    entity.status = "A";
                    this.db.tbl_notification_config.Add(entity);
                    this.db.SaveChanges();
                    id_config = entity.id_notification_config;
                    string msg = "";
                    msg = note.notification_message;
                    DbSet<tbl_user_gcm_log> tblUserGcmLog1 = this.db.tbl_user_gcm_log;
                    Expression<Func<tbl_user_gcm_log, bool>> predicate = (Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)suid);
                    foreach (tbl_user_gcm_log tblUserGcmLog2 in tblUserGcmLog1.Where<tbl_user_gcm_log>(predicate).ToList<tbl_user_gcm_log>())
                    {
                        tbl_user_gcm_log gcm = tblUserGcmLog2;
                        if (!string.IsNullOrEmpty(gcm.GCMID))
                            Task.Run<Notification>((Func<Notification>)(() => new Notification(gcm.GCMID, msg, note.notification_name)));
                    }
                }
            }
            if (mailId.EMAIL != "" && note != null)
                new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment, int32_1);
            this.sendContentNotification(preConfigNot.id_content.ToString());
            return "1";
        }

        public string sendNotificationToUserContMulti(string nid)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32 = Convert.ToInt32(content.ID_USER);
            int id_config = 0;
            List<int> usersOfOrg = new BuisinessLogic().getUsersOfOrg(orgid);
            int nids = Convert.ToInt32(nid);
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            tbl_notification_pre_config pre = new BuisinessLogic().getPreConfigNot("select * from tbl_notification_pre_config where id_notification=" + (object)note.id_notification);
            foreach (int num in usersOfOrg)
            {
                int temp = num;
                tbl_content_user_assisgnment contentUserAssisgnment1 = this.db.tbl_content_user_assisgnment.Where<tbl_content_user_assisgnment>((Expression<Func<tbl_content_user_assisgnment, bool>>)(t => t.id_content == pre.id_content && t.id_organization == (int?)orgid && t.id_user == (int?)temp)).FirstOrDefault<tbl_content_user_assisgnment>();
                tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(temp);
                if (contentUserAssisgnment1 == null)
                {
                    tbl_content_user_assisgnment contentUserAssisgnment2 = new tbl_content_user_assisgnment();
                    contentUserAssisgnment2.id_user = new int?(temp);
                    contentUserAssisgnment2.id_organization = new int?(orgid);
                    contentUserAssisgnment2.id_content = pre.id_content;
                    contentUserAssisgnment2.id_category = pre.id_category;
                    contentUserAssisgnment2.status = "A";
                    contentUserAssisgnment2.start_date = pre.start_date;
                    contentUserAssisgnment2.expiry_date = pre.end_date;
                    contentUserAssisgnment2.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_content_user_assisgnment.Add(contentUserAssisgnment2);
                    this.db.SaveChanges();
                    if (contentUserAssisgnment2.id_content_user_assisgnment > 0 && note != null)
                    {
                        tbl_notification_config entity = new tbl_notification_config();
                        entity.id_assessment = new int?(0);
                        entity.id_category = pre.id_content;
                        entity.id_content = pre.id_category;
                        entity.id_notification = pre.id_notification;
                        entity.start_date = pre.start_date;
                        entity.end_date = pre.end_date;
                        entity.id_user = new int?(temp);
                        entity.notification_key = pre.notification_key;
                        entity.notification_action_type = pre.notification_action_type;
                        entity.read_timestamp = new DateTime?(DateTime.Now);
                        entity.updated_date_time = new DateTime?(DateTime.Now);
                        entity.user_type = "1";
                        entity.status = "P";
                        entity.id_creater = new int?(int32);
                        this.db.tbl_notification_config.Add(entity);
                        this.db.SaveChanges();
                        id_config = entity.id_notification_config;
                    }
                    if (mailId.EMAIL != "" && note != null)
                        new addCMS_CategoryModel().sendmail(mailId, note, id_config, contentUserAssisgnment2, orgid);
                }
                else
                {
                    contentUserAssisgnment1.start_date = pre.start_date;
                    contentUserAssisgnment1.expiry_date = pre.end_date;
                    contentUserAssisgnment1.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.SaveChanges();
                }
            }
            this.sendContentNotification(pre.id_content.ToString());
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

        public string sendNotificationToUserAss(string nid, string uid)
        {
            int id_config = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.id_ORGANIZATION);
            int int32_2 = Convert.ToInt32(content.ID_USER);
            int suid = Convert.ToInt32(uid);
            int nids = Convert.ToInt32(nid);
            tbl_assessment_user_assignment assessmentUserAssignment = new tbl_assessment_user_assignment();
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == nids && t.status == "A")).FirstOrDefault<tbl_notification>();
            tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(suid);
            tbl_notification_pre_config preConfigNot = new BuisinessLogic().getPreConfigNot("select * from tbl_notification_pre_config where id_notification=" + (object)note.id_notification);
            if (note != null)
            {
                tbl_notification_config notificationConfig = this.db.tbl_notification_config.Where<tbl_notification_config>((Expression<Func<tbl_notification_config, bool>>)(t => t.id_notification == (int?)note.id_notification && t.id_user == (int?)suid && t.id_category == (int?)0 && t.id_content == (int?)0 && t.id_assessment == (int?)0)).FirstOrDefault<tbl_notification_config>();
                assessmentUserAssignment.id_user = new int?(suid);
                assessmentUserAssignment.id_organization = new int?(int32_1);
                assessmentUserAssignment.id_assessment = preConfigNot.id_assessment;
                assessmentUserAssignment.status = "A";
                assessmentUserAssignment.start_date = preConfigNot.start_date;
                assessmentUserAssignment.expire_date = preConfigNot.end_date;
                assessmentUserAssignment.id_category = preConfigNot.id_category;
                assessmentUserAssignment.updated_date_time = new DateTime?(DateTime.Now);
                this.db.tbl_assessment_user_assignment.Add(assessmentUserAssignment);
                this.db.SaveChanges();
                if (notificationConfig == null)
                {
                    tbl_notification_config entity = new tbl_notification_config();
                    entity.id_assessment = preConfigNot.id_assessment;
                    entity.id_category = preConfigNot.id_category;
                    entity.id_content = new int?(0);
                    entity.id_notification = preConfigNot.id_notification;
                    entity.id_user = new int?(suid);
                    entity.id_creater = new int?(int32_2);
                    entity.notification_key = note.notification_key;
                    entity.notification_action_type = preConfigNot.notification_action_type;
                    entity.read_timestamp = new DateTime?(DateTime.Now);
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    entity.user_type = "1";
                    entity.status = "A";
                    this.db.tbl_notification_config.Add(entity);
                    this.db.SaveChanges();
                    id_config = entity.id_notification_config;
                    string msg = "";
                    msg = note.notification_message;
                    DbSet<tbl_user_gcm_log> tblUserGcmLog1 = this.db.tbl_user_gcm_log;
                    Expression<Func<tbl_user_gcm_log, bool>> predicate = (Expression<Func<tbl_user_gcm_log, bool>>)(t => t.id_user == (int?)suid);
                    foreach (tbl_user_gcm_log tblUserGcmLog2 in tblUserGcmLog1.Where<tbl_user_gcm_log>(predicate).ToList<tbl_user_gcm_log>())
                    {
                        tbl_user_gcm_log gcm = tblUserGcmLog2;
                        if (!string.IsNullOrEmpty(gcm.GCMID))
                            Task.Run<Notification>((Func<Notification>)(() => new Notification(gcm.GCMID, msg, note.notification_name)));
                    }
                }
            }
            if (mailId.EMAIL != "" && note != null)
                new addCMS_CategoryModel().sendmail_assessment(mailId, note, id_config, assessmentUserAssignment, int32_1);
            this.sendAssessmentNotification(preConfigNot.id_assessment.ToString());
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

        public string setUserBasedAssessmentToMultiUser(string nid)
        {
            int id_config = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            int int32 = Convert.ToInt32(content.ID_USER);
            int no_id = Convert.ToInt32(nid);
            List<int> usersOfOrg = new BuisinessLogic().getUsersOfOrg(orgid);
            DateTime? nullable1 = new DateTime?();
            DateTime? nullable2 = new DateTime?();
            tbl_notification note = this.db.tbl_notification.Where<tbl_notification>((Expression<Func<tbl_notification, bool>>)(t => t.id_notification == no_id && t.status == "A")).FirstOrDefault<tbl_notification>();
            tbl_notification_pre_config pre = new BuisinessLogic().getPreConfigNot("select * from tbl_notification_pre_config where id_notification=" + (object)note.id_notification);
            foreach (int num in usersOfOrg)
            {
                int user_id = num;
                tbl_profile mailId = new addCMS_CategoryModel().get_mail_id(user_id);
                tbl_assessment_user_assignment assessmentUserAssignment1 = this.db.tbl_assessment_user_assignment.Where<tbl_assessment_user_assignment>((Expression<Func<tbl_assessment_user_assignment, bool>>)(t => t.id_assessment == pre.id_assessment && t.id_organization == (int?)orgid && t.id_user == (int?)user_id)).FirstOrDefault<tbl_assessment_user_assignment>();
                if (assessmentUserAssignment1 == null)
                {
                    tbl_assessment_user_assignment assessmentUserAssignment2 = new tbl_assessment_user_assignment();
                    assessmentUserAssignment2.id_user = new int?(num);
                    assessmentUserAssignment2.id_organization = new int?(orgid);
                    assessmentUserAssignment2.id_assessment = pre.id_assessment;
                    assessmentUserAssignment2.id_category = pre.id_category;
                    assessmentUserAssignment2.status = "A";
                    assessmentUserAssignment2.start_date = pre.start_date;
                    assessmentUserAssignment2.expire_date = pre.end_date;
                    assessmentUserAssignment2.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_user_assignment.Add(assessmentUserAssignment2);
                    this.db.SaveChanges();
                    if (assessmentUserAssignment2.id_assessment_user_assignment > 0 && note != null)
                    {
                        tbl_notification_config entity = new tbl_notification_config();
                        entity.id_assessment = pre.id_assessment;
                        entity.id_category = pre.id_category;
                        entity.id_content = new int?(0);
                        entity.id_creater = new int?(int32);
                        entity.id_notification = pre.id_notification;
                        entity.id_user = new int?(num);
                        entity.start_date = pre.start_date;
                        entity.end_date = pre.end_date;
                        entity.notification_key = pre.notification_key;
                        entity.notification_action_type = pre.notification_action_type;
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
            this.sendAssessmentNotification(pre.id_assessment.ToString());
            return "1";
        }

        public List<tbl_category> getCategories()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int oid = Convert.ToInt32(content.id_ORGANIZATION);
            List<tbl_category> list1 = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>)(t => t.ID_ORGANIZATION == oid)).ToList<tbl_category>();
            List<int> intList = new List<int>();
            foreach (tbl_category tblCategory in list1)
            {
                List<tbl_content_organization_mapping> list2 = this.db.tbl_content_organization_mapping.SqlQuery("select * from tbl_content_organization_mapping where id_category in (select id_category from tbl_category where id_organization =" + content.id_ORGANIZATION + ")").ToList<tbl_content_organization_mapping>();
                intList.Add(list2.Count);
            }
            return list1;
        }

        public List<tbl_content> getContentedit(string val)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.id_ORGANIZATION);
            return new contentDashboardModel().getcon(new contentDashboardModel().get_tbl_content_cat(val, content.id_ORGANIZATION));
        }

        public List<tbl_assessment> getAssessments(string cat)
        {
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_assessment_categoty_mapping> list = this.db.tbl_assessment_categoty_mapping.SqlQuery("select * from tbl_assessment_categoty_mapping where id_category=" + cat + " AND status='A'").ToList<tbl_assessment_categoty_mapping>();
            List<tbl_assessment> assessments = new List<tbl_assessment>();
            foreach (tbl_assessment_categoty_mapping assessmentCategotyMapping in list)
            {
                tbl_assessment assName = new BuisinessLogic().getAssName("select * from tbl_assessment where id_assessment=" + (object)assessmentCategotyMapping.id_assessment + " AND status='A'");
                assessments.Add(assName);
            }
            return assessments;
        }
    }
}
