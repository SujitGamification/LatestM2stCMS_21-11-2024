// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.RoleAccessController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using System.Web.Mvc;
using System.Web.Routing;

namespace m2ostnext.Controllers
{
  public class RoleAccessController : ActionFilterAttribute
  {
    public int KEY { get; set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      UserSession content = (UserSession) filterContext.HttpContext.Session.Contents["UserSession"];
      if (content == null)
        filterContext.Result = (ActionResult) new RedirectToRouteResult(new RouteValueDictionary()
        {
          {
            "Controller",
            (object) "Home"
          },
          {
            "Action",
            (object) "Index"
          }
        });
      else if (new RoleBasedAccess().checkAccess(content.action, this.KEY))
        base.OnActionExecuting(filterContext);
      else
        filterContext.Result = (ActionResult) new RedirectToRouteResult(new RouteValueDictionary()
        {
          {
            "Controller",
            (object) "Information"
          },
          {
            "Action",
            (object) "Forbidden"
          }
        });
    }
  }
}
