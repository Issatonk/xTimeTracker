using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTimeTracker.Core;

namespace xTimeTracker.DataAccess.MSSQL
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<Core.Project, Entities.Project>()
                .ForMember(proj => proj.TimeSpent, option=> option.MapFrom(src=> src.TimeSpent.TotalSeconds))
                .ForMember(proj=> proj.Plan, option => option.MapFrom(src => src.Plan.TotalSeconds));

            CreateMap<Entities.Project, Core.Project>()
                .ForMember(proj=>proj.TimeSpent, option =>option.MapFrom(src => TimeSpan.FromSeconds(src.TimeSpent)))
                .ForMember(proj=>proj.Plan, option => option.MapFrom(src => TimeSpan.FromSeconds(src.Plan)));

            CreateMap<Core.Task, Entities.Task>()
                .ForMember(task => task.TimeSpent, option => option.MapFrom(src => src.TimeSpent.TotalSeconds))
                .ForMember(task => task.Plan, option => option.MapFrom(src => src.Plan.TotalSeconds));

            CreateMap<Entities.Task, Core.Task>()
                .ForMember(task => task.TimeSpent, option => option.MapFrom(src => TimeSpan.FromSeconds(src.TimeSpent)))
                .ForMember(task => task.Plan, option => option.MapFrom(src => TimeSpan.FromSeconds(src.Plan)));

            CreateMap<Core.Log, Entities.Log>()
                .ForMember(log => log.TimeSpent, option => option.MapFrom(src => src.TimeSpent.TotalSeconds));

            CreateMap<Entities.Log, Core.Log>()
                .ForMember(log => log.TimeSpent, option => option.MapFrom(src => TimeSpan.FromSeconds(src.TimeSpent)));
        }
    }
}
