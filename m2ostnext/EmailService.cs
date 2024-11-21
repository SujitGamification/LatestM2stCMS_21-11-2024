// Decompiled with JetBrains decompiler
// Type: m2ostnext.EmailService
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace m2ostnext
{
  public class EmailService : IIdentityMessageService
  {
    public Task SendAsync(IdentityMessage message) => (Task) Task.FromResult<int>(0);
  }
}
