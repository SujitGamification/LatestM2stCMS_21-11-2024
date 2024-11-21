// Decompiled with JetBrains decompiler
// Type: m2ostnext.ApplicationSignInManager
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace m2ostnext
{
  public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
  {
    public ApplicationSignInManager(
      ApplicationUserManager userManager,
      IAuthenticationManager authenticationManager)
      : base((Microsoft.AspNet.Identity.UserManager<ApplicationUser, string>) userManager, authenticationManager)
    {
    }

    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user) => user.GenerateUserIdentityAsync((Microsoft.AspNet.Identity.UserManager<ApplicationUser>) this.UserManager);

    public static ApplicationSignInManager Create(
      IdentityFactoryOptions<ApplicationSignInManager> options,
      IOwinContext context)
    {
      return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    }
  }
}
