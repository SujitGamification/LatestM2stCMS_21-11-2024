// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ticker
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext.Models
{
  public class ticker
  {
    public int Id_ticker { get; set; }

    public int Id_org { get; set; }

    public int Id_creator { get; set; }

    public string status { get; set; }

    public DateTime update_date { get; set; }

    public DateTime expiry_date { get; set; }

    public string ticker_news { get; set; }

    public string background_color { get; set; }

    public string font_color { get; set; }
  }
}
