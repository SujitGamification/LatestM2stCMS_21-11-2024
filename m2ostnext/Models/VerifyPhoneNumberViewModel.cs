// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.VerifyPhoneNumberViewModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.ComponentModel.DataAnnotations;

namespace m2ostnext.Models
{
  public class VerifyPhoneNumberViewModel
  {
    [Required]
    [Display(Name = "Code")]
    public string Code { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
  }
}
