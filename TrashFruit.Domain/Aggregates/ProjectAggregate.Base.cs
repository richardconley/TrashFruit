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
    public partial class ProjectAggregate : Aggregate
        
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
