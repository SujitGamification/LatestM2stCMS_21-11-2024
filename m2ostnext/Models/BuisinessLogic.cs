// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.BuisinessLogic
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace m2ostnext.Models
{
  public class BuisinessLogic
  {
    private MySqlConnection conn;

    public BuisinessLogic() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

    public tbl_non_disclousure_clause_content getContent(int oid)
    {
      tbl_non_disclousure_clause_content content = new tbl_non_disclousure_clause_content();
      try
      {
        string str = "select * from tbl_non_disclousure_clause_content where id_org = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) oid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            content.content = mySqlDataReader["content"].ToString();
            content.status = mySqlDataReader["status"].ToString();
            content.content_title = mySqlDataReader["content_title"].ToString();
            content.id_clause_content = Convert.ToInt32(mySqlDataReader["id_clause_content"].ToString());
            content.id_creator = Convert.ToInt32(mySqlDataReader["id_creator"].ToString());
            content.id_org = Convert.ToInt32(mySqlDataReader["id_org"].ToString());
            content.updated_date_time = Convert.ToDateTime(mySqlDataReader["updated_date_time"].ToString());
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
      }
      return content;
    }

    public void UpdateContent(tbl_non_disclousure_clause_content content)
    {
      this.conn.CreateCommand();
      menu menu = new menu();
      MySqlCommand command = this.conn.CreateCommand();
      this.conn.Open();
      command.CommandText = "update  tbl_non_disclousure_clause_content set content_title=@value1, content=@value2, updated_date_time=@value3 , id_creator=@value4, status=@value6 where id_org=@value5";
      command.Parameters.AddWithValue("value1", (object) content.content_title);
      command.Parameters.AddWithValue("value2", (object) content.content);
      command.Parameters.AddWithValue("value3", (object) content.updated_date_time);
      command.Parameters.AddWithValue("value4", (object) content.id_creator);
      command.Parameters.AddWithValue("value5", (object) content.id_org);
      command.Parameters.AddWithValue("value6", (object) "A");
      command.ExecuteNonQuery();
      this.conn.Close();
    }

    public void AddContent(tbl_non_disclousure_clause_content content)
    {
      try
      {
        this.conn.CreateCommand();
        menu menu = new menu();
        MySqlCommand command = this.conn.CreateCommand();
        this.conn.Open();
        command.CommandText = "INSERT INTO tbl_non_disclousure_clause_content(id_org, content,content_title,id_creator,updated_date_time,status) VALUES (@value1,@value2,@value3,@value5,@value6,@value7)";
        command.Parameters.AddWithValue("value1", (object) content.id_org);
        command.Parameters.AddWithValue("value2", (object) content.content);
        command.Parameters.AddWithValue("value3", (object) content.content_title);
        command.Parameters.AddWithValue("value5", (object) content.id_creator);
        command.Parameters.AddWithValue("value6", (object) content.updated_date_time);
        command.Parameters.AddWithValue("value7", (object) "A");
        command.ExecuteNonQuery();
        this.conn.Close();
      }
      catch (Exception ex)
      {
      }
    }

    public void ActivateNonDisclosureContent(int oid, int uid, string status)
    {
      this.conn.CreateCommand();
      menu menu = new menu();
      MySqlCommand command = this.conn.CreateCommand();
      this.conn.Open();
      command.CommandText = "update  tbl_non_disclousure_clause_content set  updated_date_time=@value3 , id_creator=@value4, status=@value6 where id_org=@value5";
      command.Parameters.AddWithValue("value3", (object) DateTime.Now);
      command.Parameters.AddWithValue("value4", (object) uid);
      command.Parameters.AddWithValue("value5", (object) oid);
      command.Parameters.AddWithValue("value6", (object) status);
      command.ExecuteNonQuery();
      this.conn.Close();
    }

    public tbl_notification_pre_config getPreConfigNot(string sqlstr)
    {
      tbl_notification_pre_config preConfigNot = new tbl_notification_pre_config();
      try
      {
        string str = sqlstr;
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            preConfigNot.end_date = new DateTime?(Convert.ToDateTime(mySqlDataReader["end_date"].ToString()));
            preConfigNot.id_content = new int?(Convert.ToInt32(mySqlDataReader["id_content"].ToString()));
            preConfigNot.id_category = new int?(Convert.ToInt32(mySqlDataReader["id_category"].ToString()));
            preConfigNot.id_creater = new int?(Convert.ToInt32(mySqlDataReader["id_creater"].ToString()));
            preConfigNot.id_notification = new int?(Convert.ToInt32(mySqlDataReader["id_notification"].ToString()));
            preConfigNot.notification_action_type = mySqlDataReader["notification_action_type"].ToString();
            preConfigNot.notification_key = mySqlDataReader["notification_key"].ToString();
            preConfigNot.read_timestamp = new DateTime?(Convert.ToDateTime(mySqlDataReader["read_timestamp"].ToString()));
            preConfigNot.start_date = new DateTime?(Convert.ToDateTime(mySqlDataReader["start_date"].ToString()));
            preConfigNot.status = mySqlDataReader["status"].ToString();
            preConfigNot.updated_date_time = new DateTime?(Convert.ToDateTime(mySqlDataReader["updated_date_time"].ToString()));
            preConfigNot.user_type = mySqlDataReader["user_type"].ToString();
            preConfigNot.id_assessment = new int?(Convert.ToInt32(mySqlDataReader["id_assessment"].ToString()));
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
      }
      return preConfigNot;
    }

    public List<int> getUsersOfOrg(int oid)
    {
      List<int> usersOfOrg = new List<int>();
      try
      {
        string str = "select * from tbl_user where ID_ORGANIZATION=" + (object) oid;
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            int int32 = Convert.ToInt32(mySqlDataReader["ID_USER"].ToString());
            usersOfOrg.Add(int32);
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
      }
      return usersOfOrg;
    }

    public tbl_assessment getAssName(string sqlstr)
    {
      tbl_assessment assName = new tbl_assessment();
      try
      {
        string str = sqlstr;
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            assName.id_assessment = Convert.ToInt32(mySqlDataReader["id_assessment"].ToString());
            assName.assessment_title = mySqlDataReader["assessment_title"].ToString();
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
      }
      return assName;
    }
  }
}
