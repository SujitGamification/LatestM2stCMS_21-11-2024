using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class tbl_temp_user_uploadNew
    {
        public int id_temp_user_upload { get; set; }
        public string temp_user_upload_key { get; set; }
        public string EMPLOYEEID { get; set; }
        public string ROLE { get; set; }
        public Nullable<int> ID_ROLE { get; set; }
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string AGE { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE { get; set; }
        public string GENDER { get; set; }
        public string CITY { get; set; }
        public string OFFICE_ADDRESS { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string DATE_OF_JOINING { get; set; }
        public string user_department { get; set; }
        public string user_designation { get; set; }
        public string user_function { get; set; }
        public string user_grade { get; set; }
        public string user_status { get; set; }
        public string reporting_manager { get; set; }
        public Nullable<int> id_reporting_manager { get; set; }
        public string status { get; set; }
        public string Location { get; set; }

        public string L4 { get; set; }
        public string L3 { get; set; }
        public string L2 { get; set; }
        public string L1 { get; set; }
        public string Spectator { get; set; }

        private static string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString; // Replace with your MySQL connection string
        

        public static List<tbl_temp_user_uploadNew> GetUserUploads(string upKey)
        {
            List<tbl_temp_user_uploadNew> list = new List<tbl_temp_user_uploadNew>();

            string query = @"
        SELECT EMPLOYEEID, ROLE, status, USERID, PASSWORD, FIRSTNAME, LASTNAME,
               AGE, EMAIL, MOBILE, GENDER, CITY, OFFICE_ADDRESS, DATE_OF_BIRTH,
               DATE_OF_JOINING, user_department, user_designation, user_function,
               user_grade, user_status, reporting_manager, id_reporting_manager,
               id_role, temp_user_upload_key, Location,L4,L3,L2,L1,Spectator
        FROM tbl_temp_user_upload
        WHERE temp_user_upload_key = @upKey AND status = 'A'";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@upKey", upKey);

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tbl_temp_user_uploadNew userUpload = new tbl_temp_user_uploadNew
                            {
                                EMPLOYEEID = reader["EMPLOYEEID"].ToString(),
                                ROLE = reader["ROLE"].ToString(),
                                status = reader["status"].ToString(),
                                USERID = reader["USERID"].ToString(),
                                PASSWORD = reader["PASSWORD"].ToString(),
                                FIRSTNAME = reader["FIRSTNAME"].ToString(),
                                LASTNAME = reader["LASTNAME"].ToString(),
                                AGE = reader["AGE"].ToString(),
                                EMAIL = reader["EMAIL"].ToString(),
                                MOBILE = reader["MOBILE"].ToString(),
                                GENDER = reader["GENDER"].ToString(),
                                CITY = reader["CITY"].ToString(),
                                OFFICE_ADDRESS = reader["OFFICE_ADDRESS"].ToString(),
                                DATE_OF_BIRTH = reader["DATE_OF_BIRTH"].ToString(),
                                DATE_OF_JOINING = reader["DATE_OF_JOINING"].ToString(),
                                user_department = reader["user_department"].ToString(),
                                user_designation = reader["user_designation"].ToString(),
                                user_function = reader["user_function"].ToString(),
                                user_grade = reader["user_grade"].ToString(),
                                user_status = reader["user_status"].ToString(),
                                reporting_manager = reader["reporting_manager"].ToString(),
                                id_reporting_manager = reader["id_reporting_manager"] != DBNull.Value ? (int?)Convert.ToInt32(reader["id_reporting_manager"]) : null,
                                ID_ROLE = reader["id_role"] != DBNull.Value ? (int?)Convert.ToInt32(reader["id_role"]) : null,
                                temp_user_upload_key = reader["temp_user_upload_key"].ToString(),
                                Location = reader["Location"].ToString(),
                                L4 = reader["L4"].ToString(),
                                L3 = reader["L3"].ToString(),
                                L2 = reader["L2"].ToString(),
                                L1 = reader["L1"].ToString(),
                                Spectator = reader["Spectator"].ToString()
                                
                            };

                            list.Add(userUpload);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return list;
        }

    }
}