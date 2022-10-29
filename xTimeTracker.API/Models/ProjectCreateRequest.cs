namespace xTimeTracker.API.Models
{
    public class ProjectCreateRequest
    {
        public string Name { get; set; }

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}
