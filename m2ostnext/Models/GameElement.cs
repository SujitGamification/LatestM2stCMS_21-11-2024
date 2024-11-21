// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.GameElement
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using MySql.Data.MySqlClient;
using System;

namespace m2ostnext.Models
{
  public class GameElement
  {
    public int id_game { get; set; }

    public int id_organization { get; set; }

    public string element_name { get; set; }

    public string element_type { get; set; }

    public string is_mandatory { get; set; }

    public int sequence_number { get; set; }

    public double weightage { get; set; }

    public GameElement(MySqlDataReader reader)
    {
      this.element_name = Convert.ToString(reader[nameof (element_name)]);
      this.element_type = Convert.ToString(reader[nameof (element_type)]);
      this.is_mandatory = Convert.ToString(reader[nameof (is_mandatory)]);
      this.weightage = Convert.ToDouble(reader[nameof (weightage)]);
      this.id_organization = Convert.ToInt32(reader[nameof (id_organization)]);
      this.id_game = Convert.ToInt32(reader[nameof (id_game)]);
      this.sequence_number = Convert.ToInt32(reader[nameof (sequence_number)]);
    }
  }
}
