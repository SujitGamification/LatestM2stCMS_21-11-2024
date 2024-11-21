// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ProgramScoringModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace m2ostnext.Models
{
  public class ProgramScoringModel
  {
    private db_m2ostEntities db = new db_m2ostEntities();
    private MySqlConnection connection;

    public ProgramScoringModel() => this.connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

    public string checkProgramComplition(int cid, int uid, int oid) => this.db.sc_game_element_weightage.SqlQuery("select * from sc_game_element_weightage where element_type=1 and id_category=" + (object) cid + " and id_user=" + (object) uid + " ").FirstOrDefault<sc_game_element_weightage>() == null ? "0" : "1";

    public double getWeightage(string sqlq)
    {
      try
      {
        double weightage = 0.0;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = sqlq;
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        if (mySqlDataReader.HasRows)
        {
          while (mySqlDataReader.Read())
            weightage = Convert.ToDouble(mySqlDataReader.GetDouble(0));
          mySqlDataReader.Close();
        }
        return weightage;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
    }

    public double getContentWeightage(int? cid, double? value)
    {
      if (Convert.ToDouble((object) value) <= 0.0)
        return 0.0;
      return this.getWeightage("" + " SELECT b.kpi_value weightage FROM tbl_kpi_master a, tbl_kpi_grid b, tbl_kpi_program_scoring c " + " WHERE   a.id_kpi_master = b.id_kpi_master AND a.id_kpi_master = c.id_kpi_master AND b.id_kpi_master = c.id_kpi_master " + " and " + (object) value + " > b.start_range and " + (object) value + " <=end_range and c.id_category=" + (object) cid + " and c.kpi_type=1 ");
    }

    public double getAssessmentWeightage(int? aid, int? cid, double? value)
    {
      if (Convert.ToDouble((object) value) <= 0.0)
        return 0.0;
      return this.getWeightage("" + " SELECT b.kpi_value weightage FROM tbl_kpi_master a, tbl_kpi_grid b, tbl_kpi_program_scoring c " + " WHERE   a.id_kpi_master = b.id_kpi_master     AND a.id_kpi_master = c.id_kpi_master AND b.id_kpi_master = c.id_kpi_master " + " and " + (object) value + " > b.start_range and " + (object) value + " <=end_range and c.id_assessment=" + (object) aid + " and c.kpi_type=2 ");
    }

    public double getKPIWeightage(int kid, int cid, double? value)
    {
      if (Convert.ToDouble((object) value) <= 0.0)
        return 0.0;
      return this.getWeightage("" + " SELECT  b.kpi_value weightage FROM tbl_kpi_master a, tbl_kpi_grid b, tbl_kpi_program_scoring c " + " WHERE   a.id_kpi_master = b.id_kpi_master AND a.id_kpi_master = c.id_kpi_master AND b.id_kpi_master = c.id_kpi_master " + " and " + (object) value + " > b.start_range and " + (object) value + " <=end_range and c.id_category=" + (object) cid + " and c.kpi_type=3 ");
    }

    public void setKPIWeightage(string tid)
    {
      DbSet<sc_program_kpi_score> scProgramKpiScore1 = this.db.sc_program_kpi_score;
      Expression<Func<sc_program_kpi_score, bool>> predicate = (Expression<Func<sc_program_kpi_score, bool>>) (t => t.transactionid == tid);
      foreach (sc_program_kpi_score scProgramKpiScore2 in scProgramKpiScore1.Where<sc_program_kpi_score>(predicate).ToList<sc_program_kpi_score>())
      {
        sc_program_kpi_score item = scProgramKpiScore2;
        if (this.db.sc_program_kpi_weightage.Where<sc_program_kpi_weightage>((Expression<Func<sc_program_kpi_weightage, bool>>) (t => t.id_category == item.id_category && t.id_kpi_master == item.id_kpi_master && t.id_user == item.id_user)).FirstOrDefault<sc_program_kpi_weightage>() == null)
        {
          sc_program_kpi_weightage entity = new sc_program_kpi_weightage();
          double kpiWeightage = new ProgramScoringModel().getKPIWeightage(Convert.ToInt32((object) item.id_kpi_master), Convert.ToInt32((object) item.id_category), item.score);
          entity.id_category = item.id_category;
          entity.id_kpi_master = item.id_kpi_master;
          entity.id_organization = item.id_organization;
          entity.id_user = item.id_user;
          entity.score = item.score;
          entity.kpi_wieghtage = new double?(kpiWeightage);
          entity.log_datetime = item.log_datetime;
          entity.updated_date_time = new DateTime?(DateTime.Now);
          entity.status = "A";
          this.db.sc_program_kpi_weightage.Add(entity);
          this.db.SaveChanges();
        }
      }
    }

    public void generateGameWeightage(string gameid) => this.db.tbl_game_creation.Where<tbl_game_creation>((Expression<Func<tbl_game_creation, bool>>) (t => t.gameid == gameid && t.status == "A")).FirstOrDefault<tbl_game_creation>();

    public List<scoring_matrix> getScoreMatrix(string STR)
    {
      List<scoring_matrix> scoreMatrix = new List<scoring_matrix>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          scoring_matrix scoringMatrix = new scoring_matrix(reader);
          scoreMatrix.Add(scoringMatrix);
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
      return scoreMatrix;
    }

    public double getPhase2WeightageContent(int? cid, double? value)
    {
      if (Convert.ToDouble((object) value) <= 0.0)
        return 0.0;
      return this.getWeightage("" + " SELECT (" + (object) value + " * a.ps_weightage / 100) weightage FROM  tbl_kpi_program_scoring a WHERE  a.id_category =  " + (object) cid + " AND a.kpi_type=1 ");
    }

    public double getPhase2WeightageAssessment(int? cid, int? aid, double? value)
    {
      if (Convert.ToDouble((object) value) <= 0.0)
        return 0.0;
      return this.getWeightage("" + " SELECT (" + (object) value + " * a.ps_weightage / 100) weightage FROM  tbl_kpi_program_scoring a WHERE  a.id_category =  " + (object) cid + " AND  a.id_assessment=" + (object) aid + " AND a.kpi_type=2 ");
    }

    public double getPhase2WeightageKPI(int? cid, int? kid, double? value)
    {
      if (Convert.ToDouble((object) value) <= 0.0)
        return 0.0;
      return this.getWeightage("" + " SELECT (" + (object) value + " * a.ps_weightage / 100) weightage FROM  tbl_kpi_program_scoring a WHERE  a.id_category =  " + (object) cid + " AND  a.id_kpi_master=" + (object) kid + " AND a.kpi_type=3 ");
    }

    public double getPhase3Weightage(int? gid, int? cid, double? value)
    {
      if (Convert.ToDouble((object) value) <= 0.0)
        return 0.0;
      return this.getWeightage("" + (" SELECT (" + (object) value + "* a.weightage / 100) weightage FROM  tbl_game_process_path a WHERE  a.id_category =  " + (object) cid + "  and a.id_game=" + (object) gid));
    }

    public List<ProgramReport> getProgramReport(string STR)
    {
      List<ProgramReport> programReport1 = new List<ProgramReport>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          ProgramReport programReport2 = new ProgramReport(reader);
          programReport1.Add(programReport2);
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
      return programReport1;
    }

    public List<ProgramPercentile> getProgramPercentile(string STR)
    {
      List<ProgramPercentile> programPercentile1 = new List<ProgramPercentile>();
      try
      {
        string str = STR;
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          ProgramPercentile programPercentile2 = new ProgramPercentile(reader);
          programPercentile1.Add(programPercentile2);
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
      return programPercentile1;
    }
  }
}
