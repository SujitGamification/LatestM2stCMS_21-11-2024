using m2ostnext.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    public class VideoConfigurationController : Controller
    {
        // GET: VideoConfiguration
        public ActionResult VideoIndex()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Iitialization Video List";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];

            int num = Convert.ToInt32(item.id_ORGANIZATION);

            Convert.ToInt32(item.ID_USER);

            addVideoModel VideoModel = new addVideoModel();

            List<tbl_video_configuration> VideoList = VideoModel.GetVideoData(num);

            ViewData["VideoList"] = VideoList;

            // Pass the data to the view

            return View();
        }

        public ActionResult SaveVideoConfiguration(tbl_video_configuration form)
        {

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Initlization Video";
                string Id_assessment = null;
                string Id_category = null;
                string Id_operation = "Save";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLogopration(id_user, orgid1, Page1, Id_assessment, Id_category, Id_operation);
                // Now you have orgid and id_user available for further processing
            }
            tbl_video_configuration temp = new tbl_video_configuration();
                temp.Id_video = form.Id_video;
                temp.Id_organization = form.Id_organization;
            if (form.Video_name_webFile != null)
            {
                // Handling web video file
                if (form.Video_name_webFile != null && form.Video_name_webFile.ContentLength > 0)
                {
                    string uploadDir = "~/Content/SKILLMUNI_DATA/Video"; // Specify the directory where you want to save the videos
                    string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    string videoFileName = Path.GetFileName(form.Video_name_webFile.FileName);
                    string videoPath = Path.Combine(serverPath, videoFileName);
                    form.Video_name_webFile.SaveAs(videoPath);

                    // Store the path of the saved video in the database
                    temp.Video_name_web = videoFileName;
                }

            }
            else
            {
                temp.Video_name_web = form.Video_name_web;
            }
            if (form.Video_name_mobileFile != null)
            {
                // Handling mobile video file
                if (form.Video_name_mobileFile != null && form.Video_name_mobileFile.ContentLength > 0)
                {
                    string uploadDir = "~/Content/SKILLMUNI_DATA/Video"; // Specify the directory where you want to save the videos
                    string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    string videoFileName = Path.GetFileName(form.Video_name_mobileFile.FileName);
                    string videoPath = Path.Combine(serverPath, videoFileName);
                    form.Video_name_mobileFile.SaveAs(videoPath);

                    // Store the path of the saved video in the database
                    temp.Video_name_mobile = videoFileName;
                }

            }
            else
            {
                temp.Video_name_mobile = form.Video_name_mobile;
            }

                temp.Header_text = form.Header_text;





            // return new addVideoModel().add_video(temp).Equals("TRUE") ? (ActionResult)this.RedirectToAction("VideoIndex") : (ActionResult)this.RedirectToAction("VideoIndex1");


            //return View(form);
            bool videoAddedSuccessfully = new addVideoModel().add_video(temp).Equals("TRUE");

            if (videoAddedSuccessfully)
            {

                TempData["MessageVideo"] = "Data Inserted Successfully";              
                return RedirectToAction("VideoIndex");
            }
            else
            {
                return (ActionResult)this.RedirectToAction("VideoIndex1");
            }
        }




    }
}