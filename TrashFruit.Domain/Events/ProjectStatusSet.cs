using System;
using System.Collections.Generic;
using System.Text;
using TrashFruit.Domain.Commands;

namespace TrashFruit.Domain.Events
{
    public class ProjectStatusSet
    {
        public Guid Id;
        public ProjectStatusLane Status;
    }
}
