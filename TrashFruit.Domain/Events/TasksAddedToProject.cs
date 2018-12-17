using System;
using System.Collections.Generic;
using System.Text;
using TrashFruit.Domain.Aggregates;

namespace TrashFruit.Domain.Events
{
    public class TasksAddedToProject
    {
        public Guid Id;
        public List<ProjectTask> ProjectTasks;
    }
}
