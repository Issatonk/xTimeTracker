using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace xTimeTracker.API.Models
{
    public class GetProjectWithTimeRequest
    {
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
    }
}
