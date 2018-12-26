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

namespace TrashFruit.Tests
{
    class WebAPITests:WebAPIHarness
    {
        public ProjectController p;
        public Guid projGuid;
        public string eventMessage;

        [SetUp]
        public void Setup()
        {
            p = new ProjectController(disp);
            projGuid = Guid.NewGuid();
            eventMessage = "This is a new Project";
        }

        [Test]
        public void StartNewProjectSendsAStartProjectCommand()
        {
            var events = new EventGrabber<ProjectStarted>();

            disp.AddHandlerFor<StartProject, ProjectAggregate>();
            disp.AddSubscriberFor<ProjectStarted>(events);
            IActionResult a = p.StartNewProject(eventMessage,projGuid);

            Assert.AreEqual(1, events.internalEvent.Count);
            Assert.AreEqual(projGuid, ((ProjectStarted)events.internalEvent[0]).Id);
            Assert.AreEqual(eventMessage, ((ProjectStarted)events.internalEvent[0]).Title);
           

        }
    }

  
}
