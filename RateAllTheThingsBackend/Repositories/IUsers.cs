using System;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IUsers
    {
        string Create(string email);
        User Get(string email);
        User Get(Int64 id);
        bool Auth(string email, string password);
        Int64 GetIdByUsername(string userName);
        string GetUserEmail(Int64 userId);
        void ChangePassword(Int64 id, string hashedPassword);
        void Update(User user);
    }
}