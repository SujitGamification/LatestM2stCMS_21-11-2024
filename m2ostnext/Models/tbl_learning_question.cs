using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Input;

namespace m2ostnext.Models
{
    public class tbl_learning_question
    {
        public int IdLearningQuestion { get; set; }
        public int IdOrganization { get; set; }
        public int IdCmsUser { get; set; }
        public int IdLearningCategory { get; set; }
        public IEnumerable<SelectListItem> IdLearningCategoryList { get; set; }

        public int IdLearningSubCategory { get; set; }
        public IEnumerable<SelectListItem> IdLearningSubCategoryList { get; set; }


        public string Title { get; set; }
        public string Question { get; set; }
        public int OptionNumber { get; set; }
        public int OptionNumber_Q1 { get; set; }
        public int OptionNumber_Q2 { get; set; }
        public int OptionNumber_Q3 { get; set; }

        // public string ImageUrl { get; set; }

        public string Image_path { get; set; } // Store the file path as a string
        public HttpPostedFileBase ImageFile { get; set; }

        public string YoutubeUrl { get; set; }
        // public string VideoUrl { get; set; }

        public string VideoUrl { get; set; } // Store the file path as a string
        public HttpPostedFileBase videlImageFile { get; set; }



        public int Points { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; }



        public int IdLearningQuestionAnswer { get; set; }
        public int OptionNumber_QA { get; set; }
        public int OptionNumber_QA1 { get; set; }
        public int OptionNumber_QA2 { get; set; }
        public string OptionAnswer { get; set; }
        public string OptionAnswer_1 { get; set; }
        public string OptionAnswer_2 { get; set; }
        public string OptionAnswer_3 { get; set; }
        public string OptionAnswer_4 { get; set; }


        public int IsCorrectAnswer { get; set; }
        public bool IsCorrectAnswer_1 { get; set; }
        public bool IsCorrectAnswer_2 { get; set; }
        public bool IsCorrectAnswer_3 { get; set; }
        public bool IsCorrectAnswer_4 { get; set; }
        public string StatusQA { get; set; }
        public string category_name { get; set; }
        public string sub_category_name { get; set; }
        public int Numberofattempts { get; set; }

    }

    public class tbl_learning_assigment
    {
        public int IdAssignment { get; set; }
        public int IdOrganization { get; set; }
        public int IdLearningCategory { get; set; }
        public int IdLearningSubCategory { get; set; }
        public string IdUser { get; set; }
        public string IdRole { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }  // Assuming status can be nullable or blank, so keeping it as string
        public DateTime UpdateDateTime { get; set; }
    }

    public class Addlearning_questionModel

    {

        private MySqlConnection conn;



        public Addlearning_questionModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);



