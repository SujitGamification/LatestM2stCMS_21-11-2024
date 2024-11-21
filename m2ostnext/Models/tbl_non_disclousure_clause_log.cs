// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.tbl_non_disclousure_clause_log
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext.Models
{
  public class tbl_non_disclousure_clause_log
  {
    public int id_clause_log { get; set; }

    public int id_user { get; set; }

    public int id_org { get; set; }

    public string log_status { get; set; }

    public DateTime updated_date_time { get; set; }
  }
}
