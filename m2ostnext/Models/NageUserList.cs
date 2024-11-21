using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace m2ostnext.Models
{
    public class NageUserList
    {
        public class Ngagetbl_user
        {
            public int Id_User { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone_No { get; set; }
            public string Password { get; set; }
            public bool IsActive { get; set; }
            public DateTime? Updated_Date_Time { get; set; }
            public int Id_Department { get; set; }
            public int ID_ORGANIZATION { get; set; }
            public int ID_ROLE { get; set; }
            public string RoleName { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public string DepartmentName { get; set; }
        }

        private MySqlConnection connection;



        public NageUserList()

        {

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            this.connection = new MySqlConnection(connectionString);


        }

        public List<Ngagetbl_user> GetUsers(int ID_ORGANIZATION)
        {
            List<Ngagetbl_user> users = new List<Ngagetbl_user>();

            connection.Open();

            string query = @"SELECT 
                          T.Id_User,
                          T.Name,
                          T.Email,
                          T.Phone_No,
                          T.Password,
                          T.IsActive,
                          T.Updated_Date_Time,
                          T.Id_Department,
                          T.ID_ORGANIZATION,
                          T.ID_ROLE,
                          mo.ROLENAME,
                          co.ORGANIZATION_NAME,
                          prod.DepartmentName
                       FROM 
                          tbl_users T
                       LEFT JOIN 
                          tbl_organization co ON T.ID_ORGANIZATION = co.ID_ORGANIZATION
                       LEFT JOIN 
                          tbl_organization_department prod ON T.Id_Department = prod.Id_Department
                       LEFT JOIN 
                          tbl_app_role mo ON T.ID_ROLE = mo.ID_ROLE
                       WHERE 
                          T.ID_ORGANIZATION = @ID_ORGANIZATION";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID_ORGANIZATION", ID_ORGANIZATION);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Ngagetbl_user user = new Ngagetbl_user
                    {
                        Id_User = reader.GetInt32("Id_User"),
                        Name = reader.GetString("Name"),
                        Email = reader.GetString("Email"),
                        Phone_No = reader.GetString("Phone_No"),
                        Password = reader.GetString("Password"),
                        IsActive = reader.GetBoolean("IsActive"),
                        Updated_Date_Time = reader.IsDBNull(reader.GetOrdinal("Updated_Date_Time")) ? (DateTime?)null : reader.GetDateTime("Updated_Date_Time"),
                        Id_Department = reader.GetInt32("Id_Department"),
                        ID_ORGANIZATION = reader.GetInt32("ID_ORGANIZATION"),
                        ID_ROLE = reader.GetInt32("ID_ROLE"),
                        RoleName = reader.GetString("ROLENAME"),
                        ORGANIZATION_NAME = reader.GetString("ORGANIZATION_NAME"),
                        DepartmentName = reader.GetString("DepartmentName")
                    };

                    users.Add(user);
                }
            }

            connection.Close();

            return users;
        }
    

     }

}  