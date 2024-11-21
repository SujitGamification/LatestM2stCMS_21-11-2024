// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ProgramScoringClass
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class ProgramScoringClass
  {
    public int id_kpi_program_scoring { get; set; }

    public int id_kpi_master { get; set; }

    public string kpi_name { get; set; }

    public int id_category { get; set; }

    public int id_assessment { get; set; }

    public string assessment_title { get; set; }

    public double ps_weightage { get; set; }

    public int id_organization { get; set; }

    public int kpi_type { get; set; }

    public string kpi_type_lable { get; set; }

    public ProgramScoringClass(MySqlDataReader reader)
    {
      this.id_kpi_program_scoring = Convert.ToInt32(reader[nameof (id_kpi_program_scoring)]);
      this.id_kpi_master = Convert.ToInt32(reader[nameof (id_kpi_master)]);
      this.id_category = Convert.ToInt32(reader[nameof (id_category)]);
      this.id_assessment = Convert.ToInt32(reader[nameof (id_assessment)]);
      this.id_organization = Convert.ToInt32(reader[nameof (id_organization)]);
      this.id_organization = Convert.ToInt32(reader[nameof (id_organization)]);
      this.kpi_name = Convert.ToString(reader[nameof (kpi_name)]);
      this.assessment_title = Convert.ToString(reader[nameof (assessment_title)]);
      this.kpi_type_lable = Convert.ToString(reader[nameof (kpi_type_lable)]);
      this.ps_weightage = Convert.ToDouble(reader[nameof (ps_weightage)]);
    }
  }
}
