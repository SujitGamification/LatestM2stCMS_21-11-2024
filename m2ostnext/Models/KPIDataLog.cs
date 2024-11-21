using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class KPIDataLog
    {
        public class KPIDataLogt_service
        {
            private MySqlConnection connection;



            public KPIDataLogt_service()
            {

                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

                this.connection = new MySqlConnection(connectionString);

            }
            public System.Data.DataTable GetPontlist(string orgid1, int program, int assesmentID, string startDate, string expiryDate)
            {
                System.Data.DataTable dataTable = new System.Data.DataTable();
                string formattedDate = "";
                string expiryDateDate1 = "";
                try
                {
                    string dateString = startDate; 
                    DateTime date;
                  

                    if (DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                       
                    }
                    string dateString1 = expiryDate; 
                    DateTime date1;
                    if (DateTime.TryParseExact(dateString1, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1))
                    {
                        expiryDateDate1 = date1.ToString("yyyy-MM-dd HH:mm");

                    }


                    string query = "SELECT * FROM tbl_user_kpi_data_log WHERE Content_Assessment_ID = '" + assesmentID + "' AND IsActive = 'A' AND created_date BETWEEN '"+ formattedDate + "' AND '"+ expiryDateDate1 + "'";

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

                            string updateQuery = "UPDATE tbl_user_kpi_data_log SET IsActive = @IsActive, created_date=@created_date WHERE id_user_kpi_data_log = @id";

                            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                // Prepare the update command
                                updateCommand.Parameters.Add("@IsActive", MySqlDbType.VarChar);

                                updateCommand.Parameters.Add("@created_date", MySqlDbType.DateTime);

                                updateCommand.Parameters.Add("@id", MySqlDbType.Int32);

                                // Iterate through each row and perform bulk update
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    int id = Convert.ToInt32(row["id_user_kpi_data_log"]);

                                    updateCommand.Parameters["@IsActive"].Value = "D";

                                    updateCommand.Parameters["@created_date"].Value = formattedDate;

                                    updateCommand.Parameters["@id"].Value = id;


                                    int rowsAffected = updateCommand.ExecuteNonQuery();
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


            public System.Data.DataTable GetCoinlist(string orgid1, int program, int assesmentID, string startDate, string expiryDate)
            {
                System.Data.DataTable dataTable = new System.Data.DataTable();
                string formattedDate = "";
                string expiryDateDate1 = "";
                try
                {
                    string dateString = startDate;
                    DateTime date;


                    if (DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        formattedDate = date.ToString("yyyy-MM-dd HH:mm");

                    }
                    string dateString1 = expiryDate;
                    DateTime date1;
                    if (DateTime.TryParseExact(dateString1, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1))
                    {
                        expiryDateDate1 = date1.ToString("yyyy-MM-dd HH:mm");

                    }


                    string query = "SELECT * FROM tbl_coins_details WHERE id_assessment = '" + assesmentID + "' AND IsActive = 'A' AND Update_date BETWEEN '" + formattedDate + "' AND '" + expiryDateDate1 + "'";

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

                            string updateQuery = "UPDATE tbl_coins_details SET status = @status WHERE id_assessment = @id";

                            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                // Prepare the update command
                                updateCommand.Parameters.Add("@status", MySqlDbType.VarChar);

                          

                                updateCommand.Parameters.Add("@id", MySqlDbType.Int32);

                                // Iterate through each row and perform bulk update
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    int id = Convert.ToInt32(row["id_assessment"]);

                                    updateCommand.Parameters["@status"].Value = "D";

                               

                                    updateCommand.Parameters["@id"].Value = id;


                                    int rowsAffected = updateCommand.ExecuteNonQuery();
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