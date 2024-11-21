// Decompiled with JetBrains decompiler
// Type: m2ostnext.Models.ContentLocationWise
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

namespace m2ostnext.Models
{
    public class ContentVideoCompletion
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string ContentTitle { get; set; }

        public string CategoryName { get; set; }

        public string video_timer { get; set; }

        public string total_video_timer { get; set; }

        public string complete_per { get; set; }

        public string ID { get; set; }

        public string created_Date { get; set; }

        public string CompletionStatus { get; set; }
    }

    public class ContentDropdown
    {
        public string ID_CONTENT { get; set; }

        ////public string CONTENT_TITLE { get; set; }
        public string CONTENT_QUESTION { get; set; }
    }
}
