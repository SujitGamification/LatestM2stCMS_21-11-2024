using m2ostnext.Models;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class FeedbackConfigurationController : Controller
    {
      
        // GET: FeedbackConfiguration
        public ActionResult FeedbackIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Feedback Configuration";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            addFeedbackModel feedbackModel = new addFeedbackModel();
            List<tbl_feedback_configuration> feedbackList = feedbackModel.GetFeedbackData(num);
            ViewData["feedbackList"] = feedbackList;
            // Pass the data to the view
            return View();
        }

        public ActionResult SaveFeedbackConfiguration(tbl_feedback_configuration form)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Feedback Configuration";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            tbl_feedback_configuration temp = new tbl_feedback_configuration();
            temp.Id_Feedback = form.Id_Feedback;
            temp.Organization_id = form.Organization_id;
            string imageFileName = null;
            string imagePathInDatabase = null;
            string imagePathForDatabase = null;

            if (form.Id_Feedback != 0)
            {
                if(form.ImageFile != null)
                {
                    if (form.ImageFile != null && form.ImageFile.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/feedbackimage"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        imageFileName = Path.GetFileName(form.ImageFile.FileName);
                        string imagePath = Path.Combine(serverPath, imageFileName);
                        form.ImageFile.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        imagePathInDatabase = Path.Combine(uploadDir, imageFileName); // Store the relative path
                    }

                    //imagePathForDatabase = imagePathInDatabase.Replace("\\", "/");


                    temp.Image_path = form.ImageFile.FileName;
                    temp.Header_Text = form.Header_Text;
                    temp.Feedback_Text = form.Feedback_Text;
                    temp.Text_Button_Colour = form.Text_Button_Colour;
                }
                else
                {
                    temp.Image_path = form.Image_path;
                    temp.Header_Text = form.Header_Text;
                    temp.Feedback_Text = form.Feedback_Text;
                    temp.Text_Button_Colour = form.Text_Button_Colour;

                }
            }
            else {

                if (form.ImageFile != null && form.ImageFile.ContentLength > 0)
                {
                    string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/feedbackimage"; // Specify the directory where you want to save the images
                    string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    imageFileName = Path.GetFileName(form.ImageFile.FileName);
                    string imagePath = Path.Combine(serverPath, imageFileName);
                    form.ImageFile.SaveAs(imagePath);

                    // Store the path of the saved image in the database
                    imagePathInDatabase = Path.Combine(uploadDir, imageFileName);
                    //imagePathForDatabase = imagePathInDatabase.Replace("\\", "/");
                    
                }



                temp.Image_path = form.ImageFile.FileName;
                temp.Header_Text = form.Header_Text;
                temp.Feedback_Text = form.Feedback_Text;
                temp.Text_Button_Colour = form.Text_Button_Colour;


            }
            return new addFeedbackModel().add_feedback(temp).Equals("TRUE") ? (ActionResult)this.RedirectToAction("FeedbackIndex") : (ActionResult)this.RedirectToAction("FeedbackIndex1");


            //temp.Organization =
            //// Save data to MySQL database
            //string connectionString = "YourConnectionString"; // Replace with your MySQL connection string
            //using (var connection = new MySqlConnection(connectionString))
            //{
            //    string query = "INSERT INTO FeedbackTable (Organization, ImagePath, HeaderText, FeedbackText, TextButtonColor) VALUES (@Organization, @ImagePath, @HeaderText, @FeedbackText, @TextButtonColor)";

            //    using (var command = new MySqlCommand(query, connection))
            //    {
            //        // Add parameters to the command
            //        command.Parameters.AddWithValue("@Organization", organization);
            //        command.Parameters.AddWithValue("@ImagePath", imagePathInDatabase);
            //        command.Parameters.AddWithValue("@HeaderText", headtext);
            //        command.Parameters.AddWithValue("@FeedbackText", feedback);
            //        command.Parameters.AddWithValue("@TextButtonColor", txtbutton);

            //        try
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //        }
            //        catch (Exception ex)
            //        {
            //            // Handle exceptions
            //            Console.WriteLine("Error: " + ex.Message);
            //        }
            //    }
            //}

            // Redirect to the feedback index page
            // return RedirectToAction("FeedbackIndex");
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            addFeedbackModel feedbackModel = new addFeedbackModel();
            feedbackModel.DeleteFeedback(id);
            return Json(new { success = true });
           
        }

    }
}