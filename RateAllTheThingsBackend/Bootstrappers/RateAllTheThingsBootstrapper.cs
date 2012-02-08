using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Bootstrapper;
using SharpBrake;

namespace RateAllTheThingsBackend.Bootstrappers
{
    public class RateAllTheThingsBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(
                                                    container.Resolve<IUserValidator>(),
                                                    "RateAllTheThings"));

            pipelines.OnError += (context, exception) =>
            {
                exception.SendToAirbrake();
                return null;
            };
        }
    }
}