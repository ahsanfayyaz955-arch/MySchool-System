using System.Net;
using System.Net.Mail;

namespace MySchool_System.EmailSender
{
    public class EmailSender
    {
        public static bool SendMail(string to, string subject, string message)
        {
            try
            {
                using (var msg = new MailMessage())
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    msg.From = new MailAddress("ahsanfayyaz955@gmail.com", "My School System");
                    msg.To.Add(to);
                    msg.Subject = subject;
                    msg.Body = message;
                    msg.IsBodyHtml = true;

                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("ahsanfayyaz955@gmail.com", "whyh plvf wtdt hbjm"); // Gmail App Password

                    smtp.Send(msg);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
                return false;
            }
        }
    }
}
