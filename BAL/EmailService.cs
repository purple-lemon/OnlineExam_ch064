using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace BAL
{
    public class EmailService
    {
        public void SendEmail(string ToEmail, string Message, string Subject)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("localhost");
            mail.From = new MailAddress("SoftServe@gmail.com");
            mail.To.Add(ToEmail);
            mail.Subject = Subject;
            mail.Body = Message;

            SmtpServer.Port = 25;

            SmtpServer.Send(mail);
        }
    }
}