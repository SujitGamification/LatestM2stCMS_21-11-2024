using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class UserUploadClassNew
    {
        public string EMPLOYEEID { get; set; }
        public string ROLE { get; set; }
        public string STATUS { get; set; }
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public int AGE { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE { get; set; }
        public string GENDER { get; set; }
        public string CITY { get; set; }
        public string OFFICE_ADDRESS { get; set; }
        public DateTime DATE_OF_BIRTH { get; set; }
        public DateTime DATE_OF_JOINING { get; set; }
        public string user_department { get; set; }
        public string user_designation { get; set; }
        public string user_function { get; set; }
        public string user_grade { get; set; }
        public string user_status { get; set; }
        public string reporting_manager { get; set; }
        public int id_reporting_manager { get; set; }
        public int id_role { get; set; }
        public string Location { get; set; }
    }

    
}