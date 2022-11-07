namespace xTimeTracker.API.Models
{
    public class TaskCreateRequest
    {
        public string Name { get; set; }

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public int ProjectId { get;set; }
    }
}
