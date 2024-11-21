// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ContentReportModel2
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace m2ostnext.Models
{
  public class ContentReportModel2
  {
    private MySqlConnection conn;

    public ContentReportModel2() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

    public List<ContentLike> getContentLikes(string str)
    {
      List<ContentLike> contentLikes = new List<ContentLike>();
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        while (mySqlDataReader.Read())
          contentLikes.Add(new ContentLike()
          {
            ID_CONTENT = Convert.ToInt32(mySqlDataReader["id_content"].ToString()),
            LIKES = Convert.ToInt32(mySqlDataReader["LikeCount"].ToString()),
            DISLIKES = Convert.ToInt32(mySqlDataReader["DisLikeCount"].ToString()),
            CONTENTACCESS = Convert.ToInt32(mySqlDataReader["COUNTER"].ToString()),
            ENDDATE = mySqlDataReader["EXPIRY_DATE"].ToString(),
            LASTACCESS = mySqlDataReader["LASTACCESS"].ToString(),
            CONTENT = mySqlDataReader["CONTENT_QUESTION"].ToString()
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
      return contentLikes;
    }

    public List<string> getLocationList(int oid, string lAdd)
    {
      List<string> locationList = new List<string>();
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = "select Distinct LOCATION from tbl_profile where id_user in (select id_user from tbl_role_user_mapping where id_organization=" + (object) oid + lAdd + ")";
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        while (mySqlDataReader.Read())
        {
          string str = mySqlDataReader["LOCATION"].ToString();
          if (!string.IsNullOrEmpty(str))
            locationList.Add(str);
        }
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
      return locationList;
    }

    public List<ContentLike> getContentAccess(string str)
    {
      List<ContentLike> contentAccess = new List<ContentLike>();
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        while (mySqlDataReader.Read())
          contentAccess.Add(new ContentLike()
          {
            ID_CONTENT = Convert.ToInt32(mySqlDataReader["id_content"].ToString()),
            LIKES = 0,
            DISLIKES = 0,
            CONTENTACCESS = Convert.ToInt32(mySqlDataReader["COUNTER"].ToString()),
            ENDDATE = mySqlDataReader["EXPIRY_DATE"].ToString(),
            LASTACCESS = mySqlDataReader["LASTACCESS"].ToString(),
            CONTENT = mySqlDataReader["CONTENT_QUESTION"].ToString()
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
      return contentAccess;
    }

    public MonthData getContentCount(string str)
    {
      MonthData contentCount = new MonthData();
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        while (mySqlDataReader.Read())
        {
          contentCount.LIKES = Convert.ToInt32(mySqlDataReader["LIKES"].ToString());
          contentCount.DISLIKES = Convert.ToInt32(mySqlDataReader["DISLIKES"].ToString());
        }
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
      return contentCount;
    }

    public List<ContentLocationWise> getLocationWiseContentAccess(string str)
    {
      List<ContentLocationWise> wiseContentAccess = new List<ContentLocationWise>();
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        while (mySqlDataReader.Read())
          wiseContentAccess.Add(new ContentLocationWise()
          {
            ID_USER = Convert.ToInt32(mySqlDataReader["id_user"].ToString()),
            CONTENTACCESS = Convert.ToInt32(mySqlDataReader["COUNTER"].ToString()),
            USERID = mySqlDataReader["USERID"].ToString(),
            FIRSTNAME = mySqlDataReader["FIRSTNAME"].ToString(),
            LASTNAME = mySqlDataReader["LASTNAME"].ToString(),
            LOCATION = mySqlDataReader["LOCATION"].ToString().ToUpper()
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
      return wiseContentAccess;
    }
  }
}
