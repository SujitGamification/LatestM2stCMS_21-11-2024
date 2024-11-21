// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.LoginController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using m2ostnextservice.Models;
using System.Web;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Excel;

namespace m2ostnext.Controllers
{
    public class LoginController : Controller
    {
        AESAlgorithm ency = new AESAlgorithm();
        private db_m2ostEntities db = new db_m2ostEntities();
       

        public ActionResult Index() => (ActionResult)this.View();

        public ActionResult loginPost(FormCollection collection)
        {
            try
            {
                System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                Login login = new contentDashboardModel().checkUser(new Login()
                {
                    Username = this.Request.Form["UI"],
                    Password = this.Request.Form["PD"]
                });
                if (login == null)
                    return (ActionResult)this.RedirectToAction("Index", "Home");

                UserSession orgStatus = new addCMS_CategoryModel().get_org_status(new UserSession()
                {
                    Username = login.Username,
                    Roleid = login.Roleid,
                    ID_USER = login.ID_USER,
                    id_ORGANIZATION = login.ID_ORG

                });
                int uid = Convert.ToInt32(login.ID_USER);
                int rid = Convert.ToInt32(login.Roleid);
                int oid = Convert.ToInt32(login.ID_ORG);
                

                tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>)(t => t.ID_USER == uid && t.STATUS == "A" || t.STATUS == "S" || t.STATUS == "F")).FirstOrDefault<tbl_cms_users>();
                orgStatus.USER = tblCmsUsers;
                List<tbl_cms_role_action_mapping> list = this.db.tbl_cms_role_action_mapping.Where<tbl_cms_role_action_mapping>((Expression<Func<tbl_cms_role_action_mapping, bool>>)(t => t.id_cms_role == (int?)rid && t.id_organization == (int?)oid)).ToList<tbl_cms_role_action_mapping>();
                orgStatus.action = list;
                System.Web.HttpContext.Current.Session["UserSession"] = (object)orgStatus;
                if (orgStatus.org_status == "S" || orgStatus.org_status == "F" || orgStatus.org_status == "H")
                {
                    if (orgStatus.exp_date < DateTime.Now)
                        return (ActionResult)this.RedirectToAction("index", "Home");
                    DateTime expDate = orgStatus.exp_date;
                }
                UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

                if (userSession != null)
                {
                    // Extract ID_USER and id_ORGANIZATION from the userSession object
                    string orgid = oid != null ? Convert.ToString(oid) : "Unknown";
                    string id_user = uid != null ? Convert.ToString(uid) : "Unknown";
                    string Page1 = "Loginpage";

                    userSession.id_ORGANIZATION = orgid;
                    userSession.ID_USER = id_user;

                    // Optionally, update the session variable if needed
                    System.Web.HttpContext.Current.Session["UserSession"] = userSession;

                    UserLogDetails userLogDetails = new UserLogDetails();
                  
                    userLogDetails.AddUserDataLog(id_user, orgid, Page1);
                }



                

                 







                return (ActionResult)this.RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult LoginCheckFromSSO(string USERID)
        {
            try
            {
                System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                Login login = new contentDashboardModel().checkUser_SSO(new Login()
                {
                    Username = this.Decrypt(USERID)
                });

                if (login == null)
                    return (ActionResult)this.RedirectToAction("Index", "Home");

                UserSession orgStatus = new addCMS_CategoryModel().get_org_status(new UserSession()
                {
                    Username = login.Username,
                    Roleid = login.Roleid,
                    ID_USER = login.ID_USER,
                    id_ORGANIZATION = login.ID_ORG
                });

                int uid = Convert.ToInt32(login.ID_USER);
                int rid = Convert.ToInt32(login.Roleid);
                int oid = Convert.ToInt32(login.ID_ORG);
                tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>)(t => t.ID_USER == uid && t.STATUS == "A" || t.STATUS == "S" || t.STATUS == "F")).FirstOrDefault<tbl_cms_users>();
                orgStatus.USER = tblCmsUsers;
                List<tbl_cms_role_action_mapping> list = this.db.tbl_cms_role_action_mapping.Where<tbl_cms_role_action_mapping>((Expression<Func<tbl_cms_role_action_mapping, bool>>)(t => t.id_cms_role == (int?)rid && t.id_organization == (int?)oid)).ToList<tbl_cms_role_action_mapping>();
                orgStatus.action = list;
                System.Web.HttpContext.Current.Session["UserSession"] = (object)orgStatus;
                if (orgStatus.org_status == "S" || orgStatus.org_status == "F" || orgStatus.org_status == "H")
                {
                    if (orgStatus.exp_date < DateTime.Now)
                        return (ActionResult)this.RedirectToAction("index", "Home");
                    DateTime expDate = orgStatus.exp_date;
                }
                return (ActionResult)this.RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getSessionStatus() => (UserSession)this.HttpContext.Session.Contents["UserSession"] == null ? "0" : "1";

