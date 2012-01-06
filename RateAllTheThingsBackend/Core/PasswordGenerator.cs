using System.Linq;
using System.Security.Cryptography;

namespace RateAllTheThingsBackend.Core
{
    public class PasswordGenerator:IPasswordGenerator
    {
        public string Generate()
        {
            byte[] pw = new byte[35];
            var random = new RNGCryptoServiceProvider();
            random.GetBytes(pw);
            return string.Concat(pw.Select(b => b.ToString("X2")).ToArray());
        }
    }
}