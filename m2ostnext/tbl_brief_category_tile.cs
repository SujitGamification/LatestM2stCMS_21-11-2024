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
    
    public partial class tbl_brief_category_tile
    {
        public int id_brief_category_tile { get; set; }
        public Nullable<int> id_organization { get; set; }
        public string category_tile { get; set; }
        public string tile_code { get; set; }
        public string tile_description { get; set; }
        public string tile_image { get; set; }
        public Nullable<int> category_tile_type { get; set; }
        public Nullable<int> tile_position { get; set; }
        public Nullable<int> assessment_complation { get; set; }
        public Nullable<int> attempt_limit { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    }
}
