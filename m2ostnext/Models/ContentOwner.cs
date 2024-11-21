// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ContentOwner
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

namespace m2ostnext.Models
{
  public class ContentOwner
  {
    public tbl_content Content { get; set; }

    public tbl_organization Organization { get; set; }

    public string flag { get; set; }

    public int used { get; set; }
  }
}
