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
    
    public partial class tbl_category_heading
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_category_heading()
        {
            this.tbl_category_associantion = new HashSet<tbl_category_associantion>();
        }
    
        public int id_category_heading { get; set; }
        public string Heading_title { get; set; }
        public Nullable<int> id_category_tiles { get; set; }
        public Nullable<int> heading_order { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_category_associantion> tbl_category_associantion { get; set; }
        public virtual tbl_category_tiles tbl_category_tiles { get; set; }
    }
}
