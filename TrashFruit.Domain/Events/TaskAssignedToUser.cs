using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Events
{
    public class TaskAssignedToUser
    {
        public Guid Id;
        public int TaskId;
        public Guid AssignedToUser;
        public Guid AssignedFromUser;
    }
}
