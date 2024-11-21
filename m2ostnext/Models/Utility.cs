// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.Utility
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Globalization;

namespace m2ostnext.Models
{
  public class Utility
  {
    public DateTime StringToDatetime(string dateString)
    {
      DateTime result = new DateTime();
      if (string.IsNullOrEmpty(dateString))
      {
        result = new DateTime(DateTime.Now.Year, 1, 1);
      }
      else
      {
        dateString = dateString.Trim();
        if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "dd-MM-yyyy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "dd-MM-yy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "MM/dd/yyyy hh:mm:tt", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "dd-MM-yyyy hh:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "yyyy-MM-dd hh:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "yyyy-dd-MM hh:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "MM/dd/yyyy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && !DateTime.TryParseExact(dateString, "MM-dd-yyyy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
          result = DateTime.ParseExact("0001-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture);
      }
      return result;
    }
  }
}
