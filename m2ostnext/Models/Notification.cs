// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.Notification
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

namespace m2ostnext.Models
{
  public class Notification
  {
    public AndroidGCMPushNotification apnGCM;

    public Notification(string deviceid, string message, string head)
    {
      this.apnGCM = new AndroidGCMPushNotification();
      this.apnGCM.SendNotification(deviceid, message, head);
    }
  }
}
