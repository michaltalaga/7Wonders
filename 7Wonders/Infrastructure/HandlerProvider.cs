using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Infrastructure
{
    public class HandlerProvider
    {
        SimpleInjector.Container container;
        public HandlerProvider(SimpleInjector.Container container)
        {
            this.container = container;
        }

        public ICommandHandler<T> GetCommandHandler<T>()
        {
            return container.GetInstance<ICommandHandler<T>>();
            //return (ICommandHandler<T>)container.Resolve(typeof(ICommandHandler<T>), null);
        }
        public IEnumerable<IEventHandler<T>> GetEventHandlers<T>() where T : Event
        {
            return container.GetAllInstances<IEventHandler<T>>();
            //return container.ResolveAll(typeof(IEventHandler<T>)).Cast<IEventHandler<T>>();
        }

    }

}