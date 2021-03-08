//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoteMarketPlace.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpamReports
    {
        public int SpamID { get; set; }
        public int NoteID { get; set; }
        public int UserID { get; set; }
        public int DownloadID { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Downloads Downloads { get; set; }
        public virtual Notes Notes { get; set; }
        public virtual SpamReports SpamReports1 { get; set; }
        public virtual SpamReports SpamReports2 { get; set; }
        public virtual Users Users { get; set; }
    }
}