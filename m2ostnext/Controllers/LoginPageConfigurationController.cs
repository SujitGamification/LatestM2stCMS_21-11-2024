using m2ostnext.Models;

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

    public class LoginPageConfigurationController : Controller

    {

        // GET: FeedbackConfiguration

        public ActionResult LoginIndex()

        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Login Page Configuration";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];

            int num = Convert.ToInt32(item.id_ORGANIZATION);

            Convert.ToInt32(item.ID_USER);

            addLoginModel loginModel = new addLoginModel();

            List<tbl_Login_page> loginPageList = loginModel.GetLoginPageData(num);

            ViewData["loginPageList"] = loginPageList;

            // Pass the data to the view

            return View();

        }





        [HttpPost]

        public ActionResult SetLoginPage(tbl_Login_page model)

        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "LOGIN PAGE";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            tbl_Login_page temp = new tbl_Login_page();

            string BGimageFileName = null;

            temp.Id_login = model.Id_login;

            temp.Organization = model.Organization;

            string BGimagePathForDatabase = null;

            string LogoimageFileName = null;

            string LogoimagePathForDatabase = null;

            string filePath = model.bgImage_pathhidden;

            // Get the file name from the file path
            string ImgeName = Path.GetFileName(filePath);

            string fileLogoPath = model.logoImage_pathhidden;

            // Get the file name from the file path
            string ImgeLogoName = Path.GetFileName(fileLogoPath);

            if (model.Id_login != 0)

            {

                if (ModelState.IsValid)

                {

                    // Process uploaded images

                    if (model.BackgroundFile != null)

                    {

                        if (model.BackgroundFile != null && model.BackgroundFile.ContentLength > 0)

                        {

                            string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/LoginImage"; // Specify the directory where you want to save the images

                            string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path



                            if (!Directory.Exists(serverPath))

                            {

                                Directory.CreateDirectory(serverPath);

                            }



                            BGimageFileName = Path.GetFileName(model.BackgroundFile.FileName);

                            string imagePath = Path.Combine(serverPath, BGimageFileName);

                            model.BackgroundFile.SaveAs(imagePath);

                            // Store the path of the saved image in the database

                            string BGimagePathInDatabase = Path.Combine(uploadDir, BGimageFileName); // Store the relative path

                            BGimagePathForDatabase = BGimagePathInDatabase.Replace("\\", "/");

                            temp.Background_Image = BGimageFileName;

                         


                        }

                    }

                    else

                    {

                        temp.Background_Image = ImgeName;



                    }



                    if (model.LogoFile != null)

                    {

                        if (model.LogoFile != null && model.LogoFile.ContentLength > 0)

                        {

                            string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/LoginImage"; // Specify the directory where you want to save the images

                            string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path



                            if (!Directory.Exists(serverPath))

                            {

                                Directory.CreateDirectory(serverPath);

                            }



                            LogoimageFileName = Path.GetFileName(model.LogoFile.FileName);

                            string imagePath = Path.Combine(serverPath, LogoimageFileName);

                            model.LogoFile.SaveAs(imagePath);



                            // Store the path of the saved image in the database

                            string LogoimagePathInDatabase = Path.Combine(uploadDir, LogoimageFileName); // Store the relativ

                            LogoimagePathForDatabase = LogoimagePathInDatabase.Replace("\\", "/");

                            temp.Logo_Image = LogoimageFileName;

                        }

                    }

                    else

                    {

                        temp.Logo_Image = ImgeLogoName;



                    }



                }



                temp.Organization = model.Organization;

                temp.Text_Button_Color = model.Text_Button_Color;

            }

            else

            {

                if (model.BackgroundFile != null && model.BackgroundFile.ContentLength > 0)

                {

                    string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/LoginImage"; // Specify the directory where you want to save the images

                    string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path



                    if (!Directory.Exists(serverPath))

                    {

                        Directory.CreateDirectory(serverPath);

                    }



                    BGimageFileName = Path.GetFileName(model.BackgroundFile.FileName);

                    string imagePath = Path.Combine(serverPath, BGimageFileName);

                    model.BackgroundFile.SaveAs(imagePath);

                    // Store the path of the saved image in the database

                    string BGimagePathInDatabase = Path.Combine(uploadDir, BGimageFileName); // Store the relative path

                    BGimagePathForDatabase = BGimagePathInDatabase.Replace("\\", "/");



                }

                if (model.LogoFile != null && model.LogoFile.ContentLength > 0)

                {

                    string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/LoginImage"; // Specify the directory where you want to save the images

                    string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path



                    if (!Directory.Exists(serverPath))

                    {

                        Directory.CreateDirectory(serverPath);

                    }



                    LogoimageFileName = Path.GetFileName(model.LogoFile.FileName);

                    string imagePath = Path.Combine(serverPath, LogoimageFileName);

                    model.LogoFile.SaveAs(imagePath);



                    // Store the path of the saved image in the database

                    string LogoimagePathInDatabase = Path.Combine(uploadDir, LogoimageFileName); // Store the relativ

                    LogoimagePathForDatabase = LogoimagePathInDatabase.Replace("\\", "/");

                }

                temp.Background_Image = BGimageFileName;

                temp.Logo_Image = LogoimageFileName;

                temp.Organization = model.Organization;

                temp.Text_Button_Color = model.Text_Button_Color;

            }

            return new addLoginModel().add_login(temp).Equals("TRUE") ? (ActionResult)this.RedirectToAction("LoginIndex") : (ActionResult)this.RedirectToAction("LoginIndex1");





        }



        [HttpPost]

        public ActionResult Delete(int id)

        {

            addLoginModel loginModel = new addLoginModel();

            loginModel.DeleteLogin(id);

            return Json(new { success = true });



        }

    }



}