        public string Addlearning_question(tbl_learning_question temp, string[] QAArray,tbl_learning_question model)
        {

            //string str = (string)null;
            string result = "FALSE";
            bool isProcessed = false;

            try

            {

                if (temp.IdLearningQuestion != 0)
                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        //Open the connection
                        conn.Open();

                        // Update the tbl_learning_question table
                        command.CommandText = @"
                    UPDATE tbl_learning_question 
                    SET id_organization = @id_organization, 
                        id_cms_user = @id_cms_user, 
                        id_learning_category = @id_learning_category, 
                        id_learning_sub_category = @id_learning_sub_category, 
                        title = @title, 
                        question = @question, 
                        option_number_file_type = @option_number, 
                        image_path = @image_path, 
                        youtube_url = @youtube_url, 
                        video_url = @video_url, 
                        points = @points,
                        number_of_attempt =@Numberofattempts
                       
                    WHERE id_learning_question = @id_learning_question";

                        // Add parameters for tbl_learning_question
                        command.Parameters.AddWithValue("@id_organization", temp.IdOrganization);
                        command.Parameters.AddWithValue("@id_cms_user", temp.IdCmsUser);
                        command.Parameters.AddWithValue("@id_learning_category", temp.IdLearningCategory);
                        command.Parameters.AddWithValue("@id_learning_sub_category", temp.IdLearningSubCategory);
                        command.Parameters.AddWithValue("@title", temp.Title);
                        command.Parameters.AddWithValue("@question", temp.Question);
                        command.Parameters.AddWithValue("@option_number", temp.OptionNumber);
                        command.Parameters.AddWithValue("@image_path", temp.Image_path);
                        command.Parameters.AddWithValue("@youtube_url", temp.YoutubeUrl);
                        command.Parameters.AddWithValue("@video_url", temp.VideoUrl);
                        command.Parameters.AddWithValue("@points", temp.Points);
                        command.Parameters.AddWithValue("@Numberofattempts", temp.Numberofattempts);
                        command.Parameters.AddWithValue("@status", temp.Status);
                        command.Parameters.AddWithValue("@id_learning_question", temp.IdLearningQuestion);

                        // Execute the update command
                        result = command.ExecuteNonQuery() == 1 ? "TRUE" : "FALSE";

                        if (result == "TRUE")
                        {
                            int correctAnswer = 0;
                            if (model.IsCorrectAnswer_1 == true) correctAnswer = 1;
                            else if (model.IsCorrectAnswer_2 == true) correctAnswer = 2;
                            else if (model.IsCorrectAnswer_3 == true) correctAnswer = 3;
                            else correctAnswer = 4;

                            // Select id_learning_question_answer for the question
                            command.CommandText = @"
                                                    SELECT id_learning_question_answer 
                                                    FROM tbl_learning_question_answer 
                                                    WHERE id_learning_question = @id_learning_question";

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id_learning_question", temp.IdLearningQuestion);

                            // Execute the command and store the result in a DataTable
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                            {
                                System.Data.DataTable dtQuestionAnswers = new System.Data.DataTable();
                                adapter.Fill(dtQuestionAnswers);

                                // Iterate over the QAArray and update or delete as necessary
                                for (int i = 0; i < QAArray.Length; i++)
                                {
                                    if (QAArray[i] != null)
                                    {
                                        if (i < dtQuestionAnswers.Rows.Count) // If there's a corresponding existing answer
                                        {
                                            // Update existing answer
                                            command.CommandText = @"
                                            UPDATE tbl_learning_question_answer 
                                            SET id_learning_question = @id_learning_question, 
                                                option_number_qa = @option_number_qa, 
                                                option_answer = @option_answer, 
                                                is_correct_answer = @is_correct_answer 
                                            WHERE id_learning_question_answer = @id_learning_question_answer";

                                            // Add parameters for tbl_learning_question_answer
                                            command.Parameters.Clear();
                                            command.Parameters.AddWithValue("@id_learning_question", temp.IdLearningQuestion);
                                            command.Parameters.AddWithValue("@option_number_qa", temp.OptionNumber_QA);
                                            command.Parameters.AddWithValue("@option_answer", QAArray[i]);

                                            command.Parameters.AddWithValue("@is_correct_answer", (i + 1 == correctAnswer) ? 1 : 0);

                                            command.Parameters.AddWithValue("@id_learning_question_answer", dtQuestionAnswers.Rows[i]["id_learning_question_answer"]);

                                            // Execute the update command
                                            command.ExecuteNonQuery();
                                        }
                                        else if (i >= 2) // If the index exceeds the existing answers count and is greater than 3
                                        {
                                            
                                            command.CommandText = @"
                                            INSERT INTO tbl_learning_question_answer 
                                            (id_learning_question, option_number_qa, option_answer, is_correct_answer) 
                                            VALUES (@id_learning_question, @option_number_qa, @option_answer, @is_correct_answer)";

                                            // Add parameters for tbl_learning_question_answer
                                            command.Parameters.Clear();
                                            command.Parameters.AddWithValue("@id_learning_question", temp.IdLearningQuestion);
                                            command.Parameters.AddWithValue("@option_number_qa", temp.OptionNumber_QA); // Assuming sequential option numbers
                                            command.Parameters.AddWithValue("@option_answer", QAArray[i]);
                                             command.Parameters.AddWithValue("@is_correct_answer", (i + 1 == correctAnswer) ? 1 : 0);

                                            //command.Parameters.AddWithValue("@is_correct_answer", temp.IsCorrectAnswer);

                                            // Execute the insert command
                                            command.ExecuteNonQuery();
                                        }
                                        else if (i == 0) // If the index exceeds the existing answers count and is greater than 3
                                        {

                                            command.CommandText = @"
                                            INSERT INTO tbl_learning_question_answer 
                                            (id_learning_question, option_number_qa, option_answer, is_correct_answer) 
                                            VALUES (@id_learning_question, @option_number_qa, @option_answer, @is_correct_answer)";

                                            // Add parameters for tbl_learning_question_answer
                                            command.Parameters.Clear();
                                            command.Parameters.AddWithValue("@id_learning_question", temp.IdLearningQuestion);
                                            command.Parameters.AddWithValue("@option_number_qa", temp.OptionNumber_QA); // Assuming sequential option numbers
                                            command.Parameters.AddWithValue("@option_answer", QAArray[i]);
                                            command.Parameters.AddWithValue("@is_correct_answer", (i + 1 == correctAnswer) ? 1 : 0);

                                            //command.Parameters.AddWithValue("@is_correct_answer", temp.IsCorrectAnswer);

                                            // Execute the insert command
                                            command.ExecuteNonQuery();
                                        }

                                    }
                                    else
                                    {
                                        if (dtQuestionAnswers.Rows.Count > 2)
                                        {
                                           
                                                command.CommandText = @"
                                                    UPDATE tbl_learning_question_answer 
                                                    SET status = 'D'
                                                    WHERE id_learning_question_answer = @id_learning_question_answer";

                                                command.Parameters.Clear();
                                                command.Parameters.AddWithValue("@id_learning_question_answer", dtQuestionAnswers.Rows[i]["id_learning_question_answer"]);

                                                // Execute the delete (set status 'D')
                                                command.ExecuteNonQuery();
                                            
                                        }
                                        // Set status to 'D' for the deleted entry

                                    }
                                }


                                // Update tbl_learning_question_answer for each QA entry
                                //for (int i = 0; i < QAArray.Length; i++)
                                //{
                                //    if (QAArray[i] != null)
                                //    {
                                //        command.CommandText = @"
                                //UPDATE tbl_learning_question_answer 
                                //SET id_learning_question = @id_learning_question, 
                                //    option_number_qa = @option_number_qa, 
                                //    option_answer = @option_answer, 
                                //    is_correct_answer = @is_correct_answer 
                                //WHERE id_learning_question = @id_learning_question";

                                //        // Add parameters for tbl_learning_question_answer
                                //        command.Parameters.Clear();
                                //        command.Parameters.AddWithValue("@id_learning_question", temp.IdLearningQuestion);
                                //        command.Parameters.AddWithValue("@option_number_qa", temp.OptionNumber_QA); // Assuming sequential option numbers
                                //        command.Parameters.AddWithValue("@option_answer", QAArray[i]);
                                //        command.Parameters.AddWithValue("@is_correct_answer", temp.IsCorrectAnswer);

                                //        // Assuming this comes from temp
                                //        //command.Parameters.AddWithValue("@id_learning_question_answer", temp.IdLearningQuestionAnswer); // Use appropriate Id

                                //        // Execute the update command for QAArray
                                //        command.ExecuteNonQuery();

                                //    }

                                //}
                            }
                        }
                    }

                }

