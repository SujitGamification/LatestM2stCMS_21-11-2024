// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GamificationModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace m2ostnext.Models
{
  public class GamificationModel
  {
    private MySqlConnection connection;

    public GamificationModel() => this.connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

    public Tuple<Tempmodel, List<assess_title>> CreateGame(GameElements game)
    {
      DateTime now = DateTime.Now;
      string str1 = "SELECT FIRSTNAME FROM tbl_profile where ID_USER = @value20";
      string str2 = "Insert into tbl_game_creation (id_organisation,id_game_creator,game_title,game_description,game_creator_name,game_expiry_date,game_mode,game_type,id_game_path,player_type,id_group,game_comment,status,game_creation_datetime,updated_date_time,game_phase) values(@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8,@value9,@value10,@value11,@value12,@value13,@value14,@value15,@value16)";
      string str3 = " ";
      this.connection.Open();
      MySqlCommand command1 = this.connection.CreateCommand();
      command1.CommandText = str1;
      command1.Parameters.AddWithValue("value20", (object) game.id_game_creator);
      MySqlDataReader mySqlDataReader1 = command1.ExecuteReader();
      while (mySqlDataReader1.Read())
        str3 = mySqlDataReader1["FIRSTNAME"].ToString();
      this.connection.Close();
      this.connection.Open();
      MySqlCommand command2 = this.connection.CreateCommand();
      command2.CommandText = str2;
      command2.Parameters.AddWithValue("value1", (object) game.id_organization);
      command2.Parameters.AddWithValue("value2", (object) game.id_game_creator);
      command2.Parameters.AddWithValue("value3", (object) game.game_title);
      command2.Parameters.AddWithValue("value4", (object) game.game_description);
      command2.Parameters.AddWithValue("value5", (object) str3);
      command2.Parameters.AddWithValue("value6", (object) game.game_expiry_date);
      command2.Parameters.AddWithValue("value7", (object) game.game_mode);
      command2.Parameters.AddWithValue("value8", (object) game.game_type);
      command2.Parameters.AddWithValue("value9", (object) game.id_game_path);
      command2.Parameters.AddWithValue("value10", (object) game.player_type);
      command2.Parameters.AddWithValue("value11", (object) game.id_group);
      command2.Parameters.AddWithValue("value12", (object) game.game_comment);
      command2.Parameters.AddWithValue("value13", (object) game.game_status);
      command2.Parameters.AddWithValue("value14", (object) now);
      command2.Parameters.AddWithValue("value15", (object) now);
      command2.Parameters.AddWithValue("value16", (object) 1);
      command2.ExecuteNonQuery();
      int int32 = Convert.ToInt32(command2.LastInsertedId);
      this.connection.Close();
      Tempmodel tempmodel = new Tempmodel();
      tempmodel.idGame = int32;
      tempmodel.phaseflag = 1;
      string str4 = "SELECT assessment_title FROM tbl_assessment where id_organization =@value21 ;";
      this.connection.Open();
      MySqlCommand command3 = this.connection.CreateCommand();
      command3.CommandText = str4;
      command3.Parameters.AddWithValue("value21", (object) game.id_organization);
      List<assess_title> assessTitleList = new List<assess_title>();
      MySqlDataReader mySqlDataReader2 = command3.ExecuteReader();
      while (mySqlDataReader2.Read())
        assessTitleList.Add(new assess_title()
        {
          assessment_title = mySqlDataReader2["assessment_title"].ToString()
        });
      this.connection.Close();
      return Tuple.Create<Tempmodel, List<assess_title>>(tempmodel, assessTitleList);
    }

    public List<programs_title> get_prog(int orgid)
    {
      string str = "SELECT CATEGORYNAME , ID_CATEGORY FROM tbl_category where CATEGORY_TYPE=0 and ID_ORGANIZATION=@value0;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) orgid);
      List<programs_title> prog = new List<programs_title>();
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        prog.Add(new programs_title()
        {
          programs_titl = mySqlDataReader["CATEGORYNAME"].ToString(),
          program_id = Convert.ToInt32(mySqlDataReader["ID_CATEGORY"].ToString())
        });
      this.connection.Close();
      return prog;
    }

    public List<assess_title> get_assess(int orgid)
    {
      string str = "SELECT assessment_title,id_assessment FROM tbl_assessment where id_organization =@value0 ;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) orgid);
      List<assess_title> assess = new List<assess_title>();
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        assess.Add(new assess_title()
        {
          assessment_title = mySqlDataReader["assessment_title"].ToString(),
          assess_id = Convert.ToInt32(mySqlDataReader["id_assessment"].ToString())
        });
      this.connection.Close();
      return assess;
    }

    public List<assess_title> Insert_Assessment(string id, string gameIdval)
    {
      int int32_1 = Convert.ToInt32(id);
      int int32_2 = Convert.ToInt32(gameIdval);
      string str1 = "Insert into  tbl_game_path (id_assessment,id_game) values(@value0,@value1);";
      this.connection.Open();
      MySqlCommand command1 = this.connection.CreateCommand();
      command1.CommandText = str1;
      command1.Parameters.AddWithValue("value0", (object) int32_1);
      command1.Parameters.AddWithValue("value1", (object) int32_2);
      command1.ExecuteNonQuery();
      this.connection.Close();
      string str2 = "SELECT assessment_title,id_assessment FROM tbl_assessment where id_assessment =@value0 ;";
      this.connection.Open();
      MySqlCommand command2 = this.connection.CreateCommand();
      command2.CommandText = str2;
      command2.Parameters.AddWithValue("value0", (object) int32_1);
      List<assess_title> assessTitleList = new List<assess_title>();
      MySqlDataReader mySqlDataReader = command2.ExecuteReader();
      while (mySqlDataReader.Read())
        assessTitleList.Add(new assess_title()
        {
          assessment_title = mySqlDataReader["assessment_title"].ToString(),
          assess_id = Convert.ToInt32(mySqlDataReader["id_assessment"].ToString())
        });
      this.connection.Close();
      return assessTitleList;
    }

    public List<programs_title> Insert_Program(string id, string gameIdval)
    {
      int int32 = Convert.ToInt32(id);
      Convert.ToInt32(gameIdval);
      string str1 = "Insert into  tbl_game_path (id_category_heading,id_game) values(@value0,@value1);";
      this.connection.Open();
      MySqlCommand command1 = this.connection.CreateCommand();
      command1.CommandText = str1;
      command1.Parameters.AddWithValue("value0", (object) int32);
      command1.Parameters.AddWithValue("value1", (object) gameIdval);
      command1.ExecuteNonQuery();
      this.connection.Close();
      string str2 = "SELECT CATEGORYNAME,ID_CATEGORY FROM tbl_category where CATEGORY_TYPE=0 and ID_CATEGORY=@value0;";
      this.connection.Open();
      MySqlCommand command2 = this.connection.CreateCommand();
      command2.CommandText = str2;
      command2.Parameters.AddWithValue("value0", (object) int32);
      List<programs_title> programsTitleList = new List<programs_title>();
      MySqlDataReader mySqlDataReader = command2.ExecuteReader();
      while (mySqlDataReader.Read())
        programsTitleList.Add(new programs_title()
        {
          programs_titl = mySqlDataReader["CATEGORYNAME"].ToString(),
          program_id = Convert.ToInt32(mySqlDataReader["ID_CATEGORY"].ToString())
        });
      this.connection.Close();
      return programsTitleList;
    }

    public bool get_assessmentlist(GameElements game) => true;

    public List<game_path_components> get_game_path(int game_id)
    {
      string str = "SELECT * FROM tbl_game_path where id_game =@value0 ;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) game_id);
      List<game_path_components> ass_id = new List<game_path_components>();
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        ass_id.Add(new game_path_components()
        {
          id_assessment = Convert.ToInt32(mySqlDataReader["id_assessment"]),
          id_program = Convert.ToInt32(mySqlDataReader["id_category_heading"]),
          game_id = game_id
        });
      this.connection.Close();
      return new GamificationModel().getAssessTitle(ass_id);
    }

    public string get_game_name(int game_id)
    {
      string str = "SELECT game_title FROM tbl_game_creation where id_game =@value0 ;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) game_id);
      string gameName = "";
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        gameName = mySqlDataReader["game_title"].ToString();
      this.connection.Close();
      return gameName;
    }

    public List<game_path_components> getAssessTitle(List<game_path_components> ass_id)
    {
      string str = "SELECT assessment_title,id_assessment FROM tbl_assessment where id_assessment =@value0 ;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      List<game_path_components> assessTitle = new List<game_path_components>();
      foreach (game_path_components gamePathComponents in ass_id)
      {
        if (gamePathComponents.id_assessment != 0)
        {
          command.Parameters.AddWithValue("value0", (object) gamePathComponents.id_assessment);
          MySqlDataReader mySqlDataReader = command.ExecuteReader();
          while (mySqlDataReader.Read())
            assessTitle.Add(new game_path_components()
            {
              assessment_title = mySqlDataReader["assessment_title"].ToString(),
              id_assessment = Convert.ToInt32(mySqlDataReader["id_assessment"].ToString()),
              game_id = gamePathComponents.game_id
            });
        }
      }
      this.connection.Close();
      return assessTitle;
    }

    public List<game_path_components> getProgTitle(List<game_path_components> ass_id)
    {
      string str = "SELECT CATEGORYNAME , ID_CATEGORY FROM tbl_category where CATEGORY_TYPE=0 and ID_CATEGORY=@value0;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      List<game_path_components> progTitle = new List<game_path_components>();
      foreach (game_path_components gamePathComponents in ass_id)
      {
        if (gamePathComponents.id_program != 0)
        {
          ass_id.Count<game_path_components>();
          command.Parameters.AddWithValue("value0", (object) gamePathComponents.id_program);
          MySqlDataReader mySqlDataReader = command.ExecuteReader();
          while (mySqlDataReader.Read())
            progTitle.Add(new game_path_components()
            {
              program_title = mySqlDataReader["CATEGORYNAME"].ToString(),
              id_program = Convert.ToInt32(mySqlDataReader["ID_CATEGORY"].ToString()),
              game_id = gamePathComponents.game_id
            });
        }
      }
      this.connection.Close();
      return progTitle;
    }

    public List<game_path_components> get_prog_review(int game_id)
    {
      string str = "SELECT * FROM tbl_game_path where id_game =@value0 ;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) game_id);
      List<game_path_components> ass_id = new List<game_path_components>();
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        ass_id.Add(new game_path_components()
        {
          id_assessment = Convert.ToInt32(mySqlDataReader["id_assessment"]),
          id_program = Convert.ToInt32(mySqlDataReader["id_category_heading"]),
          game_id = game_id
        });
      this.connection.Close();
      return new GamificationModel().getProgTitle(ass_id);
    }

    public bool Delete_Program(int game_id, int pro_id)
    {
      string str = "delete from tbl_game_path where id_category_heading=@value0 and id_game=@value1";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) pro_id);
      command.Parameters.AddWithValue("value1", (object) game_id);
      command.ExecuteNonQuery();
      this.connection.Close();
      return true;
    }

    public bool Delete_Assessment(int game_id, int ass_id)
    {
      string str = "delete from tbl_game_path where id_assessment=@value0 and id_game=@value1";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) ass_id);
      command.Parameters.AddWithValue("value1", (object) game_id);
      command.ExecuteNonQuery();
      this.connection.Close();
      return true;
    }

    public int insert_group_name(string grp_name)
    {
      string str = "insert into tbl_game_group(group_name) values(@value0)";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) grp_name);
      command.ExecuteNonQuery();
      int int32 = Convert.ToInt32(command.LastInsertedId);
      this.connection.Close();
      return int32;
    }

    public List<GameElements> game_dashboard(int orgid)
    {
      string str = "SELECT * FROM tbl_game_creation where id_organisation=@value0;";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) orgid);
      List<GameElements> gameElementsList = new List<GameElements>();
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        gameElementsList.Add(new GameElements()
        {
          id_game = Convert.ToInt32(mySqlDataReader["id_game"].ToString()),
          id_game_creator = Convert.ToInt32(mySqlDataReader["id_game_creator"].ToString()),
          game_title = mySqlDataReader["game_title"].ToString(),
          game_description = mySqlDataReader["game_description"].ToString(),
          game_expiry_date = Convert.ToDateTime(mySqlDataReader["game_expiry_date"].ToString()),
          game_mode = mySqlDataReader["game_mode"].ToString(),
          game_type = mySqlDataReader["game_type"].ToString(),
          id_game_path = Convert.ToInt32(mySqlDataReader["id_game_path"].ToString()),
          player_type = mySqlDataReader["player_type"].ToString(),
          id_group = Convert.ToInt32(mySqlDataReader["id_group"].ToString()),
          game_comment = mySqlDataReader["game_comment"].ToString(),
          game_status = mySqlDataReader["status"].ToString(),
          game_creation_datetime = Convert.ToDateTime(mySqlDataReader["game_creation_datetime"].ToString())
        });
      this.connection.Close();
      return gameElementsList;
    }

    public List<GameElements> getSoloGames(int orgid)
    {
      string str = "SELECT * FROM tbl_game_creation where id_organisation=@value0 and player_type='Solo';";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) orgid);
      List<GameElements> soloGames = new List<GameElements>();
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        soloGames.Add(new GameElements()
        {
          id_game = Convert.ToInt32(mySqlDataReader["id_game"].ToString()),
          game_title = mySqlDataReader["game_title"].ToString()
        });
      this.connection.Close();
      return soloGames;
    }

    public List<GameElements> getGroupGames(int orgid)
    {
      string str = "SELECT * FROM tbl_game_creation where id_organisation=@value0 and player_type='group';";
      this.connection.Open();
      MySqlCommand command = this.connection.CreateCommand();
      command.CommandText = str;
      command.Parameters.AddWithValue("value0", (object) orgid);
      List<GameElements> groupGames = new List<GameElements>();
      MySqlDataReader mySqlDataReader = command.ExecuteReader();
      while (mySqlDataReader.Read())
        groupGames.Add(new GameElements()
        {
          id_game = Convert.ToInt32(mySqlDataReader["id_game"].ToString()),
          game_title = mySqlDataReader["game_title"].ToString()
        });
      this.connection.Close();
      return groupGames;
    }

    public List<GameElement> getGameElementsList(string STR)
    {
      List<GameElement> gameElementsList = new List<GameElement>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          GameElement gameElement = new GameElement(reader);
          gameElementsList.Add(gameElement);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return gameElementsList;
    }

    public List<GroupUsers> getGroupUserList(string STR)
    {
      List<GroupUsers> groupUserList = new List<GroupUsers>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          GroupUsers groupUsers = new GroupUsers(reader);
          groupUserList.Add(groupUsers);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return groupUserList;
    }

    public List<GameMaster> getGameList(string STR)
    {
      List<GameMaster> gameList = new List<GameMaster>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          GameMaster gameMaster = new GameMaster(reader);
          gameList.Add(gameMaster);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return gameList;
    }

    public List<GameGroupSummary> getGameGroupSummary(string STR)
    {
      List<GameGroupSummary> gameGroupSummary1 = new List<GameGroupSummary>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          GameGroupSummary gameGroupSummary2 = new GameGroupSummary(reader);
          gameGroupSummary1.Add(gameGroupSummary2);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return gameGroupSummary1;
    }

    public List<GameUserSummary> getGameUserSummary(string STR)
    {
      List<GameUserSummary> gameUserSummary1 = new List<GameUserSummary>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          GameUserSummary gameUserSummary2 = new GameUserSummary(reader);
          gameUserSummary1.Add(gameUserSummary2);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return gameUserSummary1;
    }

    public List<GameGroup> getGroupData(string STR)
    {
      List<GameGroup> groupData = new List<GameGroup>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          GameGroup gameGroup = new GameGroup(reader);
          groupData.Add(gameGroup);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return groupData;
    }

    public List<ProgramScoringClass> getProgramScoring(string STR)
    {
      List<ProgramScoringClass> programScoring = new List<ProgramScoringClass>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          ProgramScoringClass programScoringClass = new ProgramScoringClass(reader);
          programScoring.Add(programScoringClass);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return programScoring;
    }

    public List<GameReport> getGameReport(string STR)
    {
      List<GameReport> gameReport1 = new List<GameReport>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          GameReport gameReport2 = new GameReport(reader);
          gameReport1.Add(gameReport2);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return gameReport1;
    }
  }
}
