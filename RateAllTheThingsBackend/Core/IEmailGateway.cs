namespace RateAllTheThingsBackend.Core
{
    public interface IEmailGateway
    {
        void SendNewPasswordEmail(string email, string password, string alias = "");
    }
}