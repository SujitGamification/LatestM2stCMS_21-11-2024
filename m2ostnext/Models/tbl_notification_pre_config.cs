// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.tbl_notification_pre_config
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext.Models
{
  public class tbl_notification_pre_config
  {
    public int id_notification_config { get; set; }

    public int? id_notification { get; set; }

    public int? id_creater { get; set; }

    public string notification_key { get; set; }

    public string notification_action_type { get; set; }

    public int? id_user { get; set; }

    public int? id_content { get; set; }

    public int? id_category { get; set; }

    public int? id_assessment { get; set; }

    public string user_type { get; set; }

    public DateTime? read_timestamp { get; set; }

    public DateTime? start_date { get; set; }

    public DateTime? end_date { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
