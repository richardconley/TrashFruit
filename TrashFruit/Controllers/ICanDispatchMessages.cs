using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Edument.CQRS;
using System.Collections;

namespace TrashFruit.Controllers
{
    public interface ICanDispatchMessages
            {
            MessageDispatcher dispatcher { get; set; }
           }
}