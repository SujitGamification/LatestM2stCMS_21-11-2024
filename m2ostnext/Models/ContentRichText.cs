// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ContentRichText
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Web.Mvc;

namespace m2ostnext.Models
{
  public class ContentRichText
  {
    public string t_title { get; set; }

    public string t_header { get; set; }

    public string t_question { get; set; }

    public int select_level { get; set; }

    public int select_category { get; set; }

    public string category_list { get; set; }

    public DateTime content_expiry { get; set; }

    public string t_metadata { get; set; }

    [AllowHtml]
    public string content_answer { get; set; }

    public int conId { get; set; }
  }
}
