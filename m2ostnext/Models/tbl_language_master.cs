using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace m2ostnext.Models
{

    public class tbl_language_master
    {
        public int id_language { get; set; } // Corresponds to id_language int(11) AI PK
        public string GoogleLanguageCode { get; set; } // Corresponds to google_language_code varchar(10)
        public string LanguageName { get; set; } // Corresponds to language_name varchar(100)
        public char Status { get; set; } // Corresponds to status char(1)
        public DateTime UpdateDateTime { get; set; }
    }

    public class UserLanguageDetails
    {
        private MySqlConnection conn;

        public UserLanguageDetails()
        {
            // Initializing the connection using the connection string from the config
            this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);
        }

        public List<tbl_language_master> GetLanguageList()
        {
            // List to store the retrieved language data
            List<tbl_language_master> languageList = new List<tbl_language_master>();

            // Ensure the connection object is used consistently
            using (MySqlConnection connection = this.conn)
            {
                connection.Open();

                // Construct the query string
                string query = "SELECT * FROM tbl_language_master WHERE status = 'A'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Use parameterized query to prevent SQL injection
                    //command.Parameters.AddWithValue("@IdOrganization", orgid1);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Assuming 'tbl_language_master' class has properties to set values
                            tbl_language_master language = new tbl_language_master
                            {
                                // Set properties according to your table columns
                                // Example:
                                id_language = reader.GetInt32(reader.GetOrdinal("id_language")),
                                LanguageName = reader.GetString(reader.GetOrdinal("language_name")),
                                // Add other properties as required
                            };

                            languageList.Add(language);
                        }
                    }
                }
            }

            return languageList;
        }
    }
}