using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using RateAllTheThingsBackend.Core;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class UsersModule : NancyModule
    {
        private readonly IUsers users;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IHashing hashing;
        private readonly IEmailGateway emailGateway;

        public UsersModule(IUsers users, IPasswordGenerator passwordGenerator,
            IHashing hashing, IEmailGateway emailGateway) : base("/User")
        {
            this.users = users;
            this.passwordGenerator = passwordGenerator;
            this.hashing = hashing;
            this.emailGateway = emailGateway;

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

            Get["/PasswordReset"] = x =>
                                        {
                                            return View["passwordreset"];
                                        };

            Post["/PasswordReset"] = x =>
                                         {
                                             bool result = false;
                                             var input = this.Bind<PasswordResetBody>();
                                             if(!string.IsNullOrWhiteSpace(input.email))
                                             {
                                                 var user = this.users.Get(input.email);
                                                 if(user!= null)
                                                 {
                                                     var password = this.passwordGenerator.Generate();
                                                     var hashedPassword = this.hashing.Hash(password);
                                                     this.users.ChangePassword(user.Id, hashedPassword);
                                                     this.emailGateway.SendNewPasswordEmail(user.Email, password);
                                                     result = true;
                                                 }
                                             }

                                             //TODO: Log ?

                                             return View["passwordreset", new {Success = result}];
                                         };
            Get["/TrollLol"] = x =>
                                   {
                                       this.emailGateway.SendNewPasswordEmail("fredrik.leijon@gmail.com", "troll lol");
                                       return Response.AsJson(Enumerable.Empty<int>());
                                   };
        }

        class PasswordResetBody
        {
            public string email { get; set; }
        }
    }
}