// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.NotificationManager
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using System;
using System.IO;
using System.Net;
using System.Text;

namespace m2ostnext.Models
{
  public class NotificationManager
  {
    public string SendNotification(string deviceRegIds, string message)
    {
      string str1 = "AIzaSyDv-klhBu-VjOZTL3adZPeOejB-DlLiMFs";
      string str2 = "290144898053";
      string str3 = string.Join("\",\"", new string[1]
      {
        deviceRegIds
      });
      WebRequest webRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
      webRequest.Method = "post";
      webRequest.ContentType = "application/json";
      webRequest.Headers.Add(string.Format("Authorization: key={0}", (object) str1));
      webRequest.Headers.Add(string.Format("Sender: id={0}", (object) str2));
      byte[] bytes = Encoding.UTF8.GetBytes("{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + message + ",\"time\": \"" + DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + str3 + "\"]}");
      webRequest.ContentLength = (long) bytes.Length;
      Stream requestStream = webRequest.GetRequestStream();
      requestStream.Write(bytes, 0, bytes.Length);
      requestStream.Close();
      WebResponse response = webRequest.GetResponse();
      Stream responseStream = response.GetResponseStream();
      StreamReader streamReader = new StreamReader(responseStream);
      string end = streamReader.ReadToEnd();
      streamReader.Close();
      responseStream.Close();
      response.Close();
      return end;
    }
  }
}
