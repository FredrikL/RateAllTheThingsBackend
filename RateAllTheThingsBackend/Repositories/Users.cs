using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using RateAllTheThingsBackend.Core;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class Users : IUsers
    {
        private readonly IHashing hashing;
        private readonly IPasswordGenerator passwordGenerator;

        public Users(IHashing hashing, IPasswordGenerator passwordGenerator)
        {
            this.hashing = hashing;
            this.passwordGenerator = passwordGenerator;
        }

        private static SqlConnection Connection()
        {
            return new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]);
        }

        public string Create(string email)
        {
            string generatedpassword = this.passwordGenerator.Generate();
            var hashedPassword = this.hashing.Hash(generatedpassword);

            using (SqlConnection connection = Connection())
            {
                connection.Open();
                connection.Execute("INSERT INTO Users(Email, Password) values(@EMAIL, @PASSWORD);", new { EMAIL = email, PASSWORD = hashedPassword });
                return generatedpassword;
            }
        }

        public User Get(string email)
        {
            using (SqlConnection connection = Connection())
            {
                connection.Open();
                var users =  connection.Query<User>("SELECT Id, Email, Alias FROM Users WHERE Email = @EMAIL", new { EMAIL = email}).ToArray();
                if (users.Any())
                    return users.Single();
            }
            return null;
        }

        public bool Auth(string email, string password)
        {
            var hashedpassword = this.hashing.Hash(password);

            using (SqlConnection connection = Connection())
            {
                connection.Open();
                return connection.Query<User>("SELECT Id FROM Users WHERE Email = @EMAIL and Password = @PASSWORD", new { EMAIL = email, PASSWORD = hashedpassword }).Any();
            }
        }
    }
}