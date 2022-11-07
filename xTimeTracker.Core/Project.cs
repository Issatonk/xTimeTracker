using System.ComponentModel.DataAnnotations;

namespace xTimeTracker.Core
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TimeSpan Plan { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public int Percent => (int)(TimeSpent / Plan * 100);

        public ICollection<Task> Tasks { get; set; }
    }
}