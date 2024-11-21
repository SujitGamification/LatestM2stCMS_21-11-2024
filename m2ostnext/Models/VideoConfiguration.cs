using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class VideoConfiguration
    {
    }
    public class tbl_video_configuration
    {
       
       public int Id_video { get; set; }
        public int Id_organization { get; set; }

        public string Header_text { get; set; }

        public string Video_name_web { get; set; }
        public HttpPostedFileBase Video_name_webFile { get; set; }

        public string Video_name_mobile { get; set; }
        public HttpPostedFileBase Video_name_mobileFile { get; set; }

        public string Organization_name { get; set; }

    }
    public class addVideoModel

    {

        private MySqlConnection conn;



        public addVideoModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);



        public string add_video(tbl_video_configuration temp)

        {

            string str = (string)null;

            try

            {

                if (temp.Id_video != 0)

                {

                    MySqlCommand command = this.conn.CreateCommand();

                    command.CommandText = "UPDATE tbl_video_configuration SET Id_organization = @Id_organization, Header_text = @Header_text, Video_name_web = @Video_name_web, Video_name_mobile = @Video_name_mobile WHERE Id_video = " + temp.Id_video + "";

                    command.Parameters.AddWithValue("@Id_organization", temp.Id_organization);

                    command.Parameters.AddWithValue("@Header_text", temp.Header_text);

                    command.Parameters.AddWithValue("@Video_name_web", temp.Video_name_web);

                    command.Parameters.AddWithValue("@Video_name_mobile", temp.Video_name_mobile);

                    this.conn.Open();

                    str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";

                }

                else

                {

                    MySqlCommand command = this.conn.CreateCommand();

                    command.CommandText = "INSERT INTO tbl_video_configuration (Id_organization, Header_text, Video_name_web, Video_name_mobile) VALUES (@Id_organization, @Header_text, @Video_name_web, @Video_name_mobile)";

                    command.Parameters.AddWithValue("@Id_organization", temp.Id_organization);

                    command.Parameters.AddWithValue("@Header_text", temp.Header_text);

                    command.Parameters.AddWithValue("@Video_name_web", temp.Video_name_web);

                    command.Parameters.AddWithValue("@Video_name_mobile", temp.Video_name_mobile);

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



        public List<tbl_video_configuration> GetVideoData(int num)

        {

            List<tbl_video_configuration> VideoList = new List<tbl_video_configuration>();



            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))

            {

                // SQL query to fetch feedback data

                string query = @" SELECT vc.*,
                             org.ORGANIZATION_NAME
                             FROM tbl_video_configuration AS vc
                             LEFT JOIN tbl_organization AS org ON vc.ID_ORGANIZATION = org.ID_ORGANIZATION
                             WHERE org.ID_ORGANIZATION = @ID_ORGANIZATION AND vc.STATUS = 'A'";
                




                using (MySqlCommand command = new MySqlCommand(query, connection))

                {

                    // Add parameter for the organization ID

                    command.Parameters.AddWithValue("@Id_organization", num);



                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();



                    while (reader.Read())

                    {

                        tbl_video_configuration video = new tbl_video_configuration();

                        video.Id_video = Convert.ToInt32(reader["Id_video"]);

                        video.Id_organization = Convert.ToInt32(reader["Id_organization"]);

                        video.Header_text = reader["Header_text"].ToString();

                        video.Video_name_web = reader["Video_name_web"].ToString();

                        video.Video_name_mobile = reader["Video_name_mobile"].ToString();



                        // Add organization name if you fetched it from the query

                        if (!reader.IsDBNull(reader.GetOrdinal("ORGANIZATION_NAME")))

                        {

                            video.Organization_name = reader["ORGANIZATION_NAME"].ToString();

                        }



                        VideoList.Add(video);

                    }



                    reader.Close();

                }

            }



            return VideoList;

        }







    }
}