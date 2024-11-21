using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class tbl_learning_category
    {
        public int IdLearningCategory { get; set; }  
        public int IdOrganization { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; } 
        public int IdCmsUser { get; set; }           
        public DateTime CreatedDate { get; set; }    
        public string Status { get; set; }             
        public int CategoryOrderNumber { get; set; }

    }

    public class Addlearning_categoryModel

    {

        private MySqlConnection conn;



        public Addlearning_categoryModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);



        public string Addlearning_category(tbl_learning_category temp)

        {

            string str = (string)null;

            try

            {

                if (temp.IdLearningCategory != 0)

                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        // Open the connection
                        conn.Open();

                        command.CommandText = "UPDATE tbl_learning_category  SET id_organization = @id_organization, category_name = @category_name, category_description = @category_description, id_cms_user = @id_cms_user,category_order_number=@category_order_number WHERE id_learning_category = " + temp.IdLearningCategory + "";

                        command.Parameters.AddWithValue("@id_organization", temp.IdOrganization);

                        command.Parameters.AddWithValue("@category_name", temp.CategoryName);

                        command.Parameters.AddWithValue("@category_description", temp.CategoryDescription);

                        command.Parameters.AddWithValue("@id_cms_user", temp.IdCmsUser);

                        command.Parameters.AddWithValue("@category_order_number", temp.CategoryOrderNumber);



                        str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                    }

                }

                else

                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        // Open the connection
                        conn.Open();

                        command.CommandText = "INSERT INTO tbl_learning_category (Id_organization, category_name, category_description, id_cms_user, created_date, category_order_number) VALUES (@id_organization, @category_name, @category_description, @id_cms_user, @created_date, @category_order_number)";

                        command.Parameters.AddWithValue("@id_organization", temp.IdOrganization);
                        command.Parameters.AddWithValue("@category_name", temp.CategoryName);
                        command.Parameters.AddWithValue("@category_description", temp.CategoryDescription);
                        command.Parameters.AddWithValue("@id_cms_user", temp.IdCmsUser);
                        command.Parameters.AddWithValue("@created_date", temp.CreatedDate);
                        command.Parameters.AddWithValue("@category_order_number", temp.CategoryOrderNumber);


                        str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";

                    }
                }

            }

            catch (Exception ex)

            {


            }

            finally
            {
                // Ensure connection is closed
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return str;

        }



        public List<tbl_learning_category> GetCategoryData(int num)
        {

            List<tbl_learning_category> CategoryList = new List<tbl_learning_category>();



            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))

            {

              
                string query = @"SELECT * FROM tbl_learning_category where id_organization = @Id_organization and status ='A' order by id_learning_category asc";





                using (MySqlCommand command = new MySqlCommand(query, connection))

                {

                 

                    command.Parameters.AddWithValue("@Id_organization", num);



                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();



                    while (reader.Read())

                    {

                        tbl_learning_category category = new tbl_learning_category();

                        category.IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]);

                        category.IdOrganization = Convert.ToInt32(reader["id_organization"]);

                        category.CategoryName = reader["category_name"].ToString();

                        category.CategoryDescription = reader["category_description"].ToString();

                        category.IdCmsUser =Convert.ToInt32( reader["id_cms_user"]);
                      
                        category.CreatedDate = Convert.ToDateTime(reader["created_date"]);

                        category.Status = reader["status"].ToString();

                        category.CategoryOrderNumber =Convert.ToInt32(reader["category_order_number"]);




                        CategoryList.Add(category);

                    }



                    reader.Close();

                }

            }



            return CategoryList;

        }


        public string Deletelearning_category(int temp)

        {

            string str = (string)null;

            try

            {

             
                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                       
                        conn.Open();




                    command.CommandText = "UPDATE tbl_learning_category SET status = 'D' WHERE id_learning_category = " + temp;
                    int rowsAffectedCategory = command.ExecuteNonQuery();

                    if(rowsAffectedCategory > 0)
                    {
                        DataTable dt = new DataTable();

                        // Use a DataAdapter to fill the DataTable with the results of the SELECT query
                        command.CommandText = "SELECT * FROM tbl_learning_sub_category WHERE id_learning_category = " + temp;

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            // Update the tbl_learning_sub_category if records were found
                            command.CommandText = "UPDATE tbl_learning_sub_category SET status = 'D' WHERE id_learning_category = " + temp;
                            int rowsAffectedSubCategory = command.ExecuteNonQuery();

                            // Check if both updates were successful
                            str = (rowsAffectedCategory > 0 && rowsAffectedSubCategory > 0) ? "TRUE" : "FALSE";
                        }
                        str = (rowsAffectedCategory > 0 && rowsAffectedCategory > 0) ? "TRUE" : "FALSE";

                    }


                }



            }

            catch (Exception ex)

            {


            }

            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return str;

        }


        public string Updatelearning_category(int id,int newOrder)
        {

            string str = (string)null;

            try

            {

                using (MySqlCommand command = this.conn.CreateCommand())
                {

                    conn.Open();

                   
                    command.CommandText = "UPDATE tbl_learning_category  SET category_order_number = " + newOrder + " WHERE id_learning_category = " + id + "";


                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                }
            }

            catch (Exception ex)

            {


            }

            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return str;
        }



    }
}