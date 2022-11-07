using System.ComponentModel.DataAnnotations;

namespace xTimeTracker.Core
{
    public class Log
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public int TaskId { get; set; }
    }
}