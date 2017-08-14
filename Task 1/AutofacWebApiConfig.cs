using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DomainModel.Repository;

namespace Task_1
{
    public static class AutofacWebApiConfig
    {
        private static IContainer _container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        private static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var file_repository = ConfigurationManager.AppSettings["FileRepositoryPath"];
            var full_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file_repository);
            builder.RegisterInstance(new FileRepository(full_path))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            _container = builder.Build();

            return _container;
        }

    }
}