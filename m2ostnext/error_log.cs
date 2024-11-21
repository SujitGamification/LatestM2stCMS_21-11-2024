// Decompiled with JetBrains decompiler
// Type: m2ostnext.error_log
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext
{
  public class error_log
  {
    public int Error_ID { get; set; }

    public string Error_Message { get; set; }

    public string Error_Inner { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATEDDATETIME { get; set; }
  }
}
