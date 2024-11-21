// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.ContentViewController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
  public class ContentViewController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult theme1(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme2(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme3(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme4(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme5(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme6(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme7(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme8(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme9(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme10(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme11(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme12(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme13(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme14(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme16(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult theme17(int id)
    {
      if (id == 0)
      {
        this.ViewData["content"] = (object) null;
        this.ViewData["answer"] = (object) null;
      }
      else
      {
        tbl_content tblContent = this.db.tbl_content.Find(new object[1]
        {
          (object) id
        });
        tbl_content_answer tblContentAnswer = this.db.tbl_content_answer.Where<tbl_content_answer>((Expression<Func<tbl_content_answer, bool>>) (t => t.ID_CONTENT == id)).FirstOrDefault<tbl_content_answer>();
        this.ViewData["content"] = (object) tblContent;
        this.ViewData["answer"] = (object) tblContentAnswer;
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep8(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep9(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep10(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep11(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep12(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep13(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep14(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep15(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep16(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep17(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep18(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep19(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep20(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep21(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep22(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep23(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep24(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep25(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep26(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }

    public ActionResult themestep27(int id, int vid)
    {
      if (id == 0)
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) new tbl_content_answer_steps();
      }
      else
      {
        this.ViewData["step-count"] = (object) id;
        this.ViewData["stepinfo"] = (object) this.db.tbl_content_answer_steps.Find(new object[1]
        {
          (object) vid
        });
      }
      return (ActionResult) this.View();
    }
  }
}
