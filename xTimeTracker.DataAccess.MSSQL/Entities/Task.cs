using System.ComponentModel.DataAnnotations;


namespace xTimeTracker.DataAccess.MSSQL.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan Plan { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public int ProjectId { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}