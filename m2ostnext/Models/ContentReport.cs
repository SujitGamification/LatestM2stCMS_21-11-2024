// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ContentReport
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Collections.Generic;

namespace m2ostnext.Models
{
  public class ContentReport
  {
    public List<usersdetails> listuserdetails = new List<usersdetails>();

    public int ID_USER { get; set; }

    public string USERID { get; set; }

    public string content_name { get; set; }

    public string orgnization_name { get; set; }

    public DateTime created_dated { get; set; }

    public DateTime expity_date { get; set; }

    public DateTime lastaccess_date { get; set; }

    public int count_accessed { get; set; }

    public string flag { get; set; }

    public int countflag { get; set; }

    public string location { get; set; }

    public string username { get; set; }
  }
}