                else

                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        int correctAnswer = 0;
                        if (model.IsCorrectAnswer_1 == true) correctAnswer = 1;
                        else if (model.IsCorrectAnswer_2 == true) correctAnswer = 2;
                        else if (model.IsCorrectAnswer_3 == true) correctAnswer = 3;
                        else correctAnswer = 4;

                        //Open the connection
                        conn.Open();
                        command.CommandText = @"
            INSERT INTO tbl_learning_question 
            (id_organization, id_cms_user, id_learning_category, id_learning_sub_category, title, question, option_number_file_type, image_path, youtube_url, video_url, points,created_date) 
            VALUES 
            (@id_organization, @id_cms_user, @id_learning_category, @id_learning_sub_category, @title, @question, @option_number, @image_path, @youtube_url, @video_url, @points,@createddate)";

                        // Add parameters for tbl_learning_question
                        command.Parameters.AddWithValue("@id_organization", temp.IdOrganization);
                        command.Parameters.AddWithValue("@id_cms_user", temp.IdCmsUser);
                        command.Parameters.AddWithValue("@id_learning_category", temp.IdLearningCategory);
                        command.Parameters.AddWithValue("@id_learning_sub_category", temp.IdLearningSubCategory);
                        command.Parameters.AddWithValue("@title", temp.Title);
                        command.Parameters.AddWithValue("@question", temp.Question);
                        command.Parameters.AddWithValue("@option_number", temp.OptionNumber);
                        command.Parameters.AddWithValue("@image_path", temp.Image_path);
                        command.Parameters.AddWithValue("@youtube_url", temp.YoutubeUrl);
                        command.Parameters.AddWithValue("@video_url", temp.VideoUrl);
                        command.Parameters.AddWithValue("@points", temp.Points);
                        command.Parameters.AddWithValue("@createddate", temp.CreatedDate);
                        
                 

