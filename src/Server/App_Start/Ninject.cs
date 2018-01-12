using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using Ninject.Web.Common;
using Server.DAL;

namespace Server
{
    public static class NinjectConfig
    {
        private static IKernel _kernel;
        private static object _syncRoot = new object();

        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    lock (_syncRoot)
                    {
                        if (_kernel == null)
                        {
                            _kernel = new StandardKernel();

                            // this configure EF to use in-memory DB via Effort library
                            _kernel.Bind<IServerDbContex>().ToMethod(x => new ServerDbContext(Effort.DbConnectionFactory.CreatePersistent("ServerDb"))).InRequestScope();
                            _kernel.Bind<INodeDAL>().To<NodeDAL>().InSingletonScope();
                            _kernel.Bind<INodePluginProvider>().To<NodePluginProvider>().InSingletonScope();
                            _kernel.Bind<IWizardSession>().To<WizardSession>().InSingletonScope();
                            _kernel.Bind<IWizardStepsProvider>().To<WizardStepsProvider>().InSingletonScope();
                            _kernel.Bind<INodeService>().To<NodeService>().InSingletonScope();
                            _kernel.Bind<IFilesContentProvider>().To<FilesContentProvider>().InSingletonScope();
                            _kernel.Bind<IConfigurationProvider>().To<ConfigurationProvider>().InSingletonScope();
                            _kernel.Bind<IDateTimeProvider>().To<DateTimeProvider>().InSingletonScope();
                            _kernel.Bind(typeof(ICache<>)).To(typeof(Cache<>)).InSingletonScope();
                        }
                    }
                }

                return _kernel;
            }
        }

        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new NinjectResolver(Kernel);
        }
    }

    public class NinjectResolver : NinjectScope, IDependencyResolver
    {
        private IKernel _kernel;
        public NinjectResolver(IKernel kernel) : base(kernel)
        {
            _kernel = kernel;
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectScope(_kernel.BeginBlock());
        }
    }

    public class NinjectScope : IDependencyScope
    {
        protected IResolutionRoot resolutionRoot;
        public NinjectScope(IResolutionRoot kernel)
        {
            resolutionRoot = kernel;
        }
        public object GetService(Type serviceType)
        {
            IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolutionRoot.Resolve(request).SingleOrDefault();
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolutionRoot.Resolve(request).ToList();
        }
        public void Dispose()
        {
            IDisposable disposable = (IDisposable)resolutionRoot;
            if (disposable != null) disposable.Dispose();
            resolutionRoot = null;
        }
    }
}