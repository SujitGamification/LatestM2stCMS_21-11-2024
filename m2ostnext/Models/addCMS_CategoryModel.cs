// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.addCMS_CategoryModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace m2ostnext.Models
{
  public class addCMS_CategoryModel
  {
    private MySqlConnection conn;

    public addCMS_CategoryModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

    public string add_cms_category(tbl_category temp)
    {
      string str = (string) null;
      try
      {
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = "INSERT INTO tbl_category(CATEGORYNAME, DESCRIPTION, IMAGE_PATH, STATUS, UPDATED_DATE_TIME,ID_ORGANIZATION) VALUES (@value1,@value2,@value3,@value4,@value5,@value6)";
        command.Parameters.AddWithValue("value1", (object) temp.CATEGORYNAME);
        command.Parameters.AddWithValue("value2", (object) temp.DESCRIPTION);
        command.Parameters.AddWithValue("value3", (object) temp.IMAGE_PATH);
        command.Parameters.AddWithValue("value4", (object) temp.STATUS);
        command.Parameters.AddWithValue("value5", (object) DateTime.Now);
        command.Parameters.AddWithValue("value6", (object) temp.ID_ORGANIZATION);
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

    public string edit_cms_category(tbl_category temp)
    {
      string str = (string) null;
      try
      {
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = "UPDATE tbl_category SET ID_ORGANIZATION=@value7,CATEGORYNAME=@value2,DESCRIPTION=@value3,IMAGE_PATH=@value4,STATUS=@value5,UPDATED_DATE_TIME=@value6 WHERE ID_CATEGORY=@value1";
        command.Parameters.AddWithValue("value1", (object) temp.ID_CATEGORY);
        command.Parameters.AddWithValue("value2", (object) temp.CATEGORYNAME);
        command.Parameters.AddWithValue("value3", (object) temp.DESCRIPTION);
        command.Parameters.AddWithValue("value4", (object) temp.IMAGE_PATH);
        command.Parameters.AddWithValue("value5", (object) temp.STATUS);
        command.Parameters.AddWithValue("value6", (object) DateTime.Now);
        command.Parameters.AddWithValue("value7", (object) temp.ID_ORGANIZATION);
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

    public string delete_cms_category(string temp)
    {
      string str = (string) null;
      try
      {
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = "update tbl_category set status='X' WHERE ID_CATEGORY=@value1";
        command.Parameters.AddWithValue("value1", (object) temp);
        this.conn.Open();
        str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
      }
      catch
      {
        str = "TRUE";
      }
      finally
      {
        this.conn.Close();
      }
      return str;
    }

    public void updat_content(ContentRichText obContentRichText, int orgid, int userid)
    {
      try
      {
        DateTime now = DateTime.Now;
        string str1 = "update  tbl_content SET STATUS=@value11, ID_THEME=@value10,ID_USER=@value8,ID_CONTENT_LEVEL=@value1,CONTENT_HEADER=@value4,CONTENT_QUESTION=@value5,CONTENT_COUNTER=0,UPDATED_DATE_TIME=@value12,EXPIRY_DATE=@value2,CONTENT_OWNER=@value9,CONTENT_TITLE=@value3 where ID_CONTENT=@value13";
        ContentRichText contentRichText = new ContentRichText();
        this.conn.Open();
        MySqlCommand command1 = this.conn.CreateCommand();
        command1.CommandText = str1;
        command1.Parameters.AddWithValue("value0", (object) obContentRichText.select_category);
        command1.Parameters.AddWithValue("value1", (object) obContentRichText.select_level);
        command1.Parameters.AddWithValue("value2", (object) obContentRichText.content_expiry);
        command1.Parameters.AddWithValue("value3", (object) obContentRichText.t_title);
        command1.Parameters.AddWithValue("value4", (object) obContentRichText.t_header);
        command1.Parameters.AddWithValue("value5", (object) obContentRichText.t_question);
        command1.Parameters.AddWithValue("value6", (object) obContentRichText.content_answer);
        command1.Parameters.AddWithValue("value7", (object) obContentRichText.t_metadata);
        command1.Parameters.AddWithValue("value8", (object) userid);
        command1.Parameters.AddWithValue("value9", (object) orgid);
        command1.Parameters.AddWithValue("value10", (object) 15);
        command1.Parameters.AddWithValue("value11", (object) 'A');
        command1.Parameters.AddWithValue("value12", (object) now);
        command1.Parameters.AddWithValue("value13", (object) obContentRichText.conId);
        command1.ExecuteNonQuery();
        int int32 = Convert.ToInt32(command1.LastInsertedId);
        this.conn.Close();
        string str2 = "select CONTENT_HEADER from tbl_content where ID_CONTENT=@value0";
        string str3 = "";
        this.conn.Open();
        MySqlCommand command2 = this.conn.CreateCommand();
        command2.CommandText = str2;
        command2.Parameters.AddWithValue("value0", (object) obContentRichText.conId);
        MySqlDataReader mySqlDataReader = command2.ExecuteReader();
        while (mySqlDataReader.Read())
          str3 = mySqlDataReader["CONTENT_HEADER"].ToString();
        this.conn.Close();
        string str4 = "update  tbl_content_answer set STATUS=@value4,CONTENT_ANSWER_HEADER=@value1,CONTENT_ANSWER1=@value2,UPDATEDTIME=@value6,CONTENT_ANSWER_COUNTER=@value5 where ID_CONTENT=@value0";
        this.conn.Open();
        MySqlCommand command3 = this.conn.CreateCommand();
        command3.CommandText = str4;
        command3.Parameters.AddWithValue("value0", (object) obContentRichText.conId);
        command3.Parameters.AddWithValue("value1", (object) str3);
        command3.Parameters.AddWithValue("value2", (object) obContentRichText.content_answer);
        command3.Parameters.AddWithValue("value3", (object) obContentRichText.content_expiry);
        command3.Parameters.AddWithValue("value4", (object) 'A');
        command3.Parameters.AddWithValue("value5", (object) 0);
        command3.Parameters.AddWithValue("value6", (object) now);
        command3.ExecuteNonQuery();
        this.conn.Close();
        string str5 = "update  tbl_content_organization_mapping set id_organization=@value0,id_category=@value1,status=@value3,updated_date_time=@value4 where id_content=@value2";
        this.conn.Open();
        MySqlCommand command4 = this.conn.CreateCommand();
        command4.CommandText = str5;
        command4.Parameters.AddWithValue("value0", (object) orgid);
        command4.Parameters.AddWithValue("value1", (object) obContentRichText.select_category);
        command4.Parameters.AddWithValue("value2", (object) int32);
        command4.Parameters.AddWithValue("value3", (object) 'A');
        command4.Parameters.AddWithValue("value4", (object) now);
        command4.ExecuteNonQuery();
        this.conn.Close();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void insert_menu(List<tbl_menu> men, int oid)
    {
      try
      {
        this.conn.Open();
        foreach (tbl_menu man in men)
        {
          if (man.menu_name != null)
          {
            MySqlCommand command = this.conn.CreateCommand();
            menu menu = new menu();
            command.CommandText = "INSERT INTO tbl_menu(id_org, menu_name, menu_url, menu_icon, updated_date_time) VALUES (@value4,@value1,@value2,@value3,@value5)";
            command.Parameters.AddWithValue("value1", (object) man.menu_name);
            command.Parameters.AddWithValue("value2", (object) man.menu_url);
            command.Parameters.AddWithValue("value3", (object) man.menu_icon);
            command.Parameters.AddWithValue("value4", (object) oid);
            command.Parameters.AddWithValue("value5", (object) DateTime.Now);
            command.ExecuteNonQuery();
          }
        }
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void insert_menu_type(int oid, int type)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        menu menu = new menu();
        command.CommandText = "INSERT INTO tbl_menu_type(id_organisation, menu_type) VALUES (@value1,@value2)";
        command.Parameters.AddWithValue("value1", (object) oid);
        command.Parameters.AddWithValue("value2", (object) type);
        command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void insert_color_config(List<ColorConfig> color)
    {
      try
      {
        this.conn.CreateCommand();
        menu menu = new menu();
        foreach (ColorConfig colorConfig in color)
        {
          MySqlCommand command = this.conn.CreateCommand();
          this.conn.Open();
          command.CommandText = "INSERT INTO tbl_color_config(id_organisation, config_type,grid1_bk_color,grid1_text_color,grid2_bk_color,grid2_text_color,status,created_date_time,updated_date_time) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8,@value9)";
          command.Parameters.AddWithValue("value1", (object) colorConfig.id_organisation);
          command.Parameters.AddWithValue("value2", (object) colorConfig.config_type);
          command.Parameters.AddWithValue("value3", (object) colorConfig.grid1_bk_color);
          command.Parameters.AddWithValue("value4", (object) colorConfig.grid1_text_color);
          command.Parameters.AddWithValue("value5", (object) colorConfig.grid2_bk_color);
          command.Parameters.AddWithValue("value6", (object) colorConfig.grid2_text_color);
          command.Parameters.AddWithValue("value7", (object) "A");
          command.Parameters.AddWithValue("value8", (object) DateTime.Now);
          command.Parameters.AddWithValue("value9", (object) DateTime.Now);
          command.ExecuteNonQuery();
          this.conn.Close();
        }
      }
      catch (Exception ex)
      {
      }
    }

    public void insert_color_config_flag(int oid, int flag)
    {
      try
      {
        this.conn.CreateCommand();
        menu menu = new menu();
        MySqlCommand command = this.conn.CreateCommand();
        this.conn.Open();
        command.CommandText = "INSERT INTO tbl_color_config_flag(id_org, flag) VALUES (@value1,@value2)";
        command.Parameters.AddWithValue("value1", (object) oid);
        command.Parameters.AddWithValue("value2", (object) flag);
        command.ExecuteNonQuery();
        this.conn.Close();
      }
      catch (Exception ex)
      {
      }
    }

    public List<ColorConfig> get_color_config(int id)
    {
      List<ColorConfig> colorConfig = new List<ColorConfig>();
      try
      {
        string str = "select * from tbl_color_config where id_organisation = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) id);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            colorConfig.Add(new ColorConfig()
            {
              id_color_config = Convert.ToInt32(mySqlDataReader["id_color_config"].ToString()),
              id_organisation = Convert.ToInt32(mySqlDataReader["id_organisation"].ToString()),
              config_type = Convert.ToInt32(mySqlDataReader["config_type"].ToString()),
              grid1_bk_color = mySqlDataReader["grid1_bk_color"].ToString(),
              grid1_text_color = mySqlDataReader["grid1_text_color"].ToString(),
              grid2_bk_color = mySqlDataReader["grid2_bk_color"].ToString(),
              grid2_text_color = mySqlDataReader["grid2_text_color"].ToString()
            });
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
      return colorConfig;
    }

    public void insert_welcomegif(WelcomeGif gif)
    {
      try
      {
        this.conn.CreateCommand();
        menu menu = new menu();
        MySqlCommand command = this.conn.CreateCommand();
        this.conn.Open();
        command.CommandText = "INSERT INTO tbl_welcome_gif(id_org, gif,status,created_date,updated_date,flag) VALUES (@value1,@value2,@value3,@value4,@value5,@value6)";
        command.Parameters.AddWithValue("value1", (object) gif.id_org);
        command.Parameters.AddWithValue("value2", (object) gif.gif);
        command.Parameters.AddWithValue("value3", (object) gif.status);
        command.Parameters.AddWithValue("value4", (object) gif.created_date);
        command.Parameters.AddWithValue("value5", (object) gif.updated_date);
        command.Parameters.AddWithValue("value6", (object) gif.flag);
        command.ExecuteNonQuery();
        this.conn.Close();
      }
      catch (Exception ex)
      {
      }
    }

    public WelcomeGif get_welcomeGif(int oid)
    {
      WelcomeGif welcomeGif = new WelcomeGif();
      try
      {
        string str = "select * from tbl_welcome_gif where id_org = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) oid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            welcomeGif.id_welcome_gif = Convert.ToInt32(mySqlDataReader["id_welcome_gif"].ToString());
            welcomeGif.id_org = Convert.ToInt32(mySqlDataReader["id_org"].ToString());
            welcomeGif.gif = ConfigurationManager.AppSettings["welcome"].ToString() + mySqlDataReader["gif"].ToString();
            welcomeGif.status = mySqlDataReader["status"].ToString();
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
      return welcomeGif;
    }

    public int get_color_config_flag(int oid)
    {
      WelcomeGif welcomeGif = new WelcomeGif();
      int colorConfigFlag = 2;
      try
      {
        string str = "select flag from tbl_color_config_flag where id_org = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) oid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            colorConfigFlag = Convert.ToInt32(mySqlDataReader["flag"].ToString());
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
      return colorConfigFlag;
    }

    public int get_gif_flag(int oid)
    {
      WelcomeGif welcomeGif = new WelcomeGif();
      int gifFlag = 2;
      try
      {
        string str = "select flag from tbl_welcome_gif where id_org = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) oid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            gifFlag = Convert.ToInt32(mySqlDataReader["flag"].ToString());
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
      return gifFlag;
    }

    public void update_color_confif(List<ColorConfig> color, int oid, int flag)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command1 = this.conn.CreateCommand();
        command1.CommandText = "delete from tbl_color_config where id_organisation=@value1";
        command1.Parameters.AddWithValue("value1", (object) oid);
        command1.ExecuteNonQuery();
        foreach (ColorConfig colorConfig in color)
        {
          string str = "INSERT INTO tbl_color_config(id_organisation, config_type,grid1_bk_color,grid1_text_color,grid2_bk_color,grid2_text_color,status,created_date_time,updated_date_time) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8,@value9)";
          MySqlCommand command2 = this.conn.CreateCommand();
          command2.CommandText = str;
          command2.Parameters.AddWithValue("value1", (object) colorConfig.id_organisation);
          command2.Parameters.AddWithValue("value2", (object) colorConfig.config_type);
          command2.Parameters.AddWithValue("value3", (object) colorConfig.grid1_bk_color);
          command2.Parameters.AddWithValue("value4", (object) colorConfig.grid1_text_color);
          command2.Parameters.AddWithValue("value5", (object) colorConfig.grid2_bk_color);
          command2.Parameters.AddWithValue("value6", (object) colorConfig.grid2_text_color);
          command2.Parameters.AddWithValue("value7", (object) "A");
          command2.Parameters.AddWithValue("value8", (object) DateTime.Now);
          command2.Parameters.AddWithValue("value9", (object) DateTime.Now);
          command2.ExecuteNonQuery();
        }
        string str1 = "update  tbl_color_config_flag set  flag=@value2 ,updated_date=@value3 where id_org=@value1";
        MySqlCommand command3 = this.conn.CreateCommand();
        command3.CommandText = str1;
        command3.Parameters.AddWithValue("value1", (object) oid);
        command3.Parameters.AddWithValue("value2", (object) flag);
        command3.Parameters.AddWithValue("value3", (object) DateTime.Now);
        command3.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void update_color_config_flag(int oid, int flag)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command1 = this.conn.CreateCommand();
        command1.CommandText = "delete from tbl_color_config where id_organisation=@value4";
        command1.Parameters.AddWithValue("value4", (object) oid);
        command1.ExecuteNonQuery();
        string str = "update  tbl_color_config_flag set  flag=@value2 ,updated_date=@value3 where id_org=@value1";
        MySqlCommand command2 = this.conn.CreateCommand();
        command2.CommandText = str;
        command2.Parameters.AddWithValue("value1", (object) oid);
        command2.Parameters.AddWithValue("value2", (object) flag);
        command2.Parameters.AddWithValue("value3", (object) DateTime.Now);
        command2.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void update_gif_flag(WelcomeGif gif, int oid, int flag)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command1 = this.conn.CreateCommand();
        command1.CommandText = "delete from tbl_welcome_gif where id_org=@value1";
        command1.Parameters.AddWithValue("value1", (object) oid);
        command1.ExecuteNonQuery();
        string str = "INSERT INTO tbl_welcome_gif(id_org, gif,status,created_date,updated_date,flag) VALUES (@value1,@value2,@value3,@value4,@value5,@value6)";
        MySqlCommand command2 = this.conn.CreateCommand();
        command2.CommandText = str;
        command2.Parameters.AddWithValue("value1", (object) gif.id_org);
        command2.Parameters.AddWithValue("value2", (object) gif.gif);
        command2.Parameters.AddWithValue("value3", (object) gif.status);
        command2.Parameters.AddWithValue("value4", (object) gif.created_date);
        command2.Parameters.AddWithValue("value5", (object) gif.updated_date);
        command2.Parameters.AddWithValue("value6", (object) gif.flag);
        command2.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void update_gif_status(string status, int oid)
    {
      this.conn.CreateCommand();
      MySqlCommand command = this.conn.CreateCommand();
      this.conn.Open();
      command.CommandText = "update tbl_welcome_gif set status=@value1 where id_org=@value2";
      command.Parameters.AddWithValue("value1", (object) status);
      command.Parameters.AddWithValue("value2", (object) oid);
      command.ExecuteNonQuery();
      this.conn.Close();
    }

    public void delete_Menu(int id)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        menu menu = new menu();
        command.CommandText = "delete from tbl_menu where id_menu=@value1";
        command.Parameters.AddWithValue("value1", (object) id);
        command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void update_menu_type(int oid, int type)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        menu menu = new menu();
        command.CommandText = "update tbl_menu_type set menu_type=@value2 where id_organisation=@value1;";
        command.Parameters.AddWithValue("value1", (object) oid);
        command.Parameters.AddWithValue("value2", (object) type);
        command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void update_menu(int oid)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        menu menu = new menu();
        command.CommandText = "delete from tbl_menu where id_org=@value1";
        command.Parameters.AddWithValue("value1", (object) oid);
        command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void update_menu(List<tbl_menu> menu, int oid, int old_count)
    {
      try
      {
        int num = 1;
        int count = menu.Count;
        MySqlCommand mySqlCommand = this.conn.CreateCommand();
        menu menu1 = new menu();
        if (old_count != 0)
        {
          foreach (tbl_menu tblMenu in menu)
          {
            if (num <= old_count)
            {
              MySqlCommand command = this.conn.CreateCommand();
              this.conn.Open();
              if (tblMenu.menu_icon == "null")
              {
                string str = "update tbl_menu set menu_name=@value1 , menu_url=@value2 where id_menu=@value3";
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object) tblMenu.menu_name);
                command.Parameters.AddWithValue("value2", (object) tblMenu.menu_url);
                command.Parameters.AddWithValue("value3", (object) tblMenu.id_menu);
              }
              else
              {
                string str = "update tbl_menu set menu_name=@value1 , menu_url=@value2,menu_icon=@value4 where id_menu=@value3";
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object) tblMenu.menu_name);
                command.Parameters.AddWithValue("value2", (object) tblMenu.menu_url);
                command.Parameters.AddWithValue("value3", (object) tblMenu.id_menu);
                command.Parameters.AddWithValue("value3", (object) tblMenu.menu_icon);
              }
              command.ExecuteNonQuery();
              this.conn.Close();
              mySqlCommand = (MySqlCommand) null;
            }
            else
            {
              MySqlCommand command = this.conn.CreateCommand();
              this.conn.Open();
              string str = "insert into tbl_menu (id_org,menu_name,menu_url,menu_icon) values(@value4,@value5,@value6,@value7)";
              command.CommandText = str;
              command.Parameters.AddWithValue("value4", (object) oid);
              command.Parameters.AddWithValue("value5", (object) tblMenu.menu_name);
              command.Parameters.AddWithValue("value6", (object) tblMenu.menu_url);
              command.Parameters.AddWithValue("value7", (object) tblMenu.menu_icon);
              command.ExecuteNonQuery();
              this.conn.Close();
              mySqlCommand = (MySqlCommand) null;
            }
            ++num;
          }
        }
        if (old_count == 0)
        {
          foreach (tbl_menu tblMenu in menu)
          {
            MySqlCommand command = this.conn.CreateCommand();
            this.conn.Open();
            string str = "insert into tbl_menu (id_org,menu_name,menu_url,menu_icon) values(@value4,@value5,@value6,@value7)";
            command.CommandText = str;
            command.Parameters.AddWithValue("value4", (object) oid);
            command.Parameters.AddWithValue("value5", (object) tblMenu.menu_name);
            command.Parameters.AddWithValue("value6", (object) tblMenu.menu_url);
            command.Parameters.AddWithValue("value7", (object) tblMenu.menu_icon);
            command.ExecuteNonQuery();
            this.conn.Close();
            mySqlCommand = (MySqlCommand) null;
          }
        }
        MySqlCommand command1 = this.conn.CreateCommand();
        this.conn.Open();
        string str1 = "update tbl_menu_type set menu_type=@value2  where id_organisation=@value1";
        command1.CommandText = str1;
        command1.Parameters.AddWithValue("value1", (object) oid);
        command1.Parameters.AddWithValue("value2", (object) 2);
        command1.ExecuteNonQuery();
        this.conn.Close();
        mySqlCommand = (MySqlCommand) null;
      }
      catch (Exception ex)
      {
      }
    }

    public List<tbl_menu> get_menu(int id)
    {
      List<tbl_menu> menu = new List<tbl_menu>();
      try
      {
        string str = "select * from tbl_menu where id_org = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) id);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            menu.Add(new tbl_menu()
            {
              id_menu = Convert.ToInt32(mySqlDataReader["id_menu"].ToString()),
              id_org = Convert.ToInt32(mySqlDataReader["id_org"].ToString()),
              menu_name = mySqlDataReader["menu_name"].ToString(),
              menu_url = mySqlDataReader["menu_url"].ToString(),
              menu_icon = mySqlDataReader["menu_icon"].ToString()
            });
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
      return menu;
    }

    public int get_menu_type(int oid)
    {
      int menuType = 0;
      try
      {
        string str = "select menu_type from tbl_menu_type where id_organisation = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) oid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            menuType = Convert.ToInt32(mySqlDataReader["menu_type"].ToString());
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
      return menuType;
    }

    public void delete_content(int cid)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        menu menu = new menu();
        command.CommandText = " update tbl_content set STATUS='D'  where ID_CONTENT=@value1";
        command.Parameters.AddWithValue("value1", (object) cid);
        command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public void delete_meta(int ans_id)
    {
      try
      {
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        menu menu = new menu();
        command.CommandText = " delete from tbl_content_metadata where ID_CONTENT_ANSWER=@value1";
        command.Parameters.AddWithValue("value1", (object) ans_id);
        command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
    }

    public UserSession get_org_status(UserSession user)
    {
      try
      {
        string str = "select * from tbl_organisation_status where id_organisation = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) user.id_ORGANIZATION);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            user.exp_date = Convert.ToDateTime(mySqlDataReader["expiry_date"].ToString());
            user.org_status = mySqlDataReader["status"].ToString();
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
      return user;
    }

    public int check_appuser_count(int orgid)
    {
      int num = 0;
      try
      {
        string str = "SELECT COUNT(ID_USER) FROM tbl_user WHERE ID_ORGANIZATION = @value1;";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) orgid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            num = Convert.ToInt32(mySqlDataReader["COUNT(ID_USER)"].ToString());
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
      return num;
    }

    public ticker get_ticker(int id)
    {
      ticker ticker = new ticker();
      try
      {
        string str = "select * from tbl_news_ticker where Id_org = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) id);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            ticker.Id_ticker = Convert.ToInt32(mySqlDataReader["Id_ticker"].ToString());
            ticker.Id_creator = Convert.ToInt32(mySqlDataReader["Id_creator"].ToString());
            ticker.Id_org = Convert.ToInt32(mySqlDataReader["Id_org"].ToString());
            ticker.status = mySqlDataReader["status"].ToString();
            ticker.ticker_news = mySqlDataReader["ticker_news"].ToString();
            ticker.background_color = mySqlDataReader["background_color"].ToString();
            ticker.font_color = mySqlDataReader["font_color"].ToString();
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
      return ticker;
    }

    public void add_ticker(ticker ticker)
    {
      try
      {
        this.conn.CreateCommand();
        menu menu = new menu();
        MySqlCommand command = this.conn.CreateCommand();
        this.conn.Open();
        command.CommandText = "INSERT INTO tbl_news_ticker(Id_org, Id_creator,status,update_date,ticker_news,background_color,font_color) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7)";
        command.Parameters.AddWithValue("value1", (object) ticker.Id_org);
        command.Parameters.AddWithValue("value2", (object) ticker.Id_creator);
        command.Parameters.AddWithValue("value3", (object) ticker.status);
        command.Parameters.AddWithValue("value4", (object) ticker.update_date);
        command.Parameters.AddWithValue("value5", (object) ticker.ticker_news);
        command.Parameters.AddWithValue("value6", (object) ticker.background_color);
        command.Parameters.AddWithValue("value7", (object) ticker.font_color);
        command.ExecuteNonQuery();
        this.conn.Close();
      }
      catch (Exception ex)
      {
      }
    }

    public void edit_ticker(ticker ticker)
    {
      try
      {
        this.conn.CreateCommand();
        menu menu = new menu();
        MySqlCommand command = this.conn.CreateCommand();
        this.conn.Open();
        command.CommandText = "update  tbl_news_ticker set  Id_creator=@value2,status=@value3,update_date=@value4,ticker_news=@value5,background_color=@value6,font_color=@value7 where Id_ticker=@value1 ";
        command.Parameters.AddWithValue("value1", (object) ticker.Id_ticker);
        command.Parameters.AddWithValue("value2", (object) ticker.Id_creator);
        command.Parameters.AddWithValue("value3", (object) ticker.status);
        command.Parameters.AddWithValue("value4", (object) ticker.update_date);
        command.Parameters.AddWithValue("value5", (object) ticker.ticker_news);
        command.Parameters.AddWithValue("value6", (object) ticker.background_color);
        command.Parameters.AddWithValue("value7", (object) ticker.font_color);
        command.ExecuteNonQuery();
        this.conn.Close();
      }
      catch (Exception ex)
      {
      }
    }

    public void delete_ticker(int id)
    {
      try
      {
        this.conn.CreateCommand();
        menu menu = new menu();
        MySqlCommand command = this.conn.CreateCommand();
        this.conn.Open();
        command.CommandText = "delete from  tbl_news_ticker where Id_ticker=" + (object) id;
        command.ExecuteNonQuery();
        this.conn.Close();
      }
      catch (Exception ex)
      {
      }
    }

    public tbl_profile get_mail_id(int uid)
    {
      tbl_profile mailId = new tbl_profile();
      try
      {
        string str = "select * from tbl_profile where ID_USER = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) uid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
          {
            mailId.EMAIL = mySqlDataReader["EMAIL"].ToString();
            mailId.FIRSTNAME = mySqlDataReader["FIRSTNAME"].ToString() + mySqlDataReader["LASTNAME"].ToString();
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
      return mailId;
    }

    public void sendmail(
      tbl_profile profile,
      tbl_notification note,
      int id_config,
      tbl_content_user_assisgnment con = null,
      int oid = 0)
    {
      try
      {
        string str1 = "paathshala-learningtech@paathshala.biz";
        string email = profile.EMAIL;
        string str2 = string.Empty;
        if (note.notification_type == 1)
        {
          string path = ConfigurationManager.AppSettings["mail_generic"];
          string appSetting = ConfigurationManager.AppSettings["generic_link"];
          if (path == null)
            path = "";
          if (path == null)
            path = "";
          using (StreamReader streamReader = new StreamReader(path))
            str2 = streamReader.ReadToEnd();
          string body = str2.Replace("{MESSAGE}", note.notification_message).Replace("{NAME}", profile.FIRSTNAME).Replace("{DES}", note.notification_description).Replace("{LINK}", appSetting + (object) id_config);
          string subject = "Generic Notification - " + note.notification_name;
          string notificationDescription = note.notification_description;
          new SmtpClient()
          {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = ((ICredentialsByHost) new NetworkCredential(str1, "Pls@210312")),
            Timeout = 30000
          }.Send(new MailMessage(str1, email, subject, body)
          {
            IsBodyHtml = true
          });
        }
        else if (note.notification_type == 2)
        {
          string path = ConfigurationManager.AppSettings["mail_event"];
          string appSetting = ConfigurationManager.AppSettings["event_link"];
          if (path == null)
            path = "";
          if (path == null)
            path = "";
          using (StreamReader streamReader = new StreamReader(path))
            str2 = streamReader.ReadToEnd();
          string body = str2.Replace("{MESSAGE}", note.notification_message).Replace("{NAME}", profile.FIRSTNAME).Replace("{SDATE}", Convert.ToString((object) note.start_date)).Replace("{EDATE}", Convert.ToString((object) note.end_date)).Replace("{NOTLINK}", appSetting + (object) id_config);
          string subject = "Event Based Notification - " + note.notification_name;
          string notificationDescription = note.notification_description;
          new SmtpClient()
          {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = ((ICredentialsByHost) new NetworkCredential(str1, "Pls@210312")),
            Timeout = 30000
          }.Send(new MailMessage(str1, email, subject, body)
          {
            IsBodyHtml = true
          });
        }
        else if (note.notification_type == 3)
        {
          string catImage = this.get_cat_image(Convert.ToInt32((object) con.id_category));
          string path = ConfigurationManager.AppSettings["mail_content"];
          string appSetting = ConfigurationManager.AppSettings["content_link"];
          if (path == null)
            path = "";
          if (path == null)
            path = "";
          using (StreamReader streamReader = new StreamReader(path))
            str2 = streamReader.ReadToEnd();
          string catName = this.get_cat_name(Convert.ToInt32((object) con.id_category));
          string body = str2.Replace("{TILE}", catName).Replace("{MESSAGE}", note.notification_message).Replace("{NAME}", profile.FIRSTNAME).Replace("{SDATE}", Convert.ToString((object) note.start_date)).Replace("{EDATE}", Convert.ToString((object) note.end_date)).Replace("{IMAGE}", ConfigurationManager.AppSettings["content_image"] + "/" + (object) oid + "/" + catImage).Replace("{NOTELINK}", ConfigurationManager.AppSettings["content_link"] + (object) con.id_content + "&CategoryID=" + (object) con.id_category);
          string subject = "Content Specific Notification - " + note.notification_name;
          string notificationDescription = note.notification_description;
          new SmtpClient()
          {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = ((ICredentialsByHost) new NetworkCredential(str1, "Pls@210312")),
            Timeout = 30000
          }.Send(new MailMessage(str1, email, subject, body)
          {
            IsBodyHtml = true
          });
        }
        else
        {
          if (note.notification_type == 4 || note.notification_type != 5)
            return;
          string catImage = this.get_cat_image(Convert.ToInt32((object) con.id_category));
          string path = ConfigurationManager.AppSettings["mail_content"];
          string appSetting = ConfigurationManager.AppSettings["content_link"];
          if (path == null)
            path = "";
          if (path == null)
            path = "";
          using (StreamReader streamReader = new StreamReader(path))
            str2 = streamReader.ReadToEnd();
          string catName = this.get_cat_name(Convert.ToInt32((object) con.id_category));
          string body = str2.Replace("{TILE}", catName).Replace("{MESSAGE}", note.notification_message).Replace("{NAME}", profile.FIRSTNAME).Replace("{SDATE}", Convert.ToString((object) note.start_date)).Replace("{EDATE}", Convert.ToString((object) note.end_date)).Replace("{IMAGE}", ConfigurationManager.AppSettings["content_image"] + "/" + (object) oid + "/" + catImage).Replace("{NOTELINK}", ConfigurationManager.AppSettings["content_link"] + (object) con.id_content + "&CategoryID=" + (object) con.id_category);
          string subject = "Content Specific Notification - " + note.notification_name;
          string notificationDescription = note.notification_description;
          new SmtpClient()
          {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = ((ICredentialsByHost) new NetworkCredential(str1, "Pls@210312")),
            Timeout = 30000
          }.Send(new MailMessage(str1, email, subject, body)
          {
            IsBodyHtml = true
          });
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Message : " + ex.Message);
        Console.WriteLine("Trace : " + ex.StackTrace);
      }
    }

    public string get_cat_image(int cat_id)
    {
      string catImage = "";
      try
      {
        string str = "select IMAGE_PATH from tbl_category where ID_CATEGORY = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) cat_id);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            catImage = mySqlDataReader["IMAGE_PATH"].ToString();
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
      return catImage;
    }

    public void sendmail_program(
      tbl_profile profile,
      tbl_notification note,
      int id_config,
      tbl_content_program_mapping con = null,
      int oid = 0)
    {
      try
      {
        string str1 = "paathshala-learningtech@paathshala.biz";
        string email = profile.EMAIL;
        string str2 = string.Empty;
        if (note.notification_type == 3)
        {
          string catImage = this.get_cat_image(Convert.ToInt32((object) con.id_category));
          string path = ConfigurationManager.AppSettings["mail_content"];
          string appSetting = ConfigurationManager.AppSettings["content_link"];
          if (path == null)
            path = "";
          if (path == null)
            path = "";
          using (StreamReader streamReader = new StreamReader(path))
            str2 = streamReader.ReadToEnd();
          string catName = this.get_cat_name(Convert.ToInt32((object) con.id_category));
          string body = str2.Replace("{TILE}", catName).Replace("{MESSAGE}", note.notification_message).Replace("{NAME}", profile.FIRSTNAME).Replace("{SDATE}", Convert.ToString((object) note.start_date)).Replace("{EDATE}", Convert.ToString((object) note.end_date)).Replace("{IMAGE}", ConfigurationManager.AppSettings["category_image"] + "/" + (object) oid + "/" + catImage).Replace("{NOTELINK}", ConfigurationManager.AppSettings["program_link"] + (object) con.id_category);
          string subject = "Content Specific Notification - " + note.notification_name;
          string notificationDescription = note.notification_description;
          new SmtpClient()
          {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = ((ICredentialsByHost) new NetworkCredential(str1, "Pls@210312")),
            Timeout = 30000
          }.Send(new MailMessage(str1, email, subject, body)
          {
            IsBodyHtml = true
          });
        }
        else
        {
          int notificationType = note.notification_type;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Message : " + ex.Message);
        Console.WriteLine("Trace : " + ex.StackTrace);
      }
    }

    public void sendmail_assessment(
      tbl_profile profile,
      tbl_notification note,
      int id_config,
      tbl_assessment_user_assignment con = null,
      int oid = 0)
    {
      try
      {
        string str1 = "paathshala-learningtech@paathshala.biz";
        string email = profile.EMAIL;
        string str2 = string.Empty;
        if (note.notification_type == 7)
        {
          string catImage = this.get_cat_image(Convert.ToInt32((object) con.id_category));
          string path = ConfigurationManager.AppSettings["mail_content"];
          string appSetting = ConfigurationManager.AppSettings["content_link"];
          if (path == null)
            path = "";
          if (path == null)
            path = "";
          using (StreamReader streamReader = new StreamReader(path))
            str2 = streamReader.ReadToEnd();
          string assesmentName = this.get_Assesment_Name(Convert.ToInt32((object) con.id_assessment));
          string body = str2.Replace("{TILE}", assesmentName).Replace("{MESSAGE}", note.notification_message).Replace("{NAME}", profile.FIRSTNAME).Replace("{SDATE}", Convert.ToString((object) note.start_date)).Replace("{EDATE}", Convert.ToString((object) note.end_date)).Replace("{IMAGE}", ConfigurationManager.AppSettings["category_image"] + "/" + (object) oid + "/" + catImage).Replace("{NOTELINK}", ConfigurationManager.AppSettings["assessment_link"] + (object) con.id_assessment);
          string subject = "Content Specific Notification - " + note.notification_name;
          string notificationDescription = note.notification_description;
          new SmtpClient()
          {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = ((ICredentialsByHost) new NetworkCredential(str1, "Pls@210312")),
            Timeout = 30000
          }.Send(new MailMessage(str1, email, subject, body)
          {
            IsBodyHtml = true
          });
        }
        else
        {
          int notificationType = note.notification_type;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Message : " + ex.Message);
        Console.WriteLine("Trace : " + ex.StackTrace);
      }
    }

    public string get_Assesment_Name(int assid)
    {
      string assesmentName = "";
      try
      {
        string str = "select * from tbl_assessment where id_assessment = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) assid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            assesmentName = mySqlDataReader["assessment_title"].ToString();
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
      return assesmentName;
    }

    public string get_cat_name(int catid)
    {
      string catName = "";
      try
      {
        string str = "select * from tbl_category where ID_CATEGORY = @value1 ";
        this.conn.Open();
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) catid);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            catName = mySqlDataReader["CATEGORYNAME"].ToString();
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
      return catName;
    }

    public string configcon(tbl_notification_pre_config config)
    {
      string str = (string) null;
      try
      {
        MySqlCommand command = this.conn.CreateCommand();
        command.CommandText = "INSERT INTO tbl_notification_pre_config(id_notification, id_creater, notification_key, notification_action_type, id_user,id_content,user_type,read_timestamp,start_date,end_date,status,updated_date_time,id_category,id_assessment) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8,@value9,@value10,@value11,@value12,@value13,@value14)";
        command.Parameters.AddWithValue("value1", (object) config.id_notification);
        command.Parameters.AddWithValue("value2", (object) config.id_creater);
        command.Parameters.AddWithValue("value3", (object) config.notification_key);
        command.Parameters.AddWithValue("value4", (object) config.notification_action_type);
        command.Parameters.AddWithValue("value5", (object) config.id_user);
        command.Parameters.AddWithValue("value6", (object) config.id_content);
        command.Parameters.AddWithValue("value7", (object) config.user_type);
        command.Parameters.AddWithValue("value8", (object) config.read_timestamp);
        command.Parameters.AddWithValue("value9", (object) config.start_date);
        command.Parameters.AddWithValue("value10", (object) config.end_date);
        command.Parameters.AddWithValue("value11", (object) config.status);
        command.Parameters.AddWithValue("value12", (object) DateTime.Now);
        command.Parameters.AddWithValue("value13", (object) config.id_category);
        command.Parameters.AddWithValue("value14", (object) config.id_assessment);
        this.conn.Open();
        command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        this.conn.Close();
      }
      return str;
    }
  }
}
