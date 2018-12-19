using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Commands
{
    public class AssignProjectToUser
    {
        public Guid Id;
        public Guid AssignedToUser;
    }
}
