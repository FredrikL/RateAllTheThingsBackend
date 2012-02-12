using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using RateAllTheThingsBackend.Controller;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class AccontModule : NancyModule
    {
        private readonly IUserController userController;
        private readonly IUsers users;

        public AccontModule(IUserController userController, IUsers users) : base("/Account")
        {
            this.userController = userController;
            this.users = users;
            this.RequiresAuthentication();

            Post["/"] = x =>
                            {
                                var user = this.Bind<User>();
                                var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);  
                                if(user.Id == userId)
                                    this.userController.Update(user);
                                else
                                    return Response.AsJson(false);
                                return Response.AsJson(true);
                            };

            Get["/"] = x =>
                           {
                               var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                               var user = this.users.Get(userId);
                               return Response.AsJson(user);
                           };
        }
    }
}