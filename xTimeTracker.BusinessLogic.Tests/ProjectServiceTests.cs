using AutoFixture;
using AutoFixture.Dsl;
using FluentAssertions;
using Moq;
using System.Security.Cryptography.X509Certificates;
using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace xTimeTracker.BusinessLogic.Tests
{
    public class ProjectServiceTests
    {
        private readonly ProjectService _service;
        private readonly Mock<IProjectRepository> _repositoryMock;
        private readonly Fixture _fixture;

        public ProjectServiceTests()
        {
            _repositoryMock = new Mock<IProjectRepository>();
            _service = new ProjectService(_repositoryMock.Object);
            _fixture = new Fixture();
            
        }

        [Fact]
        public async Task CreateProject_ProjectIsValid_ShouldReturnTrue()
        {
            //arrange
            var project = _fixture.Build<Project>()
                .With(x => x.Id, 0)
                .Without(x=>x.Tasks)
                .Create();
            _repositoryMock.Setup(x => x.CreateProject(project)).ReturnsAsync(true);
            //act
            var result = await _service.CreateProject(project);
            //assert
            _repositoryMock.Verify(x=> x.CreateProject(project), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task CreateProject_ProjectIsNull_ShouldThrowArgumentNullException()
        {
            //arrange
            Project project = null;
            //act

            //assert    
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.CreateProject(project));
            _repositoryMock.Verify(x => x.CreateProject(project), Times.Never);
        }

        [Theory]
        [MemberData(nameof(TestInvalidProjects.GetTestProjectForCreate), MemberType = typeof(TestInvalidProjects))]
        [MemberData(nameof(TestInvalidProjects.GetTestProjectForCreateAndUpdate), MemberType = typeof(TestInvalidProjects))]
        public async Task CreateProject_ProjectISInvalid_ShouldThrowArgumentException(Project project)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateProject(project));
            _repositoryMock.Verify(x => x.CreateProject(project), Times.Never);
        }

        [Fact]
        public async Task GetProjects_ShouldReturnProjects()
        {
            //arrange
            var projects = _fixture.Build<Project>().Without(x=>x.Tasks).CreateMany();
            _repositoryMock.Setup(x=>x.GetProjects()).ReturnsAsync(projects);
            //act
            var result = await _service.GetProjects();
            //assert
            Assert.Equal(projects, result);
            _repositoryMock.Verify(x => x.GetProjects(), Times.Once);
        }

        [Fact]
        public async Task GetTimeForProjectsByDate_DatesIsValid_ShouldReturnCollectionByDates()
        {
            //arrange
            Random rnd = new Random();
            var startDate = new DateTime(2022, 11, 13);
            var endDate = new DateTime(2022, 11, 16);
            const int increasedDateRange = 2;
            int range = (endDate - startDate).Days + increasedDateRange;

            List<Log> log = _fixture.Build<Log>().Without(x => x.Task)
                .CreateMany(8).ToList();
            foreach(var l in log) { 
                l.Date = startDate.AddDays(rnd.Next(range)-increasedDateRange/2); 
                l.TimeSpent = new TimeSpan(rnd.Next(2),rnd.Next(60), rnd.Next(60));
            }
            var task1 = _fixture.Build<Core.Task>()
                .Without(x=>x.Project)
                .With(x=>x.Logs, log.Take(log.Count / 2).ToList()).Create();
            var task2 = _fixture.Build<Core.Task>()
                .Without(x=>x.Project)
                .With(x => x.Logs, log.TakeLast(log.Count() / 2).ToList()).Create();
            var project = _fixture.Build<Project>().With(x => x.Tasks, new List<Core.Task>() { task1, task2 }).Create();

            var expected = new List<TimeProjectsByDate>()
            {
                GenerateTimeProjectByDate(project, startDate),
                GenerateTimeProjectByDate(project, startDate.AddDays(1)),
                GenerateTimeProjectByDate(project, startDate.AddDays(2)),
                GenerateTimeProjectByDate(project, startDate.AddDays(3)),

            };

            _repositoryMock
                .Setup(x => x.GetProjectsWithLogs(startDate, endDate))
                .ReturnsAsync(new List<Project> { project });

            //act
            var result = await _service.GetTimeForProjectsByDate(startDate, endDate);
            //assert

            _repositoryMock.Verify(x=>x.GetProjectsWithLogs(startDate, endDate), Times.Once());
            result.Should().BeEquivalentTo(expected);
            
        }

        [Fact]
        public async Task GetTimeForProjectByDate_DateIsInvalid_ShouldThrowArgumentException()
        {
            //arrange
            var startDate = new DateTime(2022, 11, 16);
            var endDate = new DateTime(2022, 11, 13);
            //act

            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetTimeForProjectsByDate(startDate, endDate));
            _repositoryMock.Verify(x => x.GetProjectsWithLogs(startDate, endDate), Times.Never);
        }

        [Fact]
        public async Task UpdateProject_ProjectIsValid_ShouldReturnTrue()
        {
            //arrange
            var project = _fixture.Build<Project>().Without(x=>x.Tasks).Create();
            _repositoryMock.Setup(x=>x.UpdateProject(project)).ReturnsAsync(true);
            //act
            var result = await _service.UpdateProject(project);
            //assert
            _repositoryMock.Verify(x=>x.UpdateProject(project), Times.Once());
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateProject_ProjectIsNull_ShouldThrowArgumentNullException()
        {
            //arrange
            Project project = null;
            //act

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateProject(project));
            _repositoryMock.Verify(x => x.UpdateProject(project), Times.Never);
        }

        [Theory]
        [MemberData(nameof(TestInvalidProjects.GetTestProjectForUpdate), MemberType = typeof(TestInvalidProjects))]
        [MemberData(nameof(TestInvalidProjects.GetTestProjectForCreateAndUpdate), MemberType = typeof(TestInvalidProjects))]
        public async Task UpdateProject_ProjectIsInvalid_ShouldThrowArgumentException(Project project)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateProject(project));
            _repositoryMock.Verify(x => x.UpdateProject(project), Times.Never);
        }

        [Fact]
        public async Task DeleteProject_ProjectIdIsValid_ShouldCallDeleteMethod()
        {
            //arrange
            int projectId = 1;

            //act
            await _service.DeleteProject(projectId);
            //assert
            _repositoryMock.Verify(x=>x.DeleteProject(projectId), Times.Once);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteProject_ProjectIdIsInvalid_ShouldThrowArgumentException(int projectId)
        {
            //arrange
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.DeleteProject(projectId));
            _repositoryMock.Verify(x => x.DeleteProject(projectId), Times.Never);

        }

        public TimeProjectsByDate GenerateTimeProjectByDate(Project project,DateTime date)
        {
            var arrayLogs = project.Tasks.Select(x => x.Logs);
            var logs = arrayLogs.First().Union(arrayLogs.Last());
            return new TimeProjectsByDate()
            {
                Date = date,
                TimeProjects =
                    new List<ProjectNameWithTime>() {
                        new ProjectNameWithTime
                        {
                            Name = project.Name,
                            Time = Math.Truncate(logs.Where(x=>x.Date == date).Sum(x=>x.TimeSpent.TotalMilliseconds))
                        } 
                    }
            };
        }
    }

    public class TestInvalidProjects
    {
        static IPostprocessComposer<Project> fixture = new Fixture().Build<Project>().Without(x=>x.Tasks);

        public static IEnumerable<object[]> GetTestProjectForCreate()
        {
            List<object[]> tests = new List<object[]>
            {
                new object[] { fixture.With(x => x.Id, 1).Create()},
                new object[] { fixture.With(x => x.Id, -1).Create()}
            };
            return tests;
        }

        public static IEnumerable<object[]> GetTestProjectForUpdate()
        {
            List<object[]> tests = new List<object[]>
            { 
                new object[] { fixture.With(x => x.Id, 0).Create()},
                new object[] { fixture.With(x => x.Id, -1).Create()}
            };

            return tests;
        }

        public static IEnumerable<object[]> GetTestProjectForCreateAndUpdate()
        {
            List<object[]> tests = new List<object[]>
            {
                new object[] {fixture.With(x=>x.Id, 0).Without(x=>x.Name).Create()},
                new object[] {fixture.With(x=>x.Id, 0).With(x=>x.Name, "").Create()},
                new object[] {fixture.With(x=>x.Id, 0).With(x=>x.Name, "    ").Create()},
                new object[] {fixture.With(x => x.Id, 0).With(x=>x.Plan, new TimeSpan(-1000)).Create()},
                new object[] {fixture.With(x => x.Id, 0).With(x => x.Plan, new TimeSpan(0)).Create() },
            };
            return tests;
        }
    }
}