                        // Execute the insert command
                        command.ExecuteNonQuery();

                        // Optionally retrieve the last inserted ID if needed
                        long lastInsertedId = command.LastInsertedId;

                        // Insert into tbl_learning_question_answer for each QA entry
                        for (int i = 0; i < QAArray.Length; i++)
                        {
                            if (QAArray[i] != null)
                            {
                                command.CommandText = @"
                                INSERT INTO tbl_learning_question_answer 
                                (id_learning_question, option_number_qa, option_answer, is_correct_answer,created_date) 
                                VALUES 
                                (@IdLearningQuestion, @OptionNumber_QA, @OptionAnswer, @IsCorrectAnswer,@createddate)";

                                // Add parameters for tbl_learning_question_answer
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@IdLearningQuestion", lastInsertedId); 
                                command.Parameters.AddWithValue("@OptionNumber_QA", temp.OptionNumber_QA); 
                                command.Parameters.AddWithValue("@OptionAnswer", QAArray[i]);


                                command.Parameters.AddWithValue("@IsCorrectAnswer", (i + 1 == correctAnswer) ? 1 : 0);





                                //command.Parameters.AddWithValue("@IsCorrectAnswer", temp.IsCorrectAnswer);
                                command.Parameters.AddWithValue("@createddate", temp.CreatedDate);

                                // Execute the insert command for QAArray
                                result = command.ExecuteNonQuery() == 1 ? "TRUE" : "FALSE";
                            }

                        }
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

