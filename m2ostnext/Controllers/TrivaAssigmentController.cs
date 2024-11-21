using m2ostnext.Models;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace m2ostnext.Controllers
{
    public class TrivaAssigmentController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        // GET: TrivaAssigment
        public ActionResult Index()
        {
            Session.Remove("IdCategory");
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string orgid = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "TrivaAssigment";


                UserLogDetails userLogDetails = new UserLogDetails();
                userLogDetails.AddUserDataLog(id_user, orgid, Page1);
            }
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];

            int num = Convert.ToInt32(item.id_ORGANIZATION);

            Convert.ToInt32(item.ID_USER);

            Addlearning_categoryModel CategoryModel = new Addlearning_categoryModel();

            List<tbl_learning_category> CategoryList = CategoryModel.GetCategoryData(num);

            ViewData["CategoryList"] = CategoryList;

            return View();

        }

        public ActionResult SubCategoryList(int id)
        {
            Session["IdCategory"] = id;
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string orgid = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "TrivaAssigment";


                UserLogDetails userLogDetails = new UserLogDetails();
                userLogDetails.AddUserDataLog(id_user, orgid, Page1);
            }
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];

            int num = Convert.ToInt32(item.id_ORGANIZATION);

            Convert.ToInt32(item.ID_USER);
            tbl_learning_sub_categoryModel SubCategoryModel = new tbl_learning_sub_categoryModel();

            List<tbl_learning_sub_category> SubCategoryList = SubCategoryModel.sub_categoryListAssigment(num, id);

            ViewData["SubCategoryList"] = SubCategoryList;

            return View();

        }


        public ActionResult TrivaAssigmentQuestion(int id)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string orgid_1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "TrivaAssigment";


                UserLogDetails userLogDetails = new UserLogDetails();
                userLogDetails.AddUserDataLog(id_user, orgid_1, Page1);
            }
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];

            int orgid = Convert.ToInt32(item.id_ORGANIZATION);

            this.ViewData["RoleList"] = (object)this.db.tbl_csst_role.SqlQuery("select * from tbl_csst_role where ID_ORGANIZATION=" + (object)orgid + " and status='A'").ToList<tbl_csst_role>();
            Addlearning_questionModel QuestionModel = new Addlearning_questionModel();

            List<tbl_learning_question> QuestionModelList = QuestionModel.QuestionListTrivia(orgid, id);

            ViewData["QuestionModelList"] = QuestionModelList;
            int cids = (int)Session["IdCategory"];
            ViewData["IdCategory"] = cids;
            return View();
        }

        public string getUserListForRole(string id, string pattern, string cid, string sid, string type)
        {
            int int32 = Convert.ToInt32(id);
            int sids = Convert.ToInt32(sid);
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
                    str8 = str8 + "<td>" + item.UPDATEDTIME + " </td>";
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
                        // tbl_content_program_mapping contentProgramMapping = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
                        Addlearning_questionModel QuestionModel = new Addlearning_questionModel();
                        tbl_learning_assigment QuestionModelList = QuestionModel.GetlearningassigmentMapping(cids, sids, orgid, item.ID_USER);

                        if (QuestionModelList == null)
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
                            DateTime? nullable = Convert.ToDateTime(QuestionModelList.StartDate);
                            DateTime dateTime = nullable.Value;
                            strArray[2] = dateTime.ToShortDateString();
                            strArray[3] = " to ";
                            nullable = Convert.ToDateTime(QuestionModelList.EndDate);
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

                        // tbl_content_program_mapping contentProgramMapping = this.db.tbl_content_program_mapping.Where<tbl_content_program_mapping>((Expression<Func<tbl_content_program_mapping, bool>>)(t => t.id_category == (int?)cids && t.id_organization == (int?)orgid && t.id_user == (int?)item.ID_USER)).FirstOrDefault<tbl_content_program_mapping>();
                        Addlearning_questionModel QuestionModel = new Addlearning_questionModel();
                        tbl_learning_assigment QuestionModelList = QuestionModel.GetlearningassigmentMapping(cids, sids, orgid, item.ID_USER);

                        if (QuestionModelList != null)
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

        [HttpPost]
        public ActionResult uplodeUserID(List<tbl_learning_assigment> userIdStatusArray, bool isLastChunk)
        {

            int IdCategory = (int)Session["IdCategory"];
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
            string page = "Triva ASSIGNMENT Bulk UploadUserID";
            string idProgram = Request.Form["id_program"];
            string idOperation = "Save";

            // Log the user action
            UserLogDetails userLogDetails = new UserLogDetails();
            userLogDetails.AddUserDataLogopration(userIdSession, orgId, page, idProgram, null, idOperation);

            foreach (var userStatus in userIdStatusArray)
            {
                string userId = userStatus.IdUser;
                int orgIdInt = Convert.ToInt32(userSession.id_ORGANIZATION);
                int IdsubCategory = userStatus.IdLearningSubCategory;

                // Check if user exists
                var tblUser = this.db.tbl_user.FirstOrDefault(t => t.USERID == userId);
                if (tblUser != null)
                {
                    int userRoleId;
                    if (!int.TryParse(userStatus.IdRole, out userRoleId))
                    {
                        return Json(new { success = false, message = "Invalid role ID for user " + userId });
                    }

                    int userContentId = tblUser.ID_USER;

                    var roleMapping = this.db.tbl_role_user_mapping
                        .FirstOrDefault(t => t.id_csst_role == userRoleId && t.id_user == userContentId);
                    if (roleMapping != null)
                    {
                        //int idProgram_1 = Convert.ToInt16(userStatus.IdLearningSubCategory);

                        //var program_mapping = this.db.tbl_content_program_mapping
                        //    .FirstOrDefault(c => c.id_category == idProgram_1 && c.id_user == userContentId);

                        Addlearning_questionModel QuestionModel = new Addlearning_questionModel();

                        int QuestionModelList = QuestionModel.QuestionListTriviastatus(orgIdInt, userContentId, IdsubCategory, IdCategory);

                        if (QuestionModelList == 0)
                        {
                            //DateTime datetime1 = userStatus.StartDate;
                            //DateTime datetime2 = userStatus.EndDate;
                            DateTime datetime1 = new Utility().StringToDatetime(userStatus.StartDate);
                            DateTime datetime2 = new Utility().StringToDatetime(userStatus.EndDate);

                            var QuestionResult = QuestionModel.QuestionTriviainsert(orgIdInt, userContentId, IdsubCategory, IdCategory, userRoleId, datetime1, datetime2);
                            // Create a new mapping entry
                            //tbl_content_program_mapping entity = new tbl_content_program_mapping
                            //{
                            //    map_type = 1,
                            //    id_role = userRoleId,
                            //    id_user = userContentId,
                            //    id_organization = orgIdInt,
                            //    id_category = Convert.ToInt16(userStatus.IdLearningSubCategory),
                            //    status = "A",
                            //    option_type = 0,
                            //    start_date = new DateTime?(datetime1),
                            //    expiry_date = new DateTime?(datetime2),
                            //    id_assessment_sheet = 0,
                            //    updated_date_time = DateTime.Now  
                            //};


                            //this.db.tbl_content_program_mapping.Add(entity);
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
            //Session.Remove("IdCategory");
            Session["NonExistentUserIds"] = nonExistentUserIds;
            Session["AllreadyUserIds"] = allreadyUserIds;

            if (hasUpdates)
            {
                this.db.SaveChanges();
            }

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

        public string removeTrivaAssessmentToUser(string cid, string value, string sid, string type)
        {
            try
            {

                int cds = Convert.ToInt32(cid);
                int sds = Convert.ToInt32(sid);
                int opts = Convert.ToInt32(value);
                int int32 = Convert.ToInt32(type);
                int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
                Addlearning_questionModel QuestionModel = new Addlearning_questionModel();

                int Result = QuestionModel.DeletelearningassigmentMapping(cds, sds, orgid, opts);

                return Result > 0 ? "1" : "2";

            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return $"Error: {ex.Message}";
            }
        }


        [HttpPost]
        public string removeProgramToUserall_1(string cid, string sid, List<string> values, string type)
        {

            string user = "0";
            int cids = Convert.ToInt32(cid);
            int sids = Convert.ToInt32(sid);

            int int32 = Convert.ToInt32(type);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

            foreach (var value in values)
            {
                int opts = Convert.ToInt32(value);
                Addlearning_questionModel QuestionModel = new Addlearning_questionModel();

                int Result = QuestionModel.DeletelearningassigmentMapping(cids, sids, orgid, opts);

                if (Result > 0)
                {
                    user = "1"; // Indicating success
                }
            }

            return user;
        }

        public string setTrivaToMultiUser()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Triva ASSIGMENT";
                string Id_assessment = this.Request.Form["id_program"];
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            int id_config = 0;
            string multiUser = "1";

            if (!int.TryParse(this.Request.Form["id_Category"], out int ids))
            {
                throw new FormatException("id_Category is not in a correct format.");
            }

            int idrole = Convert.ToInt32(this.Request.Form["role-Id"]);


            int sbCategory = Convert.ToInt32(this.Request.Form["id_sbCategory"]);



            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            if (!int.TryParse(content.id_ORGANIZATION, out int orgid))
            {
                throw new FormatException("id_ORGANIZATION is not in a correct format.");
            }

            if (!int.TryParse(content.ID_USER, out int int32_3))
            {
                throw new FormatException("ID_USER is not in a correct format.");
            }




            int[] source;
            try
            {
                source = Array.ConvertAll(this.Request.Form["chk_user"].Split(','), int.Parse);
            }
            catch (FormatException)
            {
                throw new FormatException("chk_user contains values that are not in a correct format.");
            }

            //DateTime? nullable1 = !string.IsNullOrWhiteSpace(this.Request.Form["start_date"])
            //    ? DateTime.ParseExact(this.Request.Form["start_date"], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
            //    : (DateTime?)null;

            //DateTime? nullable2 = !string.IsNullOrWhiteSpace(this.Request.Form["end_date"])
            //    ? DateTime.ParseExact(this.Request.Form["end_date"], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
            //    : (DateTime?)null;
            string startDateStr = this.Request.Form["start_date"];
            string endDateStr = this.Request.Form["end_date"];

            // Use the string versions directly in the StringToDatetime method
            DateTime datetime1 = new Utility().StringToDatetime(startDateStr);
            DateTime datetime2 = new Utility().StringToDatetime(endDateStr);

            for (int i = 0; i < source.Length; i += 1000)
            {
                var batchSource = source.Skip(i).Take(1000).ToList();

                foreach (int user_idn in batchSource)
                {

                    string selectQuery = @"SELECT * FROM tbl_learning_assigment 
                               WHERE id_learning_category = @id_category 
                               AND id_organization = @id_organization 
                               AND id_learning_sub_category =@subcategory
                               AND id_user = @id_user";

                    using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                    {
                        conn.Open();

                        using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                        {
                            selectCmd.Parameters.AddWithValue("@id_category", ids);
                            selectCmd.Parameters.AddWithValue("@subcategory", sbCategory);
                            selectCmd.Parameters.AddWithValue("@id_organization", orgid);
                            selectCmd.Parameters.AddWithValue("@id_user", user_idn);

                            using (MySqlDataReader reader = selectCmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    reader.Close();

                                    Addlearning_questionModel QuestionModel = new Addlearning_questionModel();
                                    var QuestionResult = QuestionModel.QuestionTriviainsert(orgid, user_idn, sbCategory, ids, idrole, datetime1, datetime2);

                                    multiUser = QuestionResult;
                                }
                                else
                                {
                                    reader.Close();

                                    Addlearning_questionModel QuestionModel = new Addlearning_questionModel();
                                    var QuestionResult = QuestionModel.QuestionTriviaupdate(orgid, user_idn, sbCategory, ids, idrole, datetime1, datetime2);
                                    multiUser = QuestionResult;
                                }
                            }
                        }
                    }
                }



            }

            return multiUser;

        }


        public string setTrivaToAllUser(
     string value,
     string type,
     string cdt,
     string edt,
     string cid,
     string sid
 )
        {
            string allUser = "0";
            int ids, int32_2;

            if (!int.TryParse(cid, out ids) || !int.TryParse(sid, out int32_2))
            {
                return "Invalid category or subcategory ID";
            }

            DateTime datetime1 = new Utility().StringToDatetime(cdt);
            DateTime datetime2 = new Utility().StringToDatetime(edt);

            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);

             List<tbl_user> list = this.db.tbl_user.SqlQuery("select * from tbl_user where STATUS = 'A' && id_user in (select distinct id_user from tbl_role_user_mapping where id_csst_role in (select id_csst_role from tbl_csst_role where id_organization=" + (object)orgid + "))").ToList<tbl_user>();


            const int batchSize = 1000;
            int totalUsers = list.Count;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    conn.Open();

                    for (int i = 0; i < totalUsers; i += batchSize)
                    {
                        var batch = list.Skip(i).Take(batchSize).ToList();

                        foreach (tbl_user tblUser in batch)
                        {
                            string selectQuery = @"SELECT * FROM tbl_learning_assigment 
                                           WHERE id_learning_category = @id_category 
                                           AND id_organization = @id_organization 
                                           AND id_learning_sub_category = @subcategory 
                                           AND id_user = @id_user";

                            using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                            {
                                selectCmd.Parameters.AddWithValue("@id_category", ids);
                                selectCmd.Parameters.AddWithValue("@subcategory", int32_2);
                                selectCmd.Parameters.AddWithValue("@id_organization", orgid);
                                selectCmd.Parameters.AddWithValue("@id_user", tblUser.ID_USER);

                                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                                {
                                    Addlearning_questionModel questionModel = new Addlearning_questionModel();
                                    if (!reader.HasRows)
                                    {
                                        reader.Close();
                                        var questionResult = questionModel.QuestionTriviainsert(
                                            orgid, tblUser.ID_USER, int32_2, ids, tblUser.ID_ROLE, datetime1, datetime2);
                                        allUser = questionResult;
                                    }
                                    else
                                    {
                                        reader.Close();
                                        var questionResult = questionModel.QuestionTriviaupdate(
                                            orgid, tblUser.ID_USER, int32_2, ids, tblUser.ID_ROLE, datetime1, datetime2);
                                        allUser = questionResult;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or return an error message
                return $"Error occurred: {ex.Message}";
            }

            return allUser;
        }

    }
}