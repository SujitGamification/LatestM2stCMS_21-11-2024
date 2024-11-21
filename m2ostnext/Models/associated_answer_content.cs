// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.associated_answer_content
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Collections.Generic;

namespace m2ostnext.Models
{
  public class associated_answer_content
  {
    public tbl_content_answer Answer { get; set; }

    public List<tbl_content_type> Type { get; set; }
  }
}
