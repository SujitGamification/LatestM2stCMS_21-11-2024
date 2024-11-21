// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.AssessmentController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using Google.Protobuf.WellKnownTypes;
using m2ostnext.Models;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using static m2ostnext.Models.Ngage;

namespace m2ostnext.Controllers
{
    [UserFilter]
    public class AssessmentController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();


        [RoleAccessController(KEY = 9)]
        public ActionResult Index(int flag = 0)
        {
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            this.ViewData["assessment"] = (object)this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_organization == (int?)oid && t.status != "D")).ToList<tbl_assessment>();
            this.ViewData[nameof(flag)] = (object)flag;
            return (ActionResult)this.View();
        }

        public ActionResult AssessmentDetails(string id)
        {
            int ids = Convert.ToInt32(id);
            Assessment assessment = new Assessment();
            assessment.tbl_assessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == ids && t.status == "A")).FirstOrDefault<tbl_assessment>();
            if (assessment.tbl_assessment == null)
                return (ActionResult)this.RedirectToAction("Index");
            List<AssessmentQuestion> assessmentQuestionList = new List<AssessmentQuestion>();
            DbSet<tbl_assessment_question> assessmentQuestion1 = this.db.tbl_assessment_question;
            Expression<Func<tbl_assessment_question, bool>> predicate = (Expression<Func<tbl_assessment_question, bool>>)(t => t.id_assessment == (int?)ids);
            foreach (tbl_assessment_question assessmentQuestion2 in assessmentQuestion1.Where<tbl_assessment_question>(predicate).ToList<tbl_assessment_question>())
            {
                tbl_assessment_question obtbl_assessment_question = assessmentQuestion2;
                AssessmentQuestion assessmentQuestion3 = new AssessmentQuestion();
                assessmentQuestion3.tbl_assessment_question = obtbl_assessment_question;
                assessmentQuestion3.tbl_assessment_answer = this.db.tbl_assessment_answer.Where<tbl_assessment_answer>((Expression<Func<tbl_assessment_answer, bool>>)(t => t.id_assessment_question == (int?)obtbl_assessment_question.id_assessment_question)).ToList<tbl_assessment_answer>();
                assessmentQuestionList.Add(assessmentQuestion3);
            }
            assessment.assessment_question = assessmentQuestionList;
            this.ViewData["Assessment"] = (object)assessment;
            return (ActionResult)this.PartialView(nameof(AssessmentDetails), (object)id);
        }

        //[RoleAccessController(KEY = 8)]
        //public ActionResult createAssessment() => (ActionResult)this.View();

        [RoleAccessController(KEY = 8)]
        public ActionResult createAssessment()
        {
            Ngage_service kpiService = new Ngage_service();
            List<SelectListItem> gameList = kpiService.GetngageList();
            Ngage_service kpiService1 = new Ngage_service();
            List<SelectListItem> OrgIDList = kpiService1.GetOrgIdList();

            //Ngage_service kpiService2 = new Ngage_service();
            //List<SelectListItem> AssList = kpiService2.GetAssList();

            ViewBag.gameList = gameList;
            ViewBag.OrgIDList = OrgIDList;
            //ViewBag.AssList = AssList;

            return (ActionResult)this.View();
        }
        public ActionResult editAssessment(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == ids)).FirstOrDefault<tbl_assessment>();
            if (tblAssessment == null)
                return (ActionResult)this.RedirectToAction("Index");
            List<tbl_assessment_scoring_key> list = this.db.tbl_assessment_scoring_key.Where<tbl_assessment_scoring_key>((Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == (int?)ids)).OrderBy<tbl_assessment_scoring_key, int?>((Expression<Func<tbl_assessment_scoring_key, int?>>)(t => t.position)).ToList<tbl_assessment_scoring_key>();
            this.ViewData["assessment"] = (object)tblAssessment;
            this.ViewData["scoringkey"] = (object)list;
            return (ActionResult)this.View();
        }

        public ActionResult edit_assessment_action()
        {
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            this.Request.Form["btn_submit"].ToString();
            int assid = Convert.ToInt32(this.Request.Form["id_assessment"].ToString());
            tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == assid)).FirstOrDefault<tbl_assessment>();
            if (tblAssessment == null)
                return (ActionResult)this.RedirectToAction("Index");
            tblAssessment.assessment_title = this.Request.Form["assessment-name"].ToString();
            tblAssessment.assesment_description = this.Request.Form["assessment-desc"].ToString();

            //tblAssessment.answer_description = this.Request.Form["answer-description"].ToString();

            string encodeurl = this.Request.Form["answer-description"].ToString();

            tblAssessment.answer_description = HttpUtility.UrlEncode(encodeurl);

            tblAssessment.assess_created = new DateTime?(new Utility().StringToDatetime(this.Request.Form["assessment-started"].ToString()));
            tblAssessment.assess_ended = new DateTime?(new Utility().StringToDatetime(this.Request.Form["assessment-ended"].ToString()));
            tblAssessment.total_attempt = new int?(Convert.ToInt32(this.Request.Form["max-attempt"].ToString()));
            tblAssessment.ans_requiered = new int?(0);
            int? assessGroup1 = tblAssessment.assess_group;
            int num1 = 1;
            if ((assessGroup1.GetValueOrDefault() == num1 ? (assessGroup1.HasValue ? 1 : 0) : 0) != 0)
                tblAssessment.ans_requiered = new int?(Convert.ToInt32(this.Request.Form["answer-display"].ToString()));
            int? assessGroup2 = tblAssessment.assess_group;
            int num2 = 3;
            if ((assessGroup2.GetValueOrDefault() == num2 ? (assessGroup2.HasValue ? 1 : 0) : 0) != 0)
            {
                tblAssessment.lower_title = this.Request.Form["low-value-title"].ToString();
                tblAssessment.lower_value = this.Request.Form["low-value"].ToString();
                tblAssessment.high_title = this.Request.Form["high-value-title"].ToString();
                tblAssessment.high_value = this.Request.Form["high-value"].ToString();
            }
            tblAssessment.updated_date_time = new DateTime?(DateTime.Now);
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("LoadAssessment", "Assessment", (object)new
            {
                id = tblAssessment.id_assessment
            });
        }

        public ActionResult viewAssessment(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == ids)).FirstOrDefault<tbl_assessment>();
            this.db.tbl_assessment_question.Where<tbl_assessment_question>((Expression<Func<tbl_assessment_question, bool>>)(t => t.id_assessment == (int?)ids)).ToList<tbl_assessment_question>();
            this.ViewData["assessment"] = (object)tblAssessment;
            return (ActionResult)this.View();
        }

        public ActionResult add_assessment()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "ADD ASSESSMENT";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            string str = this.Request.Form["btn_submit"].ToString();
            tbl_assessment entity1 = new tbl_assessment();
            entity1.id_organization = new int?(int32_1);
            entity1.assessment_title = this.Request.Form["assessment-name"].ToString().TrimStart(',');

            // Use the selected assessment ID to fetch the title from your data source
            ///string assessmentTitle = FetchAssessmentTitle(selectedAssessmentID);


            //entity1.assessment_title = this.Request.Form["assessment-name"].ToString();
            entity1.assesment_description = this.Request.Form["assessment-desc"].ToString();

            //entity1.answer_description = this.Request.Form["answer-description"].ToString();


            string encodeurl = this.Request.Form["answer-description"].ToString();

            entity1.answer_description = HttpUtility.UrlEncode(encodeurl);

            entity1.assess_start = new DateTime?(new Utility().StringToDatetime(this.Request.Form["assessment-created"].ToString()));
            entity1.assess_created = new DateTime?(new Utility().StringToDatetime(this.Request.Form["assessment-started"].ToString()));
            entity1.assess_ended = new DateTime?(new Utility().StringToDatetime(this.Request.Form["assessment-ended"].ToString()));
            entity1.assess_type = this.Request.Form["assessment-type"].ToString();
            entity1.assessment_type = new int?(Convert.ToInt32(this.Request.Form["assessment-div"].ToString()));
            entity1.assess_group = new int?(Convert.ToInt32(this.Request.Form["assessment-group"].ToString()));
            entity1.total_attempt = new int?(Convert.ToInt32(this.Request.Form["max-attempt"].ToString()));
            entity1.ans_requiered = new int?(0);
            int? assessGroup = entity1.assess_group;
            int num1 = 1;
            if ((assessGroup.GetValueOrDefault() == num1 ? (assessGroup.HasValue ? 1 : 0) : 0) != 0)
                entity1.ans_requiered = new int?(Convert.ToInt32(this.Request.Form["answer-display"].ToString()));
            assessGroup = entity1.assess_group;
            int num2 = 3;
            if ((assessGroup.GetValueOrDefault() == num2 ? (assessGroup.HasValue ? 1 : 0) : 0) != 0)
            {
                entity1.lower_title = this.Request.Form["low-value-title"].ToString();
                entity1.lower_value = this.Request.Form["low-value"].ToString();
                entity1.high_title = this.Request.Form["high-value-title"].ToString();
                entity1.high_value = this.Request.Form["high-value"].ToString();
            }
            entity1.status = "T";
            entity1.updated_date_time = new DateTime?(DateTime.Now);
            this.db.tbl_assessment.Add(entity1);
            this.db.SaveChanges();
            assessGroup = entity1.assess_group;
            int num3 = 2;
            if ((assessGroup.GetValueOrDefault() == num3 ? (assessGroup.HasValue ? 1 : 0) : 0) != 0)
            {
                int int32_2 = Convert.ToInt32(this.Request.Form["no-of-key-vak"].ToString());
                for (int index = 1; index <= int32_2; ++index)
                {
                    tbl_assessment_scoring_key entity2 = new tbl_assessment_scoring_key()
                    {
                        header_name = this.Request.Form["t-scoring-key-" + (object)index].ToString()
                    };
                    entity2.header_name = entity2.header_name.Trim(' ', ',');
                    entity2.id_assessment = new int?(entity1.id_assessment);
                    entity2.position = new int?(index);
                    entity2.id_assessment_theme = entity1.assess_group;
                    entity2.status = "A";
                    entity2.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_scoring_key.Add(entity2);
                    this.db.SaveChanges();
                }
            }
            assessGroup = entity1.assess_group;
            int num4 = 4;
            if ((assessGroup.GetValueOrDefault() == num4 ? (assessGroup.HasValue ? 1 : 0) : 0) != 0)
            {
                int int32_3 = Convert.ToInt32(this.Request.Form["no-of-key-rank"].ToString());
                for (int index = 1; index <= int32_3; ++index)
                {
                    tbl_assessment_scoring_key entity3 = new tbl_assessment_scoring_key()
                    {
                        header_name = this.Request.Form["t-scoring-key-" + (object)index].ToString()
                    };
                    entity3.header_name = entity3.header_name.Trim(' ', ',');
                    entity3.id_assessment = new int?(entity1.id_assessment);
                    entity3.position = new int?(index);
                    entity3.id_assessment_theme = entity1.assess_group;
                    entity3.status = "A";
                    entity3.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_scoring_key.Add(entity3);
                    this.db.SaveChanges();
                }
            }
            assessGroup = entity1.assess_group;
            int num5 = 3;
            if ((assessGroup.GetValueOrDefault() == num5 ? (assessGroup.HasValue ? 1 : 0) : 0) != 0)
            {
                int int32_4 = Convert.ToInt32(this.Request.Form["no-of-key-range"].ToString());
                for (int index = 1; index <= int32_4; ++index)
                {
                    tbl_assessment_scoring_key entity4 = new tbl_assessment_scoring_key()
                    {
                        header_name = this.Request.Form["t-scoring-key-" + (object)index].ToString()
                    };
                    entity4.header_name = entity4.header_name.Trim(' ', ',');
                    entity4.id_assessment = new int?(entity1.id_assessment);
                    entity4.position = new int?(index);
                    entity4.id_assessment_theme = entity1.assess_group;
                    entity4.status = "A";
                    entity4.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_scoring_key.Add(entity4);
                    this.db.SaveChanges();
                }
            }
            this.db.tbl_assessment_sheet.Add(new tbl_assessment_sheet()
            {
                id_organization = new int?(int32_1),
                id_assesment = entity1.id_assessment,
                id_assessment_theme = entity1.assess_group,
                status = "A",
                updated_date_time = new DateTime?(DateTime.Now)
            });
            this.db.SaveChanges();
            return str.Equals("Save") ? (ActionResult)this.RedirectToAction("assessmentQuestions", "Assessment", (object)new
            {
                id = entity1.id_assessment,
                flag = 1
            }) : (ActionResult)this.RedirectToAction("Index", (object)new
            {
                flag = 1
            });
        }

        public ActionResult assessmentQuestions(string id, int flag = 0)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int ids = Convert.ToInt32(id);
            tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == ids && t.id_organization == (int?)orgid)).FirstOrDefault<tbl_assessment>();
            if (tblAssessment == null)
                return (ActionResult)this.RedirectToAction("Index");
            List<tbl_assessment_question> list = this.db.tbl_assessment_question.Where<tbl_assessment_question>((Expression<Func<tbl_assessment_question, bool>>)(t => t.id_assessment == (int?)ids)).ToList<tbl_assessment_question>();
            this.ViewData["scoring_key"] = (object)this.db.tbl_assessment_scoring_key.Where<tbl_assessment_scoring_key>((Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == (int?)ids)).ToList<tbl_assessment_scoring_key>();
            this.ViewData["assessment_question"] = (object)list;
            this.ViewData[nameof(flag)] = (object)flag;
            this.ViewData["assessment"] = (object)tblAssessment;

            //For scroing type new table
            KPI_name_service kpiService1 = new KPI_name_service();
            List<SelectListItem> scoringList = kpiService1.GetnewscroingList(orgid);
            ViewBag.scoringList = scoringList;


            KPI_name_service kpiService = new KPI_name_service();
            List<SelectListItem> kpiList = kpiService.GetKpiList(orgid);
            ViewBag.KPIList = kpiList;

            //coins
            ViewBag.coinList = Coins(orgid, tblAssessment.id_assessment);

            //mastrypoint
            List<tbl_scoring_type_detailsTable> pointList = kpiService.Getpointlist(tblAssessment.id_assessment, orgid);
            ViewBag.pointList = pointList;

            //rightAns
            List<tbl_kpi_scoring_master_details_rightAns> pointAns = kpiService.GetpointAns(tblAssessment.id_assessment, orgid);
            ViewData["pointRightAns"] = pointAns;

            //ontme
            List<tbl_content_asessment_completion_timeframe_details> ontime = kpiService.GetOntimeData(tblAssessment.id_assessment, orgid);
            ViewData["ontime"] = ontime;

            // Point(tblAssessment.id_assessment, orgid);


            return (ActionResult)this.View();
        }

      
        public List<tbl_scoring_type_detailsTable> Point(int id_assessment, int orgid)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int id_user = Convert.ToInt32(content.ID_USER);
            KPI_name_service kpiService = new KPI_name_service();
            List<tbl_scoring_type_detailsTable> pointList = kpiService.Getpointlist(id_assessment, orgid);
            List<tbl_kpi_scoring_master_details_rightAns> pointAns = kpiService.GetpointAns(id_assessment, orgid);
            List<tbl_content_asessment_completion_timeframe_details> ontime = kpiService.GetOntimeData(id_assessment, orgid);
            ViewData["pointList"] = pointList;
            ViewData["pointRightAns"] = pointAns;
            ViewData["ontime"] = ontime;
            Coins(orgid, id_assessment);
            return pointList;

            //if (pointList.Count > 0)
            //{
            //    ViewData["pointList"] = pointList;
            //    //return pointList;
            //}
            //else if (pointAns.Count > 0)
            //{
            //    ViewData["pointRightAns"] = pointAns;
            //    //return pointList;

            //}
            //else if (ontime.Count > 0)
            //{
            //    ViewData["ontime"] = ontime;
            //    //return pointList;

            //}
            //else
            //{
            //    Coins(orgid, id_assessment);
            //    return pointList;
            //}
            //ViewData["pointList"] = pointList;
            //ViewData["pointRightAns"] = pointAns;
            //ViewData["ontime"] = ontime;
            //return pointList;
        }


        //public List<tbl_content_asessment_completion_timeframe_details> ontimeData(int id_assessment, int orgid)
        //{
        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int id_user = Convert.ToInt32(content.ID_USER);
        //    KPI_name_service kpiService = new KPI_name_service();
        //    //List<tbl_content_asessment_completion_timeframe_details> timePointList = kpiService.GetOntimeData(id_assessment, orgid);

        //  //  List<tbl_kpi_scoring_master_details_rightAns> pointTime = kpiService.GetpointAns(id_assessment, orgid);
        //    if (timePointList.Count == 0)
        //    {
        //        Coins(orgid, id_assessment);
        //        return timePointList;
        //    }
        //    ViewData["pointList"] = timePointList;
        //    ViewData["timePointList"] = timePointList;
        //    return timePointList;
        //}

        public List<tbl_coins_master> Coins(int orgId, int assId)
        {
            KPI_name_service kpiService = new KPI_name_service();
            List<tbl_coins_master> coinList = kpiService.GetCoinlist(orgId, assId);

            ViewData["coinList"] = coinList;

            return coinList;
        }


        //kpi
        [HttpPost]
        public ActionResult SaveKPIandScroing(List<tbl_scoring_type_details> data)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    foreach (var dataItem in data)
                    {
                        if (dataItem.Id_scoring_type_details == 0)
                        {
                            // Insert query
                            string insertSql = "INSERT INTO tbl_scoring_type_details (Id_organization, Name_kpi_id, Id_assessment, Sub_type_name_Id, Category_Id, Attempt_number, Mastery_percentage, Points) " +
                                              "VALUES (@Id_organization, @Name_kpi_id, @Id_assessment, @Sub_type_name_Id, @Category_Id, @Attempt_number,  @Mastery_percentage,  @Points)";
                            MySqlCommand insertCommand = new MySqlCommand(insertSql, connection);
                            insertCommand.Parameters.AddWithValue("@Id_organization", orgid);
                            insertCommand.Parameters.AddWithValue("@Name_kpi_id", dataItem.Name_kpi_id);
                            insertCommand.Parameters.AddWithValue("@Id_assessment", dataItem.Id_assessment);
                            insertCommand.Parameters.AddWithValue("@Sub_type_name_Id", dataItem.Sub_type_name_Id);
                            insertCommand.Parameters.AddWithValue("@Category_Id", dataItem.Category_Id);
                            insertCommand.Parameters.AddWithValue("@Attempt_number", dataItem.Attempt_number);
                            insertCommand.Parameters.AddWithValue("@Mastery_percentage", dataItem.Mastery_percentage);
                            insertCommand.Parameters.AddWithValue("@Points", dataItem.Points);

                            insertCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            // Update query
                            string updateSql = "UPDATE tbl_scoring_type_details SET Name_kpi_id = @Name_kpi_id, Id_assessment = @Id_assessment, " +
                                               "Sub_type_name_Id = @Sub_type_name_Id, Category_Id = @Category_Id, Attempt_number = @Attempt_number, " +
                                               "Mastery_percentage = @Mastery_percentage, Points = @Points WHERE Id_scoring_type_details = '" + dataItem.Id_scoring_type_details + "'";
                            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
                            updateCommand.Parameters.AddWithValue("@Name_kpi_id", dataItem.Name_kpi_id);
                            updateCommand.Parameters.AddWithValue("@Id_assessment", dataItem.Id_assessment);
                            updateCommand.Parameters.AddWithValue("@Sub_type_name_Id", dataItem.Sub_type_name_Id);
                            updateCommand.Parameters.AddWithValue("@Category_Id", dataItem.Category_Id);
                            updateCommand.Parameters.AddWithValue("@Attempt_number", dataItem.Attempt_number);
                            updateCommand.Parameters.AddWithValue("@Mastery_percentage", dataItem.Mastery_percentage);
                            updateCommand.Parameters.AddWithValue("@Points", dataItem.Points);
                            updateCommand.Parameters.AddWithValue("@Id_scoring_type_details", dataItem.Id_scoring_type_details);

                            updateCommand.ExecuteNonQuery();
                        }
                    }

                    return Json(new { success = true, message = "Data saved successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return Json(new { success = false, message = "An error occurred while saving data." });
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }



        public ActionResult SaveCoins(List<tbl_coins_master> data1)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int id_user = Convert.ToInt32(content.ID_USER);
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                connection.Open();
                foreach (var dataItem in data1)
                {

                    if (dataItem.Id_Coins == 0)
                    {
                        // Open the connection


                        // Create a MySQL command with parameters for your INSERT statement
                        string insertQuery = "INSERT INTO tbl_coins_master (Attempt_no, Set_percentage, Set_Score, Id_organization, Id_assessment,Created_by) " +
                                             "VALUES (@Attempt_no, @Set_percentage, @Set_Score, @Id_organization, @Id_assessment,@Created_by)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            // Add parameters to the command
                            command.Parameters.Add("@Attempt_no", MySqlDbType.Int32).Value = dataItem.Attempt_no;
                            command.Parameters.Add("@Set_percentage", MySqlDbType.Int32).Value = dataItem.Set_percentage;
                            command.Parameters.Add("@Set_Score", MySqlDbType.Int32).Value = dataItem.Set_Score;
                            command.Parameters.Add("@Id_organization", MySqlDbType.Int32).Value = orgid;
                            command.Parameters.Add("@Id_assessment", MySqlDbType.Int32).Value = dataItem.Id_assessment;
                            command.Parameters.Add("@Created_by", MySqlDbType.Int32).Value = id_user;

                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Create a MySQL command with parameters for your UPDATE statement
                        string updateQuery = "UPDATE tbl_coins_master " +
                     "SET Attempt_no = @Attempt_no,Created_by=@Created_by, Set_percentage = @Set_percentage, Set_Score = @Set_Score " +
                     "WHERE Id_organization = @Id_organization AND Id_assessment = @Id_assessment AND Id_Coins = @Id_Coins";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            // Add parameters to the update command
                            updateCommand.Parameters.Add("@Attempt_no", MySqlDbType.Int32).Value = dataItem.Attempt_no;
                            updateCommand.Parameters.Add("@Set_percentage", MySqlDbType.Int32).Value = dataItem.Set_percentage;
                            updateCommand.Parameters.Add("@Set_Score", MySqlDbType.Int32).Value = dataItem.Set_Score;
                            updateCommand.Parameters.Add("@Id_organization", MySqlDbType.Int32).Value = orgid;
                            updateCommand.Parameters.Add("@Id_assessment", MySqlDbType.Int32).Value = dataItem.Id_assessment;
                            updateCommand.Parameters.Add("@Created_by", MySqlDbType.Int32).Value = id_user;
                            updateCommand.Parameters.Add("@Id_Coins", MySqlDbType.Int32).Value = dataItem.Id_Coins; // Assuming Id_Coins is an integer


                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }


            return Json(new { success = true, message = "Data saved successfully." });
            //return (ActionResult)this.View();
        }

        public ActionResult OnTime(List<tbl_scoring_type_details> data2)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_scoring_type_details temp = new tbl_scoring_type_details();
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                try
                {
                    if (data2[0].Id_scoring_type_details == 0)
                    {
                        connection.Open();

                        string sql = "INSERT INTO tbl_scoring_type_details (Id_organization, Name_kpi_id, Id_assessment, Sub_type_name_Id, Duration, Points) " +
                                     "VALUES (@Id_organization, @Name_kpi_id, @Id_assessment, @Sub_type_name_Id,  @Duration,  @Points)";

                        MySqlCommand command = new MySqlCommand(sql, connection);
                        command.Parameters.Add("@Id_organization", MySqlDbType.Int32);
                        command.Parameters.Add("@Name_kpi_id", MySqlDbType.VarChar);
                        command.Parameters.Add("@Id_assessment", MySqlDbType.Int32);
                        command.Parameters.Add("@Sub_type_name_Id", MySqlDbType.Int32);


                        command.Parameters.Add("@Duration", MySqlDbType.VarChar);

                        command.Parameters.Add("@Points", MySqlDbType.VarChar);


                        foreach (var dataItem in data2)
                        {
                            command.Parameters["@Id_organization"].Value = orgid;
                            command.Parameters["@Name_kpi_id"].Value = dataItem.Name_kpi_id;
                            command.Parameters["@Id_assessment"].Value = dataItem.Id_assessment;
                            command.Parameters["@Sub_type_name_Id"].Value = dataItem.Sub_type_name_Id;
                            command.Parameters["@Duration"].Value = dataItem.Duration;

                            command.Parameters["@Points"].Value = dataItem.Points;


                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        connection.Open();
                        string updateSql = "UPDATE tbl_scoring_type_details " +
                    "SET Id_organization = @Id_organization, " +
                    "    Name_kpi_id = @Name_kpi_id, " +
                    "    Id_assessment = @Id_assessment, " +
                    "    Sub_type_name_Id = @Sub_type_name_Id, " +
                    "    Duration = @Duration, " +
                    "    Points = @Points " +
                    "WHERE Id_scoring_type_details = '" + data2[0].Id_scoring_type_details + "'";

                        MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);

                        updateCommand.Parameters.Add("@Id_organization", MySqlDbType.Int32);
                        updateCommand.Parameters.Add("@Name_kpi_id", MySqlDbType.VarChar);
                        updateCommand.Parameters.Add("@Id_assessment", MySqlDbType.Int32);
                        updateCommand.Parameters.Add("@Sub_type_name_Id", MySqlDbType.Int32);
                        updateCommand.Parameters.Add("@Duration", MySqlDbType.VarChar);
                        updateCommand.Parameters.Add("@Points", MySqlDbType.VarChar);
                        foreach (var dataItem in data2)
                        {
                            updateCommand.Parameters["@Id_organization"].Value = orgid;
                            updateCommand.Parameters["@Name_kpi_id"].Value = dataItem.Name_kpi_id;
                            updateCommand.Parameters["@Id_assessment"].Value = dataItem.Id_assessment;
                            updateCommand.Parameters["@Sub_type_name_Id"].Value = dataItem.Sub_type_name_Id;
                            updateCommand.Parameters["@Duration"].Value = dataItem.Duration;
                            updateCommand.Parameters["@Points"].Value = dataItem.Points;

                            updateCommand.ExecuteNonQuery();
                        }
                    }



                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }

            return Json(new { success = true, message = "Data saved successfully." });
        }

        public ActionResult OnTimeEnd(List<tbl_content_asessment_completion_timeframe_details> data2)
        {

            DateTime currentDate = DateTime.Now;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int id_user = Convert.ToInt32(content.ID_USER);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    foreach (var dataItem in data2)
                    {
                        if (dataItem.ID_Scoring_Matrix == 0)
                        {
                            // Insert query



                            string query = "SELECT COUNT(*) FROM tbl_kpi_scoring_master_details WHERE ID_Scoring_Matrix = @idScoringMatrix;";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@idScoringMatrix", data2[0].ID_Scoring_Matrix);

                                try
                                {

                                    int count = Convert.ToInt32(command.ExecuteScalar());

                                    if (count > 0)
                                    {
                                        string query1 = @"INSERT INTO tbl_content_asessment_completion_timeframe_details
                                             (ID_Scoring_Matrix, AttemptNo, Score, Points, IsActive, created_by, created_date,updated_by)
                                             VALUES
                                             (@ID_Scoring_Matrix, @AttemptNo, @Score, @Points, @IsActive, @created_by, @created_date,@updated_by)";

                                        using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                                        {
                                            // Add parameters to the command object
                                            command1.Parameters.AddWithValue("@ID_Scoring_Matrix", data2[0].ID_Scoring_Matrix);
                                            command1.Parameters.AddWithValue("@KPI_Type", 1);

                                            command1.Parameters.AddWithValue("@Category", data2[0].Category);

                                            object timePeriodValue;
                                            if (data2[0].TimePeriod != null)
                                            {
                                                timePeriodValue = data2[0].TimePeriod;
                                            }
                                            else
                                            {
                                                timePeriodValue = DBNull.Value;
                                            }

                                            command1.Parameters.AddWithValue("@TimePeriod", timePeriodValue);


                                            command1.Parameters.AddWithValue("@Points", dataItem.Points);
                                            command1.Parameters.AddWithValue("@IsActive", 'A');
                                            command1.Parameters.AddWithValue("@created_by", 1);
                                            command1.Parameters.AddWithValue("@created_date", currentDate);
                                            //command1.Parameters.AddWithValue("@updated_by", id_user);

                                            try
                                            {

                                                int rowsAffected = command1.ExecuteNonQuery();

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"Error: {ex.Message}");
                                            }
                                        }

                                    }
                                    else
                                    {
                                        string insertSql = @"INSERT INTO tbl_kpi_scoring_master_details
                                             (ID_KPI, ID_Assessment_Type, Content_Assessment_ID, ApplyMasterScoreMultipleAttempts,
                                              ApplyRightAnswerMultipleAttempts, IsActive, created_by, created_date)
                                              VALUES
                                              (@ID_KPI, @ID_Assessment_Type, @Content_Assessment_ID, @ApplyMasterScoreMultipleAttempts,
                                               @ApplyRightAnswerMultipleAttempts, @IsActive, @CreatedBy, @CreatedDate);
                                              SELECT LAST_INSERT_ID()";

                                        MySqlCommand insertCommand = new MySqlCommand(insertSql, connection);
                                        insertCommand.Parameters.AddWithValue("@ID_KPI", dataItem.ID_KPI);
                                        insertCommand.Parameters.AddWithValue("@ID_Assessment_Type", 2); // Add missing parameter
                                        insertCommand.Parameters.AddWithValue("@Content_Assessment_ID", dataItem.Content_Assessment_ID);
                                        insertCommand.Parameters.AddWithValue("@ApplyMasterScoreMultipleAttempts", 1);
                                        insertCommand.Parameters.AddWithValue("@ApplyRightAnswerMultipleAttempts", 1);
                                        insertCommand.Parameters.AddWithValue("@IsActive", 'A');
                                        insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                        insertCommand.Parameters.AddWithValue("@CreatedDate", currentDate);
                                        //Use appropriate value for updated_date


                                        int idScoringMatrix = Convert.ToInt32(insertCommand.ExecuteScalar());
                                        if (idScoringMatrix != 0)
                                        {

                                            string query1 = @"INSERT INTO tbl_content_asessment_completion_timeframe_details
                                             (ID_Scoring_Matrix, KPI_Type, Category,TimePeriod, Points, IsActive, created_by, created_date)
                                             VALUES
                                             (@ID_Scoring_Matrix, @KPI_Type,@Category,@TimePeriod,@Points, @IsActive, @created_by, @created_date)";

                                            using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                                            {
                                                // Add parameters to the command object
                                                command1.Parameters.AddWithValue("@ID_Scoring_Matrix", idScoringMatrix);
                                                command1.Parameters.AddWithValue("@KPI_Type", 1);

                                                command1.Parameters.AddWithValue("@Category", data2[0].Category);

                                                object timePeriodValue;
                                                if (data2[0].TimePeriod != null)
                                                {
                                                    timePeriodValue = data2[0].TimePeriod;
                                                }
                                                else
                                                {
                                                    timePeriodValue = DBNull.Value;
                                                }

                                                command1.Parameters.AddWithValue("@TimePeriod", timePeriodValue);
                                                command1.Parameters.AddWithValue("@Points", dataItem.Points);
                                                command1.Parameters.AddWithValue("@IsActive", 'A');
                                                command1.Parameters.AddWithValue("@created_by", 1);
                                                command1.Parameters.AddWithValue("@created_date", currentDate);
                                                //command1.Parameters.AddWithValue("@updated_by", id_user);

                                                try
                                                {

                                                    int rowsAffected = command1.ExecuteNonQuery();

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Error: {ex.Message}");
                                                }
                                            }

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }

                        }
                        else
                        {
                            string query1 = @"
                                UPDATE tbl_content_asessment_completion_timeframe_details 
                                SET 
                                    KPI_Type = @KPI_Type,
                                    Category = @Category,
                                    TimePeriod = @TimePeriod,
                                    Points = @Points,
                                    IsActive = @IsActive,
                                    updated_by = @updated_by,
                                    updated_date = @updated_date
                                WHERE 
                                    ID_Scoring_Matrix = @ID_Scoring_Matrix";

                            using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                            {
                                // Add parameters to the command object
                                command1.Parameters.AddWithValue("@ID_Scoring_Matrix", dataItem.ID_Scoring_Matrix);

                                command1.Parameters.AddWithValue("@KPI_Type", 1);

                                command1.Parameters.AddWithValue("@Category", data2[0].Category);

                                object timePeriodValue;
                                if (data2[0].TimePeriod != null)
                                {
                                    timePeriodValue = data2[0].TimePeriod;
                                }
                                else
                                {
                                    timePeriodValue = DBNull.Value;
                                }

                                command1.Parameters.AddWithValue("@TimePeriod", timePeriodValue);
                                command1.Parameters.AddWithValue("@Points", dataItem.Points);
                                command1.Parameters.AddWithValue("@IsActive", 'A');
                                command1.Parameters.AddWithValue("@updated_by", id_user);
                                command1.Parameters.AddWithValue("@updated_date", currentDate);
                                //command1.Parameters.AddWithValue("@updated_by", id_user); // Uncomment and add this line if you have an updated_by parameter

                                try
                                {
                                    int rowsAffected = command1.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }

                        }
                    }

                    return Json(new { success = true, message = "Data saved successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return Json(new { success = false, message = "An error occurred while saving data." });
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }

        }
        //


        public ActionResult GridBase(List<tbl_scoring_type_details> data1)
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_scoring_type_details temp = new tbl_scoring_type_details();
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                try
                {
                    if (data1[0].Id_scoring_type_details == 0)
                    {


                        connection.Open();

                        string sql = "INSERT INTO tbl_scoring_type_details (Id_organization, Name_kpi_id, Id_assessment, Sub_type_name_Id,Start_range,End_range,Points) " +
                                     "VALUES (@Id_organization, @Name_kpi_id, @Id_assessment, @Sub_type_name_Id,@Start_range,@End_range,@Points)";

                        MySqlCommand command = new MySqlCommand(sql, connection);
                        command.Parameters.Add("@Id_organization", MySqlDbType.Int32);
                        command.Parameters.Add("@Name_kpi_id", MySqlDbType.VarChar);
                        command.Parameters.Add("@Id_assessment", MySqlDbType.Int32);
                        command.Parameters.Add("@Sub_type_name_Id", MySqlDbType.Int32);
                        command.Parameters.Add("@Start_range", MySqlDbType.VarChar);
                        command.Parameters.Add("@End_range", MySqlDbType.VarChar);
                        command.Parameters.Add("@Points", MySqlDbType.VarChar);


                        foreach (var dataItem in data1)
                        {
                            command.Parameters["@Id_organization"].Value = orgid;
                            command.Parameters["@Name_kpi_id"].Value = dataItem.Name_kpi_id;
                            command.Parameters["@Id_assessment"].Value = dataItem.Id_assessment;
                            command.Parameters["@Sub_type_name_Id"].Value = dataItem.Sub_type_name_Id;
                            command.Parameters["@Start_range"].Value = dataItem.Start_range;
                            command.Parameters["@End_range"].Value = dataItem.End_range;
                            command.Parameters["@Points"].Value = dataItem.Points;


                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        connection.Open();
                        foreach (var dataItem in data1)
                        {
                            string updateSql = "UPDATE tbl_scoring_type_details " +
                                               "SET Id_organization = @Id_organization, " +
                                               "    Name_kpi_id = @Name_kpi_id, " +
                                               "    Id_assessment = @Id_assessment, " +
                                               "    Sub_type_name_Id = @Sub_type_name_Id, " +
                                               "    Duration = @Duration, " +
                                               "    Points = @Points " +
                                               "WHERE Id_scoring_type_details ='" + dataItem.Id_scoring_type_details + "'";

                            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
                            updateCommand.Parameters.Add("@Id_organization", MySqlDbType.Int32);
                            updateCommand.Parameters.Add("@Name_kpi_id", MySqlDbType.VarChar);
                            updateCommand.Parameters.Add("@Id_assessment", MySqlDbType.Int32);
                            updateCommand.Parameters.Add("@Sub_type_name_Id", MySqlDbType.Int32);
                            updateCommand.Parameters.Add("@Duration", MySqlDbType.VarChar);
                            updateCommand.Parameters.Add("@Points", MySqlDbType.VarChar);
                            updateCommand.Parameters.Add("@Id_scoring_type_details", MySqlDbType.Int32);

                            // Assign parameter values
                            updateCommand.Parameters["@Id_organization"].Value = orgid;
                            updateCommand.Parameters["@Name_kpi_id"].Value = dataItem.Name_kpi_id;
                            updateCommand.Parameters["@Id_assessment"].Value = dataItem.Id_assessment;
                            updateCommand.Parameters["@Sub_type_name_Id"].Value = dataItem.Sub_type_name_Id;
                            updateCommand.Parameters["@Duration"].Value = dataItem.Duration;
                            updateCommand.Parameters["@Points"].Value = dataItem.Points;
                            updateCommand.Parameters["@Id_scoring_type_details"].Value = dataItem.Id_scoring_type_details;

                            // Execute the update query
                            updateCommand.ExecuteNonQuery();
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }


            return Json(new { success = true, message = "Data saved successfully." });

        }


        public ActionResult SaveKPIandScroingMaster(List<tbl_kpi_scoring_master_details> data)
        {

            DateTime currentDate = DateTime.Now;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int id_user = Convert.ToInt32(content.ID_USER);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    foreach (var dataItem in data)
                    {
                        if (dataItem.ID_Scoring_Matrix == 0)
                        {
                            // Insert query



                            string query = "SELECT COUNT(*) FROM tbl_kpi_scoring_master_details WHERE ID_Scoring_Matrix = @idScoringMatrix;";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@idScoringMatrix", data[0].ID_Scoring_Matrix);

                                try
                                {

                                    int count = Convert.ToInt32(command.ExecuteScalar());

                                    if (count > 0)
                                    {
                                        string query1 = @"INSERT INTO tbl_assessment_mastery_score_details
                                                 (ID_Scoring_Matrix, AttemptNo, Score, Points, IsActive, created_by, created_date)
                                                 VALUES
                                                 (@ID_Scoring_Matrix, @AttemptNo, @Score, @Points, @IsActive, @created_by, @created_date)";

                                        using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                                        {
                                            // Add parameters to the command object
                                            command1.Parameters.AddWithValue("@ID_Scoring_Matrix", data[0].ID_Scoring_Matrix);
                                            command1.Parameters.AddWithValue("@AttemptNo", dataItem.AttemptNo);
                                            command1.Parameters.AddWithValue("@Score", dataItem.Score);
                                            command1.Parameters.AddWithValue("@Points", dataItem.Points);
                                            command1.Parameters.AddWithValue("@IsActive", 'A');
                                            command1.Parameters.AddWithValue("@created_by", id_user);
                                            command1.Parameters.AddWithValue("@created_date", currentDate);
                                            


                                            try
                                            {

                                                int rowsAffected = command1.ExecuteNonQuery();

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"Error: {ex.Message}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string insertSql = @"INSERT INTO tbl_kpi_scoring_master_details
                                                 (ID_KPI, ID_Assessment_Type, Content_Assessment_ID, ApplyMasterScoreMultipleAttempts,
                                                  ApplyRightAnswerMultipleAttempts, IsActive, created_by, created_date,updated_by)
                                                  VALUES
                                                  (@ID_KPI, @ID_Assessment_Type, @Content_Assessment_ID, @ApplyMasterScoreMultipleAttempts,
                                                   @ApplyRightAnswerMultipleAttempts, @IsActive, @CreatedBy, @CreatedDate,@updated_by);
                                                  SELECT LAST_INSERT_ID()";

                                        MySqlCommand insertCommand = new MySqlCommand(insertSql, connection);
                                        insertCommand.Parameters.AddWithValue("@ID_KPI", dataItem.ID_KPI);
                                        insertCommand.Parameters.AddWithValue("@ID_Assessment_Type", 2); // Add missing parameter
                                        insertCommand.Parameters.AddWithValue("@Content_Assessment_ID", dataItem.Content_Assessment_ID);
                                        insertCommand.Parameters.AddWithValue("@ApplyMasterScoreMultipleAttempts", 1);
                                        insertCommand.Parameters.AddWithValue("@ApplyRightAnswerMultipleAttempts", 1);
                                        insertCommand.Parameters.AddWithValue("@IsActive", 'A');
                                        insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                        insertCommand.Parameters.AddWithValue("@CreatedDate", currentDate);
                                        insertCommand.Parameters.AddWithValue("@updated_by", id_user);
                                        // Use appropriate value for updated_date

                                        int idScoringMatrix = Convert.ToInt32(insertCommand.ExecuteScalar());
                                        if (idScoringMatrix != 0)
                                        {

                                            string query1 = @"INSERT INTO tbl_assessment_mastery_score_details
                                                 (ID_Scoring_Matrix, AttemptNo, Score, Points, IsActive, created_by, created_date)
                                                 VALUES
                                                 (@ID_Scoring_Matrix, @AttemptNo, @Score, @Points, @IsActive, @created_by, @created_date)";

                                            using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                                            {
                                                // Add parameters to the command object
                                                command1.Parameters.AddWithValue("@ID_Scoring_Matrix", idScoringMatrix);
                                                command1.Parameters.AddWithValue("@AttemptNo", dataItem.AttemptNo);
                                                command1.Parameters.AddWithValue("@Score", dataItem.Score);
                                                command1.Parameters.AddWithValue("@Points", dataItem.Points);
                                                command1.Parameters.AddWithValue("@IsActive", 'A');
                                                command1.Parameters.AddWithValue("@created_by", id_user);
                                                command1.Parameters.AddWithValue("@created_date", currentDate);
                                             


                                                try
                                                {

                                                    int rowsAffected = command1.ExecuteNonQuery();

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Error: {ex.Message}");
                                                }
                                            }

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }








                        }
                        else
                        {
                            string query = @"UPDATE tbl_assessment_mastery_score_details
                                        SET AttemptNo = @AttemptNo,
                                            Score = @Score,
                                            Points = @Points,
                                            IsActive = @IsActive,
                                            updated_by = @updated_by,
                                            updated_date = @updated_date,
                                            ID_Scoring_Matrix = @ID_Scoring_Matrix
                                        WHERE ID = @ID";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                // Add parameters to the command object
                                command.Parameters.AddWithValue("@ID", dataItem.ID);
                                command.Parameters.AddWithValue("@ID_Scoring_Matrix", dataItem.ID_Scoring_Matrix);
                                command.Parameters.AddWithValue("@AttemptNo", dataItem.AttemptNo);
                                command.Parameters.AddWithValue("@Score", dataItem.Score);
                                command.Parameters.AddWithValue("@Points", dataItem.Points);
                                command.Parameters.AddWithValue("@IsActive", 'A');
                                command.Parameters.AddWithValue("@updated_by", id_user);
                                command.Parameters.AddWithValue("@updated_date", currentDate);

                                try
                                {

                                    int rowsAffected = command.ExecuteNonQuery();
                                    Console.WriteLine($"Rows affected: {rowsAffected}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }
                        }
                    }

                    return Json(new { success = true, message = "Data saved successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return Json(new { success = false, message = "An error occurred while saving data." });
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }

        public ActionResult SaveRightAns(List<tbl_kpi_scoring_master_details_rightAns> datarightans)
        {

            DateTime currentDate = DateTime.Now;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int id_user = Convert.ToInt32(content.ID_USER);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    foreach (var dataItem in datarightans)
                    {
                        if (dataItem.ID_Scoring_Matrix == 0)
                        {
                            // Insert query



                            string query = "SELECT COUNT(*) FROM tbl_kpi_scoring_master_details WHERE ID_Scoring_Matrix = @idScoringMatrix;";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@idScoringMatrix", datarightans[0].ID_Scoring_Matrix);

                                try
                                {

                                    int count = Convert.ToInt32(command.ExecuteScalar());

                                    if (count > 0)
                                    {
                                        string query1 = @"INSERT INTO tbl_assessment_right_answer_details
                                                 (ID_Scoring_Matrix, AttemptNo, Points, IsActive, created_by, created_date,updated_by)
                                                 VALUES
                                                 (@ID_Scoring_Matrix, @AttemptNo, @Points, @IsActive, @created_by, @created_date,@updated_by)";

                                        using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                                        {
                                            // Add parameters to the command object
                                            command1.Parameters.AddWithValue("@ID_Scoring_Matrix", datarightans[0].ID_Scoring_Matrix);
                                            command1.Parameters.AddWithValue("@AttemptNo", dataItem.AttemptNo);

                                            command1.Parameters.AddWithValue("@Points", dataItem.Points);
                                            command1.Parameters.AddWithValue("@IsActive", 'A');
                                            command1.Parameters.AddWithValue("@created_by", 1);
                                            command1.Parameters.AddWithValue("@created_date", currentDate);
                                            command1.Parameters.AddWithValue("@updated_by", id_user);


                                            try
                                            {

                                                int rowsAffected = command1.ExecuteNonQuery();

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"Error: {ex.Message}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string insertSql = @"INSERT INTO tbl_kpi_scoring_master_details
                                                 (ID_KPI, ID_Assessment_Type, Content_Assessment_ID, ApplyMasterScoreMultipleAttempts,
                                                  ApplyRightAnswerMultipleAttempts, IsActive, created_by, created_date,updated_by)
                                                  VALUES
                                                  (@ID_KPI, @ID_Assessment_Type, @Content_Assessment_ID, @ApplyMasterScoreMultipleAttempts,
                                                   @ApplyRightAnswerMultipleAttempts, @IsActive, @CreatedBy, @CreatedDate,@updated_by);
                                                  SELECT LAST_INSERT_ID()";

                                        MySqlCommand insertCommand = new MySqlCommand(insertSql, connection);
                                        insertCommand.Parameters.AddWithValue("@ID_KPI", dataItem.ID_KPI);
                                        insertCommand.Parameters.AddWithValue("@ID_Assessment_Type", 2); // Add missing parameter
                                        insertCommand.Parameters.AddWithValue("@Content_Assessment_ID", dataItem.Content_Assessment_ID);
                                        insertCommand.Parameters.AddWithValue("@ApplyMasterScoreMultipleAttempts", 1);
                                        insertCommand.Parameters.AddWithValue("@ApplyRightAnswerMultipleAttempts", 1);
                                        insertCommand.Parameters.AddWithValue("@IsActive", 'A');
                                        insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                        insertCommand.Parameters.AddWithValue("@CreatedDate", currentDate);
                                        insertCommand.Parameters.AddWithValue("@updated_by", id_user);
                                        // Use appropriate value for updated_date

                                        int idScoringMatrix = Convert.ToInt32(insertCommand.ExecuteScalar());
                                        if (idScoringMatrix != 0)
                                        {

                                            string query1 = @"INSERT INTO tbl_assessment_right_answer_details
                                                 (ID_Scoring_Matrix, AttemptNo, Points, IsActive, created_by, created_date,updated_by)
                                                 VALUES
                                                 (@ID_Scoring_Matrix, @AttemptNo, @Points, @IsActive, @created_by, @created_date,@updated_by)";

                                            using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                                            {
                                                // Add parameters to the command object
                                                command1.Parameters.AddWithValue("@ID_Scoring_Matrix", idScoringMatrix);
                                                command1.Parameters.AddWithValue("@AttemptNo", dataItem.AttemptNo);

                                                command1.Parameters.AddWithValue("@Points", dataItem.Points);
                                                command1.Parameters.AddWithValue("@IsActive", 'A');
                                                command1.Parameters.AddWithValue("@created_by", 1);
                                                command1.Parameters.AddWithValue("@created_date", currentDate);
                                                command1.Parameters.AddWithValue("@updated_by", id_user);


                                                try
                                                {

                                                    int rowsAffected = command1.ExecuteNonQuery();

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Error: {ex.Message}");
                                                }
                                            }

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }








                        }
                        else
                        {
                            string query = @"UPDATE tbl_assessment_right_answer_details
                                        SET AttemptNo = @AttemptNo,
                                           
                                            Points = @Points,
                                            IsActive = @IsActive,
                                            updated_by = @updated_by,
                                            updated_date = @updated_date,
                                            ID_Scoring_Matrix = @ID_Scoring_Matrix
                                            WHERE ID = @ID";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                // Add parameters to the command object
                                command.Parameters.AddWithValue("@ID", dataItem.ID);
                                command.Parameters.AddWithValue("@ID_Scoring_Matrix", dataItem.ID_Scoring_Matrix);
                                command.Parameters.AddWithValue("@AttemptNo", dataItem.AttemptNo);

                                command.Parameters.AddWithValue("@Points", dataItem.Points);
                                command.Parameters.AddWithValue("@IsActive", 'A');
                                command.Parameters.AddWithValue("@updated_by", id_user);
                                command.Parameters.AddWithValue("@updated_date", currentDate);

                                try
                                {

                                    int rowsAffected = command.ExecuteNonQuery();
                                    Console.WriteLine($"Rows affected: {rowsAffected}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }
                        }
                    }

                    return Json(new { success = true, message = "Data saved successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return Json(new { success = false, message = "An error occurred while saving data." });
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteMastery(int id) // Change parameter name to id
        {
            try
            {
                KPI_name_service kpiService = new KPI_name_service();
                kpiService.DeleteMastery(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine("Exception: " + ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteRightAnswer(int id) // Change parameter name to id
        {
            try
            {
                KPI_name_service kpiService = new KPI_name_service();
                kpiService.DeleteRightAnswer(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine("Exception: " + ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult OnTimeDelete(int id) // Change parameter name to id
        {
            try
            {
                KPI_name_service kpiService = new KPI_name_service();
                kpiService.OnTimeDelete(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine("Exception: " + ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }



        [HttpPost]
        public ActionResult CoinsDelete(int id) // Change parameter name to id
        {
            try
            {
                KPI_name_service kpiService = new KPI_name_service();
                kpiService.CoinsDelete(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine("Exception: " + ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        //


        public ActionResult GetSubtypes(int kpiId)
        {
            KPI_name_service kpiService = new KPI_name_service();
            List<SelectListItem> subtypes = kpiService.GetSubtypesByKpi(kpiId);
            return Json(subtypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult add_assessment_question()
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int assint = Convert.ToInt32(this.Request.Form["id-assessment"].ToString());
            tbl_assessment tblAssessment1 = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == assint)).FirstOrDefault<tbl_assessment>();
            tbl_assessment_question question = new tbl_assessment_question();
            question.assessment_question = this.Request.Form["q-title"].ToString();
            question.id_organization = new int?(int32_1);
            question.id_assessment = new int?(assint);
            int? assessGroup1 = tblAssessment1.assess_group;
            int num1 = 3;
            if ((assessGroup1.GetValueOrDefault() == num1 ? (assessGroup1.HasValue ? 1 : 0) : 0) != 0)
            {
                int int32_2 = Convert.ToInt32(this.Request.Form["select-scoring-key"].ToString());
                question.id_assessment_scoring_key = new int?(int32_2);
            }
            else
                question.id_assessment_scoring_key = new int?(0);
            question.status = "A";
            question.updated_date_time = new DateTime?(DateTime.Now);
            this.db.tbl_assessment_question.Add(question);
            this.db.SaveChanges();
            if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
            {
                HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["question-image"];
                string extension = Path.GetExtension(System.Web.HttpContext.Current.Request.Files["question-image"].FileName);
                if (file.ContentLength > 0)
                {
                    if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Assessment/")))
                        Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Assessment/"));
                    string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Assessment/"), "aq_" + (object)question.id_assessment_question + extension);
                    file.SaveAs(filename);
                    question.question_image = "aq_" + (object)question.id_assessment_question + extension;
                    this.db.SaveChanges();
                }
            }
            tbl_assessment tblAssessment2 = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => (int?)t.id_assessment == question.id_assessment)).FirstOrDefault<tbl_assessment>();
            int? assessGroup2 = tblAssessment2.assess_group;
            int num2 = 3;
            if ((assessGroup2.GetValueOrDefault() == num2 ? (!assessGroup2.HasValue ? 1 : 0) : 1) != 0)
            {
                int num3 = 0;
                int num4 = 0;
                assessGroup2 = tblAssessment2.assess_group;
                int num5 = 1;
                if ((assessGroup2.GetValueOrDefault() == num5 ? (assessGroup2.HasValue ? 1 : 0) : 0) != 0)
                {
                    num4 = Convert.ToInt32(this.Request.Form["select-answer"].ToString());
                    num3 = Convert.ToInt32(this.Request.Form["select-options-type"].ToString());
                }
                else
                {
                    assessGroup2 = tblAssessment2.assess_group;
                    int num6 = 2;
                    if ((assessGroup2.GetValueOrDefault() == num6 ? (assessGroup2.HasValue ? 1 : 0) : 0) != 0)
                    {
                        num3 = this.db.tbl_assessment_scoring_key.Where<tbl_assessment_scoring_key>((Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == question.id_assessment)).ToList<tbl_assessment_scoring_key>().Count;
                    }
                    else
                    {
                        assessGroup2 = tblAssessment2.assess_group;
                        int num7 = 4;
                        if ((assessGroup2.GetValueOrDefault() == num7 ? (assessGroup2.HasValue ? 1 : 0) : 0) != 0)
                            num3 = this.db.tbl_assessment_scoring_key.Where<tbl_assessment_scoring_key>((Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == question.id_assessment)).ToList<tbl_assessment_scoring_key>().Count;
                    }
                }
                for (int index = 1; index <= num3; ++index)
                {
                    tbl_assessment_answer entity = new tbl_assessment_answer();
                    entity.answer_description = this.Request.Form["t-content-" + (object)index].ToString();
                    entity.id_assessment_question = new int?(question.id_assessment_question);
                    entity.id_assessment = question.id_assessment;
                    entity.position = new int?(index);
                    assessGroup2 = tblAssessment2.assess_group;
                    int num8 = 4;
                    int num9 = assessGroup2.GetValueOrDefault() == num8 ? (assessGroup2.HasValue ? 1 : 0) : 0;
                    entity.id_assessment_scoring_key = num9 == 0 ? new int?(0) : new int?(Convert.ToInt32(this.Request.Form["t-scoring-key-" + (object)index].ToString()));
                    entity.status = "A";
                    entity.updated_date_time = new DateTime?(DateTime.Now);
                    this.db.tbl_assessment_answer.Add(entity);
                    this.db.SaveChanges();
                    if (num4 == index)
                    {
                        question.aq_answer = Convert.ToString(entity.id_assessment_answer);
                        this.db.SaveChanges();
                    }
                }
            }
            return (ActionResult)this.RedirectToAction("assessmentQuestions", "Assessment", (object)new
            {
                id = question.id_assessment
            });
        }

        public string edit_assessment_question_action()
        {
            int int32_1 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            int assint = Convert.ToInt32(this.Request.Form["id-assessment"].ToString());
            tbl_assessment tblAssessment1 = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == assint)).FirstOrDefault<tbl_assessment>();
            int qtnId = Convert.ToInt32(this.Request.Form["id_hid_question"].ToString());
            tbl_assessment_question question = this.db.tbl_assessment_question.Where<tbl_assessment_question>((Expression<Func<tbl_assessment_question, bool>>)(t => t.id_assessment_question == qtnId && t.id_assessment == (int?)assint)).FirstOrDefault<tbl_assessment_question>();
            if (question != null)
            {
                question.assessment_question = this.Request.Form["q-title"].ToString();
                question.id_organization = new int?(int32_1);
                question.id_assessment = new int?(assint);
                int? assessGroup1 = tblAssessment1.assess_group;
                int num1 = 3;
                if ((assessGroup1.GetValueOrDefault() == num1 ? (assessGroup1.HasValue ? 1 : 0) : 0) != 0)
                {
                    int int32_2 = Convert.ToInt32(this.Request.Form["select-scoring-key"].ToString());
                    question.id_assessment_scoring_key = new int?(int32_2);
                }
                else
                    question.id_assessment_scoring_key = new int?(0);
                question.status = "A";
                question.updated_date_time = new DateTime?(DateTime.Now);
                if (((IEnumerable<string>)System.Web.HttpContext.Current.Request.Files.AllKeys).Any<string>())
                {
                    HttpPostedFile file1 = System.Web.HttpContext.Current.Request.Files["question-image"];
                    HttpPostedFile file2 = System.Web.HttpContext.Current.Request.Files["question-image"];
                    if (file1.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(file2.FileName);
                        if (!Directory.Exists(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Assessment/")))
                            Directory.CreateDirectory(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Assessment/"));
                        string filename = Path.Combine(this.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/Assessment/"), "aq_" + (object)question.id_assessment_question + extension);
                        file1.SaveAs(filename);
                        question.question_image = "aq_" + (object)question.id_assessment_question + extension;
                        this.db.SaveChanges();
                    }
                }
                this.db.SaveChanges();
                List<tbl_assessment_answer> list = this.db.tbl_assessment_answer.Where<tbl_assessment_answer>((Expression<Func<tbl_assessment_answer, bool>>)(t => t.id_assessment_question == (int?)question.id_assessment_question)).ToList<tbl_assessment_answer>();
                tbl_assessment tblAssessment2 = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => (int?)t.id_assessment == question.id_assessment)).FirstOrDefault<tbl_assessment>();
                int? assessGroup2 = tblAssessment2.assess_group;
                int num2 = 3;
                if ((assessGroup2.GetValueOrDefault() == num2 ? (!assessGroup2.HasValue ? 1 : 0) : 1) != 0)
                {
                    int num3 = 0;
                    int num4 = 0;
                    int? assessGroup3 = tblAssessment2.assess_group;
                    int num5 = 1;
                    int? assessGroup4;
                    if ((assessGroup3.GetValueOrDefault() == num5 ? (assessGroup3.HasValue ? 1 : 0) : 0) != 0)
                    {
                        num4 = Convert.ToInt32(this.Request.Form["select-answer"].ToString());
                        num3 = list.Count<tbl_assessment_answer>();
                    }
                    else
                    {
                        assessGroup4 = tblAssessment2.assess_group;
                        int num6 = 2;
                        if ((assessGroup4.GetValueOrDefault() == num6 ? (assessGroup4.HasValue ? 1 : 0) : 0) != 0)
                        {
                            num3 = this.db.tbl_assessment_scoring_key.Where<tbl_assessment_scoring_key>((Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == question.id_assessment)).ToList<tbl_assessment_scoring_key>().Count;
                        }
                        else
                        {
                            assessGroup4 = tblAssessment2.assess_group;
                            int num7 = 4;
                            if ((assessGroup4.GetValueOrDefault() == num7 ? (assessGroup4.HasValue ? 1 : 0) : 0) != 0)
                                num3 = this.db.tbl_assessment_scoring_key.Where<tbl_assessment_scoring_key>((Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == question.id_assessment)).ToList<tbl_assessment_scoring_key>().Count;
                        }
                    }
                    for (int index = 1; index <= num3; ++index)
                    {
                        int sAns = Convert.ToInt32(this.Request.Form["hid_answer_id_" + (object)index].ToString());
                        tbl_assessment_answer assessmentAnswer = this.db.tbl_assessment_answer.Where<tbl_assessment_answer>((Expression<Func<tbl_assessment_answer, bool>>)(t => t.id_assessment_answer == sAns)).FirstOrDefault<tbl_assessment_answer>();
                        if (assessmentAnswer != null)
                        {
                            assessmentAnswer.answer_description = this.Request.Form["t-content-" + (object)index].ToString();
                            assessmentAnswer.id_assessment_question = new int?(question.id_assessment_question);
                            assessmentAnswer.id_assessment = question.id_assessment;
                            assessmentAnswer.position = new int?(index);
                            assessGroup4 = tblAssessment2.assess_group;
                            int num8 = 4;
                            int num9 = assessGroup4.GetValueOrDefault() == num8 ? (assessGroup4.HasValue ? 1 : 0) : 0;
                            assessmentAnswer.id_assessment_scoring_key = num9 == 0 ? new int?(0) : new int?(Convert.ToInt32(this.Request.Form["t-scoring-key-" + (object)index].ToString()));
                            assessmentAnswer.status = "A";
                            assessmentAnswer.updated_date_time = new DateTime?(DateTime.Now);
                            this.db.SaveChanges();
                        }
                        if (num4 == index)
                        {
                            question.aq_answer = Convert.ToString(assessmentAnswer.id_assessment_answer);
                            this.db.SaveChanges();
                        }
                    }
                }
            }
            return "1";
        }

        public ActionResult edit_assessment_question(string id)
        {
            int ids = Convert.ToInt32(id);
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_assessment_question assessment = this.db.tbl_assessment_question.Where<tbl_assessment_question>((Expression<Func<tbl_assessment_question, bool>>)(t => t.id_assessment_question == ids && t.id_organization == (int?)oid)).FirstOrDefault<tbl_assessment_question>();
            if (assessment == null)
                return (ActionResult)this.RedirectToAction("Index");
            tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => (int?)t.id_assessment == assessment.id_assessment)).FirstOrDefault<tbl_assessment>();
            List<tbl_assessment_answer> list = this.db.tbl_assessment_answer.Where<tbl_assessment_answer>((Expression<Func<tbl_assessment_answer, bool>>)(t => t.id_assessment_question == (int?)assessment.id_assessment_question)).ToList<tbl_assessment_answer>();
            this.ViewData["assessment"] = (object)tblAssessment;
            this.ViewData["question"] = (object)assessment;
            this.ViewData["answers"] = (object)list;
            this.ViewData["scoringkey"] = (object)this.db.tbl_assessment_scoring_key.Where<tbl_assessment_scoring_key>((Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == assessment.id_assessment)).ToList<tbl_assessment_scoring_key>();
            return (ActionResult)this.View();
        }

        public int DeleteQuestion(int id)
        {
            tbl_assessment_question entity = this.db.tbl_assessment_question.Find(new object[1]
            {
        (object) id
            });
            if (entity == null)
                return 0;
            this.db.tbl_assessment_question.Remove(entity);
            this.db.SaveChanges();
            return 1;
        }

        public ActionResult publish_assessment()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "ASSESSMENT";
                string Id_assessment = this.Request.Form["asses-id"].ToString();
                string Id_category = null;
                string Id_operation = "Publish assessment";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }

            tbl_assessment tblAssessment = this.db.tbl_assessment.Find(new object[1]
            {
        (object) Convert.ToInt32(this.Request.Form["asses-id"].ToString())
            });
            if (tblAssessment != null)
            {
                tblAssessment.status = "A";
                this.db.SaveChanges();
            }
            return (ActionResult)this.RedirectToAction("Index");
        }

        public ActionResult submit_assesment()
        {
            tbl_assessment tblAssessment = this.db.tbl_assessment.Find(new object[1]
            {
        (object) Convert.ToInt32(this.Request.Form["asses-id"].ToString())
            });
            if (tblAssessment != null && tblAssessment.status != "A")
            {
                if (this.Request.Form["btn_submit"].ToString().Equals("edit"))
                    return (ActionResult)this.RedirectToAction("assessmentQuestions", "Assessment", (object)new
                    {
                        id = tblAssessment.id_assessment
                    });
                this.publish_assessment();
            }
            return (ActionResult)this.RedirectToAction("Index");
        }

        [RoleAccessController(KEY = 8)]
        public ActionResult AssessmentSheets()
        {
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            this.ViewData["assessment"] = (object)this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_organization == (int?)oid)).ToList<tbl_assessment>();
            return (ActionResult)this.View();
        }

        [HttpPost]
        public JsonResult NgageID(int Idgame, int Idorg)
        {
            Ngage_service kpiService2 = new Ngage_service();
            List<SelectListItem> GetgameList = kpiService2.GetgameId(Idgame, Idorg);

            if (GetgameList.Count == 0)
            {
                return Json(GetgameList);
            }

            return Json(GetgameList);
        }

        [HttpPost]
        public JsonResult NgageIDGame(int Idgame, int Idorg, int ngageDropdown)
        {
            Ngage_service kpiService2 = new Ngage_service();
            List<SelectListItem> GetgameListurl = kpiService2.GetgameUrl1(ngageDropdown);

            if (GetgameListurl.Count == 0)
            {
                return Json(GetgameListurl);
            }
            string GameUrl = GetgameListurl[0].Text;

            string redirectionUrl = $"{GameUrl}?OrgID={Idorg}&gameassid={Idgame}&idgame={ngageDropdown}";

            return Json(redirectionUrl);
        }

        [HttpPost]
        public JsonResult NgageOrgID(int Idorg)
        {
            Ngage_service kpiService2 = new Ngage_service();
            List<SelectListItem> AssList = kpiService2.GetAssList(Idorg);

            if (AssList.Count == 0)
            {
                return Json(AssList);
            }

            return Json(AssList);
        }
        public ActionResult createAssessmentSheets(string id)
        {
            int int32 = Convert.ToInt32(id);
            if (int32 > 0)
            {
                tbl_assessment tblAssessment = this.db.tbl_assessment.Find(new object[1]
                {
          (object) int32
                });
                int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
                this.ViewData["questionList"] = (object)this.db.tbl_assessment_question.Where<tbl_assessment_question>((Expression<Func<tbl_assessment_question, bool>>)(t => t.id_organization == (int?)orgid)).ToList<tbl_assessment_question>();
                this.ViewData["assessment"] = (object)tblAssessment;
            }
            return (ActionResult)this.View();
        }

        public string getAssessmentList()
        {
            string str1 = "";
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_assessment> list = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_organization == (int?)oid)).ToList<tbl_assessment>();
            string str2 = "";
            string assessmentList;
            if (list.Count > 0)
            {
                foreach (tbl_assessment tblAssessment in list)
                {
                    str1 += "<tr>";
                    str1 = str1 + "<td>" + tblAssessment.assessment_title + "</td>";
                    str1 = str1 + "<td>" + tblAssessment.assesment_description + "</td>";
                    string str3 = str1;
                    DateTime dateTime = tblAssessment.assess_start.Value;
                    string str4 = dateTime.ToString("dd-mm-yyyy");
                    str1 = str3 + "<td>" + str4 + "</td>";
                    string str5 = str1;
                    dateTime = tblAssessment.assess_ended.Value;
                    string str6 = dateTime.ToString("dd-mm-yyyy");
                    str1 = str5 + "<td>" + str6 + "</td>";
                    str1 += "</tr>";
                }
                assessmentList = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th>Assessment Title</th><th>Description</th><th>Start Date</th><th>End Date</th></tr></thead>" + "<tbody>" + str1 + "</tbody></table>";
            }
            else
                assessmentList = (str2 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th>There are no Assessment in the Sheet</th></tr></thead>") + "</table>";
            return assessmentList;
        }

        public string getAssessmentQuestionList()
        {
            string str1 = "";
            int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_assessment_question> list = this.db.tbl_assessment_question.Where<tbl_assessment_question>((Expression<Func<tbl_assessment_question, bool>>)(t => t.id_organization == (int?)oid)).ToList<tbl_assessment_question>();
            string str2 = "";
            string assessmentQuestionList;
            if (list.Count > 0)
            {
                foreach (tbl_assessment_question assessmentQuestion in list)
                {
                    str1 += "<tr>";
                    str1 = str1 + "<td>" + assessmentQuestion.assessment_question + "</td>";
                    str1 = str1 + "<td>" + assessmentQuestion.aq_answer + "</td>";
                    str1 += "</tr>";
                }
                assessmentQuestionList = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th>Question</th><th>Answer Option</th> </tr></thead>" + "<tbody>" + str1 + "</tbody></table>";
            }
            else
                assessmentQuestionList = (str2 = "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th>There are no Assessment Question in the Sheet</th></tr></thead>") + "</table>";
            return assessmentQuestionList;
        }

        public ActionResult LoadAssessment(string id)
        {
            int ids = Convert.ToInt32(id);
            Assessment assessment = new Assessment();
            tbl_assessment sheet = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == ids)).FirstOrDefault<tbl_assessment>();
            if (sheet == null)
                return (ActionResult)this.RedirectToAction("Index");
            assessment.tbl_assessment = sheet;
            List<AssessmentQuestion> assessmentQuestionList = new List<AssessmentQuestion>();
            DbSet<tbl_assessment_question> assessmentQuestion1 = this.db.tbl_assessment_question;
            Expression<Func<tbl_assessment_question, bool>> predicate = (Expression<Func<tbl_assessment_question, bool>>)(t => t.id_assessment == (int?)sheet.id_assessment);
            foreach (tbl_assessment_question assessmentQuestion2 in assessmentQuestion1.Where<tbl_assessment_question>(predicate).ToList<tbl_assessment_question>())
            {
                tbl_assessment_question item = assessmentQuestion2;
                AssessmentQuestion assessmentQuestion3 = new AssessmentQuestion();
                List<tbl_assessment_answer> list = this.db.tbl_assessment_answer.Where<tbl_assessment_answer>((Expression<Func<tbl_assessment_answer, bool>>)(t => t.id_assessment_question == (int?)item.id_assessment_question)).ToList<tbl_assessment_answer>();
                assessmentQuestion3.tbl_assessment_question = item;
                assessmentQuestion3.tbl_assessment_answer = list;
                assessmentQuestionList.Add(assessmentQuestion3);
            }
            assessment.assessment_question = assessmentQuestionList;
            this.ViewData["assessment"] = (object)assessment;
            return (ActionResult)this.View();
        }

        public ActionResult AssessmentToContent(string id)
        {
            int ids = Convert.ToInt32(id);
            tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == ids)).FirstOrDefault<tbl_assessment>();
            if (tblAssessment == null)
                return (ActionResult)this.RedirectToAction("Index");
            tbl_assessment_sheet AssSheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => t.id_assesment == ids)).FirstOrDefault<tbl_assessment_sheet>();
            this.ViewData["assessment"] = (object)tblAssessment;
            this.ViewData["assessment_sheet"] = (object)AssSheet;
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_assessment_mapping> list = this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_assessment_sheet == (int?)AssSheet.id_assessment_sheet && t.id_organization == (int?)orgid)).ToList<tbl_assessment_mapping>();
            List<tbl_content> tblContentList = new List<tbl_content>();
            foreach (tbl_assessment_mapping assessmentMapping in list)
            {
                tbl_assessment_mapping item = assessmentMapping;
                tbl_content tblContent = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => (int?)t.ID_CONTENT == item.id_content)).FirstOrDefault<tbl_content>();
                if (tblContent != null)
                    tblContentList.Add(tblContent);
            }
            this.ViewData["CategoryList"] = (object)this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            this.ViewData["contentlist"] = (object)tblContentList;
            return (ActionResult)this.View();
        }

        public string getAssessmentContent(string id, string pattern, string aid)
        {
            if (id == "")
                id = "0";
            int int32 = Convert.ToInt32(id);
            int aids = Convert.ToInt32(aid);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            pattern = pattern == null ? "" : pattern.Replace("'", "''");
            string sql = "select * from tbl_content where true  ";
            if (int32 > 0)
                sql = sql + " and id_content in (select id_content from tbl_content_organization_mapping where id_category=" + (object)int32 + ")";
            if (!string.IsNullOrEmpty(pattern))
                sql = sql + "and upper(CONTENT_QUESTION) like '%" + pattern + "%'";
            List<tbl_content> list1 = this.db.tbl_content.SqlQuery(sql).Take<tbl_content>(100).ToList<tbl_content>();
            string str = "";
            foreach (tbl_content tblContent in list1)
            {
                tbl_content item = tblContent;
                tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => t.id_assessment_sheet == aids)).FirstOrDefault<tbl_assessment_sheet>();
                this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == sheet.id_assesment)).FirstOrDefault<tbl_assessment>();
                List<tbl_assessment_mapping> list2 = this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_content == (int?)item.ID_CONTENT && t.id_organization == (int?)orgid)).ToList<tbl_assessment_mapping>();
                tbl_assessment_mapping assessmentMapping = this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_content == (int?)item.ID_CONTENT && t.id_assessment_sheet == (int?)sheet.id_assessment_sheet && t.id_organization == (int?)orgid)).FirstOrDefault<tbl_assessment_mapping>();
                str = str + "<tr><td><a target=\"_blank\" href=" + this.Url.Action("LoadContent", "contentDashboard", (object)new
                {
                    id = item.ID_CONTENT
                }) + ">" + item.CONTENT_QUESTION + "(" + (object)item.ID_CONTENT + ")</a></td>";
                str += "<td> ";
                str = assessmentMapping != null ? str + "Assessment Already attached" : (list2.Count != 0 ? str + "No. of Assessment attached : " + (object)list2.Count : str + "No Assessment attached");
                str += "</td><td> ";
                if (assessmentMapping == null)
                    str = str + "  <a href=\"#\" onclick=\"addAssessmentToContent(" + (object)item.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-plus\"></i></a>";
                else
                    str = str + "  <a href=\"#\" onclick=\"removeContenFromAssessment(" + (object)item.ID_CONTENT + ")\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                str += "</td></tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"><thead><tr><th width=\"45%\">Content</th><th width=\"45%\"></th><th width=\"8%\"></th></tr></thead>" + "<tbody>" + str + "</tbody></table>";
        }

        public string addAssessmentToContent(string id, string cid)
        {
            int ids = Convert.ToInt32(id);
            int cids = Convert.ToInt32(cid);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content content = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == cids)).FirstOrDefault<tbl_content>();
            if (content != null)
            {
                if (this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_content == (int?)content.ID_CONTENT && t.id_assessment_sheet == (int?)ids && t.id_organization == (int?)orgid)).FirstOrDefault<tbl_assessment_mapping>() == null)
                {
                    this.db.tbl_assessment_mapping.Add(new tbl_assessment_mapping()
                    {
                        id_assessment_sheet = new int?(ids),
                        id_content = new int?(content.ID_CONTENT),
                        id_organization = new int?(orgid),
                        status = "A",
                        updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public string removeContenFromAssessment(string id, string cid)
        {
            int ids = Convert.ToInt32(id);
            int cids = Convert.ToInt32(cid);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            tbl_content content = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == cids)).FirstOrDefault<tbl_content>();
            if (content != null)
            {
                tbl_assessment_mapping entity = this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_content == (int?)content.ID_CONTENT && t.id_assessment_sheet == (int?)ids && t.id_organization == (int?)orgid)).FirstOrDefault<tbl_assessment_mapping>();
                if (entity != null)
                {
                    this.db.tbl_assessment_mapping.Remove(entity);
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public string deleteAssessment(string id)
        {
            int aids = Convert.ToInt32(id);
            if (aids > 0)
            {
                int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
                tbl_assessment assess = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == aids)).FirstOrDefault<tbl_assessment>();
                if (assess != null)
                {
                    tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => t.id_assesment == assess.id_assessment)).FirstOrDefault<tbl_assessment_sheet>();
                    DbSet<tbl_assessment_scoring_key> assessmentScoringKey1 = this.db.tbl_assessment_scoring_key;
                    Expression<Func<tbl_assessment_scoring_key, bool>> predicate1 = (Expression<Func<tbl_assessment_scoring_key, bool>>)(t => t.id_assessment == (int?)assess.id_assessment);
                    foreach (tbl_assessment_scoring_key assessmentScoringKey2 in assessmentScoringKey1.Where<tbl_assessment_scoring_key>(predicate1).ToList<tbl_assessment_scoring_key>())
                    {
                        assessmentScoringKey2.status = "D";
                        this.db.SaveChanges();
                    }
                    DbSet<tbl_assessment_question> assessmentQuestion1 = this.db.tbl_assessment_question;
                    Expression<Func<tbl_assessment_question, bool>> predicate2 = (Expression<Func<tbl_assessment_question, bool>>)(t => t.id_assessment == (int?)assess.id_assessment && t.id_organization == (int?)orgid);
                    foreach (tbl_assessment_question assessmentQuestion2 in assessmentQuestion1.Where<tbl_assessment_question>(predicate2).ToList<tbl_assessment_question>())
                    {
                        tbl_assessment_question item = assessmentQuestion2;
                        item.status = "D";
                        this.db.SaveChanges();
                        DbSet<tbl_assessment_answer> assessmentAnswer1 = this.db.tbl_assessment_answer;
                        Expression<Func<tbl_assessment_answer, bool>> predicate3 = (Expression<Func<tbl_assessment_answer, bool>>)(t => t.id_assessment_question == (int?)item.id_assessment_question);
                        foreach (tbl_assessment_answer assessmentAnswer2 in assessmentAnswer1.Where<tbl_assessment_answer>(predicate3).ToList<tbl_assessment_answer>())
                        {
                            assessmentAnswer2.status = "D";
                            this.db.SaveChanges();
                        }
                        if (sheet != null)
                        {
                            DbSet<tbl_assessment_categoty_mapping> assessmentCategotyMapping1 = this.db.tbl_assessment_categoty_mapping;
                            Expression<Func<tbl_assessment_categoty_mapping, bool>> predicate4 = (Expression<Func<tbl_assessment_categoty_mapping, bool>>)(t => t.id_assessment_sheet == sheet.id_assessment_sheet);
                            foreach (tbl_assessment_categoty_mapping assessmentCategotyMapping2 in assessmentCategotyMapping1.Where<tbl_assessment_categoty_mapping>(predicate4).ToList<tbl_assessment_categoty_mapping>())
                            {
                                assessmentCategotyMapping2.status = "D";
                                this.db.SaveChanges();
                            }
                            DbSet<tbl_assessment_mapping> assessmentMapping1 = this.db.tbl_assessment_mapping;
                            Expression<Func<tbl_assessment_mapping, bool>> predicate5 = (Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_assessment_sheet == (int?)sheet.id_assessment_sheet && t.id_organization == (int?)orgid);
                            foreach (tbl_assessment_mapping assessmentMapping2 in assessmentMapping1.Where<tbl_assessment_mapping>(predicate5).ToList<tbl_assessment_mapping>())
                            {
                                assessmentMapping2.status = "D";
                                this.db.SaveChanges();
                            }
                            DbSet<tbl_assessment_push> tblAssessmentPush1 = this.db.tbl_assessment_push;
                            Expression<Func<tbl_assessment_push, bool>> predicate6 = (Expression<Func<tbl_assessment_push, bool>>)(t => t.id_assesment_sheet == sheet.id_assessment_sheet);
                            foreach (tbl_assessment_push tblAssessmentPush2 in tblAssessmentPush1.Where<tbl_assessment_push>(predicate6).ToList<tbl_assessment_push>())
                            {
                                tblAssessmentPush2.status = "D";
                                this.db.SaveChanges();
                            }
                            DbSet<tbl_assessment_general> assessmentGeneral1 = this.db.tbl_assessment_general;
                            Expression<Func<tbl_assessment_general, bool>> predicate7 = (Expression<Func<tbl_assessment_general, bool>>)(t => t.id_assesment_sheet == sheet.id_assessment_sheet && t.id_organization == (int?)orgid);
                            foreach (tbl_assessment_general assessmentGeneral2 in assessmentGeneral1.Where<tbl_assessment_general>(predicate7).ToList<tbl_assessment_general>())
                            {
                                assessmentGeneral2.status = "D";
                                this.db.SaveChanges();
                            }
                        }
                    }
                    if (sheet != null)
                    {
                        sheet.status = "D";
                        this.db.SaveChanges();
                    }
                    assess.status = "D";
                    this.db.SaveChanges();
                }
            }
            return "";
        }

        public ActionResult ContentToAssessment(string id)
        {
            int ids = Convert.ToInt32(id);
            List<tbl_assessment> tblAssessmentList1 = new List<tbl_assessment>();
            tbl_content content = this.db.tbl_content.Where<tbl_content>((Expression<Func<tbl_content, bool>>)(t => t.ID_CONTENT == ids)).FirstOrDefault<tbl_content>();
            if (content == null)
                return (ActionResult)this.RedirectToAction("Index", "dashboard");
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_assessment> list1 = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_organization == (int?)orgid && t.status == "A")).ToList<tbl_assessment>();
            List<tbl_assessment_mapping> list2 = this.db.tbl_assessment_mapping.Where<tbl_assessment_mapping>((Expression<Func<tbl_assessment_mapping, bool>>)(t => t.id_content == (int?)content.ID_CONTENT && t.id_organization == (int?)orgid)).ToList<tbl_assessment_mapping>();
            List<tbl_assessment> tblAssessmentList2 = new List<tbl_assessment>();
            foreach (tbl_assessment_mapping assessmentMapping in list2)
            {
                tbl_assessment_mapping item = assessmentMapping;
                tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => (int?)t.id_assessment_sheet == item.id_assessment_sheet)).FirstOrDefault<tbl_assessment_sheet>();
                if (sheet != null)
                {
                    tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == sheet.id_assesment)).FirstOrDefault<tbl_assessment>();
                    if (tblAssessment != null)
                        tblAssessmentList2.Add(tblAssessment);
                }
            }
            this.ViewData["assessData"] = (object)tblAssessmentList2;
            this.ViewData["assessment"] = (object)list1;
            this.ViewData["content"] = (object)content;
            return (ActionResult)this.View();
        }

        public ActionResult CategoryToAssessment()
        {
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_category> list1 = this.db.tbl_category.SqlQuery("select * from tbl_category where ID_ORGANIZATION=" + (object)orgid + " AND CATEGORY_TYPE in (0,1,2) AND status='A' order by CATEGORYNAME").ToList<tbl_category>();
            List<tbl_assessment_sheet> list2 = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => t.id_organization == (int?)orgid && t.status == "A")).ToList<tbl_assessment_sheet>();
            List<AssessmentSheet> assessmentSheetList = new List<AssessmentSheet>();
            foreach (tbl_assessment_sheet tblAssessmentSheet in list2)
            {
                tbl_assessment_sheet item = tblAssessmentSheet;
                tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == item.id_assesment && t.status == "A")).FirstOrDefault<tbl_assessment>();
                if (tblAssessment != null)
                    assessmentSheetList.Add(new AssessmentSheet()
                    {
                        Sheet = item,
                        Assessment = tblAssessment
                    });
            }
            this.ViewData["assessmentList"] = (object)assessmentSheetList;
            this.ViewData["category"] = (object)list1;
            return (ActionResult)this.View();
        }

        public string addAssessmentToCategory(string cid, string aid)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "ASSESSMENT TO CATEGORY ASSIGMENT";
                string Id_assessment = aid;
                string Id_category = cid;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            int cids = Convert.ToInt32(cid);
            int aids = Convert.ToInt32(aid);
            string category = "0";
            tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => t.id_assesment == aids && t.status == "A")).FirstOrDefault<tbl_assessment_sheet>();
            if (sheet != null)
            {
                if (this.db.tbl_assessment_categoty_mapping.Where<tbl_assessment_categoty_mapping>((Expression<Func<tbl_assessment_categoty_mapping, bool>>)(t => t.id_assessment_sheet == sheet.id_assessment_sheet && t.id_category == cids)).FirstOrDefault<tbl_assessment_categoty_mapping>() == null)
                {
                    this.db.tbl_assessment_categoty_mapping.Add(new tbl_assessment_categoty_mapping()
                    {
                        id_category = cids,
                        id_assessment_sheet = sheet.id_assessment_sheet,
                        id_assessment = new int?(aids),
                        status = "A",
                        updated_date_time = new DateTime?(DateTime.Now)
                    });
                    this.db.SaveChanges();
                    category = "1";
                }
                else
                    category = "2";
            }
            return category;
        }

        public string removeAssessmentToCategory(string cid, string aid)
        {
            int cids = Convert.ToInt32(cid);
            int aids = Convert.ToInt32(aid);
            string category = "0";
            tbl_assessment_sheet sheet = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => t.id_assessment_sheet == aids && t.status == "A")).FirstOrDefault<tbl_assessment_sheet>();
            if (sheet != null)
            {
                tbl_assessment_categoty_mapping entity = this.db.tbl_assessment_categoty_mapping.Where<tbl_assessment_categoty_mapping>((Expression<Func<tbl_assessment_categoty_mapping, bool>>)(t => t.id_assessment == (int?)sheet.id_assesment && t.id_category == cids)).FirstOrDefault<tbl_assessment_categoty_mapping>();
                if (entity != null)
                {
                    this.db.tbl_assessment_categoty_mapping.Remove(entity);
                    this.db.SaveChanges();
                    category = "1";
                }
            }
            return category;
        }

        public string getAssessmentForCategoryList(string id)
        {
            int cids = Convert.ToInt32(id);
            int orgid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_assessment_sheet> list = this.db.tbl_assessment_sheet.Where<tbl_assessment_sheet>((Expression<Func<tbl_assessment_sheet, bool>>)(t => t.id_organization == (int?)orgid && t.status == "A")).ToList<tbl_assessment_sheet>();
            List<AssessmentSheet> assessmentSheetList = new List<AssessmentSheet>();
            foreach (tbl_assessment_sheet tblAssessmentSheet in list)
            {
                tbl_assessment_sheet item = tblAssessmentSheet;
                tbl_assessment tblAssessment = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>)(t => t.id_assessment == item.id_assesment && t.status == "A")).FirstOrDefault<tbl_assessment>();
                if (tblAssessment != null)
                {
                    tbl_assessment_categoty_mapping assessmentCategotyMapping = this.db.tbl_assessment_categoty_mapping.Where<tbl_assessment_categoty_mapping>((Expression<Func<tbl_assessment_categoty_mapping, bool>>)(t => t.id_assessment == (int?)item.id_assesment && t.id_category == cids)).FirstOrDefault<tbl_assessment_categoty_mapping>();
                    assessmentSheetList.Add(new AssessmentSheet()
                    {
                        Sheet = item,
                        Assessment = tblAssessment,
                        FLAG = assessmentCategotyMapping != null
                    });
                }
            }
            string str = "";
            foreach (AssessmentSheet assessmentSheet in assessmentSheetList)
            {
                str = str + " <tr><td>" + assessmentSheet.Assessment.assessment_title + "</td>";
                str += " <td>";
                if (assessmentSheet.FLAG)
                {
                    str = str + " <a  style=\"display:none;\" id=\"add_" + (object)assessmentSheet.Assessment.id_assessment + "\" href=\"#\" onclick=\"addAssessmentCategory('" + (object)assessmentSheet.Assessment.id_assessment + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a>";
                    str = str + " <i id=\"done_" + (object)assessmentSheet.Assessment.id_assessment + "\" class=\"glyphicon glyphicon-ok\"></i>";
                    str = str + " <a id=\"link_" + (object)assessmentSheet.Assessment.id_assessment + "\" href=\"#\" onclick=\"removeAssessmentCategory('" + (object)assessmentSheet.Assessment.id_assessment + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                }
                else
                {
                    str = str + " <a id=\"add_" + (object)assessmentSheet.Assessment.id_assessment + "\" href=\"#\" onclick=\"addAssessmentCategory('" + (object)assessmentSheet.Assessment.id_assessment + "')\"><i class=\"glyphicon glyphicon-plus-sign\"></i></a>";
                    str = str + " <i style=\"display:none;\"  id=\"done_" + (object)assessmentSheet.Assessment.id_assessment + "\" class=\"glyphicon glyphicon-ok\"></i>";
                    str = str + " <a style=\"display:none;\"  id=\"link_" + (object)assessmentSheet.Assessment.id_assessment + "\" href=\"#\" onclick=\"removeAssessmentCategory('" + (object)assessmentSheet.Assessment.id_assessment + "')\"><i class=\"glyphicon glyphicon-remove\"></i></a>";
                }
                str += " </td></tr>";
            }
            return "<table id=\"report-table\" class=\"table table-striped table-bordered dataTable small\" cellspacing=\"0\" width=\"100%\"> <thead>  <tr><th width=\"95%\">Assesment</th><th width=\"5%\">Action</th> </tr></thead> <tbody>" + str + " </tbody></table>";
        }

        public string getAssessments(int cat)
        {
            Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            List<tbl_assessment_categoty_mapping> list = this.db.tbl_assessment_categoty_mapping.SqlQuery("select * from tbl_assessment_categoty_mapping where id_category=" + (object)cat + " AND status='A'").ToList<tbl_assessment_categoty_mapping>();
            string assessments = "";
            foreach (tbl_assessment_categoty_mapping assessmentCategotyMapping in list)
            {
                tbl_assessment assName = new BuisinessLogic().getAssName("select * from tbl_assessment where id_assessment=" + (object)assessmentCategotyMapping.id_assessment + " AND status='A'");
                assessments = "<option value=" + (object)assName.id_assessment + ">" + assName.assessment_title + "</option>";
            }
            return assessments;
        }
    }
}
