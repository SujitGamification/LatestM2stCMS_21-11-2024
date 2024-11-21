// Decompiled with JetBrains decompiler
// Type: m2ostnext.ApplicationUserManager
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Data.Entity;

namespace m2ostnext
{
  public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
      : base(store)
    {
    }

    public static ApplicationUserManager Create(
      IdentityFactoryOptions<ApplicationUserManager> options,
      IOwinContext context)
    {
      ApplicationUserManager manager = new ApplicationUserManager((IUserStore<ApplicationUser>) new UserStore<ApplicationUser>((DbContext) context.Get<ApplicationDbContext>()));
      ApplicationUserManager applicationUserManager1 = manager;
      Microsoft.AspNet.Identity.UserValidator<ApplicationUser> userValidator = new Microsoft.AspNet.Identity.UserValidator<ApplicationUser>((UserManager<ApplicationUser, string>) manager);
      userValidator.AllowOnlyAlphanumericUserNames = false;
      userValidator.RequireUniqueEmail = true;
      applicationUserManager1.UserValidator = (IIdentityValidator<ApplicationUser>) userValidator;
      manager.PasswordValidator = (IIdentityValidator<string>) new Microsoft.AspNet.Identity.PasswordValidator()
      {
        RequiredLength = 6,
        RequireNonLetterOrDigit = true,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true
      };
      manager.UserLockoutEnabledByDefault = true;
      manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5.0);
      manager.MaxFailedAccessAttemptsBeforeLockout = 5;
      ApplicationUserManager applicationUserManager2 = manager;
      PhoneNumberTokenProvider<ApplicationUser> provider1 = new PhoneNumberTokenProvider<ApplicationUser>();
      provider1.MessageFormat = "Your security code is {0}";
      applicationUserManager2.RegisterTwoFactorProvider("Phone Code", (IUserTokenProvider<ApplicationUser, string>) provider1);
      ApplicationUserManager applicationUserManager3 = manager;
      EmailTokenProvider<ApplicationUser> provider2 = new EmailTokenProvider<ApplicationUser>();
      provider2.Subject = "Security Code";
      provider2.BodyFormat = "Your security code is {0}";
      applicationUserManager3.RegisterTwoFactorProvider("Email Code", (IUserTokenProvider<ApplicationUser, string>) provider2);
      manager.EmailService = (IIdentityMessageService) new m2ostnext.EmailService();
      manager.SmsService = (IIdentityMessageService) new m2ostnext.SmsService();
      IDataProtectionProvider protectionProvider = options.DataProtectionProvider;
      if (protectionProvider != null)
        manager.UserTokenProvider = (IUserTokenProvider<ApplicationUser, string>) new DataProtectorTokenProvider<ApplicationUser>(protectionProvider.Create("ASP.NET Identity"));
      return manager;
    }
  }
}
