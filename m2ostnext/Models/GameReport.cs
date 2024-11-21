// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GameReport
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class GameReport
  {
    public int id_user { get; set; }

    public string USERID { get; set; }

    public string EMPLOYEEID { get; set; }

    public string UNAME { get; set; }

    public string LOCATION { get; set; }

    public string user_designation { get; set; }

    public string uStatus { get; set; }

    public string user_department { get; set; }

    public string user_function { get; set; }

    public int id_game { get; set; }

    public string game_title { get; set; }

    public double final_weightage { get; set; }

    public double final_score { get; set; }

    public double ucount { get; set; }

    public string game_type { get; set; }

    public string game_mode { get; set; }

    public string player_type { get; set; }

    public GameReport(MySqlDataReader reader)
    {
      this.USERID = Convert.ToString(reader[nameof (USERID)]);
      this.EMPLOYEEID = Convert.ToString(reader[nameof (EMPLOYEEID)]);
      this.UNAME = Convert.ToString(reader[nameof (UNAME)]);
      this.user_designation = Convert.ToString(reader[nameof (user_designation)]);
      this.LOCATION = Convert.ToString(reader[nameof (LOCATION)]);
      this.uStatus = Convert.ToString(reader[nameof (uStatus)]);
      this.id_user = Convert.ToInt32(reader[nameof (id_user)]);
      this.game_title = Convert.ToString(reader[nameof (game_title)]);
      this.id_game = Convert.ToInt32(reader[nameof (id_game)]);
      this.user_department = Convert.ToString(reader[nameof (user_department)]);
      this.user_function = Convert.ToString(reader[nameof (user_function)]);
      this.final_weightage = Convert.ToDouble(reader[nameof (final_weightage)]);
      this.ucount = Convert.ToDouble(reader[nameof (ucount)]);
      this.player_type = Convert.ToString(reader[nameof (player_type)]);
      this.game_mode = Convert.ToString(reader[nameof (game_mode)]);
      this.game_type = Convert.ToString(reader[nameof (game_type)]);
      if (this.final_weightage > 0.0 && this.ucount > 0.0)
        this.final_score = this.final_weightage / this.ucount;
      else
        this.final_score = 0.0;
    }
  }
}
