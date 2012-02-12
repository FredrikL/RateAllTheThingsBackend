using System;
using System.Collections.Generic;
using Nancy.Security;

namespace RateAllTheThingsBackend.Models
{
    public class User : IUserIdentity
    {
        public Int64 Id { get; set; }

        public string Email { get; set; }
        
        public string Alias { get; set; }

        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}