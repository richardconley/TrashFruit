using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Events
{
    public class ProjectAssignedToUser
    {
        public Guid Id;
        public Guid AssignedToUser;
    }
}