        public string setSessionStatus(string uname, string password)
        {
            System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
            Login login = new contentDashboardModel().checkUser(new Login()
            {
                Username = uname,
                Password = password
            });
            if (login == null)
                return "0";
            UserSession userSession = new UserSession();
            userSession.Username = login.Username;
            userSession.Roleid = login.Roleid;
            userSession.ID_USER = login.ID_USER;
            userSession.id_ORGANIZATION = login.ID_ORG;
            int uid = Convert.ToInt32(login.ID_USER);
            int rid = Convert.ToInt32(login.Roleid);
            int oid = Convert.ToInt32(login.ID_ORG);
            tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>)(t => t.ID_USER == uid && t.STATUS == "A")).FirstOrDefault<tbl_cms_users>();
            userSession.USER = tblCmsUsers;
            List<tbl_cms_role_action_mapping> list = this.db.tbl_cms_role_action_mapping.Where<tbl_cms_role_action_mapping>((Expression<Func<tbl_cms_role_action_mapping, bool>>)(t => t.id_cms_role == (int?)rid && t.id_organization == (int?)oid)).ToList<tbl_cms_role_action_mapping>();
            userSession.action = list;
            System.Web.HttpContext.Current.Session["UserSession"] = (object)userSession;
            return "1";
        }

        public ActionResult Logout()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Logout";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
            return (ActionResult)this.RedirectToAction(nameof(Logout), "Home", (object)new
            {
                sessoin = "1"
            });
        }

    
        public ActionResult ChangePassword() => (ActionResult)this.View();

        public JsonResult change_password(FormCollection collection)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            string str1 = this.Request.Form["current_password"].ToString();
            string str2 = this.Request.Form["new_password"].ToString();
            string str3 = this.Request.Form["confirm_password"].ToString();
            int id_user = Convert.ToInt32(content.ID_USER);
            tbl_cms_users tblCmsUsers = this.db.tbl_cms_users.Where<tbl_cms_users>((Expression<Func<tbl_cms_users, bool>>)(t => t.ID_USER == id_user)).FirstOrDefault<tbl_cms_users>();
            if (tblCmsUsers == null)
                return this.Json((object)new
                {
                    flag = true,
                    msg = "Invalid User.Please login again.",
                    redirectUrl = this.Url.Action("Logout", "login")
                });
            string str4 = str1.Trim();
            if (!(tblCmsUsers.PASSWORD == str4))
                return this.Json((object)new
                {
                    flag = false,
                    msg = "current password doesnot match ,please try again."
                });
            tblCmsUsers.PASSWORD = str2;
            tblCmsUsers.UPDATED_DATE_TIME = DateTime.Now;
            this.db.SaveChanges();
            return tblCmsUsers.PASSWORD == str3 ? this.Json((object)new
            {
                flag = true,
                msg = "Your password updated successfully",
                redirectUrl = this.Url.Action("Index", "dashboard")
            }) : this.Json((object)new
            {
                flag = true,
                msg = "Your password not updated ,please try again."
            });
        }
        [ValidateInput(false)]
        public ActionResult SSOLogin(string UID)
        {
            ActionResult action;
            ActionResult actionResult;
            try
            {
                System.Web.HttpContext.Current.Session["UserSession"] = null;
                Login login = new Login()
                {
                    Username = this.Decrypt(UID),
                    Password = "Bgss@123"
                };
                Login login1 = (new contentDashboardModel()).checkUser(login);
                if (login1 != null)
                {
                    UserSession userSession = new UserSession()
                    {
                        Username = UID,
                        Roleid = login1.Roleid,
                        ID_USER = login1.ID_USER,
                        id_ORGANIZATION = login1.ID_ORG
                    };
                    userSession = (new addCMS_CategoryModel()).get_org_status(userSession);
                    int num = Convert.ToInt32(login1.ID_USER);
                    int num1 = Convert.ToInt32(login1.Roleid);
                    int num2 = Convert.ToInt32(login1.ID_ORG);
                    tbl_cms_users tblCmsUser = (
                        from t in this.db.tbl_cms_users
                        where t.ID_USER == num && t.STATUS == "A" || t.STATUS == "S" || t.STATUS == "F"
                        select t).FirstOrDefault<tbl_cms_users>();
                    userSession.USER = tblCmsUser;
                    List<tbl_cms_role_action_mapping> list = (
                        from t in this.db.tbl_cms_role_action_mapping
                        where t.id_cms_role == (int?)num1 && t.id_organization == (int?)num2
                        select t).ToList<tbl_cms_role_action_mapping>();
                    userSession.action = list;
                    System.Web.HttpContext.Current.Session["UserSession"] = userSession;
                    if (userSession.org_status == "S" || userSession.org_status == "F" || userSession.org_status == "H")
                    {
                        if (userSession.exp_date >= DateTime.Now)
                        {
                            DateTime expDate = userSession.exp_date;
                        }
                        else
                        {

                            actionResult = base.RedirectToAction("index", "Home");
                            return actionResult;
                        }
                    }
                    action = base.RedirectToAction("Index", "Dashboard");

                }
                else
                {
                    action = base.RedirectToAction("Index", "Home");
                }
                return action;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return actionResult;
        }

        public string Decrypt(string USERID)
        {
            string str = "L12M13S19$";
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            tripleDESCryptoServiceProvider.Key = mD5CryptoServiceProvider.ComputeHash(Encoding.ASCII.GetBytes(str));
            tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
            byte[] numArray = Convert.FromBase64String(USERID);
            return Encoding.ASCII.GetString(cryptoTransform.TransformFinalBlock(numArray, 0, (int)numArray.Length));
        }
        [Route("~/api/Encrypt")]
        public string Encrypt(string USERID)
        {

            string str = "L12M13S19$";
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            tripleDESCryptoServiceProvider.Key = mD5CryptoServiceProvider.ComputeHash(Encoding.ASCII.GetBytes(str));
            tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
            byte[] inputBytes = Encoding.ASCII.GetBytes(USERID);
            byte[] encryptedBytes = cryptoTransform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
