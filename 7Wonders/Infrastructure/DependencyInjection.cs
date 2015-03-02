using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector.Extensions;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace _7Wonders.Infrastructure
{
    public class DependencyInjection
    {
        public static void Initialize()
        {
            var container = BuildContainer();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorDependencyResolver(container);
        }
        private static Container container;
        public static Container Container { get { return container; } }
        public static Container BuildContainer()
        {
            container = new Container();
            container.Register<CommandBus>(Lifestyle.Singleton);
            container.Register<EventBus>(Lifestyle.Singleton);
            //var lifestyle = Lifestyle.Singleton;
            container.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), typeof(ICommandHandler<>).Assembly);
            container.RegisterManyForOpenGeneric(typeof(IEventHandler<>), (serviceType, implTypes) => container.RegisterAll(serviceType, implTypes), typeof(IEventHandler<>).Assembly);
            container.Verify();

            return container;
        }
        private class SimpleInjectorDependencyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver, System.Web.Http.Dependencies.IDependencyScope
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SimpleInjectorDependencyResolver"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="container"/> is a null
            /// reference.</exception>
            public SimpleInjectorDependencyResolver(Container container)
            {
                if (container == null)
                {
                    throw new ArgumentNullException("container");
                }

                this.Container = container;
            }

            /// <summary>Gets the container.</summary>
            /// <value>The <see cref="Container"/>.</value>
            public Container Container { get; private set; }

            /// <summary>Resolves singly registered services that support arbitrary object creation.</summary>
            /// <param name="serviceType">The type of the requested service or object.</param>
            /// <returns>The requested service or object.</returns>
            public object GetService(Type serviceType)
            {
                // By calling GetInstance instead of GetService when resolving a controller, we prevent the
                // container from returning null when the controller isn't registered explicitly and can't be
                // created because of an configuration error. GetInstance will throw a descriptive exception
                // instead. Not doing this will cause MVC to throw a non-descriptive "Make sure that the 
                // controller has a parameterless public constructor" exception.
                if (!serviceType.IsAbstract && typeof(IController).IsAssignableFrom(serviceType))
                {
                    return this.Container.GetInstance(serviceType);
                }

                return ((IServiceProvider)this.Container).GetService(serviceType);
            }

            /// <summary>Resolves multiply registered services.</summary>
            /// <param name="serviceType">The type of the requested services.</param>
            /// <returns>The requested services.</returns>
            public IEnumerable<object> GetServices(Type serviceType)
            {
                return this.Container.GetAllInstances(serviceType);
            }

            IDependencyScope System.Web.Http.Dependencies.IDependencyResolver.BeginScope()
            {
                return this;
            }

            object IDependencyScope.GetService(Type serviceType)
            {
                return ((IServiceProvider)this.Container)
                    .GetService(serviceType);
            }

            IEnumerable<object> IDependencyScope.GetServices(Type serviceType)
            {
                return this.Container.GetAllInstances(serviceType);
            }

            void IDisposable.Dispose()
            {
            }
        }
    }
}