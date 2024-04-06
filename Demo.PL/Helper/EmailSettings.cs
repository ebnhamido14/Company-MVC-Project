using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587); // Specify Mail Server
            Client.EnableSsl = true; // Encrypted Mail
            Client.Credentials = new NetworkCredential("Ebnhamido8@gmail.com", "miye uagy xzbw owzj");
            Client.Send("Ebnhamido8@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
