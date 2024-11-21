// Decompiled with JetBrains decompiler
// Type: m2ostnext.RouteConfig
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Web.Mvc;
using System.Web.Routing;

namespace m2ostnext
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
      routes.MapRoute("Default", "{controller}/{action}/{id}", (object) new
      {
        controller = "Home",
        action = "Index",
        id = UrlParameter.Optional
      });
    }
  }
}
