using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edument.CQRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TrashFruit.Domain.Commands;

namespace TrashFruit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase,
        ICanDispatchMessages
    {
        public MessageDispatcher dispatcher { get; set; }
        public ProjectController(MessageDispatcher Dispatcher)
        {
           dispatcher = Dispatcher;
        }

        public IActionResult StartNewProject(string Title)
        {
            return StartNewProject(Title,Guid.NewGuid());
        }

        public IActionResult StartNewProject(string Title, Guid guid)
        {
            dispatcher.SendCommand<StartProject>(new StartProject
            {
                Title = Title,
                Id = guid
            });
            return Ok();
        }


    }
}