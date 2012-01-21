using System;
using System.Security.Cryptography;

namespace RateAllTheThingsBackend.Core
{
    public class PasswordGenerator:IPasswordGenerator
    {
        public string Generate()
        {
            byte[] pw = new byte[50];
            var random = new RNGCryptoServiceProvider();
            random.GetBytes(pw);
            return Convert.ToBase64String(pw);
            //return string.Concat(pw.Select(b => b.ToString("X2")).ToArray());
        }
    }
}