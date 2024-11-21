// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.dashboardController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    [UserFilter]
    public class dashboardController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public ActionResult Index(int flag = 0)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.ID_USER);
            int rid = Convert.ToInt32(content.Roleid);
            int oid = Convert.ToInt32(content.id_ORGANIZATION);
            List<tbl_cms_role_action_mapping> list = this.db.tbl_cms_role_action_mapping.Where<tbl_cms_role_action_mapping>((Expression<Func<tbl_cms_role_action_mapping, bool>>)(t => t.id_cms_role == (int?)rid && t.id_organization == (int?)oid)).ToList<tbl_cms_role_action_mapping>();
            content.action = list;
            this.HttpContext.Session.Contents["UserSession"] = (object)content;
            this.ViewData["organisation"] = (object)this.db.tbl_organization.OrderBy<tbl_organization, string>((Expression<Func<tbl_organization, string>>)(t => t.ORGANIZATION_NAME)).ToList<tbl_organization>();
            this.ViewData[nameof(flag)] = (object)flag;
            return (ActionResult)this.View();
        }

        public void setOrganisarion(string id)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            content.id_ORGANIZATION = id;
            System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
            System.Web.HttpContext.Current.Session["UserSession"] = (object)content;
        }

        [RoleAccessController(KEY = 15)]
        public ActionResult display_user_list()
        {
            List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + ")) order by USERID").ToList<tbl_user>();
            List<tbl_profile> tblProfileList = new List<tbl_profile>();
            foreach (tbl_user tblUser in list)
            {
                tbl_user user = tblUser;
                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == user.ID_USER)).FirstOrDefault<tbl_profile>();
                tblProfileList.Add(tblProfile);
            }
            this.ViewData["profile"] = (object)tblProfileList;
            this.ViewData["users"] = (object)list;
            return (ActionResult)this.View();
        }

        public ActionResult userRoleMapping()
        {
            List<tbl_profile> tblProfileList = new List<tbl_profile>();
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_csst_role> list1 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            List<tbl_user> list2 = this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + ")) order by USERID").ToList<tbl_user>();
            foreach (tbl_user tblUser in list2)
            {
                tbl_user item = tblUser;
                tbl_profile tblProfile = this.db.tbl_profile.Where<tbl_profile>((Expression<Func<tbl_profile, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_profile>();
                tblProfileList.Add(tblProfile);
            }
            this.ViewData["profile"] = (object)tblProfileList;
            this.ViewData["user_list"] = (object)list2;
            this.ViewData["user_roles"] = (object)list1;
            return (ActionResult)this.View();
        }

        public ActionResult add_content_tips()
        {
            int ido = Convert.ToInt32(((UserSession)System.Web.HttpContext.Current.Session["UserSession"]).id_ORGANIZATION);
            tbl_category tblCategory = this.db.tbl_category.Where<tbl_category>((Expression<Func<tbl_category, bool>>)(t => t.CATEGORYNAME.Contains("Tips") && t.ID_ORGANIZATION == ido)).FirstOrDefault<tbl_category>();
            if (tblCategory != null)
                this.ViewData["category"] = (object)null;
            else
                this.ViewData["category"] = (object)tblCategory;
            return (ActionResult)this.View();
        }

        [RoleAccessController(KEY = 5)]
        public ActionResult display_content()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Display ALL";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)int32 + " AND CATEGORY_TYPE in (1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            List<tbl_category> list2 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)int32 + " AND CATEGORY_TYPE in (0) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            this.ViewData["CategoryList"] = (object)list1;
            this.ViewData["ProgramList"] = (object)list2;
            return (ActionResult)this.View();
        }

        public ActionResult display_approval_content()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Content Approval";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string sql = "select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME";
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery(sql).ToList<tbl_category>();
            this.ViewData["content"] = (object)this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.CONTENT_OWNER == orgid && t.STATUS != "A")).OrderBy<tbl_content, string>((Expression<Func<tbl_content, string>>)(t => t.CONTENT_QUESTION)).ToList<tbl_content>();
            string str = "select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME";
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery(sql).ToList<tbl_category>();
            return (ActionResult)this.View();
        }

        public string getContentReport(string id, string pattern)
        {
            if (id == "")
                id = "0";
            int int32_1 = Convert.ToInt32(id);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_2 = Convert.ToInt32(content.id_ORGANIZATION);
            bool flag = new RoleBasedAccess().checkAccess(content.action, 6);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string str1 = "";
            if (!string.IsNullOrEmpty(pattern))
            {
                string str2 = str1 + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            }
            string sql = "";
            if (int32_1 == 1)
                //Rani Changes date 16-6-2023--
                //sql = sql + " select * from tbl_content where status in ('A','P') and content_owner=" + (object)int32_2 + " and id_content not in (select id_content from tbl_content_organization_mapping where id_organization=" + (object)int32_2 + " and status='A')    order by CONTENT_QUESTION ";
            sql = sql + " select * from tbl_content where status in ('A','P') and content_owner=" + (object)int32_2 + " order by CONTENT_QUESTION ";
            //end rani Changes
            if (int32_1 == 2)
                sql = sql + " select * from tbl_content where status in ('A')  and id_content in (select id_content from tbl_content_right_association where id_organization =" + (object)int32_2 + " and status='R' )   order by CONTENT_QUESTION ";
            if (int32_1 > 2)
                sql = "select * from tbl_content where status in ('A','P')  and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32_1 + ")   order by CONTENT_QUESTION ";
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery(sql).Take<tbl_content>(100).ToList<tbl_content>();
            List<int> intList = new List<int>();
            foreach (tbl_content tblContent in list1)
            {
                tbl_content tbl_view = tblContent;
                List<tbl_subscriptions> list2 = this.db.tbl_subscriptions.Where<tbl_subscriptions>((Expression<Func<tbl_subscriptions, bool>>)(t => t.ID_CONTENT == tbl_view.ID_CONTENT)).ToList<tbl_subscriptions>();
                intList.Add(list2.Count);
            }
            string str3 = "";
            int index = 0;
            foreach (tbl_content tblContent in list1)
            {
                tbl_content item = tblContent;
                str3 = str3 + "<tr><td><a target=\"_blank\" href=" + this.Url.Action("LoadContent", "contentDashboard", (object)new
                {
                    id = item.ID_CONTENT
                }) + ">" + item.CONTENT_QUESTION + "(" + (object)item.ID_CONTENT + ")</a></td>";
                tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>)(t => t.ID_USER == item.ID_USER)).FirstOrDefault<tbl_cms_users>();
                str3 += "<td>";
                str3 = tblCmsUsers == null ? str3 ?? "" : str3 + (tblCmsUsers.USERNAME ?? "");
                str3 += "</td>";
                str3 = str3 + "<td>" + item.UPDATED_DATE_TIME.ToShortDateString() + "</td>";
                if (flag)
                {
                    str3 += "<td> ";
                    if (item.STATUS == "P")
                        str3 += "Pending Approval";
                    else if (item.STATUS == "D")
                        str3 += "Rejected";
                    else if (item.CONTENT_OWNER == int32_2)
                    {
                        if (!(content.ID_USER == item.ID_USER.ToString()))
                        {
                            int? cmdUserType = content.USER.cmd_user_type;
                            int num = 0;
                            if ((cmdUserType.GetValueOrDefault() == num ? (cmdUserType.HasValue ? 1 : 0) : 0) == 0)
                            {
                                str3 = str3 ?? "";
                                goto label_30;
                            }
                        }
                        str3 = str3 + "<a href=" + this.Url.Action("editContent", "contentDashboard", (object)new
                        {
                            id = item.ID_CONTENT
                        }) + "><i class=\"glyphicon glyphicon-edit\"></i></a>";
                        if (intList[index] > 0)
                        {
                            str3 += "<a href=\"javascript:void(0)\" style=\"color:#ff0000;\" title=\"Content contains Subscription .Cannot Delete Content\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                        }
                        else
                        {
                            string str4 = item.CONTENT_QUESTION.Replace("'", "");
                            str3 = str3 + " | <a href=\"javascript:void(0)\" onclick=\"deleteContent(" + (object)item.ID_CONTENT + ",'" + str4 + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                        }
                    }
                    else
                        str3 += "Right To use";
                    label_30:
                    str3 += "</td>";
                }
                str3 += "</tr>";
            }
            string str5 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"70%\">Content</th><th width=\"10%\">Creator </th><th width=\"10%\" align=\"center\">Created Date</th>";
            if (flag)
                str5 += "<th width=\"10%\">Action</th>";
            return str5 + "</tr></thead>" + "<tbody>" + str3 + "</tbody></table>";
        }

        public ActionResult edit_answer_step(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)this.RedirectToAction("display_content_answer");
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[1]
            {
        (object) id
            });
            if (tblContentAnswer == null)
                return (ActionResult)this.RedirectToAction("display_content_answer");
            return (ActionResult)this.View((object)new answerSteps()
            {
                Answer = tblContentAnswer,
                STEPS = new contentDashboardModel().get_tbl_content_answer_steps(tblContentAnswer.ID_CONTENT_ANSWER)
            });
        }

        [ApproverFilter]
        public ActionResult ApprovalDashboard() => (ActionResult)this.View((object)this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select id_content from tbl_content_organization_mapping where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + ") and status='A' order by CONTENT_QUESTION").ToList<tbl_content>().ToList<tbl_content>());

        [ApproverFilter]
        public ActionResult PendingDashboard()
        {
            this.ViewData["content"] = (object)this.db.tbl_content.SqlQuery("select * from tbl_content where status='P' and id_category in (select Id_category from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + ") order by CONTENT_QUESTION").ToList<tbl_content>();
            return (ActionResult)this.View();
        }

        [ApproverFilter]
        public ActionResult contentApproval(int id)
        {
            if (id == 0)
                return (ActionResult)this.RedirectToAction("display_content");
            tbl_content content = this.db.tbl_content.Find(new object[1]
            {
        (object) Convert.ToInt32(id)
            });
            List<tbl_content_answer_steps> contentAnswerStepsList = new List<tbl_content_answer_steps>();
            List<tbl_content_metadata> tblContentMetadataList = new List<tbl_content_metadata>();
            tbl_content_answer content_answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == content.ID_CONTENT)).FirstOrDefault<tbl_content_answer>();
            List<tbl_content_answer_steps> list1 = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_answer_steps>();
            if (list1.Count > 0)
                contentAnswerStepsList = list1;
            List<tbl_content_metadata> list2 = this.db.tbl_content_metadata.Where<tbl_content_metadata>((Expression<Func<tbl_content_metadata, bool>>)(t => t.ID_CONTENT_ANSWER == content_answer.ID_CONTENT_ANSWER)).ToList<tbl_content_metadata>();
            if (list2.Count > 0)
                tblContentMetadataList = list2;
            this.ViewData["users"] = (object)(UserSession)this.HttpContext.Session.Contents["UserSession"];
            this.ViewData["content"] = (object)content;
            this.ViewData["content_answer"] = (object)content_answer;
            this.ViewData["answer_step"] = (object)contentAnswerStepsList;
            this.ViewData["metadata"] = (object)tblContentMetadataList;
            return (ActionResult)this.View();
        }

        public bool pendingApproval(string id)
        {
            string str = id;
            char[] chArray = new char[1] { ',' };
            foreach (string id1 in str.Split(chArray))
            {
                if (!string.IsNullOrEmpty(id1))
                    new contentDashboardModel().approveContent(id1);
            }
            return true;
        }

        public ActionResult approve_content()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Content Approval";
                string Id_assessment = this.Request.Form["id_content"];
                string Id_category = null;
                string Id_operation = "save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            string str1 = this.Request.Form["btn_submit"];
            string id = this.Request.Form["id_content"];
            string aid = this.Request.Form["id_answer"];
            string str2 = this.Request.Form["Responce"];
            switch (str1)
            {
                case "Approve":
                    new contentDashboardModel().approveContent(id, aid, str2);
                    return (ActionResult)this.RedirectToAction("display_approval_content");
                case "Reject":
                    new contentDashboardModel().RejectContent(id, aid, str2);
                    return (ActionResult)this.RedirectToAction("display_approval_content");
                default:
                    return (ActionResult)null;
            }
        }

        public string getContentapproveReport(string id, string pattern)
        {
            if (id == "")
                id = "0";
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string sql = "select * from tbl_content where status='P'  and content_owner=" + (object)int32_2 + "  ";
            if (int32_1 > 0)
                sql = sql + " and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32_1 + ")";
            if (!string.IsNullOrEmpty(pattern))
                sql = sql + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery(sql).Take<tbl_content>(100).ToList<tbl_content>();
            List<int> intList = new List<int>();
            foreach (tbl_content tblContent in list1)
            {
                tbl_content tbl_view = tblContent;
                List<tbl_subscriptions> list2 = this.db.tbl_subscriptions.Where<tbl_subscriptions>((Expression<Func<tbl_subscriptions, bool>>)(t => t.ID_CONTENT == tbl_view.ID_CONTENT)).ToList<tbl_subscriptions>();
                intList.Add(list2.Count);
            }
            string str = "";
            foreach (tbl_content tblContent in list1)
            {
                str += "<tr>";
                str = str + "<td>" + tblContent.CONTENT_QUESTION + "</td>";
                str = str + "<td><a href=\"" + this.Url.Action("ContentApproval", "contentDashboard", (object)new
                {
                    id = tblContent.ID_CONTENT
                }) + "><i class=\"glyphicon glyphicon-ok-sign\"></i>Approve</a></td>";
                str = str + "<td><div class=\"checkbox pending_lable\"><label><input type=\"checkbox\" id=\"" + (object)tblContent.ID_CONTENT + "\" value=\"" + (object)tblContent.ID_CONTENT + "\" name=\"pending_check\">Approve</label></div></td>";
                str += "</tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"80%\">Content</th><th width=\"20%\">Approve</th></tr></thead>" + "<tbody>" + str + "</tbody></table>";
        }

        public ActionResult add_content_links()
        {
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            return (ActionResult)this.View();
        }

        public ActionResult add_content_step()
        {
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            return (ActionResult)this.View();
        }

        public string getContentStepReport(string id, string pattern)
        {
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern.Replace("'", "''");
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select id_content from tbl_content_organization_mapping where id_organization=" + (object)int32_2 + " AND id_category=" + (object)int32_1 + ") and status in ('A','D') and upper(CONTENT_QUESTION) like '%" + pattern + "%' order by UPDATED_DATE_TIME DESC").OrderBy<tbl_content, string>((Func<tbl_content, string>)(t => t.CONTENT_QUESTION)).Take<tbl_content>(100).ToList<tbl_content>();
            List<int> intList = new List<int>();
            foreach (tbl_content tblContent in list1)
            {
                tbl_content tbl_view = tblContent;
                List<tbl_subscriptions> list2 = this.db.tbl_subscriptions.Where<tbl_subscriptions>((Expression<Func<tbl_subscriptions, bool>>)(t => t.ID_CONTENT == tbl_view.ID_CONTENT)).ToList<tbl_subscriptions>();
                intList.Add(list2.Count);
            }
            string str = "";
            foreach (tbl_content tblContent in list1)
            {
                str = str + "<tr><td>" + tblContent.CONTENT_QUESTION + "</td>";
                str += "<td>";
                str = str + "<a href=" + this.Url.Action("content_steps_link", "dashboard", (object)new
                {
                    id = tblContent.ID_CONTENT
                }) + " class=\"btn-sm btn-primary\">continue..</a>";
                str += "</td>";
                str += "</tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"70%\">Content</th><th width=\"30%\"></th></tr></thead>" + "<tbody>" + str + "</tbody></table>";
        }

        public string getContentForLinkReport(string id, string pattern)
        {
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string str1 = "select * from tbl_content where status in ('A','D')  and content_owner=" + (object)int32_2 + " ";
            if (int32_1 > 0)
                str1 = str1 + " and id_content in (select id_content from tbl_content_organization_mapping where id_organization=" + (object)int32_2 + " AND id_category=" + (object)int32_1 + ") ";
            if (!string.IsNullOrEmpty(pattern))
                str1 = str1 + " and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery(str1 + " order by UPDATED_DATE_TIME DESC").Take<tbl_content>(100).ToList<tbl_content>();
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
                str2 = str2 + "<tr><td>" + tblContent.CONTENT_QUESTION + "</td>";
                str2 += "<td>";
                str2 = str2 + "<a href=" + this.Url.Action("content_links", "dashboard", (object)new
                {
                    id = tblContent.ID_CONTENT
                }) + " class=\"btn-sm btn-primary\">continue..</a>";
                str2 += "</td>";
                str2 += "</tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"70%\">Content</th><th width=\"30%\"></th></tr></thead>" + "<tbody>" + str2 + "</tbody></table>";
        }

        public ActionResult content_links(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_content tblContent = this.db.tbl_content.Find(new object[1]
            {
        (object) ids
            });
            if (tblContent != null)
            {
                tbl_content_answer answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == ids)).FirstOrDefault<tbl_content_answer>();
                if (answer != null)
                {
                    this.ViewData[nameof(content_links)] = (object)this.db.tbl_content_type_link.Where<tbl_content_type_link>((Expression<Func<tbl_content_type_link, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_content_type_link>();
                    this.ViewData["content_answer"] = (object)answer;
                    this.ViewData["content_data"] = (object)tblContent;
                }
                else
                {
                    this.ViewData[nameof(content_links)] = (object)null;
                    this.ViewData["content_data"] = (object)null;
                    this.ViewData["content_answer"] = (object)null;
                }
            }
            else
                this.ViewData["content_data"] = (object)tblContent;
            this.ViewData["content_type"] = (object)this.db.tbl_content_type.ToList<tbl_content_type>();
            return (ActionResult)this.View();
        }

        public ActionResult content_steps_link(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_content tblContent = this.db.tbl_content.Find(new object[1]
            {
        (object) ids
            });
            tbl_content_answer answer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>)(t => t.ID_CONTENT == ids)).FirstOrDefault<tbl_content_answer>();
            List<tbl_content_answer_steps> contentAnswerStepsList = (List<tbl_content_answer_steps>)null;
            if (answer != null)
                contentAnswerStepsList = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER)).ToList<tbl_content_answer_steps>();
            this.ViewData["content_links"] = (object)contentAnswerStepsList;
            this.ViewData["content_data"] = (object)tblContent;
            this.ViewData["content_answer"] = (object)answer;
            return (ActionResult)this.View();
        }

        public string DeleteContentLinks(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_content_type_link entity = this.db.tbl_content_type_link.Where<tbl_content_type_link>((Expression<Func<tbl_content_type_link, bool>>)(t => t.ID_CONTENT_TYPE_LINK == ids)).FirstOrDefault<tbl_content_type_link>();
            if (entity != null)
            {
                this.db.tbl_content_type_link.Remove(entity);
                this.db.SaveChanges();
            }
            return "1";
        }

        public string DeleteContentSteps(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_content_answer_steps entity = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_ANSWER_STEP == ids)).FirstOrDefault<tbl_content_answer_steps>();
            if (entity != null)
            {
                this.db.tbl_content_answer_steps.Remove(entity);
                this.db.SaveChanges();
            }
            return "1";
        }

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
            this.ViewData["content_answer"] = (object)tblContentAnswer;
            this.ViewData["content_type"] = (object)this.db.tbl_content_type.ToList<tbl_content_type>();
            return (ActionResult)this.View();
        }

        public ActionResult link_answer_content(FormCollection collection)
        {
            string str1 = this.Request.Form["btn_submit"];
            int int32_1 = Convert.ToInt32(this.Request.Form["ID_CONTENT_ANSWER"]);
            string str2 = this.Request.Form["description"].ToString();
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[1]
            {
        (object) int32_1
            });
            int int32_2 = Convert.ToInt32(this.Request.Form["select_type"]);
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
                                DESCRIPTION = str2,
                                STATUS = "A",
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
            return (ActionResult)this.RedirectToAction("content_links", (object)new
            {
                id = tblContentAnswer.ID_CONTENT
            });
        }

        public ActionResult step_answer_content(FormCollection collection)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int answerid = Convert.ToInt32(this.Request.Form["ID_CONTENT_ANSWER"]);
            tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Find(new object[1]
            {
        (object) answerid
            });
            int int32 = Convert.ToInt32(this.Request.Form["select_type"]);
            int num = this.db.tbl_content_answer_steps.Where<tbl_content_answer_steps>((Expression<Func<tbl_content_answer_steps, bool>>)(t => t.ID_CONTENT_ANSWER == answerid)).OrderByDescending<tbl_content_answer_steps, int>((Expression<Func<tbl_content_answer_steps, int>>)(t => t.STEPNO)).ToList<tbl_content_answer_steps>()[0].STEPNO + 1;
            switch (int32)
            {
                case 8:
                    string str1 = this.Request.Form["step-8-part1"].ToString();
                    string str2 = this.Request.Form["step-8-part2"].ToString();
                    this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                    {
                        ANSWER_STEPS_PART1 = str1,
                        ANSWER_STEPS_PART2 = str2,
                        ANSWER_STEPS_PART3 = "",
                        ID_CONTENT_ANSWER = answerid,
                        ID_THEME = new int?(8),
                        STEPNO = num,
                        ANSWER_STEPS_IMG1 = "",
                        ANSWER_STEPS_IMG2 = "",
                        ANSWER_STEPS_IMG3 = "",
                        STATUS = "A"
                    });
                    this.db.SaveChanges();
                    break;
                case 9:
                    string str3 = "";
                    string str4 = this.Request.Form["step-9-part1"].ToString();
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["step-9-btn"];
                        str3 = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["step-9-btn"].FileName);
                        if (file.ContentLength > 0)
                        {
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/"), "step-" + (object)tblContentAnswer.ID_CONTENT + (object)num + str3);
                            file.SaveAs(filename);
                        }
                    }
                    this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                    {
                        ANSWER_STEPS_PART1 = str4,
                        ANSWER_STEPS_PART2 = "",
                        ANSWER_STEPS_PART3 = "",
                        ID_CONTENT_ANSWER = answerid,
                        ID_THEME = new int?(9),
                        STEPNO = num,
                        ANSWER_STEPS_IMG1 = "step-" + (object)tblContentAnswer.ID_CONTENT + (object)num + str3,
                        ANSWER_STEPS_IMG2 = "",
                        ANSWER_STEPS_IMG3 = "",
                        STATUS = "A"
                    });
                    this.db.SaveChanges();
                    break;
                case 10:
                    string str5 = this.Request.Form["step-10-part1"].ToString();
                    string str6 = this.Request.Form["step-10-part2"].ToString();
                    string str7 = this.Request.Form["step-10-part3"].ToString();
                    this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                    {
                        ANSWER_STEPS_PART1 = str5,
                        ANSWER_STEPS_PART2 = str6,
                        ANSWER_STEPS_PART3 = str7,
                        ID_CONTENT_ANSWER = answerid,
                        ID_THEME = new int?(10),
                        STEPNO = num,
                        ANSWER_STEPS_IMG1 = "",
                        ANSWER_STEPS_IMG2 = "",
                        ANSWER_STEPS_IMG3 = "",
                        STATUS = "A"
                    });
                    this.db.SaveChanges();
                    break;
                case 11:
                    string str8 = "";
                    if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                    {
                        HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["step-11-btn"];
                        str8 = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["step-11-btn"].FileName);
                        if (file.ContentLength > 0)
                        {
                            if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/")))
                                Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/"));
                            string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/" + content.id_ORGANIZATION + "/" + (object)tblContentAnswer.ID_CONTENT + "/"), "step-" + (object)tblContentAnswer.ID_CONTENT + (object)num + str8);
                            file.SaveAs(filename);
                        }
                    }
                    this.db.tbl_content_answer_steps.Add(new tbl_content_answer_steps()
                    {
                        ANSWER_STEPS_PART1 = "",
                        ANSWER_STEPS_PART2 = "",
                        ANSWER_STEPS_PART3 = "",
                        ID_CONTENT_ANSWER = answerid,
                        ID_THEME = new int?(11),
                        STEPNO = num,
                        ANSWER_STEPS_IMG1 = "step-" + (object)tblContentAnswer.ID_CONTENT + (object)num + str8,
                        ANSWER_STEPS_IMG2 = "",
                        ANSWER_STEPS_IMG3 = "",
                        STATUS = "A"
                    });
                    this.db.SaveChanges();
                    break;
            }
            return (ActionResult)this.RedirectToAction("content_steps_link", (object)new
            {
                id = tblContentAnswer.ID_CONTENT
            });
        }

        public ActionResult setRoleAccess()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Set Role Based Access";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            return (ActionResult)this.View();
        }

        public string getContentRoleReport(string id, string pattern)
        {
            int int32 = Convert.ToInt32(id);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_csst_role> list1 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>)(t => t.csst_role)).ToList<tbl_csst_role>();
            pattern = pattern.Replace("'", "''");
            string sql = "select * from tbl_content where  id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32 + ") and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<int> intList = new List<int>();
            List<tbl_content> list2 = this.db.tbl_content.SqlQuery(sql).ToList<tbl_content>();
            foreach (tbl_content tblContent in list2)
            {
                tbl_content tbl_view = tblContent;
                List<tbl_subscriptions> list3 = this.db.tbl_subscriptions.Where<tbl_subscriptions>((Expression<Func<tbl_subscriptions, bool>>)(t => t.ID_CONTENT == tbl_view.ID_CONTENT)).ToList<tbl_subscriptions>();
                intList.Add(list3.Count);
            }
            string str1 = "";
            foreach (tbl_content tblContent in list2)
            {
                tbl_content item = tblContent;
                str1 = str1 + "<tr><td>" + item.CONTENT_QUESTION + " (" + (object)item.ID_CONTENT + ") </td>";
                foreach (tbl_csst_role tblCsstRole in list1)
                {
                    tbl_csst_role cscc = tblCsstRole;
                    str1 += "<td>";
                    tbl_content_role_mapping contentRoleMapping = this.db.tbl_content_role_mapping.Where<tbl_content_role_mapping>((Expression<Func<tbl_content_role_mapping, bool>>)(t => t.id_content == (int?)item.ID_CONTENT && t.id_csst_role == (int?)cscc.id_csst_role)).FirstOrDefault<tbl_content_role_mapping>();
                    string str2 = "";
                    if (contentRoleMapping != null)
                        str2 = " checked ";
                    str1 = str1 + "<input class=\"inline\"  type=\"checkbox\" onchange=\"setCsstRole('" + (object)item.ID_CONTENT + "_" + (object)cscc.id_csst_role + "')\" id=\"" + (object)item.ID_CONTENT + "_" + (object)cscc.id_csst_role + "\" " + str2 + " >";
                    str1 += "</td>";
                }
                str1 += "</tr>";
            }
            string str3 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr>" + " <th>Content</th>";
            foreach (tbl_csst_role tblCsstRole in list1)
                str3 = str3 + "<th>" + tblCsstRole.csst_role + "</th>";
            return str3 + "</thead>" + "<tbody>" + str1 + "</tbody></table>";
        }

        public string getTableLine() => "";

        public string setContentRole(string str, string opt)
        {
            int int32 = Convert.ToInt32(opt);
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string[] strArray = str.Split('_');
            int id_content = Convert.ToInt32(strArray[0]);
            int id_role = Convert.ToInt32(strArray[1]);
            tbl_content_role_mapping entity = this.db.tbl_content_role_mapping.Where<tbl_content_role_mapping>((Expression<Func<tbl_content_role_mapping, bool>>)(t => t.id_content == (int?)id_content && t.id_csst_role == (int?)id_role)).FirstOrDefault<tbl_content_role_mapping>();
            if (int32 == 1)
            {
                if (entity == null)
                {
                    this.db.tbl_content_role_mapping.Add(new tbl_content_role_mapping()
                    {
                        id_content = new int?(id_content),
                        id_csst_role = new int?(id_role),
                        status = "A",
                        updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                }
            }
            else if (entity != null)
            {
                this.db.tbl_content_role_mapping.Remove(entity);
                this.db.SaveChanges();
            }
            return "";
        }

        public string setUserRole(string str, string opt)
        {
            int int32 = Convert.ToInt32(opt);
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string[] strArray = str.Split('_');
            int id_user = Convert.ToInt32(strArray[0]);
            int id_role = Convert.ToInt32(strArray[1]);
            tbl_role_user_mapping entity = this.db.tbl_role_user_mapping.Where<tbl_role_user_mapping>((Expression<Func<tbl_role_user_mapping, bool>>)(t => t.id_user == (int?)id_user && t.id_csst_role == (int?)id_role)).FirstOrDefault<tbl_role_user_mapping>();
            if (int32 == 1)
            {
                if (entity == null)
                {
                    this.db.tbl_role_user_mapping.Add(new tbl_role_user_mapping()
                    {
                        id_user = new int?(id_user),
                        id_csst_role = new int?(id_role),
                        status = "A",
                        updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                }
            }
            else if (entity != null)
            {
                this.db.tbl_role_user_mapping.Remove(entity);
                this.db.SaveChanges();
            }
            return "";
        }

        public ActionResult add_banner(int flag = 0)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add New Banner";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            this.ViewData[nameof(flag)] = (object)flag;
            return (ActionResult)this.View();
        }

        public ActionResult add_banner_action()
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_banner entity = new tbl_banner();
            entity.banner_name = this.Request.Form["banner-name"].ToString();
            entity.banner_action_url = this.Request.Form["banner-url"].ToString();
            entity.id_organization = int32;
            entity.status = "A";
            entity.updated_date_time = new DateTime?(DateTime.Now);
            this.db.tbl_banner.Add(entity);
            this.db.SaveChanges();
            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
            {
                HttpPostedFile file1 = System.Web.HttpContext.Current.Request.Files["banner-image"];
                HttpPostedFile file2 = System.Web.HttpContext.Current.Request.Files["banner-image"];
                if (file1.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file2.FileName);
                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Banner/")))
                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Banner/"));
                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Banner/"), entity.id_organization.ToString() + "_BANNER_" + (object)entity.id_banner + extension);
                    file1.SaveAs(filename);
                    entity.banner_image = entity.id_organization.ToString() + "_BANNER_" + (object)entity.id_banner + extension;
                    this.db.SaveChanges();
                }
            }
            return this.Request.Form["btn_submit"].ToString().Equals("Save") ? (ActionResult)this.RedirectToAction("add_banner", "dashboard", (object)new
            {
                flag = 1
            }) : (ActionResult)this.RedirectToAction("Index", (object)new
            {
                flag = 1
            });
        }

        public ActionResult edit_banner(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_banner tblBanner = new tbl_banner();
            if (ids <= 0)
                return (ActionResult)this.RedirectToAction("Index");
            this.ViewData["banner"] = (object)this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == ids && t.status == "A")).FirstOrDefault<tbl_banner>();
            return (ActionResult)this.View();
        }

        public ActionResult edit_banner_action()
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(this.Request.Form["banner_id"]);
            tbl_banner tblBanner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == ids && t.status == "A")).FirstOrDefault<tbl_banner>();
            if (tblBanner != null)
            {
                tblBanner.banner_name = this.Request.Form["banner-name"].ToString();
                tblBanner.banner_action_url = this.Request.Form["banner-url"].ToString();
                tblBanner.id_organization = int32;
                tblBanner.status = "A";
                tblBanner.updated_date_time = new DateTime?(DateTime.Now);
                this.db.SaveChanges();
            }
            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
            {
                HttpPostedFile file1 = System.Web.HttpContext.Current.Request.Files["banner-image"];
                HttpPostedFile file2 = System.Web.HttpContext.Current.Request.Files["banner-image"];
                if (file1.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file2.FileName);
                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Banner/")))
                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Banner/"));
                    string str = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Banner/"), tblBanner.id_organization.ToString() + "_BANNER_" + (object)tblBanner.id_banner + extension);
                    if (System.IO.File.Exists(str))
                        System.IO.File.Delete(str);
                    file1.SaveAs(str);
                    tblBanner.banner_image = tblBanner.id_organization.ToString() + "_BANNER_" + (object)tblBanner.id_banner + extension;
                    this.db.SaveChanges();
                }
            }
            this.Request.Form["btn_submit"].ToString();
            return (ActionResult)this.RedirectToAction("display_banner");
        }

        public ActionResult display_banner()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Banner Display";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_banner> list = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_organization == oid)).ToList<tbl_banner>();
            this.ViewData["urls"] = (object)(ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Banner/");
            this.ViewData["banner"] = (object)list;
            return (ActionResult)this.View();
        }

        public ActionResult ContentBanner(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            tbl_banner tblBanner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == ids && t.id_organization == orgid && t.status == "A")).FirstOrDefault<tbl_banner>();
            if (tblBanner == null)
                return (ActionResult)this.RedirectToAction("Index");
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            this.ViewData["banner"] = (object)tblBanner;
            this.ViewData["urls"] = (object)(ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Banner/");
            return (ActionResult)this.View();
        }

        public ActionResult BannerContentList(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            tbl_banner banner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == ids && t.id_organization == orgid && t.status == "A")).FirstOrDefault<tbl_banner>();
            if (banner == null)
                return (ActionResult)this.RedirectToAction("Index");
            List<tbl_content_banner> list = this.db.tbl_content_banner.Where<tbl_content_banner>((Expression<Func<tbl_content_banner, bool>>)(t => t.id_banner == banner.id_banner)).ToList<tbl_content_banner>();
            List<tbl_content> tblContentList = new List<tbl_content>();
            foreach (tbl_content_banner tblContentBanner in list)
            {
                tbl_content_banner item = tblContentBanner;
                tbl_content tblContent = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == item.id_content)).FirstOrDefault<tbl_content>();
                if (tblContent != null)
                    tblContentList.Add(tblContent);
            }
            string str = "select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND status='A' order by CATEGORYNAME";
            this.ViewData["banner"] = (object)banner;
            this.ViewData["content"] = (object)tblContentList;
            this.ViewData["urls"] = (object)(ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Banner/");
            return (ActionResult)this.View();
        }

        public string deactivateBanner(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            tbl_banner banner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == ids && t.id_organization == orgid && t.status == "A")).FirstOrDefault<tbl_banner>();
            if (banner == null)
                return "";
            banner.status = "D";
            this.db.SaveChanges();
            List<tbl_content_banner> list = this.db.tbl_content_banner.Where<tbl_content_banner>((Expression<Func<tbl_content_banner, bool>>)(t => t.id_banner == banner.id_banner)).ToList<tbl_content_banner>();
            if (list != null)
            {
                foreach (tbl_content_banner tblContentBanner in list)
                {
                    tblContentBanner.status = "D";
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public string activateBanner(string id)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            tbl_banner tblBanner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == ids && t.id_organization == orgid && t.status == "D")).FirstOrDefault<tbl_banner>();
            if (tblBanner == null)
                return "";
            tblBanner.status = "A";
            this.db.SaveChanges();
            return "";
        }

        public string getContentBanner(string id, string pattern, string bid)
        {
            if (id == "")
                id = "0";
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(bid);
            int int32_3 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string sql = "select * from tbl_content where   content_owner=" + (object)int32_3 + "   ";
            if (int32_1 > 0)
                sql = sql + " and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32_1 + ")";
            if (!string.IsNullOrEmpty(pattern))
                sql = sql + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<tbl_content> list = this.db.tbl_content.SqlQuery(sql).OrderBy<tbl_content, string>((Func<tbl_content, string>)(t => t.CONTENT_QUESTION)).Take<tbl_content>(100).ToList<tbl_content>();
            string str = "";
            foreach (tbl_content tblContent in list)
            {
                tbl_content item = tblContent;
                tbl_content_banner cbanner = this.db.tbl_content_banner.Where<tbl_content_banner>((Expression<Func<tbl_content_banner, bool>>)(t => t.id_content == item.ID_CONTENT && t.status == "A")).FirstOrDefault<tbl_content_banner>();
                str = str + "<tr><td><a target=\"_blank\" href=" + this.Url.Action("LoadContent", "contentDashboard", (object)new
                {
                    id = item.ID_CONTENT
                }) + ">" + item.CONTENT_QUESTION + "(" + (object)item.ID_CONTENT + ")</a></td>";
                str += "<td> ";
                if (cbanner == null)
                {
                    str += "No Banner attached";
                }
                else
                {
                    tbl_banner tblBanner = this.db.tbl_banner.Where<tbl_banner>((Expression<Func<tbl_banner, bool>>)(t => t.id_banner == cbanner.id_banner && t.status == "A")).FirstOrDefault<tbl_banner>();
                    str += tblBanner.banner_name ?? "";
                }
                str += "</td><td> ";
                if (cbanner != null)
                {
                    if (cbanner.id_banner == int32_2)
                        str = str + "  <a href=\"javascript:void(0)\" onclick=\"removeBannerToContent(" + (object)item.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                    else
                        str = str + "  <a href=\"javascript:void(0)\" onclick=\"addBannerToContent(" + (object)item.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-plus\"></i></a>";
                }
                else
                    str = str + "  <a href=\"javascript:void(0)\" onclick=\"addBannerToContent(" + (object)item.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-plus\"></i></a>";
                str += "</td></tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"45%\">Content</th><th width=\"45%\">Banner</th><th width=\"8%\"></th></tr></thead>" + "<tbody>" + str + "</tbody></table>";
        }

        public string addBannerToContent(string id, string cid)
        {
            int int32_1 = Convert.ToInt32(id);
            int cids = Convert.ToInt32(cid);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content content = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == cids)).FirstOrDefault<tbl_content>();
            if (content != null)
            {
                tbl_content_banner tblContentBanner = this.db.tbl_content_banner.Where<tbl_content_banner>((Expression<Func<tbl_content_banner, bool>>)(t => t.id_content == content.ID_CONTENT)).FirstOrDefault<tbl_content_banner>();
                if (tblContentBanner == null)
                {
                    this.db.tbl_content_banner.Add(new tbl_content_banner()
                    {
                        id_banner = int32_1,
                        id_content = content.ID_CONTENT,
                        id_organization = int32_2,
                        status = "A",
                        date_assigned = new DateTime?(DateTime.Now),
                        date_removed = new DateTime?(DateTime.Now),
                        updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                }
                else
                {
                    tblContentBanner.id_banner = int32_1;
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public string removeBannerToContent(string id, string cid)
        {
            Convert.ToInt32(id);
            int cids = Convert.ToInt32(cid);
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content content = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == cids)).FirstOrDefault<tbl_content>();
            if (content != null)
            {
                tbl_content_banner tblContentBanner = this.db.tbl_content_banner.Where<tbl_content_banner>((Expression<Func<tbl_content_banner, bool>>)(t => t.id_content == content.ID_CONTENT)).FirstOrDefault<tbl_content_banner>();
                if (tblContentBanner != null)
                {
                    tblContentBanner.date_removed = new DateTime?(DateTime.Now);
                    tblContentBanner.status = "D";
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public ActionResult DisplayContentLink()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Manage Linked Content";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION) + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            return (ActionResult)this.View();
        }

        public string getContentLinkReport(string id, string pattern)
        {
            if (id == "")
                id = "0";
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string str1 = "select * from tbl_content where status in ('A')  and content_owner=" + (object)int32_2 + "  ";
            if (int32_1 > 0)
                str1 = str1 + " and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32_1 + ")";
            if (!string.IsNullOrEmpty(pattern))
                str1 = str1 + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery(str1 + "  order by CONTENT_QUESTION ").Take<tbl_content>(100).ToList<tbl_content>();
            string str2 = "";
            foreach (tbl_content tblContent in list1)
            {
                tbl_content item = tblContent;
                List<tbl_content_link> list2 = this.db.tbl_content_link.Where<tbl_content_link>((Expression<Func<tbl_content_link, bool>>)(t => t.ID_CONTENT_PARENT == item.ID_CONTENT)).ToList<tbl_content_link>();
                str2 = str2 + "<tr><td><a target=\"_blank\" href=" + this.Url.Action("LoadContent", "contentDashboard", (object)new
                {
                    id = item.ID_CONTENT
                }) + ">" + item.CONTENT_QUESTION + "(" + (object)item.ID_CONTENT + ")</a></td>";
                str2 += "<td> ";
                str2 = list2 != null ? str2 + "Connected Content Count : " + (object)list2.Count : str2 + "Connected Content Count : 0 ";
                str2 += "</td><td> ";
                str2 = str2 + "  <a href=" + this.Url.Action("AddContentLink", "dashboard", (object)new
                {
                    id = item.ID_CONTENT
                }) + " \"><i class=\"glyphicon glyphicon-plus\"></i></a>";
                str2 += "</td></tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"45%\">Content</th><th width=\"45%\"></th><th width=\"8%\"></th></tr></thead>" + "<tbody>" + str2 + "</tbody></table>";
        }

        public string getContentToContentLink(string id, string pattern, string cpid)
        {
            if (id == "")
                id = "0";
            int int32_1 = Convert.ToInt32(id);
            int parentid = Convert.ToInt32(cpid);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string str1 = "select * from tbl_content where id_content not in (" + (object)parentid + ") and status='A'  and content_owner=" + (object)int32_2 + "   ";
            if (int32_1 > 0)
                str1 = str1 + " and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32_1 + ")";
            if (!string.IsNullOrEmpty(pattern))
                str1 = str1 + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<tbl_content> list = this.db.tbl_content.SqlQuery(str1 + "  order by CONTENT_QUESTION ").Take<tbl_content>(100).ToList<tbl_content>();
            string str2 = "";
            foreach (tbl_content tblContent in list)
            {
                tbl_content item = tblContent;
                tbl_content_link tblContentLink = this.db.tbl_content_link.Where<tbl_content_link>((Expression<Func<tbl_content_link, bool>>)(t => t.ID_CONTENT_PARENT == parentid && t.ID_CONTENT_CHILD == item.ID_CONTENT)).FirstOrDefault<tbl_content_link>();
                str2 = str2 + "<tr><td><a target=\"_blank\" href=" + this.Url.Action("LoadContent", "contentDashboard", (object)new
                {
                    id = item.ID_CONTENT
                }) + ">" + item.CONTENT_QUESTION + "(" + (object)item.ID_CONTENT + ")</a></td>";
                str2 += "<td> ";
                if (tblContentLink == null)
                    str2 = str2 + "<a href=\"javascript:void(0)\" onclick=\"addContentToContent(" + (object)item.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-plus\"></i></a>";
                else
                    str2 += "Already Linked ";
                str2 += "</td>";
                str2 += "</tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"55%\">Content</th><th width=\"45%\"></th></tr></thead>" + "<tbody>" + str2 + "</tbody></table>";
        }

        public ActionResult AddContentLink(string id)
        {
            int ids = Convert.ToInt32(id);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content content = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == ids)).FirstOrDefault<tbl_content>();
            if (content == null)
                return (ActionResult)this.RedirectToAction("Index");
            List<tbl_content> list = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select id_content_child from tbl_content_link where id_content_parent=" + (object)ids + ") and status='A'  and content_owner=" + (object)orgid + "   order by CONTENT_QUESTION").ToList<tbl_content>();
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            this.ViewData["content"] = (object)content;
            this.ViewData["content_list"] = (object)list;
            this.ViewData["mapping"] = (object)this.db.tbl_content_organization_mapping.Where<tbl_content_organization_mapping>((Expression<Func<tbl_content_organization_mapping, bool>>)(t => t.id_content == content.ID_CONTENT && t.id_organization == orgid)).ToList<tbl_content_organization_mapping>();
            return (ActionResult)this.View();
        }

        public string GetLinkedQuestions(string id)
        {
            string str1 = "";
            int ids = Convert.ToInt32(id);
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == ids && t.STATUS == "A")).FirstOrDefault<tbl_content>();
            List<tbl_content> list = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select id_content_child from tbl_content_link where id_content_parent=" + (object)ids + ") and status='A'  and content_owner=" + (object)int32 + " ").OrderBy<tbl_content, string>((Func<tbl_content, string>)(t => t.CONTENT_QUESTION)).ToList<tbl_content>();
            string str2 = str1 + "<table id=\"report-table-link\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead> <tr>  <th>Content/Activity</th><th>De-link</th> </tr></thead><tbody>";
            foreach (tbl_content tblContent in list)
            {
                str2 += "<tr>  <td>";
                str2 += " <i class=\"glyphicon glyphicon-list\"></i>&nbsp; " + tblContent.CONTENT_QUESTION;
                str2 = str2 + "</td><td><a href=\"javascript:void(0)\" onclick=\"deleteContentToContent(" + (object)tblContent.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-remove\"></i></a></td> </tr>";
            }
            return str2 + "</tbody>  </table>";
        }

        public string addContentToContent(string cid, string cpid)
        {
            int ids = Convert.ToInt32(cid);
            int cids = Convert.ToInt32(cpid);
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == ids && t.STATUS == "A")).FirstOrDefault<tbl_content>() != null)
            {
                if (this.db.tbl_content_link.Where<tbl_content_link>((Expression<Func<tbl_content_link, bool>>)(t => t.ID_CONTENT_PARENT == ids && t.ID_CONTENT_CHILD == cids)).FirstOrDefault<tbl_content_link>() == null)
                {
                    this.db.tbl_content_link.Add(new tbl_content_link()
                    {
                        ID_CONTENT_PARENT = ids,
                        ID_CONTENT_CHILD = cids,
                        ID_LINK_TYPE = 1,
                        STATUS = "A",
                        UPDATED_DATE_TIME = DateTime.Now
                    });
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public string deleteContentToContent(string cid, string cpid)
        {
            int ids = Convert.ToInt32(cid);
            int cids = Convert.ToInt32(cpid);
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            if (this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == ids && t.STATUS == "A")).FirstOrDefault<tbl_content>() != null)
            {
                tbl_content_link entity = this.db.tbl_content_link.Where<tbl_content_link>((Expression<Func<tbl_content_link, bool>>)(t => t.ID_CONTENT_PARENT == ids && t.ID_CONTENT_CHILD == cids)).FirstOrDefault<tbl_content_link>();
                if (entity != null)
                {
                    this.db.tbl_content_link.Remove(entity);
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public ActionResult ticker_action(string id)
        {
            Convert.ToInt32(id);
            this.ViewData["ticker"] = (object)new addCMS_CategoryModel().get_ticker(Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION));
            return (ActionResult)this.View();
        }

        public ActionResult add_ticker(string id)
        {
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            new addCMS_CategoryModel().add_ticker(new ticker()
            {
                Id_org = int32_2,
                Id_creator = int32_1,
                status = "A",
                update_date = DateTime.Now,
                ticker_news = this.Request.Form["ticker_news"].ToString(),
                background_color = this.Request.Form["back_new"].ToString(),
                font_color = this.Request.Form["font_new"].ToString()
            });
            return (ActionResult)this.RedirectToAction("ticker_action");
        }

        public ActionResult edit_ticker(string id)
        {
            int int32_1 = Convert.ToInt32(id);
            int int32_2 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            new addCMS_CategoryModel().edit_ticker(new ticker()
            {
                Id_org = int32_2,
                Id_creator = int32_1,
                status = "A",
                update_date = DateTime.Now,
                Id_ticker = Convert.ToInt32(this.Request.Form["id_ticker"].ToString()),
                ticker_news = this.Request.Form["ticker_news_edit"].ToString(),
                background_color = this.Request.Form["back"].ToString(),
                font_color = this.Request.Form["font"].ToString()
            });
            return (ActionResult)this.RedirectToAction("ticker_action");
        }

        public ActionResult delete_ticker()
        {
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            new addCMS_CategoryModel().delete_ticker(Convert.ToInt32(this.Request.Form["id"].ToString()));
            return (ActionResult)this.RedirectToAction("ticker_action");
        }

        public ActionResult NonDisclosureContent()
        {
            this.ViewData["content"] = (object)new BuisinessLogic().getContent(Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION));
            return (ActionResult)this.View();
        }

        public ActionResult EditNonDisclosureContent()
        {
            this.ViewData["content"] = (object)new BuisinessLogic().getContent(Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION));
            return (ActionResult)this.View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNonDisclosureContentAction(string t_title, string content_answer)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32 = Convert.ToInt32(content.id_ORGANIZATION);
            new BuisinessLogic().UpdateContent(new tbl_non_disclousure_clause_content()
            {
                content_title = t_title,
                content = content_answer,
                id_creator = Convert.ToInt32(content.ID_USER),
                updated_date_time = DateTime.Now,
                id_org = int32
            });
            return (ActionResult)this.RedirectToAction("NonDisclosureContent");
        }

        public ActionResult AddNonDisclosureContent()
        {
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            return (ActionResult)this.View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNonDisclosureContentAction(string t_title, string content_answer)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(content.id_ORGANIZATION);
            new BuisinessLogic().AddContent(new tbl_non_disclousure_clause_content()
            {
                content_title = t_title,
                content = content_answer,
                id_creator = Convert.ToInt32(content.ID_USER),
                id_org = Convert.ToInt32(content.id_ORGANIZATION),
                updated_date_time = DateTime.Now
            });
            return (ActionResult)this.RedirectToAction("NonDisclosureContent");
        }

        public ActionResult ActivateNonDisclosureContent()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            new BuisinessLogic().ActivateNonDisclosureContent(Convert.ToInt32(content.id_ORGANIZATION), Convert.ToInt32(content.ID_USER), "A");
            return (ActionResult)this.RedirectToAction("NonDisclosureContent");
        }

        public ActionResult DeactivateNonDisclosureContent()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            new BuisinessLogic().ActivateNonDisclosureContent(Convert.ToInt32(content.id_ORGANIZATION), Convert.ToInt32(content.ID_USER), "D");
            return (ActionResult)this.RedirectToAction("NonDisclosureContent");
        }
    }
}
