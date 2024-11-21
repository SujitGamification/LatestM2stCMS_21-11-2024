// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.M2ostDbContext
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Data.Entity;

namespace m2ostnext.Models
{
  public class M2ostDbContext : DbContext
  {
    static M2ostDbContext() => Database.SetInitializer<M2ostDbContext>((IDatabaseInitializer<M2ostDbContext>) null);

    public M2ostDbContext()
      : base("name=dbconnectionstring")
    {
    }
  }
}
