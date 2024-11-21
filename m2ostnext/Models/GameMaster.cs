// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GameMaster
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class GameMaster
  {
    public int id_game { get; set; }

    public int id_organisation { get; set; }

    public string game_title { get; set; }

    public string game_description { get; set; }

    public string game_stutus { get; set; }

    public string game_type { get; set; }

    public string game_mode { get; set; }

    public string player_type { get; set; }

    public string gameid { get; set; }

    public string start_date { get; set; }

    public string end_date { get; set; }

    public GameMaster(MySqlDataReader reader)
    {
      this.id_organisation = Convert.ToInt32(reader[nameof (id_organisation)]);
      this.id_game = Convert.ToInt32(reader[nameof (id_game)]);
      this.end_date = Convert.ToString(reader[nameof (end_date)]);
      this.start_date = Convert.ToString(reader[nameof (start_date)]);
      this.gameid = Convert.ToString(reader[nameof (gameid)]);
      this.game_description = Convert.ToString(reader[nameof (game_description)]);
      this.game_mode = Convert.ToString(reader[nameof (game_mode)]);
      this.game_stutus = Convert.ToString(reader[nameof (game_stutus)]);
      this.game_title = Convert.ToString(reader[nameof (game_title)]);
      this.game_type = Convert.ToString(reader[nameof (game_type)]);
      this.player_type = Convert.ToString(reader[nameof (player_type)]);
    }
  }
}
