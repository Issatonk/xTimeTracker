namespace xTimeTracker.API.Models
{
    public class ProjectUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}
