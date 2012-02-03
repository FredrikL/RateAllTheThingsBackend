using Nancy;
using Nancy.Security;

namespace RateAllTheThingsBackend.Modules
{
    public class AuthModule : NancyModule
    {
        public AuthModule(): base("/Auth")
        {
            this.RequiresAuthentication();

            Get["/"] = x =>
                           {
                               return Response.AsJson(new[] {true});
                           };
        }
    }
}