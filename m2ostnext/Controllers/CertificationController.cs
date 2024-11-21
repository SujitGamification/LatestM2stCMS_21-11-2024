// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.CertificationController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class CertificationController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult CertificateDashboard()
    {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            CertificateLogicModel certificateModel = new CertificateLogicModel();
            List<CertificateAssignmentTheme> certificateList = certificateModel.GetCertificateDataList(num);
            ViewData["certificateList"] = certificateList;

            Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      this.ViewData["certificates"] = (object) new CertificateLogic().get_Certificate_list();
      return (ActionResult) this.View();
    }

    public ActionResult CertificateAssignment()
    {
     int oid = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
     List<tbl_assessment> list = this.db.tbl_assessment.Where<tbl_assessment>((Expression<Func<tbl_assessment, bool>>) (t => t.id_organization == (int?) oid)).ToList<tbl_assessment>();
     List<tbl_certificate_configure> certificateList = new CertificateLogic().get_Certificate_list();




      this.ViewData["ass_list"] = (object) list;
      this.ViewData["certificates"] = (object) certificateList;




      return (ActionResult) this.View();
    }

    public ActionResult AssignCertification(tbl_certificate_assignment cert)
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      new CertificateLogic().assignCertificate(cert, int32);
      return (ActionResult) this.RedirectToAction("AssignedCertificate");
    }

    public ActionResult AssignedCertificate()
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      List<tbl_certificate_assignment> certificateAssignmentList = new List<tbl_certificate_assignment>();
      this.ViewData["Assign"] = (object) new CertificateLogic().getAssignList(int32);
      return (ActionResult) this.View();
    }

    public ActionResult RemoveCertificate(int id)
    {
      new CertificateLogic().DeleteAsssign(id);
      return (ActionResult) this.RedirectToAction("AssignedCertificate");
    }

        [HttpPost]
        public ActionResult CreateCertificateSave(CertificateAssignmentTheme model)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            CertificateAssignmentTheme temp = new CertificateAssignmentTheme();
            temp.IdCertificate = model.IdCertificate;
            temp.Id_organization = int32;
            temp.SelectTheme = model.SelectTheme;
            temp.HeaderThemeFirst = model.HeaderThemeFirst;
            temp.SubText1ThemeFirst = model.SubText1ThemeFirst;
            temp.SubText2ThemeFirst = model.SubText2ThemeFirst;
            temp.SubText3ThemeFirst = model.SubText3ThemeFirst;
            temp.TextColorHeaderThemeFirst = model.TextColorHeaderThemeFirst;
            temp.TextColorSubTextThemeFirst = model.TextColorSubTextThemeFirst;
            string imageFileName = null;
            string LogoImageFileName = null;
            string BackgroundImageFileName = null;

            if (model.IdCertificate != 0)
            {
                // var LogoImageLeftThemeFirst = model.LogoImageLeftThemeFirst;
                // var LogoImageLeftThemeFirstUploadDir =model.LogoImageLeftThemeFirstUploadDir;
                if (model.LogoImageLeftThemeFirstUploadDir != null)
                {
                    if (model.LogoImageLeftThemeFirstUploadDir != null && model.LogoImageLeftThemeFirstUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        imageFileName = Path.GetFileName(model.LogoImageLeftThemeFirstUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, imageFileName);
                        model.LogoImageLeftThemeFirstUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageLeftThemeFirst = imageFileName;
                }
                else
                {
                    temp.LogoImageLeftThemeFirst = model.LogoImageLeftThemeFirst;
                }
                if (model.LogoImageRightThemeFirstUploadDir != null)
                {
                    if (model.LogoImageRightThemeFirstUploadDir != null && model.LogoImageRightThemeFirstUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        LogoImageFileName = Path.GetFileName(model.LogoImageRightThemeFirstUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, LogoImageFileName);
                        model.LogoImageRightThemeFirstUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageRightThemeFirst = LogoImageFileName;
                }
                else
                {
                    temp.LogoImageRightThemeFirst = model.LogoImageRightThemeFirst;
                }
                if (model.BackgroundImageThemeFirstUploadDir != null)
                {
                    if (model.BackgroundImageThemeFirstUploadDir != null && model.BackgroundImageThemeFirstUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        BackgroundImageFileName = Path.GetFileName(model.BackgroundImageThemeFirstUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, BackgroundImageFileName);
                        model.BackgroundImageThemeFirstUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.BackgroundImageThemeFirst = BackgroundImageFileName;
                }
                else
                {
                    temp.BackgroundImageThemeFirst = model.BackgroundImageThemeFirst;
                }
            }
            else
            {
                if (model.LogoImageLeftThemeFirstUploadDir != null)
                {
                    if (model.LogoImageLeftThemeFirstUploadDir != null && model.LogoImageLeftThemeFirstUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        imageFileName = Path.GetFileName(model.LogoImageLeftThemeFirstUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, imageFileName);
                        model.LogoImageLeftThemeFirstUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageLeftThemeFirst = imageFileName;
                }
                else
                {
                    temp.LogoImageLeftThemeFirst = model.LogoImageLeftThemeFirst;
                }
                if (model.LogoImageRightThemeFirstUploadDir != null)
                {
                    if (model.LogoImageRightThemeFirstUploadDir != null && model.LogoImageRightThemeFirstUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        LogoImageFileName = Path.GetFileName(model.LogoImageRightThemeFirstUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, LogoImageFileName);
                        model.LogoImageRightThemeFirstUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageRightThemeFirst = LogoImageFileName;
                }
                else
                {
                    temp.LogoImageRightThemeFirst = model.LogoImageRightThemeFirst;
                }
                if (model.BackgroundImageThemeFirstUploadDir != null)
                {
                    if (model.BackgroundImageThemeFirstUploadDir != null && model.BackgroundImageThemeFirstUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        BackgroundImageFileName = Path.GetFileName(model.BackgroundImageThemeFirstUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, BackgroundImageFileName);
                        model.BackgroundImageThemeFirstUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.BackgroundImageThemeFirst = BackgroundImageFileName;
                }
                else
                {
                    temp.BackgroundImageThemeFirst = model.BackgroundImageThemeFirst;
                }
            }


            bool CertificateaddedSuccessfull = new CertificateLogicModel().add_certificate(temp).Equals("TRUE");

            if (CertificateaddedSuccessfull)
            {
                TempData["MessageCertifatate"] = "Data Inserted Successfully";
                return RedirectToAction("CertificateDashboard");
            }
            else
            {
                return (ActionResult)this.RedirectToAction("CertificateDashboard1");
            }
        }

        [HttpPost]
        public ActionResult CreateCertificate2Save(CertificateAssignmentTheme model)
        {
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
            CertificateAssignmentTheme temp = new CertificateAssignmentTheme();
            temp.IdCertificate = model.IdCertificate;
            temp.Id_organization = int32;
            temp.SelectTheme = model.SelectTheme;
            temp.HeaderThemeSecond = model.HeaderThemeSecond;
            temp.SubTextFirstThemeSecond = model.SubTextFirstThemeSecond;
            temp.RightNameThemeSecond = model.RightNameThemeSecond;
            temp.LeftDesignationThemeSecond = model.LeftDesignationThemeSecond;
            temp.RightDepartmentThemeSecond = model.RightDepartmentThemeSecond;
            temp.LeftRegionThemeSecond = model.LeftRegionThemeSecond;
            temp.SubText2ThemeSecond = model.SubText2ThemeSecond;
            temp.TextColorHeaderThemeSecond = model.TextColorHeaderThemeSecond;
            temp.TextColorThemeSecond = model.TextColorThemeSecond;
            string imageFileName2 = null;
            string LogoImageFileName2 = null;
            string BackgroundImageFileName2 = null;

            if (model.IdCertificate != 0)
            {
              
                if (model.LogoImageLeftThemeSecondUploadDir != null)
                {
                    if (model.LogoImageLeftThemeSecondUploadDir != null && model.LogoImageLeftThemeSecondUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        imageFileName2 = Path.GetFileName(model.LogoImageLeftThemeSecondUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, imageFileName2);
                        model.LogoImageLeftThemeSecondUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageLeftThemeSecond = imageFileName2;
                }
                else
                {
                    temp.LogoImageLeftThemeSecond = model.LogoImageLeftThemeSecond;
                }
                if (model.LogoImageRightThemeSecondUploadDir != null)
                {
                    if (model.LogoImageRightThemeSecondUploadDir != null && model.LogoImageRightThemeSecondUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        LogoImageFileName2 = Path.GetFileName(model.LogoImageRightThemeSecondUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, LogoImageFileName2);
                        model.LogoImageRightThemeSecondUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageRightThemeSecond = LogoImageFileName2;
                }
                else
                {
                    temp.LogoImageRightThemeSecond = model.LogoImageRightThemeSecond;
                }
                if (model.BackgroundImageThemeSecondUploadDir != null)
                {
                    if (model.BackgroundImageThemeSecondUploadDir != null && model.BackgroundImageThemeSecondUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        BackgroundImageFileName2 = Path.GetFileName(model.BackgroundImageThemeSecondUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, BackgroundImageFileName2);
                        model.BackgroundImageThemeSecondUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.BackgroundImageThemeSecond = BackgroundImageFileName2;
                }
                else
                {
                    temp.BackgroundImageThemeSecond = model.BackgroundImageThemeSecond;
                }
            }
            else
            {
                if (model.LogoImageLeftThemeSecondUploadDir != null)
                {
                    if (model.LogoImageLeftThemeSecondUploadDir != null && model.LogoImageLeftThemeSecondUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        imageFileName2 = Path.GetFileName(model.LogoImageLeftThemeSecondUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, imageFileName2);
                        model.LogoImageLeftThemeSecondUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageLeftThemeSecond = imageFileName2;
                }
                else
                {
                    temp.LogoImageLeftThemeSecond = model.LogoImageLeftThemeSecond;
                }
                if (model.LogoImageRightThemeSecondUploadDir != null)
                {
                    if (model.LogoImageRightThemeSecondUploadDir != null && model.LogoImageRightThemeSecondUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        LogoImageFileName2 = Path.GetFileName(model.LogoImageRightThemeSecondUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, LogoImageFileName2);
                        model.LogoImageRightThemeSecondUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.LogoImageRightThemeSecond = LogoImageFileName2;
                }
                else
                {
                    temp.LogoImageRightThemeSecond = model.LogoImageRightThemeSecond;
                }
                if (model.BackgroundImageThemeSecondUploadDir != null)
                {
                    if (model.BackgroundImageThemeSecondUploadDir != null && model.BackgroundImageThemeSecondUploadDir.ContentLength > 0)
                    {
                        string uploadDir = "~/Content/SKILLMUNI_DATA/CATEGORY_IMAGE/CertificationAssessment"; // Specify the directory where you want to save the images
                        string serverPath = Server.MapPath(uploadDir); // Map the virtual path to a physical path

                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        BackgroundImageFileName2 = Path.GetFileName(model.BackgroundImageThemeSecondUploadDir.FileName);
                        string imagePath = Path.Combine(serverPath, BackgroundImageFileName2);
                        model.BackgroundImageThemeSecondUploadDir.SaveAs(imagePath);

                        // Store the path of the saved image in the database
                        // Store the relative path
                    }
                    temp.BackgroundImageThemeSecond = BackgroundImageFileName2;
                }
                else
                {
                    temp.BackgroundImageThemeSecond = model.BackgroundImageThemeSecond;
                }
            }




            bool CertificateaddedSuccessfull = new CertificateLogicModel().add_certificate2(temp).Equals("TRUE");
            
            if(CertificateaddedSuccessfull)
            {
                TempData["MessageCertifatate"] = "Data Inserted Successfully";
                return RedirectToAction("CertificateDashboard");
            }
            else
            {
                return (ActionResult)this.RedirectToAction("CertificateDashboard1");
            }
                
                
                


        }


    }
}
