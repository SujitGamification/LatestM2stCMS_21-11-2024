using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Models
{
    public class tbl_learning_sub_category
    {
        public int IdLearningSubCategory { get; set; }
        public IEnumerable<SelectListItem> IdLearningCategoryList { get; set; }
        public int IdLearningCategory { get; set; }
        public string LearningCategoryName { get; set; }
        public int IdOrganization { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
        public int IdCmsUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }


    public class tbl_learning_sub_categoryModel
    {
        private MySqlConnection conn;

        // Constructor to initialize connection string
        public tbl_learning_sub_categoryModel()
        {
            conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);
        }

        // Method to retrieve categories from the database
        public IEnumerable<SelectListItem> GetCategories(int Orgid)
        {
            var categories = new List<SelectListItem>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(conn.ConnectionString))
                {
                    string query = "SELECT id_learning_category, category_name FROM tbl_learning_category where id_organization = " + Orgid + " AND status='A' ";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new SelectListItem
                            {
                                Value = reader["id_learning_category"].ToString(),
                                Text = reader["category_name"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle error (consider using Serilog or other logging)
                // For example: Log.Error(ex, "Error fetching categories");
            }

            return categories;
        }

        public IEnumerable<SelectListItem> GetSubCategories(int Orgid,int id)
        {
            var categories = new List<SelectListItem>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(conn.ConnectionString))
                {
                    string query = "SELECT id_learning_sub_category, sub_category_name FROM tbl_learning_sub_category where id_organization = " + Orgid + " AND id_learning_category ="+ id +" AND status='A' ";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new SelectListItem
                            {
                                Value = reader["id_learning_sub_category"].ToString(),
                                Text = reader["sub_category_name"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle error (consider using Serilog or other logging)
                // For example: Log.Error(ex, "Error fetching categories");
            }

            return categories;
        }

        public IEnumerable<SelectListItem> GetSubCategoriesQ(int Orgid, int id ,int sb)
        {
            var categories = new List<SelectListItem>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(conn.ConnectionString))
                {
                    string query = "SELECT id_learning_sub_category, sub_category_name FROM tbl_learning_sub_category where id_organization = " + Orgid + " AND id_learning_category ="+ id + " AND id_learning_sub_category ="+sb+" AND status='A' ";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new SelectListItem
                            {
                                Value = reader["id_learning_sub_category"].ToString(),
                                Text = reader["sub_category_name"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle error (consider using Serilog or other logging)
                // For example: Log.Error(ex, "Error fetching categories");
            }

            return categories;
        }

        public string Addlearning_sub_category(tbl_learning_sub_category temp)

        {

            string str = (string)null;

            try

            {

                if (temp.IdLearningSubCategory != 0)

                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        // Open the connection
                        conn.Open();

                        command.CommandText = "UPDATE tbl_learning_sub_category  SET id_learning_category =@id_learning_category, id_organization = @id_organization, sub_category_name = @sub_category_name, sub_category_description = @sub_category_description, id_cms_user = @id_cms_user WHERE id_learning_sub_category = " + temp.IdLearningSubCategory + "";

                        command.Parameters.AddWithValue("@id_learning_category", temp.IdLearningCategory);

                        command.Parameters.AddWithValue("@id_organization", temp.IdOrganization);

                        command.Parameters.AddWithValue("@sub_category_name", temp.SubCategoryName);

                        command.Parameters.AddWithValue("@sub_category_description", temp.SubCategoryDescription);

                        command.Parameters.AddWithValue("@id_cms_user", temp.IdCmsUser);

                


                        str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                    }

                }

                else

                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        // Open the connection
                        conn.Open();

                        command.CommandText = "INSERT INTO tbl_learning_sub_category (id_learning_category,Id_organization, sub_category_name, sub_category_description, id_cms_user, created_date) VALUES (@id_learning_category,@id_organization, @sub_category_name, @sub_category_description, @id_cms_user, @created_date)";

                        command.Parameters.AddWithValue("@id_learning_category", temp.IdLearningCategory);
                        command.Parameters.AddWithValue("@id_organization", temp.IdOrganization);
                        command.Parameters.AddWithValue("@sub_category_name", temp.SubCategoryName);
                        command.Parameters.AddWithValue("@sub_category_description", temp.SubCategoryDescription);
                        command.Parameters.AddWithValue("@id_cms_user", temp.IdCmsUser);
                        command.Parameters.AddWithValue("@created_date", temp.CreatedDate);
    


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



        public List<tbl_learning_sub_category> sub_categoryList(int num)
        {

            List<tbl_learning_sub_category> CategoryList = new List<tbl_learning_sub_category>();



            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))

            {


                string query = @"SELECT sc.id_learning_sub_category,sc.id_learning_category,c.category_name,sc.id_organization,sc.sub_category_name,sc.sub_category_description,sc.id_cms_user,sc.created_date FROM tbl_learning_sub_category sc join tbl_learning_category c where sc.id_organization = @Id_organization AND sc.status ='A' AND sc.id_learning_category = c.id_learning_category";




                using (MySqlCommand command = new MySqlCommand(query, connection))

                {



                    command.Parameters.AddWithValue("@Id_organization", num);



                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();



                    while (reader.Read())

                    {

                        tbl_learning_sub_category subcategory = new tbl_learning_sub_category();

                        subcategory.IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]);

                        subcategory.IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]);

                        subcategory.LearningCategoryName = reader["category_name"].ToString();

                        subcategory.IdOrganization = Convert.ToInt32(reader["id_organization"]);

                        subcategory.SubCategoryName = reader["sub_category_name"].ToString();

                        subcategory.SubCategoryDescription = reader["sub_category_description"].ToString();

                        subcategory.IdCmsUser = Convert.ToInt32(reader["id_cms_user"]);

                        subcategory.CreatedDate = Convert.ToDateTime(reader["created_date"]);




                        CategoryList.Add(subcategory);

                    }



                    reader.Close();

                }

            }



            return CategoryList;

        }

        public string SubDeletelearning_category(int temp)

        {

            string str = (string)null;

            try

            {


                using (MySqlCommand command = this.conn.CreateCommand())
                {

                    conn.Open();

                    command.CommandText = "UPDATE tbl_learning_sub_category  SET status = 'D' WHERE id_learning_sub_category = " + temp + "";


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

        public List<tbl_learning_sub_category> sub_categoryListAssigment(int num,int id)
        {
            List<tbl_learning_sub_category> CategoryList = new List<tbl_learning_sub_category>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {

                string query = @"SELECT * FROM tbl_learning_sub_category where id_learning_category = @id_learning_category AND id_organization = @Id_organization AND status ='A' ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id_organization", num);
                    command.Parameters.AddWithValue("@id_learning_category", id);

                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        tbl_learning_sub_category subcategory = new tbl_learning_sub_category();

                        subcategory.IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]);

                        subcategory.IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]);

                        subcategory.IdOrganization = Convert.ToInt32(reader["id_organization"]);

                        subcategory.SubCategoryName = reader["sub_category_name"].ToString();

                        subcategory.SubCategoryDescription = reader["sub_category_description"].ToString();




                        CategoryList.Add(subcategory);

                    }



                    reader.Close();

                }

            }



            return CategoryList;

        }

    }


}