// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.RoleBasedAccess
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System.Collections.Generic;

namespace m2ostnext.Models
{
  public class RoleBasedAccess
  {
    public bool checkAccess(List<tbl_cms_role_action_mapping> mapping, int type)
    {
      foreach (tbl_cms_role_action_mapping roleActionMapping in mapping)
      {
        int num = type;
        int? idCmsRoleAction = roleActionMapping.id_cms_role_action;
        int valueOrDefault = idCmsRoleAction.GetValueOrDefault();
        if ((num == valueOrDefault ? (idCmsRoleAction.HasValue ? 1 : 0) : 0) != 0)
          return true;
      }
      return false;
    }
  }
}
