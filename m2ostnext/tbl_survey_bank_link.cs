//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace m2ostnext
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_survey_bank_link
    {
        public int ID_SURVEY_BANK_LINK { get; set; }
        public int ID_SURVEY { get; set; }
        public int ID_SURVEY_BANK { get; set; }
        public string STATUS { get; set; }
        public System.DateTime UPDATED_DATE_TIME { get; set; }
    
        public virtual tbl_survey tbl_survey { get; set; }
        public virtual tbl_survey_bank tbl_survey_bank { get; set; }
    }
}
