using System;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Controller
{
    public class UserController : IUserController
    {
        private readonly IUsers users;

        public UserController(IUsers users)
        {
            this.users = users;
        }

        public void Update(User user)
        {
            this.users.Update(user);
        }
    }
}