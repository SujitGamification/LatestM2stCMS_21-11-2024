// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.tbl_certificate_assignment
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext.Models
{
  public class tbl_certificate_assignment
  {
    public int id_certificate_assignment { get; set; }

    public int id_certificate { get; set; }

    public int id_assessment { get; set; }

    public string status { get; set; }

    public DateTime updated_date_time { get; set; }

    public string assessment_title { get; set; }

    public string certificate_title { get; set; }
  }
}
