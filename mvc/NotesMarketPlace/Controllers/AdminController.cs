﻿using System;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.Net.Mail;
using System.Text;
using NoteMarketPlace.Models;
using System.Linq;
using NoteMarketPlace.EmailTemplates;
using System.IO;
using System.Web;
using System.Data.Entity.Validation;
using System.IO.Compression;
using System.Collections.Generic;
using PagedList.Mvc;
using PagedList;
using Type = NoteMarketPlace.Models.Type;

namespace NoteMarketPlace.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize]
        public ActionResult addAdministrator()
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var countrycode = DBobj.Country.ToList();
                ViewBag.countrycode = new SelectList(countrycode, "CountryID", "CountryCode");

                return View();
            }
        }
        [HttpPost]
        public ActionResult addAdministrator(AddAdmin adm)
        {

            if (ModelState.IsValid)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    var countrycode = DBobj.Country.ToList();
                    ViewBag.countrycode = new SelectList(countrycode, "CountryID", "CountryCode");


                    int id = (int)Session["UserID"];

                    Users u = new Users();
                    u.RoleID = 1003;
                    u.FirstName = adm.FirstName;
                    u.LastName = adm.LastName;
                    u.EmailId = adm.EmailID;
                    u.Password = "Admin123&";
                    u.IsEmailVerified = true;
                    u.PhoneNoCountryCode = adm.PhoneCountryCode;
                    u.PhoneNo = adm.PhoneNo;
                    u.CreatedDate = DateTime.Now;
                    u.CreatedBy = id;
                    u.ModifiedDate = DateTime.Now;
                    u.ModifiedBy = id;
                    u.IsActive = true;

                    DBobj.Users.Add(u);
                    DBobj.SaveChanges();
                }
                   
            }
            return View();
        }
        [Authorize]
        public ActionResult addCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addCategory(AddCategory ac)
        {

            if (ModelState.IsValid)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    int id = (int)Session["UserID"];
                    Categories cat = new Categories();
                    cat.CategoryName = ac.CategoryName;
                    cat.Description = ac.CategoryDescription;
                    cat.CreatedDate = DateTime.Now;
                    cat.CreatedBy = id;
                    cat.ModifiedDate = DateTime.Now;
                    cat.ModifiedBy = id;
                    cat.IsActive = true;
                    DBobj.Categories.Add(cat);
                    DBobj.SaveChanges();
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult addCountry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addCountry(AddCountry acn)
        {

            if (ModelState.IsValid)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    int id = (int)Session["UserID"];
                    Country cn = new Country();
                    cn.CountryName = acn.CountryName;
                    cn.CountryCode = acn.CountryCode;
                    cn.CreatedDate = DateTime.Now;
                    cn.CreatedBy = id;
                    cn.ModifiedDate = DateTime.Now;
                    cn.ModifiedBy = id;
                    cn.IsActive = true;
                    DBobj.Country.Add(cn);
                    DBobj.SaveChanges();
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult addType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addType(AddType at)
        {

            if (ModelState.IsValid)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    int id = (int)Session["UserID"];
                    Models.Type tp = new Models.Type();
                    tp.TypeName = at.TypeName;
                    tp.Description = at.TypeDescription;
                    tp.CreatedDate = DateTime.Now;
                    tp.CreatedBy = id;
                    tp.ModifiedDate = DateTime.Now;
                    tp.ModifiedBy = id;
                    tp.IsActive = true;
                    DBobj.Type.Add(tp);
                    DBobj.SaveChanges();
                    ModelState.Clear();

                }
            }
            return View();
        }
        [Authorize]
        public ActionResult changepassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changePassword(ChangePassword cp)
        {
            if (ModelState.IsValid)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    int id = (int)Session["UserID"];
                    Users u = DBobj.Users.Where(x => x.UserID == id).FirstOrDefault();
                    if (u.Password == cp.OldPassword)
                    {
                        u.Password = cp.NewPassword;
                        DBobj.SaveChanges();
                        ViewBag.PassMessage = "<p><span><i class='fas fa-check-circle'></i></span> Your Password has been Changed successfully</p>";
                        return RedirectToAction("login", "Admin");
                    }

                }
            }
            return View();
        }

        //GET: Dashboard
        [Authorize]
        public ActionResult dashboard(int? page, string dashsearch, int? Month)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.LazyLoadingEnabled = false;

                var sevenDays = DateTime.Now.AddDays(-7);
                ViewBag.noteunderreview = DBobj.Notes.Where(x => x.Status == 4).Count();
                ViewBag.downlodednote = DBobj.Downloads.Where(x => x.IsSellerHasAllowedDownload == true && x.CreatedDate > sevenDays).Count();
                ViewBag.newregistration = DBobj.Users.Where(x => x.CreatedDate > sevenDays && x.RoleID == 1005).Count();

                var notes = DBobj.Notes.Where(x => x.Status == 2 && x.IsActive == true &&
                (x.NotesTitle.StartsWith(dashsearch) || dashsearch == null) && (x.NotesPublishedDateTime.Value.Month == Month || String.IsNullOrEmpty(Month.ToString()))).ToList();

                
                var dnotes = DBobj.Downloads.Where(x => x.IsSellerHasAllowedDownload == true && x.IsActive == true).ToList();

                var publishenotes = (from n in notes
                                     join ct in DBobj.Categories.ToList() on n.NotesCategory equals ct.CategoryID
                                     join usr in DBobj.Users.ToList() on n.SellerID equals usr.UserID
                                     join sn in DBobj.SellerNoteAttachment.ToList() on n.NoteID equals sn.NoteID
                                     select new MyProgressNotes
                                     {
                                         Category = ct,
                                         u = usr,
                                         SellerNotes = n,
                                         noteattach = sn
                                     }).ToList();

                ViewBag.publishednotes = publishenotes.ToPagedList(page ?? 1, 5);

                  var download =     (from n in notes
                                     join db in DBobj.Downloads.ToList() on n.NoteID equals db.NoteID
                                      where db.IsAttachmentDownloaded == true && db.AttachmentPath != null
                                     select new MyProgressNotes
                                     {
                                         SellerNotes =n,
                                         downloadnote =db
                                     }).ToList();

                ViewBag.DownloadCount = download.Count();

                return View();
            }

        }
        
        [AllowAnonymous]
        public ActionResult forgot()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult forgot(Users model)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                string allowedChars = "";
                string passwordString = "";
                string temp = "";

                bool isValid = DBobj.Users.Any(x => x.EmailId == model.EmailId);
                if (isValid)
                {
                    Users u = DBobj.Users.Where(x => x.EmailId == model.EmailId).FirstOrDefault();
                    allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
                    allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
                    allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";

                    char[] sep = { ',' };
                    string[] arr = allowedChars.Split(sep);
                    Random rand = new Random();

                    for (int i = 0; i < 6; i++)

                    {
                        temp = arr[rand.Next(0, arr.Length)];
                        passwordString += temp;
                    }

                    //  Save Password
                    u.Password = passwordString;
                    DBobj.SaveChanges();

                    //Sending new password on mail
                    ForgotEmail.SendOtpToEmail(u, passwordString);

                    TempData["SuccessMessage"] = "<p>New password sent to your registered email-address</p>";
                    return RedirectToAction("login", "Admin");
                }
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(Users model)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                    {


                        bool isValid = DBobj.Users.Any(x => x.EmailId == model.EmailId && x.Password == model.Password && x.RoleID == 1003 && x.IsActive == true);
                        if (isValid)
                        {
                            if (DBobj.Users.Any(x => x.EmailId == model.EmailId && x.IsEmailVerified == true))
                            {
                                FormsAuthentication.SetAuthCookie(model.EmailId, false);
                                Session["EmailID"] = model.EmailId;
                                var namedata = DBobj.Users.Where(x => x.EmailId == model.EmailId).FirstOrDefault();
                                Session["FirstName"] = namedata.FirstName;
                                Session["LastName"] = namedata.LastName;
                                Session["FullName"] = namedata.FirstName + " " + namedata.LastName;
                                Session["UserID"] = namedata.UserID;

                                return RedirectToAction("dashboard", "Admin");

                            }
                            else
                            {
                                TempData["Message"] = "<p>Your email is not verified, Please verify it..!</p>";
                            }

                        }
                        else
                        {
                            TempData["incorrectPass"] = "<p>The password that you've entered is incorrect </p>";
                            ViewBag.incorrectPass = "<p>The password that you've entered is incorrect </p>";
                            return View();
                        }

                    }
                    return View();
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }
       
       [Authorize]
        public ActionResult Logout()
        {
          

            FormsAuthentication.SignOut();
            
            return RedirectToAction("login");
        }
        [Authorize]
        public ActionResult manageType(int? page,string tSearch, string submit)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.LazyLoadingEnabled = false;
                int id = (int)Session["UserID"];


                List<Models.Type> typess = null;
                if (submit == "Search")
                {
                    typess = DBobj.Type.Where(x=>x.TypeName.StartsWith(tSearch) || tSearch == null).ToList();
                }
                else
                {
                    typess = DBobj.Type.ToList();
                }


                var typemanage = (from t in typess
                                  join usr in DBobj.Users on t.CreatedBy equals usr.UserID
                                  where usr.RoleID == 1003
                                  select new MyProgressNotes
                                  {
                                      types = t,
                                      u = usr

                                  }).ToList();
                ViewBag.typemanagement = typemanage.ToPagedList(page ?? 1, 5);
                ViewBag.typemanagementCount = typemanage.Count();
                return View();
            }
        }

        [Route("deleteType/typeid")]
         [Authorize]
          public ActionResult DeleteType(int? typeid)
          {
               NotesMarketPlaceEntities Dbobj = new NotesMarketPlaceEntities();
             
               var deltype = Dbobj.Type.Where(x=>x.TypeID == typeid).FirstOrDefault();

               deltype.IsActive = false;

                Dbobj.SaveChanges();
                 
                return RedirectToAction("dashboard", "Admin");
             
          } 
       
        [Authorize]
        public ActionResult manageAdministrator(int? page,string aSearch, string submit)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {

                List<Users> user = null;
                if (submit == "Search")
                {
                    user = DBobj.Users.Where(x=>x.RoleID == 1003 && ( x.FirstName.StartsWith(aSearch) || aSearch == null)).ToList();
                }
                else
                {
                    user = DBobj.Users.Where(x=>x.RoleID == 1003).ToList();
                }

                int id = (int)Session["UserID"];


                var adminmanage = (from uu in user
                                      join usr in DBobj.UserRoles on uu.RoleID equals usr.RoleID
                                      where usr.RoleID == 1003
                                      select new MyProgressNotes
                                      {
                                          u = uu,
                                          role = usr
                                      }).ToList();


                ViewBag.adminmanagement = adminmanage.ToPagedList(page ?? 1, 5);
                ViewBag.adminmanagementCount = adminmanage.Count();

                return View();
            }
        }
        [Route("deleteAdministrator/adminid")]
        public ActionResult deleteAdministrator(int? adminid)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var deltype = DBobj.Users.Where(x => x.UserID== adminid);
               
                Users u = DBobj.Users.Where(x => x.UserID == adminid).FirstOrDefault();
                   
                u.IsActive = false;
                DBobj.SaveChanges();
               return RedirectToAction("dashboard", "Admin");
            }
        }
        [Authorize]
        public ActionResult manageCategory(int? page,string cSearch, string submit)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
               
                int id = (int)Session["UserID"];


                List<Categories> cateogry = null;
                if (submit == "Search")
                {
                    cateogry = DBobj.Categories.Where(x => x.CategoryName.StartsWith(cSearch) || cSearch == null).ToList();
                }
                else
                {
                    cateogry = DBobj.Categories.ToList();
                }


                var categorymanage = (from cat in cateogry
                                      join usr in DBobj.Users on cat.CreatedBy equals usr.UserID
                                      where usr.RoleID == 1003
                                      select new MyProgressNotes
                                      {
                                          Category = cat,
                                          u = usr

                                      }).ToList();
                ViewBag.categorymanagement = categorymanage.ToPagedList(page ?? 1, 5);
                ViewBag.categorymanagementCount = categorymanage.Count();
                return View();
            }

        }

        
         
        [Route("deleteCategory/id")]
        public ActionResult deleteCategory(int? id)
        {
              NotesMarketPlaceEntities Dbobj = new NotesMarketPlaceEntities();
           
                var delcategory = Dbobj.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
              
               
                 delcategory.IsActive = false;
                 Dbobj.SaveChanges();
               
                return RedirectToAction("dashboard", "Admin");
           
        }
        public ActionResult manageCountry(int? page,string cSearch, string submit)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
               
                int id = (int)Session["UserID"];


                List<Country> countries = null;
                if (submit == "Search")
                {
                    countries = DBobj.Country.Where(x => x.CountryName.StartsWith(cSearch) || cSearch == null).ToList();
                }
                else
                {
                    countries = DBobj.Country.ToList();
                }



                var countrymanage = (from cn in DBobj.Country
                                     join usr in DBobj.Users on cn.CreatedBy equals usr.UserID
                                     where usr.RoleID == 1003
                                     select new MyProgressNotes
                                     {
                                         country = cn,
                                         u = usr

                                     }).ToList();
                ViewBag.countrymanagement = countrymanage.ToPagedList(page ?? 1, 5);
                ViewBag.countrymanagementCount = countrymanage.Count();
                return View();
            }
        }

        [Authorize]
        [Route("deleteCountry/id")]
        public ActionResult deleteCountry(int? id)
        {
            NotesMarketPlaceEntities Dbobj = new NotesMarketPlaceEntities();

            var delcountry = Dbobj.Country.Where(x => x.CountryID == id).FirstOrDefault();


            delcountry.IsActive = false;
            Dbobj.SaveChanges();

            return RedirectToAction("dashboard", "Admin");

        }
        

        public ActionResult managesystemconfiguration()
        {
            return View();
        }
        [Authorize]
        public ActionResult memberDetails(int? page,int id)
        { 
            using(NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.LazyLoadingEnabled = false;
                var country = DBobj.Country.ToList();
                var category = DBobj.Categories.ToList();
                var reference = DBobj.Reference.ToList();
                var sts = DBobj.NotesStatus.ToList();
                var profileInformation = DBobj.UsersProfile.Where(x=>x.UserID == id).ToList();

                 var members = (from pp in profileInformation
                                join us in DBobj.Users on pp.UserID equals us.UserID
                                where us.RoleID == 1005
                                join c in country on pp.Country equals c.CountryID
                                join r in reference on pp.Gender equals r.ReferenceID
                                select new MyProgressNotes
                                {
                                  userprofile = pp,
                                  u = us,
                                  country = c,
                                  refer = r


                                }).ToList();
                ViewBag.Profiledetails = members.ToPagedList(page ?? 1, 5); 
                ViewBag.ProfiledetailsCount = members.Count();

                var notes = DBobj.Notes.Where(x => x.SellerID == id).ToList();
                var downloadnotes = DBobj.Downloads.Where(x => x.Seller == id).ToList();
                ViewBag.DownloadCount = downloadnotes.Count();
                var notedetail = (from n in notes
                                  join us in DBobj.Users on n.SellerID equals us.UserID
                                  where us.RoleID == 1005
                                  join c in category on n.NotesCategory equals c.CategoryID
                                  join st in sts on n.Status equals  st.StatusID
                                  select new MyProgressNotes
                                  {
                                      SellerNotes = n,
                                      u=us,
                                      Category = c,
                                      Status = st


                                  }).ToList();

                ViewBag.NoteDetails = notedetail.ToPagedList(page ?? 1, 5);
                ViewBag.NoteDetailsCount = notedetail.Count();

                var earn = DBobj.Downloads.Where(x => x.Seller == id).ToList();
                int sum = 0;
                foreach (var i in earn)
                {
                     ViewBag.earnings = i.PurchasedPrice;
                     ViewBag.sumTotal = sum + ViewBag.earnings;
                }
               
                return View();
            }
        }
        [Authorize]
         // GET: Members
        public ActionResult members(int? page, string membersearch)
        {
                NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities();
            
                var members = DBobj.Users.Where(x => x.RoleID == 1005 && ((x.FirstName + " " + x.LastName).StartsWith(membersearch) || membersearch == null)).ToList();

                ViewBag.members = members.ToPagedList(page ?? 1, 5);
                ViewBag.membersCount = members.Count();

                return View();
            
         }

        [Route("Deactivate/id")]
        public ActionResult Deactivate(int id)
        {

            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var delmember = DBobj.Users.FirstOrDefault(x => x.UserID == id);
                Users u = DBobj.Users.Where(x => x.UserID == id).FirstOrDefault();
                if (u.IsActive == true)
                {
                    u.IsActive = false;
                    DBobj.SaveChanges();
                    return RedirectToAction("dashboard", "Admin");
                }
                return RedirectToAction("members", "Admin");
                  
            }

        }
        
        [Authorize]
        public ActionResult myProfile()
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
               
                var countrycode = DBobj.Country.ToList();
                ViewBag.countrycode = new SelectList(countrycode, "CountryID", "CountryCode");
                return View();

            }
        }
        [HttpPost]
        public ActionResult myProfile(AdminProfile profile)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                    {
                        int id = (int)Session["UserID"];

                        var countrycode = DBobj.Country.ToList();
                        ViewBag.countrycode = new SelectList(countrycode, "CountryID", "CountryCode");
                        int p = DBobj.UsersProfile.Where(x => x.UserID == id).Count();
                        if (p > 0)
                        {
                            UsersProfile pr = DBobj.UsersProfile.Where(x => x.UserID == id).FirstOrDefault();
                            pr.UserID = id;
                            pr.FirstName = profile.FirstName;
                            pr.LastName = profile.LastName;
                            pr.EmailID = profile.EmailId;
                            pr.Gender = 2;
                            pr.PhoneCountryCode = profile.CountryCode;
                            pr.PhoneNumber = profile.PhoneNo;
                            pr.Country = 2;
                            pr.SecondaryEmailAddress = profile.SecondaryEmailAddress;
                            pr.CreatedDate = DateTime.Now;
                            pr.CreatedBy = id;
                            pr.ModifiedBy = id;
                            pr.ModifiedDate = DateTime.Now;
                            pr.IsActive = true;
                            DBobj.UsersProfile.Add(pr);
                            DBobj.SaveChanges();

                            string path = Path.Combine(Server.MapPath("~/Member/" + Session["UserID"].ToString()));

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            if (profile.ProfilePic != null && profile.ProfilePic.ContentLength > 0)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(profile.ProfilePic.FileName);
                                string extension = Path.GetExtension(profile.ProfilePic.FileName);
                                fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                                string finalpath = Path.Combine(path, fileName);
                                profile.ProfilePic.SaveAs(finalpath);

                                pr.ProfilePic = Path.Combine(("/Member/" + Session["UserID"].ToString()) + "/", fileName);
                                DBobj.SaveChanges();
                            }

                        }


                        else
                        {

                            UsersProfile pr = new UsersProfile();
                            pr.UserID = id;
                            pr.FirstName = profile.FirstName;
                            pr.LastName = profile.LastName;
                            pr.EmailID = profile.EmailId;
                            pr.Gender = 2;
                            pr.PhoneCountryCode = profile.CountryCode;
                            pr.PhoneNumber = profile.PhoneNo;
                            pr.Country = 2;
                            pr.SecondaryEmailAddress = profile.SecondaryEmailAddress;
                            pr.CreatedDate = DateTime.Now;
                            pr.CreatedBy = id;
                            pr.ModifiedBy = id;
                            pr.ModifiedDate = DateTime.Now;
                            pr.IsActive = true;
                            DBobj.UsersProfile.Add(pr);
                            DBobj.SaveChanges();
                            string path = Path.Combine(Server.MapPath("~/Member/" + Session["UserID"].ToString()));

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            if (profile.ProfilePic != null && profile.ProfilePic.ContentLength > 0)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(profile.ProfilePic.FileName);
                                string extension = Path.GetExtension(profile.ProfilePic.FileName);
                                fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                                string finalpath = Path.Combine(path, fileName);
                                profile.ProfilePic.SaveAs(finalpath);

                                pr.ProfilePic = Path.Combine(("/Member/" + Session["UserID"].ToString() + "/"), fileName);
                                DBobj.SaveChanges();
                                return RedirectToAction("dashboard", "Admin");
                            }
                        }
                    }

                        ModelState.Clear();
                        return RedirectToAction("dashboard", "Admin");
                 }   
                

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return View();
        }
        //GET: Notedetails
        [Route("noteDetails/{id}")]
        public ActionResult noteDetails(int id)
        {
           
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {

                    var ni = DBobj.Notes.Where(x => x.NoteID == id).FirstOrDefault();
                    Categories noteCategory = DBobj.Categories.Find(ni.NotesCategory);
                    ViewBag.Category = noteCategory.CategoryName;
                    Country country = DBobj.Country.Find(ni.Country);
                    ViewBag.Country = country.CountryName;

                    var reviewdetail = (from nr in DBobj.UsersReviews
                                        join n in DBobj.Notes on nr.NoteID equals n.NoteID
                                        join us in DBobj.Users on nr.ReviewedBy equals us.UserID
                                        orderby nr.CreatedDate descending
                                        select new MyProgressNotes
                                        {
                                            SellerNotes = n,
                                            review = nr,
                                            u = us
                                        }).Take(3).ToList();

                    ViewBag.reviewdetailbag = reviewdetail;
                    ViewBag.reviewcount = reviewdetail.Count();
                    ViewBag.ratingCount = DBobj.UsersReviews.Where(x => x.NoteID == id).Select(x => x.Ratings).Count();

                    if (ViewBag.ratingcount > 0)
                    {
                        ViewBag.ratingSum = DBobj.UsersReviews.Where(x => x.NoteID == id).Select(x => x.Ratings).Sum();
                    }
                    else
                    {
                        ViewBag.ratingSum = "No Review Found !";
                    }

                    ViewBag.spamtotalcount = DBobj.SpamReports.Where(x => x.NoteID == id).Count();

                    return View(ni);
                }
              
        }
   
        [Authorize]
        public ActionResult notesUnderReview(int? page,string uSearch, string sellerID)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {

                var seller = DBobj.Users.Where(x => x.RoleID == 1005).ToList();
                ViewBag.seller = new SelectList(seller, "UserID", "FirstName");

                DBobj.Configuration.LazyLoadingEnabled = false;

                

                var underReview = (from n in DBobj.Notes.Where(x=>x.Status == 4 || x.Status == 5 && (x.NotesTitle.StartsWith(uSearch) || uSearch == null))
                                   join us in DBobj.Users on n.SellerID equals us.UserID
                                   join cat in DBobj.Categories on n.NotesCategory equals cat.CategoryID
                                   join usr in DBobj.Users on n.SellerID equals usr.UserID
                                   join st in DBobj.NotesStatus on n.Status equals st.StatusID
                                   where(n.SellerID.ToString() == sellerID || String.IsNullOrEmpty(sellerID))
                                   select new MyProgressNotes
                                  {
                                     SellerNotes = n,
                                     Category = cat,
                                     u = usr,
                                     Status =st
                                   }).ToList();
                ViewBag.notesUnderReview = underReview.ToPagedList(page ?? 1, 5);
                ViewBag.notesUnderReviewCount = underReview.Count();
                return View();
            }
        }
        [Route("ConfirmApproval/noteid")]
        public ActionResult ConfirmApproval(int? noteid)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                int id = (int)Session["UserID"];
                Notes n = DBobj.Notes.Where(x => x.NoteID == noteid).FirstOrDefault();
                if (n.Status == 4 || n.Status == 5)
                {
                    n.Status = 2;
                    n.NotesPublishedDateTime = DateTime.Now;
                    n.ActionedBy = id;
                    DBobj.SaveChanges();
                }
                    return RedirectToAction("notesUnderReview", "Admin");
            }
        }
      
        public ActionResult RejectNotes()
        {
            return View();
        }
        [HttpPost]
        [Route("RejectNotes/noteid")]
        public ActionResult RejectNotes(RejectModel model, int? noteid)
        {
            if(ModelState.IsValid)
            {
                using(NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    int id = (int)Session["UserID"];
                    Notes n = DBobj.Notes.Where(x => x.NoteID == noteid).FirstOrDefault();
                    if (n.Status == 4 || n.Status == 5)
                    {
                        n.Status = 3;
                        n.AdminRemarks = model.Remarks;
                        n.ActionedBy = id;
                        n.ModifiedDate = DateTime.Now;
                        n.ModifiedBy = id;
                        DBobj.SaveChanges();
                    }
                  

                }
                return RedirectToAction("notesUnderReview", "Admin");
            }
            return RedirectToAction("notesUnderReview", "Admin");
        }
        [Route("InReview/noteid")]
        public ActionResult InReview(int? noteid)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                Notes n = DBobj.Notes.Where(x => x.NoteID == noteid).FirstOrDefault();
                if (n.Status == 4)
                {
                    n.Status = 5;
                    DBobj.SaveChanges();
                }
                return RedirectToAction("notesUnderReview","Admin");
            }
        }

        [Authorize]
        public ActionResult publishedNotes(int? page,string pSearch,string sellerID, string submit)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.LazyLoadingEnabled = false;

                var seller = DBobj.Users.Where(x=>x.RoleID == 1005).ToList();
                ViewBag.seller = new SelectList(seller, "UserID", "FirstName");

               
                var publish = (from n in DBobj.Notes.Where(x => x.Status == 2 && x.IsActive == true && (x.NotesTitle.StartsWith(pSearch) || pSearch == null))
                               join us in DBobj.Users on n.SellerID equals us.UserID
                               join cat in DBobj.Categories on n.NotesCategory equals cat.CategoryID
                               join usr in DBobj.Users on n.SellerID equals usr.UserID
                               join st in DBobj.NotesStatus on n.Status equals st.StatusID
                               join nat in DBobj.SellerNoteAttachment on n.NoteID equals nat.NoteID
                               where (n.SellerID.ToString() == sellerID || String.IsNullOrEmpty(sellerID))
                               select new MyProgressNotes
                               {
                                   SellerNotes = n,
                                   Category = cat,
                                   u = usr,
                                   Status = st,
                                   noteattach = nat
                               }).ToList();
                ViewBag.publishNotes = publish.ToPagedList(page ?? 1, 5);
                ViewBag.publishNotesCount = publish.Count();

               

                var approvedby = (from n in DBobj.Notes
                                  join us in DBobj.Users on n.ActionedBy equals us.UserID
                                  where n.Status == 2
                                  select new MyProgressNotes
                                  {
                                      u = us,
                                      SellerNotes = n
                                  }).FirstOrDefault();

                ViewBag.ApprovedBy = approvedby.u.FirstName + " "+ approvedby.u.LastName;


                return View();
            }
        }

        [Route("DownloadFile/id")]
        public ActionResult DownloadFile(int id)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                SellerNoteAttachment snt = DBobj.SellerNoteAttachment.Where(x => x.NoteID == id).FirstOrDefault();
       

                    //Return files

                    var filesPath = snt.FilePath.Split(';');
                    var filesName = snt.FileName.Split(';');
                    using (var ms = new MemoryStream())
                    {

                        using (var z = new ZipArchive(ms, ZipArchiveMode.Create, true))
                        {
                            foreach (var FilePath in filesPath)
                            {
                                string FullPath = Path.Combine(Server.MapPath(FilePath));
                                string FileName = Path.GetFileName(FullPath);
                                if (FileName == "DownloadFile")
                                {
                                    continue;
                                }
                                else
                                {
                                    z.CreateEntryFromFile(FullPath, FileName);
                                }
                            }
                        }
                        return File(ms.ToArray(), "application/zip", "Attachement.zip");
                    }
                }
              
            }
        
        public ActionResult NumberofDownloads(int ? page,int? id)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.LazyLoadingEnabled = false;

                var downloading = DBobj.Downloads.Where(x => x.NoteID == id).ToList();

                var download = (from down in downloading
                                join uss in DBobj.Users on down.Downloader equals uss.UserID
                                where down.IsSellerHasAllowedDownload == true && down.IsActive == true
                                join cat in DBobj.Categories on down.NoteCategory equals cat.CategoryID
                                join nat in DBobj.SellerNoteAttachment on down.NoteID equals nat.NoteID
                                select new MyProgressNotes
                                {
                                    downloadnote = down,
                                    u = uss,
                                    Category = cat,
                                    noteattach = nat


                                }).ToList();

                ViewBag.DownloadedNotes = download.ToPagedList(page ?? 1, 5);
                ViewBag.DownloadedNotesCount = download.Count();
                return View();
            }
        }
        [Route("Unpublish/noteid")]
        public ActionResult Unpublish(int? noteid)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                int userid = (int)Session["UserID"];
                var note = DBobj.Notes.FirstOrDefault(x => x.NoteID == noteid && x.Status == 2);
                if (note != null)
                {
                    Notes n = new Notes();
                    n.Status = 6;
                    n.ModifiedDate = DateTime.Now;
                    n.ModifiedBy = userid;
                    DBobj.SaveChanges();
                }
               
                var unpublish = DBobj.Notes.Where(x => x.NoteID == noteid && x.Status == 2).FirstOrDefault();
                int sellerId = unpublish.SellerID;
                var sellerdetail = DBobj.Users.Where(x => x.UserID == sellerId);
                var notedetail = DBobj.Notes.Where(x => x.NoteID == noteid && x.Status == 2).ToList();
                foreach (var i in sellerdetail)
                {
                    ViewBag.SellerName = i.FirstName + " " + i.LastName;
                    ViewBag.EmailId = i.EmailId;
                }
                foreach (var i in notedetail)
                {
                    ViewBag.NoteTitle = i.NotesTitle;
                    ViewBag.Remarks = i.AdminRemarks;
                }
                UnpublishNotify.UnPublishing(ViewBag.SellerName, ViewBag.NoteTitle, ViewBag.Remarks, ViewBag.EmailId);
                return RedirectToAction("dashboard", "Admin");
            }
        }
        //GET: DownloadNotes
        [Authorize]
          public ActionResult downloadNotes(int? page,string Note, string Seller, string Buyer, string dnsearch)
           {
               using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
               { 
                   var download = (from dn in DBobj.Downloads
                                   join n in DBobj.Notes.Where(x => x.NotesTitle.StartsWith(dnsearch) || dnsearch == null) on dn.NoteID equals n.NoteID
                                   where (dn.IsSellerHasAllowedDownload == true && dn.AttachmentPath != null )
                                   && (n.NotesTitle == Note || String.IsNullOrEmpty(Note))
                                   join nc in DBobj.Categories on n.NotesCategory equals nc.CategoryID
                                   join usr in DBobj.Users on dn.Seller equals usr.UserID
                                   where (usr.FirstName == Seller || String.IsNullOrEmpty(Seller))
                                   join usr1 in DBobj.Users on dn.Downloader equals usr1.UserID
                                   where (usr1.FirstName == Buyer || String.IsNullOrEmpty(Buyer))
                                   select new MyProgressNotes
                                   {
                                       u = usr,
                                       SellerNotes = n,
                                       Category = nc,
                                       u1 = usr1,
                                       downloadnote = dn

                                   }).ToList();

                   ViewBag.DownloadedNotes = download.ToPagedList(page ?? 1, 5);
                   ViewBag.DownloadedNotesCount = download.Count();


                ViewBag.dnSellers = new SelectList(DBobj.Users.Where(x => x.RoleID == 1005).ToList(), "FirstName", "FirstName");
                ViewBag.dnBuyers = new SelectList(DBobj.Users.Where(x => x.RoleID == 1005).ToList(), "FirstName", "FirstName");
                ViewBag.dnNote = new SelectList(DBobj.Notes.ToList(), "NotesTitle", "NotesTitle");
                return View();
               }
           } 

        //GET: DownloadNotes
        [Authorize]
        public ActionResult rejectednotes(int? page,string rSearch, string sellerID, string submit)
        {
          using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
         {
           var seller = DBobj.Users.Where(x => x.RoleID == 1005).ToList();
           ViewBag.seller = new SelectList(seller, "UserID", "FirstName");

           DBobj.Configuration.LazyLoadingEnabled = false;         


           var reject = (from n in DBobj.Notes.Where(x => x.Status == 3 && (x.NotesTitle.StartsWith(rSearch) || rSearch == null))
                         join us in DBobj.Users on n.SellerID equals us.UserID
                          join cat in DBobj.Categories on n.NotesCategory equals cat.CategoryID
                          join usr in DBobj.Users on n.SellerID equals usr.UserID
                          join st in DBobj.NotesStatus on n.Status equals st.StatusID
                         where(n.SellerID.ToString() == sellerID || String.IsNullOrEmpty(sellerID))
                          select new MyProgressNotes
                          {
                              SellerNotes = n,
                              Category = cat,
                              u = usr,
                              Status = st

                          }).ToList();
           ViewBag.rejectNotes = reject.ToPagedList(page ?? 1, 5);
           ViewBag.rejectNotesCount = reject.Count();


           var remarkedby = (from n in DBobj.Notes
                             join us in DBobj.Users on n.ActionedBy equals us.UserID
                             where n.Status == 3
                             select new MyProgressNotes
                             {
                                 u = us,
                                 SellerNotes = n
                             }).FirstOrDefault();
                if (remarkedby != null)
                {

                    ViewBag.RemarkedBy = remarkedby.u.FirstName + remarkedby.u.LastName;
                }
               
           return View();
       }
   }

   [Route("Approve/id")]
   public ActionResult Approve(int? id)
   {
       using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
       {
           int userid = (int)Session["UserID"];
           var note = DBobj.Notes.FirstOrDefault(x => x.NoteID == id && x.Status == 3);
           if (note != null)
           {

               note.Status = 2;
               note.ModifiedDate = DateTime.Now;
               note.ModifiedBy = userid;
               DBobj.SaveChanges();
           }
           return RedirectToAction("dashboard", "Admin");
       }

   }
    [Authorize]
   public ActionResult spamreports(int? page,string spSearch)
   {
        NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities();
       
                DBobj.Configuration.LazyLoadingEnabled = false;
               
                var spamReports = (from sr in DBobj.SpamReports
                                   join usr in DBobj.Users on sr.UserID equals usr.UserID
                                   join n in DBobj.Notes on sr.NoteID equals n.NoteID
                                   where n.IsActive == true && (n.NotesTitle.StartsWith(spSearch) || spSearch == null)
                                   join c in DBobj.Categories on n.NotesCategory equals c.CategoryID
                                   select new MyProgressNotes
                                   {
                                       Category = c,
                                       u = usr,
                                       spam = sr,
                                       SellerNotes = n
                                   }).ToList();

           ViewBag.SpamReports = spamReports.ToPagedList(page ?? 1, 5);
           ViewBag.SpamReportsCount = spamReports.Count();
           return View();
      
   }

   [Route("DeleteNotes/id")]
   public ActionResult DeleteNotes(int? id)
   {
       using (NotesMarketPlaceEntities Dbobj = new NotesMarketPlaceEntities())
       {
           var delnote = Dbobj.SellerNoteAttachment.FirstOrDefault(x => x.NoteID == id);
           var note = Dbobj.Notes.FirstOrDefault(x => x.NoteID == id);
           var downote = Dbobj.Downloads.FirstOrDefault(x => x.NoteID == id);
           var review = Dbobj.UsersReviews.FirstOrDefault(x => x.NoteID == id);
           var spam = Dbobj.SpamReports.FirstOrDefault(x => x.NoteID == id);
            if((delnote !=null && note!= null && downote!=null) ||(review !=null || spam!=null))
            { 
               Dbobj.SellerNoteAttachment.Remove(delnote);
               Dbobj.Notes.Remove(note);
               Dbobj.Downloads.Remove(downote);
               Dbobj.UsersReviews.Remove(review);
               Dbobj.SpamReports.Remove(spam);

               Dbobj.SaveChanges();
            }
           return RedirectToAction("dashboard", "Admin");
       }
   }

}
}
 