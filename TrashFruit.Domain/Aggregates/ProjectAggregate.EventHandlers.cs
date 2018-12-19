using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Edument.CQRS;
using TrashFruit.Domain.Events;
using TrashFruit.Domain.Commands;
using System.Linq;
namespace TrashFruit.Domain.Aggregates
{
    public partial class ProjectAggregate :
        IApplyEvent<ProjectStarted>,
        IApplyEvent<TasksAddedToProject>,
        IApplyEvent<TaskCompleted>,
        IApplyEvent<TaskAssignedToUser>,
        IApplyEvent<ProjectStatusSet>,
        IApplyEvent<TaskUpdated>
    {
        #region Events
        void IApplyEvent<ProjectStarted>.Apply(ProjectStarted e)
        {
            started = true;
            Title = e.Title;
        }

        void IApplyEvent<TasksAddedToProject>.Apply(TasksAddedToProject e)
        {
            ProjectTasks.AddRange(e.ProjectTasks);
        }

        void IApplyEvent<TaskCompleted>.Apply(TaskCompleted e)
        {
            ProjectTask working = GetTaskByID(e.TaskId);
            working.Status = TaskStatus.Completed;
        }

        void IApplyEvent<TaskAssignedToUser>.Apply(TaskAssignedToUser e)
        {
            ProjectTask working = GetTaskByID(e.TaskId);
            working.AssignedToUser = e.AssignedToUser;
        }

        #endregion
    }
}
