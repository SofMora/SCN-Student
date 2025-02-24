using System.Net.Mail;

namespace ProyectoStudent.Models
{
    public class SendMails
    {
        const string user = "smartparking500@gmail.com";
        const string password = "tmybnncgjlyhihzl";
        public void sendMailsSmartParking(string message, string mailClient, string subject)
        {

            try
            {

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("smartparking500@gmail.com");
                mail.To.Add(mailClient);
                mail.Subject = subject;
                mail.Body = message.ToString();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(user, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void sendMailsSmartParkingPdf(string message, string mailClient, string subject)
        {

            try
            {

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("smartparking500@gmail.com");
                mail.To.Add(mailClient);
                mail.Subject = subject;
                mail.Body = message.ToString();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(user, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
