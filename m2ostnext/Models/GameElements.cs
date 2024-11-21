// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GameElements
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;

namespace m2ostnext.Models
{
  public class GameElements
  {
    public int id_game { get; set; }

    public int id_organization { get; set; }

    public int id_game_creator { get; set; }

    public string game_title { get; set; }

    public string game_description { get; set; }

    public string game_creator_name { get; set; }

    public DateTime game_expiry_date { get; set; }

    public string game_mode { get; set; }

    public string game_type { get; set; }

    public int id_game_path { get; set; }

    public string player_type { get; set; }

    public int id_group { get; set; }

    public string game_status { get; set; }

    public DateTime game_creation_datetime { get; set; }

    public DateTime game_update_datetime { get; set; }

    public int game_phase { get; set; }

    public string status { get; set; }

    public string game_comment { get; set; }
  }
}
