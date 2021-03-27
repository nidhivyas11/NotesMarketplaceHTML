﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using NoteMarketPlace.Models;

namespace NoteMarketPlace.EmailTemplates
{
    public class NotifyAdmin
    {
        public static void sendNotification(string sellername)
        {
            var fromEmail = new MailAddress("nkvyas99@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress("vyasnidhi11@gmail.com");
            var fromEmailPassword = "Lovemaggi99"; // Replace with actual password
            string subject = sellername + "sent his note for review";
            string msg = "Hello Admins";
            msg += "We want to inform you that" + sellername + "sent his Note for review. Please look at the notes and take review. <br/>";
            msg += "<br/><br/>Regards,<br/>";
            msg += "Notes Marketplace";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = msg,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}