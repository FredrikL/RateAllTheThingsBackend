using System;
using System.Net.Mail;

namespace RateAllTheThingsBackend.Core
{
    public class EmailGateway : IEmailGateway
    {
        public void SendNewPasswordEmail(string email, string password, string alias = "")
        {
            using (var client = new SmtpClient())
            {
                string message = "Hello " + alias + Environment.NewLine + "Your new password is: " + password + Environment.NewLine + Environment.NewLine + "/http://rateallthethings.com";
                client.Send("noreply@rateallthethings.com", email, "Password reset", message);
            }
        }
    }
}