using System.ComponentModel.DataAnnotations;

namespace xTimeTracker.API.Models
{
    public class LogCreateRequest
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public int TaskId { get; set; }
    }
}
