using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class GravatarService : IGravatarService
    {
        private readonly IUsers users;
        private const string GravatarBaseUrl = "http://www.gravatar.com/avatar/{0}";

        public GravatarService(IUsers users)
        {
            this.users = users;
        }

        private static string GetMD5Hash(string input)
        {
            var x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();

        }

        public IEnumerable<Comment> AddAvatarToComments(IEnumerable<Comment> comments)
        {
            foreach (var comment in comments)
            {
                var email = this.users.GetUserEmail(comment.Author);
                comment.Avatar = string.Format(GravatarBaseUrl, GetMD5Hash(email));
                yield return comment;
            }
        }
    }
}