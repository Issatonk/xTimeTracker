using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTimeTracker.Core
{
    public class TimeProjectsByDate
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public List<ProjectNameWithTime> TimeProjects{ get; set; }
    }

    public class ProjectNameWithTime
    {
        public string Name { get; set; }
        public double Time { get; set; }
    }
}
