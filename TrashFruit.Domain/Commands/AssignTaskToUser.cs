using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Commands
{
    public class AssignTaskToUser
    {
        public Guid Id;
        public int TaskId;
        public Guid AssignedToUser;
    }
}
