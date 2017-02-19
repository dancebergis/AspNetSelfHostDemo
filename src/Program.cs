using System.Web.Http;
using Ninject.Modules;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.WebApi;
using Topshelf.WebApi.Ninject;

namespace AspNetSelfHostDemo
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(c =>
            {
                c.UseNinject(new Binding()); //Initiates Ninject and consumes Modules

                c.Service<TaxManagerService>(s =>
                {
                    //Specifies that Topshelf should delegate to Ninject for construction
                    s.ConstructUsingNinject();

                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    //Topshelf.WebApi - Begins configuration of an endpoint
                    s.WebApiEndpoint(api =>
                        //Topshelf.WebApi - Uses localhost as the domain, defaults to port 8080.
                        //You may also use .OnHost() and specify an alternate port.
                        api.OnLocalhost()
                            //Topshelf.WebApi - Pass a delegate to configure your routes
                            .ConfigureRoutes(Configure)
                            //Topshelf.WebApi.Ninject (Optional) - You may delegate controller 
                            //instantiation to Ninject.
                            //Alternatively you can set the WebAPI Dependency Resolver with
                            //.UseDependencyResolver()
                            .UseNinjectDependencyResolver()
                            //Instantaties and starts the WebAPI Thread.
                            .Build());
                });
            });
        }

        private static void Configure(HttpRouteCollection routes)
        {
            routes.MapHttpRoute("TaxApi", "api/{controller}/{municipality}/{date}",
                new {municipality = RouteParameter.Optional, date = RouteParameter.Optional}
                );
        }
    }

    public class Binding : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaxService>().To<TaxService>();
            Bind<ITaxRepository>().To<TaxRepository>();
        }
    }
}
