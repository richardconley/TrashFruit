using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TrashFruit.Domain.Aggregates;
using TrashFruit.Domain.Commands;
using TrashFruit.Domain.Events;
using Edument.CQRS;
using TrashFruit;
using TrashFruit.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace TrashFruit.Tests
{
    public class WebAPIHarness
    {
        public MessageDispatcher disp;
        public IEventStore ES;
       

        [SetUp]
        public void WebAPIHarnessSetup()
        {
            ES = new InMemoryEventStore();
            disp = new MessageDispatcher(ES);
            

        }
    }

    //public class CommandGrabber<TCommand> : Aggregate,IHandleCommand<TCommand>
    //{
    //    object command;
    //    public IEnumerable Handle(TCommand c)
    //    {
    //        command = c;
    //        yield return new NoOpEvent();
    //    }
    //}

    public class EventGrabber<TEvent> : ISubscribeTo<TEvent>
    {
        public List<TEvent> internalEvent;
        public EventGrabber()
            {
            internalEvent = new List<TEvent>();
            }


        public void Handle(TEvent e)
        {
            internalEvent.Add(e);
        }
    }

    public class NoOpEvent { }
}
