using System.ComponentModel.DataAnnotations;


namespace xTimeTracker.DataAccess.MSSQL.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Plan { get; set; }

        public int TimeSpent { get; set; }

        public int ProjectId { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}