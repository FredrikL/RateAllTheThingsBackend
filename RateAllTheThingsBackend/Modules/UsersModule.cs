using System.Linq;
using Nancy;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class UsersModule : NancyModule
    {
        private readonly IUsers users;

        public UsersModule(IUsers users) : base("/User")
        {
            this.users = users;

            Post["/"] = x =>
                            {
                                var email = Request.Query.email.ToString();
                                if (!string.IsNullOrEmpty(email))
                                {
                                    var fromDb = this.users.Get(email);
                                    if (fromDb != null)
                                        return Response.AsJson(new[] {fromDb});

                                    var password = this.users.Create(email);
                                    return Response.AsJson(new[] {new {password}});
                                }
                                return Response.AsJson(Enumerable.Empty<string>());
                            };
        }
    }
}