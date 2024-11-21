// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.UserSession
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Collections.Generic;

namespace m2ostnext.Models
{
  public class UserSession
  {
    public string Username { get; set; }

    public string Roleid { get; set; }

    public string ID_USER { get; set; }

    public string id_ORGANIZATION { get; set; }

    public tbl_cms_users USER { get; set; }

    public List<tbl_cms_role_action_mapping> action { get; set; }

    public string org_status { get; set; }

    public DateTime exp_date { get; set; }
  }
}
