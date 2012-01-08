using System;
using System.Web.Script.Serialization;

namespace RateAllTheThingsBackend.Models
{
    public class User
    {
        public Int64 Id { get; set; }

        [ScriptIgnore]
        public string Email { get; set; }
        
        public string Alias { get; set; }
    }
}