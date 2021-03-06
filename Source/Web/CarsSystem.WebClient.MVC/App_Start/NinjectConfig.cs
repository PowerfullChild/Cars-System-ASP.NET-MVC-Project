[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CarsSystem.WebClient.MVC.App_Start.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CarsSystem.WebClient.MVC.App_Start.NinjectConfig), "Stop")]

namespace CarsSystem.WebClient.MVC.App_Start
{
    using Data;
    using Data.Repositories;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Services;
    using Services.Contracts;
    using System;
    using System.Web;

    public static class NinjectConfig 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICarsSystemDbContext>().To<CarsSystemDbContext>().InRequestScope();
            kernel.Bind(typeof(IEfGenericRepository<>)).To(typeof(EfGenericRepository<>));

            kernel.Bind<IUsersService>().To<UsersService>();
            kernel.Bind<ICarsService>().To<CarsService>();
            kernel.Bind<IFilterService>().To<FilterService>();
            kernel.Bind<IMailService>().To<MailService>();
        }        
    }
}
