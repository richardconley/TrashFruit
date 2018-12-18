using System;
using System.Collections.Generic;
using System.Text;
using TrashFruit.Domain.Aggregates;

namespace TrashFruit.Domain.Events
{
    public class TaskUpdated
    {
        public Guid Id;
        public int TaskId;
        public TaskStatus Status;
        public string Comment;
    }
}
