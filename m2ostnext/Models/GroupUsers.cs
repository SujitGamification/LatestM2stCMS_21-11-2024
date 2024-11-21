// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GroupUsers
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class GroupUsers
  {
    public int id_game_group { get; set; }

    public int id_organization { get; set; }

    public string user_name { get; set; }

    public string group_status { get; set; }

    public int id_user { get; set; }

    public string userid { get; set; }

    public string user_status { get; set; }

    public GroupUsers(MySqlDataReader reader)
    {
      this.group_status = Convert.ToString(reader[nameof (group_status)]);
      this.userid = Convert.ToString(reader[nameof (userid)]);
      this.user_name = Convert.ToString(reader[nameof (user_name)]);
      this.user_status = Convert.ToString(reader[nameof (user_status)]);
      this.id_organization = Convert.ToInt32(reader[nameof (id_organization)]);
      this.id_game_group = Convert.ToInt32(reader[nameof (id_game_group)]);
      this.id_user = Convert.ToInt32(reader[nameof (id_user)]);
    }
  }
}
