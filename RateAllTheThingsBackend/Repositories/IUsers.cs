using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IUsers
    {
        string Create(string email);
        User Get(string email);
        bool Auth(string email, string password);
    }
}