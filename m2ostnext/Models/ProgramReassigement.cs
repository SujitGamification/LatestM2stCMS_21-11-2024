using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Models
{
    
    public class ProgramReassigement
    {
        public class ProgramReassigement_service
        {
            private MySqlConnection connection;



            public ProgramReassigement_service()

            {

                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

                this.connection = new MySqlConnection(connectionString);

            }

            public List<SelectListItem> get_tbl_category_list(string orgid)
            {


                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    string ngageQuery = "SELECT * FROM tbl_category WHERE CATEGORY_TYPE = '0' AND ID_ORGANIZATION = '" + orgid + "' and status= 'A'";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem orgItem = new SelectListItem
                            {
                                Value = reader["ID_CATEGORY"].ToString(),
                                Text = reader["CATEGORYNAME"].ToString()
                            };

                            list.Add(orgItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, throw it, etc.)
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                return list;
            }

            public List<SelectListItem> get_tbl_category_tiles_list(string orgid)
            {


                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    string ngageQuery = "SELECT * FROM tbl_category_tiles WHERE id_organization = '" + orgid + "' and status= 'A' and category_theme='1'";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem orgItem = new SelectListItem
                            {
                                Value = reader["id_category_tiles"].ToString(),
                                Text = reader["tile_heading"].ToString()
                            };

                            list.Add(orgItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, throw it, etc.)
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                return list;
            }

            public List<SelectListItem> get_tbl_category_heading_list(int idtitles)
            {


                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    string ngageQuery = "SELECT * FROM tbl_category_heading WHERE id_category_tiles = '" + idtitles + "' and status= 'A'";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem orgItem = new SelectListItem
                            {
                                Value = reader["id_category_heading"].ToString(),
                                Text = reader["Heading_title"].ToString()
                            };

                            list.Add(orgItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, throw it, etc.)
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                return list;
            }

            public System.Data.DataTable GetCategorylist(string orgid1,int program,int title,int category,string startDate,string expiryDate)
            {

                System.Data.DataTable dataTable = new System.Data.DataTable();



                try
                {
                    string query = "SELECT * FROM tbl_content_program_mapping WHERE id_category = '" + program + "' AND id_organization = '" + orgid1 + "' AND status = 'A'";

                    // Create a DataTable to store the result
                 

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();

                     
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }

                        if (dataTable.Rows.Count != 0)
                        {

                            string updateQuery = "UPDATE tbl_content_program_mapping SET id_category_tile = @id_category_tile, id_category_heading = @id_category_heading, start_date = @start_date, expiry_date = @expiry_date, updated_date_time=@updated_date_time WHERE id_content_program_mapping = @id";

                            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                // Prepare the update command
                                updateCommand.Parameters.Add("@id_category_tile", MySqlDbType.VarChar);
                                updateCommand.Parameters.Add("@id_category_heading", MySqlDbType.VarChar);
                                updateCommand.Parameters.Add("@start_date", MySqlDbType.DateTime);
                                updateCommand.Parameters.Add("@expiry_date", MySqlDbType.DateTime);
                                updateCommand.Parameters.Add("@updated_date_time", MySqlDbType.DateTime);
                                updateCommand.Parameters.Add("@id", MySqlDbType.Int32);

                                // Iterate through each row and perform bulk update
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    int id = Convert.ToInt32(row["id_content_program_mapping"]);
                                    string startDateString = startDate;
                                    DateTime startDate1 = DateTime.ParseExact(startDateString, "dd-MM-yyyy HH:mm", null);
                                    string expiryDateString = expiryDate;
                                    DateTime expiryDate1 = DateTime.ParseExact(expiryDateString, "dd-MM-yyyy HH:mm", null);
                                    DateTime currentUtcTime = DateTime.UtcNow;

                                    updateCommand.Parameters["@id_category_tile"].Value = title;
                                    updateCommand.Parameters["@id_category_heading"].Value = category;
                                    updateCommand.Parameters["@start_date"].Value = startDate1;
                                    updateCommand.Parameters["@expiry_date"].Value = expiryDate1;
                                    updateCommand.Parameters["@updated_date_time"].Value = currentUtcTime;
                                    updateCommand.Parameters["@id"].Value = id;


                                    int rowsAffected = updateCommand.ExecuteNonQuery();
                                }
                            }


                            //foreach (DataRow row in dataTable.Rows)
                            //{


                            //    int id = Convert.ToInt32(row["id_content_program_mapping"]);

                            //    string updateQuery = "UPDATE tbl_content_program_mapping SET id_category_tile = @id_category_tile, id_category_heading = @id_category_heading, start_date = @start_date, expiry_date = @expiry_date,updated_date_time=@updated_date_time WHERE id_content_program_mapping = @id";

                            //    using (MySqlCommand command1 = new MySqlCommand(updateQuery, connection))
                            //    {
                            //        command1.Parameters.AddWithValue("@id", id);
                            //        command1.Parameters.AddWithValue("@id_category_tile", title);
                            //        command1.Parameters.AddWithValue("@id_category_heading", category);

                            //        string startDateString = startDate;
                            //        DateTime startDate1 = DateTime.ParseExact(startDateString, "dd-MM-yyyy HH:mm", null);
                            //        string formattedStartDate = startDate1.ToString("yyyy-MM-dd HH:mm:ss");

                            //        command1.Parameters.AddWithValue("@start_date", formattedStartDate);

                            //        string expiryDateString = expiryDate;
                            //        DateTime expiryDate1 = DateTime.ParseExact(expiryDateString, "dd-MM-yyyy HH:mm", null);
                            //        string formattedexpiryDate = expiryDate1.ToString("yyyy-MM-dd HH:mm:ss");

                            //        command1.Parameters.AddWithValue("@expiry_date", formattedexpiryDate);

                            //        DateTime currentUtcTime = DateTime.UtcNow;
                            //        string formattedDateTime = currentUtcTime.ToString("yyyy-MM-dd HH:mm:ss");
                            //        command1.Parameters.AddWithValue("@updated_date_time", formattedDateTime);
                            //        int rowsAffected = command1.ExecuteNonQuery();

                            //    }
                            //}


                        }
                        else
                        {

                        }
                        //string query1 = "SELECT * FROM tbl_content_program_mapping WHERE id_category = '" + program + "' AND id_organization = '" + orgid1 + "' AND status = 'A'";

                        //using (MySqlCommand command2 = new MySqlCommand(query1, connection))
                        //{
                          
                        //    // Execute the command and load the result into the DataTable
                        //    using (MySqlDataReader reader = command2.ExecuteReader())
                        //    {
                        //        dataTable.Load(reader);
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, throw it, etc.)
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                return dataTable;
            }

            public System.Data.DataTable GetCategorylistInsert(string orgid1, int program, int title, int category, string startDate, string expiryDate)
            {

                System.Data.DataTable dataTable = new System.Data.DataTable();



                try
                {
                    string query = "SELECT distinct id_user,id_role  FROM tbl_content_program_mapping WHERE id_category = '" + program + "' AND id_organization = '" + orgid1 + "' AND status = 'A'";

                    // Create a DataTable to store the result


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();


                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }

                        if (dataTable.Rows.Count != 0)
                        {

                            string insertQuery = "INSERT INTO tbl_content_program_mapping (id_organization,id_category,id_assessment_sheet,id_user,id_role,map_type,option_type,id_category_tile, id_category_heading, start_date, expiry_date, updated_date_time) " +
                                    "VALUES (@id_organization,@id_category,@id_assessment_sheet,@id_user,@id_role,@map_type,@option_type,@id_category_tile, @id_category_heading, @start_date, @expiry_date, @updated_date_time)";

                            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                            {
                                // Prepare the update command
                                insertCommand.Parameters.Add("@id_organization", MySqlDbType.Int32);
                                insertCommand.Parameters.Add("@id_category", MySqlDbType.Int32);
                                insertCommand.Parameters.Add("@id_assessment_sheet", MySqlDbType.Int32);
                                insertCommand.Parameters.Add("@id_user", MySqlDbType.Int32);
                                insertCommand.Parameters.Add("@id_role", MySqlDbType.Int32);
                                insertCommand.Parameters.Add("@map_type", MySqlDbType.Int32);
                                insertCommand.Parameters.Add("@option_type", MySqlDbType.Int32);
                                insertCommand.Parameters.Add("@id_category_tile", MySqlDbType.VarChar);
                                insertCommand.Parameters.Add("@id_category_heading", MySqlDbType.VarChar);
                                insertCommand.Parameters.Add("@start_date", MySqlDbType.DateTime);
                                insertCommand.Parameters.Add("@expiry_date", MySqlDbType.DateTime);
                                insertCommand.Parameters.Add("@updated_date_time", MySqlDbType.DateTime);

                                // Iterate through each row and perform bulk update
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    int id_id_user = Convert.ToInt32(row["id_user"]);
                                    int id_role = Convert.ToInt32(row["id_role"]);

                                    string startDateString = startDate;
                                    DateTime startDate1 = DateTime.ParseExact(startDateString, "dd-MM-yyyy HH:mm", null);
                                    string expiryDateString = expiryDate;
                                    DateTime expiryDate1 = DateTime.ParseExact(expiryDateString, "dd-MM-yyyy HH:mm", null);
                                    DateTime currentUtcTime = DateTime.UtcNow;

                                    insertCommand.Parameters["@id_organization"].Value = orgid1;
                                    insertCommand.Parameters["@id_category"].Value = program;
                                    insertCommand.Parameters["@id_assessment_sheet"].Value = 0;
                                    insertCommand.Parameters["@id_user"].Value = id_id_user;
                                    insertCommand.Parameters["@id_role"].Value = id_role;
                                    insertCommand.Parameters["@map_type"].Value = 1;
                                    insertCommand.Parameters["@option_type"].Value = 0;
                                    insertCommand.Parameters["@id_category_tile"].Value = title;
                                    insertCommand.Parameters["@id_category_heading"].Value = category;
                                    insertCommand.Parameters["@start_date"].Value = startDate1;
                                    insertCommand.Parameters["@expiry_date"].Value = expiryDate1;
                                    insertCommand.Parameters["@updated_date_time"].Value = currentUtcTime;
                                    


                                    int rowsAffected = insertCommand.ExecuteNonQuery();
                                }
                            }                         

                        }
                        else
                        {

                        }
                  
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, throw it, etc.)
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                return dataTable;
            }

        }

    }
}