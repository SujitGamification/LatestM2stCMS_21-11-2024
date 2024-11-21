// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.UploadUtilityController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using static m2ostnext.Controllers.AdministratorController;

namespace m2ostnext.Controllers
{
    public class UploadUtilityController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public class UserIdDataUplodeModel
        {
            public string userId { get; set; }
            public string userGrade { get; set; }
            public string userFunction { get; set; }
            public string city { get; set; }
            public string location { get; set; }
            public string officeAddress { get; set; }
            public string L4 { get; set; }
            public string L3 { get; set; }
            public string L2 { get; set; }
            public string L1 { get; set; }
            public string Spectator { get; set; }
        }


        public ActionResult Index() => (ActionResult)this.View();

        public ActionResult AppUserUpload(int error = 0)
        {
            if (error == 1)
                this.ViewData[nameof(error)] = (object)"Uploaded sheet is not in correct format.Please refer template for refrance";
            else
                this.ViewData[nameof(error)] = (object)"null";
            return (ActionResult)this.View();
        }

        public ActionResult UserUploadDisplay()
        {
            List<UserUploadClass> userUploadClassList = new List<UserUploadClass>();
            List<UserUploadClass> source = new List<UserUploadClass>();
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);
            string upKey = "UP_" + content.id_ORGANIZATION + "_" + content.ID_USER;
            if (this.Request != null)
            {
                HttpPostedFileBase file = this.Request.Files["uploadBtn"];
                if (file != null)
                {
                    if (file.ContentLength >= 0 || !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string contentType = file.ContentType;
                        byte[] buffer = new byte[file.ContentLength];
                        file.InputStream.Read(buffer, 0, Convert.ToInt32(file.ContentLength));
                        using (ExcelPackage excelPackage = new ExcelPackage(file.InputStream))
                        {
                            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault<ExcelWorksheet>();
                            if (excelWorksheet != null)
                            {
                                int column = excelWorksheet.Dimension.End.Column;
                                int row = excelWorksheet.Dimension.End.Row;
                                if (column != 27)
                                    return (ActionResult)this.RedirectToAction("AppUserUpload", (object)new
                                    {
                                        error = 1
                                    });
                                for (int Row = 2; Row <= row; ++Row)
                                {
                                    UserUploadClass userUploadClass = new UserUploadClass();
                                    try
                                    {
                                        userUploadClass.EMPLOYEEID = excelWorksheet.Cells[Row, 1].Value.ToString();
                                        userUploadClass.ROLE = excelWorksheet.Cells[Row, 2].Value.ToString();
                                        userUploadClass.USERID = excelWorksheet.Cells[Row, 3].Value.ToString();
                                        userUploadClass.PASSWORD = excelWorksheet.Cells[Row, 4].Value.ToString();
                                        userUploadClass.FIRSTNAME = excelWorksheet.Cells[Row, 5].Value.ToString();
                                        userUploadClass.LASTNAME = Convert.ToString(excelWorksheet.Cells[Row, 6].Value);
                                        userUploadClass.AGE = excelWorksheet.Cells[Row, 7].Value.ToString();
                                        userUploadClass.EMAIL = excelWorksheet.Cells[Row, 8].Value.ToString();
                                        userUploadClass.MOBILE = excelWorksheet.Cells[Row, 9].Value.ToString();
                                        userUploadClass.GENDER = excelWorksheet.Cells[Row, 10].Value.ToString();
                                        userUploadClass.CITY = excelWorksheet.Cells[Row, 11].Value.ToString();
                                        userUploadClass.OFFICE_ADDRESS = excelWorksheet.Cells[Row, 12].Value.ToString();
                                        string s1 = excelWorksheet.Cells[Row, 13].Value.ToString();
                                        string s2 = excelWorksheet.Cells[Row, 14].Value.ToString();
                                        DateTime dateTime1 = DateTime.FromOADate(double.Parse(s1));
                                        DateTime dateTime2 = DateTime.FromOADate(double.Parse(s2));
                                        userUploadClass.DATE_OF_BIRTH = dateTime1.ToString("dd-MM-yyyy");
                                        userUploadClass.DATE_OF_JOINING = dateTime2.ToString("dd-MM-yyyy");
                                        userUploadClass.user_department = excelWorksheet.Cells[Row, 15].Value.ToString();
                                        userUploadClass.user_designation = excelWorksheet.Cells[Row, 16].Value.ToString();
                                        userUploadClass.user_function = excelWorksheet.Cells[Row, 17].Value.ToString();
                                        userUploadClass.user_grade = excelWorksheet.Cells[Row, 18].Value.ToString();
                                        userUploadClass.user_status = excelWorksheet.Cells[Row, 19].Value.ToString();
                                        userUploadClass.reporting_manager = excelWorksheet.Cells[Row, 21].Value.ToString();
                                        userUploadClass.Location = excelWorksheet.Cells[Row, 22].Value.ToString();

                                        userUploadClass.L4 = excelWorksheet.Cells[Row, 23].Value.ToString();
                                        userUploadClass.L3 = excelWorksheet.Cells[Row, 24].Value.ToString();
                                        userUploadClass.L2 = excelWorksheet.Cells[Row, 25].Value.ToString();
                                        userUploadClass.L1 = excelWorksheet.Cells[Row, 26].Value.ToString();
                                        userUploadClass.Spectator = excelWorksheet.Cells[Row, 27].Value.ToString();

                                        userUploadClassList.Add(userUploadClass);
                                    }
                                    catch (Exception ex)
                                    {
                                        userUploadClassList.Add(userUploadClass);
                                    }
                                }
                            }
                        }
                        foreach (UserUploadClass userUploadClass1 in userUploadClassList)
                        {
                            UserUploadClass item = userUploadClass1;
                            UserUploadClass userUploadClass2 = new UserUploadClass();
                            tbl_user tblUser1 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.EMPLOYEEID == item.EMPLOYEEID && t.ID_ORGANIZATION == (int?)orgid && t.USERID.ToLower() == item.USERID.ToLower())).FirstOrDefault<tbl_user>();
                            userUploadClass2.EMPLOYEEID = item.EMPLOYEEID.Trim();
                            userUploadClass2.ROLE = item.ROLE;
                            string rStr = item.ROLE;
                            tbl_csst_role tblCsstRole = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>)(t => t.csst_role.ToUpper() == rStr.ToUpper() && t.id_organization == (int?)orgid)).FirstOrDefault<tbl_csst_role>();
                            userUploadClass2.STATUS = "A";
                            if (tblCsstRole == null)
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.id_role = 0;
                                userUploadClass2.Message = "Role Not Found";
                            }
                            else
                            userUploadClass2.id_role = tblCsstRole.id_csst_role;
                            userUploadClass2.USERID = item.USERID;
                            userUploadClass2.PASSWORD = item.PASSWORD;
                            if (item.FIRSTNAME != null)
                            {
                                userUploadClass2.FIRSTNAME = item.FIRSTNAME.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.LASTNAME != null)
                            {
                                userUploadClass2.LASTNAME = item.LASTNAME.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            userUploadClass2.AGE = item.AGE;
                            if (item.EMAIL != null)
                            {
                                userUploadClass2.EMAIL = item.EMAIL.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            userUploadClass2.AGE = item.AGE;
                            if (item.MOBILE != null)
                            {
                                userUploadClass2.MOBILE = item.MOBILE.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.GENDER != null)
                            {
                                userUploadClass2.GENDER = item.GENDER.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.CITY != null)
                            {
                                userUploadClass2.CITY = item.CITY.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.OFFICE_ADDRESS != null)
                            {
                                userUploadClass2.OFFICE_ADDRESS = item.OFFICE_ADDRESS.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.DATE_OF_BIRTH != null)
                            {
                                userUploadClass2.DATE_OF_BIRTH = item.DATE_OF_BIRTH.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.DATE_OF_JOINING != null)
                            {
                                userUploadClass2.DATE_OF_JOINING = item.DATE_OF_JOINING.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.user_department != null)
                            {
                                userUploadClass2.user_department = item.user_department.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.user_designation != null)
                            {
                                userUploadClass2.user_designation = item.user_designation.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.user_function != null)
                            {
                                userUploadClass2.user_function = item.user_function.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.user_grade != null)
                            {
                                userUploadClass2.user_grade = item.user_grade.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.user_status != null)
                            {
                                userUploadClass2.user_status = item.user_status.Trim();
                            }
                            else
                            {
                                userUploadClass2.STATUS = "R";
                                userUploadClass2.Message = "|Some data Not Found";
                            }
                            if (item.reporting_manager != null)
                            {
                                if (!(item.reporting_manager == "NA"))
                                {
                                    tbl_user tblUser2 = this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.EMPLOYEEID == item.reporting_manager && t.ID_ORGANIZATION == (int?)orgid)).FirstOrDefault<tbl_user>();
                                    if (tblUser2 == null)
                                    {
                                        userUploadClass2.STATUS = "R";
                                        userUploadClass2.id_reporting_manager = 0;
                                        userUploadClass2.Message += "|Reporting Manager Not Found";
                                    }
                                    else
                                        userUploadClass2.id_reporting_manager = tblUser2.ID_USER;
                                }
                                userUploadClass2.reporting_manager = item.reporting_manager.Trim();
                            }
                               
                           
                            if (tblUser1 != null)
                            {
                                userUploadClass2.Message += "|User already Present.";
                                userUploadClass2.STATUS = "R";
                            }
                            if (userUploadClass2.DATE_OF_BIRTH == null)
                            {
                                userUploadClass2.Message += "|Date of Birth is not in correct format";
                                userUploadClass2.STATUS = "R";
                            }
                            if (userUploadClass2.DATE_OF_JOINING == null)
                            {
                                userUploadClass2.Message += "|Date of joining is not in correct format";
                                userUploadClass2.STATUS = "R";
                            }
                            if (userUploadClass2.USERID == null)
                            {
                                userUploadClass2.Message += "|USERID is null";
                                userUploadClass2.STATUS = "R";
                            }
                            if (userUploadClass2.PASSWORD == null)
                            {
                                userUploadClass2.Message += "|USERID is null";
                                userUploadClass2.STATUS = "R";
                            }
                           
                          
                                userUploadClass2.Location = item.Location.Trim();
                           
                        
                          
                           
                                userUploadClass2.L4 = item.L4.Trim();
                           
                     
                                userUploadClass2.L3 = item.L3.Trim();
                          
                                userUploadClass2.L2 = item.L2.Trim();
                           
                                userUploadClass2.L1 = item.L1.Trim();
                           
                                userUploadClass2.Spectator = item.Spectator.Trim();
                           
                            source.Add(userUploadClass2);
                        }
                        DbSet<tbl_temp_user_upload> tblTempUserUpload = this.db.tbl_temp_user_upload;
                        Expression<Func<tbl_temp_user_upload, bool>> predicate = (Expression<Func<tbl_temp_user_upload, bool>>)(t => t.temp_user_upload_key == upKey);
                        foreach (tbl_temp_user_upload entity in tblTempUserUpload.Where<tbl_temp_user_upload>(predicate).ToList<tbl_temp_user_upload>())
                        {
                            this.db.tbl_temp_user_upload.Remove(entity);
                            this.db.SaveChanges();
                        }
                        foreach (UserUploadClass userUploadClass in source)
                        {
                            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString; // Replace with your MySQL connection string

                            string query = @"
                                            INSERT INTO tbl_temp_user_upload (
                                                EMPLOYEEID, ROLE, status, USERID, PASSWORD, FIRSTNAME, LASTNAME,
                                                AGE, EMAIL, MOBILE, GENDER, CITY, OFFICE_ADDRESS, DATE_OF_BIRTH,
                                                DATE_OF_JOINING, user_department, user_designation, user_function,
                                                user_grade, user_status, reporting_manager, id_reporting_manager,
                                                ID_ROLE, temp_user_upload_key,Location,L4,L3,L2,L1,Spectator
                                            ) VALUES (
                                                @EMPLOYEEID, @ROLE, @status, @USERID, @PASSWORD, @FIRSTNAME, @LASTNAME,
                                                @AGE, @EMAIL, @MOBILE, @GENDER, @CITY, @OFFICE_ADDRESS, @DATE_OF_BIRTH,
                                                @DATE_OF_JOINING, @user_department, @user_designation, @user_function,
                                                @user_grade, @user_status, @reporting_manager, @id_reporting_manager,
                                                @ID_ROLE, @temp_user_upload_key,@Location,@L4,@L3,@L2,@L1,@Spectator
                                            )";

                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                MySqlCommand cmd = new MySqlCommand(query, conn);

                                // Add parameters
                                cmd.Parameters.AddWithValue("@EMPLOYEEID", userUploadClass.EMPLOYEEID);
                                cmd.Parameters.AddWithValue("@ROLE", userUploadClass.ROLE);
                                cmd.Parameters.AddWithValue("@status", userUploadClass.STATUS);
                                cmd.Parameters.AddWithValue("@USERID", userUploadClass.USERID);
                                cmd.Parameters.AddWithValue("@PASSWORD", userUploadClass.PASSWORD);
                                cmd.Parameters.AddWithValue("@FIRSTNAME", userUploadClass.FIRSTNAME);
                                cmd.Parameters.AddWithValue("@LASTNAME", userUploadClass.LASTNAME);
                                cmd.Parameters.AddWithValue("@AGE", userUploadClass.AGE);
                                cmd.Parameters.AddWithValue("@EMAIL", userUploadClass.EMAIL);
                                cmd.Parameters.AddWithValue("@MOBILE", userUploadClass.MOBILE);
                                cmd.Parameters.AddWithValue("@GENDER", userUploadClass.GENDER);
                                cmd.Parameters.AddWithValue("@CITY", userUploadClass.CITY);
                                cmd.Parameters.AddWithValue("@OFFICE_ADDRESS", userUploadClass.OFFICE_ADDRESS);
                                cmd.Parameters.AddWithValue("@DATE_OF_BIRTH", userUploadClass.DATE_OF_BIRTH);
                                cmd.Parameters.AddWithValue("@DATE_OF_JOINING", userUploadClass.DATE_OF_JOINING);
                                cmd.Parameters.AddWithValue("@user_department", userUploadClass.user_department);
                                cmd.Parameters.AddWithValue("@user_designation", userUploadClass.user_designation);
                                cmd.Parameters.AddWithValue("@user_function", userUploadClass.user_function);
                                cmd.Parameters.AddWithValue("@user_grade", userUploadClass.user_grade);
                                cmd.Parameters.AddWithValue("@user_status", userUploadClass.user_status);
                                cmd.Parameters.AddWithValue("@reporting_manager", userUploadClass.reporting_manager);
                                cmd.Parameters.AddWithValue("@id_reporting_manager", userUploadClass.id_reporting_manager);
                                cmd.Parameters.AddWithValue("@ID_ROLE", userUploadClass.id_role);
                                cmd.Parameters.AddWithValue("@temp_user_upload_key", upKey);
                                cmd.Parameters.AddWithValue("@Location", userUploadClass.Location);
                                cmd.Parameters.AddWithValue("@L4", userUploadClass.L4);
                                cmd.Parameters.AddWithValue("@L3", userUploadClass.L3);
                                cmd.Parameters.AddWithValue("@L2", userUploadClass.L2);
                                cmd.Parameters.AddWithValue("@L1", userUploadClass.L1);
                                cmd.Parameters.AddWithValue("@Spectator", userUploadClass.Spectator);

                                try
                                {
                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    // Handle exception
                                    Console.WriteLine("An error occurred: " + ex.Message);
                                }
                            }

                                //this.db.tbl_temp_user_upload.Add(new tbl_temp_user_upload()
                                //{
                                //    EMPLOYEEID = userUploadClass.EMPLOYEEID,
                                //    ROLE = userUploadClass.ROLE,
                                //    status = userUploadClass.STATUS,
                                //    USERID = userUploadClass.USERID,
                                //    PASSWORD = userUploadClass.PASSWORD,
                                //    FIRSTNAME = userUploadClass.FIRSTNAME,
                                //    LASTNAME = userUploadClass.LASTNAME,
                                //    AGE = userUploadClass.AGE,
                                //    EMAIL = userUploadClass.EMAIL,
                                //    MOBILE = userUploadClass.MOBILE,
                                //    GENDER = userUploadClass.GENDER,
                                //    CITY = userUploadClass.CITY,
                                //    OFFICE_ADDRESS = userUploadClass.OFFICE_ADDRESS,
                                //    DATE_OF_BIRTH = userUploadClass.DATE_OF_BIRTH,
                                //    DATE_OF_JOINING = userUploadClass.DATE_OF_JOINING,
                                //    user_department = userUploadClass.user_department,
                                //    user_designation = userUploadClass.user_designation,
                                //    user_function = userUploadClass.user_function,
                                //    user_grade = userUploadClass.user_grade,
                                //    user_status = userUploadClass.user_status,
                                //    reporting_manager = userUploadClass.reporting_manager,
                                //    id_reporting_manager = new int?(userUploadClass.id_reporting_manager),
                                //    ID_ROLE = new int?(userUploadClass.id_role),
                                //    temp_user_upload_key = upKey
                                //});
                                //this.db.SaveChanges();
                        }
                    }
                }
                else
                {
                    DbSet<tbl_temp_user_upload> tblTempUserUpload1 = this.db.tbl_temp_user_upload;
                    Expression<Func<tbl_temp_user_upload, bool>> predicate = (Expression<Func<tbl_temp_user_upload, bool>>)(t => t.temp_user_upload_key == upKey);
                    foreach (tbl_temp_user_upload tblTempUserUpload2 in tblTempUserUpload1.Where<tbl_temp_user_upload>(predicate).ToList<tbl_temp_user_upload>())
                    source.Add(new UserUploadClass()
                        {
                            EMPLOYEEID = tblTempUserUpload2.EMPLOYEEID,
                            ROLE = tblTempUserUpload2.ROLE,
                            STATUS = tblTempUserUpload2.status,
                            USERID = tblTempUserUpload2.USERID,
                            PASSWORD = tblTempUserUpload2.PASSWORD,
                            FIRSTNAME = tblTempUserUpload2.FIRSTNAME,
                            LASTNAME = tblTempUserUpload2.LASTNAME,
                            AGE = tblTempUserUpload2.AGE,
                            EMAIL = tblTempUserUpload2.EMAIL,
                            MOBILE = tblTempUserUpload2.MOBILE,
                            GENDER = tblTempUserUpload2.GENDER,
                            CITY = tblTempUserUpload2.CITY,
                            OFFICE_ADDRESS = tblTempUserUpload2.OFFICE_ADDRESS,
                            DATE_OF_BIRTH = tblTempUserUpload2.DATE_OF_BIRTH,
                            DATE_OF_JOINING = tblTempUserUpload2.DATE_OF_JOINING,
                            user_department = tblTempUserUpload2.user_department,
                            user_designation = tblTempUserUpload2.user_designation,
                            user_function = tblTempUserUpload2.user_function,
                            user_grade = tblTempUserUpload2.user_grade,
                            user_status = tblTempUserUpload2.user_status,
                            reporting_manager = tblTempUserUpload2.reporting_manager,
                         
                        });
                }
            }
            List<UserUploadClass> list1 = source.Where<UserUploadClass>((Func<UserUploadClass, bool>)(t => t.STATUS == "A")).ToList<UserUploadClass>();
            List<UserUploadClass> list2 = source.Where<UserUploadClass>((Func<UserUploadClass, bool>)(t => t.STATUS != "A")).ToList<UserUploadClass>();
            this.ViewData["acceptList"] = (object)list1;
            this.ViewData["rejectList"] = (object)list2;
            return (ActionResult)this.View();
        }

        public ActionResult UpdateUserData()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = Convert.ToInt32(content.id_ORGANIZATION);

            string upKey = "UP_" + content.id_ORGANIZATION + "_" + content.ID_USER;
            List<tbl_temp_user_uploadNew> list = tbl_temp_user_uploadNew.GetUserUploads(upKey);

            List<tbl_temp_user_upload> list1 = this.db.tbl_temp_user_upload.Where<tbl_temp_user_upload>((Expression<Func<tbl_temp_user_upload, bool>>)(t => t.temp_user_upload_key == upKey && t.status == "A")).ToList<tbl_temp_user_upload>();


            foreach (tbl_temp_user_uploadNew tblTempUserUpload in list)
            {

                tbl_temp_user_uploadNew item = tblTempUserUpload;
                tbl_user entity = new tbl_user();
                entity.EMPLOYEEID = item.EMPLOYEEID;
                entity.USERID = item.USERID.Trim();



                if (this.db.tbl_user.Where<tbl_user>((Expression<Func<tbl_user, bool>>)(t => t.EMPLOYEEID == item.EMPLOYEEID && t.ID_ORGANIZATION == (int?)orgid && t.USERID.ToLower() == item.USERID.ToLower())).FirstOrDefault<tbl_user>() == null)
                {
                    entity.PASSWORD = item.PASSWORD.ToMD5Hash();
                    entity.ID_ORGANIZATION = new int?(orgid);
                    entity.FBSOCIALID = "";
                    entity.GPSOCIALID = "";
                    entity.EXPIRY_DATE = new DateTime?(DateTime.Now);
                    entity.STATUS = "A";
                    entity.UPDATEDTIME = DateTime.Now;
                    entity.ID_CODE = 1;
                    entity.user_department = item.user_department;
                    entity.user_designation = item.user_designation;
                    entity.user_function = item.user_function;
                    entity.user_grade = item.user_grade;
                    entity.user_status = item.user_status;
                    entity.ID_ROLE = Convert.ToInt32((object)item.ID_ROLE);
                    entity.reporting_manager = item.id_reporting_manager;

                  

                    this.db.tbl_user.Add(entity);
                    this.db.SaveChanges();

                    UserService userService = new UserService();
                    bool isUpdated = userService.updatethetable_user(entity.ID_USER, item.L4, item.L3, item.L2, item.L1, item.Spectator);



                    if (entity.ID_USER > 0)
                    {
                        this.db.tbl_role_user_mapping.Add(new tbl_role_user_mapping()
                        {
                            id_csst_role = item.ID_ROLE,
                            id_user = new int?(entity.ID_USER),
                            status = "A",
                            updated_date_time = new DateTime?(DateTime.Now)
                        });
                        this.db.SaveChanges();
                        this.db.tbl_profile.Add(new tbl_profile()
                        {
                            FIRSTNAME = item.FIRSTNAME,
                            LASTNAME = item.LASTNAME,
                            EMAIL = item.EMAIL,
                            MOBILE = item.MOBILE,
                            AGE = new int?((int)Convert.ToInt16(item.AGE)),
                            CITY = item.CITY,
                            // LOCATION = "",
                            LOCATION = item.Location,
                            DATE_OF_BIRTH = new DateTime?(new Utility().StringToDatetime(item.DATE_OF_BIRTH)),
                            DATE_OF_JOINING = new DateTime?(new Utility().StringToDatetime(item.DATE_OF_JOINING)),
                            DESIGNATION = item.user_designation,
                            GENDER = item.GENDER,
                            ID_USER = entity.ID_USER,
                            OFFICE_ADDRESS = item.OFFICE_ADDRESS,
                            REPORTING_MANAGER = item.reporting_manager
                        });
                        this.db.SaveChanges();

                        //
                        UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
                        if (userSession != null)
                        {
                            string orgid1 = userSession.id_ORGANIZATION;
                            string id_user = userSession.ID_USER;
                            string Page1 = "Upload Build Data";
                            UserLogDetails userLogDetails = new UserLogDetails();

                            userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                            // Now you have orgid and id_user available for further processing
                        }

                        /// Data is uploded in Ngage
                        UserLogDetails userLogDetails1 = new UserLogDetails();


                        int id_org = userLogDetails1.CheckUserDataNgageOrg(orgid);
                        if (id_org >= 1)
                        {
                            int id_role1 = userLogDetails1.CheckUserroleDataNgage(item.ROLE.ToString());

                            if (id_role1 >= 1)
                            {
                                string Name = item.FIRSTNAME + " " + item.LASTNAME;
                                string Email = item.USERID.Trim();
                                string Mobile = item.MOBILE;
                                //string Password = item.PASSWORD;
                                string Password = "Tgc@1234";
                                string Id_Department = "12";
                                int Id_Role = id_role1;
                                int id_orgname = id_org;
                                int login_type = 1;
                                string country_id = "229";
                                string states_id = "22";
                                string city_id = "3763";
                                int Id_CmsUser = 21;


                                string pass = userLogDetails1.getEncryptedString(Password);
                                //Insert in Ngage
                                userLogDetails1.AddUserDataNgage(Name, Email, Mobile, pass, Id_Department, Id_Role, id_orgname, login_type, country_id, states_id, city_id, Id_CmsUser);

                            }

                        }


                        // For one time User uplode
                       

                        ////Insert in Coroebus

                        //System.Data.DataTable data = userLogDetails1.CheckUserDataCoroebusOrgList(orgid, item.user_function);

                        //System.Data.DataTable data1 = userLogDetails1.CheckUserDataCoroebusRoleList(item.ROLE.ToString());


                        //string tid = "ABC " + DateTime.Now.ToString("yyyyMMddHHmmss");
                        ////D
                        //int id_coroebus_organization = Convert.ToInt32(data.Rows[0]["Coroebus_org_Id"]);
                        ////D
                        //int id_coroebus_game = Convert.ToInt32(data.Rows[0]["Coroebus_game_Id"]);
                        ////D
                        //string group_name = data.Rows[0]["Group"].ToString();
                        ////D
                        //int id_role = Convert.ToInt32(data1.Rows[0]["Coroebus_id_role"]);
                        ////S
                        ////string role_name = data1.Rows[0]["Cororbus_rolemapping"].ToString();
                        //string role_name = data1.Rows[0]["tbl_ngagerole_mappingcol"].ToString();

                        //string USERID = item.USERID.Trim();
                        //string PASSWORD = "Tgc@1234";
                        //string EMPLOYEEID = item.EMPLOYEEID;
                        //string first_name = item.FIRSTNAME + " " + item.LASTNAME;
                        ////string last_name;
                        //string email_id = item.EMAIL;
                        //string contact_number = item.MOBILE;

                        //string user_department = "India";
                        ////D
                        //string user_designation = data1.Rows[0]["Coroebus_designation"].ToString();

                        //string user_function = item.OFFICE_ADDRESS;
                        //string user_grade = "na";
                        //string supervisor = item.reporting_manager;
                        //string team_name = "";
                        ////D
                        //int sr_no = Convert.ToInt32(data1.Rows[0]["Coroebus_sr"]);



                        //int listcb1 = userLogDetails1.InsertUserDataCoroebus(tid, id_coroebus_organization, id_coroebus_game, group_name, id_role, role_name, USERID, PASSWORD, EMPLOYEEID, first_name, email_id, contact_number, user_department, user_designation, user_function, user_grade, supervisor, team_name, sr_no);

                        //if (listcb1 == 0)
                        //{
                        //    int listcb12 = userLogDetails1.Insertcobours(tid, id_coroebus_organization, id_coroebus_game, group_name, id_role, role_name, USERID, PASSWORD, EMPLOYEEID, first_name, email_id, contact_number, user_department, user_designation, user_function, user_grade, supervisor, team_name, sr_no);


                        //}


                        //int listcb = userLogDetails1.CheckUserDataNgageOrgList(orgid);

                        // End of the User uplode
                    }
                }
            }
            this.db.tbl_temp_user_upload.Where<tbl_temp_user_upload>((Expression<Func<tbl_temp_user_upload, bool>>)(t => t.temp_user_upload_key == upKey)).ToList<tbl_temp_user_upload>();
            foreach (tbl_temp_user_upload entity in list1)
            {
                this.db.tbl_temp_user_upload.Remove(entity);
                this.db.SaveChanges();
            }
            //TempData["AlertMessage"] = "Data is Uploaded";
            //return (ActionResult)this.RedirectToAction("app_user_display", "Administrator");
            TempData["AlertMessage"] = "Data is Uploaded";
            
            return (ActionResult)this.RedirectToAction("AppUserUpload", "UploadUtility");
        }

        public ActionResult AppActiveUpload()
        {
            return (ActionResult)this.View();
        }

        public ActionResult UploadDataUser()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            string orgid1 = userSession.id_ORGANIZATION;
            string id_user = userSession.ID_USER;
            string Page1 = "uplode bulk user data";
            UserLogDetails userLogDetails = new UserLogDetails();
            userLogDetails.AddUserDataLog(id_user, orgid1, Page1);

            return View();
        }


        [HttpPost]
        public ActionResult uplodeDataUser(List<UserIdDataUplodeModel> userIdStatusArray, bool isLastChunk)
        {
            if (userIdStatusArray == null || !userIdStatusArray.Any())
            {
                return Json(new { success = false, message = "No data received." });
            }

            // Log the count of received rows
            //  System.Diagnostics.Debug.WriteLine("Received rows count: " + userIdStatusArray.Count);



            List<string> nonExistentUserIds = Session["NonExistentUserIds"] as List<string> ?? new List<string>();
            List<string> activeUserIds = new List<string>();
            bool hasUpdates = false; // Flag to track if there are updates to save

            foreach (var userStatus in userIdStatusArray)
            {
                // Extract user data from the model
                string userId = userStatus.userId;
                string userGrade = userStatus.userGrade;
                string userFunction = userStatus.userFunction;
                string city = userStatus.city;
                string location = userStatus.location;
                string officeAddress = userStatus.officeAddress;
                string L4 = userStatus.L4;
                string L3 = userStatus.L3;
                string L2 = userStatus.L2;
                string L1 = userStatus.L1;
                string Spectator = userStatus.Spectator;

                // Check if user exists in tbl_user
                var tbluser = this.db.tbl_user.FirstOrDefault(t => t.USERID == userId);

                if (tbluser != null)
                {
                    // Update tbl_user record if found
                    if (userGrade != null)
                    {
                        tbluser.user_grade = userGrade;

                    }
                    if (userFunction != null)
                    {
                        tbluser.user_function = userFunction;

                    }
                    int ID_user = tbluser.ID_USER;



                    UserService userService = new UserService();
                    bool isUpdated = userService.updatethetable(ID_user, L4, L3, L2, L1, Spectator);


                    // Check if profile exists in tbl_profile for the user
                    var tblProfile = this.db.tbl_profile.FirstOrDefault(t => t.ID_USER == ID_user);
                    if (tblProfile != null)
                    {
                        if (city != null)
                        {
                            tblProfile.CITY = city;
                        }
                        if (location != null)
                        {
                            tblProfile.LOCATION = location;

                        }
                        if (officeAddress != null)
                        {
                            tblProfile.OFFICE_ADDRESS = officeAddress;
                        }

                    }
                    // Mark that there are updates to save
                    hasUpdates = true;


                    activeUserIds.Add(userId);
                }
                else
                {

                    nonExistentUserIds.Add(userId);
                }
            }

            // Save changes only if updates were made
            Session["NonExistentUserIds"] = nonExistentUserIds;

            if (hasUpdates)
                this.db.SaveChanges();

            // Only send response when the last chunk is received
            if (isLastChunk)
            {

                return Json(new { success = true, message = "Status changed successfully" });
            }

            return new EmptyResult();


        }


        //[HttpPost]
        //public ActionResult uplodeDataUser(List<UserIdDataUplodeModel> userIdStatusArray, bool isLastChunk)
        //{
        //    if (userIdStatusArray == null || !userIdStatusArray.Any())
        //    {
        //        return Json(new { success = false, message = "No data received." });
        //    }

        //    List<string> nonExistentUserIds = Session["NonExistentUserIds"] as List<string> ?? new List<string>();
        //    List<string> activeUserIds = new List<string>();
        //    bool hasUpdates = false;

        //    foreach (var userStatus in userIdStatusArray)
        //    {
        //        string userId = userStatus.userId;
        //        var tbluser = this.db.tbl_user.FirstOrDefault(t => t.USERID == userId);

        //        if (tbluser != null)
        //        {
        //            if (userStatus.userGrade != null)
        //                tbluser.user_grade = userStatus.userGrade;

        //            UserService userService = new UserService();
        //            userService.updatethetable(tbluser.ID_USER, userStatus.L4, userStatus.L3, userStatus.L2, userStatus.L1, userStatus.Spectator);

        //            var tblProfile = this.db.tbl_profile.FirstOrDefault(t => t.ID_USER == tbluser.ID_USER);
        //            if (tblProfile != null)
        //            {
        //                tblProfile.CITY = userStatus.city ?? tblProfile.CITY;
        //                tblProfile.LOCATION = userStatus.location ?? tblProfile.LOCATION;
        //                tblProfile.OFFICE_ADDRESS = userStatus.officeAddress ?? tblProfile.OFFICE_ADDRESS;
        //            }

        //            activeUserIds.Add(userId);
        //            hasUpdates = true;
        //        }
        //        else
        //        {
        //            nonExistentUserIds.Add(userId);
        //        }
        //    }
        //    Session["NonExistentUserIds"] = nonExistentUserIds;

        //    if (hasUpdates)
        //      this.db.SaveChanges();

        //    // Only send response when the last chunk is received
        //    if (isLastChunk)
        //    {
               
        //        return Json(new { success = true, message = "Status changed successfully" });
        //    }

        //    return new EmptyResult(); // No response for intermediate chunks
        //}


        [HttpGet]
        public ActionResult GetNonExistentUserIds()
        {
            var nonExistentUserIds = Session["NonExistentUserIds"] as List<string> ?? new List<string>();
            Session.Remove("NonExistentUserIds");
            return Json(new { success = true, nonExistentUserIds }, JsonRequestBehavior.AllowGet);
        }




    }
}