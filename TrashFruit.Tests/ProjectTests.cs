using Edument.CQRS;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TrashFruit.Domain;
using TrashFruit.Domain.Aggregates;
using TrashFruit.Domain.Commands;
using TrashFruit.Domain.Events;

namespace TrashFruit.Tests
{
    public class ProjectTests:BDDTest<ProjectAggregate>
    {
        private Guid testId;
        private string testTitle;
        private ProjectTask testTask1;
        private DateTime testCompletedTime;
        private Guid getsTaskUser;
        private Guid losesTaskUser;
        private Guid finalUser;

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

        }

        [Test]
        public void CanStartANewProject()
        {
            Test(
                Given(),
                When(new StartProject
                {
                    Id = testId,
                    Title = testTitle
                }),
                Then(new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                }));
        }

        [Test]
        public void CannotStartANewProjectWithARedundantGUID()
        {
            Test(
                Given(new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                }),
                When(new StartProject
                {
                    Id = testId,
                    Title = testTitle
                }),
                ThenFailWith<ProjectAlreadyExists>());
        }

        [Test]
        public void CanAdvanceProjectStatus()
        {
            Test(
                Given(new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                }),
                When(new SetProjectStatus
                {
                    Id = testId,
                    Status = ProjectStatusLane.InProgress
                }),
                Then(new ProjectStatusSet
                {
                    Id = testId,
                    Status = ProjectStatusLane.InProgress
                }
                ));
        }

        [Test]
        public void CannotAssignCancelledProjectToUser()
        {
            Test(
                Given(new ProjectStarted
                {
                    Id = testId,
                    Title = testTitle
                },
                new ProjectStatusSet
                {
                    Id = testId,
                    Status = ProjectStatusLane.Cancelled
                }),
                When(new AssignProjectToUser
                {
                    Id = testId,
                    AssignedToUser = finalUser
                }),
                ThenFailWith<CancelledProjectCannotBeAssigned>());
        }

      
    }


}