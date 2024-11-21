using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class loginPageConfiguration
    {

    }
    public class tbl_Login_page

    {

        public int Id_login { get; set; }

        public int Organization { get; set; }

        public HttpPostedFileBase BackgroundFile { get; set; }

        public string Background_Image { get; set; }

        public HttpPostedFileBase LogoFile { get; set; }

        public string Logo_Image { get; set; }

        public string Text_Button_Color { get; set; }

        public string Organization_name { get; set; }

        public string bgImage_pathhidden { get; set; }

        public string logoImage_pathhidden { get;set; }

    }

    public class addLoginModel

    {

        private MySqlConnection conn;



        public addLoginModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);



        public string add_login(tbl_Login_page temp)

        {

            string str = (string)null;

            try

            {

                if (temp.Id_login != 0)

                {

                    MySqlCommand command = this.conn.CreateCommand();

                    command.CommandText = "UPDATE tbl_Login_page SET Organization = @Organization, Background_Image = @Background_Image, Logo_Image = @Logo_Image, Text_Button_Color = @Text_Button_Color WHERE Id_login = " + temp.Id_login + "";

                    command.Parameters.AddWithValue("@Organization", temp.Organization);

                    command.Parameters.AddWithValue("@Background_Image", temp.Background_Image);

                    command.Parameters.AddWithValue("@Logo_Image", temp.Logo_Image);

                    command.Parameters.AddWithValue("@Text_Button_Color", temp.Text_Button_Color);

                    this.conn.Open();

                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";

                }

                else

                {

                    MySqlCommand command = this.conn.CreateCommand();

                    command.CommandText = "INSERT INTO tbl_Login_page (Organization, Background_Image, Logo_Image, Text_Button_Color) VALUES (@Organization, @Background_Image, @Logo_Image, @Text_Button_Color)";

                    command.Parameters.AddWithValue("@Organization", temp.Organization);

                    command.Parameters.AddWithValue("@Background_Image", temp.Background_Image);

                    command.Parameters.AddWithValue("@Logo_Image", temp.Logo_Image);

                    command.Parameters.AddWithValue("@Text_Button_Color", temp.Text_Button_Color);

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



        public List<tbl_Login_page> GetLoginPageData(int num)

        {

            List<tbl_Login_page> loginPageList = new List<tbl_Login_page>();



            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))

            {

                // SQL query to fetch feedback data

                string query = @"SELECT fc.Id_login, fc.Organization, fc.Background_Image, fc.Logo_Image,fc.Text_Button_Color,

                org.ORGANIZATION_NAME

                FROM tbl_Login_page AS fc

                LEFT JOIN tbl_organization AS org ON fc.Organization = org.ID_ORGANIZATION

                WHERE org.ID_ORGANIZATION = @Organization AND fc.Status='A'";





                using (MySqlCommand command = new MySqlCommand(query, connection))

                {

                    // Add parameter for the organization ID

                    command.Parameters.AddWithValue("@Organization", num);



                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();



                    while (reader.Read())

                    {

                        tbl_Login_page loginPage = new tbl_Login_page();

                        loginPage.Id_login = Convert.ToInt32(reader["Id_login"]);

                        loginPage.Organization = Convert.ToInt32(reader["Organization"]);

                        loginPage.Background_Image = reader["Background_Image"].ToString();

                        loginPage.Logo_Image = reader["Logo_Image"].ToString();

                        loginPage.Text_Button_Color = reader["Text_Button_Color"].ToString();



                        // Add organization name if you fetched it from the query

                        if (!reader.IsDBNull(reader.GetOrdinal("ORGANIZATION_NAME")))

                        {

                            loginPage.Organization_name = reader["ORGANIZATION_NAME"].ToString();

                        }



                        loginPageList.Add(loginPage);

                    }



                    reader.Close();

                }

                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }

            }



            return loginPageList;

        }







        public void DeleteLogin(int id)

        {

            try

            {

                conn.Open();

                string query = "UPDATE tbl_Login_page SET Status = 'D' WHERE Id_login = @Id";

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