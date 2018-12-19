using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Commands
{
    public class SetProjectStatus
    {
        public Guid Id;
        public ProjectStatusLane Status;
    }

    public enum ProjectStatusLane { None, Defined, Scheduled, InProgress,Completed, Cancelled} 
}
