using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Models
{
    public class tbl_kpi_master_details
    {
        public int ID_KPI { get; set; }

        public string KPI_Name { get; set; }

        public int ID_Organization { get; set; }

        public int KPI_Type { get; set; }

        public int KPI_SubType { get; set; }

        public string Description { get; set; }

        public int Scoring_Logic { get; set; }

        public int Corebus_KPI_ID { get; set; }

        public string IsActive { get; set; }

        public int created_by { get; set; }

        public DateTime CreatedAt { get; set; }

        public int updated_by { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int game_id { get; set; }
    }

    public class tbl_kpi_sub_type_details
    {
        public int ID_KPI_Sub_Type { get; set; }
        public int ID_KPI_Type { get; set; }
        public string KPI_Sub_Type { get; set;}

        public string IsActive { get; set; }

        public DateTime created_date {  get; set; } 

        public int updated_by { get; set;}
        public DateTime UpdatedDate { get; set;}

    }
    public class tbl_scoring_type
    {
        public int Id_scoring_type { get; set; }
         public int   Id_organization { get; set; }
        public string  Sub_type_name { get; set; }
        public int Sub_type_name_id { get; set; }
         public string   Category_type_name { get; set;}    
         public int   Category_id { get; set; }
         public string   Attempt_required { get; set; }
    }

    public class tbl_scoring_type_details
    {
        public int Id_scoring_type_details {  get; set; }
          public int Id_organization { get; set; }  
         public int Name_kpi_id {  get; set; }  
         public int Id_assessment { get; set; } 
          public int Sub_type_name_Id {  get; set; }
          public int Category_Id {  get; set; }
          public int Attempt_number { get; set; }
           public int Attempt_Id { get; set; }
         public string Mastery_percentage {  get; set; }
         public string Duration { get; set; }
         public string Start_range { get; set; }
         public string End_range { get; set; }
        public string Points { get; set; }

        public string KPI_Name { get; set; }

        public string Sub_type_name { get; set; }
        public string Status { get; set; }
    }

    public class tbl_coins_master
    {
        public int Id_Coins { get; set; }
        public int Attempt_no { get; set; }
        public int Set_percentage { get; set; }
        public int Set_Score { get; set; }
        public string status { get; set; }
        public int Id_organization { get; set; }
        public int Id_assessment { get; set; }
 
    }

    public class tbl_kpi_scoring_master_details
    {
        public int ID { get; set; }
       public int  ID_Scoring_Matrix { get; set; }
        public int ID_KPI { get;set; }

        public int ID_Assessment_Type { get; set; }

        public int Content_Assessment_ID { get; set; }   

        public int ApplyMasterScoreMultipleAttempts { get; set; }

        public  int ApplyRightAnswerMultipleAttempts {  get; set; }

        public string IsActive { get; set; }    

        public int created_by { get; set; }   
        
        public string created_date {  get; set; }


  
        public int AttemptNo { get; set; }
        public int Score { get; set; }
        public decimal Points { get; set; }
        public int updated_by { get; set; } 

    }

    public class ScoringMatrix
    {
        public int ID { get; set; }
        public int ID_Scoring_Matrix { get; set; }
        public int AttemptNo { get; set; }
        public int Score { get; set; }
        public decimal Points { get; set; }
        public char IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class tbl_scoring_type_detailsTable
    {
        public int ID { get; set; }
        public int Id_scoring_matrix { get; set; }
        public int Id_kpi { get; set; }
        public string KPI_Name { get; set; }
        public int Content_Assessment_ID { get; set; }
        public int AttemptNo { get; set; }
        public int Score { get; set; }
        public decimal Points { get; set; }
        public char IsActive { get; set; }
    }

    public class tbl_kpi_scoring_master_details_rightAns
    {
        //public int Id { get; set; }
        public int ID_Scoring_Matrix { get; set; }
        public int ID_KPI { get; set; }
        public string KPI_Name { get; set; }
        public int ID_Assessment_Type { get; set; }

        public int Content_Assessment_ID { get; set; }

        public int ApplyMasterScoreMultipleAttempts { get; set; }

        public int ApplyRightAnswerMultipleAttempts { get; set; }

        public char IsActive { get; set; }

        public int created_by { get; set; }

        public string created_date { get; set; }


        public int ID { get; set; }

        public int AttemptNo { get; set; }
        public decimal Points { get; set; }
    
        public int updated_by { get; set; }
        public DateTime updated_date { get; set; }


    }

    public class tbl_content_asessment_completion_timeframe_details
    {
        public int ID_Scoring_Matrix { get; set; }
        public int KPI_Type { get; set; }
        public string Category { get; set; }

        public int ID_KPI { get; set; }

        public int TimePeriod { get; set; }

        public decimal Points { get; set; }

        public char IsActive { get; set; }

        public int created_by { get; set; }

        public string created_date { get; set; }

        public int updated_by { get; set; }
        public DateTime updated_date { get; set; }
        public string KPI_Name { get; set; }
        public int Content_Assessment_ID { get; set; }
        public int AttemptNo { get; set; }
        public int Score { get; set; }



    }

    public class KPI_name_service

    {

        private MySqlConnection connection;



        public KPI_name_service()

        {

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            this.connection = new MySqlConnection(connectionString);

        }



        public List<SelectListItem> GetKpiList(int orgid)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
                connection.Open();

                string kpiQuery = "SELECT ID_KPI, KPI_Name FROM tbl_kpi_master_details where ID_Organization ='" + orgid + "' AND IsActive='A' order by KPI_SubType asc";
                MySqlCommand cmd = new MySqlCommand(kpiQuery, connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SelectListItem kpiItem = new SelectListItem
                        {
                            Value = reader["ID_KPI"].ToString(),
                            Text = reader["KPI_Name"].ToString()
                        };

                        list.Add(kpiItem);
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

        public List<SelectListItem> GetSubtypesByKpi(int ID_KPI_Type)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
                connection.Open();

                string kpiQuery = "SELECT ID_KPI_Type, KPI_Sub_Type FROM tbl_kpi_sub_type_details where ID_KPI_Type = '" + ID_KPI_Type +"'";
                MySqlCommand cmd = new MySqlCommand(kpiQuery, connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SelectListItem kpiItem = new SelectListItem
                        {
                            Value = reader["ID_KPI_Type"].ToString(),
                            Text = reader["KPI_Sub_Type"].ToString()
                        };

                        list.Add(kpiItem);
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

        public List<SelectListItem> GetnewscroingList(int orgid)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
              
                    connection.Open();

                    string kpiQuery = "SELECT Sub_type_name, Sub_type_name_id FROM tbl_scoring_type WHERE Id_organization='"+orgid+"'";
                    MySqlCommand cmd = new MySqlCommand(kpiQuery, connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem scroingItem = new SelectListItem
                            {
                                Value = reader["Sub_type_name_id"].ToString(),
                                Text = reader["Sub_type_name"].ToString()
                            };

                            list.Add(scroingItem);
                        }
                    }
                
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, throw it, etc.)
            }
            connection.Close();

            return list;
        }

        public List<tbl_scoring_type_detailsTable> Getpointlist(int Id_assessment, int orgid)
        {
            List<tbl_scoring_type_detailsTable> list = new List<tbl_scoring_type_detailsTable>();

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                SELECT mastery.ID, kpi.ID_Scoring_Matrix, kpi.ID_KPI,mastery.AttemptNo, tbl_kpi_master_details.KPI_Name, kpi.Content_Assessment_ID,
                        mastery.Score, mastery.Points, kpi.IsActive
                FROM tbl_kpi_scoring_master_details AS kpi
                LEFT JOIN tbl_assessment_mastery_score_details AS mastery ON kpi.ID_Scoring_Matrix = mastery.ID_Scoring_Matrix
                LEFT JOIN tbl_kpi_master_details ON kpi.ID_KPI = tbl_kpi_master_details.ID_KPI
                WHERE kpi.Content_Assessment_ID = @Id_assessment
                AND kpi.IsActive = 'A'
                AND mastery.IsActive = 'A';";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_assessment", Id_assessment);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tbl_scoring_type_detailsTable scoringTypeDetails = new tbl_scoring_type_detailsTable();

                                scoringTypeDetails.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : Convert.ToInt32(reader["ID"]);
                                scoringTypeDetails.Id_scoring_matrix = reader.IsDBNull(reader.GetOrdinal("ID_Scoring_Matrix")) ? 0 : Convert.ToInt32(reader["ID_Scoring_Matrix"]);
                                scoringTypeDetails.Id_kpi = reader.IsDBNull(reader.GetOrdinal("ID_KPI")) ? 0 : Convert.ToInt32(reader["ID_KPI"]);
                                scoringTypeDetails.KPI_Name = reader.IsDBNull(reader.GetOrdinal("KPI_Name")) ? string.Empty : reader["KPI_Name"].ToString();
                                scoringTypeDetails.Content_Assessment_ID = reader.IsDBNull(reader.GetOrdinal("Content_Assessment_ID")) ? 0 : Convert.ToInt32(reader["Content_Assessment_ID"]);
                                scoringTypeDetails.AttemptNo = reader.IsDBNull(reader.GetOrdinal("AttemptNo")) ? 0 : Convert.ToInt32(reader["AttemptNo"]);
                                scoringTypeDetails.Score = reader.IsDBNull(reader.GetOrdinal("Score")) ? 0 : Convert.ToInt32(reader["Score"]);
                                scoringTypeDetails.Points = reader.IsDBNull(reader.GetOrdinal("Points")) ? 0 : Convert.ToDecimal(reader["Points"]);
                                scoringTypeDetails.IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? ' ' : Convert.ToChar(reader["IsActive"]);

                                list.Add(scoringTypeDetails);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    connection.Close();
                }
            }

            return list;
        }

        public List<tbl_kpi_scoring_master_details_rightAns> GetpointAns(int Id_assessment, int orgid)
        {
            List<tbl_kpi_scoring_master_details_rightAns> list = new List<tbl_kpi_scoring_master_details_rightAns>();

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                SELECT mastery.ID,kpi.ID_Scoring_Matrix, kpi.ID_KPI,mastery.AttemptNo, tbl_kpi_master_details.KPI_Name, kpi.Content_Assessment_ID,
        mastery.AttemptNo, mastery.Points, kpi.IsActive
FROM tbl_kpi_scoring_master_details AS kpi
LEFT JOIN tbl_assessment_right_answer_details AS mastery ON kpi.ID_Scoring_Matrix = mastery.ID_Scoring_Matrix
LEFT JOIN tbl_kpi_master_details ON kpi.ID_KPI = tbl_kpi_master_details.ID_KPI
WHERE kpi.Content_Assessment_ID = @Id_assessment
AND kpi.IsActive = 'A'
AND mastery.IsActive = 'A';";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_assessment", Id_assessment);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tbl_kpi_scoring_master_details_rightAns scoringTypeDetails = new tbl_kpi_scoring_master_details_rightAns();

                                scoringTypeDetails.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : Convert.ToInt32(reader["ID"]);
                                scoringTypeDetails.ID_Scoring_Matrix = reader.IsDBNull(reader.GetOrdinal("ID_Scoring_Matrix")) ? 0 : Convert.ToInt32(reader["ID_Scoring_Matrix"]);
                                scoringTypeDetails.ID_KPI = reader.IsDBNull(reader.GetOrdinal("ID_KPI")) ? 0 : Convert.ToInt32(reader["ID_KPI"]);
                                scoringTypeDetails.KPI_Name = reader.IsDBNull(reader.GetOrdinal("KPI_Name")) ? string.Empty : reader["KPI_Name"].ToString();
                                scoringTypeDetails.Content_Assessment_ID = reader.IsDBNull(reader.GetOrdinal("Content_Assessment_ID")) ? 0 : Convert.ToInt32(reader["Content_Assessment_ID"]);
                                scoringTypeDetails.AttemptNo = reader.IsDBNull(reader.GetOrdinal("AttemptNo")) ? 0 : Convert.ToInt32(reader["AttemptNo"]);
                                scoringTypeDetails.Points = reader.IsDBNull(reader.GetOrdinal("Points")) ? 0 : Convert.ToDecimal(reader["Points"]);
                                //scoringTypeDetails.IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? ' ' : Convert.ToChar(reader["IsActive"]);

                                list.Add(scoringTypeDetails);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    connection.Close();
                }
            }

            return list;
        }

        public List<tbl_coins_master> GetCoinlist(int orgId, int assid)

        {

            List<tbl_coins_master> Coinslist = new List<tbl_coins_master>();


            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))

            {

                // SQL query to fetch feedback data

                string query = @" SELECT * FROM tbl_coins_master WHERE id_organization = @ID_ORGANIZATION AND STATUS = 'A' AND Id_assessment=@Id_assessment";

                using (MySqlCommand command = new MySqlCommand(query, connection))

                {

                    // Add parameter for the organization ID

                    command.Parameters.AddWithValue("@Id_organization", orgId);
                    command.Parameters.AddWithValue("@Id_assessment", assid);


                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();



                    while (reader.Read())

                    {

                        tbl_coins_master Coins = new tbl_coins_master();

                        Coins.Id_Coins = Convert.ToInt32(reader["Id_Coins"]);

                        Coins.Attempt_no = Convert.ToInt32(reader["Attempt_no"]);

                        Coins.Set_percentage = Convert.ToInt32(reader["Set_percentage"]);

                        Coins.Set_Score = Convert.ToInt32(reader["Set_Score"]);

                        Coins.Id_organization = Convert.ToInt32(reader["Id_organization"]);

                        Coins.Id_assessment = Convert.ToInt32(reader["Id_assessment"]);

                        Coinslist.Add(Coins);

                    }



                    reader.Close();

                }
                connection.Close(); 

            }



            return Coinslist;

        }

        public List<tbl_content_asessment_completion_timeframe_details> GetOntimeData(int Id_assessment, int orgid)
        {
            List<tbl_content_asessment_completion_timeframe_details> list = new List<tbl_content_asessment_completion_timeframe_details>();

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
SELECT kpi.ID_Scoring_Matrix, kpi.ID_KPI,timrFrame.Category,tbl_kpi_master_details.KPI_Name, kpi.Content_Assessment_ID,
        timrFrame.TimePeriod, timrFrame.Points, kpi.IsActive
FROM tbl_kpi_scoring_master_details AS kpi
LEFT JOIN tbl_content_asessment_completion_timeframe_details AS timrFrame ON kpi.ID_Scoring_Matrix = timrFrame.ID_Scoring_Matrix
LEFT JOIN tbl_kpi_master_details ON kpi.ID_KPI = tbl_kpi_master_details.ID_KPI
WHERE kpi.Content_Assessment_ID = @Id_assessment
AND kpi.IsActive = 'A'
AND timrFrame.IsActive = 'A';";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_assessment", Id_assessment);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tbl_content_asessment_completion_timeframe_details scoringTypeDetails = new tbl_content_asessment_completion_timeframe_details();

                                scoringTypeDetails.ID_Scoring_Matrix = reader.IsDBNull(reader.GetOrdinal("ID_Scoring_Matrix")) ? 0 : Convert.ToInt32(reader["ID_Scoring_Matrix"]);
                                scoringTypeDetails.ID_KPI = reader.IsDBNull(reader.GetOrdinal("ID_KPI")) ? 0 : Convert.ToInt32(reader["ID_KPI"]);
                                scoringTypeDetails.Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? string.Empty : reader["Category"].ToString();
                                scoringTypeDetails.KPI_Name = reader.IsDBNull(reader.GetOrdinal("KPI_Name")) ? string.Empty : reader["KPI_Name"].ToString();
                                scoringTypeDetails.Content_Assessment_ID = reader.IsDBNull(reader.GetOrdinal("Content_Assessment_ID")) ? 0 : Convert.ToInt32(reader["Content_Assessment_ID"]);
                                scoringTypeDetails.TimePeriod = reader.IsDBNull(reader.GetOrdinal("TimePeriod")) ? 0 : Convert.ToInt32(reader["TimePeriod"]);
                                scoringTypeDetails.Points = reader.IsDBNull(reader.GetOrdinal("Points")) ? 0 : Convert.ToDecimal(reader["Points"]);
                                scoringTypeDetails.IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? ' ' : Convert.ToChar(reader["IsActive"]);

                                list.Add(scoringTypeDetails);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    connection.Close();
                }
            }

            return list;
        }

        public void DeleteMastery(int id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
                connection = new MySqlConnection(connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE tbl_assessment_mastery_score_details SET IsActive = 'D' WHERE ID = @ID";
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteRightAnswer(int id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
                connection = new MySqlConnection(connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE tbl_assessment_right_answer_details SET IsActive = 'D' WHERE ID = @ID";
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
        }


        public void OnTimeDelete(int id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
                connection = new MySqlConnection(connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE tbl_content_asessment_completion_timeframe_details SET IsActive = 'D' WHERE ID_Scoring_Matrix = @ID_Scoring_Matrix";
                command.Parameters.AddWithValue("@ID_Scoring_Matrix", id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
        }

        public void CoinsDelete(int id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
                connection = new MySqlConnection(connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE tbl_coins_master SET status = 'D' WHERE Id_Coins = @Id_Coins";
                command.Parameters.AddWithValue("@Id_Coins", id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
        }

        public List<tbl_kpi_master_details> GetKpiMasterDetails(int organizationId)
        {
            List<tbl_kpi_master_details> list = new List<tbl_kpi_master_details>();
            string query = "SELECT * FROM tbl_kpi_master_details WHERE IsActive = 'A' AND id_organization = @OrganizationId";

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrganizationId", organizationId);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tbl_kpi_master_details item = new tbl_kpi_master_details
                            {
                                ID_KPI = reader.GetInt32(reader.GetOrdinal("ID_KPI")),
                                KPI_Name = reader.GetString(reader.GetOrdinal("KPI_Name")),
                                ID_Organization = reader.GetInt32(reader.GetOrdinal("ID_Organization")),
                                KPI_Type = reader.GetInt32(reader.GetOrdinal("KPI_Type")),
                                KPI_SubType = reader.GetInt32(reader.GetOrdinal("KPI_SubType")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Scoring_Logic = reader.GetInt32(reader.GetOrdinal("Scoring_Logic"))
                              
                               
                               
                             
                            };
                            list.Add(item);
                        }
                    }
                }
                conn.Close();
            }
            
            return list;
        }
        public void Add_new(string kpi_name, string kpi_type, string kpi_Subtype, int Orgid)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO tbl_kpi_master_details (kpi_name, kpi_type, kpi_Subtype, Orgid) VALUES (@kpi_name, @kpi_type, @kpi_Subtype, @Orgid)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kpi_name", kpi_name);
                        command.Parameters.AddWithValue("@kpi_type", kpi_type);
                        command.Parameters.AddWithValue("@kpi_Subtype", kpi_Subtype);
                        command.Parameters.AddWithValue("@Orgid", Orgid);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

    }
}

