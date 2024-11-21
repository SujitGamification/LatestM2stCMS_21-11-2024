using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class AppUserList
    {
  
            public List<tbl_csst_role> Role { get; set; }

        public User User { get; set; }
            public tbl_profile Profile { get; set; }
        
    }
    public class User
    {
        public int? ID_USER { get; set; } // Nullable value type
        public int? ID_CODE { get; set; } // Nullable value type
        public int? ID_ORGANIZATION { get; set; } // Nullable value type
        public int? ID_ROLE { get; set; } // Nullable value type
        public string USERID { get; set; } // Nullable by default
        public string PASSWORD { get; set; } // Nullable by default
        public string FBSOCIALID { get; set; } // Nullable by default
        public string GPSOCIALID { get; set; } // Nullable by default
        public char? STATUS { get; set; } // Nullable value type
        public DateTime? UPDATEDTIME { get; set; } // Nullable value type
        public DateTime? EXPIRY_DATE { get; set; } // Nullable value type
        public string EMPLOYEEID { get; set; } // Nullable by default
        public string user_department { get; set; } // Nullable by default
        public string user_designation { get; set; } // Nullable by default
        public string user_function { get; set; } // Nullable by default
        public string user_grade { get; set; } // Nullable by default
        public string user_status { get; set; } // Nullable by default
        public int? reporting_manager { get; set; } // Nullable value type
        public int? is_reporting { get; set; } // Nullable value type
        public DateTime? created_datetime { get; set; } // Nullable value type
        public DateTime? last_modified_datetime { get; set; } // Nullable value type
        public string L4 { get; set; } // Nullable by default
        public string L3 { get; set; } // Nullable by default
        public string L2 { get; set; } // Nullable by default
        public string L1 { get; set; } // Nullable by default
        public string Spectator { get; set; } // Nullable by default
    }

    public class UserService
    {
        public bool updatethetable_user(int userId, string str20, string str21, string str22, string str23, string str24)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                connection.Open();

                // Corrected the SQL query syntax for the UPDATE statement
                string query = "UPDATE tbl_user SET L4 = @L4, L3 = @L3, L2 = @L2, L1 = @L1, Spectator = @Spectator WHERE ID_USER = @ID_USER";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Set the parameter values
                    command.Parameters.AddWithValue("@ID_USER", userId);
                    command.Parameters.AddWithValue("@L4", str20);
                    command.Parameters.AddWithValue("@L3", str21);
                    command.Parameters.AddWithValue("@L2", str22);
                    command.Parameters.AddWithValue("@L1", str23);
                    command.Parameters.AddWithValue("@Spectator", str24);

                    // Execute the command and check the number of affected rows
                    int rowsAffected = command.ExecuteNonQuery();

                    // Return true if at least one row was affected, indicating success
                    return rowsAffected > 0;
                }
            }
        }

        public bool updatethetable_user1(int userId, string str20, string str21, string str22, string str23, string str24, string rolesAsString)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                connection.Open();

                string query = "UPDATE tbl_user SET ID_ROLE = @ID_ROLE, L4 = @L4, L3 = @L3, L2 = @L2, L1 = @L1, Spectator = @Spectator WHERE ID_USER = @ID_USER";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_USER", userId);
                    command.Parameters.AddWithValue("@ID_ROLE", rolesAsString);
                    command.Parameters.AddWithValue("@L4", str20);
                    command.Parameters.AddWithValue("@L3", str21);
                    command.Parameters.AddWithValue("@L2", str22);
                    command.Parameters.AddWithValue("@L1", str23);
                    command.Parameters.AddWithValue("@Spectator", str24);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool updatethetable(int userId, string str20, string str21, string str22, string str23, string str24)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                connection.Open();

                // Start building the SQL query
                string query = "UPDATE tbl_user SET ";
                bool firstFieldAdded = false;

                // Conditionally append L4 based on whether str20 is null or empty
                if (!string.IsNullOrEmpty(str20))
                {
                    query += "L4 = @L4";
                    firstFieldAdded = true;
                }

                // Conditionally append L3
                if (!string.IsNullOrEmpty(str21))
                {
                    query += firstFieldAdded ? ", L3 = @L3" : "L3 = @L3";
                    firstFieldAdded = true;
                }

                // Conditionally append L2
                if (!string.IsNullOrEmpty(str22))
                {
                    query += firstFieldAdded ? ", L2 = @L2" : "L2 = @L2";
                    firstFieldAdded = true;
                }

                // Conditionally append L1
                if (!string.IsNullOrEmpty(str23))
                {
                    query += firstFieldAdded ? ", L1 = @L1" : "L1 = @L1";
                    firstFieldAdded = true;
                }

                // Conditionally append Spectator
                if (!string.IsNullOrEmpty(str24))
                {
                    query += firstFieldAdded ? ", Spectator = @Spectator" : "Spectator = @Spectator";
                }

                // If no fields were added to the query, return false (nothing to update)
                if (!firstFieldAdded)
                {
                    return false; // No fields to update
                }

                // Add WHERE clause
                query += " WHERE ID_USER = @ID_USER";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Set the parameter for userId
                    command.Parameters.AddWithValue("@ID_USER", userId);

                    // Add parameters only for the columns being updated
                    if (!string.IsNullOrEmpty(str20))
                    {
                        command.Parameters.AddWithValue("@L4", str20);
                    }

                    if (!string.IsNullOrEmpty(str21))
                    {
                        command.Parameters.AddWithValue("@L3", str21);
                    }

                    if (!string.IsNullOrEmpty(str22))
                    {
                        command.Parameters.AddWithValue("@L2", str22);
                    }

                    if (!string.IsNullOrEmpty(str23))
                    {
                        command.Parameters.AddWithValue("@L1", str23);
                    }

                    if (!string.IsNullOrEmpty(str24))
                    {
                        command.Parameters.AddWithValue("@Spectator", str24);
                    }

                    // Execute the command and check the number of affected rows
                    int rowsAffected = command.ExecuteNonQuery();

                    // Return true if at least one row was affected, indicating success
                    return rowsAffected > 0;
                }
            }
        }

    }
}