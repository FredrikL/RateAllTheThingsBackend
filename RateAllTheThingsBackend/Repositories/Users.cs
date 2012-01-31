using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Nancy.Authentication.Basic;
using Nancy.Security;
using RateAllTheThingsBackend.Core;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class Users : BaseRepo, IUsers, IUserValidator
    {
        private readonly IHashing hashing;
        private readonly IPasswordGenerator passwordGenerator;

        public Users(IHashing hashing, IPasswordGenerator passwordGenerator)
        {
            this.hashing = hashing;
            this.passwordGenerator = passwordGenerator;
        }

        public string Create(string email)
        {
            string generatedpassword = this.passwordGenerator.Generate();
            var hashedPassword = this.hashing.Hash(generatedpassword);

            using (SqlConnection connection = Connection)
            {
                connection.Open();
                connection.Execute("INSERT INTO Users(Email, Password) values(@EMAIL, @PASSWORD);", new { EMAIL = email, PASSWORD = hashedPassword });
                return generatedpassword;
            }
        }

        public User Get(string email)
        {
            using (SqlConnection connection = Connection)
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

            using (SqlConnection connection = Connection)
            {
                connection.Open();
                return connection.Query<User>("SELECT Id FROM Users WHERE Email = @EMAIL and Password = @PASSWORD", new { EMAIL = email, PASSWORD = hashedpassword }).Any();
            }
        }

        public long GetIdByUsername(string userName)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                return connection.Query<long>("SELECT Id FROM Users WHERE Email = @EMAIL", new { EMAIL = userName }).Single();
            }
        }

        public string GetUserEmail(long userId)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                return connection.Query<string>("SELECT email FROM Users WHERE Id = @ID", new { ID = userId }).Single();
            }
        }

        public void ChangePassword(long id, string hashedPassword)
        {
            using(SqlConnection connection = Connection)
            {
                connection.Open();
                connection.Execute("UPDATE Users set Password = @Password where Id = @Id", new {Password = hashedPassword, Id = id});
            }
        }

        public IUserIdentity Validate(string username, string password)
        {

            var hashedpassword = this.hashing.Hash(password);

            using (SqlConnection connection = Connection)
            {
                connection.Open();                
                if(connection.Query<User>("SELECT Id FROM Users WHERE Email = @EMAIL and Password = @PASSWORD", new { EMAIL = username, PASSWORD = hashedpassword }).Any())
                    return new User() {UserName = username};
            }
            return new User();
        }
        
    }
}