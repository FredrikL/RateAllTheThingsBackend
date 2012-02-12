using System;
using System.Collections.Generic;
using Nancy.Security;

namespace RateAllTheThingsBackend.Models
{
    public class User
    {
        public Int64 Id { get; set; }

        public string Email { get; set; }
        
        public string Alias { get; set; }        
    }

    public class UserIdentity :User, IUserIdentity
    {
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}