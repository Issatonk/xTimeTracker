using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTimeTracker.DataAccess.MSSQL
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<Core.Project, Entities.Project>().ReverseMap();
        }
    }
}
