// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ProgramReport
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class ProgramReport
  {
    public int id_user { get; set; }

    public double percentage { get; set; }

    public double weightage { get; set; }

    public double pweightage { get; set; }

    public double score { get; set; }

    public ProgramReport(MySqlDataReader reader)
    {
      this.id_user = Convert.ToInt32(reader[nameof (id_user)]);
      this.percentage = Convert.ToDouble(reader[nameof (percentage)]);
      this.weightage = Convert.ToDouble(reader[nameof (weightage)]);
      this.pweightage = Convert.ToDouble(reader[nameof (pweightage)]);
      this.score = Convert.ToDouble(reader[nameof (score)]);
    }
  }
}