            return result;

        }


        public List<tbl_learning_question> QuestionList(int organizationId)
        {
            List<tbl_learning_question> questionList = new List<tbl_learning_question>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"
        SELECT 
            LQ.id_learning_question,
            LQ.id_organization,
            LQ.id_cms_user,
            LQ.id_learning_category,
            C.category_name,
            LQ.id_learning_sub_category,
            SC.sub_category_name,
            LQ.title,
            LQ.question,
            LQ.option_number_file_type,
            LQ.image_path,
            LQ.youtube_url,
            LQ.video_url,
            LQ.points,
            LQ.number_of_attempt,
            LQ.status
        FROM 
            tbl_learning_question LQ
        JOIN 
            tbl_learning_sub_category SC ON LQ.id_learning_sub_category = SC.id_learning_sub_category
        JOIN 
            tbl_learning_category C ON SC.id_learning_category = C.id_learning_category
        WHERE
            LQ.id_organization = @Id_organization
            AND LQ.status = 'A'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_organization", organizationId);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tbl_learning_question question = new tbl_learning_question
                            {
                                IdLearningQuestion = Convert.ToInt32(reader["id_learning_question"]),
                                IdOrganization = Convert.ToInt32(reader["id_organization"]),
                                IdCmsUser = Convert.ToInt32(reader["id_cms_user"]),
                                IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]),
                                category_name = reader["category_name"].ToString(),
                                IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]),
                                sub_category_name = reader["sub_category_name"].ToString(),
                                Title = reader["title"].ToString(),
                                Question = reader["question"].ToString(),
                                OptionNumber = Convert.ToInt32(reader["option_number_file_type"]),
                                Image_path = reader["image_path"].ToString(),
                                YoutubeUrl = reader["youtube_url"].ToString(),
                                VideoUrl = reader["video_url"].ToString(),
                                Points = Convert.ToInt32(reader["points"]),
                                Numberofattempts = reader["number_of_attempt"] != DBNull.Value ? Convert.ToInt32(reader["number_of_attempt"]) : 0,
                                Status = reader["status"].ToString()
                            };

                            questionList.Add(question);
                        }
                    }
                }
            }

            return questionList;
        }

        public List<tbl_learning_question> GetQuestionAnswer(int Id)
        {
            List<tbl_learning_question> questionListA = new List<tbl_learning_question>();
            int loopCounter = 0;
            int countOfCorrectAnswers = 0;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"
                                SELECT 
                                id_learning_question,
                                option_number_qa,
                                option_answer,
                                is_correct_answer from tbl_learning_question_answer where id_learning_question = @Id
                                AND status = 'A'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tbl_learning_question question = new tbl_learning_question
                            {
                                IdLearningQuestion = Convert.ToInt32(reader["id_learning_question"]),
                                OptionNumber_QA = Convert.ToInt32(reader["option_number_qa"]),
                                OptionAnswer = reader["option_answer"].ToString(),
                                IsCorrectAnswer = Convert.ToInt32(reader["is_correct_answer"]),

                            };

                            questionListA.Add(question);

                            // Increment the loop counter
                            loopCounter++;

                            // Check if IsCorrectAnswer is 1 and increment the correctAnswerCount
                            if (question.IsCorrectAnswer == 1)
                            {
                                question.IsCorrectAnswer = loopCounter;
                            }
                        }
                    }
                }
            }

            return questionListA;
        }

        public string Deletelearning_Question(int temp)

        {

            string str = (string)null;

            try

            {


                using (MySqlCommand command = this.conn.CreateCommand())
                {

                    conn.Open();




                    command.CommandText = "UPDATE tbl_learning_question SET status = 'D' WHERE id_learning_question = " + temp;
                    int rowsAffectedCategory = command.ExecuteNonQuery();

                    if (rowsAffectedCategory > 0)
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();

                        // Use a DataAdapter to fill the DataTable with the results of the SELECT query
                        command.CommandText = "SELECT * FROM tbl_learning_question_answer WHERE id_learning_question = " + temp;

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            // Update the tbl_learning_sub_category if records were found
                            command.CommandText = "UPDATE tbl_learning_question_answer SET status = 'D' WHERE id_learning_question = " + temp;
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

        public List<tbl_learning_question> QuestionListTrivia(int organizationId,int id)
        {
            List<tbl_learning_question> questionList = new List<tbl_learning_question>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"select * from tbl_learning_question where id_organization =@Id_organization AND id_learning_sub_category = @id_learning_sub_category AND status='A';";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_organization", organizationId);
                    command.Parameters.AddWithValue("@id_learning_sub_category", id);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tbl_learning_question question = new tbl_learning_question
                            {
                                IdLearningQuestion = Convert.ToInt32(reader["id_learning_question"]),
                                IdOrganization = Convert.ToInt32(reader["id_organization"]),
                                IdCmsUser = Convert.ToInt32(reader["id_cms_user"]),
                                IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]),
                                
                                IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]),
                               
                              
                                Question = reader["question"].ToString(),
                            
                            };

                            questionList.Add(question);
                        }
                    }
                }
            }

            return questionList;
        }

        public int QuestionListTriviastatus(int organizationId,int userContentId,int IdsubCategory,int IdCategory)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"
             SELECT COUNT(*) FROM tbl_learning_assigment
             WHERE id_organization = @Id_organization
             AND id_learning_category = @IdCategory
             AND id_learning_sub_category = @IdsubCategory
             AND id_user = @userContentId
             AND status = 'A'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_organization", organizationId);
                    command.Parameters.AddWithValue("@userContentId", userContentId);
                    command.Parameters.AddWithValue("@IdsubCategory", IdsubCategory);
                    command.Parameters.AddWithValue("@IdCategory", IdCategory);

                    connection.Open();

                    // Use ExecuteScalar to get the count of matching rows
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public string QuestionTriviainsert(int organizationId, int userContentId, int IdsubCategory, int IdCategory, int userRoleId, DateTime datetime1, DateTime datetime2)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                connection.Open();
                try
                {
                    // SQL Insert query
                    string insertQuery = @"
                INSERT INTO tbl_learning_assigment (id_organization, id_learning_category, id_learning_sub_category, id_user, id_role, start_date, end_date, status)
                VALUES (@Id_organization, @IdCategory, @IdsubCategory, @userContentId, @userRoleId, @dateStart, @dateEnd, 'A')";

                    // Create a command using the query and connection
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        // Add parameters for the insert
                        insertCommand.Parameters.AddWithValue("@Id_organization", organizationId);
                        insertCommand.Parameters.AddWithValue("@IdCategory", IdCategory);
                        insertCommand.Parameters.AddWithValue("@IdsubCategory", IdsubCategory);
                        insertCommand.Parameters.AddWithValue("@userContentId", userContentId);
                        insertCommand.Parameters.AddWithValue("@userRoleId", userRoleId);
                        insertCommand.Parameters.AddWithValue("@dateStart", datetime1);
                        insertCommand.Parameters.AddWithValue("@dateEnd", datetime2);

                        // Execute the insert command
                        int result = insertCommand.ExecuteNonQuery();

                        // Return success or failure
                        if (result > 0)
                        {
                            return "1";
                        }
                        else
                        {
                            return "2";
                        }
                    }
                }
                finally
                {
                    // Close the connection explicitly
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        public tbl_learning_assigment GetlearningassigmentMapping(int cids,int scid, int orgid, int userId)
        {
            tbl_learning_assigment contentProgramMapping = null;

            string query = @"
        SELECT * 
        FROM tbl_learning_assigment
        WHERE id_learning_category = @cids 
        AND id_learning_sub_category = @scid 
        AND id_organization = @orgid 
        AND id_user = @userId
        AND status = 'A'
        LIMIT 1";  // Assuming you're using MySQL; for SQL Server, use TOP 1

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@cids", cids);
                    command.Parameters.AddWithValue("@scid", scid);
                    command.Parameters.AddWithValue("@orgid", orgid);
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contentProgramMapping = new tbl_learning_assigment
                            {
                                IdLearningCategory = Convert.ToInt32(reader["id_learning_category"] != DBNull.Value ? Convert.ToInt32(reader["id_learning_category"]) : (int?)null),
                                IdOrganization = Convert.ToInt32(reader["id_organization"] != DBNull.Value ? Convert.ToInt32(reader["id_organization"]) : (int?)null),
                                IdUser =Convert.ToString( reader["id_user"] != DBNull.Value ? Convert.ToInt32(reader["id_user"]) : (int?)null),
                                StartDate = reader["start_date"] != DBNull.Value
                ? Convert.ToDateTime(reader["start_date"]).ToString("yyyy-MM-dd") // Specify your desired format
                : string.Empty, // or "N/A" or whatever you prefer
                                EndDate = reader["end_date"] != DBNull.Value
                ? Convert.ToDateTime(reader["end_date"]).ToString("yyyy-MM-dd") // Specify your desired format
                : string.Empty,

                            };
                        }
                    }
                }
            }

            return contentProgramMapping;
        }

        public int DeletelearningassigmentMapping(int cids, int scid, int orgid, int userId)
        {
            int result = 0;

            string query = @"
        UPDATE tbl_learning_assigment
        SET status = 'D'
        WHERE id_learning_category = @cids
        AND id_learning_sub_category = @scid
        AND id_organization = @orgid
        AND id_user = @userId
        AND status = 'A'"; 

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@cids", cids);
                    command.Parameters.AddWithValue("@scid", scid);
                    command.Parameters.AddWithValue("@orgid", orgid);
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();

                    // Execute the command and get the number of rows affected
                    result = command.ExecuteNonQuery(); // Executes the query and stores the number of rows affected
                }
            }

            return result;
        }

        public string QuestionTriviaupdate(int organizationId, int userContentId, int IdsubCategory, int IdCategory, int userRoleId, DateTime datetime1, DateTime datetime2)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                connection.Open();
                try
                {
                    // SQL Insert query
                    string insertQuery = @"
               UPDATE tbl_learning_assigment 
                SET id_organization = @Id_organization, 
                    id_learning_category = @IdCategory, 
                    id_learning_sub_category = @IdsubCategory, 
                    id_user = @userContentId, 
                    id_role = @userRoleId, 
                    start_date = @dateStart, 
                    end_date = @dateEnd, 
                    status = 'A'
                WHERE id_user = @userContentId 
                  AND id_learning_category = @IdCategory 
                  AND id_learning_sub_category = @IdsubCategory";
                   
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        // Add parameters for the insert
                        insertCommand.Parameters.AddWithValue("@Id_organization", organizationId);
                        insertCommand.Parameters.AddWithValue("@IdCategory", IdCategory);
                        insertCommand.Parameters.AddWithValue("@IdsubCategory", IdsubCategory);
                        insertCommand.Parameters.AddWithValue("@userContentId", userContentId);
                        insertCommand.Parameters.AddWithValue("@userRoleId", userRoleId);
                        insertCommand.Parameters.AddWithValue("@dateStart", datetime1);
                        insertCommand.Parameters.AddWithValue("@dateEnd", datetime2);

                        // Execute the insert command
                        int result = insertCommand.ExecuteNonQuery();

                        // Return success or failure
                        if (result > 0)
                        {
                            return "1";
                        }
                        else
                        {
                            return "2";
                        }
                    }
                }
                finally
                {
                    // Close the connection explicitly
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }


    }


}