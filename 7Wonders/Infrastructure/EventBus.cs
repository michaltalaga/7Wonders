using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Infrastructure
{
    public class EventBus
    {
        HandlerProvider handlerProvider;
        public EventBus(HandlerProvider handlerProvider)
        {
            this.handlerProvider = handlerProvider;
        }

        public void Publish<T>(T evt) where T : Event
        {
            foreach (var handler in handlerProvider.GetEventHandlers<T>())
            {
                handler.Handle(evt);
            }
        }
    }
}