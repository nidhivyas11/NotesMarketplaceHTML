using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class MyProgressNotes
    {
        public Notes SellerNotes { get; set; }
        public Categories Category { get; set; }
        public NotesStatus Status { get; set; }
    }
    public class MyPublishNotes
    {
        public Notes SellerNotes { get; set; }
        public Categories Category { get; set; }
    }

    public class nd
    {
        public Notes note { get; set; }
        public Categories Category { get; set; }
        public Country contryname { get; set; }
    }

    public class countrytable
    {
        public Notes note { get; set; }
        public Country c { get; set; }
    }
    

    public class userprofiledata
    {
        public Notes note { get; set; }
        public Users u { get; set; }
        public UsersProfile upd { get; set; }
    }

    public class reviewtable
    {
        public Notes note { get; set; }
        public UsersReviews notereview { get; set; }
        public Users usr { get; set; }
    }
    public class spamtable
    {
        public Notes note { get; set; }
        public SpamReports spamrpt { get; set; }
    }
}


