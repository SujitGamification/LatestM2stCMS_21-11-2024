// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.ReportingController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Configuration;

namespace m2ostnext.Controllers
{
    public class ReportingController : Controller
    {
        public string VideoContentDBKey = ConfigurationManager.AppSettings["VideoContentDBKey"].ToString();
        private db_m2ostEntities db = new db_m2ostEntities();
        private string[] monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

        public ActionResult Index() => (ActionResult)this.View();

        public ActionResult Likes()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = " Content Like Report";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            this.ViewData["functions"] = (object)list1;
            this.ViewData["roleList"] = (object)list2;
            return (ActionResult)this.View();
        }

        public string getContentLikeReport(string rid, string fid, string stdate, string endate)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and a.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and a.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i') ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            List<ContentLike> contentLikes = new ContentReportModel().getContentLikes("SELECT count(a.id_content) COUNTER,a.id_content,date(b.EXPIRY_DATE) EXPIRY_DATE, b.CONTENT_QUESTION,sum(case when choice = 1 then 1 else 0 end) LikeCount,sum(case when choice = 0 then 1 else 0 end) DisLikeCount,MAX(a.UPDATED_DATE_TIME) LASTACCESS from tbl_report_content a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32 + " " + str1 + str2 + str3 + str4 + " group by a.id_content order by COUNTER desc limit 100");
            string str5 = "";
            if (contentLikes.Count > 0)
            {
                foreach (ContentLike contentLike in contentLikes)
                {
                    str5 += "<tr>";
                    str5 = str5 + "<td>" + (object)contentLike.ID_CONTENT + "</td>";
                    str5 = str5 + "<td>" + contentLike.CONTENT + "</td>";
                    str5 = str5 + "<td>" + contentLike.ENDDATE + "</td>";
                    str5 = str5 + "<td>" + (object)contentLike.LIKES + "</td>";
                    str5 = str5 + "<td>" + (object)contentLike.DISLIKES + "</td>";
                    str5 = str5 + "<td>" + (object)contentLike.CONTENTACCESS + "</td>";
                    str5 = str5 + "<td>" + contentLike.LASTACCESS + "</td>";
                    str5 += "</tr>";
                }
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>Content</td><td>Expirt Date</td><td>Like</td><td>Dislike</td><td>Totle Count</td><td>Last Activity</td></tr>" + "</thead><tbody>" + str5 + "</tbody></table>";
        }

