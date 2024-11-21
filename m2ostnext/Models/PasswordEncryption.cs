// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.PasswordEncryption
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace m2ostnext.Models
{
  public static class PasswordEncryption
  {
    public static string ToMD5Hash(this byte[] bytes)
    {
      StringBuilder hash = new StringBuilder();
      ((IEnumerable<byte>) MD5.Create().ComputeHash(bytes)).ToList<byte>().ForEach((Action<byte>) (b => hash.AppendFormat("{0:x2}", (object) b)));
      return hash.ToString();
    }

    public static string ToMD5Hash(this string inputString) => Encoding.UTF8.GetBytes(inputString).ToMD5Hash();
  }
}
