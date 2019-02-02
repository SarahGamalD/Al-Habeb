using AlHabieb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace AlHabieb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Services()
        {

            return View();
        }
        public ActionResult BusinessSolutions()
        {

            return View();
        }
       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ContactUs()
        {
            ViewBag.Message = "Test Form";
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(MailModels e)
        {
            if (ModelState.IsValid)
            {

                //prepare email
                var toAddress = "sarah123gamal@gmail.com";
                var fromAddress = e.Email.ToString();
                var subject = "Test enquiry from " + e.Name;
                var message = new StringBuilder();
                message.Append("Name: " + e.Name + "\n");
                message.Append("Email: " + e.Email + "\n");
                message.Append("Telephone: " + e.Telephone + "\n\n");
                message.Append(e.Message);

                //start email Thread
                var tEmail = new Thread(() =>
               SendEmail(toAddress, fromAddress, subject, message.ToString()));
                tEmail.Start();
            }
            return View();
        }
        public void SendEmail(string toAddress, string fromAddress,
                      string subject, string message)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    const string email = "testemailtest57@gmail.com";
                    const string password = "p@2Sword";

                    var loginInfo = new NetworkCredential(email, password);


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                try
                {
                    using (var smtpClient = new SmtpClient(
                                                         "smtp.gmail.com", 587))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }

                }

                finally
                {
                    //dispose the client
                    mail.Dispose();
                }

            }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        Response.Write("Failed to deliver message to {0}");
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
        //_______________________________________
        [HttpGet]
        public ActionResult ContactUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactUser(EmailFormModel model)
        {
       
            if (ModelState.IsValid)
            {
                var body = "<div style=\"border: 5px solid #9f0000;padding: 20px 20px 20px 20px;text-align: center; border-radius: 40px;width:700px;background: #fafafa;\"><img src=\"~/Content/Images/WhatsApp Image 2019-01-27 at 4.06.34 PM.jpeg\" alt=\"logo\" style=\"width: 150px; border: 1px solid #ddd;  border-radius: 4px; padding: 5px;  width: 150px;\"><p style=\"text-align: center; \"><h4 style=\"color:#9f0000; text-decoration: underline; text-shadow: 3px 2px #9f0000;font-family: 'Roboto', sans-serif;\">Email From:</h4> <p>{0}</p><h4 style=\"color:#9f0000;text-decoration: underline;  text-shadow: 3px 2px red;font-family:'Roboto', sans-serif;\">Email:</h4><p>{1}</p><h4 style=\"color:#9f0000;text-decoration: underline;  text-shadow: 3px 2px #9f0000;font-family:'Roboto', sans-serif;\">Telephone:</h4><p>{3}</p><h4 style=\"color:#9f0000;text-decoration: underline;  text-shadow: 3px 2px #9f0000;font-family:'Roboto', sans-serif;\">Message:</h4><p>{2}</p></p></div>";
                //border-style: outset;border-color: #32606c; 
                //<img src=\"" + MakeImageSrcData("D:/Projects/AlHabieb/AlHabieb/Content/Images") + "\"/>
                
                var message = new MailMessage();
                message.To.Add(new MailAddress("alhabebmail@gmail.com"));  // replace with valid value 
                //message.From = new MailAddress(model.FromEmail);  // replace with valid value
                //var From = model.FromEmail.ToString();  // replace with valid value
                message.From = new MailAddress(model.FromEmail);
                message.Subject = model.Subject;
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message,model.Telephone,model.ImagePath);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "alhabebmail@gmail.com",  // replace with valid value
                        Password = "p@2Sword"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    TempData["Message"] = "Your Email has been sent Successfully ";
                    return RedirectToAction("ContactUser");
                }
            }
            return View(model);
        }
        public PartialViewResult ShowContactModal()
        {
            return PartialView("ShowContactModal");
        }
        public ActionResult Sent()
        {
            return View();
        }
        //string MakeImageSrcData(string filename)
        //{
        //    //FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        //    //byte[] filebytes = new byte[fs.Length];
        //    //fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
        //    //return "data:image/png;base64," +
        //    //  Convert.ToBase64String(filebytes, Base64FormattingOptions.None);

        //    byte[] filebytes = System.IO.File.ReadAllBytes(filename);
        //    return "data:image/png;base64," + Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
        //}

    }
}