namespace xTimeTracker.Core
{
    public class LogWithTaskNameAndProjectName : Log
    {
        public string TaskName { get; set; }

        public string ProjectName { get; set; }
    }
}
