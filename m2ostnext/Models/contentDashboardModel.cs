// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.contentDashboardModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace m2ostnext.Models
{
    public class contentDashboardModel
    {
        private MySqlConnection conn;

        public contentDashboardModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

        public Login checkUser(Login login)
        {
            Login login1 = new Login();
            try
            {
                string username = login.Username;
                string password = login.Password;
                string str = "select a.*,b.ID_ORGANIZATION from tbl_cms_users a,tbl_cms_roles b where USERNAME = @value1 AND PASSWORD=@value2 AND a.id_role=b.id_role";
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)username);
                command.Parameters.AddWithValue("value2", (object)password);
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        login1.Username = mySqlDataReader["USERNAME"].ToString();
                        login1.Roleid = mySqlDataReader["ID_ROLE"].ToString();
                        login1.ID_USER = mySqlDataReader["ID_USER"].ToString();
                        login1.ID_ORG = mySqlDataReader["ID_ORGANIZATION"].ToString();
                    }
                }
                else
                    login1 = (Login)null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.conn.Close();
            }
            return login1;
        }

        public Login checkUser_SSO(Login login)
        {
            Login login1 = new Login();
            try
            {
                string username = login.Username;
                string password = login.Password;
                string str = "select a.*,b.ID_ORGANIZATION from tbl_cms_users a,tbl_cms_roles b where USERNAME = @value1 AND a.id_role=b.id_role";
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)username);

                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        login1.Username = mySqlDataReader["USERNAME"].ToString();
                        login1.Roleid = mySqlDataReader["ID_ROLE"].ToString();
                        login1.ID_USER = mySqlDataReader["ID_USER"].ToString();
                        login1.ID_ORG = mySqlDataReader["ID_ORGANIZATION"].ToString();
                    }
                }
                else
                    login1 = (Login)null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.conn.Close();
            }
            return login1;
        }

        public string getOrganisationID(string id)
        {
            string organisationId = "0";
            try
            {
                string str = "select * from tbl_cms_roles where ID_ROLE = @value1 ";
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)id);
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        organisationId = mySqlDataReader["ID_ORGANIZATION"].ToString();
                }
                else
                    organisationId = "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.conn.Close();
            }
            return organisationId;
        }

        public string add_temp_content(temp_content temp)
        {
            string str = (string)null;
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "INSERT INTO cms_temp( USERID, CONTENT_TYPE, CONTENT, ANSWER_STEP) VALUES (@value1,@value2,@value3,@value4)";
                command.Parameters.AddWithValue("value1", (object)temp.UserID);
                command.Parameters.AddWithValue("value2", (object)temp.Type);
                command.Parameters.AddWithValue("value3", (object)temp.Content);
                command.Parameters.AddWithValue("value4", (object)temp.Step);
                this.conn.Open();
                str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
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

        public string get_temp_answer_step(string id)
        {
            string tempAnswerStep = (string)null;
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from cms_temp where USERID=@value1 order by ANSWER_STEP DESC LIMIT 1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        tempAnswerStep = mySqlDataReader["ANSWER_STEP"].ToString();
                }
                else
                    tempAnswerStep = "0";
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tempAnswerStep;
        }

        public void clear_userdata(string id)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "delete from cms_temp where USERID=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
        }

        public void clear_contentdata(string id)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "delete from cms_temp where CONTENT_TYPE NOT IN(1) AND USERID=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
        }

        public tbl_content get_temp_content(string id)
        {
            tbl_content tempContent = new tbl_content();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from cms_temp where USERID=@value1 and CONTENT_TYPE=@value2";
                command.Parameters.AddWithValue("value1", (object)id);
                command.Parameters.AddWithValue("value2", (object)"1");
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        tempContent.ID_USER = Convert.ToInt32(id);
                        tempContent.STATUS = "D";
                        tempContent.CONTENT_QUESTION = mySqlDataReader["CONTENT"].ToString();
                        tempContent.CONTENT_COUNTER = 0;
                        tempContent.UPDATED_DATE_TIME = DateTime.Now;
                    }
                }
                else
                    tempContent = (tbl_content)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tempContent;
        }

        public tbl_content_answer get_temp_content_answer(string id)
        {
            tbl_content_answer tempContentAnswer = new tbl_content_answer();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from cms_temp where USERID=@value1 and CONTENT_TYPE=@value2";
                command.Parameters.AddWithValue("value1", (object)id);
                command.Parameters.AddWithValue("value2", (object)"2");
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        tempContentAnswer.STATUS = "D";
                        tempContentAnswer.CONTENT_ANSWER1 = mySqlDataReader["CONTENT"].ToString();
                        tempContentAnswer.CONTENT_ANSWER_COUNTER = 0;
                        tempContentAnswer.UPDATEDTIME = DateTime.Now;
                    }
                }
                else
                    tempContentAnswer = (tbl_content_answer)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tempContentAnswer;
        }

        public List<tbl_content_metadata> get_temp_content_metadata(string id)
        {
            List<tbl_content_metadata> tempContentMetadata = new List<tbl_content_metadata>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from cms_temp where USERID=@value1 and CONTENT_TYPE=@value2";
                command.Parameters.AddWithValue("value1", (object)id);
                command.Parameters.AddWithValue("value2", (object)"3");
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        tempContentMetadata.Add(new tbl_content_metadata()
                        {
                            STATUS = "D",
                            CONTENT_METADATA = mySqlDataReader["CONTENT"].ToString(),
                            CONTENT_METADATA_COUNTER = 0,
                            UPDATEDTIME = DateTime.Now
                        });
                }
                else
                    tempContentMetadata = (List<tbl_content_metadata>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tempContentMetadata;
        }

        public List<tbl_content_answer_steps> get_temp_content_answerstep(string id)
        {
            List<tbl_content_answer_steps> contentAnswerstep = new List<tbl_content_answer_steps>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from cms_temp where USERID=@value1 and CONTENT_TYPE=@value2 order by ANSWER_STEP ";
                command.Parameters.AddWithValue("value1", (object)id);
                command.Parameters.AddWithValue("value2", (object)"4");
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        contentAnswerstep.Add(new tbl_content_answer_steps()
                        {
                            STATUS = "D",
                            ANSWER_STEPS_PART1 = mySqlDataReader["CONTENT"].ToString(),
                            STEPNO = Convert.ToInt32(mySqlDataReader["ANSWER_STEP"]),
                            UPDATEDDATETIME = DateTime.Now
                        });
                }
                else
                    contentAnswerstep = (List<tbl_content_answer_steps>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return contentAnswerstep;
        }

        public List<tbl_content_type> get_tbl_content_type()
        {
            List<tbl_content_type> tblContentType = new List<tbl_content_type>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_type WHERE STATUS='A'";
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        tblContentType.Add(new tbl_content_type()
                        {
                            ID_CONTENT_TYPE = Convert.ToInt32(mySqlDataReader["ID_CONTENT_TYPE"]),
                            TYPE_NAME = mySqlDataReader["TYPE_NAME"].ToString(),
                            TYPE_DESCRIPTION = mySqlDataReader["TYPE_DESCRIPTION"].ToString(),
                            STATUS = "1"
                        });
                }
                else
                    tblContentType = (List<tbl_content_type>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tblContentType;
        }

        public List<string> getUserLocation(string sql)
        {
            List<string> userLocation = new List<string>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = sql;
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        string str = mySqlDataReader["LOCATION"].ToString();
                        if (!string.IsNullOrEmpty(str))
                            userLocation.Add(str);
                    }
                }
                else
                    userLocation = (List<string>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return userLocation;
        }

        public List<string> getUserFunction(string sql)
        {
            List<string> userFunction = new List<string>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = sql;
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        string str = mySqlDataReader["user_function"].ToString();
                        if (!string.IsNullOrEmpty(str))
                            userFunction.Add(str);
                    }
                }
                else
                    userFunction = (List<string>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return userFunction;
        }

        public int insert_tbl_content(tbl_content content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                string str = "INSERT INTO tbl_content(ID_USER, CONTENT_QUESTION, CONTENT_COUNTER, STATUS, UPDATED_DATE_TIME) VALUES (@value1,@value2,@value3,@value4,@value5)";
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)content.ID_USER);
                command.Parameters.AddWithValue("value2", (object)content.CONTENT_QUESTION);
                command.Parameters.AddWithValue("value3", (object)content.CONTENT_COUNTER);
                command.Parameters.AddWithValue("value4", (object)content.STATUS);
                command.Parameters.AddWithValue("value5", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? Convert.ToInt32(command.LastInsertedId) : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int insert_tbl_content_answer(tbl_content_answer content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                string str = "INSERT INTO tbl_content_answer(ID_CONTENT, CONTENT_ANSWER, CONTENT_ANSWER_COUNTER, STATUS, UPDATED_DATE_TIME) VALUES (@value1,@value2,@value3,@value4,@value5)";
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)content.ID_CONTENT);
                command.Parameters.AddWithValue("value2", (object)content.CONTENT_ANSWER1);
                command.Parameters.AddWithValue("value3", (object)content.CONTENT_ANSWER_COUNTER);
                command.Parameters.AddWithValue("value4", (object)content.STATUS);
                command.Parameters.AddWithValue("value5", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? Convert.ToInt32(command.LastInsertedId) : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int insert_tbl_content_metadata(tbl_content_metadata content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                string str = "INSERT INTO tbl_content_metadata(CONTENT_METADATA, CONTENT_METADATA_COUNTER, ID_CONTENT_ANSWER, STATUS, UPDATED_DATE_TIME) VALUES (@value1,@value2,@value3,@value4,@value5)";
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)content.CONTENT_METADATA);
                command.Parameters.AddWithValue("value2", (object)content.CONTENT_METADATA_COUNTER);
                command.Parameters.AddWithValue("value3", (object)content.ID_CONTENT_ANSWER);
                command.Parameters.AddWithValue("value4", (object)content.STATUS);
                command.Parameters.AddWithValue("value5", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? Convert.ToInt32(command.LastInsertedId) : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int insert_tbl_content_answer_steps(tbl_content_answer_steps content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                string str = "INSERT INTO tbl_content_answer_steps(ID_CONTENT_ANSWER, STEPNO, ANSWER_STEPS, STATUS, UPDATEDDATETIME) VALUES (@value1,@value2,@value3,@value4,@value5)";
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)content.ID_CONTENT_ANSWER);
                command.Parameters.AddWithValue("value2", (object)content.STEPNO);
                command.Parameters.AddWithValue("value3", (object)content.ANSWER_STEPS_PART1);
                command.Parameters.AddWithValue("value4", (object)content.STATUS);
                command.Parameters.AddWithValue("value5", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? Convert.ToInt32(command.LastInsertedId) : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int update_tbl_content(tbl_content content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "UPDATE tbl_content SET ID_USER=@value2,CONTENT_QUESTION=@value3,CONTENT_COUNTER=@value4,STATUS=@value5,UPDATED_DATE_TIME=@value6 WHERE ID_CONTENT=@value1";
                command.Parameters.AddWithValue("value1", (object)content.ID_CONTENT);
                command.Parameters.AddWithValue("value2", (object)content.ID_USER);
                command.Parameters.AddWithValue("value3", (object)content.CONTENT_QUESTION);
                command.Parameters.AddWithValue("value4", (object)content.CONTENT_COUNTER);
                command.Parameters.AddWithValue("value5", (object)content.STATUS);
                command.Parameters.AddWithValue("value6", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int update_tbl_content_answer(tbl_content_answer content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "UPDATE tbl_content_answer SET ID_CONTENT=@value2,CONTENT_ANSWER=@value3,CONTENT_ANSWER_COUNTER=@value4,STATUS=@value5,UPDATED_DATE_TIME=@value6 WHERE ID_CONTENT_ANSWER=@value1";
                command.Parameters.AddWithValue("value1", (object)content.ID_CONTENT_ANSWER);
                command.Parameters.AddWithValue("value2", (object)content.ID_CONTENT);
                command.Parameters.AddWithValue("value3", (object)content.CONTENT_ANSWER1);
                command.Parameters.AddWithValue("value4", (object)content.CONTENT_ANSWER_COUNTER);
                command.Parameters.AddWithValue("value5", (object)content.STATUS);
                command.Parameters.AddWithValue("value6", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int update_tbl_content_metadata(tbl_content_metadata content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "UPDATE tbl_content_metadata SET CONTENT_METADATA=@value2,CONTENT_METADATA_COUNTER=@value3,ID_CONTENT_ANSWER=@value4,STATUS=@value5,UPDATED_DATE_TIME=@value6 WHERE ID_CONTENT_METADATA=@value1";
                command.Parameters.AddWithValue("value1", (object)content.ID_CONTENT_METADATA);
                command.Parameters.AddWithValue("value2", (object)content.CONTENT_METADATA);
                command.Parameters.AddWithValue("value3", (object)content.CONTENT_METADATA_COUNTER);
                command.Parameters.AddWithValue("value4", (object)content.ID_CONTENT_ANSWER);
                command.Parameters.AddWithValue("value5", (object)content.STATUS);
                command.Parameters.AddWithValue("value6", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int update_tbl_answer_step(tbl_content_answer_steps content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                string str = "UPDATE tbl_content_answer_steps SET ID_CONTENT_ANSWER=@value2,STEPNO=@value3,ANSWER_STEPS=@value4,STATUS=@value5,UPDATEDDATETIME=@value6 WHERE ID_ANSWER_STEP=@value1 ";
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)content.ID_ANSWER_STEP);
                command.Parameters.AddWithValue("value2", (object)content.ID_CONTENT_ANSWER);
                command.Parameters.AddWithValue("value3", (object)content.STEPNO);
                command.Parameters.AddWithValue("value4", (object)content.ANSWER_STEPS_PART1);
                command.Parameters.AddWithValue("value5", (object)content.STATUS);
                command.Parameters.AddWithValue("value6", (object)DateTime.Now);
                this.conn.Open();
                return command.ExecuteNonQuery() == 1 ? Convert.ToInt32(command.LastInsertedId) : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int delete_tbl_content(string id)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "DELETE FROM tbl_content WHERE WHERE ID_CONTENT=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int delete_tbl_content_answer(string id)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "DELETE FROM tbl_content_answer WHERE WHERE ID_CONTENT_ANSWER=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int delete_tbl_content_metadata(string id)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "DELETE FROM tbl_content_metadata WHERE WHERE ID_CONTENT_METADATA=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int delete_tbl_content_answer_steps(string id)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "DELETE FROM tbl_content_answer_steps WHERE WHERE ID_ANSWER_STEP=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public tbl_content get_tbl_content(string id)
        {
            tbl_content tblContent = new tbl_content();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content where ID_CONTENT=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        tblContent.ID_CONTENT = Convert.ToInt32(id);
                        tblContent.ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"]);
                        tblContent.STATUS = mySqlDataReader["STATUS"].ToString();
                        tblContent.CONTENT_QUESTION = mySqlDataReader["CONTENT"].ToString();
                        tblContent.CONTENT_COUNTER = Convert.ToInt32(mySqlDataReader["CONTENT_COUNTER"]);
                        tblContent.UPDATED_DATE_TIME = DateTime.Now;
                    }
                }
                else
                    tblContent = (tbl_content)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tblContent;
        }

        public tbl_content_answer get_tbl_content_answer(string id)
        {
            tbl_content_answer tblContentAnswer = new tbl_content_answer();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_answer where ID_CONTENT_ANSWER=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        tblContentAnswer.ID_CONTENT_ANSWER = Convert.ToInt32(id);
                        tblContentAnswer.ID_CONTENT = Convert.ToInt32(mySqlDataReader["ID_CONTENT"]);
                        tblContentAnswer.CONTENT_ANSWER1 = mySqlDataReader["CONTENT_ANSWER"].ToString();
                        tblContentAnswer.CONTENT_ANSWER_COUNTER = Convert.ToInt32(mySqlDataReader["CONTENT_ANSWER_COUNTER"]);
                        tblContentAnswer.STATUS = mySqlDataReader["STATUS"].ToString();
                        tblContentAnswer.UPDATEDTIME = DateTime.Now;
                    }
                }
                else
                    tblContentAnswer = (tbl_content_answer)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tblContentAnswer;
        }

        public tbl_content_metadata get_tbl_content_metadata(string id)
        {
            tbl_content_metadata tblContentMetadata = new tbl_content_metadata();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_metadata where ID_CONTENT_METADATA=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        tblContentMetadata.ID_CONTENT_METADATA = Convert.ToInt32(mySqlDataReader["ID_CONTENT_METADATA"]);
                        tblContentMetadata.ID_CONTENT_ANSWER = Convert.ToInt32(mySqlDataReader["ID_CONTENT_ANSWER"]);
                        tblContentMetadata.CONTENT_METADATA = mySqlDataReader["CONTENT_METADATA"].ToString();
                        tblContentMetadata.CONTENT_METADATA_COUNTER = Convert.ToInt32(mySqlDataReader["CONTENT_METADATA_COUNTER"]);
                        tblContentMetadata.STATUS = mySqlDataReader["STATUS"].ToString();
                        tblContentMetadata.UPDATEDTIME = DateTime.Now;
                    }
                }
                else
                    tblContentMetadata = (tbl_content_metadata)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tblContentMetadata;
        }

        public tbl_content_answer_steps get_tbl_answer_step(string id)
        {
            tbl_content_answer_steps tblAnswerStep = new tbl_content_answer_steps();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_answer_steps where ID_ANSWER_STEP=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        tblAnswerStep.ID_ANSWER_STEP = Convert.ToInt32(mySqlDataReader["ID_ANSWER_STEP"]);
                        tblAnswerStep.ID_CONTENT_ANSWER = Convert.ToInt32(mySqlDataReader["ID_CONTENT_ANSWER"]);
                        tblAnswerStep.ANSWER_STEPS_PART1 = mySqlDataReader["ANSWER_STEPS"].ToString();
                        tblAnswerStep.STEPNO = Convert.ToInt32(mySqlDataReader["STEPNO"]);
                        tblAnswerStep.STATUS = mySqlDataReader["STATUS"].ToString();
                    }
                }
                else
                    tblAnswerStep = (tbl_content_answer_steps)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tblAnswerStep;
        }

        public List<tbl_content_answer_steps> get_tbl_content_answer_steps(int id)
        {
            List<tbl_content_answer_steps> contentAnswerSteps = new List<tbl_content_answer_steps>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_answer_steps where ID_CONTENT_ANSWER=@value1 ORDER BY STEPNO";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        contentAnswerSteps.Add(new tbl_content_answer_steps()
                        {
                            ID_ANSWER_STEP = Convert.ToInt32(mySqlDataReader["ID_ANSWER_STEP"]),
                            ID_CONTENT_ANSWER = Convert.ToInt32(mySqlDataReader["ID_CONTENT_ANSWER"]),
                            ANSWER_STEPS_PART1 = mySqlDataReader["ANSWER_STEPS"].ToString(),
                            STEPNO = Convert.ToInt32(mySqlDataReader["STEPNO"]),
                            STATUS = mySqlDataReader["STATUS"].ToString()
                        });
                }
                else
                    contentAnswerSteps = (List<tbl_content_answer_steps>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return contentAnswerSteps;
        }

        public tbl_content_answer get_tbl_content_answer_by_id_content(int id)
        {
            tbl_content_answer answerByIdContent = new tbl_content_answer();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_answer where ID_CONTENT=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        answerByIdContent.ID_CONTENT_ANSWER = Convert.ToInt32(mySqlDataReader["ID_CONTENT_ANSWER"]);
                        answerByIdContent.ID_CONTENT = Convert.ToInt32(mySqlDataReader["ID_CONTENT"]);
                        answerByIdContent.CONTENT_ANSWER1 = mySqlDataReader["CONTENT_ANSWER"].ToString();
                        answerByIdContent.CONTENT_ANSWER_COUNTER = Convert.ToInt32(mySqlDataReader["CONTENT_ANSWER_COUNTER"]);
                        answerByIdContent.STATUS = mySqlDataReader["STATUS"].ToString();
                        answerByIdContent.UPDATEDTIME = DateTime.Now;
                    }
                }
                else
                    answerByIdContent = (tbl_content_answer)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return answerByIdContent;
        }

        public List<tbl_content_metadata> get_tbl_content_metadata_by_id_answer(int id)
        {
            List<tbl_content_metadata> metadataByIdAnswer = new List<tbl_content_metadata>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_metadata where ID_CONTENT_ANSWER=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        metadataByIdAnswer.Add(new tbl_content_metadata()
                        {
                            ID_CONTENT_METADATA = Convert.ToInt32(mySqlDataReader["ID_CONTENT_METADATA"]),
                            ID_CONTENT_ANSWER = Convert.ToInt32(mySqlDataReader["ID_CONTENT_ANSWER"]),
                            STATUS = mySqlDataReader["STATUS"].ToString(),
                            CONTENT_METADATA = mySqlDataReader["CONTENT_METADATA"].ToString(),
                            CONTENT_METADATA_COUNTER = Convert.ToInt32(mySqlDataReader["CONTENT_METADATA_COUNTER"]),
                            UPDATEDTIME = DateTime.Now
                        });
                }
                else
                    metadataByIdAnswer = (List<tbl_content_metadata>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return metadataByIdAnswer;
        }

        public int approveContent(string id, string aid, string res)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "UPDATE tbl_content SET STATUS='A',COMMENT=@value3 WHERE ID_CONTENT=@value1;UPDATE tbl_content_answer SET STATUS='A' WHERE ID_CONTENT_ANSWER=@value2;UPDATE tbl_content_metadata SET STATUS='A' WHERE ID_CONTENT_ANSWER=@value2;UPDATE tbl_content_answer_steps SET STATUS='A' WHERE ID_CONTENT_ANSWER=@value2;";
                command.Parameters.AddWithValue("value1", (object)id);
                command.Parameters.AddWithValue("value2", (object)aid);
                command.Parameters.AddWithValue("value3", (object)res);
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int approveContent(string id)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "UPDATE tbl_content SET STATUS='A' WHERE ID_CONTENT=@value1;UPDATE tbl_content_answer SET STATUS='A' WHERE ID_CONTENT=@value1;UPDATE tbl_content_metadata SET STATUS='A' WHERE ID_CONTENT_ANSWER IN (SELECT ID_CONTENT_ANSWER from tbl_content_answer WHERE ID_CONTENT=@value1);UPDATE tbl_content_answer_steps SET STATUS='A' WHERE ID_CONTENT_ANSWER IN (SELECT ID_CONTENT_ANSWER from tbl_content_answer WHERE ID_CONTENT=@value1)";
                command.Parameters.AddWithValue("value1", (object)id);
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int RejectContent(string id, string aid, string resp)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "UPDATE tbl_content SET STATUS='D' , COMMENT=@value3 WHERE ID_CONTENT=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                command.Parameters.AddWithValue("value3", (object)resp);
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? 1 : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public int edit_tbl_answer_step(tbl_content_answer_steps content)
        {
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                if (content.ID_ANSWER_STEP == 0)
                {
                    string str = "INSERT INTO tbl_content_answer_steps(ID_CONTENT_ANSWER, STEPNO, ANSWER_STEPS, STATUS, UPDATEDDATETIME) VALUES (@value1,@value2,@value3,@value4,@value5)";
                    command.CommandText = str;
                    command.Parameters.AddWithValue("value1", (object)content.ID_CONTENT_ANSWER);
                    command.Parameters.AddWithValue("value2", (object)content.STEPNO);
                    command.Parameters.AddWithValue("value3", (object)content.ANSWER_STEPS_PART1);
                    command.Parameters.AddWithValue("value4", (object)content.STATUS);
                    command.Parameters.AddWithValue("value5", (object)DateTime.Now);
                }
                else
                {
                    string str = "UPDATE tbl_content_answer_steps SET ID_CONTENT_ANSWER=@value2,STEPNO=@value3,ANSWER_STEPS=@value4,STATUS=@value5,UPDATEDDATETIME=@value6 WHERE ID_ANSWER_STEP=@value1 ";
                    command.CommandText = str;
                    command.Parameters.AddWithValue("value1", (object)content.ID_ANSWER_STEP);
                    command.Parameters.AddWithValue("value2", (object)content.ID_CONTENT_ANSWER);
                    command.Parameters.AddWithValue("value3", (object)content.STEPNO);
                    command.Parameters.AddWithValue("value4", (object)content.ANSWER_STEPS_PART1);
                    command.Parameters.AddWithValue("value5", (object)content.STATUS);
                    command.Parameters.AddWithValue("value6", (object)DateTime.Now);
                }
                this.conn.Open();
                return command.ExecuteNonQuery() > 0 ? Convert.ToInt32(command.LastInsertedId) : 0;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return 0;
        }

        public void exception_log(Exception ex)
        {
            string str = "INSERT INTO error_log(Error_Message, Error_Inner,STATUS,UPDATEDDATETIME) VALUES (@value2,@value3,@value4,@value5)";
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            command.Parameters.AddWithValue("value2", (object)ex.Message);
            command.Parameters.AddWithValue("value3", (object)ex.InnerException);
            command.Parameters.AddWithValue("value4", (object)"A");
            command.Parameters.AddWithValue("value5", (object)DateTime.Now);
            this.conn.Open();
            command.ExecuteNonQuery();
        }

        public string getOrgLogo(int oid)
        {
            try
            {
                string str1 = "";
                this.conn.Open();
                string str2 = "SELECT LOGO FROM tbl_organization WHERE id_organization = @value1";
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str2;
                command.Parameters.AddWithValue("value1", (object)oid);
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                while (mySqlDataReader.Read())
                    str1 = mySqlDataReader["LOGO"].ToString();
                mySqlDataReader.Close();
                return !(str1 == "") ? ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "ORGLOGO/" + str1 : ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "default.png";
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

        public string getOrgBanner(int oid)
        {
            try
            {
                string str1 = "";
                this.conn.Open();
                string str2 = "SELECT Banner_path FROM tbl_organisation_banner WHERE id_organisation = @value1";
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str2;
                command.Parameters.AddWithValue("value1", (object)oid);
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                while (mySqlDataReader.Read())
                    str1 = mySqlDataReader["Banner_path"].ToString();
                mySqlDataReader.Close();
                return !(str1 == "") ? ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BANNERIMG/" + str1 : ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "default.png";
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

        public List<tbl_content_organization_mapping> get_tbl_content_cat(string id, string oid)
        {
            List<tbl_content_organization_mapping> tblContentCat = new List<tbl_content_organization_mapping>();
            try
            {
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = "select * from tbl_content_organization_mapping where id_category=@value1";
                command.Parameters.AddWithValue("value1", (object)id);
                command.Parameters.AddWithValue("value2", (object)oid);
                this.conn.Open();
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        tblContentCat.Add(new tbl_content_organization_mapping()
                        {
                            id_content = Convert.ToInt32(mySqlDataReader["id_content"])
                        });
                }
                else
                    tblContentCat = (List<tbl_content_organization_mapping>)null;
            }
            catch
            {
            }
            finally
            {
                this.conn.Close();
            }
            return tblContentCat;
        }

        public List<tbl_content> getcon(List<tbl_content_organization_mapping> con)
        {
            List<tbl_content> tblContentList = new List<tbl_content>();
            foreach (tbl_content_organization_mapping organizationMapping in con)
            {
                try
                {
                    MySqlCommand command = this.conn.CreateCommand();
                    command.CommandText = "select * from tbl_content where ID_CONTENT=@value1";
                    command.Parameters.AddWithValue("value1", (object)organizationMapping.id_content);
                    this.conn.Open();
                    MySqlDataReader mySqlDataReader = command.ExecuteReader();
                    if (mySqlDataReader.HasRows)
                    {
                        while (mySqlDataReader.Read())
                            tblContentList.Add(new tbl_content()
                            {
                                ID_CONTENT = Convert.ToInt32(organizationMapping.id_content),
                                ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"]),
                                STATUS = mySqlDataReader["STATUS"].ToString(),
                                CONTENT_QUESTION = mySqlDataReader["CONTENT_QUESTION"].ToString(),
                                CONTENT_COUNTER = Convert.ToInt32(mySqlDataReader["CONTENT_COUNTER"]),
                                UPDATED_DATE_TIME = DateTime.Now
                            });
                    }
                    else
                        tblContentList = (List<tbl_content>)null;
                }
                catch
                {
                }
                finally
                {
                    this.conn.Close();
                }
            }
            return tblContentList;
        }
    }
}
