using System.Data.SqlClient;
using Dapper;

namespace RateAllTheThingsBackend.Repositories
{
    public interface ISignup
    {
        void AddSignup(string email);
    }

    public class Signup : BaseRepo, ISignup
    {
        public void AddSignup(string email)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();

                connection.Execute("INSERT INTO Signup(email) VALUES(@EMAIL)",
                    new
                    {
                        EMAIL = email
                    });
            }
        }
    }
}