using Google.Protobuf.Collections;
using m2ostnextservice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace m2ostnext.Models
{

    public class tbl_corobus
    {
        public int org_id { get; set; }
        public  int id_user {  get; set; }  

        public string userid{ get; set; }
        public int coroebus_org { get; set; }
        public int coroebus_game { get; set; }
        public int STATUS { get; set; }
        public string Updated_datetime { get; set; }
        public string user_grade { get; set; }
        public string user_function { get; set; }

     
    }
    public class UserLogDetails
    {
        private MySqlConnection conn;

        public UserLogDetails() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);


        public void AddUserDataLog(string ID_USER, string id_ORGANIZATION, string Page1)
        {
            string host = Dns.GetHostName();
            string ipAddress = "";
            ipAddress = GetUserIPv4Address();



            string query = "INSERT INTO tbl_cmsuserdata_log (IdUser, OrgId, Ipaddress, Page,Update_date) VALUES (@IdUser, @OrgId, @Ipaddress, @Page,@Update_date)";

            using (MySqlCommand command = new MySqlCommand(query, this.conn))
            {
                command.Parameters.AddWithValue("@IdUser", ID_USER);
                command.Parameters.AddWithValue("@OrgId", id_ORGANIZATION);
                command.Parameters.AddWithValue("@Ipaddress", ipAddress); // You need to determine how to get the IP address
                command.Parameters.AddWithValue("@Page", Page1); // You need to determine how to get the page URL
                command.Parameters.AddWithValue("@Update_date", DateTime.Now); // You need to determine how to get the page URL

                this.conn.Open();
                command.ExecuteNonQuery();
                this.conn.Close(); // Close the connection after execution
            }
        }

        public void AddUserDataLogopration(string ID_USER, string id_ORGANIZATION, string Page1,string Id_assessment, string Id_category, string Id_operation)
        {
            string host = Dns.GetHostName();
            string ipAddress = "";
            ipAddress = GetUserIPv4Address();



            string query = "INSERT INTO tbl_cmsuserdata_log (IdUser, OrgId, Ipaddress, Page,Id_assessment,Id_category,Id_operation,Update_date) VALUES (@IdUser, @OrgId, @Ipaddress, @Page,@Id_assessment,@Id_category,@Id_operation,@Update_date)";

            using (MySqlCommand command = new MySqlCommand(query, this.conn))
            {
                command.Parameters.AddWithValue("@IdUser", ID_USER);
                command.Parameters.AddWithValue("@OrgId", id_ORGANIZATION);
                command.Parameters.AddWithValue("@Ipaddress", ipAddress); // You need to determine how to get the IP address
                command.Parameters.AddWithValue("@Page", Page1);
                command.Parameters.AddWithValue("@Id_assessment", Id_assessment);
                command.Parameters.AddWithValue("@Id_category", Id_category);
                command.Parameters.AddWithValue("@Id_operation", Id_operation);
             
                command.Parameters.AddWithValue("@Update_date", DateTime.Now); // You need to determine how to get the page URL

                this.conn.Open();
                command.ExecuteNonQuery();
                this.conn.Close(); // Close the connection after execution
            }
        }

        static string GetUserIPv4Address()
        {
            string userIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(userIPAddress))
            {
                userIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                // X-Forwarded-For may return multiple IP addresses separated by comma
                // We'll use the first one, assuming it's the client's IP address
                userIPAddress = userIPAddress.Split(',')[0];
            }

            // Check if the retrieved IP address is in IPv6 format
            // If it is, we'll use the local machine's IPv4 address instead
            if (userIPAddress == "::1")
            {
                userIPAddress = GetLocalIPv4Address();
            }

            return userIPAddress;
        }

        static string GetLocalIPv4Address()
        {
            string localIPv4Address = "";
            foreach (var netInterface in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                {
                    foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addrInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            localIPv4Address = addrInfo.Address.ToString();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(localIPv4Address))
                {
                    break;
                }
            }
            return localIPv4Address;
        }

        public string CheckUserDataNgage(int id_role)
        {
            string csst_role = "";
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = "SELECT id_csst_role FROM tbl_csst_role WHERE id_role = @id_role";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_role", id_role);

             
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id_csst_role = reader.GetInt32("id_csst_role");
                            csst_role = reader.GetString("csst_role");
                          
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, log or throw as required
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return csst_role;
        }

        public int CheckUserDataNgageOrg(int orgid)
        {
            int id_Ngage = -1;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = "SELECT Ngage FROM tbl_org_mapping WHERE M2ost = @M2ost LIMIT 1";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@M2ost", orgid);


                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             id_Ngage = reader.GetInt32("Ngage");
                           

                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, log or throw as required
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }

            return id_Ngage;
        }

        public int CheckUserroleDataNgage(string ROLE)
        {
            string input = ROLE;
            char[] delimiters = new char[] { '-' };

            // Split the string
            string[] parts = input.Split(delimiters);

            int id_Ngagerole = -1;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = "SELECT Ngage_id_role FROM tbl_ngagerole_mapping WHERE M2ost_name LIKE @M2ost_name;";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@M2ost_name", parts[0] + "%");


                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id_Ngagerole = reader.GetInt32("Ngage_id_role");


                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, log or throw as required
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return id_Ngagerole;
        }

        public string getEncryptedString(string str)
        {
            string password = "3sc3RLrpd17";
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));
            // Create secret IV
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            string encrypted = new AESAlgorithm().EncryptString(str, key, iv);
            return encrypted;
        }
        public int AddUserDataNgage(string Name, string Email, string Mobile, string Password, string Id_Department, int Id_Role, int id_orgname,int login_type, string country_id, string states_id, string city_id,int Id_CmsUser)
        {
            int id_Ngagerolesave = -1;
            string connectionString = ConfigurationManager.ConnectionStrings["db_tgc_gameEntities1"].ConnectionString;

            // SQL query to insert data into the table
            string query = @"INSERT INTO tbl_users (Name, Email, Phone_No, Password, Id_Department, ID_ROLE, ID_ORGANIZATION,login_type, country_id, states_id, city_id,Id_CmsUser)
                     VALUES (@Name, @Email, @Phone_No, @Password, @Id_Department, @ID_ROLE, @ID_ORGANIZATION,@login_type, @country_id, @states_id, @city_id,@Id_CmsUser);";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                // Add parameters to the query
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Phone_No", Mobile);
                command.Parameters.AddWithValue("@Password", Password);
           
                command.Parameters.AddWithValue("@Id_Department", Id_Department);
                command.Parameters.AddWithValue("@ID_ROLE", Id_Role);
                command.Parameters.AddWithValue("@ID_ORGANIZATION", id_orgname);
                command.Parameters.AddWithValue("@login_type", login_type);
                command.Parameters.AddWithValue("@country_id", country_id);
                command.Parameters.AddWithValue("@states_id", states_id);
                command.Parameters.AddWithValue("@city_id", city_id);
                command.Parameters.AddWithValue("@Id_CmsUser", Id_CmsUser);

                try
                {
                    connection.Open();
                    // Execute the INSERT query
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Retrieve the last inserted ID (assuming it's an auto-increment field)
                        id_Ngagerolesave = (int)command.LastInsertedId;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }

            return id_Ngagerolesave;
        }

        public int CheckUserDataNgageOrgList(int orgId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            string query = @"
        SELECT 
            @orgId AS org_id, 
            id_user, 
            userid,
            CASE 
                WHEN user_function LIKE '%BATA%' THEN 80
                WHEN user_function LIKE '%HP%' THEN 81
                WHEN user_function LIKE '%FRN_BA%' THEN 82
                WHEN user_function LIKE '%FRN_HP%' THEN 83
                WHEN user_function LIKE '%Franchise%' THEN 82
            END AS coroebus_org,
            CASE 
                WHEN user_function LIKE '%BATA%' THEN 231
                WHEN user_function LIKE '%HP%' THEN 232
                WHEN user_function LIKE '%FRN_BA%' THEN 233
                WHEN user_function LIKE '%FRN_HP%' THEN 234
                WHEN user_function LIKE '%Franchise%' THEN 233
            END AS coroebus_game,
            'A' AS STATUS, 
            CURRENT_TIMESTAMP AS Updated_datetime, 
            user_grade, 
            user_function
        FROM 
            tbl_user
        WHERE 
            id_organization = @orgId 
            AND STATUS = 'A'
            AND id_user NOT IN (SELECT id_m2ost_user FROM tbl_user_game_mapping)
            AND userid NOT LIKE 'BATA%';";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameter
                        command.Parameters.AddWithValue("@orgId", orgId);

                        System.Data.DataTable dataTable = new System.Data.DataTable();

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }

                        // Define your SQL INSERT query
                        string insertQuery = @"
                    INSERT INTO tbl_user_game_mapping 
                        (Id_m2ost_og, Id_m2ost_user, m2ost_userid, Id_coroebus_org, Id_coroebus_game, Status, Updated_datetime) 
                    VALUES 
                        (@Id_m2ost_og, @Id_m2ost_user, @m2ost_userid, @Id_coroebus_org, @Id_coroebus_game, @Status, @Updated_datetime)";

                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            // Add parameters to the insert command
                            insertCommand.Parameters.Add("@Id_m2ost_og", MySqlDbType.Int32);
                            insertCommand.Parameters.Add("@Id_m2ost_user", MySqlDbType.Int32);
                            insertCommand.Parameters.Add("@m2ost_userid", MySqlDbType.VarChar, 255);
                            insertCommand.Parameters.Add("@Id_coroebus_org", MySqlDbType.Int32);
                            insertCommand.Parameters.Add("@Id_coroebus_game", MySqlDbType.Int32);
                            insertCommand.Parameters.Add("@Status", MySqlDbType.VarChar, 1);
                            insertCommand.Parameters.Add("@Updated_datetime", MySqlDbType.DateTime);

                            // Iterate through the DataTable rows and insert data into the database
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Set parameter values for each row
                                insertCommand.Parameters["@Id_m2ost_og"].Value = row["org_id"];
                                insertCommand.Parameters["@Id_m2ost_user"].Value = row["id_user"];
                                insertCommand.Parameters["@m2ost_userid"].Value = row["userid"];
                                insertCommand.Parameters["@Id_coroebus_org"].Value = row["coroebus_org"];
                                insertCommand.Parameters["@Id_coroebus_game"].Value = row["coroebus_game"];
                                insertCommand.Parameters["@Status"].Value = row["STATUS"];
                                insertCommand.Parameters["@Updated_datetime"].Value = row["Updated_datetime"];

                                // Execute the INSERT command
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                        return 1;
                    }
                  

                }
               

            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
           
               
            




        }

        public System.Data.DataTable CheckUserDataCoroebusOrgList(int orgid,string userfunction)
        {

            System.Data.DataTable dataTable = new System.Data.DataTable(); // Step 1: Create a new DataTable

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = "SELECT * FROM tbl_org_mapping WHERE M2ost = @M2ost AND User_function = @User_function";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@M2ost", orgid);
                command.Parameters.AddWithValue("@User_function", userfunction);

                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Step 2: Add columns to the DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                        }

                        // Step 3 & 4: Read data from the reader and populate DataTable rows
                        while (reader.Read())
                        {
                            DataRow row = dataTable.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dataTable.Rows.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, log or throw as required
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            

            return dataTable;
        }

        public System.Data.DataTable CheckUserDataCoroebusRoleList(string ROLE)
        {
            string input = ROLE;
            char[] delimiters = new char[] { '-' };

            // Split the string
            string[] parts = input.Split(delimiters);

            System.Data.DataTable dataTable = new System.Data.DataTable(); // Step 1: Create a new DataTable

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = "SELECT * FROM tbl_ngagerole_mapping WHERE M2ost_name LIKE @M2ost_name;";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@M2ost_name", parts[0] + "%");

                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Step 2: Add columns to the DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                        }

                        // Step 3 & 4: Read data from the reader and populate DataTable rows
                        while (reader.Read())
                        {
                            DataRow row = dataTable.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dataTable.Rows.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, log or throw as required
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return dataTable;
        }

        public int InsertUserDataCoroebus(string tid, int id_coroebus_organization, int id_coroebus_game,string  group_name,int  id_role,string role_name,string USERID,string PASSWORD,string EMPLOYEEID,string first_name,string email_id,string contact_number,string user_department,string user_designation,string user_function,string user_grade,string supervisor,string team_name,int sr_no)
        {
            int id_user = 0; // Initialize to default value

            string connectionString = "Server=" + ConfigurationManager.AppSettings["RDS_HOSTNAME"] + ";Database=" + ConfigurationManager.AppSettings["RDS_DB_NAME"] + ";Uid=" + ConfigurationManager.AppSettings["RDS_USERNAME"] + ";Pwd=" + ConfigurationManager.AppSettings["RDS_PASSWORD"] + ";";


            // string connectionString = ConfigurationManager.ConnectionStrings["db_tgc_corobus"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO temp_user_v2 (tid, id_coroebus_organization, id_coroebus_game, group_name, id_role, role_name, USERID, PASSWORD, EMPLOYEEID, first_name, last_name, email_id, contact_number, user_department, user_designation, user_function, user_grade, supervisor, team_name, sr_no) VALUES (@tid, @id_coroebus_organization, @id_coroebus_game, @group_name, @id_role, @role_name, @USERID, @PASSWORD, @EMPLOYEEID, @first_name, @last_name, @email_id, @contact_number, @user_department, @user_designation, @user_function, @user_grade, @supervisor, @team_name, @sr_no)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    // Add parameters to the query
                    command.Parameters.AddWithValue("@tid", tid);
                    command.Parameters.AddWithValue("@id_coroebus_organization", id_coroebus_organization);
                    command.Parameters.AddWithValue("@id_coroebus_game", id_coroebus_game);
                    command.Parameters.AddWithValue("@group_name", group_name);
                    command.Parameters.AddWithValue("@id_role", id_role);
                    command.Parameters.AddWithValue("@role_name", role_name);
                    command.Parameters.AddWithValue("@USERID", USERID);
                    command.Parameters.AddWithValue("@PASSWORD", PASSWORD);
                    command.Parameters.AddWithValue("@EMPLOYEEID", EMPLOYEEID);
                    command.Parameters.AddWithValue("@first_name", first_name);
                    command.Parameters.AddWithValue("@last_name", ""); // Assuming last_name is not available in your method parameters
                    command.Parameters.AddWithValue("@email_id", email_id);
                    command.Parameters.AddWithValue("@contact_number", contact_number);
                    command.Parameters.AddWithValue("@user_department", user_department);
                    command.Parameters.AddWithValue("@user_designation", user_designation);
                    command.Parameters.AddWithValue("@user_function", user_function);
                    command.Parameters.AddWithValue("@user_grade", user_grade);
                    command.Parameters.AddWithValue("@supervisor", supervisor);
                    command.Parameters.AddWithValue("@team_name", team_name);
                    command.Parameters.AddWithValue("@sr_no", sr_no);

                    connection.Open();
                    // Execute the query
                    command.ExecuteScalar();

                    // Retrieve the last inserted ID
                    //id_user = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                }
            }
           

            //if(id_user == 0)
            //{
            //    Insertcobours(tid, id_coroebus_organization, id_coroebus_game, group_name, id_role, role_name, USERID, PASSWORD, EMPLOYEEID, first_name, email_id, contact_number, user_department, user_designation, user_function, user_grade, supervisor, team_name, sr_no);

            //}

            return id_user;
        }

        public int Insertcobours(string tid, int id_coroebus_organization, int id_coroebus_game, string group_name, int id_role, string role_name, string USERID, string PASSWORD, string EMPLOYEEID, string first_name, string email_id, string contact_number, string user_department, string user_designation, string user_function, string user_grade, string supervisor, string team_name, int sr_no)
        {
            string connectionString = "Server=" + ConfigurationManager.AppSettings["RDS_HOSTNAME"] + ";Database=" + ConfigurationManager.AppSettings["RDS_DB_NAME"] + ";Uid=" + ConfigurationManager.AppSettings["RDS_USERNAME"] + ";Pwd=" + ConfigurationManager.AppSettings["RDS_PASSWORD"] + ";";

            //string connectionString = ConfigurationManager.AppSettings["RDS_DB_NAME"];
            // string connectionString = ConfigurationManager.ConnectionStrings["db_tgc_corobus"].ConnectionString;

            int id_user = 0;
            string sup = "";
            string idrolemanger = "";
            int count3;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM tbl_coroebus_user WHERE USERID = @USERID";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@USERID", USERID);

                    count3 = Convert.ToInt32(cmd.ExecuteScalar());

                    string query3 = "SELECT COUNT(*)  FROM tbl_coroebus_user WHERE id_coroebus_organization = @id_coroebus_organization and USERID = @USERID";
                    MySqlCommand cmd3 = new MySqlCommand(query, connection);
                    cmd3.Parameters.AddWithValue("@id_coroebus_organization", id_coroebus_organization);
                    cmd3.Parameters.AddWithValue("@USERID", USERID);

                    count3 = Convert.ToInt32(cmd3.ExecuteScalar());

                    if (id_role == 6)
                    {
                        sup = Convert.ToString(checksupervisor(supervisor));
                        idrolemanger = Convert.ToString(checkreportingmanger(supervisor));
                    }
                    else if (id_role == 4)
                    {
                        sup = Convert.ToString(checksupervisor(supervisor));
                        idrolemanger = sup;
                    }
                    else if (id_role == 3)
                    {
                        sup = "0";
                        idrolemanger = Convert.ToString(checksupervisor(supervisor));
                    }
                    else if (id_role == 8)
                    {
                        sup = "0";
                        idrolemanger = Convert.ToString(checksupervisor(supervisor));
                    }
                    else if (id_role == 9)
                    {
                        sup = "0";
                        idrolemanger = "0";
                    }
                    else
                    {
                        count3 = 1;
                    }




                    string userid = USERID;


                    if (count3 == 0)
                    {
                        id_user = 1;

                        try
                        {


                            string query1 = @"INSERT INTO tbl_coroebus_user (cd_coroebus_user, id_coroebus_organization, id_role, USERID, PASSWORD, EMPLOYEEID,first_name, email_id, contact_number, 
                                user_department, user_designation, user_function, user_grade, supervisor, reporting_manager, view_status, status,updated_date_time,deviceid) 
                                VALUES (@cd_coroebus_user, @id_coroebus_organization, @id_role, @USERID, @PASSWORD,@EMPLOYEEID, @first_name, @email_id, 
                                @contact_number, @user_department, @user_designation, @user_function, @user_grade, @supervisor, @reporting_manager, 
                                @view_status, @status,@updated_date_time, @deviceid)";

                            MySqlCommand cmd1 = new MySqlCommand(query1, connection);

                            // Add parameters
                            cmd1.Parameters.AddWithValue("@cd_coroebus_user", tid);
                            cmd1.Parameters.AddWithValue("@id_coroebus_organization", id_coroebus_organization);
                            cmd1.Parameters.AddWithValue("@id_role", id_role);
                            cmd1.Parameters.AddWithValue("@USERID", userid);

                            if (PASSWORD != null)
                            {
                                string pass = getEncryptedString(PASSWORD);
                                cmd1.Parameters.AddWithValue("@PASSWORD", pass);
                            }



                            cmd1.Parameters.AddWithValue("@EMPLOYEEID", EMPLOYEEID);

                            cmd1.Parameters.AddWithValue("@first_name", first_name);
                            //cmd.Parameters.AddWithValue("@last_name", last_name);
                            if (email_id != null)
                            {
                                string email = getEncryptedString(email_id);

                                cmd1.Parameters.AddWithValue("@email_id", email);
                            }
                            string mobile = "9850182228";
                            string mobileno = getEncryptedString(mobile);
                            cmd1.Parameters.AddWithValue("@contact_number", mobileno);


                            cmd1.Parameters.AddWithValue("@user_department", user_department);

                            cmd1.Parameters.AddWithValue("@user_designation", user_designation);
                            cmd1.Parameters.AddWithValue("@user_function", user_function);

                            cmd1.Parameters.AddWithValue("@user_grade", "na");
                            // cmd.Parameters.AddWithValue("@image_path", image_path);



                            cmd1.Parameters.AddWithValue("@supervisor", sup);



                            cmd1.Parameters.AddWithValue("@reporting_manager", idrolemanger);





                            cmd1.Parameters.AddWithValue("@view_status", "A");
                            cmd1.Parameters.AddWithValue("@status", "A");
                            cmd1.Parameters.AddWithValue("@updated_date_time", DateTime.Now.ToString("yyyyMMddHHmmss"));
                            cmd1.Parameters.AddWithValue("@deviceid", "");
                            //cmd.Parameters.AddWithValue("@cms_progress_status", "");
                            // cmd.Parameters.AddWithValue("@m2ost_status", m2ost_status);
                            // cmd.Parameters.AddWithValue("@id_coroebus_game", "");

                            // Execute the query
                            int rowsAffected = cmd1.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Data inserted successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Failed to insert data.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }






                    }
                    else
                    {
                        id_user = -1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return id_user;
        }

        public int checksupervisor(string str)
        {
            int id_coroebus_role = 0;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["db_tgc_corobus"].ConnectionString))
            {
                string query = "SELECT id_coroebus_user FROM tbl_coroebus_user WHERE USERID = @supervisor;";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@supervisor", str);


                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id_coroebus_role = reader.GetInt32("id_coroebus_user");


                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, log or throw as required
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }

            }
            return id_coroebus_role;

        }

        public int checkreportingmanger(string str)
        {
            int id_role = 0;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["db_tgc_corobus"].ConnectionString))
            {
                string query = "SELECT reporting_manager FROM tbl_coroebus_user WHERE USERID = @sup;";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@sup", str);


                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id_role = reader.GetInt32("reporting_manager");


                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, log or throw as required
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return id_role;

        }
    }
}
