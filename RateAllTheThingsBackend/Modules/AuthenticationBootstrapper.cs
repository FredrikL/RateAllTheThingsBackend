using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Bootstrapper;

namespace RateAllTheThingsBackend.Modules
{
    public class AuthenticationBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(
                                                    container.Resolve<IUserValidator>(),
                                                    "RateAllTheThings"));
        }
    }
}