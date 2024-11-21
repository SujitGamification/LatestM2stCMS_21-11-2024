// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.cms_content_managementModel
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System.Configuration;

namespace m2ostnext.Models
{
  public class cms_content_managementModel
  {
    private MySqlConnection conn;

    public cms_content_managementModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);
  }
}
