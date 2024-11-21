using MySql.Data.MySqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Models
{
    public class Ngage
    {
        public class tbl_game_master
        {
            public int Id { get; set; }
            public string GameName { get; set; }
            public char IsActive { get; set; }
            public DateTime UpdatedDateTime { get; set; }
            public int CmsUserId { get; set; }
            public string GameUrl { get; set; }
        }

        public class tbl_assessment
        {
            public int Id { get; set; }
            public int GameId { get; set; }
            public string Title { get; set; }
            public int OrganizationId { get; set; }
            public char IsActive { get; set; }
            public DateTime UpdatedDateTime { get; set; }
            public int CmsUserId { get; set; }
            public int AllowAttempt { get; set; }
            public int PreviousButton { get; set; }
            public string SurveyType { get; set; }
        }

        public class Ngage_service

        {

            private MySqlConnection connection;



            public Ngage_service()

            {

                string connectionString = ConfigurationManager.ConnectionStrings["db_tgc_gameEntities1"].ConnectionString;

                this.connection = new MySqlConnection(connectionString);

            }

            public List<SelectListItem> GetngageList()
            {
                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    string ngageQuery = "select * from tbl_game_master where IsActive = 'A'";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem ngageItem = new SelectListItem
                            {
                                Value = reader["Id"].ToString(),
                                Text = reader["GameName"].ToString()
                            };

                            list.Add(ngageItem);
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

            public List<SelectListItem> GetOrgIdList()
            {
                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    string ngageQuery = "select * from tbl_organization where STATUS = 'A'";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem orgItem = new SelectListItem
                            {
                                Value = reader["ID_ORGANIZATION"].ToString(),
                                Text = reader["ORGANIZATION_NAME"].ToString()
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

            public List<SelectListItem> GetgameId(int Idgame,int Idorg)
            {
                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    string ngageQuery = "SELECT * FROM tbl_assessment WHERE Id_Game = '"+ Idgame + "' AND ID_ORGANIZATION = '"+ Idorg + "' and IsActive= 'A'";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem orgItem = new SelectListItem
                            {
                                Value = reader["Id_Assessment"].ToString(),
                                Text = reader["Assessment_Title"].ToString()
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

            public List<SelectListItem> GetgameUrl1(int ngageDropdown)
            {
                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    //string ngageQuery = "SELECT GameUrl FROM tbl_game_master WHERE Id = @ngageDropdown1 AND IsActive = 'A'";



                    //using (MySqlCommand command = new MySqlCommand(ngageQuery, connection))

                    //{
                    //    List<tbl_game_master> Coinslist = new List<tbl_game_master>();
                    //    // Add parameter for the organization ID

                    //    command.Parameters.AddWithValue("@ngageDropdown1", ngageDropdown);
                     


                        
                    //    MySqlDataReader reader = command.ExecuteReader();



                    //    while (reader.Read())

                    //    {

                    //        tbl_game_master Coins1 = new tbl_game_master();



                    //        Coins1.GameUrl = reader["GameUrl"].ToString();


                    //        Coinslist.Add(Coins1);

                    //    }



                    //    reader.Close();

                    //}

                    string ngageQuery = "SELECT GameUrl FROM tbl_game_master WHERE Id ='"+ngageDropdown+"' AND IsActive = 'A'";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem orgItem = new SelectListItem
                            {
                                Value = reader["GameUrl"].ToString(),
                                Text = reader["GameUrl"].ToString()
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

            public List<SelectListItem> GetAssList(int Idorg)
            {
                List<SelectListItem> list = new List<SelectListItem>();

                try
                {
                    connection.Open();

                    string ngageQuery = "select * from tbl_assessment where IsActive = 'A' and ID_ORGANIZATION='" + Idorg + "'  order by Id_Assessment desc";
                    MySqlCommand cmd = new MySqlCommand(ngageQuery, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectListItem ngageItem = new SelectListItem
                            {
                                Value = reader["Assessment_Title"].ToString(),
                                Text = reader["Assessment_Title"].ToString()
                            };

                            list.Add(ngageItem);
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

        }
    }
}