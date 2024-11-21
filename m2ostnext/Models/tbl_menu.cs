// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.tbl_menu
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

namespace m2ostnext.Models
{
  public class tbl_menu
  {
    public int id_menu { get; set; }

    public int id_org { get; set; }

    public string menu_name { get; set; }

    public string menu_url { get; set; }

    public string menu_icon { get; set; }

    public string updated_date_time { get; set; }
  }
}
