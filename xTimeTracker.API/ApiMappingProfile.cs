using AutoMapper;
using xTimeTracker.API.Models;

namespace xTimeTracker.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<ProjectCreateRequest, Core.Project>()
                .ForMember(proj => proj.Plan, option=> option.MapFrom(src=> new TimeSpan(src.Hours, src.Minutes, src.Seconds)));

            CreateMap<ProjectUpdateRequest, Core.Project>()
                .ForMember(proj => proj.Plan, option => option.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, src.Seconds)));

            CreateMap<TaskCreateRequest, Core.Task>()
                .ForMember(task => task.Plan, option => option.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, src.Seconds)));

            CreateMap<TaskUpdateRequest, Core.Task>()
                .ForMember(task => task.Plan, option => option.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, src.Seconds)));

            CreateMap<LogCreateRequest, Core.Log>()
                .ForMember(log => log.TimeSpent, option => option.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, src.Seconds)));

            CreateMap<LogUpdateRequest, Core.Log>()
                .ForMember(log => log.TimeSpent, option => option.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, src.Seconds)));
        }
    }
}
