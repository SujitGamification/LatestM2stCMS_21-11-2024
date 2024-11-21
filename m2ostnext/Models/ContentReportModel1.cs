// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ContentReportModel1
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace m2ostnext.Models
{
  public class ContentReportModel1
  {
    private MySqlConnection conn;

    public ContentReportModel1() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

    public List<tbl_user> get_user_organization(string org_id)
    {
      List<tbl_user> userOrganization = new List<tbl_user>();
      try
      {
        string str = "select b.USERID,b.ID_USER from tbl_organization a,tbl_user b,tbl_role c " + " where a.ID_ORGANIZATION = @value1  AND b.ID_ROLE = c.ID_ROLE and c.ID_ORGANIZATION = a.ID_ORGANIZATION and b.STATUS ='A' ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("@value1", (object) org_id);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        while (mySqlDataReader.Read())
          userOrganization.Add(new tbl_user()
          {
            ID_USER = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("ID_USER")),
            USERID = mySqlDataReader["USERID"].ToString()
          });
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
      }
      finally
      {
        this.conn.Close();
        this.conn = (MySqlConnection) null;
      }
      return userOrganization;
    }

    public List<ContentReport> getContentReportfilterlist(string query)
    {
      List<ContentReport> reportfilterlist = new List<ContentReport>();
      try
      {
        this.conn.Open();
        MySqlDataReader mySqlDataReader = new MySqlCommand(query, this.conn).ExecuteReader();
        while (mySqlDataReader.Read())
          reportfilterlist.Add(new ContentReport()
          {
            ID_USER = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("ID_USER")),
            USERID = mySqlDataReader["USERID"].ToString(),
            content_name = mySqlDataReader["CONTENT_QUESTION"].ToString(),
            orgnization_name = mySqlDataReader["ORGANIZATION_NAME"].ToString(),
            created_dated = Convert.ToDateTime(mySqlDataReader["UPDATED_DATE_TIME"].ToString()),
            expity_date = Convert.ToDateTime(mySqlDataReader["EXPIRY_DATE"].ToString()),
            count_accessed = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("CONTENT_COUNTER"))
          });
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
        this.conn = (MySqlConnection) null;
      }
      return reportfilterlist;
    }

    public List<ContentReport> getContentOptionfilterlist(string query)
    {
      List<ContentReport> optionfilterlist = new List<ContentReport>();
      try
      {
        this.conn.Open();
        MySqlDataReader mySqlDataReader = new MySqlCommand(query, this.conn).ExecuteReader();
        while (mySqlDataReader.Read())
          optionfilterlist.Add(new ContentReport()
          {
            ID_USER = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("ID_USER")),
            USERID = mySqlDataReader["USERID"].ToString(),
            content_name = mySqlDataReader["CONTENT_QUESTION"].ToString(),
            created_dated = Convert.ToDateTime(mySqlDataReader["UPDATED_DATE_TIME"].ToString()),
            expity_date = Convert.ToDateTime(mySqlDataReader["EXPIRY_DATE"].ToString()),
            countflag = Convert.ToInt32(mySqlDataReader["count"].ToString())
          });
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
        this.conn = (MySqlConnection) null;
      }
      return optionfilterlist;
    }

    public List<ContentReport> getContentLoc(string query)
    {
      List<ContentReport> contentLoc = new List<ContentReport>();
      try
      {
        this.conn.Open();
        MySqlDataReader mySqlDataReader = new MySqlCommand(query, this.conn).ExecuteReader();
        while (mySqlDataReader.Read())
          contentLoc.Add(new ContentReport()
          {
            location = mySqlDataReader["LOCATION"].ToString(),
            username = mySqlDataReader["FIRSTNAME"].ToString(),
            ID_USER = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("ID_USER"))
          });
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
        this.conn = (MySqlConnection) null;
      }
      return contentLoc;
    }

    public List<usersdetails> getContentTopUser(string query)
    {
      List<usersdetails> contentTopUser = new List<usersdetails>();
      try
      {
        this.conn.Open();
        MySqlDataReader mySqlDataReader = new MySqlCommand(query, this.conn).ExecuteReader();
        while (mySqlDataReader.Read())
          contentTopUser.Add(new usersdetails()
          {
            ID_USER = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("ID_USER")),
            count = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("Lcount"))
          });
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.conn.Close();
        this.conn = (MySqlConnection) null;
      }
      return contentTopUser;
    }
  }
}
