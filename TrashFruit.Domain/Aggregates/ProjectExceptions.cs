using System;
using System.Collections.Generic;
using System.Text;

namespace TrashFruit.Domain.Aggregates
{
    public class ProjectNotStarted:Exception
    {
        public Guid Id;
    }

    public class TaskAlreadyCompleted:Exception
    {
        public Guid Id;
        public int TaskId;
    }
    public class TaskInInvalidState:Exception
    {
        public Guid Id;
        public int TaskId;
        public TaskStatus Status;
        public object Command;
    }
    public class TaskCannotBeUnassigned:Exception
    {
        public Guid Id;
        public int TaskId;
    }
}
