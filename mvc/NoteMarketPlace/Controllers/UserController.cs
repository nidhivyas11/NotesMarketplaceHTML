using System;
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

namespace NoteMarketPlace.Controllers
{
    public class UserController : Controller
    {

        public ActionResult home()
        {
            return View();
        }

        //GET: signUp
        public ActionResult signUp()
        {
            return View();
        }
        //POST: signUp
        [AllowAnonymous]
        [HttpPost]
        public ActionResult signUp(signUp model)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                    {
                        Users u = new Users();
                        u.RoleID = 1003;
                        u.FirstName = model.FirstName;
                        u.LastName = model.LastName;
                        u.EmailId = model.EmailId;
                        u.Password = model.Password;
                        u.IsEmailVerified = false;
                        u.CreatedDate = DateTime.Now;
                        u.IsActive = true;

                        DBobj.Users.Add(u);
                        DBobj.SaveChanges();

                        if (u.UserID > 0)
                        {
                            ModelState.Clear();
                            ViewBag.IsSuccess = "<p><span><i class='fas fa-check-circle'></i></span> Your account has been successfully created </p>";
                            TempData["name"] = model.FirstName;


                              //  Email Verification Link
                              var activationCode = model.Password;
                              var verifyUrl = "/User/VerifyAccount/" + activationCode;
                              var activationlink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);


                              // Sending Email
                              EmailVerification.SendVerifyLinkEmail(u, activationlink);
                              ViewBag.Title = "NotesMarketPlace"; 
                              @TempData["UserName"] = model.FirstName.ToString();


                              return RedirectToAction("emailverification", "User"); 
                        }
                    }

                }

                return View();
            }
            catch (Exception e)
            {
                return View();
            }

        }

        //VerifyEmail
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.ValidateOnSaveEnabled = false; //avoid confirm password doesn't match issue on save changes
                var ema = DBobj.Users.Where(x => x.Password == id).FirstOrDefault();
                if (ema != null)
                {
                    ema.IsEmailVerified = true;
                    DBobj.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }

            TempData["Message"] = "Your Email Is Verified You Can Login Here";
            return RedirectToAction("login", "User");
        }


        //GET: login
        public ActionResult login()
        {
            return View();
        }
        //POST: login
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


                        bool isValid = DBobj.Users.Any(x => x.EmailId == model.EmailId && x.Password == model.Password);
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
                                if (DBobj.UsersProfile.Any(x => x.EmailID == model.EmailId))
                                {
                                    return RedirectToAction("dashboard", "User");
                                }
                                else
                                {
                                    return RedirectToAction("userProfile", "User");
                                }
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


        //GET: Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }


        //GET: emailVerification
        public ActionResult emailVerification(string Name, string Email)
        {
            return View();
        }


        //GET: forgotPassword
        [AllowAnonymous]
        public ActionResult forgot()
        {
            return View();
        }
        //POST: forgotPassword
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
                    return RedirectToAction("Login", "User");
                }
                return View();
            }
        }


        //GET: addNote
        public ActionResult addNote()
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var notecategory = DBobj.Categories.ToList();
                var notetype = DBobj.Type.ToList();
                var country = DBobj.Country.ToList();
                ViewBag.notecategoies = new SelectList(notecategory, "CategoryID", "CategoryName");
                ViewBag.notetypes = new SelectList(notetype, "TypeID", "TypeName");
                ViewBag.countries = new SelectList(country, "CountryID", "CountryName");
                return View();
            }
           
        }
        //POST: addNote
        [HttpPost]
        public ActionResult addNote(addnote notedetails)
            {
            try {
                if (ModelState.IsValid)
                {
                    if (notedetails.SellType == "Paid" && notedetails.PreviewUpload == null)
                    {
                        NotesMarketPlaceEntities nm = new NotesMarketPlaceEntities();
                        ViewBag.previewmessage = "Note Preview Is Required For Paid Note...";
                        var notecategory = nm.Categories.ToList();
                        var notetype = nm.Type.ToList();
                        var country = nm.Country.ToList();
                        ViewBag.notecategoies = new SelectList(notecategory, "CategoryID", "CategoryName");
                        ViewBag.notetypes = new SelectList(notetype, "TypeID", "TypeName");
                        ViewBag.countries = new SelectList(country, "CountryID", "CountryName");
                        return View();


                    }
                

                    using (NotesMarketPlaceEntities nm = new NotesMarketPlaceEntities())
                    {
                        Notes nd = new Notes();
                        SellerNoteAttachment sna = new SellerNoteAttachment();
                        nd.SellerID = (int)Session["UserID"];
                        nd.NotesTitle = notedetails.NoteTitle;
                        nd.NotesCategory = notedetails.NoteCategoryID;
                        nd.NotesType = notedetails.NoteTypeID;
                        nd.UniversityInformation = notedetails.UniversityInformation;
                        nd.Country = notedetails.CountryID;
                        nd.CourseName = notedetails.Course;
                        nd.CourseCode = notedetails.CourseCode;
                        nd.Professor = notedetails.ProfessorName;
                        nd.SellType = notedetails.SellType;
                        nd.SellPrice = notedetails.SellPrice;
                        nd.NoOfPages = notedetails.NumberOfPages;
                        nd.Description = notedetails.NoteDescription;
                        nd.Status = 4;
                        nd.CreatedDate = DateTime.Now;
                        nd.IsActive = true;
                        nm.Notes.Add(nd);
                        nm.SaveChanges();

                        string path = Path.Combine(Server.MapPath("~/Member/" + Session["UserID"].ToString()), nd.NoteID.ToString());

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (notedetails.DisplayPicture != null && notedetails.DisplayPicture.ContentLength > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(notedetails.DisplayPicture.FileName);
                            string extension = Path.GetExtension(notedetails.DisplayPicture.FileName);
                            fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                            string finalpath = Path.Combine(path, fileName);
                            notedetails.DisplayPicture.SaveAs(finalpath);

                            nd.NotesDp = Path.Combine(("~/Member/" + Session["UserID"].ToString() + "/" + nd.NoteID.ToString() + "/"), fileName);
                            nm.SaveChanges();
                        }
                        if (notedetails.PreviewUpload != null && notedetails.PreviewUpload.ContentLength > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(notedetails.PreviewUpload.FileName);
                            string extension = Path.GetExtension(notedetails.PreviewUpload.FileName);
                            fileName = "PREVIEW_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                            string finalpath = Path.Combine(path, fileName);
                            notedetails.PreviewUpload.SaveAs(finalpath);

                            nd.PreviewUpload = Path.Combine(("~/Member/" + Session["UserID"].ToString() + "/" + nd.NoteID.ToString() + "/"), fileName);
                            nm.SaveChanges();
                        }

                        string attachmentpath = Path.Combine(Server.MapPath("~/Member/" + Session["UserID"].ToString() + "/" + nd.NoteID.ToString()), "attachment");

                        if (!Directory.Exists(attachmentpath))
                        {
                            Directory.CreateDirectory(attachmentpath);
                        }
                        if (notedetails.NotesAttachment != null && notedetails.NotesAttachment[0].ContentLength > 0)
                        {
                            var count = 1;
                            var FilePath = "";
                            var FileName = "";
                            foreach (var file in notedetails.NotesAttachment)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                string extension = Path.GetExtension(file.FileName);
                                fileName = "Attachment_" + count + "_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                                string finalpath = Path.Combine(attachmentpath, fileName);
                                file.SaveAs(finalpath);
                                FileName += fileName + ";";
                                FilePath += Path.Combine(("/Member/" + Session["UserID"].ToString() + "/" + nd.NoteID.ToString() + "/attachment/"), fileName) + ";";
                                count++;
                            }
                            sna.FileName = FileName;
                            sna.FilePath = FilePath;

                            sna.NoteID = nd.NoteID;
                            sna.IsActive = true;
                            sna.CreatedDate = DateTime.Now;
                            sna.CreatedBy = (int)Session["UserID"];
                            nm.SaveChanges();
                        }
                        nm.SellerNoteAttachment.Add(sna);
                        nm.SaveChanges();
                    }
                }
                ViewBag.SellerName = Session["FullName"];
                NotifyAdmin.sendNotification(ViewBag.SellerName);
                    ModelState.Clear();
                    return RedirectToAction("addNote", "User");


                }
                 catch (DbEntityValidationException e)
                {
                   Console.WriteLine(e);
                }

            return View();
            }


        
        
        
        public ActionResult searchNotes(string search, string NoteTypeID, string NoteCategoryID, string UniversityInformation, string CountryID, string Course) 
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var notecategory = DBobj.Categories.ToList();
                var notetype = DBobj.Type.ToList();
                var country = DBobj.Country.ToList();
                var university = DBobj.Notes.ToList();
                var course = DBobj.Notes.ToList();
                ViewBag.notecategoies = new SelectList(notecategory.Distinct(), "CategoryID", "CategoryName");
                ViewBag.notetypes = new SelectList(notetype.Distinct(), "TypeID", "TypeName");
                ViewBag.countries = new SelectList(country.Distinct(), "CountryID", "CountryName");
                ViewBag.universities = new SelectList(university.Distinct(), "NoteID", "UniversityInformation");
                ViewBag.courses = new SelectList(course.Distinct(), "NoteID", "CourseName");

                var notes = DBobj.Notes.Where(x => x.NotesTitle.StartsWith(search) || search == null).ToList();
                var cr = DBobj.Country.ToList();

                var seachedNotes = (from n in notes
                                    join c in cr on n.Country equals c.CountryID into table1
                                    from c in table1.ToList()
                                    where ((n.NotesType.ToString() == NoteTypeID || String.IsNullOrEmpty(NoteTypeID))
                                    && (n.NotesCategory.ToString() == NoteCategoryID || String.IsNullOrEmpty(NoteCategoryID))
                                    && (n.UniversityInformation.ToString() == UniversityInformation || String.IsNullOrEmpty(UniversityInformation))
                                    && (n.Country.ToString() == CountryID || String.IsNullOrEmpty(CountryID))
                                    && (n.CourseName.ToString() == Course || String.IsNullOrEmpty(Course)))
                                    select new nd
                                    {
                                        note = n,
                                        contryname = c
                                    }).ToList();

                ViewBag.filterNotes = seachedNotes;

                ViewBag.nd = seachedNotes.Count();



                return View();
            }

        }

        //GET: ContactUs
        public ActionResult contactUs()
        {
            return View();
        }
        //POST: ContactUs
        [HttpPost]
        public ActionResult contactUs(ContactUs model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                    {
                        ContactUs u = new ContactUs();
                        u.FullName = model.FullName;
                        u.EmailAddress = model.EmailAddress;
                        u.Subject = model.Subject;
                        u.Comments = model.Comments;
                        u.CreatedDate = DateTime.Now;
                        u.IsActive = true;

                        DBobj.ContactUs.Add(u);
                        DBobj.SaveChanges();

                        if (u.ContactID > 0)
                        {
                            ModelState.Clear();
                            ContactUsEmail.ContactUs(model.Subject, model.FullName, model.Comments);
                            return View();
                        }
                    }

                }

                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }


       [Authorize]
        public ActionResult dashboard(string pSearch, string submit)
        {

            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                int userid = (int)Session["UserID"];

                ViewBag.mySoldNotes = DBobj.Downloads.Where(x => x.Seller == userid).Count();
                ViewBag.totalEarning = DBobj.Downloads.Where(x => x.Seller == userid).Select(x => x.PurchasedPrice).Sum();
                ViewBag.myDownloadNotes = DBobj.Downloads.Where(x => x.Downloader == userid && x.IsSellerHasAllowedDownload == true).Count();
                ViewBag.myRejectedNotes = DBobj.Notes.Where(x => x.SellerID == userid && x.Status == 3).Count();
                ViewBag.buyerRequests = DBobj.Downloads.Where(x => x.Seller == userid && x.IsSellerHasAllowedDownload == false).Count();

                List<Notes> SellerNotes = null;
                if (submit == "Search")
                {
                    SellerNotes = DBobj.Notes.Where(x => x.SellerID == userid && (x.Status == 4 || x.Status == 1) &&
                                                        (x.NotesTitle.StartsWith(pSearch) || pSearch == null)).ToList();
                }
                else
                {
                    SellerNotes = DBobj.Notes.Where(x => x.SellerID == userid && (x.Status == 4 || x.Status == 1)).ToList();
                }

               
                var Category = DBobj.Categories.ToList();
                var Status = DBobj.NotesStatus.ToList();

                var ProgressNotes = (from sell in SellerNotes
                                     join cate in Category on sell.NotesCategory equals cate.CategoryID into table1
                                     from cate in table1.ToList()
                                     join stu in Status on sell.Status equals stu.StatusID into table2
                                     from stu in table2.ToList()
                                     select new MyProgressNotes
                                     {
                                         SellerNotes = sell,
                                         Category = cate,
                                         Status = stu
                                     }).ToList();

                ViewBag.inProgressNotes = ProgressNotes;


                List<Notes> PublishedNote = null;
                if (submit == "Search2")
                {
                    PublishedNote = DBobj.Notes.Where(x => x.SellerID == userid && x.Status == 2 &&
                                                           (x.NotesTitle.StartsWith(pSearch) || pSearch == null)).ToList();
                }
                else
                {
                    PublishedNote = DBobj.Notes.Where(x => x.SellerID == userid && x.Status == 2).ToList();
                }

               var PublishedNoted = (from sell in PublishedNote
                                     join cate in Category on sell.NotesCategory equals cate.CategoryID into table1
                                      from cate in table1.ToList()
                                      select new MyPublishNotes
                                      {
                                          SellerNotes = sell,
                                          Category = cate,

                                      }).ToList();

               ViewBag.publishedNote = PublishedNoted;
              
                return View();
            }

        }

        //GET: Notedetails
        public ActionResult noteDetails(int? id)
        {
            
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var note = DBobj.Notes.Where(x => x.NoteID == id).ToList();
                var category = DBobj.Categories.ToList();
                var country = DBobj.Country.ToList();
                var notedetail = (from n in note
                                  join c in category on n.NotesCategory equals c.CategoryID into table1
                                  from c in table1.ToList()
                                  join cr in country on n.Country equals cr.CountryID into table2
                                  from cr in table2.ToList()
                                  select new nd
                                  {
                                      note = n,
                                      Category = c,
                                      contryname = cr
                                  }).ToList();
                ViewBag.notedetailbag = notedetail;


                var reviews = DBobj.UsersReviews.ToList();
                var users = DBobj.Users.ToList();
                var reviewdetail = (from nr in reviews
                                    join n in note on nr.NoteID equals n.NoteID into table3
                                    from n in table3.ToList()
                                    join us in users on nr.ReviewedBy equals us.UserID into table4
                                    from us in table4.ToList()
                                    select new reviewtable
                                    {
                                        note = n,
                                        notereview = nr,
                                        usr = us
                                    }).ToList();

                ViewBag.reviewdetailbag = reviewdetail;
                ViewBag.reviewcount = reviewdetail.Count();

                var spam = DBobj.SpamReports.ToList();
                var spamtotal = (from sp in spam
                                 join n in note on sp.NoteID equals id into table5
                                 from n in table5.ToList()
                                 select new spamtable
                                 {
                                     note = n,
                                     spamrpt = sp
                                 }).ToList();

                ViewBag.spamtotalcount = spamtotal.Count();

            }
            return View();
        }

        [Route("DeleteNotes/id")]
        public ActionResult DeleteNotes(int? id)
        {
            using (NotesMarketPlaceEntities Dbobj = new NotesMarketPlaceEntities())
            {
                var delnote = Dbobj.SellerNoteAttachment.FirstOrDefault(x => x.NoteID == id);
                var note = Dbobj.Notes.FirstOrDefault(x => x.NoteID == id);
                if (delnote != null && note != null)
                {
                    Dbobj.SellerNoteAttachment.Remove(delnote);
                    Dbobj.Notes.Remove(note);
                    Dbobj.SaveChanges();
                }
                return RedirectToAction("dashboard", "User");
            }
        }



        [Authorize]
        public ActionResult buyerRequests(string buySearch, string submit)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                int userid = (int)Session["UserID"];
                List<Downloads> BuyersRequest = null;
                if (submit == "Search")
                {
                    BuyersRequest = DBobj.Downloads.Where(x => x.Seller == userid &&  x.IsSellerHasAllowedDownload == false && x.IsActive == true && x.SellFor == "Paid" &&
                                                        (x.NoteTitle.StartsWith(buySearch) || buySearch == null)).ToList();
                }
                else
                {
                    BuyersRequest = DBobj.Downloads.Where(x => x.Seller == userid && x.IsSellerHasAllowedDownload == false && x.IsActive == true && x.SellFor == "Paid").ToList();
                }


                var buyRequest = (from d in BuyersRequest
                                  join n in DBobj.Notes on d.NoteID equals n.NoteID
                                  where d.Seller == userid && d.IsSellerHasAllowedDownload == false && d.IsActive == true && d.SellFor == "Paid"
                                  join nc in DBobj.Categories on n.NotesCategory equals nc.CategoryID
                                  join usr in DBobj.Users on d.Seller equals usr.UserID
                                  select new MyProgressNotes
                                  {
                                      u = usr,
                                      SellerNotes = n,
                                      downloadnote = d,
                                      Category = nc

                                  }).ToList();
                ViewBag.BuyerRequests = buyRequest;
                ViewBag.BuyersRequestCount = buyRequest.Count();
                return View();
            }
        }

        [Authorize]
        public ActionResult AcceptBuyersRequest(int? id)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var accepted = DBobj.Downloads.Where(x => x.NoteID == id && x.IsSellerHasAllowedDownload == false).FirstOrDefault();
                accepted.IsSellerHasAllowedDownload = true;
                accepted.ModifiedBy = (int)Session["UserID"];
                accepted.ModifiedDate = DateTime.Now;
                var path = DBobj.SellerNoteAttachment.Where(x => x.NoteID == id && x.FilePath != null).FirstOrDefault();
                accepted.AttachmentPath = path.FilePath;
                DBobj.SaveChanges();

                int BuyerId = accepted.Downloader;
                var Buyerdetail = DBobj.Users.Where(x => x.UserID == BuyerId);
                foreach (var i in Buyerdetail)
                {
                    ViewBag.BuyerName = i.FirstName + " " + i.LastName;
                    ViewBag.EmailId = i.EmailId;
                }
                ViewBag.SellerName = Session["FullName"];
                InformBuyer.InformingBuyer(ViewBag.BuyerName, ViewBag.EmailId, ViewBag.SellerName);

            }
            return RedirectToAction("buyerRequests", "User");
        }



        [Authorize]
        public ActionResult changePassword()
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
                        return RedirectToAction("login", "User");
                    }

                }
            }
            return View();

        }




        public ActionResult faq()
        {
            return View();
        }

        public ActionResult DownloadFile(int id)
        {
            if (Session["UserID"] != null)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {

                    int sellerId = 0;

                    //Free Note
                    int isFree = DBobj.Notes.Where(x => x.NoteID == id && x.SellType == "Free").Count();
                    if (isFree > 0)
                    {
                        Downloads dn = new Downloads();
                        var notetitle = DBobj.Notes.Where(x => x.NoteID == id);
                        var sellerAttachement = DBobj.SellerNoteAttachment.Where(x => x.NoteID == id).FirstOrDefault();

                        dn.Downloader = (int)Session["UserID"];
                        dn.IsSellerHasAllowedDownload = true;
                        dn.SellFor = "Free";
                        dn.IsAttachmentDownloaded = false;
                        dn.CreatedDate = DateTime.Now;
                        dn.CreatedBy = (int)Session["UserID"];
                        dn.IsActive = true;
                        foreach (var iv in notetitle)
                        {
                            dn.NoteID = iv.NoteID;
                            dn.Seller = iv.SellerID;
                            dn.PurchasedPrice = iv.SellPrice;
                            dn.NoteTitle = iv.NotesTitle;
                            dn.NoteCategory = iv.NotesCategory;
                            sellerId = iv.SellerID;
                        }
                        dn.AttachmentPath = sellerAttachement.FilePath;

                        DBobj.Downloads.Add(dn);
                        DBobj.SaveChanges();

                        //Return files

                        var filesPath = sellerAttachement.FilePath.Split(';');
                        var filesName = sellerAttachement.FileName.Split(';');
                        using (var ms = new MemoryStream())
                        {

                            using (var z = new ZipArchive(ms, ZipArchiveMode.Create, true))
                            {
                                foreach (var FilePath in filesPath)
                                {
                                    string FullPath = Path.Combine(Server.MapPath(FilePath));
                                    string FileName = Path.GetFileName(FullPath);
                                    if (FileName == "User")
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

                    List<MyProgressNotes> Seller = (from n in DBobj.Notes
                                                    join usr in DBobj.Users on n.SellerID equals usr.UserID
                                                    where n.NoteID == id
                                                    select new MyProgressNotes
                                                    {
                                                        SellerNotes = n,
                                                        u = usr
                                                    }).ToList();

                    IEnumerable<NoteMarketPlace.Models.MyProgressNotes> sellername = Seller;
                    foreach (var iv in sellername)
                    {
                        ViewBag.SellerName = iv.u.FirstName + " " + iv.u.LastName;
                    }
                    var i = DBobj.Notes.Where(x => x.NoteID == id && x.SellType == "Paid").Count();
                    if (i > 0)
                    {
                        Downloads dn = new Downloads();
                        var notetitle = DBobj.Notes.Where(x => x.NoteID == id);

                        dn.Downloader = (int)Session["Userid"];
                        dn.IsSellerHasAllowedDownload = true;
                        dn.SellFor = "Paid";
                        dn.IsAttachmentDownloaded = false;
                        dn.CreatedDate = DateTime.Now;
                        dn.CreatedBy = (int)Session["Userid"];
                        dn.IsActive = true;
                        foreach (var iv in notetitle)
                        {
                            dn.NoteID = iv.NoteID;
                            dn.Downloader = iv.SellerID;
                            dn.PurchasedPrice = iv.SellPrice;
                            dn.NoteTitle = iv.NotesTitle;
                            dn.NoteCategory = (iv.NotesCategory);
                            sellerId = iv.SellerID;
                        }

                        DBobj.Downloads.Add(dn);
                        DBobj.SaveChanges();

                        var sellerInfo = DBobj.Users.Where(x => x.UserID == sellerId).ToList();
                        foreach (var s in sellerInfo)
                        {
                            ViewBag.sellerName = s.FirstName + " " + s.LastName;
                            ViewBag.sellerEmailId = s.EmailId;
                        }

                        string buyerName = (string)Session["FullName"];

                        //Send mail to owner
                        NotifySeller.InformingSeller(ViewBag.sellerName, ViewBag.sellerEmailId, buyerName);
                    }
                }
                return RedirectToAction("noteDetails", "User", new { id = id });

            }
            
            return RedirectToAction("login", "User");
        }

       [Authorize]
        public ActionResult myDownloads(string downloadSearch, string submit)
        {

            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.LazyLoadingEnabled = false;

                int userid = (int)Session["UserID"];

                List<Downloads> DownloadNotes = null;
                if (submit == "Search")
                {
                    DownloadNotes = DBobj.Downloads.Where(x => x.Downloader == userid &&  x.NoteTitle.StartsWith(downloadSearch) || downloadSearch == null).ToList();
                }
                else
                {
                    DownloadNotes = DBobj.Downloads.Where(x => x.Downloader == userid).ToList();
                }

                var downloadnotes = (from d in DownloadNotes
                                     join n in DBobj.Notes on d.NoteID equals n.NoteID
                                     where d.Downloader == userid && d.IsSellerHasAllowedDownload == true && d.IsActive == true
                                     join nc in DBobj.Categories on n.NotesCategory equals nc.CategoryID
                                     join usr in DBobj.Users on d.Seller equals usr.UserID
                                     select new MyProgressNotes
                                     {
                                         u = usr,
                                         SellerNotes = n,
                                         downloadnote = d,
                                         Category = nc

                                     }).ToList();
                ViewBag.downloadednotes = downloadnotes;
                ViewBag.DownloadNotesCount = downloadnotes.Count();

                return View();
            }
        }

       [Route("noteReview/noteID")]
        //GET: NoteReview
        public ActionResult noteReview(Review model, int noteID)
        {
            if (ModelState.IsValid)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    int id = (int)Session["UserID"];

                    UsersReviews nr = new UsersReviews();
                    var downloaddata = DBobj.Downloads.Where(x => x.NoteID == noteID && x.Downloader== id).FirstOrDefault();
                    nr.AgainstDownload = downloaddata.DownloadID;
                    nr.ReviewedBy = id;
                    nr.Ratings = model.Ratings;
                    nr.Comments = model.Comments;
                    nr.NoteID = noteID;
                    nr.IsActive = true;
                    nr.CreatedDate = DateTime.Now;
                    nr.CreatedBy = id;
                    nr.ModifiedBy = id;
                    nr.ModifiedDate = DateTime.Now;

                    DBobj.UsersReviews.Add(nr);
                    DBobj.SaveChanges();

                }
            }
            return RedirectToAction("myDownloads", "User");
        }

       


        [Authorize]
        public ActionResult myRejectedNotes(string rejectSearch, string submit)
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                int id = (int)Session["UserID"];
                List<Notes> RejectNotes = null;
                if (submit == "Search")
                {
                    RejectNotes = DBobj.Notes.Where(x => x.SellerID == id && x.Status == 3 &&
                                                        (x.NotesTitle.StartsWith(rejectSearch) || rejectSearch == null)).ToList();
                }
                else
                {
                    RejectNotes = DBobj.Notes.Where(x => x.SellerID == id && x.Status == 3).ToList();
                }
                var rejected = (from n in RejectNotes
                                join cat in DBobj.Categories on n.NotesCategory equals cat.CategoryID
                                where n.Status == 3 && n.SellerID == id
                                select new MyProgressNotes
                                {
                                    SellerNotes = n,
                                    Category = cat
                                }).ToList();
                ViewBag.RejectedNote = rejected;
                ViewBag.RejectedNoteCount = rejected.Count();

                return View();
            }
        }


        [Authorize]
        public ActionResult mySoldNotes()
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                DBobj.Configuration.LazyLoadingEnabled = false;
                int id = (int)Session["UserID"];
                
                var soldnotes = (from n in DBobj.Notes
                                 join d in DBobj.Downloads on n.NoteID equals d.NoteID
                                 where d.IsSellerHasAllowedDownload == true && d.Seller == id
                                 join cat in DBobj.Categories on n.NotesCategory equals cat.CategoryID
                                 join usr in DBobj.Users on d.Downloader equals usr.UserID
                                 select new MyProgressNotes
                                 {
                                     SellerNotes = n,
                                     downloadnote = d,
                                     Category = cat,
                                     u = usr

                                 }).ToList();
                ViewBag.mysoldnotes = soldnotes;
                ViewBag.mysoldnotesCount = soldnotes.Count();
                return View();
            }
        }

       
        //GET: UserProfile
        [Authorize]
        public ActionResult userProfile()
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var country = DBobj.Country.ToList();
                var gender = DBobj.Reference.ToList();
                var countrycode = DBobj.Country.ToList();

                ViewBag.countries = new SelectList(country, "CountryID", "CountryName");
                ViewBag.gender = new SelectList(gender, "ReferenceID", "Value");
                ViewBag.countrycode = new SelectList(countrycode, "CountryID", "CountryCode");
                return View();
            }
        }

        //POST: UserProfile
        [HttpPost]
        public ActionResult userProfile(UserProfileModel profile)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                    {
                        int id = (int)Session["UserID"];

                        var country = DBobj.Country.ToList();
                        var gender = DBobj.Reference.ToList();
                        var countrycode = DBobj.Country.ToList();


                        ViewBag.countries = new SelectList(country, "CountryID", "CountryName");
                        ViewBag.gender = new SelectList(gender, "ReferenceID", "Value");
                        ViewBag.countrycode = new SelectList(countrycode, "CountryID", "CountryCode");

                        UsersProfile pr = new UsersProfile();
                        pr.FirstName = profile.FirstName;
                        pr.LastName = profile.LastName;
                        pr.EmailID = profile.EmailId;
                        pr.DOB = profile.DOB;
                        pr.Gender = profile.Gender;
                        pr.PhoneCountryCode = profile.CountryCode;
                        pr.PhoneNumber = profile.PhoneNo;
                        pr.AddressLine1 = profile.Address1;
                        pr.AddressLine2 = profile.Address2;
                        pr.City = profile.City;
                        pr.State = profile.State;
                        pr.ZipCode = profile.ZipCode;
                        pr.Country = profile.Country;
                        pr.University = profile.University;
                        pr.College = profile.College;
                        pr.CreatedDate = DateTime.Now;
                        pr.CreatedBy = id;
                        pr.ModifiedBy = id;
                        pr.ModifiedDate = DateTime.Now;
                        pr.IsActive = true;
                        DBobj.UsersProfile.Add(pr);
                        DBobj.SaveChanges();

                        string path = Path.Combine(Server.MapPath("~/Member/" + Session["UserID"].ToString()), pr.ProfileID.ToString());
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

                            pr.ProfilePic = Path.Combine(("~/Member/" + Session["UserID"].ToString() + "/" + pr.ProfileID.ToString() + "/"), fileName);
                            DBobj.SaveChanges();
                        }
                    }
                    ViewBag.Message = "<p><span><i class='fas fa-check-circle'></i></span> Your Profile has succesfully been updated</p>";
                }
                else
                {

                    ViewBag.Message = "<p> Your Profile has not been updated.</p>";
                }

                ModelState.Clear();
                return RedirectToAction("dashboard", "User");

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return View();

        }
    }
}
