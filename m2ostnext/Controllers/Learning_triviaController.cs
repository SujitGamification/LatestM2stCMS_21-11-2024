using m2ostnext.Models;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class Learning_triviaController : Controller
    {
        // GET: Learning_trivia_
        public ActionResult QuestionIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_Question";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }
            int Orgid = Convert.ToInt32(userSession.id_ORGANIZATION);
            var model = new tbl_learning_question
            {
                IdLearningSubCategory = 0,
                IdLearningCategoryList = GetCategories(Orgid), 
                IdLearningSubCategoryList = new List<SelectListItem>()  
            };

            Addlearning_questionModel QuestionModel = new Addlearning_questionModel();

            List<tbl_learning_question> QuestionModelList = QuestionModel.QuestionList(Orgid);

            ViewData["QuestionModelList"] = QuestionModelList;

            return View(model);
        }

        [HttpPost]
        public ActionResult QuestionDeleteConfiguration(int num)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_Question";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Delete";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            bool QuestionAddedSuccessfully = new Addlearning_questionModel().Deletelearning_Question(num).Equals("TRUE");

            if (QuestionAddedSuccessfully)
            {

                return Json(new { success = true });
            }
            else
            {
                return (ActionResult)this.RedirectToAction("CategoryIndex");
            }


        }


        [HttpPost]
        public ActionResult QuestionSaveConfiguration(tbl_learning_question model)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_Question";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            tbl_learning_question temp = new tbl_learning_question();
            temp.IdLearningQuestion = model.IdLearningQuestion;
            temp.IdOrganization = Convert.ToInt32(userSession.id_ORGANIZATION);
            temp.IdCmsUser= Convert.ToInt32(userSession.ID_USER);
            temp.IdLearningCategory = model.IdLearningCategory;
            temp.IdLearningSubCategory = model.IdLearningSubCategory;
            temp.Title = model.Title;
            temp.Question = model.Question;
            temp.Numberofattempts = model.Numberofattempts;

            string imageFileName = null;
            string imagePathInDatabase = null;
            string imagePathForDatabase = null;
            // option Upload Image
            if (model.OptionNumber == 1)
            {
                temp.OptionNumber = 1;
                if (model.IdLearningQuestion != 0)
                {
                    if (model.ImageFile != null)
                    {
                        if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                        {
                            string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/LearingTriviaImage"; // Specify the directory where you want to save the images
                            string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                            if (!Directory.Exists(serverPath))
                            {
                                Directory.CreateDirectory(serverPath);
                            }

                            imageFileName = Path.GetFileName(model.ImageFile.FileName);
                            string imagePath = Path.Combine(serverPath, imageFileName);
                            model.ImageFile.SaveAs(imagePath);

                            // Store the path of the saved image in the database
                            imagePathInDatabase = Path.Combine(uploadDir, imageFileName); // Store the relative path
                        }

                        //imagePathForDatabase = imagePathInDatabase.Replace("\\", "/");


                        temp.Image_path = model.ImageFile.FileName;
                        temp.YoutubeUrl = null;
                        temp.VideoUrl = null;

                    }
                    else
                    {
                        temp.Image_path = model.Image_path;
                        temp.YoutubeUrl = null;
                        temp.VideoUrl = null;


                    }
                }
                else
                {

                    if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/LearingTriviaImage"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        imageFileName = Path.GetFileName(model.ImageFile.FileName);
                        string imagePath = Path.Combine(serverPath, imageFileName);
                        model.ImageFile.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        imagePathInDatabase = Path.Combine(uploadDir, imageFileName);
                        //imagePathForDatabase = imagePathInDatabase.Replace("\\", "/");

                    }



                    temp.Image_path = model.ImageFile.FileName;
                    temp.YoutubeUrl = null;
                    temp.VideoUrl = null;



                }
            }
            else if (model.OptionNumber == 2)
            {
                temp.OptionNumber = 2;
                //VideoImage
                if (model.videlImageFile != null)
                {
                    if (model.videlImageFile != null && model.videlImageFile.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/LearingTriviaImage"; // Specify the directory where you want to save the videos
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        string videoFileName = Path.GetFileName(model.videlImageFile.FileName);
                        string videoPath = Path.Combine(serverPath, videoFileName);
                        model.videlImageFile.SaveAs(videoPath);

                        // Store the path of the saved video in the database
                        temp.VideoUrl = videoFileName;
                        temp.YoutubeUrl = null;
                        temp.Image_path = null;
                    }

                }
                else
                {
                    temp.VideoUrl = model.VideoUrl;
                    temp.YoutubeUrl = null;
                    temp.Image_path = null;
                }

            }
            else 
            {
                //Youtube
                temp.OptionNumber = 3;
                temp.YoutubeUrl = model.YoutubeUrl;
                temp.VideoUrl = null;
                temp.Image_path = null;
            }

         

         

         
          



           





            //option radio
            if (model.OptionNumber_QA == 2)
            {
                temp.OptionNumber_QA = 2;
            }
            else 
            {
                temp.OptionNumber_QA = 4;
            }

         





            temp.Points= model.Points;  
            temp.CreatedDate = DateTime.Now;
            temp.IdLearningQuestionAnswer = model.IdLearningQuestionAnswer;

            //for the Question Answer
            List<string> QA = new List<string>();

            
            QA.Add(model.OptionAnswer_1);
            QA.Add(model.OptionAnswer_2);
            QA.Add(model.OptionAnswer_3);
            QA.Add(model.OptionAnswer_4);

         
            string[] QAArray = QA.ToArray();


            //

            //if (model.IsCorrectAnswer_1 == true)
            //{
            //    temp.IsCorrectAnswer = 1;
            //}
            //else if (model.IsCorrectAnswer_2 == true)
            //{
            //    temp.IsCorrectAnswer = 2;
            //}
            //else if (model.IsCorrectAnswer_3 == true)
            //{
            //    temp.IsCorrectAnswer = 3;
            //}
            //else
            //{
            //    temp.IsCorrectAnswer = 4;
            //}

            //


            bool CategoryAddedSuccessfully = new Addlearning_questionModel().Addlearning_question(temp, QAArray, model).Equals("TRUE");
        

            if (CategoryAddedSuccessfully)
            {

                TempData["MessageVideo"] = "Data Inserted Successfully";
                return RedirectToAction("QuestionIndex");
            }
            else
            {
                return (ActionResult)this.RedirectToAction("QuestionIndex");
            }


        }


        public JsonResult GetQuestionAnswer(int id)
        {
            //UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            //int Orgid = Convert.ToInt32(userSession.id_ORGANIZATION);

            Addlearning_questionModel QuestionModel = new Addlearning_questionModel();

            List<tbl_learning_question> QuestionAnswerlList = QuestionModel.GetQuestionAnswer(id);

           
            return Json(QuestionAnswerlList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CategoryIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_Category List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
               
            }

            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];

            int num = Convert.ToInt32(item.id_ORGANIZATION);

            Convert.ToInt32(item.ID_USER);

            Addlearning_categoryModel CategoryModel = new Addlearning_categoryModel();

            List<tbl_learning_category> CategoryList = CategoryModel.GetCategoryData(num);

            ViewData["CategoryList"] = CategoryList;

            // Pass the data to the view

            return View();
        }

        [HttpPost]
        public ActionResult CategorySaveConfiguration(tbl_learning_category model)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_Category";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }
           
            tbl_learning_category temp = new tbl_learning_category();
            temp.IdLearningCategory = model.IdLearningCategory;
            temp.IdOrganization = Convert.ToInt32(userSession.id_ORGANIZATION);
            temp.CategoryName = model.CategoryName;
            temp.CategoryDescription = model.CategoryDescription;
            temp.IdCmsUser = Convert.ToInt32(userSession.ID_USER);
            temp.CreatedDate = DateTime.Now;
            temp.CategoryOrderNumber = model.CategoryOrderNumber;



            bool CategoryAddedSuccessfully = new Addlearning_categoryModel().Addlearning_category(temp).Equals("TRUE");

            if (CategoryAddedSuccessfully)
            {

                TempData["MessageVideo"] = "Data Inserted Successfully";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                return (ActionResult)this.RedirectToAction("CategoryIndex");
            }
          

        }

        [HttpPost]
        public ActionResult CategoryDeleteConfiguration(int num)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_Category";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Delete";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            bool CategoryAddedSuccessfully = new Addlearning_categoryModel().Deletelearning_category(num).Equals("TRUE");

            if (CategoryAddedSuccessfully)
            {

                return Json(new { success = true });
            }
            else
            {
                return (ActionResult)this.RedirectToAction("CategoryIndex");
            }


        }

        [HttpPost]
        public ActionResult UpdateCategoryOrder(int id,int newOrder)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_Category";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Update";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }




            bool CategoryAddedSuccessfully = new Addlearning_categoryModel().Updatelearning_category(id, newOrder).Equals("TRUE");

            if (CategoryAddedSuccessfully)
            {

                return Json(new { success = true });
            }
            else
            {
                return (ActionResult)this.RedirectToAction("CategoryIndex");
            }


        }



        public ActionResult sub_categoryIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_SubCategory";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }
            int Orgid =Convert.ToInt32(userSession.id_ORGANIZATION);
            var model = new tbl_learning_sub_category
            {
                IdLearningCategoryList = GetCategories(Orgid)// Method to populate dropdown items
            };
            tbl_learning_sub_categoryModel SubCategoryModel = new tbl_learning_sub_categoryModel();

            List<tbl_learning_sub_category> SubCategoryList = SubCategoryModel.sub_categoryList(Orgid);

            ViewData["SubCategoryList"] = SubCategoryList;

            return View(model);
            
        }

        private IEnumerable<SelectListItem> GetCategories(int Orgid)
        {

            return new tbl_learning_sub_categoryModel().GetCategories(Orgid);

        }

        public JsonResult GetSubCategoriesQ(int id,int sb)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            int Orgid = Convert.ToInt32(userSession.id_ORGANIZATION);
            // Fetch the subcategories from the database or any data source
            var subCategories = new tbl_learning_sub_categoryModel().GetSubCategoriesQ(Orgid, id,sb);

            // Return the subcategories as a JSON result
            return Json(subCategories, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetSubCategories(int id)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            int Orgid = Convert.ToInt32(userSession.id_ORGANIZATION);
            // Fetch the subcategories from the database or any data source
            var subCategories = new tbl_learning_sub_categoryModel().GetSubCategories(Orgid, id);

            // Return the subcategories as a JSON result
            return Json(subCategories, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult SubCategoryConfiguration(tbl_learning_sub_category model)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_SubCategory";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            tbl_learning_sub_category temp = new tbl_learning_sub_category();
            temp.IdLearningSubCategory = model.IdLearningSubCategory;
            temp.IdLearningCategory = model.IdLearningCategory;
            temp.IdOrganization = Convert.ToInt32(userSession.id_ORGANIZATION);
            temp.SubCategoryName = model.SubCategoryName;
            temp.SubCategoryDescription = model.SubCategoryDescription;
            temp.IdCmsUser = Convert.ToInt32(userSession.ID_USER);
            temp.CreatedDate = DateTime.Now;
      



            bool CategoryAddedSuccessfully = new tbl_learning_sub_categoryModel().Addlearning_sub_category(temp).Equals("TRUE");

            if (CategoryAddedSuccessfully)
            {

                TempData["MessageVideo"] = "Data Inserted Successfully";
                return RedirectToAction("sub_categoryIndex");
            }
            else
            {
                return (ActionResult)this.RedirectToAction("sub_categoryIndex");
            }


        }

        [HttpPost]
        public ActionResult SubCategoryDeleteConfiguration(int num)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Learning_trivia_SubCategory";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Delete";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);

            }

            bool CategoryAddedSuccessfully = new tbl_learning_sub_categoryModel().SubDeletelearning_category(num).Equals("TRUE");

            if (CategoryAddedSuccessfully)
            {

                return Json(new { success = true });
            }
            else
            {
                return (ActionResult)this.RedirectToAction("CategoryIndex");
            }


        }

    }
}