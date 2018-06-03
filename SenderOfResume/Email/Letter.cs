using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SenderOfResume.Email
{
    class Letter
    {
        private string file = @"c:\Резюме.pdf";
        private string password = File.ReadAllText(@"c:\password");

        public async void SendEmail(string email)
        {
            await Task.Run(() =>
            {
                var mail = new MailMessage("dzmitrychubryk@gmail.com", email, "Resume", "body");
                Attachment resume = new Attachment(file);
                mail.Attachments.Add(resume);
                var client = new SmtpClient("smtp.gmail.com");
                client.Port = 587;
                client.Credentials = new System.Net.NetworkCredential("dzmitrychubryk@gmail.com", password);
                client.EnableSsl = true;
                client.Send(mail);
            });
        }
    }
}
