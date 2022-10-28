using System.ComponentModel.DataAnnotations;


namespace xTimeTracker.DataAccess.MSSQL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TimeSpan Plan { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}