        public ActionResult ContentAccess()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = " Content Access Report";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            this.ViewData["functions"] = (object)list1;
            this.ViewData["roleList"] = (object)list2;
            return (ActionResult)this.View();
        }

        public string getContentAccessReport(string rid, string fid, string stdate, string endate)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and a.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and a.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            List<ContentLike> contentAccess = new ContentReportModel().getContentAccess("SELECT count(a.id_content) COUNTER,a.id_content,date(b.EXPIRY_DATE) EXPIRY_DATE, b.CONTENT_QUESTION,MAX(a.UPDATED_DATE_TIME) LASTACCESS from tbl_content_counters a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32 + " " + str1 + str2 + str3 + str4 + " group by a.id_content order by COUNTER desc limit 100");
            string str5 = "";
            if (contentAccess.Count > 0)
            {
                foreach (ContentLike contentLike in contentAccess)
                {
                    str5 += "<tr>";
                    str5 = str5 + "<td>" + (object)contentLike.ID_CONTENT + "</td>";
                    str5 = str5 + "<td>" + contentLike.CONTENT + "</td>";
                    str5 = str5 + "<td>" + contentLike.ENDDATE + "</td>";
                    str5 = str5 + "<td>" + (object)contentLike.CONTENTACCESS + "</td>";
                    str5 = str5 + "<td>" + contentLike.LASTACCESS + "</td>";
                    str5 += "</tr>";
                }
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>Content</td><td>Expirt Date</td><td># of Times Accessed</td><td>Last Activity</td></tr>" + "</thead><tbody>" + str5 + "</tbody></table>";
        }

        public string getMonthWiseContentLikeReport(
          string rid,
          string fid,
          string stdate,
          string endate)
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and a.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and a.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            string str5 = "SELECT count(a.id_content) COUNTER,a.id_content,date(b.EXPIRY_DATE)EXPIRY_DATE, b.CONTENT_QUESTION,sum(case when choice = 1 then 1 else 0 end) LikeCount,sum(case when choice = 0 then 1 else 0 end) DisLikeCount,MAX(a.UPDATED_DATE_TIME) LASTACCESS from tbl_report_content a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + str4 + " group by a.id_content order by COUNTER desc limit 100";
            List<List<int>> intListList = new List<List<int>>();
            DateTime dateTime = new Utility().StringToDatetime(stdate);
            DateTime datetime = new Utility().StringToDatetime(endate);
            int num1 = dateTime.Day;
            for (; dateTime < datetime; dateTime = dateTime.AddMonths(1))
            {
                List<int> intList = new List<int>();
                int int32_2 = Convert.ToInt32(dateTime.ToString("MM"));
                int int32_3 = Convert.ToInt32(dateTime.ToString("yyyy"));
                int day = DateTime.DaysInMonth(int32_3, int32_2);
                intList.Add(int32_2);
                intList.Add(int32_3);
                intList.Add(num1);
                intList.Add(day);
                intListList.Add(intList);
                num1 = 1;
                dateTime = new DateTime(dateTime.Year, dateTime.Month, day);
            }
            intListList.Add(new List<int>()
      {
        datetime.Month,
        datetime.Year,
        1,
        datetime.Day
      });
            List<ContentLike> contentAccess = new ContentReportModel().getContentAccess(str5);
            string str6 = "";
            if (contentAccess.Count > 0)
            {
                foreach (ContentLike contentLike in contentAccess)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + (object)contentLike.ID_CONTENT + "</td>";
                    str6 = str6 + "<td>" + contentLike.CONTENT + "</td>";
                    str6 = str6 + "<td>" + contentLike.ENDDATE + "</td>";
                    str6 = str6 + "<td>" + (object)contentLike.CONTENTACCESS + "</td>";
                    str6 = str6 + "<td>" + contentLike.LASTACCESS + "</td>";
                    foreach (List<int> intList in intListList)
                    {
                        int num2 = intList[3];
                        MonthData contentCount = new ContentReportModel().getContentCount("SELECT sum(case when choice = 1 then 1 else 0 end) LIKES,sum(case when choice = 0 then 1 else 0 end) DISLIKES from tbl_report_content a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32_1 + " " + str1 + str2 + " and a.id_content=" + (object)contentLike.ID_CONTENT + " and  a.UPDATED_DATE_TIME >= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[2] + "-" + (object)intList[0] + " 00,01', '%Y-%d-%m %H,%i') and  a.UPDATED_DATE_TIME <= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[0] + "-" + (object)num2 + " 23,59', '%Y-%m-%d %H,%i')");
                        str6 = str6 + "<td>" + (object)contentCount.LIKES + "/" + (object)contentCount.DISLIKES + "</td>";
                    }
                    str6 += "</tr>";
                }
            }
            string str7 = "<table id=\"report-table-month\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>Content</td><td>Expirt Date</td><td># of Times Accessed</td><td>Last Activity</td>";
            foreach (List<int> intList in intListList)
                str7 = str7 + "<td>" + this.monthNames[intList[0] - 1] + "-" + (object)intList[1] + "</td>";
            return str7 + "</tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        public string getMonthWiseContentAccessReport(
          string rid,
          string fid,
          string stdate,
          string endate)
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and a.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and a.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            string str5 = "SELECT count(a.id_content) COUNTER,a.id_content,date(b.EXPIRY_DATE) EXPIRY_DATE, b.CONTENT_QUESTION,MAX(a.UPDATED_DATE_TIME) LASTACCESS from tbl_content_counters a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + str4 + " group by a.id_content order by COUNTER desc limit 100";
            List<List<int>> intListList = new List<List<int>>();
            DateTime dateTime = new Utility().StringToDatetime(stdate);
            DateTime datetime = new Utility().StringToDatetime(endate);
            int num1 = dateTime.Day;
            for (; dateTime < datetime; dateTime = dateTime.AddMonths(1))
            {
                List<int> intList = new List<int>();
                int int32_2 = Convert.ToInt32(dateTime.ToString("MM"));
                int int32_3 = Convert.ToInt32(dateTime.ToString("yyyy"));
                int day = DateTime.DaysInMonth(int32_3, int32_2);
                intList.Add(int32_2);
                intList.Add(int32_3);
                intList.Add(num1);
                intList.Add(day);
                intListList.Add(intList);
                num1 = 1;
                dateTime = new DateTime(dateTime.Year, dateTime.Month, day);
            }
            intListList.Add(new List<int>()
      {
        datetime.Month,
        datetime.Year,
        1,
        datetime.Day
      });
            List<ContentLike> contentAccess = new ContentReportModel().getContentAccess(str5);
            string str6 = "";
            if (contentAccess.Count > 0)
            {
                foreach (ContentLike contentLike in contentAccess)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + contentLike.CONTENT + "</td>";
                    str6 = str6 + "<td>" + contentLike.ENDDATE + "</td>";
                    str6 = str6 + "<td>" + (object)contentLike.CONTENTACCESS + "</td>";
                    str6 = str6 + "<td>" + contentLike.LASTACCESS + "</td>";
                    foreach (List<int> intList in intListList)
                    {
                        int num2 = intList[3];
                        MonthData contentCount = new ContentReportModel().getContentCount("SELECT count(a.id_content) LIKES,'0' DISLIKES from tbl_content_counters a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32_1 + " " + str1 + str2 + " and a.id_content=" + (object)contentLike.ID_CONTENT + " and  a.UPDATED_DATE_TIME >= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[2] + "-" + (object)intList[0] + " 00,01', '%Y-%d-%m %H,%i') and  a.UPDATED_DATE_TIME <= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[0] + "-" + (object)num2 + " 23,59', '%Y-%m-%d %H,%i')" + " group by a.id_content order by LIKES desc limit 100");
                        str6 = str6 + "<td>" + (object)contentCount.LIKES + "</td>";
                    }
                    str6 += "</tr>";
                }
            }
            string str7 = "<table id=\"report-table-month\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content</td><td>Expirt Date</td><td># of Times Accessed</td><td>Last Activity</td>";
            foreach (List<int> intList in intListList)
                str7 = str7 + "<td>" + this.monthNames[intList[0] - 1] + "-" + (object)intList[1] + "</td>";
            return str7 + "</tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        public string getLocationWiseContentAccessReport(
          string rid,
          string fid,
          string stdate,
          string endate)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and a.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and a.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            string str5 = "SELECT count(a.id_content) COUNTER,a.id_content,date(b.EXPIRY_DATE) EXPIRY_DATE, b.CONTENT_QUESTION,MAX(a.UPDATED_DATE_TIME) LASTACCESS from tbl_content_counters a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32 + " " + str1 + str2 + str3 + str4 + " group by a.id_content order by COUNTER desc limit 100";
            List<List<int>> intListList = new List<List<int>>();
            string lAdd = "";
            if (rid != "0")
                lAdd = " and id_csst_role=" + rid + " ";

            lAdd += "select Distinct LOCATION from tbl_profile where id_user in (select id_user from tbl_role_user_mapping where id_organization=" + (object)int32 + lAdd + ")";
            List<string> locationList = new ContentReportModel().getLocationList(int32, lAdd);
            string str6 = "";
            if (locationList.Count > 0)
            {
                foreach (string str7 in locationList)
                {
                    foreach (tbl_user tblUser in this.db.tbl_user.SqlQuery("select * from tbl_user where id_user in (select id_user from tbl_role_user_mapping where id_organization=" + (object)int32 + lAdd + ") and id_user in (select id_user from tbl_profile where lower(location) like lower('" + str7 + "'))").ToList<tbl_user>())
                        ;
                }
                str6 += "<tr>";
            }
            List<ContentLike> contentAccess = new ContentReportModel().getContentAccess(str5);
            if (contentAccess.Count > 0)
            {
                foreach (ContentLike contentLike in contentAccess)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + contentLike.CONTENT + "</td>";
                    str6 = str6 + "<td>" + contentLike.ENDDATE + "</td>";
                    str6 = str6 + "<td>" + (object)contentLike.CONTENTACCESS + "</td>";
                    str6 = str6 + "<td>" + contentLike.LASTACCESS + "</td>";
                    str6 += "</tr>";
                }
            }
            return "<table id=\"report-table-month\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content</td><td>Expirt Date</td><td># of Times Accessed</td><td>Last Activity</td>" + "</tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        public ActionResult LocationReport()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Content Access Report - By Location";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            this.ViewData["functions"] = (object)list1;
            this.ViewData["roleList"] = (object)list2;
            return (ActionResult)this.View();
        }

        public ActionResult GenderReport()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Content Access Report - By Gender";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            List<string> list3 = this.db.tbl_profile.SqlQuery("select * from tbl_profile where id_user in (select id_user from tbl_user where id_organization=" + (object)orgid + ") group by location").ToList<tbl_profile>().Select<tbl_profile, string>((Func<tbl_profile, string>)(p => p.LOCATION)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            this.ViewData["functions"] = (object)list1;
            this.ViewData["roleList"] = (object)list2;
            this.ViewData["location"] = (object)list3;
            return (ActionResult)this.View();
        }

        public ActionResult DesignationReport()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Content Access Report - By Designation";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            List<string> list3 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_designation)).Distinct<string>().ToList<string>();
            this.ViewData["functions"] = (object)list1;
            this.ViewData["roleList"] = (object)list2;
            this.ViewData["designation"] = (object)list3;
            return (ActionResult)this.View();
        }

        public string getContentDesignationReport(
          string rid,
          string fid,
          string lid,
          string stdate,
          string endate)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (rid != "0")
                str1 = " and c.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (lid != "ALL")
                str3 = " and lower(B.DESIGNATION) like lower('" + lid + "') ";
            if (!string.IsNullOrEmpty(stdate))
                str4 = " and C.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str5 = " and C.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            List<ContentLocationGenderWise> designationAccess = new ContentReportModel().getDesignationAccess("SELECT d.id_content,d.CONTENT_QUESTION,A.user_designation DESIGNATION,COUNT(C.id_content) AS Contentaccess FROM tbl_user A,tbl_profile B,tbl_content_counters C,tbl_content D WHERE c.id_content = d.id_content AND A.id_user = B.id_user AND A.id_user = C.id_user AND A.id_organization = " + (object)int32 + " " + str1 + str2 + str3 + str4 + str5 + " group by A.user_designation,C.id_content order by id_content ,Contentaccess desc limit 20");
            string str6 = "";
            if (designationAccess.Count > 0)
            {
                foreach (ContentLocationGenderWise locationGenderWise in designationAccess)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + (object)locationGenderWise.ID_CONTENT + "</td>";
                    str6 = str6 + "<td>" + locationGenderWise.CONTENT_QUESTION + "</td>";
                    str6 = str6 + "<td>" + locationGenderWise.DESIGNATION + "</td>";
                    str6 = str6 + "<td>" + (object)locationGenderWise.CONTENTACCESS + "</td>";
                    str6 += "</tr>";
                }
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>Content Name</td><td>Designation</td><td># of Content Accessed</td></tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        public string getContentLocationGenderReport(
          string rid,
          string fid,
          string lid,
          string stdate,
          string endate)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (rid != "0")
                str1 = " and c.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower ('" + fid + "')";
            if (lid != "ALL")
                str3 = " and lower(B.LOCATION) like lower ('" + lid + "') ";
            if (!string.IsNullOrEmpty(stdate))
                str4 = " and C.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str5 = " and C.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            List<ContentLocationGenderWise> contentLocationGender = new ContentReportModel().getContentLocationGender("SELECT d.id_content,d.CONTENT_QUESTION,SUM(CASE WHEN B.gender = 'M' THEN 1 ELSE 0 END) MALE, SUM(CASE  WHEN B.gender = 'F' THEN 1    ELSE 0  END) FEMALE,COUNT(C.id_content) AS Contentaccess FROM tbl_user A,tbl_profile B,tbl_content_counters C,tbl_content D WHERE c.id_content = d.id_content AND A.id_user = B.id_user AND A.id_user = C.id_user AND A.id_organization = " + (object)int32 + " " + str1 + str2 + str3 + str4 + str5 + "group by C.id_content order by Contentaccess desc limit 20");
            string str6 = "";
            if (contentLocationGender.Count > 0)
            {
                foreach (ContentLocationGenderWise locationGenderWise in contentLocationGender)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + (object)locationGenderWise.ID_CONTENT + "</td>";
                    str6 = str6 + "<td>" + locationGenderWise.CONTENT_QUESTION + "</td>";
                    str6 = str6 + "<td>" + (object)locationGenderWise.MALE + "</td>";
                    str6 = str6 + "<td>" + (object)locationGenderWise.FEMALE + "</td>";
                    str6 = str6 + "<td>" + (object)locationGenderWise.CONTENTACCESS + "</td>";
                    str6 += "</tr>";
                }
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>Content Name</td><td>Male</td><td>Female</td><td># of Content Accessed</td></tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        public string getContentLocationReport(string rid, string fid, string stdate, string endate)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and c.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and C.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and C.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            List<ContentLocationWise> contentLocation = new ContentReportModel().getContentLocation("SELECT A.id_user,B.FIRSTNAME,B.LASTNAME, A.USERID, count(C.id_content_counters) as Contentaccess ,B.LOCATION FROM tbl_user A, tbl_profile B, tbl_content_counters C WHERE A.id_user = B.id_user AND A.id_user = C.id_user and A.id_organization = " + (object)int32 + " " + str1 + str2 + str3 + str4 + "group by A.id_user order by B.LOCATION, Contentaccess desc limit 100");
            string str5 = "";
            if (contentLocation.Count > 0)
            {
                foreach (ContentLocationWise contentLocationWise in contentLocation)
                {
                    str5 += "<tr>";
                    str5 = str5 + "<td>" + contentLocationWise.LOCATION + "</td>";
                    str5 = str5 + "<td>" + contentLocationWise.FIRSTNAME + " " + contentLocationWise.LASTNAME + "(" + contentLocationWise.USERID + "-" + (object)contentLocationWise.ID_USER + ")</td>";
                    str5 = str5 + "<td>" + (object)contentLocationWise.CONTENTACCESS + "</td>";
                    str5 += "</tr>";
                }
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Location</td><td>User</td><td># of Content Accessed</td></tr>" + "</thead><tbody>" + str5 + "</tbody></table>";
        }

        public string getMonthWiseContentDesignationReport(
          string rid,
          string fid,
          string lid,
          string stdate,
          string endate)
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (rid != "0")
                str1 = " and c.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (lid != "ALL")
                str3 = " and lower(B.DESIGNATION) = lower('" + lid + "') ";
            if (!string.IsNullOrEmpty(stdate))
                str4 = " and C.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str5 = " and C.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            string str6 = "SELECT d.id_content,d.CONTENT_QUESTION,COUNT(C.id_content) AS ContentAccess,A.user_designation DESIGNATION FROM tbl_user A,tbl_profile B,tbl_content_counters C,tbl_content D WHERE c.id_content = d.id_content AND A.id_user = B.id_user AND A.id_user = C.id_user AND A.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + str4 + str5 + " group by A.user_designation,C.id_content order by id_content ,Contentaccess desc ";
            List<List<int>> intListList = new List<List<int>>();
            DateTime dateTime = new Utility().StringToDatetime(stdate);
            DateTime datetime = new Utility().StringToDatetime(endate);
            int num1 = dateTime.Day;
            for (; dateTime < datetime; dateTime = dateTime.AddMonths(1))
            {
                List<int> intList = new List<int>();
                int int32_2 = Convert.ToInt32(dateTime.ToString("MM"));
                int int32_3 = Convert.ToInt32(dateTime.ToString("yyyy"));
                int day = DateTime.DaysInMonth(int32_3, int32_2);
                intList.Add(int32_2);
                intList.Add(int32_3);
                intList.Add(num1);
                intList.Add(day);
                intListList.Add(intList);
                num1 = 1;
                dateTime = new DateTime(dateTime.Year, dateTime.Month, day);
            }
            intListList.Add(new List<int>()
      {
        datetime.Month,
        datetime.Year,
        1,
        datetime.Day
      });
            List<ContentLocationGenderWise> designationAccess = new ContentReportModel().getDesignationAccess(str6);
            string str7 = "";
            if (designationAccess.Count > 0)
            {
                foreach (ContentLocationGenderWise locationGenderWise in designationAccess)
                {
                    str7 += "<tr>";
                    str7 = str7 + "<td>" + (object)locationGenderWise.ID_CONTENT + "</td>";
                    str7 = str7 + "<td>" + locationGenderWise.CONTENT_QUESTION + "</td>";
                    str7 = str7 + "<td>" + locationGenderWise.DESIGNATION + "</td>";
                    str7 = str7 + "<td>" + (object)locationGenderWise.CONTENTACCESS + "</td>";
                    foreach (List<int> intList in intListList)
                    {
                        int num2 = intList[3];
                        MonthData locationCount = new ContentReportModel().getLocationCount("SELECT d.id_content,d.CONTENT_QUESTION,COUNT(C.id_content) AS ContentAccess,A.user_designation DESIGNATION FROM tbl_user A,tbl_profile B,tbl_content_counters C,tbl_content D WHERE c.id_content = d.id_content AND A.id_user = B.id_user AND A.id_user = C.id_user AND A.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + " and upper(A.user_designation) ='" + locationGenderWise.DESIGNATION.ToUpper() + "' and c.id_content=" + (object)locationGenderWise.ID_CONTENT + " and  C.UPDATED_DATE_TIME >= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[2] + "-" + (object)intList[0] + " 00,01', '%Y-%d-%m %H,%i') and  C.UPDATED_DATE_TIME <= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[0] + "-" + (object)num2 + " 23,59', '%Y-%m-%d %H,%i')");
                        str7 = str7 + "<td>" + (object)locationCount.LIKES + "</td>";
                    }
                    str7 += "</tr>";
                }
            }
            string str8 = "<table id=\"report-table-month\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>Content Name</td><td>Designation</td><td># of Times Accessed</td>";
            foreach (List<int> intList in intListList)
                str8 = str8 + "<td>" + this.monthNames[intList[0] - 1] + "-" + (object)intList[1] + "</td>";
            return str8 + "</tr>" + "</thead><tbody>" + str7 + "</tbody></table>";
        }

        public string getMonthWiseContentLocationGenderReport(
          string rid,
          string fid,
          string lid,
          string stdate,
          string endate)
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (rid != "0")
                str1 = " and c.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (lid != "ALL")
                str3 = " and lower(B.LOCATION) like lower('" + lid + "') ";
            if (!string.IsNullOrEmpty(stdate))
                str4 = " and C.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str5 = " and C.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            string str6 = "SELECT d.id_content,d.CONTENT_QUESTION,SUM(CASE WHEN B.gender = 'M' THEN 1 ELSE 0 END) MALE, SUM(CASE  WHEN B.gender = 'F' THEN 1    ELSE 0  END) FEMALE,COUNT(C.id_content) AS Contentaccess FROM tbl_user A,tbl_profile B,tbl_content_counters C,tbl_content D WHERE c.id_content = d.id_content AND A.id_user = B.id_user AND A.id_user = C.id_user AND A.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + str4 + str5 + "group by C.id_content order by Contentaccess desc limit 20";
            List<List<int>> intListList = new List<List<int>>();
            DateTime dateTime = new Utility().StringToDatetime(stdate);
            DateTime datetime = new Utility().StringToDatetime(endate);
            int num1 = dateTime.Day;
            for (; dateTime < datetime; dateTime = dateTime.AddMonths(1))
            {
                List<int> intList = new List<int>();
                int int32_2 = Convert.ToInt32(dateTime.ToString("MM"));
                int int32_3 = Convert.ToInt32(dateTime.ToString("yyyy"));
                int day = DateTime.DaysInMonth(int32_3, int32_2);
                intList.Add(int32_2);
                intList.Add(int32_3);
                intList.Add(num1);
                intList.Add(day);
                intListList.Add(intList);
                num1 = 1;
                dateTime = new DateTime(dateTime.Year, dateTime.Month, day);
            }
            intListList.Add(new List<int>()
      {
        datetime.Month,
        datetime.Year,
        1,
        datetime.Day
      });
            List<ContentLocationGenderWise> locationGenderAccess = new ContentReportModel().getLocationGenderAccess(str6);
            string str7 = "";
            if (locationGenderAccess.Count > 0)
            {
                foreach (ContentLocationGenderWise locationGenderWise in locationGenderAccess)
                {
                    str7 += "<tr>";
                    str7 = str7 + "<td>" + (object)locationGenderWise.ID_CONTENT + "</td>";
                    str7 = str7 + "<td>" + locationGenderWise.CONTENT_QUESTION + "</td>";
                    str7 = str7 + "<td>" + (object)locationGenderWise.MALE + "</td>";
                    str7 = str7 + "<td>" + (object)locationGenderWise.FEMALE + "</td>";
                    str7 = str7 + "<td>" + (object)locationGenderWise.CONTENTACCESS + "</td>";
                    foreach (List<int> intList in intListList)
                    {
                        int num2 = intList[3];
                        MonthData locationGenderCount = new ContentReportModel().getLocationGenderCount("SELECT d.id_content,d.CONTENT_QUESTION,SUM(CASE WHEN B.gender = 'M' THEN 1 ELSE 0 END) MALE,SUM(CASE WHEN B.gender='F' THEN 1 ELSE 0 END) FEMALE,COUNT(C.id_content) AS Contentaccess FROM tbl_user A, tbl_profile B, tbl_content_counters C, tbl_content D WHERE c.id_content = d.id_content AND A.id_user = B.id_user AND A.id_user = C.id_user AND A.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + " and c.id_content=" + (object)locationGenderWise.ID_CONTENT + " and  C.UPDATED_DATE_TIME >= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[2] + "-" + (object)intList[0] + " 00,01', '%Y-%d-%m %H,%i') and  C.UPDATED_DATE_TIME <= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[0] + "-" + (object)num2 + " 23,59', '%Y-%m-%d %H,%i')");
                        str7 = str7 + "<td>" + (object)locationGenderCount.LIKES + "/" + (object)locationGenderCount.DISLIKES + "</td>";
                    }
                    str7 += "</tr>";
                }
            }
            string str8 = "<table id=\"report-table-month\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>Content Name</td><td>Male</td><td>Female</td><td># of Times Accessed</td>";
            foreach (List<int> intList in intListList)
                str8 = str8 + "<td>" + this.monthNames[intList[0] - 1] + "-" + (object)intList[1] + "</td>";
            return str8 + "</tr>" + "</thead><tbody>" + str7 + "</tbody></table>";
        }

        public string getMonthWiseContentLocationReport(
          string rid,
          string fid,
          string stdate,
          string endate)
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and c.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and C.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and C.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            string str5 = "SELECT A.id_user,B.FIRSTNAME,B.LASTNAME, A.USERID, count(C.id_content_counters) as Contentaccess ,B.LOCATION FROM tbl_user A, tbl_profile B, tbl_content_counters C WHERE A.id_user = B.id_user AND A.id_user = C.id_user and A.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + str4 + " group by A.id_user order by B.LOCATION, Contentaccess desc limit 100";
            List<List<int>> intListList = new List<List<int>>();
            DateTime dateTime = new Utility().StringToDatetime(stdate);
            DateTime datetime = new Utility().StringToDatetime(endate);
            int num1 = dateTime.Day;
            for (; dateTime < datetime; dateTime = dateTime.AddMonths(1))
            {
                List<int> intList = new List<int>();
                int int32_2 = Convert.ToInt32(dateTime.ToString("MM"));
                int int32_3 = Convert.ToInt32(dateTime.ToString("yyyy"));
                int day = DateTime.DaysInMonth(int32_3, int32_2);
                intList.Add(int32_2);
                intList.Add(int32_3);
                intList.Add(num1);
                intList.Add(day);
                intListList.Add(intList);
                num1 = 1;
                dateTime = new DateTime(dateTime.Year, dateTime.Month, day);
            }
            intListList.Add(new List<int>()
      {
        datetime.Month,
        datetime.Year,
        1,
        datetime.Day
      });
            List<ContentLocationWise> locationAccess = new ContentReportModel().getLocationAccess(str5);
            string str6 = "";
            if (locationAccess.Count > 0)
            {
                foreach (ContentLocationWise contentLocationWise in locationAccess)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + contentLocationWise.LOCATION + "</td>";
                    str6 = str6 + "<td>" + contentLocationWise.FIRSTNAME + " " + contentLocationWise.LASTNAME + "(" + contentLocationWise.USERID + ")</td>";
                    str6 = str6 + "<td>" + (object)contentLocationWise.CONTENTACCESS + "</td>";
                    foreach (List<int> intList in intListList)
                    {
                        int num2 = intList[3];
                        MonthData locationCount = new ContentReportModel().getLocationCount("SELECT count(C.id_content_counters) ContentAccess FROM tbl_user A, tbl_profile B, tbl_content_counters C WHERE A.id_user = B.id_user AND A.id_user = C.id_user and A.id_organization = " + (object)int32_1 + " " + str1 + str2 + " and C.id_user=" + (object)contentLocationWise.ID_USER + " and  C.UPDATED_DATE_TIME >= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[2] + "-" + (object)intList[0] + " 00,01', '%Y-%d-%m %H,%i') and  C.UPDATED_DATE_TIME <= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[0] + "-" + (object)num2 + " 23,59', '%Y-%m-%d %H,%i')");
                        str6 = str6 + "<td>" + (object)locationCount.LIKES + "</td>";
                    }
                    str6 += "</tr>";
                }
            }
            string str7 = "<table id=\"report-table-month\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Location</td><td>User</td><td># of Content Accessed</td>";
            foreach (List<int> intList in intListList)
                str7 = str7 + "<td>" + this.monthNames[intList[0] - 1] + "-" + (object)intList[1] + "</td>";
            return str7 + "</tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        //Rani Add this functinality date:6/6/23.............
        public ActionResult ContentUserReport()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Content Assign To User List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            List<string> list3 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_designation)).Distinct<string>().ToList<string>();
            this.ViewData["functions"] = (object)list1;
            this.ViewData["roleList"] = (object)list2;
            this.ViewData["designation"] = (object)list3;
            return (ActionResult)this.View();
        }
        public string getContentUserReport()
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";


            str1 = "select (select USERID from tbl_user where id_user=a.id_user) as USERID, a.id_content,a.start_date,a.expiry_date,a.UPDATED_DATE_TIME,a.STATUS,(select ID_CATEGORY from tbl_category where ID_CATEGORY=a.ID_CATEGORY)as ID_CATEGORY,(select CATEGORYNAME from tbl_category where ID_CATEGORY=a.ID_CATEGORY)as  CATEGORYNAME,(select DESCRIPTION from tbl_category where ID_CATEGORY=a.ID_CATEGORY)as DESCRIPTION from tbl_content_user_assisgnment as a  where a.id_organization=" + int32 + " order by a.UPDATED_DATE_TIME desc";

            List<ContentUserModel> ContentUserList = new ContentReportModel().getContentUser(str1);
            string str6 = "";
            if (ContentUserList.Count > 0)
            {
                foreach (ContentUserModel item in ContentUserList)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + (object)item.USERID + "</td>";
                    str6 = str6 + "<td>" + (object)item.id_content + "</td>";
                    str6 = str6 + "<td>" + (object)item.start_date + "</td>";
                    str6 = str6 + "<td>" + (object)item.expiry_date + "</td>";
                    str6 = str6 + "<td>" + (object)item.UPDATED_DATE_TIME + "</td>";
                    str6 = str6 + "<td>" + (object)item.STATUS + "</td>";
                    str6 = str6 + "<td>" + (object)item.ID_CATEGORY + "</td>";
                    str6 = str6 + "<td>" + (object)item.CATEGORYNAME + "</td>";
                    str6 = str6 + "<td>" + (object)item.DESCRIPTION + "</td>";
                    str6 += "</tr>";
                }
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>USERID</td><td>id_content</td><td>start_date</td><td>expiry_date</td><td>UPDATED_DATE_TIME</td><td>STATUS</td><td>ID_CATEGORY</td><td>CATEGORYNAME</td><td>DESCRIPTION</td></tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        public ActionResult ContentAccessDetail()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = " Content Access Report - Detailed";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();
            this.ViewData["functions"] = (object)list1;
            this.ViewData["roleList"] = (object)list2;
            return (ActionResult)this.View();
        }

        public string getContentAccessDetailReport(string rid, string fid, string stdate, string endate)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and a.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and a.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            List<ContentDetailAccess> contentAccess = new ContentReportModel().getContentDetailAccess("SELECT c.USERID, d.FIRSTNAME,a.id_content, b.CONTENT_QUESTION,a.UPDATED_DATE_TIME LASTACCESS from tbl_content_counters a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32 + " " + str1 + str2 + str3 + str4);
            string str5 = "";
            if (contentAccess.Count > 0)
            {
                foreach (ContentDetailAccess contentLike in contentAccess)
                {
                    str5 += "<tr>";
                    str5 = str5 + "<td>" + (object)contentLike.id_content + "</td>";
                    str5 = str5 + "<td>" + contentLike.USERID + "</td>";
                    str5 = str5 + "<td>" + contentLike.FIRSTNAME + "</td>";
                    str5 = str5 + "<td>" + (object)contentLike.CONTENT_QUESTION + "</td>";
                    str5 = str5 + "<td>" + contentLike.LASTACCESS + "</td>";
                    str5 += "</tr>";
                }
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content No</td><td>USERID</td><td>Name</td><td>Content Question</td><td>Activity</td></tr>" + "</thead><tbody>" + str5 + "</tbody></table>";

        }
        public string getMonthWiseContentDetReport(
        string rid,
        string fid,
        string stdate,
        string endate)
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (rid != "0")
                str1 = " and a.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            if (fid != "ALL")
                str2 = " and lower(user_function) like lower('" + fid + "')";
            if (!string.IsNullOrEmpty(stdate))
                str3 = " and a.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                str4 = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            string str5 = "SELECT c.USERID, d.FIRSTNAME,a.id_content, b.CONTENT_QUESTION,a.UPDATED_DATE_TIME LASTACCESS from tbl_content_counters a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32_1 + " " + str1 + str2 + str3 + str4;
            List<List<int>> intListList = new List<List<int>>();
            DateTime dateTime = new Utility().StringToDatetime(stdate);
            DateTime datetime = new Utility().StringToDatetime(endate);
            int num1 = dateTime.Day;
            for (; dateTime < datetime; dateTime = dateTime.AddMonths(1))
            {
                List<int> intList = new List<int>();
                int int32_2 = Convert.ToInt32(dateTime.ToString("MM"));
                int int32_3 = Convert.ToInt32(dateTime.ToString("yyyy"));
                int day = DateTime.DaysInMonth(int32_3, int32_2);
                intList.Add(int32_2);
                intList.Add(int32_3);
                intList.Add(num1);
                intList.Add(day);
                intListList.Add(intList);
                num1 = 1;
                dateTime = new DateTime(dateTime.Year, dateTime.Month, day);
            }
            intListList.Add(new List<int>()
      {
        datetime.Month,
        datetime.Year,
        1,
        datetime.Day
      });
            List<ContentLike> contentAccess = new ContentReportModel().getContentAccess(str5);
            string str6 = "";
            if (contentAccess.Count > 0)
            {
                foreach (ContentLike contentLike in contentAccess)
                {
                    str6 += "<tr>";
                    str6 = str6 + "<td>" + contentLike.CONTENT + "</td>";
                    str6 = str6 + "<td>" + contentLike.ENDDATE + "</td>";
                    str6 = str6 + "<td>" + (object)contentLike.CONTENTACCESS + "</td>";
                    str6 = str6 + "<td>" + contentLike.LASTACCESS + "</td>";
                    foreach (List<int> intList in intListList)
                    {
                        int num2 = intList[3];
                        MonthData contentCount = new ContentReportModel().getContentCount("SELECT count(a.id_content) LIKES,'0' DISLIKES from tbl_content_counters a,tbl_content b,tbl_user c,tbl_profile d,tbl_csst_role e,tbl_role_user_mapping f where a.id_content=b.id_content and a.id_user=c.id_user and c.id_user=d.id_user  and e.id_csst_role = f.id_csst_role and f.id_user = c.ID_USER  and e.id_organization = " + (object)int32_1 + " " + str1 + str2 + " and a.id_content=" + (object)contentLike.ID_CONTENT + " and  a.UPDATED_DATE_TIME >= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[2] + "-" + (object)intList[0] + " 00,01', '%Y-%d-%m %H,%i') and  a.UPDATED_DATE_TIME <= STR_TO_DATE('" + (object)intList[1] + "-" + (object)intList[0] + "-" + (object)num2 + " 23,59', '%Y-%m-%d %H,%i')" + " group by a.id_content order by LIKES desc limit 100");
                        str6 = str6 + "<td>" + (object)contentCount.LIKES + "</td>";
                    }
                    str6 += "</tr>";
                }
            }
            string str7 = "<table id=\"report-table-month\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Content</td><td>Expirt Date</td><td># of Times Accessed</td><td>Last Activity</td>";
            foreach (List<int> intList in intListList)
                str7 = str7 + "<td>" + this.monthNames[intList[0] - 1] + "-" + (object)intList[1] + "</td>";
            return str7 + "</tr>" + "</thead><tbody>" + str6 + "</tbody></table>";
        }

        public ActionResult ContentCompletionReport()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Video Content Completion Report";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }


            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            //////List<string> list1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.ID_ORGANIZATION == (int?)orgid)).Select<tbl_user, string>((Expression<Func<tbl_user, string>>)(p => p.user_function)).Distinct<string>().ToList<string>().Where<string>((Func<string, bool>)(s => !string.IsNullOrWhiteSpace(s))).ToList<string>();
            //////List<tbl_csst_role> list2 = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_csst_role>();

            List<tbl_category> LstCategory = this.db.tbl_category.SqlQuery("select * from tbl_category where id_category in (select distinct CategoryID from " + VideoContentDBKey + ".tbl_prod_que_ans_video where OrgID=" + (object)orgid + ") AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>)(t => t.CATEGORYNAME)).ToList<tbl_category>();

            ////List<tbl_category> LstCategory = this.db.tbl_category.SqlQuery("select * from tbl_category where id_organization=" + (object)orgid + " AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>)(t => t.CATEGORYNAME)).ToList<tbl_category>();

            ////this.ViewData["ContentList"] = (object)LstContent;
            this.ViewData["CategoryList"] = (object)LstCategory;
            ////this.ViewData["roleList"] = (object)list2;
            return (ActionResult)this.View();
        }

        public string getContentCompletionReport(string catid, string contentid, string compstatusid, string stdate, string endate)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string whereClause = "";
            string str5 = "";

            if (catid != "0")
            {
                whereClause += " AND tcat.ID_CATEGORY='" + catid + "' AND vid.CategoryID='" + catid + "'";
            }

            if (contentid != "0")
            {
                whereClause += " AND tcon.ID_CONTENT='" + contentid + "' AND vid.ContentID='" + contentid + "'";
            }

            if (compstatusid != "0")
            {
                if (compstatusid == "1")
                {
                    whereClause += " AND is_completed=1";
                }

                if (compstatusid == "2")
                {
                    ////whereClause += " AND is_completed<>1";
                    whereClause += " AND tu.ID_USER NOT IN (SELECT user_id FROM " + VideoContentDBKey + ".tbl_prod_que_ans_video where is_completed=1)";
                }
            }

            if (!string.IsNullOrEmpty(stdate))
                whereClause += " and vid.created_Date >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            if (!string.IsNullOrEmpty(endate))
                whereClause += " and vid.created_Date <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";

            ////////if (rid != "0")
            ////////    str1 = " and c.id_user in (select id_user from tbl_role_user_mapping where id_csst_role=" + rid + ") ";
            ////////if (fid != "ALL")
            ////////    str2 = " and lower(user_function) like lower('" + fid + "')";
            ////////if (!string.IsNullOrEmpty(stdate))
            ////////    str3 = " and C.UPDATED_DATE_TIME >= STR_TO_DATE('" + stdate + " 00,01', '%d-%m-%Y %H,%i')  ";
            ////////if (!string.IsNullOrEmpty(endate))
            ////////    str4 = " and C.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + " 23,59', '%d-%m-%Y %H,%i') ";
            List<ContentVideoCompletion> LstAllDetails = new ContentReportModel().getContentVideoCompletionDetails("select tu.EMPLOYEEID AS EmployeeId,CONCAT(tp.FirstName,' ',tp.LastName) AS EmployeeName,CONTENT_TITLE AS ContentTitle,CATEGORYNAME AS CategoryName,CASE WHEN video_timer IS NULL THEN 'N/A' ELSE video_timer END video_timer,CASE WHEN total_video_timer IS NULL THEN 'N/A' ELSE total_video_timer END total_video_timer,CASE WHEN complete_per IS NULL THEN '0' ELSE complete_per END complete_per,CASE WHEN vid.ID IS NULL THEN 'N/A' ELSE vid.ID END ID,CASE WHEN vid.created_Date IS NULL THEN 'N/A' ELSE vid.created_Date END created_Date,CASE WHEN is_completed=1 THEN 'Completed' ELSE 'InComplete' END AS CompletionStatus FROM tbl_user tu INNER JOIN tbl_profile tp on tu.ID_USER=tp.ID_USER LEFT OUTER JOIN " + VideoContentDBKey + ".tbl_prod_que_ans_video vid on vid.user_id=tu.ID_USER LEFT OUTER JOIN tbl_content tcon on tcon.ID_CONTENT=vid.ContentID LEFT OUTER JOIN tbl_category tcat on tcat.ID_CATEGORY=vid.CategoryID AND tcat.ID_ORGANIZATION=vid.OrgID WHERE tu.STATUS='A' AND tu.id_organization=" + (object)orgid + whereClause);

            List<ContentVideoCompletion> LstUserVideoDetails = new ContentReportModel().getContentVideoCompletionDetails("SELECT tu.EMPLOYEEID AS EmployeeId,CONCAT(tp.FirstName,' ',tp.LastName) AS EmployeeName,CONTENT_TITLE AS ContentTitle,CATEGORYNAME AS CategoryName,video_timer,total_video_timer,complete_per,vid.ID,vid.created_Date,CASE WHEN is_completed=1 THEN 'Completed' ELSE 'InComplete' END AS CompletionStatus FROM tbl_user tu INNER JOIN tbl_profile tp on tu.ID_USER=tp.ID_USER LEFT OUTER JOIN " + VideoContentDBKey + ".tbl_prod_que_ans_video vid on vid.user_id=tu.ID_USER INNER JOIN (SELECT MAX(ID) AS ID FROM " + VideoContentDBKey + ".tbl_prod_que_ans_video GROUP BY ContentID,CategoryID,OrgID,user_id) vid1 on vid.ID=vid1.ID LEFT OUTER JOIN tbl_content tcon on tcon.ID_CONTENT=vid.ContentID LEFT OUTER JOIN tbl_category tcat on tcat.ID_CATEGORY=vid.CategoryID AND tcat.ID_ORGANIZATION=vid.OrgID WHERE tu.STATUS='A' AND tu.id_organization=" + (object)orgid + whereClause);

            if (LstAllDetails.Count > 0)
            {
                if (LstUserVideoDetails.Count > 0)
                {
                    for (int i = 0; i < LstUserVideoDetails.Count; i++)
                    {
                        var ID = LstUserVideoDetails[i].ID;
                        var CategoryName = LstUserVideoDetails[i].CategoryName;
                        var ContentTitle = LstUserVideoDetails[i].ContentTitle;
                        var EmployeeId = LstUserVideoDetails[i].EmployeeId;

                        LstAllDetails.RemoveAll(s => s.ID != ID && s.CategoryName == CategoryName && s.ContentTitle == ContentTitle && s.EmployeeId == EmployeeId);
                    }
                }
            }

            if (LstAllDetails.Count > 0)
            {
                foreach (ContentVideoCompletion cvc in LstAllDetails)
                {
                    str5 += "<tr>";
                    str5 = str5 + "<td>" + (cvc.EmployeeId == "" ? "N/A" : cvc.EmployeeId) + "</td>";
                    str5 = str5 + "<td>" + (cvc.EmployeeName == "" ? "N/A" : cvc.EmployeeName) + "</td>";
                    str5 = str5 + "<td>" + (cvc.ContentTitle == "" ? "N/A" : cvc.ContentTitle) + "</td>";
                    str5 = str5 + "<td>" + (cvc.CategoryName == "" ? "N/A" : cvc.CategoryName) + "</td>";
                    str5 = str5 + "<td>" + (cvc.video_timer == "" ? "N/A" : cvc.video_timer) + "</td>";
                    str5 = str5 + "<td>" + (cvc.total_video_timer == "" ? "N/A" : cvc.total_video_timer) + "</td>";
                    str5 = str5 + "<td>" + (cvc.complete_per == "" ? "N/A" : cvc.complete_per) + "</td>";
                    str5 = str5 + "<td>" + (cvc.CompletionStatus == "" ? "N/A" : cvc.CompletionStatus) + "</td>";
                    str5 += "</tr>";
                }

                //////for (int i = 0; i < 1; i++)
                //////{
                //////    str5 += "<tr>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].EmployeeId + "</td>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].EmployeeName + "</td>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].ContentTitle + "</td>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].CategoryName + "</td>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].video_timer + "</td>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].total_video_timer + "</td>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].complete_per + "</td>";
                //////    str5 = str5 + "<td>" + LstAllDetails[i].CompletionStatus + "</td>";
                //////    str5 += "</tr>";
                //////}
            }

            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead>" + "<tr><td>Employee Id</td><td>Employee Name</td><td>Content Title</td><td>Category</td><td>Viewed(sec)</td><td>Total Video(sec)</td><td>Completion Per(%)</td><td>Completion Status</td></tr>" + "</thead><tbody>" + str5 + "</tbody></table>";
        }

        ////[HttpPost,HttpGet]
        public JsonResult VideoContentIDs(int CategoryID=0)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            ////List<ContentDropdown> LstContent = this.db.tbl_content.SqlQuery("select * from tbl_content where id_content in (select distinct ContentID from " + VideoContentDBKey + ".tbl_prod_que_ans_video where CategoryID=" + CategoryID + " AND OrgID=" + (object)orgid + ") AND status='A'").OrderBy<ContentDropdown, string>((Func<ContentDropdown, string>)(t => t.CONTENT_TITLE)).ToList<ContentDropdown>();

            List<ContentDropdown> LstContent = new ContentReportModel().getContentDropdownDetails("select ID_CONTENT,CONTENT_QUESTION from tbl_content where id_content in (select distinct ContentID from " + VideoContentDBKey + ".tbl_prod_que_ans_video where CategoryID=" + CategoryID + " AND OrgID=" + (object)orgid + ") AND status='A'").OrderBy(s=>s.CONTENT_QUESTION).ToList();

            ////List<tbl_category> LstCategory = this.db.tbl_category.SqlQuery("select * from tbl_category where id_category in (select distinct CategoryID from " + VideoContentDBKey + ".tbl_prod_que_ans_video where id_organization=" + (object)orgid + ") AND status='A'").OrderBy<tbl_category, string>((Func<tbl_category, string>)(t => t.CATEGORYNAME)).ToList<tbl_category>();

            try
            {
                return Json(LstContent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(LstContent, JsonRequestBehavior.AllowGet);
                ////return (ActionResult)this.RedirectToAction("error");
            }
        }

    }
}
