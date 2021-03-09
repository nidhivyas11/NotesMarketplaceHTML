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
                                Session["FullName"] = namedata.FirstName + " " + namedata.LastName;
                                Session["UserID"] = namedata.UserID;
                                return RedirectToAction("dashboard", "User");
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
                                FilePath += Path.Combine(("~/Member/" + Session["UserID"].ToString() + "/" + nd.NoteID.ToString() + "/"), fileName) + ";";
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

                    ModelState.Clear();
                    return RedirectToAction("addNote", "User");
                }
                 catch (DbEntityValidationException e)
                {
                   Console.WriteLine(e);
                }



            return View();
            }


        //GET: SearchNotes
        public ActionResult searchNotes()
        {
            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                var notecategory = DBobj.Categories.ToList();
                var notetype = DBobj.Type.ToList();
                var country = DBobj.Country.ToList();
                var university_course = DBobj.Notes.ToList();
                ViewBag.notecategoies = new SelectList(notecategory.Distinct(), "CategoryID", "CategoryName");
                ViewBag.notetypes = new SelectList(notetype.Distinct(), "TypeID", "TypeName");
                ViewBag.countries = new SelectList(country.Distinct(), "CountryID", "CountryName");
                ViewBag.universities = new SelectList(university_course.Distinct(), "UniversityInformation", "UniversityInformation");
                ViewBag.courses = new SelectList(university_course.Distinct(), "CourseName", "CourseName");
            }
            return View();

        }
        //POST: SearchNotes
        [HttpPost]
        public ActionResult searchNotes(addnote model) 
        {
            if (ModelState.IsValid)
            {
                using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
                {
                    var list = DBobj.Notes.Where(x => x.NotesCategory == model.NoteCategoryID && x.NotesType == model.NoteTypeID && x.Country == model.CountryID && x.CourseName == model.Course && x.UniversityInformation == model.UniversityInformation);
                }
            }
            return View();
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
        public ActionResult dashboard()
        {

            using (NotesMarketPlaceEntities DBobj = new NotesMarketPlaceEntities())
            {
                int userid = (int)Session["UserID"];

                ViewBag.mySoldNotes = DBobj.Downloads.Where(x => x.Seller == userid).Count();
                ViewBag.totalEarning = DBobj.Downloads.Where(x => x.Seller == userid).Select(x => x.PurchasedPrice).Sum();
                ViewBag.myDownloadNotes = DBobj.Downloads.Where(x => x.Downloader == userid && x.IsSellerHasAllowedDownload == true).Count();
                ViewBag.myRejectedNotes = DBobj.Notes.Where(x => x.SellerID == userid && x.Status == 3).Count();
                ViewBag.buyerRequests = DBobj.Downloads.Where(x => x.Seller == userid && x.IsSellerHasAllowedDownload == false).Count();

                var SellerNotes = DBobj.Notes.Where(x => x.SellerID == userid && x.Status == 4 || x.Status == 1).ToList();
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


               var PublishedNote = DBobj.Notes.Where(x => x.SellerID == userid && x.Status == 2).ToList();

               var PublishedNoted = (from sell in SellerNotes
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
        public ActionResult noteDetails()
        {
            int id = 1;
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



        [Authorize]
        public ActionResult buyerRequests()
        {
            return View();
        }


        [Authorize]
        public ActionResult changePassword()
        {
            return View();
        }




        public ActionResult faq()
        {
            return View();
        }




        [Authorize]
        public ActionResult myDownloads()
        {
            return View();
        }


        [Authorize]
        public ActionResult myRejectedNotes()
        {
            return View();
        }


        [Authorize]
        public ActionResult mySoldNotes()
        {
            return View();
        }




        [Authorize]
        public ActionResult userProfile()
        {
            return View();
        }
    }
}