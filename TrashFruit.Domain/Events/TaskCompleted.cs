using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Events
{
    public class TaskCompleted
    {
        public Guid Id;
        public int TaskId;
        public DateTime TimeCompleted;
        //public DateTime EventTime;
    }
}
