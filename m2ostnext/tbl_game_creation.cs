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
    
    public partial class tbl_game_creation
    {
        public int id_game { get; set; }
        public Nullable<int> id_organisation { get; set; }
        public Nullable<int> id_game_creator { get; set; }
        public string gameid { get; set; }
        public string game_title { get; set; }
        public string game_description { get; set; }
        public string game_creator_name { get; set; }
        public Nullable<System.DateTime> game_expiry_date { get; set; }
        public Nullable<System.DateTime> game_start_date { get; set; }
        public string game_mode { get; set; }
        public string game_type { get; set; }
        public string id_game_path { get; set; }
        public string player_type { get; set; }
        public Nullable<int> id_game_group { get; set; }
        public string game_comment { get; set; }
        public string game_phase { get; set; }
        public Nullable<System.DateTime> game_creation_datetime { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    }
}
