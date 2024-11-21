using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{

    public class FeedbackModel
    {


    }

    public class tbl_feedback_configuration
    {
        public int Id_Feedback {  get; set; }
        public int Organization_id { get; set; }
        public string Image_path { get; set; } // Store the file path as a string
        public HttpPostedFileBase ImageFile { get; set; }
        public string Header_Text { get; set; }
        public string Feedback_Text { get; set; }
        public string Text_Button_Colour { get; set; }

        public string Organization_name { get; set; }   
    }

    public class addFeedbackModel
    {
        private MySqlConnection conn;

        public addFeedbackModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

        public string add_feedback(tbl_feedback_configuration temp)
        {
            string str = (string)null;
            try
            {
                if (temp.Id_Feedback != 0)
                {
                    MySqlCommand command = this.conn.CreateCommand();
                    command.CommandText = "UPDATE tbl_feedback_configuration SET Organization_id = @Organization_id, Image_Path = @Image_Path, Header_Text = @Header_Text, Feedback_Text = @Feedback_Text, Text_Button_Colour = @Text_Button_Colour WHERE Id_Feedback = "+temp.Id_Feedback+"";
                    command.Parameters.AddWithValue("@Organization_id", temp.Organization_id);
                    command.Parameters.AddWithValue("@Image_Path", temp.Image_path);
                    command.Parameters.AddWithValue("@Header_Text", temp.Header_Text);
                    command.Parameters.AddWithValue("@Feedback_Text", temp.Feedback_Text);
                    command.Parameters.AddWithValue("@Text_Button_Colour", temp.Text_Button_Colour);
                   
                    this.conn.Open();
                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                }

                else
                {
                    MySqlCommand command = this.conn.CreateCommand();
                    command.CommandText = "INSERT INTO tbl_feedback_configuration (Organization_id, Image_Path, Header_Text, Feedback_Text, Text_Button_Colour) VALUES (@Organization_id, @Image_Path, @Header_Text, @Feedback_Text, @Text_Button_Colour)";
                    command.Parameters.AddWithValue("@Organization_id", temp.Organization_id);
                    command.Parameters.AddWithValue("@Image_Path", temp.Image_path);
                    command.Parameters.AddWithValue("@Header_Text", temp.Header_Text);
                    command.Parameters.AddWithValue("@Feedback_Text", temp.Feedback_Text);
                    command.Parameters.AddWithValue("@Text_Button_Colour", temp.Text_Button_Colour);
                    this.conn.Open();
                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                }
             
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return str;
        }

        public List<tbl_feedback_configuration> GetFeedbackData(int num)
        {
           

            List<tbl_feedback_configuration> feedbackList = new List<tbl_feedback_configuration>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                // SQL query to fetch feedback data
                string query =@"
            SELECT fc.Id_Feedback, fc.Organization_id, fc.Image_path, fc.Header_Text, fc.Feedback_Text, fc.Text_Button_Colour,
                   org.ORGANIZATION_NAME
            FROM tbl_feedback_configuration AS fc
            LEFT JOIN tbl_organization AS org ON fc.Organization_id = org.ID_ORGANIZATION
            WHERE org.ID_ORGANIZATION = @OrganizationId AND fc.Status = 'A'";


                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameter for the organization ID
                    command.Parameters.AddWithValue("@OrganizationId", num);

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tbl_feedback_configuration feedback = new tbl_feedback_configuration();
                        feedback.Id_Feedback = Convert.ToInt32(reader["Id_Feedback"]);
                        feedback.Organization_id = Convert.ToInt32(reader["Organization_id"]);
                        feedback.Image_path = reader["Image_path"].ToString();
                        feedback.Header_Text = reader["Header_Text"].ToString();
                        feedback.Feedback_Text = reader["Feedback_Text"].ToString();
                        feedback.Text_Button_Colour = reader["Text_Button_Colour"].ToString();

                        // Add organization name if you fetched it from the query
                        if (!reader.IsDBNull(reader.GetOrdinal("ORGANIZATION_NAME")))
                        {
                            feedback.Organization_name = reader["ORGANIZATION_NAME"].ToString();
                        }

                        feedbackList.Add(feedback);
                    }

                    reader.Close();
                }
            }

            return feedbackList;
        }


        public void DeleteFeedback(int id)
        {
            try
            {
                conn.Open();
                string query = "UPDATE tbl_feedback_configuration SET Status = 'D' WHERE Id_Feedback = @Id";
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.conn.Close();
            }

        }

    }

}




