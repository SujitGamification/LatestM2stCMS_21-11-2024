// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.AndroidGCMPushNotification
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace m2ostnext.Models
{
  public class AndroidGCMPushNotification
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public string SendNotification(string deviceRegIds, string message, string head)
    {
      string str1 = "";
      try
      {
        string str2 = "AAAAQ437zAU:APA91bF3M_FesGMeb-EbndVgIQ8zhpmNTyxHSVBvhDmmvbH50OKSmRv94oLMsEoeTx-t_oyutMu_JyJ7lO09DjjpV6sHQscKVE1aOWZhUKZyHBczMXFyers-CmNlafDBK-oNRhwV_Xas";
        string str3 = "290144898053";
        WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        webRequest.Method = "post";
        webRequest.Headers.Add(string.Format("Authorization: key={0}", (object) str2));
        webRequest.Headers.Add(string.Format("Sender: id={0}", (object) str3));
        webRequest.ContentType = "application/json";
        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) new
        {
          to = deviceRegIds,
          priority = "high",
          content_available = true,
          data = new
          {
            body = message,
            title = head,
            badge = 1
          }
        }).ToString());
        webRequest.ContentLength = (long) bytes.Length;
        using (Stream requestStream = webRequest.GetRequestStream())
        {
          requestStream.Write(bytes, 0, bytes.Length);
          using (WebResponse response = webRequest.GetResponse())
          {
            using (Stream responseStream = response.GetResponseStream())
            {
              if (responseStream != null)
              {
                using (StreamReader streamReader = new StreamReader(responseStream))
                  streamReader.ReadToEnd();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Stream responseStream = ((WebException) ex).Response.GetResponseStream();
        string str4 = "";
        int num;
        do
        {
          num = responseStream.ReadByte();
          str4 += ((char) num).ToString();
        }
        while (num != -1);
        responseStream.Close();
        str1 = str4;
      }
      return str1;
    }
  }
}
