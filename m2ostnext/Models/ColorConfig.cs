// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ColorConfig
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext.Models
{
  public class ColorConfig
  {
    public int id_color_config { get; set; }

    public int id_organisation { get; set; }

    public int config_type { get; set; }

    public string grid1_bk_color { get; set; }

    public string grid1_text_color { get; set; }

    public string grid2_bk_color { get; set; }

    public string grid2_text_color { get; set; }

    public string status { get; set; }

    public DateTime created_date_time { get; set; }

    public DateTime updated_date_time { get; set; }
  }
}
