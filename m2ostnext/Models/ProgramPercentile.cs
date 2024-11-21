// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ProgramPercentile
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class ProgramPercentile
  {
    public int id_user { get; set; }

    public double IndavgScore { get; set; }

    public string UNAME { get; set; }

    public string EMPLOYEEID { get; set; }

    public string USERID { get; set; }

    public string CATEGORYNAME { get; set; }

    public string game_title { get; set; }

    public double percentile { get; set; }

    public ProgramPercentile(MySqlDataReader reader)
    {
      this.id_user = Convert.ToInt32(reader[nameof (id_user)]);
      this.IndavgScore = Convert.ToDouble(reader[nameof (IndavgScore)]);
      this.UNAME = Convert.ToString(reader[nameof (UNAME)]);
      this.USERID = Convert.ToString(reader[nameof (USERID)]);
      this.CATEGORYNAME = Convert.ToString(reader[nameof (CATEGORYNAME)]);
      this.game_title = Convert.ToString(reader[nameof (game_title)]);
      this.EMPLOYEEID = Convert.ToString(reader[nameof (EMPLOYEEID)]);
    }
  }
}
