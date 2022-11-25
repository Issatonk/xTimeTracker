using AutoFixture;
using AutoFixture.Dsl;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
using Task = System.Threading.Tasks.Task;

namespace xTimeTracker.BusinessLogic.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly TaskService _service;
        private readonly Fixture _fixture;
        public TaskServiceTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _service = new TaskService(_repositoryMock.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task CreateProject_ProjectIsValid_ShouldReturnTrue()
        {
            //arrange
            var task = _fixture.Build<Core.Task>()
                .With(x => x.Id, 0)
                .Without(x => x.Logs)
                .Without(x=>x.Project)
                .Create();
            _repositoryMock.Setup(x => x.CreateTask(task)).ReturnsAsync(true);
            //act
            var result = await _service.Create(task);
            //assert
            _repositoryMock.Verify(x => x.CreateTask(task), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task CreateProject_ProjectIsNull_ShouldThrowArgumentNullException()
        {
            //arrange
            Core.Task task = null;
            //act

            //assert    
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.Create(task));
            _repositoryMock.Verify(x => x.CreateTask(task), Times.Never);
        }

        [Theory]
        [MemberData(nameof(TestInvalidTasks.GetTestTaskForCreate), MemberType = typeof(TestInvalidTasks))]
        [MemberData(nameof(TestInvalidTasks.GetTestTaskForCreateAndUpdate), MemberType = typeof(TestInvalidTasks))]
        public async Task CreateProject_ProjectIsInvalid_ShouldThrowArgumentException(Core.Task task)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.Create(task));
            _repositoryMock.Verify(x => x.CreateTask(task), Times.Never);
        }

        [Fact]
        public async Task GetTaskByProject_ProjectIdIsValid_ShouldReturnTasks()
        {
            //arrange
            const int projectId = 1;
            var tasks = _fixture.Build<Core.Task>()
                .Without(x => x.Logs)
                .Without(x => x.Project)
                .With(x=>x.ProjectId, projectId)
                .CreateMany();
            _repositoryMock.Setup(x => x.GetTasksByProject(projectId)).ReturnsAsync(tasks);
            //act
            var result = await _service.GetTasksByProject(projectId);
            //assert
            Assert.Equal(tasks, result);
            _repositoryMock.Verify(x => x.GetTasksByProject(projectId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetTaskByProject_ProjectIdIsInvalid_ShouldThrowArgumentException(int projectId)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetTasksByProject(projectId));
            _repositoryMock.Verify(x => x.GetTasksByProject(projectId), Times.Never);
        }
        [Fact]
        public async Task UpdateTask_TaskIsValid_ShouldReturnTrue()
        {
            //arrange
            var task = _fixture.Build<Core.Task>()
                .Without(x=>x.Logs)
                .Without(x => x.Project).Create();
            _repositoryMock.Setup(x => x.UpdateTask(task)).ReturnsAsync(true);
            //act
            var result = await _service.Update(task);
            //assert
            _repositoryMock.Verify(x => x.UpdateTask(task), Times.Once());
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateTask_TaskIsNull_ShouldThrowArgumentNullException()
        {
            //arrange
            Core.Task task = null;
            //act

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.Update(task));
            _repositoryMock.Verify(x => x.UpdateTask(task), Times.Never);
        }

        [Theory]
        [MemberData(nameof(TestInvalidTasks.GetTestTaskForUpdate), MemberType = typeof(TestInvalidTasks))]
        [MemberData(nameof(TestInvalidTasks.GetTestTaskForCreateAndUpdate), MemberType = typeof(TestInvalidTasks))]
        public async Task UpdateTask_TaskIsInvalid_ShouldThrowArgumentException(Core.Task task)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.Update(task));
            _repositoryMock.Verify(x => x.UpdateTask(task), Times.Never);
        }

        [Fact]
        public async Task DeleteTask_TaskIdIsValid_ShouldCallDeleteMethod()
        {
            //arrange
            int taskId = 1;

            //act
            await _service.Delete(taskId);
            //assert
            _repositoryMock.Verify(x => x.DeleteTask(taskId), Times.Once);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteTask_TaskIdIsInvalid_ShouldThrowArgumentException(int taskId)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.Delete(taskId));
            _repositoryMock.Verify(x => x.DeleteTask(taskId), Times.Never);

        }

    }


    public class TestInvalidTasks
    {
        static IPostprocessComposer<Core.Task> fixture = new Fixture()
            .Build<Core.Task>()
            .Without(x=>x.Project)
            .Without(x=>x.Logs);

        public static IEnumerable<object[]> GetTestTaskForCreate()
        {
            List<object[]> test = new List<object[]>
            {
                new object[] {fixture.With(x=>x.Id, 1).Create() },
                new object[] {fixture.With(x=>x.Id, -1).Create() }
            };
            return test;
        }

        public static IEnumerable<object[]> GetTestTaskForUpdate()
        {
            List<object[]> test = new List<object[]>
            {
                new object[] {fixture.With(x=>x.Id, 0).Create() },
                new object[] {fixture.With(x=>x.Id, -1).Create() }
            };
            return test;
        }

        public static IEnumerable<object[]> GetTestTaskForCreateAndUpdate()
        {
            List<object[]> test = new List<object[]>
            {
                new object[] {fixture.Without(x=>x.Name).Create()},
                new object[] {fixture.With(x=>x.Name, "").Create()},
                new object[] {fixture.With(x=>x.Name, "   ").Create()},
                new object[] {fixture.With(x=>x.Plan, new TimeSpan(0)).Create()},
                new object[] {fixture.With(x=>x.Plan, new TimeSpan(-1000)).Create()},
            };
            return test;
        }
    }
}
