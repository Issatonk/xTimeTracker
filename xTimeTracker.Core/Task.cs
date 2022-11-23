using System.ComponentModel.DataAnnotations;

namespace xTimeTracker.Core
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan Plan { get; set; }


        public TimeSpan TimeSpent { get; set; }

        public int Percent => Plan.Ticks > 0 ? (int)(TimeSpent / Plan * 100): 100;

        public int ProjectId { get; set; }

        public Project Project { get; set; }
        public ICollection<Log> Logs { get; set; }


    }
}