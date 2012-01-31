using System.Net.Mail;

namespace RateAllTheThingsBackend.Core
{
    public class EmailGateway : IEmailGateway
    {
        public void SendNewPasswordEmail(string email, string password, string alias = "")
        {
            using (var client = new SmtpClient())
            {
                client.Send("noreply@rateallthethings.com", email, "Password reset", password);
            }
        }
    }
}