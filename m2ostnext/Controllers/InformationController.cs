// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.InformationController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class InformationController : Controller
  {
    public ActionResult Index() => (ActionResult) this.View();

    public ActionResult Forbidden() => (ActionResult) this.View();

    public ActionResult Error() => (ActionResult) this.View();
  }
}
