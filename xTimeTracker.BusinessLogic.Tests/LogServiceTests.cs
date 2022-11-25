using AutoFixture;
using AutoFixture.Dsl;
using FluentAssertions;
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
    public class LogServiceTests
    {
        private readonly Mock<ILogRepository> _repositoryMock;
        private readonly LogService _service;
        private readonly Fixture _fixture;
        public LogServiceTests()
        {
            _repositoryMock = new Mock<ILogRepository>();
            _service = new LogService(_repositoryMock.Object);
            _fixture = new Fixture();

        }
        [Fact]
        public async Task CreateLog_LogIsValid_ShouldReturnTrue()
        {
            //arrange
            var log = _fixture.Build<Log>()
                .With(x => x.Id, 0)
                .Without(x => x.Task)
                .Create();
            _repositoryMock.Setup(x => x.CreateLog(log)).ReturnsAsync(true);
            //act
            var result = await _service.Create(log);
            //assert
            _repositoryMock.Verify(x => x.CreateLog(log), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task CreateLog_LogIsNull_ShouldThrowArgumentNullException()
        {
            //arrange
            Log log = null;
            //act

            //assert    
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.Create(log));
            _repositoryMock.Verify(x => x.CreateLog(log), Times.Never);
        }

        [Theory]
        [MemberData(nameof(TestInvalidLogs.GetTestLogForCreate), MemberType = typeof(TestInvalidLogs))]
        [MemberData(nameof(TestInvalidLogs.GetTestLogForCreateAndUpdate), MemberType = typeof(TestInvalidLogs))]
        public async Task CreateLog_LogIsInvalid_ShouldThrowArgumentException(Log log)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.Create(log));
            _repositoryMock.Verify(x => x.CreateLog(log), Times.Never);
        }

        [Fact]
        public async Task GetAll_ShouldReturnLogs()
        {
            //arrange
            var logs = _fixture.Build<Log>()
                .Without(x => x.Task)
                .CreateMany();
            _repositoryMock.Setup(x=>x.GetLogs()).ReturnsAsync(logs);
            //act
            var result = await _service.GetAll();
            //assert
            Assert.Equal(logs, result);
            _repositoryMock.Verify(x=>x.GetLogs(), Times.Once);
        }
        [Fact]
        public async Task GetLogsByDateInterval_DatesIsValid_ShouldReturnLogs()
        {
            //arrange
            var logs = _fixture.Build<Log>()
                .Without(x => x.Task)
                .CreateMany();
            DateTime start = DateTime.Now.AddDays(-7);
            DateTime end = DateTime.Now;
            _repositoryMock.Setup(x => x.GetLogsByDateInterval(start, end)).ReturnsAsync(logs);
            //act
            var result = await _service.GetLogsByDateInterval(start, end);
            //assert
            Assert.Equal(logs, result);
            _repositoryMock.Verify(x => x.GetLogsByDateInterval(start, end), Times.Once);
        }
        [Fact]
        public async Task GetLogsByDateInterval_DatesIsInvalid_ShouldThrowArgumentException()
        {
            //arrange
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddDays(-1);
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetLogsByDateInterval(start, end));
            _repositoryMock.Verify(x=>x.GetLogsByDateInterval(start,end), Times.Never);
        }

        [Fact]
        public async Task GetLogByTask_TaskIdIsValid_ShouldReturnLog()
        {
            //arrange
            const int taskId = 1;
            var logs = _fixture.Build<Log>()
                .Without(x => x.Task)
                .With(x => x.TaskId, taskId)
                .CreateMany();
            _repositoryMock.Setup(x => x.GetLogsByTask(taskId)).ReturnsAsync(logs);
            //act
            var result = await _service.GetLogsByTask(taskId);
            //assert
            Assert.Equal(logs, result);
            _repositoryMock.Verify(x => x.GetLogsByTask(taskId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetLogByTask_TaskIdIsInvalid_ShouldThrowArgumentException(int taskId)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetLogsByTask(taskId));
            _repositoryMock.Verify(x => x.GetLogsByTask(taskId), Times.Never);
        }
        [Fact]
        public async Task GetLogWithTaskNameAndProjectName_ShouldReturnLogsWithNames()
        {
            //arrange
            var log = _fixture.Build<Log>()
                .With(x=>x.Date, DateTime.UtcNow.Date)
                .With(x=>x.TimeSpent, TimeSpan.MaxValue)
                .With(x => x.Task,
                    _fixture.Build<Core.Task>()
                    .Without(x => x.Logs)
                    .With(x => x.Project,
                        _fixture.Build<Project>()
                        .Without(x => x.Tasks)
                        .Create())
                    .Create())
                .Create();
            var expected = new LogWithTaskNameAndProjectName
            {
                Id = log.Id,
                Date = log.Date,
                TimeSpent = log.TimeSpent,
                TaskName = log.Task.Name,
                ProjectName = log.Task.Project.Name
                
            };
            _repositoryMock.Setup(x => x.GetLogsWithProject()).ReturnsAsync(new List<Log> { log });
            //act
            var result = (await _service.GetLogsWithTaskNameAndProjectName()).First();
            //assert
            result.Should().BeEquivalentTo(expected);
            _repositoryMock.Verify(x=>x.GetLogsWithProject(), Times.Once);
        }
        [Fact]
        public async Task UpdateLog_LogIsValid_ShouldReturnTrue()
        {
            //arrange
            var log = _fixture.Build<Log>()
                .Without(x => x.Task)
                .Create();
            _repositoryMock.Setup(x => x.UpdateLog(log)).ReturnsAsync(true);
            //act
            var result = await _service.Update(log);
            //assert
            _repositoryMock.Verify(x => x.UpdateLog(log), Times.Once());
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateLog_LogIsNull_ShouldThrowArgumentNullException()
        {
            //arrange
            Log log = null;
            //act

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.Update(log));
            _repositoryMock.Verify(x => x.UpdateLog(log), Times.Never);
        }

        [Theory]
        [MemberData(nameof(TestInvalidLogs.GetTestLogForUpdate), MemberType = typeof(TestInvalidLogs))]
        [MemberData(nameof(TestInvalidLogs.GetTestLogForCreateAndUpdate), MemberType = typeof(TestInvalidLogs))]
        public async Task UpdateLog_LogIsInvalid_ShouldThrowArgumentException(Log log)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.Update(log));
            _repositoryMock.Verify(x => x.UpdateLog(log), Times.Never);
        }

        [Fact]
        public async Task DeleteLog_LogIdIsValid_ShouldCallDeleteMethod()
        {
            //arrange
            int logId = 1;

            //act
            await _service.Delete(logId);
            //assert
            _repositoryMock.Verify(x => x.DeleteLog(logId), Times.Once);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteLog_LogIdIsInvalid_ShouldThrowArgumentException(int logId)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.Delete(logId));
            _repositoryMock.Verify(x => x.DeleteLog(logId), Times.Never);

        }

    }
    public class TestInvalidLogs
    {
        static IPostprocessComposer<Log> fixture = new Fixture()
            .Build<Log>()
            .Without(x => x.Task);

        public static IEnumerable<object[]> GetTestLogForCreate()
        {
            List<object[]> test = new List<object[]>
            {
                new object[] {fixture.With(x=>x.Id, 1).Create() },
                new object[] {fixture.With(x=>x.Id, -1).Create() }
            };
            return test;
        }

        public static IEnumerable<object[]> GetTestLogForUpdate()
        {
            List<object[]> test = new List<object[]>
            {
                new object[] {fixture.With(x=>x.Id, 0).Create() },
                new object[] {fixture.With(x=>x.Id, -1).Create() }
            };
            return test;
        }

        public static IEnumerable<object[]> GetTestLogForCreateAndUpdate()
        {
            List<object[]> test = new List<object[]>
            {
                new object[] {fixture.Without(x=>x.Date).Create()},
                new object[] {fixture.With(x=>x.TaskId, 0).Create()},
                new object[] {fixture.With(x=>x.TaskId, -1).Create()},
                new object[] {fixture.With(x=>x.TimeSpent, new TimeSpan(0)).Create()},
                new object[] {fixture.With(x=>x.TimeSpent, new TimeSpan(-1000)).Create()},
            };
            return test;
        }
    }
}
