using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Commands
{
    public class MarkTaskCompleted
    {
        public Guid Id;
        public int TaskId;
        public DateTime TimeCompleted;
    }
}
