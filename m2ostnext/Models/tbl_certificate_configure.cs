// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.tbl_certificate_configure
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext.Models
{
  public class tbl_certificate_configure
  {
    public int id_certificate { get; set; }

    public string certificate_title { get; set; }

    public string signature { get; set; }

    public string label_1 { get; set; }

    public string label_2 { get; set; }

    public string label_3 { get; set; }

    public DateTime updated_date_time { get; set; }
  }
}
