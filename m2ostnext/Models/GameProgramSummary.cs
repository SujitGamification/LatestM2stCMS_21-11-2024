// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GameProgramSummary
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Collections.Generic;

namespace m2ostnext.Models
{
  public class GameProgramSummary
  {
    public int id_game { get; set; }

    public string game_title { get; set; }

    public int id_category { get; set; }

    public string category_name { get; set; }

    public string user_name { get; set; }

    public string userid { get; set; }

    public string employeeid { get; set; }

    public int id_user { get; set; }

    public double content_score { get; set; }

    public double content_weightage { get; set; }

    public List<scoring_matrix> assessment_score { get; set; }

    public List<scoring_matrix> kpi_score { get; set; }
  }
}
