using System;
using System.Collections.Generic;
using System.Text;
using TrashFruit.Domain.Aggregates;

namespace TrashFruit.Domain.Commands
{
    public class AddTasksToProject
    {
        public Guid Id;
        public List<ProjectTask> ProjectTasks;
    }
}
