using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Infrastructure
{
    public class CommandBus
    {
        HandlerProvider handlerProvider;
        public CommandBus(HandlerProvider handlerProvider)
        {
            this.handlerProvider = handlerProvider;
        }
        public void Send<T>(T command)
        {

            var handler = handlerProvider.GetCommandHandler<T>();
            handler.Handle(command);
        }
    }

}