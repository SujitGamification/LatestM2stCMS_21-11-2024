using m2ostnext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnext.Controllers
{
    public class AccountController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>


        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("api/loginAPI")]
        public IHttpActionResult LoginPostAPI(string Username,string Password)
        {
            try
            {
                System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                // Authentication
                Login login = new contentDashboardModel().checkUser(new Login
                {
                    Username = Username,
                    Password = Password
                });

                if (login == null)
                    return BadRequest("Invalid username or password"); // Return 404 if user not found

                UserSession orgStatus = new addCMS_CategoryModel().get_org_status(new UserSession
                {
                    Username = login.Username,
                    Roleid = login.Roleid,
                    ID_USER = login.ID_USER,
                    id_ORGANIZATION = login.ID_ORG
                });

                int uid = Convert.ToInt32(login.ID_USER);
                int rid = Convert.ToInt32(login.Roleid);
                int oid = Convert.ToInt32(login.ID_ORG);

                // Fetch user status
                tbl_cms_users tblCmsUsers = db.tbl_cms_users
                    .FirstOrDefault(t => t.ID_USER == uid && (t.STATUS == "A" || t.STATUS == "S" || t.STATUS == "F"));

                if (tblCmsUsers == null)
                    return NotFound(); // Return 404 if user not found

                orgStatus.USER = tblCmsUsers;

                // Fetch role actions
                List<tbl_cms_role_action_mapping> actions = db.tbl_cms_role_action_mapping
                    .Where(t => t.id_cms_role == rid && t.id_organization == oid)
                    .ToList();

                orgStatus.action = actions;

                // Set user session
                System.Web.HttpContext.Current.Session["UserSession"] = orgStatus;

                if (orgStatus.org_status == "S" || orgStatus.org_status == "F" || orgStatus.org_status == "H")
                {
                    if (orgStatus.exp_date < DateTime.Now)
                    {
                        // Return a successful message if session is expired
                        return Ok(new { message = "Session expired" });
                    }
                }

                // If the conditions are not met, you can return a different successful message
                return Ok(new { message = "Successful operation" }); // Redirect to Dashboard/Index if login successful
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Return 500 for internal server error
            }
        }





        public class LoginCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}