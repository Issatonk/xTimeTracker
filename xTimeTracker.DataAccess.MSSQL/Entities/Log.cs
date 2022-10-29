using System.ComponentModel.DataAnnotations;

namespace xTimeTracker.DataAccess.MSSQL.Entities
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