using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using DomainModel.Repository;
using Task_1.Controllers;

namespace Task_1
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            //var file_repository = ConfigurationManager.AppSettings["FileRepositoryPath"];
            //var full_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file_repository);
            //builder.RegisterInstance(new FileRepository(full_path))
            //    .AsSelf()
            //    .AsImplementedInterfaces()
            //    .SingleInstance();

            var connection_string = ConfigurationManager.ConnectionStrings["DefaultConnection"]
                .ConnectionString;
            builder.RegisterInstance(new DBRepository(connection_string))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SubnetContainerController>()
                .AsSelf();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    
}
