using System;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IUsers
    {
        string Create(string email);
        User Get(string email);
        bool Auth(string email, string password);
        Int64 GetIdByUsername(string userName);
        string GetUserEmail(Int64 userId);
    }
}