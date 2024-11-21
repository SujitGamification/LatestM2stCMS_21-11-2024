// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.addCms_AssociationModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Collections.Generic;

namespace m2ostnext.Models
{
  public class addCms_AssociationModel
  {
    public List<tbl_category> tb_category { get; set; }

    public List<tbl_organization> tb_organization { get; set; }

    public List<tbl_content_level> tb_content_level { get; set; }

    public List<tbl_content> tb_content { get; set; }
  }
}
