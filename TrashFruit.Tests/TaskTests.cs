using NUnit.Framework;
using System;
using System.Collections.Generic;
using TrashFruit.Domain;
using TrashFruit.Domain.Aggregates;
using TrashFruit.Domain.Commands;
using TrashFruit.Domain.Events;
using Edument.CQRS;
using EdumentCQRS.Core;

namespace TrashFruit.Tests
{
    public class TaskTests:BDDTest<ProjectAggregate>
    {
        private Guid testId;
        private string testTitle;
        private ProjectTask testTask1;
        private DateTime testCompletedTime;
        private Guid getsTaskUser;
        private Guid losesTaskUser;
        private Guid finalUser;
        private string comment; 

        [SetUp]
        public void Setup()
        {
            testId = Guid.NewGuid();
            testTitle = "Build the barn";
            testCompletedTime = DateTime.Parse("2018-01-01");
            getsTaskUser = Guid.NewGuid();
            losesTaskUser = Guid.NewGuid();
            finalUser = Guid.NewGuid();
            testTask1 = new ProjectTask { Title = "Buy Boards", Description = "Get 26 2x4s", Status = TaskStatus.UnStarted, Id = 1, AssignedToUser = losesTaskUser };
            comment  = "All the things are roundin out to done";
        }

        [Test] 
        public void CanUpdateCommentOnTask()
        {
            //TODO: Make sure comment is applied to aggregate - not just event flow
            Test(
                Given(
                new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                },
                new TasksAddedToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                }),
                When(new UpdateTaskStatus
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    Status = TaskStatus.InProgress,
                    Comment = comment
                }),
                Then(new TaskUpdated
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    Status = TaskStatus.InProgress,
                    Comment = comment
                }));

        }

        [Test]
        public void CommentOnTaskActuallyUpdated()
        {
            //TODO: Make sure comment is applied to aggregate - not just event flow
            Test(
                Given(
                new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                },
                new TasksAddedToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                },
                new TaskUpdated
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    Status = TaskStatus.InProgress,
                    Comment = comment
                }
                ),When(new UpdateTaskStatus
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    Status = TaskStatus.InProgress,
                    Comment = comment
                }),Then(new TaskUpdated
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    Status = TaskStatus.InProgress,
                    Comment = comment
                }));

            //hacky, but no cleaner way to test check this at this point
            ProjectAggregate proj = GetAggregate();
            ProjectTask tsk = proj.GetTaskByID(testTask1.Id);
            Assert.AreEqual(comment, tsk.Comment);
        }

        [Test]
        public void CannotAddTasksToUnstartedProject()
        {
            Test(
                Given(),
                When(new AddTasksToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                }),
              ThenFailWith<ProjectNotStarted>());
        }

        [Test]
        public void CanAddTasksToProject()
        {
            Test(
                Given(new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                }),
                When(new AddTasksToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                }),
                Then(new TasksAddedToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                }));
        }

        [Test]
        public void CanCompleteProjectTask()
        {
            Test(
                Given(
                new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                },
                new TasksAddedToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                }),
                When(new MarkTaskCompleted
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    TimeCompleted = testCompletedTime
                }),
                Then(
                    new TaskCompleted
                    {
                        Id = testId,
                        TaskId = testTask1.Id,
                        TimeCompleted = testCompletedTime
                    }
                    ));
        }

        [Test]
        public void CannotCompleteAlreadyCompletedTask()
        {
            Test(
                Given(
                new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                },
                new TasksAddedToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                },
                new TaskCompleted
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    TimeCompleted = testCompletedTime
                }),
                When(new MarkTaskCompleted
                {
                    Id = testId,
                    TaskId = testTask1.Id,
                    TimeCompleted = testCompletedTime
                }),
                ThenFailWith<TaskAlreadyCompleted>()
                );
        }

        [Test]
        public void CanAssignTaskToUser()
        {
            Test(
                Given(new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                },
                new TasksAddedToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                }),
                When(new AssignTaskToUser
                {
                    TaskId = testTask1.Id,
                    Id = testId,
                    AssignedToUser = getsTaskUser
                }),
                Then(new TaskAssignedToUser
                {
                    TaskId = testTask1.Id,
                    Id = testId,
                    AssignedToUser = getsTaskUser,
                    AssignedFromUser = losesTaskUser
                })

                );


        }


        [Test]
        public void CanReAssignTaskToUser()
        {
            Test(
                Given(new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                },
                new TasksAddedToProject
                {
                    Id = testId,
                    ProjectTasks = new List<ProjectTask> { testTask1 }
                },
                new TaskAssignedToUser
                {
                    TaskId = testTask1.Id,
                    Id = testId,
                    AssignedToUser = getsTaskUser
                }
                ),
                When(new AssignTaskToUser
                {
                    TaskId = testTask1.Id,
                    Id = testId,
                    AssignedToUser = finalUser
                }),
                Then(new TaskAssignedToUser
                {
                    TaskId = testTask1.Id,
                    Id = testId,
                    AssignedToUser = finalUser,
                    AssignedFromUser = getsTaskUser
                })

                );


        }

        [Test]
        public void CannotAssignCompletedTaskToUser()
        {
            Test(
               Given(new ProjectStarted
               {
                   Id = testId,
                   Title = testTitle
               },
               new TasksAddedToProject
               {
                   Id = testId,
                   ProjectTasks = new List<ProjectTask> { testTask1 }
               },
               new TaskCompleted
               {
                   Id = testId,
                   TaskId = testTask1.Id,
                   TimeCompleted = testCompletedTime
               }
               ),
               When(new AssignTaskToUser
               {
                   TaskId = testTask1.Id,
                   Id = testId,
                   AssignedToUser = finalUser
               }),
               ThenFailWith<TaskInInvalidState>()
               );

        }

        [Test]
        public void CannotUnassignTaskOnceAssigned()
        {
            Test(
              Given(new ProjectStarted
              {
                  Id = testId,
                  Title = testTitle
              },
              new TasksAddedToProject
              {
                  Id = testId,
                  ProjectTasks = new List<ProjectTask> { testTask1 }
              },
              new TaskAssignedToUser
              {
                  TaskId = testTask1.Id,
                  Id = testId,
                  AssignedToUser = getsTaskUser,
                  AssignedFromUser = losesTaskUser
              }
              ),
              When(new AssignTaskToUser
              {
                  TaskId = testTask1.Id,
                  Id = testId,
                  AssignedToUser = Guid.Empty
              }),
              ThenFailWith<TaskCannotBeUnassigned>()
              );

        }

    }
}
