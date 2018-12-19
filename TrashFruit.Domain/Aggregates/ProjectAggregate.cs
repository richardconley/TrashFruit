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
    public class ProjectAggregate : Aggregate,
        IHandleCommand<StartProject>,
        IHandleCommand<AddTasksToProject>,
        IHandleCommand<MarkTaskCompleted>,
        IHandleCommand<AssignTaskToUser>,
        IHandleCommand<UpdateTaskStatus>,
        IHandleCommand<SetProjectStatus>,
        IHandleCommand<AssignProjectToUser>,
        IApplyEvent<ProjectStarted>,
        IApplyEvent<TasksAddedToProject>,
        IApplyEvent<TaskCompleted>,
        IApplyEvent<TaskAssignedToUser>,
        IApplyEvent<ProjectStatusSet>,
        IApplyEvent<TaskUpdated>
    {
        #region AggregateParts
        private bool started;
        private string Title;
        private List<ProjectTask> ProjectTasks;
        private ProjectStatusLane Status;
        public ProjectAggregate()
        {
            ProjectTasks = new List<ProjectTask>();
            started = false;
        }

        public ProjectTask GetTaskByID(int id)
        {
            return ProjectTasks.Single(f => f.Id == id);
        }
        #endregion


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

        #region Commands
        IEnumerable IHandleCommand<StartProject>.Handle(StartProject c)
        {
            if(started)
            {
                throw new ProjectAlreadyExists { Id = c.Id };
            }
            yield return new ProjectStarted
            {
                Id = c.Id,
                Title = c.Title
            };
        }
        
        IEnumerable IHandleCommand<AddTasksToProject>.Handle(AddTasksToProject c)
        {
            if (!started){
                throw new ProjectNotStarted();
            }

            if (c.ProjectTasks.Any())
                yield return new TasksAddedToProject
                {
                    Id = c.Id,
                    ProjectTasks = c.ProjectTasks
                };
                       
        }

        IEnumerable IHandleCommand<MarkTaskCompleted>.Handle(MarkTaskCompleted c)
        {
            ProjectTask working = GetTaskByID(c.TaskId);
            if (working.Status == TaskStatus.Completed)
            {
                throw new TaskAlreadyCompleted
                {
                    Id = c.Id,
                    TaskId = c.TaskId
                };
            }
            else
            {
                yield return new TaskCompleted
                {
                    Id = c.Id,
                    TaskId = c.TaskId,
                    TimeCompleted = c.TimeCompleted
                };
            }
        }

        IEnumerable IHandleCommand<AssignTaskToUser>.Handle(AssignTaskToUser c)
        {
            ProjectTask working = GetTaskByID(c.TaskId);
            if (working.Status == TaskStatus.Cancelled || working.Status == TaskStatus.Completed)
            {
                throw new TaskInInvalidState
                {
                    Status = working.Status,
                    Command = c,
                    Id = c.Id,
                    TaskId = c.TaskId
                };
            }
            if (c.AssignedToUser == null || c.AssignedToUser == Guid.Empty)
            {
                throw new TaskCannotBeUnassigned
                {
                    Id = c.Id,
                    TaskId = c.TaskId
                };
            }
            yield return new TaskAssignedToUser
            {
                Id = c.Id,
                TaskId = c.TaskId,
                AssignedToUser = c.AssignedToUser,
                AssignedFromUser = working.AssignedToUser
            };
        }

        IEnumerable IHandleCommand<UpdateTaskStatus>.Handle(UpdateTaskStatus c)
        {
            ProjectTask worker = GetTaskByID(c.TaskId);

            yield return new TaskUpdated
            {
                Comment = c.Comment,
                Id = c.Id,
                TaskId = c.TaskId,
                Status = c.Status
            };
        }

        IEnumerable IHandleCommand<SetProjectStatus>.Handle(SetProjectStatus c)
        {
            yield return new ProjectStatusSet
            {
                Id = c.Id,
                Status = c.Status
            };
        }

        void IApplyEvent<ProjectStatusSet>.Apply(ProjectStatusSet e)
        {
            Status = e.Status;
        }

        IEnumerable IHandleCommand<AssignProjectToUser>.Handle(AssignProjectToUser c)
        {
            if (Status == ProjectStatusLane.Cancelled) throw new CancelledProjectCannotBeAssigned { Id = c.Id, AssignedToUser = c.AssignedToUser };
            yield return new ProjectAssignedToUser
            {
                Id = c.Id,
                AssignedToUser = c.AssignedToUser
        };
        }

        void IApplyEvent<TaskUpdated>.Apply(TaskUpdated e)
        {
            ProjectTask worker = GetTaskByID(e.TaskId);
            worker.Comment = e.Comment;
            //worker.Status = e.Status;
        }

        #endregion
    }

    public class ProjectTask
    {
        public int Id;
        public string Title;
        public string Description;
        public TaskStatus Status;
        public Guid AssignedToUser;
        public string Comment;
    }

    public enum TaskStatus
    {
        UnStarted,
        InProgress,
        Blocked,
        Completed,
        Cancelled
           }
}
