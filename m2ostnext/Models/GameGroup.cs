// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GameGroup
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class GameGroup
  {
    public string group_name { get; set; }

    public int group_count { get; set; }

    public int id_game_group { get; set; }

    public GameGroup(MySqlDataReader reader)
    {
      this.group_name = Convert.ToString(reader[nameof (group_name)]);
      this.group_count = Convert.ToInt32(reader[nameof (group_count)]);
      this.id_game_group = Convert.ToInt32(reader[nameof (id_game_group)]);
    }
  }
}
