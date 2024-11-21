// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.SendCodeViewModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Collections.Generic;
using System.Web.Mvc;

namespace m2ostnext.Models
{
  public class SendCodeViewModel
  {
    public string SelectedProvider { get; set; }

    public ICollection<SelectListItem> Providers { get; set; }

    public string ReturnUrl { get; set; }

    public bool RememberMe { get; set; }
  }
}
