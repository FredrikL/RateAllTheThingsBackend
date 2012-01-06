using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RateAllTheThingsBackend.Core
{
    public class Hashing : IHashing
    {
        public string Hash(string data)
        {
            var sha = new SHA512Managed();
            var result = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
            return string.Concat(result.Select(b => b.ToString("X2")).ToArray());
        }
    }
}